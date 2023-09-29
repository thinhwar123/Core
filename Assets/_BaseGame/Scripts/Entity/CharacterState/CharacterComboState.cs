using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using TW.Utility.DesignPattern;
using UnityEngine;

public class CharacterComboState : UniTaskState<Character>
{
    public interface IComboStateHandler
    {
        public void OnComboStateRequest();
        public UniTask OnComboStateEnter(CancellationToken token);
        public UniTask OnComboStateExecute(CancellationToken token);
        public UniTask OnComboStateExit(CancellationToken token);
    }
    
    private static CharacterComboState m_Instance;
    public static CharacterComboState Instance => m_Instance ??= new CharacterComboState();
    public override void OnRequest(Character owner)
    {
        (owner as IComboStateHandler).OnComboStateRequest();
    }
    public override async UniTask OnEnter(Character owner, CancellationToken token)
    {
        await (owner as IComboStateHandler).OnComboStateEnter(token);
    }
    public override async UniTask OnExecute(Character owner, CancellationToken token)
    {
        await (owner as IComboStateHandler).OnComboStateExecute(token);
    }
    public override async UniTask OnExit(Character owner, CancellationToken token)
    {
        await (owner as IComboStateHandler).OnComboStateExit(token);
    }
    
}

public partial class Character : CharacterComboState.IComboStateHandler
{
    public void OnComboStateRequest()
    {
        
    }

    public async UniTask OnComboStateEnter(CancellationToken token)
    {
        
    }

    public async UniTask OnComboStateExecute(CancellationToken token)
    {
        
    }

    public async UniTask OnComboStateExit(CancellationToken token)
    {
        
    }
}