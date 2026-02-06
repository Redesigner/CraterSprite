using Godot;
using System;
using System.Collections.Generic;

namespace CraterSprite
{
    public class SparseEventMap<TKey, TCallback>
    {
        private readonly Dictionary<TKey, CraterEvent<TCallback>> _map = new ();

        public void RegisterCallback(TKey key, Action<TCallback> callback)
        {
            if (_map.TryGetValue(key, out var callbacks))
            {
                callbacks.AddListener(callback);
            }
            else
            {
                _map.Add(key, [callback]);
            }
        }

        public void TriggerEvent(TKey key, TCallback argument)
        {
            if (!_map.TryGetValue(key, out var craterEvent))
            {
                return;
            }
            
            craterEvent.Invoke(argument);
        }

        public bool RemoveCallback(TKey key, Action<TCallback> callback)
        {
            if (!_map.TryGetValue(key, out var callables))
            {
                return false;
                
            }

            if (!callables.RemoveListener(callback))
            {
                return false;
            }
            
            if (callables.empty)
            {
                _map.Remove(key);
            }

            return true;
        }

        public IEnumerable<TKey> GetMappedEvents()
        {
            return _map.Keys;
        }
    }
    
    public class SparseEventMap<TKey>
    {
        private readonly Dictionary<TKey, CraterEvent> _map = new ();

        public void RegisterCallback(TKey key, Action callback)
        {
            if (_map.TryGetValue(key, out var callbacks))
            {
                callbacks.Add(callback);
            }
            else
            {
                _map.Add(key, [callback]);
            }
        }

        public void TriggerEvent(TKey key)
        {
            if (!_map.TryGetValue(key, out var craterEvent))
            {
                return;
            }
            
            craterEvent.Invoke();
        }

        public bool RemoveCallback(TKey key, Action callback)
        {
            if (!_map.TryGetValue(key, out var callables))
            {
                return false;
                
            }

            if (!callables.RemoveListener(callback))
            {
                return false;
            }
            
            if (callables.empty)
            {
                _map.Remove(key);
            }

            return true;
        }

        public IEnumerable<TKey> GetMappedEvents()
        {
            return _map.Keys;
        }
    }
    
    public class SparseEventMap<TKey, T0, T1>
    {
        private readonly Dictionary<TKey, CraterEvent<T0, T1>> _map = new ();

        public void RegisterCallback(TKey key, Action<T0, T1> callback)
        {
            if (_map.TryGetValue(key, out var callbacks))
            {
                callbacks.Add(callback);
            }
            else
            {
                _map.Add(key, [callback]);
            }
        }

        public void TriggerEvent(TKey key, T0 arg0, T1 arg1)
        {
            if (!_map.TryGetValue(key, out var craterEvent))
            {
                return;
            }
            
            craterEvent.Invoke(arg0, arg1);
        }

        public bool RemoveCallback(TKey key, Action<T0, T1> callback)
        {
            if (!_map.TryGetValue(key, out var callables))
            {
                return false;
                
            }

            if (!callables.RemoveListener(callback))
            {
                return false;
            }
            
            if (callables.empty)
            {
                _map.Remove(key);
            }

            return true;
        }

        public IEnumerable<TKey> GetMappedEvents()
        {
            return _map.Keys;
        }
    }
    
}
