using Godot;
using System;
using System.Collections.Generic;

namespace CraterSprite
{
    public enum InputEventType
    {
        None,
        Pressed,
        Released,
        Changed
    }

    public struct InputAxis(string positive, string negative)
    {
        public string negative = negative;
        public string positive = positive;
    }
    
    public partial class InputManager : Node
    {
        public static InputManager instance { get; private set; }

        private SparseEventMap<string, float> _keyPressedEventMap = new();
        private SparseEventMap<string, float> _keyReleasedEventMap = new();
        private SparseEventMap<string, float> _keyChangedEventMap = new();
        private SparseEventMap<InputAxis, float> _axisChangedEventMap = new();

        public override void _Ready()
        {
            instance = this;
        }
        
        public void RegisterCallback(string actionName, InputEventType type, Action<float> callback, Node owner)
        {
            GetMapForKeyType(type).RegisterCallback(actionName, callback);
            owner.TreeExited += () => RemoveCallback(actionName, type, callback);
        }
        
        public void RegisterAxisChangedCallback(string positive, string negative, Action<float> callback, Node owner)
        {
            var axis = new InputAxis(positive, negative);
            _axisChangedEventMap.RegisterCallback(axis, callback);
            owner.TreeExited += () => _axisChangedEventMap.RemoveCallback(axis, callback);
        }

        public override void _Input(InputEvent @event)
        {
            var inputEventType = GetEventType(@event);
            if (inputEventType == InputEventType.None)
            {
                return;
            }
            
            foreach (var axis in _axisChangedEventMap.GetMappedEvents())
            {
                if (!@event.IsAction(axis.negative) && !@event.IsAction(axis.positive))
                {
                    continue;
                }

                var strength = Input.GetActionStrength(axis.positive) - Input.GetActionStrength(axis.negative);
                _axisChangedEventMap.TriggerEvent(axis, strength);
                break;
            }
            

            var inputMap = GetMapForKeyType(inputEventType);
            if (inputMap == null)
            {
                return;
            }

            if (inputEventType is InputEventType.Pressed or InputEventType.Released)
            {
                foreach (var action in _keyChangedEventMap.GetMappedEvents())
                {
                    if (!@event.IsAction(action)) { continue; }
                    _keyChangedEventMap.TriggerEvent(action, Input.GetActionStrength(action));
                    break;
                }
            }
            
            foreach (var action in inputMap.GetMappedEvents())
            {
                if (!@event.IsAction(action)) { continue; }
                inputMap.TriggerEvent(action, Input.GetActionStrength(action));
                break;
            }
        }

        private void RemoveCallback(string actionName, InputEventType type, Action<float> callback)
        {
            GetMapForKeyType(type).RemoveCallback(actionName, callback);
            GD.Print("Unregistered callback");
        }

        private SparseEventMap<string, float> GetMapForKeyType(InputEventType type)
        {
            return type switch
            {
                InputEventType.Pressed => _keyPressedEventMap,
                InputEventType.Released => _keyReleasedEventMap,
                InputEventType.Changed => _keyChangedEventMap,
                _ => null
            };
        }

        private InputEventType GetEventType(InputEvent inputEvent)
        {
            if (inputEvent.IsEcho() || !inputEvent.IsActionType())
            {
                return InputEventType.None;
            }

            if (inputEvent.IsPressed())
            {
                return InputEventType.Pressed;
            }

            if (inputEvent.IsReleased())
            {
                return InputEventType.Released;
            }

            return InputEventType.Changed;
        }
    }
}
