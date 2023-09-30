using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
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
        IsReadyCombo = true;
    }

    public async UniTask OnComboStateEnter(CancellationToken token)
    {
        if (IsLeader)
        {
            CharacterModel.OnIdleStateEnter();
            await UniTask.WaitUntil(() => TeamManager.Instance.Characters.All(c => c.IsReadyCombo || c.IsIdle),
                cancellationToken: token);
            
        }
        else
        {
            SetHide(true);
            await UniTask.WaitUntil(() => !TeamManager.Instance.Characters[TeamIndex - 1].IsReadyCombo,
                cancellationToken: token);
            SetHide(false);
        }
    }

    public async UniTask OnComboStateExecute(CancellationToken token)
    {
        EAreaType comboAreaType = GetComboAreaType(CurrentMoveIndex);
        if (comboAreaType == EAreaType.None)
        {
            StateMachine.RequestTransition(CharacterIdleState.Instance);
            IsReadyCombo = false;
            return;
        }
        List<Cell> comboCells = CurrentCell.GetCell(comboAreaType);
        AroundEnemies = comboCells.Where(x => x.IsEnemyCell).Select(x => x.Owner as Enemy).ToList();
        if (AroundEnemies.Count > 0)
        {
            CellManager.Instance.FocusListCell(comboCells);
            // rotate to first enemy
            TargetEnemy = AroundEnemies[0];
            RotateTween?.Kill();
            float rotateDuration = Vector3.Angle(Transform.forward, TargetEnemy.Transform.position - Transform.position) / RotateSpeed;
            RotateTween = Transform.DORotateQuaternion(Quaternion.LookRotation(TargetEnemy.Transform.position - Transform.position), rotateDuration).SetEase(Ease.Linear);
            
            CharacterModel.OnComboStateEnter();
            // wait for animation complete
            float duration  = await GetAnimationDuration("combo", token);
            await UniTask.Delay((int) (duration * 1000), cancellationToken: token);
            
            CellManager.Instance.NormalAllCell();
        }


        IsReadyCombo = false;
        if (IsLeader)
        {
            SetHide(true);
            await UniTask.WaitUntil(() => TeamManager.Instance.Characters.All(c => !c.IsReadyCombo),
                cancellationToken: token);
            SetHide(false);
        }
        StateMachine.RequestTransition(CharacterIdleState.Instance);
        
    }

    public async UniTask OnComboStateExit(CancellationToken token)
    {

    }
}