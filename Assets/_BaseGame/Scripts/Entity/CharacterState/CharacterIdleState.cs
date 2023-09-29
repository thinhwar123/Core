using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using TW.Utility.DesignPattern;
using UnityEngine;

public class CharacterIdleState : UniTaskState<Character>
{
    public interface IIdleStateHandler
    {
        public void OnIdleStateRequest();
        public UniTask OnIdleStateEnter(CancellationToken token);
        public UniTask OnIdleStateExecute(CancellationToken token);
        public UniTask OnIdleStateExit(CancellationToken token);
    }

    private static CharacterIdleState m_Instance;
    public static CharacterIdleState Instance => m_Instance ??= new CharacterIdleState();
    public override void OnRequest(Character owner)
    {
        (owner as IIdleStateHandler).OnIdleStateRequest();
    }
    public override async UniTask OnEnter(Character owner, CancellationToken token)
    {
        await (owner as IIdleStateHandler).OnIdleStateEnter(token);
    }
    public override async UniTask OnExecute(Character owner, CancellationToken token)
    {
        await (owner as IIdleStateHandler).OnIdleStateExecute(token);
    }
    public override async UniTask OnExit(Character owner, CancellationToken token)
    {
        await (owner as IIdleStateHandler).OnIdleStateExit(token);
    }
}

public partial class Character : CharacterIdleState.IIdleStateHandler
{
    public void OnIdleStateRequest()
    {
        
    }

    public async UniTask OnIdleStateEnter(CancellationToken token)
    {
        
    }

    public async UniTask OnIdleStateExecute(CancellationToken token)
    {
       
    }

    public async UniTask OnIdleStateExit(CancellationToken token)
    {
        
    }
}