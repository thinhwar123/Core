using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterModel : MonoBehaviour
{
    [field: SerializeField] public Animator Animator { get; private set; }
    
    public void OnIdleStateEnter()
    {
        Animator.Play("idle");
    }
    public void OnMoveStateEnter()
    {
        Animator.Play("run");
    }
    public void OnAttackStateEnter()
    {
        Animator.Play("attack");
    }
    public void OnDieStateEnter()
    {
        Animator.Play("die");
    }

    public void OnComboStateEnter()
    {
        Animator.Play("combo");
    }
    public void OnActiveSkillStateEnter()
    {
        Animator.Play("active");
    }
}

