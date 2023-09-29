
using System.Collections.Generic;
using TW.Utility.DesignPattern;
using Sirenix.Utilities;
using UnityEngine;

public class TeamManager : Singleton<TeamManager>
{
    [field: SerializeField] public List<CharacterConfig> CharacterConfigs { get; set; } = new List<CharacterConfig>();
    [field: SerializeField] public Character CharacterPrefab {get; private set;}
    
    
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
            if (i == 0)
            {
                StatrCell.RegisterOwner(character);
            }
        });
    }
    public void TeamMoveFollowPath(List<Cell> path)
    {
        Characters.ForEach(x => x.MoveFollowPath(path));
    }
}
