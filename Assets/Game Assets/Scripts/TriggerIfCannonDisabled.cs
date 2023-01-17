using System;
using UnityEngine;
using UnityEngine.Events;

public class TriggerIfCannonDisabled : MonoBehaviour
{
    [SerializeField]
    UnityEvent onTriggered;

    [SerializeField]
    private CannonController cannon;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !cannon.enabled)
            onTriggered.Invoke();
    }
}
