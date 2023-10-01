#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace BaseGame.Editor
{
    public class BaseGameEditor : EditorWindow
    {
        private readonly List<EditorTab> tabs = new List<EditorTab>();
        private int selectedTabIndex = -1;
        private int prevSelectedTabIndex = -1;
        private Vector2 _scrollPos = Vector2.zero;

        /// <summary>
        /// Static initialization of the editor window.
        /// </summary>
        [MenuItem("Tools/Stars/Editor &1", false, 1)]
        private static void Init()
        {
            var window = GetWindow(typeof(BaseGameEditor));
            window.titleContent = new GUIContent("Stars");
        }

        /// <summary>
        /// Unity's OnEnable method.
        /// </summary>
        private void OnEnable()
        {
            tabs.Add(new MapEditorTab(this));
            selectedTabIndex = 0;

        }
        /// <summary>
        /// Unity's OnGUI method.
        /// </summary>
        public virtual void OnGUI()
        {
            _scrollPos = GUILayout.BeginScrollView(_scrollPos);
            {
                selectedTabIndex = GUILayout.Toolbar(selectedTabIndex,
               new[] { "Map"},
               GUILayout.Height(40), GUILayout.Width(Screen.width - 20));
                if (selectedTabIndex >= 0 && selectedTabIndex < tabs.Count)
                {
                    var selectedEditor = tabs[selectedTabIndex];
                    if (selectedTabIndex != prevSelectedTabIndex)
                    {
                        selectedEditor.OnTabSelected();
                        //GUI.FocusControl(null);
                    }
                    selectedEditor.Draw();
                    prevSelectedTabIndex = selectedTabIndex;

                }
            }
            GUILayout.EndScrollView();


        }
    }

}
#endif