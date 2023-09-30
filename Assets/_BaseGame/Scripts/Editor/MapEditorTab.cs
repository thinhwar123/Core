#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace BaseGame.Editor
{
    public class MapEditorTab : EditorTab
    {
        private MapDataModel currentItems;
        private ReorderableList listItemOrder;
        private Vector2 scrollPos;
        public const string MAP_ASSET_DATA = "Assets/Data/MapAssetData.asset";
        private MapAssetData mapAssetData;
        // detail reasearch
        private MapDetailDataModel curMapDetailModel;
        // Slot Unlock
        private LevelSlotRequestModel mapSlotRequestModel;
        private ReorderableList listSlotRequest;
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="editor">The parent editor.</param>
        public MapEditorTab(BaseGameEditor editor) : base(editor)
        {
            mapAssetData = AssetDatabase.LoadAssetAtPath<MapAssetData>(MAP_ASSET_DATA);
            CreateItemList();
        }
        /// <summary>
        /// Draw Item
        /// </summary>
        public override void Draw()
        {
            base.Draw();
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
            var oldLabelWidth = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = 200;
            GUILayout.Space(15);
            DrawMenu();
            GUILayout.Space(15);
            GUILayout.BeginVertical();
            DrawListItemsList();
            GUILayout.EndVertical();
            GUILayout.Space(15);

            EditorGUIUtility.labelWidth = oldLabelWidth;
            EditorGUILayout.EndScrollView();
        }
        /// <summary>
        /// Draw the menu
        /// </summary>
        void DrawMenu()
        {
            GUILayout.BeginHorizontal(GUILayout.Width(350));
            const string helpText = "Thêm mới chỉnh sửa Map ";
            EditorGUILayout.HelpBox(helpText, MessageType.Info);
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Save", GUILayout.Width(100), GUILayout.Height(60)))
            {
                SaveGameConfiguration(Application.dataPath + "/Resources/Items");
            }
            if (GUILayout.Button("Check Data", GUILayout.Width(100), GUILayout.Height(60)))
            {
                //CheckData();
            }
            if (GUILayout.Button("Dupticate", GUILayout.Width(100), GUILayout.Height(60)))
            {
                if (currentItems != null)
                {
                    var dupticateModel = new MapDataModel();
                    dupticateModel.CopyFromOther(currentItems);
                    mapAssetData.listMapDataModel.Add(dupticateModel);

                }
            }

            GUILayout.Space(20);


            GUILayout.EndHorizontal();
        }
        /// <summary>
        /// Creates the list of in-app purchase items.
        /// </summary>
        private void DrawListItemsList()
        {
            EditorGUILayout.LabelField("Danh sách Map", EditorStyles.boldLabel);
            GUILayout.BeginHorizontal(GUILayout.Width(300));
            const string helpText =
                "Map Data.";
            EditorGUILayout.HelpBox(helpText, MessageType.Info);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.BeginVertical(GUILayout.Width(350));
            if (listItemOrder != null)
            {
                listItemOrder.DoLayoutList();
            }
            GUILayout.EndVertical();

            if (currentItems != null)
            {
                DrawItems(currentItems);


            }

            GUILayout.EndHorizontal();
        }
        /// Draws the specified in-app purchase item.
        /// </summary>
        /// <param name="item">The in-app purchase item to draw.</param>
        private void DrawItems(MapDataModel item)
        {
            var oldLabelWidth = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = 150;
            GUILayout.BeginVertical();
            GUILayout.BeginHorizontal();
            oldLabelWidth = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = 200;
            GUILayout.BeginVertical();

            GUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Tree Floor");
            item.mapTree = EditorGUILayout.IntField(item.mapTree, GUILayout.Width(250));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Map ID");
            item.mapID = EditorGUILayout.IntField(item.mapID, GUILayout.Width(250));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Name");
            item.name = EditorGUILayout.TextField(item.name, GUILayout.Width(250));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Des");
            item.description = EditorGUILayout.TextArea(item.description, GUILayout.Width(250), GUILayout.Height(100));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Icon");
            item.sprMap = (Sprite)EditorGUILayout.ObjectField(item.sprMap, typeof(Sprite), true, GUILayout.Width(100), GUILayout.Height(100));
            GUILayout.EndHorizontal();


            GUILayout.EndVertical();

            GUILayout.EndHorizontal();
            // list slot unlock
            {
                GUILayout.Space(50);
                EditorGUILayout.LabelField("Danh sách Slot Request", EditorStyles.boldLabel);
                GUILayout.BeginHorizontal(GUILayout.Width(300));
                const string helpText =
                    "Các map ID cần để unlock.";
                EditorGUILayout.HelpBox(helpText, MessageType.Info);
                GUILayout.EndHorizontal();

                if (currentItems.mapSlotRequests == null)
                    currentItems.mapSlotRequests = new List<LevelSlotRequestModel>();
                if (listSlotRequest == null)
                    CreateMapSlotList();
                GUILayout.BeginHorizontal();
                GUILayout.BeginVertical(GUILayout.Width(450));
                if (listSlotRequest != null)
                {
                    listSlotRequest.DoLayoutList();
                }
                GUILayout.EndVertical();

                if (mapSlotRequestModel != null)
                {
                    GUILayout.BeginVertical();

                    GUILayout.BeginHorizontal();
                    EditorGUILayout.PrefixLabel("Tree Floor");
                    mapSlotRequestModel.levelTree = EditorGUILayout.IntField(mapSlotRequestModel.levelTree, GUILayout.Width(250));
                    GUILayout.EndHorizontal();


                    GUILayout.BeginHorizontal();
                    EditorGUILayout.PrefixLabel("Map ID");
                    mapSlotRequestModel.levelID = EditorGUILayout.IntField(mapSlotRequestModel.levelID, GUILayout.Width(250));
                    GUILayout.EndHorizontal();


                    GUILayout.EndVertical();

                }
                GUILayout.EndHorizontal();

            }
            // List level
            {
                GUILayout.Space(50);
                
                GUILayout.BeginHorizontal(GUILayout.Width(300));
                const string helpText =
                    "Mỗi map có thể có 1 hoặc nhiều wave";
                EditorGUILayout.HelpBox(helpText, MessageType.Info);
                GUILayout.EndHorizontal();

                if (currentItems.listMapDetailModel == null)
                    currentItems.listMapDetailModel = new List<MapDetailDataModel>();
                GUILayout.BeginHorizontal();

                if (curMapDetailModel != null)
                {
                    GUILayout.BeginVertical();

                    // GUILayout.BeginHorizontal();
                    // EditorGUILayout.PrefixLabel("Start Value");
                    // curMapDetailModel.startLevelValue = EditorGUILayout.FloatField(curMapDetailModel.startLevelValue, GUILayout.Width(250));
                    // GUILayout.EndHorizontal();

                    // GUILayout.BeginHorizontal();
                    // EditorGUILayout.PrefixLabel("Level Up Value");
                    // curMapDetailModel.levelUpValue = EditorGUILayout.FloatField(curMapDetailModel.levelUpValue, GUILayout.Width(250));
                    // GUILayout.EndHorizontal();

                    GUILayout.EndVertical();

                }
                GUILayout.EndHorizontal();
            }

            GUILayout.Space(100);
            GUILayout.EndVertical();
            EditorGUIUtility.labelWidth = oldLabelWidth;
        }

        public void SaveGameConfiguration(string path)
        {
#if UNITY_EDITOR
            if (CheckItemId())
                return;
            //var fullPath = path + "/GameItemsWeapon.json";
            //SaveJsonFile(fullPath, _lisItemModel);
            //EditorPrefs.SetString(editorItemsKnightPrefsName, fullPath);
            //AssetDatabase.Refresh();
            if (mapAssetData != null)
            {
                EditorUtility.SetDirty(mapAssetData);
            }
            else
            {
                EditorUtility.DisplayDialog("Warning", "Nothing to save!!!", "Got it");
            }
            AssetDatabase.SaveAssets();
#endif
        }
        /// <summary>
        /// check if ID item same other item
        /// </summary>
        /// <returns></returns>
        bool CheckItemId()
        {
            for (int i = 0; i < mapAssetData.listMapDataModel.Count; i++)
            {
                var checItemd = mapAssetData.listMapDataModel[i];

                for (int j = i + 1; j < mapAssetData.listMapDataModel.Count; j++)
                {
                    if (checItemd.mapTree == mapAssetData.listMapDataModel[j].mapTree &&
                        checItemd.mapID == mapAssetData.listMapDataModel[j].mapID)
                    {
                        EditorUtility.DisplayDialog("Warning!", "Có 2 Map có cùng Tree : " + checItemd.mapTree.ToString() + " - ID: " + checItemd.mapID.ToString(), "Đóng");
                        return true;
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// Creates the list of items.
        /// </summary>
        private void CreateItemList()
        {
            listItemOrder = SetupReorderableList("Tree Floor - ID - Max Level - Type", mapAssetData.listMapDataModel,
                ref currentItems, (rect, x) =>
                {
                    EditorGUI.LabelField(new Rect(rect.x, rect.y, 350, EditorGUIUtility.singleLineHeight), x.mapTree.ToString() + " - " + x.mapID );
                },
                (x) =>
                {
                    currentItems = x;
                    listSlotRequest = null;
                    curMapDetailModel = null;
                    mapSlotRequestModel = null;
                },
                () =>
                {
                    var newItem = new MapDataModel();
                    mapAssetData.listMapDataModel.Add(newItem);
                },
                (x) =>
                {
                    currentItems = null;
                    listSlotRequest = null;
                    curMapDetailModel = null;
                    mapSlotRequestModel = null;
                });
            listItemOrder.onRemoveCallback = l =>
            {
                if (!EditorUtility.DisplayDialog("Warning!", "Are you sure you want to delete this item?", "Yes", "No")
                )
                {
                    return;
                }
                ReorderableList.defaultBehaviours.DoRemoveButton(l);
                currentItems = null;
                listSlotRequest = null;
            };
        }


        private void CreateMapSlotList()
        {
            listSlotRequest = SetupReorderableList("Floor - ID ", currentItems.mapSlotRequests,
                ref mapSlotRequestModel, (rect, x) =>
                {
                    EditorGUI.LabelField(new Rect(rect.x, rect.y, 350, EditorGUIUtility.singleLineHeight), x.levelTree.ToString() + " - " + x.levelID.ToString());
                },
                (x) =>
                {
                    mapSlotRequestModel = x;
                },
                () =>
                {
                    var newItem = new LevelSlotRequestModel();
                    currentItems.mapSlotRequests.Add(newItem);
                },
                (x) =>
                {
                    mapSlotRequestModel = null;
                });
            listSlotRequest.onRemoveCallback = l =>
            {
                if (!EditorUtility.DisplayDialog("Warning!", "Are you sure you want to delete this item?", "Yes", "No")
                )
                {
                    return;
                }
                ReorderableList.defaultBehaviours.DoRemoveButton(l);
                mapSlotRequestModel = null;
            };
        }

    }
}
#endif