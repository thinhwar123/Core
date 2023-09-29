using Sirenix.OdinInspector;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterConfig", menuName = "ScriptableObjects/CharacterConfig")]
public class CharacterConfig : ScriptableObject
{
    [field: SerializeField, HorizontalGroup("Icon", 140), HideLabel, PreviewField(135)]
    public Sprite CharacterPreview { get; private set; }

    [field: SerializeField, VerticalGroup("Icon/Stat"), OnValueChanged(nameof(OnCharacterNameChange))]
    public string CharacterName { get; private set; }
    [field: SerializeField, VerticalGroup("Icon/Stat"), OnValueChanged(nameof(OnCharacterNameChange))]
    public string NickName { get; private set; }
    
    [field: SerializeField, VerticalGroup("Icon/Stat"), ProgressBar(0, 5, 1, 0.5f, 0, Segmented = true)]
    public int CharacterStar { get; private set; } = 3;

    [field: SerializeField, VerticalGroup("Icon/Stat"), OnValueChanged(nameof(OnCharacterNameChange))]
    public EAttribute CharacterAttribute { get; private set; }

    [field: SerializeField, VerticalGroup("Icon/Stat")]
    public CharacterModel CharacterModel { get; private set; }
    
    [field: SerializeField, VerticalGroup("Icon/Stat")]
    public Sprite CharacterIcon { get; private set; }
    [field: SerializeField, FoldoutGroup("Profile"), HideLabel, TextArea]
    public string Profile { get; private set; }
    
    
    [field: ProgressBar(0, 250, 1, 0.1f, 0.1f)]
    [field: SerializeField] public int AttackDamage { get; private set; }
    [field: ProgressBar(0, 500, 0.1f, 1f, 0.1f)]
    [field: SerializeField] public int HitPoint { get; private set; }
    
    [field: SerializeField] public Character.ChainCombo[] CharacterChainCombo {get; private set;}
    [field: SerializeField, InlineEditor] public SkillConfig SkillConfig { get; private set; }

    #region Editor

    private void OnCharacterNameChange()
    {
#if UNITY_EDITOR
        string assetPath = AssetDatabase.GetAssetPath(this.GetInstanceID());
        AssetDatabase.RenameAsset(assetPath, $"CharacterConfig-{CharacterName}, {NickName} - {CharacterAttribute}");
#endif
    }
    

    #endregion
}