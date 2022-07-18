using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SOListener : MonoBehaviour
{
    public List<EventListener> listOfEvents = new List<EventListener>();

    private void OnEnable()
    {
        foreach(var item in listOfEvents)
        {
            item.RegisterListener();
        }
    }

    private void OnDisable()
    {
        foreach (var item in listOfEvents)
        {
            item.UnregisterListener();
        }
    }
}

[Serializable]
public class EventListener
{
    public SOEvent _event;

    public UnityEvent<SOEventArgs> response;

    public void RegisterListener()
    {
        _event.RegisterListener(this);
    }

    public void UnregisterListener()
    {
        _event.UnregisterListener(this);
    }

    public void OnEventRaised(SOEventArgs e = null)
    {
        response.Invoke(e);
    }
}

public class SOEventArgs
{
}