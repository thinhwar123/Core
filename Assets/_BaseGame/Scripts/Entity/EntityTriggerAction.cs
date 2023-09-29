using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EntityTriggerAction : MonoBehaviour
{
    [field: SerializeField] public UnityAction OnAttackAction {get; set;}
    [field: SerializeField] public UnityAction OnComboAction {get; set;}
    [field: SerializeField] public UnityAction OnActiveAction {get; set;}
    public void OnAttack()
    {
        OnAttackAction?.Invoke();
    }
    public void OnCombo()
    {
        OnComboAction?.Invoke();
    }
    public void OnActive()
    {
        OnActiveAction?.Invoke();
    }
}
