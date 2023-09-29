using System.Collections.Generic;
using UnityEngine;
using Sirenix.Utilities;

[CreateAssetMenu(fileName = "EnemyGlobalConfig", menuName = "GlobalConfigs/EnemyGlobalConfig")]
[GlobalConfig("Assets/Resources/GlobalConfig/")]
public class EnemyGlobalConfig : GlobalConfig<EnemyGlobalConfig>
{
    [field: SerializeField] public List<EnemyConfig> EnemyConfigs { get; private set; } = new List<EnemyConfig>();

    public EnemyConfig GetEnemyConfig(int id)
    {
        return EnemyConfigs.Find(x => x.EnemyId == id);
    }
}