using UnityEngine;
using UnityEditor;
using UnityEditor.Overlays;
using UnityEditor.Toolbars;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;

namespace Synith
{
    /// <summary>
    /// SceneSelectionOverlay
    /// </summary>
    [Overlay(typeof(SceneView), "Scene Selection")]
    [Icon(ICON_PATH)]
    public class SceneSelectionOverlay : ToolbarOverlay
    {
        const string ICON_PATH = "Assets/Editor/Icons/btn_icon_album.png";

        SceneSelectionOverlay() : base(SceneDropdownToggle.ID) { }

        [EditorToolbarElement(ID, typeof(SceneView))]
        class SceneDropdownToggle : EditorToolbarDropdownToggle, IAccessContainerWindow
        {
            public EditorWindow containerWindow { get; set; }

            public const string ID = "SceneSelectionOverlay/SceneDropdownToggle";

            SceneDropdownToggle()
            {
                text = "Scenes";
                tooltip = "Select a scene to load";
                icon = AssetDatabase.LoadAssetAtPath<Texture2D>(ICON_PATH);

                dropdownClicked += ShowSceneMenu;

            }

            void ShowSceneMenu()
            {
                GenericMenu menu = new();

                Scene currentScene = SceneManager.GetActiveScene();

                // ignore these names
                string[] ignoredScenes = { "Basic", "Standard", "BuildTestScene" };

                // Use this for exclusively selecting the scenes that are in the build.
                // EditorBuildSettingsScene[] buildScenes = EditorBuildSettings.scenes;

                string[] sceneGuids = AssetDatabase.FindAssets("t:scene", null);

                for (int i = 0; i < sceneGuids.Length; i++)
                {
                    string path = AssetDatabase.GUIDToAssetPath(sceneGuids[i]);

                    string name = Path.GetFileNameWithoutExtension(path);

                    bool isIgnored = false;
                    foreach (string ignoredScene in ignoredScenes)
                    {
                        if (name == ignoredScene)
                            isIgnored = true;
                    }
                    // don't list ignored scenes
                    if (isIgnored) continue;

                    menu.AddItem(new(name), string.Compare(currentScene.name, name) == 0, () => OpenScene(currentScene, path));
                }

                menu.ShowAsContext();
            }

            void OpenScene(Scene currentScene, string path)
            {
                if (currentScene.isDirty)
                {
                    if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                    {
                        EditorSceneManager.OpenScene(path);
                    }
                }
                else
                {
                    EditorSceneManager.OpenScene(path);
                }
            }
        }
    }
}
