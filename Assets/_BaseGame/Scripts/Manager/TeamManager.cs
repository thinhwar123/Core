using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Sirenix.Utilities;
using Unity.VisualScripting;
using UnityEngine;

public class TeamManager : TW.Utility.DesignPattern.Singleton<TeamManager>
{
    [field: SerializeField] public List<CharacterConfig> CharacterConfigs { get; set; } = new List<CharacterConfig>();
    [field: SerializeField] public Character CharacterPrefab {get; private set;}
    [field: SerializeField] public float DelayMoveTime {get; private set;}
    
    [field: SerializeField] public Vector2Int StartPosition {get; private set;}
    [field: SerializeField] public List<Character> Characters { get; private set; } = new List<Character>();
    public Character Leader => Characters[0];

    public void InitTeam()
    {
        Characters.Clear();
        Cell startCell = CellManager.Instance.GetCell(StartPosition.x, StartPosition.y);
        CharacterConfigs.ForEach((cf, i) =>
        {
            Character character = Instantiate(CharacterPrefab, startCell.Transform.position, Quaternion.identity, Transform);
            character.InitConfig(cf, i, startCell);
            Characters.Add(character);
        });
    }
    public async void TeamMoveFollowPath(List<Cell> path)
    {
        GameManager.Instance.SetGameState(GameManager.GameState.CalculateTurn);
        foreach (Character c in Characters)
        {
            c.MoveFollowPath(path);
            await UniTask.Delay((int)(DelayMoveTime * 1000));
        }
        // wait for all character change to idle state
        await UniTask.WaitUntil(() => Characters.All(x => x.IsIdle));
        await CellManager.Instance.RecoverConsumedCell();
        
        if (EnemyManager.Instance.Enemies.All(e => e.IsDeath))
        {
            GameManager.Instance.OnWinGame();
            return;
        }

        
        GameManager.Instance.SetGameState(GameManager.GameState.EnemyTurn);
        await EnemyManager.Instance.PlayEnemyTurn();
        if (Leader.IsDeath)
        {
            GameManager.Instance.OnLoseGame();
            return;
        }

    }
    public void ClearTeam()
    {
        Characters.ForEach(x => Destroy(x.gameObject));
        Characters.Clear();
    }
}
