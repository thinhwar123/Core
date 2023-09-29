using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EntityTriggerAction : MonoBehaviour
{
    [field: SerializeField] public UnityAction OnHitAction {get; private set;}
    public void OnHit()
    {
        OnHitAction?.Invoke();
    }
}
