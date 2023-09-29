using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using TW.Utility.CustomComponent;
using TW.Utility.DesignPattern;
using UnityEngine;

public partial class Character : Entity
{
    [field: SerializeField, StateMachineDebug] public UniTaskStateMachine<Character> StateMachine {get; private set;}
    [field: SerializeField, InlineEditor] public CharacterConfig CharacterConfig {get; private set;}
    [field: SerializeField] public int TeamIndex {get; private set;}
    [field: SerializeField] public Transform CharacterModelContainer {get; private set;}
    [field: SerializeField] public float MovementSpeed {get; private set;}
    [field: SerializeField] public float RotateSpeed {get; private set;}
    [field: SerializeField] public Enemy TargetEnemy {get; private set;}
    [field: SerializeField,ReadOnly, FoldoutGroup("Debug")] public Cell CurrentCell {get; private set;}
    [field: SerializeField, ReadOnly, FoldoutGroup("Debug")] private CharacterModel CharacterModel {get; set;}
    [field: SerializeField, ReadOnly, FoldoutGroup("Debug")] public Animator Animator {get; private set;}
    [field: SerializeField, ReadOnly, FoldoutGroup("Debug")] public string CurrentAnimation {get; private set;}
    [field: SerializeField, ReadOnly, FoldoutGroup("Debug")] public List<Cell> CellPath {get; private set;}
    [field: SerializeField, ReadOnly, FoldoutGroup("Debug")] public int CurrentMoveIndex {get; private set;}
    
    private Tween MoveTween { get; set; }
    private Tween RotateTween { get; set; }
    private void Awake()
    {
        InitStateMachine();
    }

    private void InitStateMachine()
    {
        StateMachine = new UniTaskStateMachine<Character>(this);
        StateMachine.RegisterState(CharacterIdleState.Instance);
        StateMachine.Run();
    }
    public virtual void InitConfig(CharacterConfig characterConfig, int teamIndex, Cell startCell)
    {
        CharacterConfig = characterConfig;
        TeamIndex = teamIndex;
        CharacterModel = Instantiate(CharacterConfig.CharacterModel, CharacterModelContainer);
        Animator = CharacterModel.Animator;
        CurrentCell = startCell;
    }
    public virtual void PlayAnimation(string animationName)
    {
        if (animationName == "") return;
        if (CurrentAnimation == animationName) return;
        Animator.Play(animationName);
    }

    public void MoveFollowPath(List<Cell> cells)
    {
        CellPath = cells;
        StateMachine.RequestTransition(CharacterMoveState.Instance);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        StateMachine.Stop();
    }
}
