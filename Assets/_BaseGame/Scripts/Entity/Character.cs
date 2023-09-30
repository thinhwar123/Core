using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sirenix.OdinInspector;
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
    [field: SerializeField, ReadOnly, FoldoutGroup("Debug")] public int AttackDamage {get; private set;}
    [field: SerializeField, ReadOnly, FoldoutGroup("Debug")] public int HitPoint {get; private set;}
    [field: SerializeField,ReadOnly, FoldoutGroup("Debug")] public Cell CurrentCell {get; private set;}
    [field: SerializeField, ReadOnly, FoldoutGroup("Debug")] private CharacterModel CharacterModel {get; set;}
    [field: SerializeField, ReadOnly, FoldoutGroup("Debug")] public Animator Animator {get; private set;}
    [field: SerializeField, ReadOnly, FoldoutGroup("Debug")] public string CurrentAnimation {get; private set;}
    [field: SerializeField, ReadOnly, FoldoutGroup("Debug")] public List<Cell> CellPath {get; private set;}
    [field: SerializeField, ReadOnly, FoldoutGroup("Debug")] public List<Enemy> AroundEnemies {get; private set;}
    [field: SerializeField, ReadOnly, FoldoutGroup("Debug")] public int CurrentMoveIndex {get; private set;}
    [field: SerializeField, ReadOnly, FoldoutGroup("Debug")] public bool IsHide {get; private set;}
    [field: SerializeField, ReadOnly, FoldoutGroup("Debug")] public bool IsReadyCombo {get; private set;}
    [field: SerializeField, ReadOnly, FoldoutGroup("Debug")] public bool IsIdle {get; private set;}
    public bool IsDeath => HitPoint <= 0;
    private UIHealthBar UIHealthBar { get; set; }
    private Tween MoveTween { get; set; }
    private Tween RotateTween { get; set; }
    private bool IsLeader => TeamIndex == 0;
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

    private void InitTriggerAction()
    {
        CharacterModel.EntityTriggerAction.OnAttackAction += OnAttack;
        CharacterModel.EntityTriggerAction.OnComboAction += OnCombo;
        CharacterModel.EntityTriggerAction.OnActiveAction += OnActive;
    }
    public virtual void InitConfig(CharacterConfig characterConfig, int teamIndex, Cell startCell)
    {
        CharacterConfig = characterConfig;
        TeamIndex = teamIndex;
        AttackDamage = characterConfig.AttackDamage;
        HitPoint = characterConfig.HitPoint;
        CharacterModel = Instantiate(CharacterConfig.CharacterModel, CharacterModelContainer);
        Animator = CharacterModel.Animator;
        CurrentCell = startCell;
        IsReadyCombo = false;
        
        InitTriggerAction();

        if (IsLeader)
        {
            CurrentCell.SetupConsume();
            UIHealthBar = GameManager.Instance.CreateUIHealthBar();
            UIHealthBar.SetupHealthBar(CharacterConfig.CharacterAttribute, Transform);
            UIHealthBar.UpdateValue(1);
        }
    }
    private async UniTask<float> GetAnimationDuration(string stateAnimationName, CancellationToken token){
        if (stateAnimationName == "") return 0;
        while (!Animator.GetCurrentAnimatorStateInfo(0).IsName(stateAnimationName))
        {
            await UniTask.Delay(100, cancellationToken: token);
        }
        return Animator.GetCurrentAnimatorStateInfo(0).length;
    }

    public void MoveFollowPath(List<Cell> cells)
    {
        CellPath = cells;
        CurrentMoveIndex = 0;
        StateMachine.RequestTransition(CharacterMoveState.Instance);
    }
    

    protected override void OnDestroy()
    {
        base.OnDestroy();
        StateMachine.Stop();
    }

    public override void OnAttack()
    {
        base.OnAttack();
        TargetEnemy.OnDamage(AttackDamage);
    }

    public override void OnDamage(int damage)
    {
        base.OnDamage(damage);
        HitPoint -= damage;
        if (HitPoint < 0) HitPoint = 0;
        UIHealthBar.UpdateValue(HitPoint / (float) CharacterConfig.HitPoint);
        UIDamagePopup damagePopup = GameManager.Instance.CreateUIDamagePopup();
        damagePopup.SetupDamagePopup(damage, Transform);
        
        if (IsDeath)
        {
            StateMachine.RequestTransition(CharacterDieState.Instance);
        }
    }

    public override void OnCombo()
    {
        base.OnCombo();
        AroundEnemies.ForEach(e => e.OnDamage(AttackDamage));
    }

    public override void OnActive()
    {
        base.OnActive();
    }

    public void SetHide(bool isHide)
    {
        IsHide = isHide;
        CharacterModel.gameObject.SetActive(!IsHide);
    }
    public EAreaType GetComboAreaType(int comboIndex)
    {
        EAreaType areaType = EAreaType.None;
        for (var i = 0; i < CharacterConfig.CharacterChainCombo.Length; i++)
        {
            if (CharacterConfig.CharacterChainCombo[i].ChainCount <= comboIndex)
            {
                areaType = CharacterConfig.CharacterChainCombo[i].ChainComboType;
            }
        }
        return areaType;
    }
}
