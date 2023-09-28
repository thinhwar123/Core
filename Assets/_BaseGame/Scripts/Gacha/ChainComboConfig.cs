using System;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

#if UNITY_EDITOR
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using UnityEditor; 
#endif

[CreateAssetMenu(fileName = "ChainComboConfig", menuName = "ScriptableObjects/ChainComboConfig")]
public class ChainComboConfig : ScriptableObject
{
    [field: SerializeField, OnValueChanged(nameof(OnChainComboNameChange))] public Character.ChainCombo.Type ChangeComboType {get; private set;}
    [field: SerializeField, ComboArea] public ComboArea ComboArea {get; private set;}
    
    private void OnChainComboNameChange()
    {
#if UNITY_EDITOR
        string assetPath = AssetDatabase.GetAssetPath(this.GetInstanceID());
        AssetDatabase.RenameAsset(assetPath, $"ChainComboConfig-{ChangeComboType}");
#endif
    }
}
[System.Serializable]
public class ComboArea
{
    [field: SerializeField] public int[] AreaPosition { get; set; } = new int[49];

}

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
public sealed class ComboAreaAttribute : Attribute
{

}

#if UNITY_EDITOR
public sealed class ComboAreaAttributeDrawer : OdinAttributeDrawer<ComboAreaAttribute, ComboArea>
{
    private enum Tile
    {
        Empty = 0,
        Select = 1,
        Mid = 2,
    }
    private readonly Color[] TileColors = new Color[3]
    {
        new Color(0.3f, 0.3f, 0.3f),		// 0
        new Color(1.0f, 1.0f, 1.0f),		// 1
        new Color(0.16f, 1.0f, 1.0f),		// 2

    };

    private int tileSize;
    private int row;
    private int col;

    private bool isDrawing;

    private Tile[,] tiles;
    private int[] areaPosition;

    private Color oldColor;

    //private List<string> m_LevelResolveList;
    // private int currentSelectIndex;

    protected override void Initialize()
    {
        tileSize = 40;
        row = 7;
        col = 7;

        tiles = new Tile[row, col];

        isDrawing = false;

        oldColor = GUI.backgroundColor;
    }

    protected override void DrawPropertyLayout(GUIContent label)
    {
        Rect rect = EditorGUILayout.GetControlRect();
        EditorGUI.LabelField(rect.AlignLeft(rect.width - 100 - 4), "Combo Area");

        LoadCurrentCase();
        this.ValueEntry.WeakValues.ForceMarkDirty();

        OnDrawCase();

    }

    private void LoadCurrentCase()
    {
        areaPosition = new int[row * col];

        for (int i = 0; i < this.ValueEntry.SmartValue.AreaPosition.Length; i++)
        {
            areaPosition[i] = this.ValueEntry.SmartValue.AreaPosition[i];
        }
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                int index = areaPosition[i * col + j];
                if (i == 3 && j == 3) index = 2;
                switch (index)
                {
                    case 0:
                        tiles[i, j] = Tile.Empty;
                        this.ValueEntry.SmartValue.AreaPosition[i * col + j] = 0;
                        break;
                    case 1:
                        tiles[i, j] = Tile.Select;
                        this.ValueEntry.SmartValue.AreaPosition[i * col + j] = 1;
                        break;
                    case 2:
                        tiles[i, j] = Tile.Mid;
                        this.ValueEntry.SmartValue.AreaPosition[i * col + j] = 2;
                        break;
                }

            }
        }

        isDrawing = true;
    }

    private void SaveCurrentCase()
    {
        isDrawing = false;
    }

    private void OnDrawCase()
    {
        if (!isDrawing) return;

        Rect rect = EditorGUILayout.GetControlRect();

        rect = EditorGUILayout.GetControlRect(false, tileSize * row);
        rect = rect.AlignCenter(tileSize * col);
        rect = rect.AlignMiddle(tileSize * row);

        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                Rect tileRect = rect.SplitGrid(tileSize, tileSize, i * col + j);
                SirenixEditorGUI.DrawBorders(tileRect.SetWidth(tileRect.width + 1).SetHeight(tileRect.height + 1), 1);

                SirenixEditorGUI.DrawSolidRect(new Rect(tileRect.x + 1, tileRect.y + 1, tileRect.width - 1, tileRect.height - 1), TileColors[(int)tiles[i, j]]);
                if (tileRect.Contains(Event.current.mousePosition))
                {
                    SirenixEditorGUI.DrawSolidRect(new Rect(tileRect.x + 1, tileRect.y + 1, tileRect.width - 1, tileRect.height - 1), new Color(0f, 1f, 0f, 0.3f));

                    if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
                    {
                        int index = (int)tiles[i, j];
                        switch (index)
                        {
                            case 0:
                                tiles[i, j] = Tile.Select;
                                this.ValueEntry.SmartValue.AreaPosition[i * col + j] = 1;
                                break;
                            case 1:
                                tiles[i, j] = Tile.Empty;
                                this.ValueEntry.SmartValue.AreaPosition[i * col + j] = 0;
                                break;
                        }
                        Event.current.Use();
                    }
                }
            }
        }

        GUIHelper.RequestRepaint();
    }
    
}
#endif
