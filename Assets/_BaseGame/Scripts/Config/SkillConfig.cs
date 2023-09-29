using System;
using Sirenix.OdinInspector;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

[CreateAssetMenu(fileName = "SkillConfig", menuName = "ScriptableObjects/SkillConfig")]
public class SkillConfig : ScriptableObject
{
    [field: SerializeField, HorizontalGroup("Icon", 85), HideLabel, PreviewField(80)] 
    public Sprite SkillIcon { get; private set; }
    [field: SerializeField, VerticalGroup("Icon/Stat")] public string SkillName { get; private set; }
    [field: SerializeField, VerticalGroup("Icon/Stat"), OnValueChanged(nameof(OnSkillNameChange))] public ESkillType SkillType { get; private set; }
    [field: SerializeField, VerticalGroup("Icon/Stat"), OnValueChanged(nameof(OnSkillNameChange))] public EAttribute SkillAttribute { get; private set; }
    [field: SerializeField, VerticalGroup("Icon/Stat"), SuffixLabel("Turn ", true)] public int SkillCooldown { get; private set; }
    [field: SerializeField, VerticalGroup("Icon/Stat")] public int SkillValue { get; private set; }

    [field: SerializeField, FoldoutGroup("Description"), HideLabel]
    public string SkillDescriptionFormat { get; private set; } = "";
    
    [ShowInInspector] public string SkillDescription
    {
        get
        {
            try
            {
                return string.Format(SkillDescriptionFormat, SkillAttribute, SkillValue); 
            }
            catch (Exception e)
            {
                return "";
                
            }
        }
    } 
    
    private void OnSkillNameChange()
    {
#if UNITY_EDITOR
        string assetPath = AssetDatabase.GetAssetPath(this.GetInstanceID());
        AssetDatabase.RenameAsset(assetPath, $"SkillConfig-{SkillType}-{SkillAttribute}");
#endif
    }
}