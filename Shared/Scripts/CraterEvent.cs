using System;
using System.Collections;
using System.Collections.Generic;

namespace CraterSprite;

public class CraterEvent : IEnumerable
{
    private readonly List<Action> _list = [];

    public IEnumerator GetEnumerator() => _list.GetEnumerator();

    public void Add(Action val)
    {
        _list.Add(val);
    } 
    
    public void AddListener(Action action)
    {
        _list.Add(action);
    }

    public bool RemoveListener(Action action)
    {
        return _list.Remove(action);
    }

    public void Invoke()
    {
        foreach (var action in _list)
        {
            action();
        }
    }

    public bool empty => _list.Count == 0;
}

public class CraterEvent<T> : IEnumerable
{
    private readonly List<Action<T>> _list = [];

    public IEnumerator GetEnumerator() => _list.GetEnumerator();
    
    public void Add(Action<T> val)
    {
        _list.Add(val);
    } 
    
    public void AddListener(Action<T> action)
    {
        _list.Add(action);
    }

    public bool RemoveListener(Action<T> action)
    {
        return _list.Remove(action);
    }

    public void Invoke(T arg)
    {
        foreach (var action in _list)
        {
            action(arg);
        }
    }

    public bool empty => _list.Count == 0;
}

public class CraterEvent<T0, T1> : IEnumerable
{
    private readonly List<Action<T0, T1>> _list = [];

    public IEnumerator GetEnumerator() => _list.GetEnumerator();

    public void Add(Action<T0, T1> val)
    {
        _list.Add(val);
    }
    
    public void AddListener(Action<T0, T1> action)
    {
        _list.Add(action);
    }

    public bool RemoveListener(Action<T0, T1> action)
    {
        return _list.Remove(action);
    }

    public void Invoke(T0 arg0, T1 arg1)
    {
        foreach (var action in _list)
        {
            action(arg0, arg1);
        }
    }
    
    public bool empty => _list.Count == 0;
}