using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using TW.Utility.DesignPattern;
using UnityEngine;

public class EnemyIdleState : UniTaskState<Enemy>
{
    public interface IIdleStateHandler
    {
        public void OnIdleStateRequest();
        public UniTask OnIdleStateEnter(CancellationToken token);
        public UniTask OnIdleStateExecute(CancellationToken token);
        public UniTask OnIdleStateExit(CancellationToken token);
    }
    
    private static EnemyIdleState m_Instance;
    public static EnemyIdleState Instance => m_Instance ??= new EnemyIdleState();
    
    public override void OnRequest(Enemy owner)
    {
        (owner as IIdleStateHandler).OnIdleStateRequest();
    }
    public override async UniTask OnEnter(Enemy owner, CancellationToken token)
    {
        await (owner as IIdleStateHandler).OnIdleStateEnter(token);
    }
    public override async UniTask OnExecute(Enemy owner, CancellationToken token)
    {
        await (owner as IIdleStateHandler).OnIdleStateExecute(token);
    }
    public override async UniTask OnExit(Enemy owner, CancellationToken token)
    {
        await (owner as IIdleStateHandler).OnIdleStateExit(token);
    }
}

public partial class Enemy : EnemyIdleState.IIdleStateHandler
{
    public void OnIdleStateRequest()
    {
        CurrentCell.RegisterOwner(this);
    }

    public async UniTask OnIdleStateEnter(CancellationToken token)
    {
        EnemyModel.OnIdleStateEnter();
    }

    public async UniTask OnIdleStateExecute(CancellationToken token)
    {
        
    }

    public async UniTask OnIdleStateExit(CancellationToken token)
    {
        CurrentCell.UnRegisterOwner();
    }
}