using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using Sirenix.Utilities;
using TW.Utility.CustomType;
using TW.Utility.Extension;

[CreateAssetMenu(fileName = "GachaGlobalConfig", menuName = "GlobalConfigs/GachaGlobalConfig")]
[GlobalConfig("Assets/Resources/GlobalConfig/")]
public class GachaGlobalConfig : GlobalConfig<GachaGlobalConfig>
{
    [field: SerializeField] public CharacterConfig[] CharacterConfigs {get; private set;}
    [field: SerializeField] public Probability<int> GachaRate {get; private set;}
    [ShowInInspector] public int Star3Count => CharacterConfigs.Count(cf => cf.CharacterStar == 3);
    [ShowInInspector] public int Star4Count => CharacterConfigs.Count(cf => cf.CharacterStar == 4);
    [ShowInInspector] public int Star5Count => CharacterConfigs.Count(cf => cf.CharacterStar == 5);
    public CharacterConfig GetRandomCharacterConfig()
    {
        int randomStar = GachaRate.GetRandomItem();
        Debug.Log(randomStar);
        return CharacterConfigs.Where(cf => cf.CharacterStar == randomStar).GetRandomElement();
    }
}