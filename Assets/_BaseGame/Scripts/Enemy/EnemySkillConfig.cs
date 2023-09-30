using UnityEngine;
using System;
using Sirenix.OdinInspector;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "SkillConfig", menuName = "ScriptableObjects/EnemySkillConfig")]
public class EnemySkillConfig : ScriptableObject
{
    [field: SerializeField, HorizontalGroup("Icon", 85), HideLabel, PreviewField(80)]
    public Sprite Skillicon { get; private set; }
    [field: SerializeField, VerticalGroup("Icon/Stat"), OnValueChanged(nameof(OnSkillNameChange))] public string EnemyName { get; private set; }
    [field: SerializeField, VerticalGroup("Icon/Stat"), OnValueChanged(nameof(OnSkillNameChange))] public string SkillName { get; private set; }
    [field: SerializeField, VerticalGroup("Icon/Stat")] public int SkillRange { get; private set; }
    [field: SerializeField, VerticalGroup("Icon/Stat")] public int ComboCooldown { get; private set; }
    [field: SerializeField, VerticalGroup("Icon/Stat")] public ChainComboConfig ComboConfig { get; private set; } 
    [field: SerializeField, VerticalGroup("Icon/Stat")] public GameObject skillEffect;

    private void OnSkillNameChange()
    {
#if UNITY_EDITOR
        string assetPath = AssetDatabase.GetAssetPath(this.GetInstanceID());
        AssetDatabase.RenameAsset(assetPath, $"SkillConfig-{EnemyName}- {SkillName}");
#endif
    }
}
