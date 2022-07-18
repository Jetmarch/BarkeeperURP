using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SOEvent", menuName = "Events")]
public class SOEvent : ScriptableObject
{
    private List<EventListener> listeners = new List<EventListener>();

    public void Raise(SOEventArgs e = null)
    {
        for (int i = listeners.Count - 1; i >= 0; i--)
        {
            listeners[i].OnEventRaised(e);
        }
    }

    public void RegisterListener(EventListener listener)
    {
        listeners.Add(listener);
    }

    public void UnregisterListener(EventListener listener)
    {
        if (listeners.Contains(listener))
        {
            listeners.Remove(listener);
        }
    }
}
