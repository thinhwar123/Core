using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TW.Utility.DesignPattern;
using UnityEngine;

public class CharacterAttackState : UniTaskState<Character>
{
    public interface IAttackStateHandler
    {
        public void OnAttackStateRequest();
        public UniTask OnAttackStateEnter(CancellationToken token);
        public UniTask OnAttackStateExecute(CancellationToken token);
        public UniTask OnAttackStateExit(CancellationToken token);
    }
    
    private static CharacterAttackState m_Instance;
    public static CharacterAttackState Instance => m_Instance ??= new CharacterAttackState();
    
    public override void OnRequest(Character owner)
    {
        (owner as IAttackStateHandler).OnAttackStateRequest();
    }
    public override async UniTask OnEnter(Character owner, CancellationToken token)
    {
        await (owner as IAttackStateHandler).OnAttackStateEnter(token);
    }
    public override async UniTask OnExecute(Character owner, CancellationToken token)
    {
        await (owner as IAttackStateHandler).OnAttackStateExecute(token);
    }
    public override async UniTask OnExit(Character owner, CancellationToken token)
    {
        await (owner as IAttackStateHandler).OnAttackStateExit(token);
    }
}

public partial class Character : CharacterAttackState.IAttackStateHandler
{
    public void OnAttackStateRequest()
    {
        
    }

    public async UniTask OnAttackStateEnter(CancellationToken token)
    {
        
    }

    public async UniTask OnAttackStateExecute(CancellationToken token)
    {
        for (int i = 0; i < AroundEnemies.Count; i++)
        {
            TargetEnemy = AroundEnemies[i];
            
            RotateTween?.Kill();
            float rotateDuration = Vector3.Angle(Transform.forward, TargetEnemy.Transform.position - Transform.position) / RotateSpeed;
            RotateTween = Transform.DORotateQuaternion(Quaternion.LookRotation(TargetEnemy.Transform.position - Transform.position), rotateDuration).SetEase(Ease.Linear);
            await UniTask.Delay((int) (rotateDuration * 1000), cancellationToken: token);
            CharacterModel.OnAttackStateEnter();
            // wait for animation complete
            float duration  = await GetAnimationDuration("attack", token);
            await UniTask.Delay((int) (duration * 1000), cancellationToken: token);
        }
        StateMachine.RequestTransition(CharacterMoveState.Instance);
    }
    
    public async UniTask OnAttackStateExit(CancellationToken token)
    {
        
    }
}