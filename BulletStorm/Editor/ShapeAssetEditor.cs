using System;
using BulletStorm.Emission;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BulletStorm.Editor
{
    [CustomEditor(typeof(ShapeAsset))]
    public class ShapeAssetEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (PreviewScene.Instance.IsActive)
            {
                if (GUILayout.Button("Close preview")) PreviewScene.Instance.Close();
            }
            else
            {
                if (GUILayout.Button("Open preview"))
                {
                    PreviewScene.Instance.Open();
                    Selection.activeObject = target;
                    GUIUtility.ExitGUI();
                }
            }
        }

        private class PreviewScene
        {
            public static PreviewScene Instance => Singleton.Value;
            public bool IsActive => SceneManager.GetActiveScene() == scene;
            
            private string previousScene;
            private Scene scene;

            private static readonly Lazy<PreviewScene> Singleton = new Lazy<PreviewScene>(() => new PreviewScene());
            
            private PreviewScene()
            {}

            public void Open()
            {
                var currentScene = SceneManager.GetActiveScene();
                if (currentScene == scene) return;
                previousScene = currentScene.path;
                if (!scene.IsValid()) scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
                SceneManager.SetActiveScene(scene);
            }

            public void Close()
            {
                if (IsActive) EditorSceneManager.OpenScene(previousScene);
            }
        }
    }
}