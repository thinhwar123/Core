
using UnityEngine;

public class Enemy : Entity
{
    [field: SerializeField] public Transform EnemyModelContainer {get; private set;}
    [field: SerializeField] public EnemyConfig EnemyConfig {get; private set;} 
    [field: SerializeField] public Cell CurrentCell {get; private set;}
    [field: SerializeField] public int HitPoint {get; private set;}
    [field: SerializeField] public int AttackDamage {get; private set;}
    private UIHealthBar UIHealthBar { get; set; }
    public void InitConfig(EnemyConfig enemyConfig, Cell startCell)
    {
        EnemyConfig = enemyConfig;
        CurrentCell = startCell;

        HitPoint = EnemyConfig.HitPoint;
        AttackDamage = EnemyConfig.AttackDamage;
        
        UIHealthBar = GameManager.Instance.CreateUIHealthBar();
        UIHealthBar.SetupHealthBar(EAttribute.White, Transform);
        UIHealthBar.UpdateValue(1);
    }
    public override void OnDamage(int damage)
    {
        base.OnDamage(damage);
        HitPoint -= damage;
        if (HitPoint < 0) HitPoint = 0;
        UIHealthBar.UpdateValue(HitPoint / (float) EnemyConfig.HitPoint);
    }
    

}