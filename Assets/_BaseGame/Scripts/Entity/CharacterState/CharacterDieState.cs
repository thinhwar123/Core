using System.Threading;
using Cysharp.Threading.Tasks;
using TW.Utility.DesignPattern;

public class CharacterDieState : UniTaskState<Character>
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
    
    public override void OnRequest(Character owner)
    {
        (owner as CharacterDieState.IDieStateHandler).OnDieStateRequest();
    }
    public override async UniTask OnEnter(Character owner, CancellationToken token)
    {
        await (owner as CharacterDieState.IDieStateHandler).OnDieStateEnter(token);
    }
    public override async UniTask OnExecute(Character owner, CancellationToken token)
    {
        await (owner as CharacterDieState.IDieStateHandler).OnDieStateExecute(token);
    }
    public override async UniTask OnExit(Character owner, CancellationToken token)
    {
        await (owner as CharacterDieState.IDieStateHandler).OnDieStateExit(token);
    }
}

public partial class Character : CharacterDieState.IDieStateHandler
{
    public void OnDieStateRequest()
    {
        
    }

    public async UniTask OnDieStateEnter(CancellationToken token)
    {
        SetHide(true);
        IsDeath = true;
        
    }

    public async UniTask OnDieStateExecute(CancellationToken token)
    {
        
    }

    public async UniTask OnDieStateExit(CancellationToken token)
    {
        
    }
}
