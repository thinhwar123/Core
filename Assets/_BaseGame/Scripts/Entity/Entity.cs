using System.Collections;
using System.Collections.Generic;
using TW.Utility.CustomComponent;
using UnityEngine;

public class Entity : AwaitableCachedMonoBehaviour, IAttackAble, IDamageAble, IComboAble, IActiveAble
{
    public virtual void OnAttack()
    {
        
    }

    public virtual void OnDamage(int damage)
    {
        Debug.Log($"{gameObject.name} OnDamage {damage}");
    }

    public virtual void OnCombo()
    {
        Debug.Log($"{gameObject.name} OnCombo");
    }

    public virtual void OnActive()
    {
        Debug.Log($"{gameObject.name} OnActive");
    }
}
