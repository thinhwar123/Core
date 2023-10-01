using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using TW.Utility.DesignPattern;
using TW.Utility.Extension;
using UnityEngine;
using Sirenix.Utilities;

public class EnemyManager : Singleton<EnemyManager>
{
    [field: SerializeField] public Enemy EnemyPrefab {get; private set;}
    [field: SerializeField] public List<Enemy> Enemies {get; private set;} = new List<Enemy>();
    
    public void InitEnemy(List<BaseGame.EnemyConfig> enemyConfigs, List<Cell> cellPositions)
    {
        Enemies.Clear();
        enemyConfigs.ForEach((cf, i) =>
        {
            Enemy enemy = CreateEnemy(cellPositions[i], cf.enemyID, cf.hp, cf.atk);
            Enemies.Add(enemy);
        });
    }
    public Enemy CreateEnemy(Cell cell, int index )
    {
        Cell startCell = cell;
        Enemy enemy = Instantiate(EnemyPrefab, startCell.Transform.position, Quaternion.identity, Transform);
        enemy.InitConfig(EnemyGlobalConfig.Instance.GetEnemyConfig(index), startCell);
        startCell.RegisterOwner(enemy);
        return enemy;
    }
    
    public Enemy CreateEnemy(Cell cell, int index, int defaultHitPoint, int defaultAttackDamage)
    {
        Cell startCell = cell;
        Enemy enemy = Instantiate(EnemyPrefab, startCell.Transform.position, Quaternion.identity, Transform);
        enemy.InitConfig(EnemyGlobalConfig.Instance.GetEnemyConfig(index), startCell);
        enemy.OverrideConfig(defaultHitPoint, defaultAttackDamage);

        return enemy;
    }
        
    public async UniTask PlayEnemyTurn()
    {
        List<Enemy> newEnemies = Enemies.Where(e => !e.IsDeath).ToList();
        for (var i = 0; i < newEnemies.Count; i++)
        {
            Enemies[i].PlayTurn();
            await UniTask.WaitUntil(() => (i>-1&&i<Enemies.Count?!Enemies[i].IsTakeTurn:true));
        }
        GameManager.Instance.SetGameState(GameManager.GameState.PlayerTurn);
    }
    public void ClearEnemy()
    {
        Enemies.ForEach(x => Destroy(x.gameObject));
        Enemies.Clear();
    }
}
