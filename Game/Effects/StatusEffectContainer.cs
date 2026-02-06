using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

namespace CraterSprite.Effects;

public class StatusEffectContainer
{
    private Dictionary<StatusEffect, List<StatusEffectInstance>> _statusEffects = new ();

    private SparseEventMap<StatusEffect> _onStatusEffectAppliedMap = new();
    private SparseEventMap<StatusEffect> _onStatusEffectRemovedMap = new();
    private SparseEventMap<StatusEffect, int, float> _onStatusEffectStacksChangedMap = new();

    public void ApplyStatusEffectInstance(StatusEffectInstance instance)
    {
        if (instance.effect == null)
        {
            return;
        }
        
        if (_statusEffects.TryGetValue(instance.effect, out var effectList))
        {
            effectList.Add(instance);
        }
        else
        {
            // Create the stack in our dictionary, and invoke the event for a new stack here
            // if the event exists. Assume no one has subscribed if the event does not exist
            effectList = new List<StatusEffectInstance> { instance };
            _statusEffects.Add(instance.effect, effectList);
            _onStatusEffectAppliedMap.TriggerEvent(instance.effect);
        }
        
        _onStatusEffectStacksChangedMap.TriggerEvent(instance.effect, effectList.Count, Accumulate(instance.effect, effectList));
    }

    public void RemoveStatusEffectInstance(StatusEffectInstance instance)
    {
        if (!_statusEffects.TryGetValue(instance.effect, out var effectList))
        {
            return;
        }

        if (!effectList.Remove(instance))
        {
            return;
        }
        
        _onStatusEffectStacksChangedMap.TriggerEvent(instance.effect, effectList.Count, Accumulate(instance.effect, effectList));
        _onStatusEffectRemovedMap.TriggerEvent(instance.effect);
    }

    public void Update(float deltaSeconds)
    {
        var effectsToRemove = new List<StatusEffect>();
        foreach (var stackList in _statusEffects)
        {
            var numRemoved = stackList.Value.RemoveAll(effectInstance => effectInstance.UpdateCheckExpiration(deltaSeconds));
            if (numRemoved > 0)
            {
                _onStatusEffectStacksChangedMap.TriggerEvent(stackList.Key, stackList.Value.Count, Accumulate(stackList.Key, stackList.Value));
            }
            
            if (stackList.Value.Count == 0)
            {
                effectsToRemove.Add(stackList.Key);
            }
        }

        foreach (var effectToRemove in effectsToRemove)
        {
            _onStatusEffectRemovedMap.TriggerEvent(effectToRemove);
            _statusEffects.Remove(effectToRemove);
        }
    }

    public override string ToString()
    {
        var result = new string("");

        return _statusEffects.Aggregate(result, (current, statusEffect)
            => current + $"{statusEffect.Key.ResourceName}: '{statusEffect.Value.Count}'");
    }

    public Dictionary<StatusEffect, List<StatusEffectInstance>>.Enumerator GetEnumerator()
    {
        return _statusEffects.GetEnumerator();
    }

    public void RegisterEffectAppliedCallback(StatusEffect effect, Action action, Node owner)
    {
        _onStatusEffectAppliedMap.RegisterCallback(effect, action);
        owner.TreeExited += () => _onStatusEffectAppliedMap.RemoveCallback(effect, action);
    }

    public void RegisterEffectRemovedCallback(StatusEffect effect, Action action, Node owner)
    {
        _onStatusEffectRemovedMap.RegisterCallback(effect, action);
        owner.TreeExited += () => _onStatusEffectRemovedMap.RemoveCallback(effect, action);
    }

    public void RegisterStatusEffectChangedEvent(StatusEffect effect, Action<int, float> action, Node owner)
    {
        _onStatusEffectStacksChangedMap.RegisterCallback(effect, action);
        owner.TreeExited += () => _onStatusEffectStacksChangedMap.RemoveCallback(effect, action);
    }

    public float Accumulate(StatusEffect statusEffect, List<StatusEffectInstance> instances)
    {
        if (instances.Count == 0)
        {
            return 0.0f;
        }
        
        switch (statusEffect.accumulator)
        {
            default:
            case EffectAccumulator.None:
                return 0.0f;
            
            case EffectAccumulator.Additive:
                return instances.Sum(instance => instance.strength);
            
            case EffectAccumulator.Maximum:
                return instances.Max(instance => instance.strength);
            
            case EffectAccumulator.Minimum:
                return instances.Min(instance => instance.strength);
            
            case EffectAccumulator.Multiplicative:
                var total = instances.Aggregate(1.0f, (current, instance) => current * instance.strength);
                return total;
        }
    }

    public bool HasEffect(StatusEffect effect)
    {
        return effect != null && _statusEffects.ContainsKey(effect);
    }
}