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
    
    public void InitTeam()
    {
        Characters.Clear();
        Cell StatrCell = CellManager.Instance.GetCell(StartPosition.x, StartPosition.y);
        CharacterConfigs.ForEach((cf, i) =>
        {
            Character character = Instantiate(CharacterPrefab, StatrCell.Transform.position, Quaternion.identity, Transform);
            character.InitConfig(cf, i, StatrCell);
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
        
        GameManager.Instance.SetGameState(GameManager.GameState.PlayerTurn);
    }
}
