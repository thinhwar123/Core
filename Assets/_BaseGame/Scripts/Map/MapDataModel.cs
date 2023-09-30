using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using Sirenix.OdinInspector;
using Sirenix.Utilities;

#if UNITY_EDITOR
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using UnityEditor;
#endif

namespace BaseGame
{
    [System.Serializable]
    public class MapDataModel
    {
        public int mapTree;// id tree
        public int mapID;// id map
        public Sprite sprMap;// id map spr
        public List<MapDetailDataModel> listMapDetailModel;// list map
        public List<LevelSlotRequestModel> mapSlotRequests;// list map request
        public string description;// map des show on ui
        public string name;// name map
        public int width, height; //width, height map
        public void CopyFromOther(MapDataModel other)
        {
            mapTree = other.mapTree;
            mapID = other.mapID;
            sprMap = other.sprMap;
            description = other.description;
            name = other.name;
            width = other.width;
            height = other.height;

            if (listMapDetailModel == null)
                listMapDetailModel = new List<MapDetailDataModel>();
            listMapDetailModel.Clear();
            for (int i = 0; i < other.listMapDetailModel.Count; i++)
            {
                MapDetailDataModel mapDetailModel = new MapDetailDataModel();
                mapDetailModel.CopyFromOther(other.listMapDetailModel[i]);
                listMapDetailModel.Add(mapDetailModel);
            }
            // slot request
            if (mapSlotRequests == null)
                mapSlotRequests = new List<LevelSlotRequestModel>();
            mapSlotRequests.Clear();
            for (int i = 0; i < other.mapSlotRequests.Count; i++)
            {
                LevelSlotRequestModel mapDetailData = new LevelSlotRequestModel();
                mapDetailData.CopyFromOther(other.mapSlotRequests[i]);
                mapSlotRequests.Add(mapDetailData);
            }
        }
    }
    [System.Serializable]
    public class LevelSlotRequestModel
    {
        public int levelTree;
        public int levelID;
        public void CopyFromOther(LevelSlotRequestModel other)
        {
            levelTree = other.levelTree;
            levelID = other.levelID;
        }
    }
    [System.Serializable]
    public class MapDetailDataModel
    {
        public List<EnemyConfig> listEnemy;
        [SerializeField, MapArea] public MapArea posEnemy;

        public void CopyFromOther(MapDetailDataModel other)
        {
            posEnemy.CopyFromOther(other.posEnemy);
            if (listEnemy == null){
                listEnemy = new List<EnemyConfig>();
            }
            listEnemy.Clear();
            for(int i = 0; i < other.listEnemy.Count; i++){
                EnemyConfig enemy = new EnemyConfig();
                enemy.CopyFromOther(other.listEnemy[i]);
                listEnemy.Add(enemy);
            }
        }
    }
    [System.Serializable]
    public class EnemyConfig
    {
        public int enemyID;
        public int hp;
        public int atk;
        public int width, height;
        public void CopyFromOther(EnemyConfig other)
        {
            enemyID = other.enemyID;
            hp = other.hp;
            atk = other.atk;
            width = other.width;
            height = other.height;
        }
    }

    [System.Serializable]
    public class MapArea
    {
        [field: SerializeField] public int[] AreaPosition { get; set; } = new int[100];
        public void CopyFromOther(MapArea other)
        {
            if (AreaPosition == null){
                AreaPosition = new int[100];
            }
            for (int i = 0; i < other.AreaPosition.Length;i++){
                AreaPosition[i] = other.AreaPosition[i];
            }
        }
    }

    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public sealed class MapAreaAttribute : Attribute
    {

    }

#if UNITY_EDITOR
    public sealed class ChapterMapAttributeDrawer : OdinAttributeDrawer<MapAreaAttribute, MapArea>
    {
        private enum Tile
        {
            Empty = 0,
            Blocked = 1,
            Select = 2,
        }
        private readonly Color[] TileColors = new Color[3]
        {
        new Color(0.3f, 0.3f, 0.3f),		// 0
        new Color(1.0f, 1.0f, 1.0f),		// 1
        new Color(0.16f, 1.0f, 1.0f),       // 2

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
            tileSize = 20;
            row = 10;
            col = 10;

            tiles = new Tile[row, col];

            isDrawing = false;

            oldColor = GUI.backgroundColor;
        }

        protected override void DrawPropertyLayout(GUIContent label)
        {
            Rect rect = EditorGUILayout.GetControlRect();
            EditorGUI.LabelField(rect.AlignLeft(rect.width - 100 - 4), "Chapter Map");

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
                    switch (index)
                    {
                        case 0:
                            tiles[i, j] = Tile.Empty;
                            this.ValueEntry.SmartValue.AreaPosition[i * col + j] = 0;
                            break;
                        case 1:
                            tiles[i, j] = Tile.Blocked;
                            this.ValueEntry.SmartValue.AreaPosition[i * col + j] = 1;
                            break;
                        case 2:
                            tiles[i, j] = Tile.Select;
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
                                    tiles[i, j] = Tile.Blocked;
                                    this.ValueEntry.SmartValue.AreaPosition[i * col + j] = 1;
                                    break;
                                case 1:
                                    tiles[i, j] = Tile.Select;
                                    this.ValueEntry.SmartValue.AreaPosition[i * col + j] = 2;
                                    break;
                                case 2:
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

}
