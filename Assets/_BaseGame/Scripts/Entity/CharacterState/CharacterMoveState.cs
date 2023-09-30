using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TW.Utility.DesignPattern;
using UnityEngine;

public class CharacterMoveState : UniTaskState<Character>
{
    public interface IMoveStateHandler
    {
        public void OnMoveStateRequest();
        public UniTask OnMoveStateEnter(CancellationToken token);
        public UniTask OnMoveStateExecute(CancellationToken token);
        public UniTask OnMoveStateExit(CancellationToken token);
    }

    private static CharacterMoveState m_Instance;
    public static CharacterMoveState Instance => m_Instance ??= new CharacterMoveState();
    public override void OnRequest(Character owner)
    {
        (owner as CharacterMoveState.IMoveStateHandler).OnMoveStateRequest();
    }
    public override async UniTask OnEnter(Character owner, CancellationToken token)
    {
        await (owner as CharacterMoveState.IMoveStateHandler).OnMoveStateEnter(token);
    }
    public override async UniTask OnExecute(Character owner, CancellationToken token)
    {
        await (owner as CharacterMoveState.IMoveStateHandler).OnMoveStateExecute(token);
    }
    public override async UniTask OnExit(Character owner, CancellationToken token)
    {
        await (owner as CharacterMoveState.IMoveStateHandler).OnMoveStateExit(token);
    }
}

public partial class Character : CharacterMoveState.IMoveStateHandler
{
    
    public void OnMoveStateRequest()
    {
        if (!IsLeader)
        {
            SetHide(false);
        }
    }

    public async UniTask OnMoveStateEnter(CancellationToken token)
    {
        CharacterModel.OnMoveStateEnter();
    }

    public async UniTask OnMoveStateExecute(CancellationToken token)
    {
        if (CurrentMoveIndex >= CellPath.Count)
        {
            if (IsLeader) 
            {
                CurrentCell.RegisterOwner(this);
            }
            StateMachine.RequestTransition(CharacterComboState.Instance);
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
        CurrentCell.SetupConsume();
        List<Cell> aroundCells = CurrentCell.GetCell(EAreaType.SmallCross);
        if (aroundCells.Any(c => c.IsEnemyCell))
        {
            AroundEnemies = aroundCells.Where(c => c.IsEnemyCell).Select(c => c.Owner as Enemy).ToList();
            StateMachine.RequestTransition(CharacterAttackState.Instance);
        }
    }

    public async UniTask OnMoveStateExit(CancellationToken token)
    {
        
    }
}