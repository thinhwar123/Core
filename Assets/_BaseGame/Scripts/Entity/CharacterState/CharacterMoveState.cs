using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
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
        
    }

    public async UniTask OnMoveStateEnter(CancellationToken token)
    {
        
    }

    public async UniTask OnMoveStateExecute(CancellationToken token)
    {
        
    }

    public async UniTask OnMoveStateExit(CancellationToken token)
    {
        
    }
}