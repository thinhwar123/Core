using UnityEngine;
using Sirenix.OdinInspector;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "EnemyConfig", menuName = "ScriptableObjects/EnemyConfig")]
public class EnemyConfig : ScriptableObject
{
    [field: SerializeField, HorizontalGroup("Icon", 140), HideLabel, PreviewField(135)]
    public Sprite EnemyPreview { get; private set; }

    [field: SerializeField, VerticalGroup("Icon/Stat")]
    public int EnemyId { get; private set; }

    [field: SerializeField, VerticalGroup("Icon/Stat"), OnValueChanged(nameof(OnEnemyNameChange))]
    public string EnemyName { get; private set; }

    [field: SerializeField, VerticalGroup("Icon/Stat"), OnValueChanged(nameof(OnEnemyNameChange))]
    public string EnemyNickName { get; private set; }

    [field: SerializeField, VerticalGroup("Icon/Stat")]
    public EnemyModel EnemyModel { get; private set; }

    [field: SerializeField, VerticalGroup("Icon/Stat")]
    public Sprite EnemyIcon { get; private set; }

    [field: SerializeField, FoldoutGroup("Profile"), HideLabel, TextArea]
    public string Profile { get; private set; }

    [field: ProgressBar(0, 250, 1, 0.1f, 0.1f)]
    [field: SerializeField] public int AttackDamage { get; private set; }
    [field: ProgressBar(0, 5000, 0.1f, 1f, 0.1f)]
    [field: SerializeField] public int HitPoint { get; private set; }
    [field: SerializeField] public int MoveStep { get; private set; }
    [field: SerializeField, InlineEditor] public EnemySkillConfig SkillConfig { get; private set; }

    #region Editor
    private void OnEnemyNameChange()
    {
#if UNITY_EDITOR
        string assetPath = AssetDatabase.GetAssetPath(this.GetInstanceID());
        AssetDatabase.RenameAsset(assetPath, $"EnemeyConfig-{EnemyName}-{EnemyNickName}");
#endif
    }

    #endregion
}
