using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using TW.Utility.DesignPattern;
using UnityEngine;

public class EnemyDieState : UniTaskState<Enemy>
{
    public interface IDieStateHandler
    {
        public void OnDieStateRequest();
        public UniTask OnDieStateEnter(CancellationToken token);
        public UniTask OnDieStateExecute(CancellationToken token);
        public UniTask OnDieStateExit(CancellationToken token);
    }
    
    private static CharacterDieState m_Instance;
    public static CharacterDieState Instance => m_Instance ??= new CharacterDieState();
    
    public override void OnRequest(Enemy owner)
    {
        (owner as CharacterDieState.IDieStateHandler).OnDieStateRequest();
    }
    public override async UniTask OnEnter(Enemy owner, CancellationToken token)
    {
        await (owner as CharacterDieState.IDieStateHandler).OnDieStateEnter(token);
    }
    public override async UniTask OnExecute(Enemy owner, CancellationToken token)
    {
        await (owner as CharacterDieState.IDieStateHandler).OnDieStateExecute(token);
    }
    public override async UniTask OnExit(Enemy owner, CancellationToken token)
    {
        await (owner as CharacterDieState.IDieStateHandler).OnDieStateExit(token);
    }
}

public partial class Enemy : EnemyDieState.IDieStateHandler
{
    public void OnDieStateRequest()
    {
        
    }

    public async UniTask OnDieStateEnter(CancellationToken token)
    {
        
    }

    public async UniTask OnDieStateExecute(CancellationToken token)
    {
        
    }

    public async UniTask OnDieStateExit(CancellationToken token)
    {
        
    }
}
