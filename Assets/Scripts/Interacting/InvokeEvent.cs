using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InvokeEvent : MonoBehaviour
{
    [SerializeField] UnityEvent _Event;

    public void Invoke()
    {
        _Event.Invoke();
    }
}
