using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sirenix.OdinInspector;
using TW.Utility.DesignPattern;
using UnityEngine;

public partial class Enemy : Entity
{
    [field: SerializeField] public Transform EnemyModelContainer {get; private set;}
    [field: SerializeField] public EnemyConfig EnemyConfig {get; private set;} 
    [field: SerializeField]private float MovementSpeed { get; set; }
    [field: SerializeField] public float RotateSpeed {get; private set;}
    [field: SerializeField] public Cell CurrentCell {get; private set;}
    [field: SerializeField] public int HitPoint {get; private set;}
    [field: SerializeField] public int AttackDamage {get; private set;}
    [field: SerializeField, StateMachineDebug] UniTaskStateMachine<Enemy> StateMachine {get; set;} 
    [field: SerializeField, ReadOnly, FoldoutGroup("Debug")] public Animator Animator {get; private set;}
    [field: SerializeField, ReadOnly, FoldoutGroup("Debug")] private EnemyModel EnemyModel {get; set;}
    [field: SerializeField, ReadOnly, FoldoutGroup("Debug")] private Character CharacterTarget {get; set;}
    [field: SerializeField, ReadOnly, FoldoutGroup("Debug")] public List<Cell> AroundCells {get; private set;}
    [field: SerializeField, ReadOnly, FoldoutGroup("Debug")] private List<Cell> CellPath {get; set;}
    [field: SerializeField, ReadOnly, FoldoutGroup("Debug")] private Character TargetCharacter {get; set;}
    public bool IsTakeTurn { get; set; }
    private UIHealthBar UIHealthBar { get; set; }
    private Tween MoveTween { get; set; }
    private Tween RotateTween { get; set; }
    private int CurrentMoveIndex { get; set; }

    private void Awake()
    {
        InitStateMachine();

    }
    public void InitConfig(EnemyConfig enemyConfig, Cell startCell)
    {
        EnemyConfig = enemyConfig;
        CurrentCell = startCell;
        EnemyModel = Instantiate(EnemyConfig.EnemyModel, EnemyModelContainer);
        Animator = EnemyModel.Animator;
        HitPoint = EnemyConfig.HitPoint;
        AttackDamage = EnemyConfig.AttackDamage;

        IsTakeTurn = false;
        
        UIHealthBar = GameManager.Instance.CreateUIHealthBar();
        UIHealthBar.SetupHealthBar(EAttribute.White, Transform);
        UIHealthBar.UpdateValue(1);
    }

    private void InitStateMachine()
    {
        StateMachine = new UniTaskStateMachine<Enemy>(this);
        StateMachine.RegisterState(EnemyIdleState.Instance);
        StateMachine.Run();
    }
    
    private void InitTriggerAction()
    {
        EnemyModel.EntityTriggerAction.OnAttackAction += OnAttack;
        EnemyModel.EntityTriggerAction.OnComboAction += OnCombo;
        EnemyModel.EntityTriggerAction.OnActiveAction += OnActive;
    }
    
    public override void OnAttack()
    {
        base.OnAttack();
        CharacterTarget.OnDamage(AttackDamage);
    }

    public override void OnDamage(int damage)
    {
        base.OnDamage(damage);
        HitPoint -= damage;
        if (HitPoint < 0) HitPoint = 0;
        UIHealthBar.UpdateValue(HitPoint / (float) EnemyConfig.HitPoint);
        UIDamagePopup damagePopup = GameManager.Instance.CreateUIDamagePopup();
        damagePopup.SetupDamagePopup(damage, Transform);
    }

    public override void OnCombo()
    {
        base.OnCombo();
        CharacterTarget.OnDamage(AttackDamage);
    }

    public override void OnActive()
    {
        base.OnActive();
    }

    public void PlayTurn()
    {
        IsTakeTurn = true;
        List<Cell> moveAbleCells = CellManager.Instance.GetCellInRange(CurrentCell.XPosition, CurrentCell.YPosition, EnemyConfig.MoveStep);
        
        Cell targetCell = null; 
        float minDistance = float.MaxValue;
        foreach (Cell moveAbleCell in moveAbleCells)
        {
            float distance = Vector3.Distance(moveAbleCell.Transform.position, TeamManager.Instance.Leader.CurrentCell.Transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                targetCell = moveAbleCell;
            }
        }
        // Move to target cell cell by cell

        CellPath = CellManager.Instance.GetPath(CurrentCell, targetCell);
        CurrentMoveIndex = 0;
        StateMachine.RequestTransition(EnemyMoveState.Instance);
    }
    private async UniTask<float> GetAnimationDuration(string stateAnimationName, CancellationToken token){
        if (stateAnimationName == "") return 0;
        while (!Animator.GetCurrentAnimatorStateInfo(0).IsName(stateAnimationName))
        {
            await UniTask.Delay(100, cancellationToken: token);
        }
        return Animator.GetCurrentAnimatorStateInfo(0).length;
    }
}