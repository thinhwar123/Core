using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using TW.Utility.DesignPattern;
using TW.Utility.Extension;
using UnityEngine;

public class EnemyManager : Singleton<EnemyManager>
{
    [field: SerializeField] public Enemy EnemyPrefab {get; private set;}
    [field: SerializeField] public List<Enemy> Enemies {get; private set;} = new List<Enemy>();

    
    public void InitDemoEnemy()
    {
        Enemies.Clear();
        for (int i = 0; i < 2; i++)
        {
            Cell randomCell = CellManager.Instance.CellList
                .Where(c => c.CurrentState != Cell.State.Hide && !c.IsEnemyCell && !c.IsCharacterCell)
                .GetRandomElement();
            Enemy enemy = CreateEnemy(randomCell);
            Enemies.Add(enemy);
        }
    }

    public Enemy CreateEnemy(Cell cell)
    {
        Cell startCell = cell;
        Enemy enemy = Instantiate(EnemyPrefab, startCell.Transform.position, Quaternion.identity, Transform);
        enemy.InitConfig(EnemyGlobalConfig.Instance.GetEnemyConfig(0), startCell);
        startCell.RegisterOwner(enemy);
        return enemy;
    }
        
    public async UniTask PlayEnemyTurn()
    {
        for (var i = 0; i < Enemies.Count; i++)
        {
            Enemies[i].PlayTurn();
            await UniTask.WaitUntil(() => !Enemies[i].IsTakeTurn);
        }
        GameManager.Instance.SetGameState(GameManager.GameState.PlayerTurn);
    }
}
