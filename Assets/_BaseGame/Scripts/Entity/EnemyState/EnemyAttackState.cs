using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TW.Utility.DesignPattern;
using UnityEngine;

public class EnemyAttackState : UniTaskState<Enemy>
{
    public interface IAttackStateHandler
    {
        public void OnAttackStateRequest();
        public UniTask OnAttackStateEnter(CancellationToken token);
        public UniTask OnAttackStateExecute(CancellationToken token);
        public UniTask OnAttackStateExit(CancellationToken token);
    }
    
    private static EnemyAttackState m_Instance;
    public static EnemyAttackState Instance => m_Instance ??= new EnemyAttackState();
    
    public override void OnRequest(Enemy owner)
    {
        (owner as IAttackStateHandler).OnAttackStateRequest();
    }
    public override async UniTask OnEnter(Enemy owner, CancellationToken token)
    {
        await (owner as IAttackStateHandler).OnAttackStateEnter(token);
    }
    public override async UniTask OnExecute(Enemy owner, CancellationToken token)
    {
        await (owner as IAttackStateHandler).OnAttackStateExecute(token);
    }
    public override async UniTask OnExit(Enemy owner, CancellationToken token)
    {
        await (owner as IAttackStateHandler).OnAttackStateExit(token);
    }
}
public partial class Enemy : EnemyAttackState.IAttackStateHandler
{
    public void OnAttackStateRequest()
    {
        TargetCharacter = AroundCells.First(c => c.IsCharacterCell).Owner as Character;
    }

    public async UniTask OnAttackStateEnter(CancellationToken token)
    {
        
    }

    public async UniTask OnAttackStateExecute(CancellationToken token)
    {

        
            
        RotateTween?.Kill();
        float rotateDuration = Vector3.Angle(Transform.forward, TargetCharacter.Transform.position - Transform.position) / RotateSpeed;
        RotateTween = Transform.DORotateQuaternion(Quaternion.LookRotation(TargetCharacter.Transform.position - Transform.position), rotateDuration).SetEase(Ease.Linear);
        await UniTask.Delay((int) (rotateDuration * 1000), cancellationToken: token);

        Debug.Log(TargetCharacter == null);
        EnemyModel.OnAttackStateEnter();
        // wait for animation complete
        float duration  = await GetAnimationDuration("attack", token);
        await UniTask.Delay((int) (duration * 1000), cancellationToken: token);
        IsTakeTurn = false;
        StateMachine.RequestTransition(EnemyIdleState.Instance);
    }

    public async UniTask OnAttackStateExit(CancellationToken token)
    {
        
    }
}
