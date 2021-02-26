using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace AOTEditor.Tools.SceneWindow
{


    public class SceneWindow : EditorWindow
    {
        SceneContainer scenes;
        Dictionary<string, Button> buttons = new Dictionary<string, Button>();

        private void OnEnable()
        {
            minSize = new Vector2(300, 300);

            scenes = Resources.Load<SceneContainer>("scene-container");

            var uxml = Resources.Load<VisualTreeAsset>("scene-window");
            rootVisualElement.Add(uxml.CloneTree());
            rootVisualElement.styleSheets.Add(Resources.Load<StyleSheet>("scenewindow"));

            var container = rootVisualElement.Q("list");
            var search = rootVisualElement.Q<ToolbarSearchField>("search");
            search.RegisterValueChangedCallback(Search);

            if (scenes)
                foreach (var scene in scenes.scenes)
                {
                    var btn = new Button();
                    btn.AddToClassList("scene-btn");
                    btn.text = scene.name;
                    btn.tooltip = $"Load Scene: \"{scene.name}\"";
                    buttons.Add(scene.name, btn);
                    if (scene.screenshot)
                    {
                        btn.style.backgroundImage = new StyleBackground(scene.screenshot);
                    }
                    btn.clicked += () =>
                    {
                        if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                        {
                            EditorSceneManager.OpenScene(AssetDatabase.GetAssetPath(scene.scene));
                        }
                    };
                    container.Add(btn);

                }
            else
            {
                var lbl = (new Label("You must create a Scene Container first! Go to: \n\"Assets/Create/Scene Overview/Create Container\"\n Make sure you add it to a Resources Folder."));
                lbl.AddToClassList("msg");

                container.Add(lbl);
            }
        }

        void Search(ChangeEvent<string> searchEvent)
        {
            if(searchEvent.newValue != searchEvent.previousValue)
            {
                bool isNull = string.IsNullOrEmpty(searchEvent.newValue);

                foreach(var key in buttons.Keys)
                {
                    var btn = buttons[key];
                    bool show = isNull || key.ToLower().Contains(searchEvent.newValue.ToLower());
                    btn.style.display = new StyleEnum<DisplayStyle>(show ? DisplayStyle.Flex : DisplayStyle.None);
                }
            }
        }


        [MenuItem("Window/Scene Overview")]
        static void Open()
        {
            var win = GetWindow<SceneWindow>();
            win.titleContent.text = "Scene Window";
            win.Show();
        }
    }

}