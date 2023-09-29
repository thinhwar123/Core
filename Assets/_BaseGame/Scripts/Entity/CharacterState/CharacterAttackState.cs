using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
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
        throw new System.NotImplementedException();
    }

    public async UniTask OnAttackStateEnter(CancellationToken token)
    {
        throw new System.NotImplementedException();
    }

    public async UniTask OnAttackStateExecute(CancellationToken token)
    {
        throw new System.NotImplementedException();
    }

    public async UniTask OnAttackStateExit(CancellationToken token)
    {
        throw new System.NotImplementedException();
    }
}