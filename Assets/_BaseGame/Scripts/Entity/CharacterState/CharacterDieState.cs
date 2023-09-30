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
        (owner as IDieStateHandler).OnDieStateRequest();
    }
    public override async UniTask OnEnter(Character owner, CancellationToken token)
    {
        await (owner as IDieStateHandler).OnDieStateEnter(token);
    }
    public override async UniTask OnExecute(Character owner, CancellationToken token)
    {
        await (owner as IDieStateHandler).OnDieStateExecute(token);
    }
    public override async UniTask OnExit(Character owner, CancellationToken token)
    {
        await (owner as IDieStateHandler).OnDieStateExit(token);
    }
}

public partial class Character : CharacterDieState.IDieStateHandler
{
    public void OnDieStateRequest()
    {
        CurrentCell.UnRegisterOwner();
        Destroy(UIHealthBar.gameObject);
        SetHide(true);
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
