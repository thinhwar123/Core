using System.Collections;
using System.Collections.Generic;
using TW.Utility.DesignPattern;
using UnityEngine;

public class EnemyManager : Singleton<EnemyManager>
{
    [field: SerializeField] public Enemy EnemyPrefab {get; private set;}
    [field: SerializeField] public List<Enemy> Enemies {get; private set;} = new List<Enemy>();


    public void InitDemoEnemy()
    {
        Enemies.Clear();
        Cell startCell = CellManager.Instance.GetCell(5, 5);
        Enemy enemy = Instantiate(EnemyPrefab, startCell.Transform.position, Quaternion.identity, Transform);
        enemy.InitConfig(EnemyGlobalConfig.Instance.GetEnemyConfig(0), startCell);
        startCell.RegisterOwner(enemy);
        Enemies.Add(enemy);
    }
}
