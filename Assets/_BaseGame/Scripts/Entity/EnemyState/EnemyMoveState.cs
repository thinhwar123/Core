using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TW.Utility.DesignPattern;
using UnityEngine;

public class EnemyMoveState : UniTaskState<Enemy>
{
    public interface IMoveStateHandler
    {
        public void OnMoveStateRequest();
        public UniTask OnMoveStateEnter(CancellationToken token);
        public UniTask OnMoveStateExecute(CancellationToken token);
        public UniTask OnMoveStateExit(CancellationToken token);
    }
    
    private static EnemyMoveState m_Instance;
    public static EnemyMoveState Instance => m_Instance ??= new EnemyMoveState();
    
    public override void OnRequest(Enemy owner)
    {
        (owner as EnemyMoveState.IMoveStateHandler).OnMoveStateRequest();
    }
    public override async UniTask OnEnter(Enemy owner, CancellationToken token)
    {
        await (owner as EnemyMoveState.IMoveStateHandler).OnMoveStateEnter(token);
    }
    public override async UniTask OnExecute(Enemy owner, CancellationToken token)
    {
        await (owner as EnemyMoveState.IMoveStateHandler).OnMoveStateExecute(token);
    }
    public override async UniTask OnExit(Enemy owner, CancellationToken token)
    {
        await (owner as EnemyMoveState.IMoveStateHandler).OnMoveStateExit(token);
    }
}

public partial class Enemy : EnemyMoveState.IMoveStateHandler
{
    public void OnMoveStateRequest()
    {
        
    }

    public async UniTask OnMoveStateEnter(CancellationToken token)
    {
        EnemyModel.OnMoveStateEnter();
    }

    public async UniTask OnMoveStateExecute(CancellationToken token)
    {
        if (CurrentMoveIndex >= CellPath.Count)
        {
            AroundCells = CurrentCell.GetCell(EAreaType.SmallCross);
            if (AroundCells.Any(c => c.IsCharacterCell))
            {
                StateMachine.RequestTransition(EnemyAttackState.Instance);
            }

            IsTakeTurn = false;
            StateMachine.RequestTransition(EnemyIdleState.Instance);
            return;
        }
        CurrentCell = CellPath[CurrentMoveIndex];
        var targetPosition = CurrentCell.Transform.position;
        targetPosition.y = Transform.position.y;
        float distance = Vector3.Distance(Transform.position, targetPosition);
        float moveDuration = distance / MovementSpeed;
        MoveTween = Transform.DOMove(targetPosition, moveDuration).SetEase(Ease.Linear).OnComplete(() =>
        {
            CurrentMoveIndex++;
        });
        RotateTween?.Kill();
        float rotateDuration = Vector3.Angle(Transform.forward, targetPosition - Transform.position) / RotateSpeed;
        if (rotateDuration > 0.01f)
        {
            RotateTween = Transform.DORotateQuaternion(Quaternion.LookRotation(targetPosition - Transform.position), rotateDuration).SetEase(Ease.Linear);
        }
        await UniTask.Delay((int) (moveDuration * 1000), cancellationToken: token);
    }

    public async UniTask OnMoveStateExit(CancellationToken token)
    {
        
    }
}