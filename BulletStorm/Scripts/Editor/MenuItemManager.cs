using BulletStorm.BulletSystem;
using BulletStorm.Emitters;
using BulletStorm.Util;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace BulletStorm.Editor
{
    internal static class MenuItemManager
    {
        [MenuItem("Assets/Create/BulletStorm/ParticleBulletSystem")]
        public static void CreateParticleBulletSystemPrefab()
        {
            CreatePrefab<ParticleBulletSystem>("NewBulletSystem", GetCurrentAssetDirectory("Assets/Prefabs/BulletStorm"));
        }

        [MenuItem("Assets/Create/BulletStorm/GameObjectBulletSystem")]
        public static void CreateGameObjectBulletSystemPrefab()
        {
            CreatePrefab<GameObjectBulletSystem>("NewBulletSystem", GetCurrentAssetDirectory("Assets/Prefabs/BulletStorm"));
        }

        [MenuItem("GameObject/3D Object/BulletStorm/AutoBulletEmitter")]
        public static void CreateAutoBulletEmitter()
        {
            CreateGameObject<AutoBulletEmitter>();
        }
        
        
        /// <summary>
        /// Create a game object scene.
        /// </summary>
        /// <typeparam name="T">Script type.</typeparam>
        private static void CreateGameObject<T>() where T : MonoBehaviour
        {
            var transforms = Selection.transforms;
            if (transforms.Length == 0)
            {
                _ = new GameObject(typeof(T).Name, typeof(T));
            }
            else
            {
                foreach (var transform in Selection.transforms)
                {
                    
                    var go = new GameObject(typeof(T).Name, typeof(T));
                    go.transform.SetParent(transform, false);
                }
            }
        }
        
        /// <summary>
        /// Creates a prefab and save to assets.
        /// </summary>
        /// <param name="name">Prefab name</param>
        /// <param name="path">Asset folder path</param>
        /// <typeparam name="T">Script type</typeparam>
        private static void CreatePrefab<T>(string name, string path) where T : MonoBehaviour
        {
            var go = EditorUtility.CreateGameObjectWithHideFlags(
                name, HideFlags.DontSaveInEditor & HideFlags.HideInHierarchy, typeof(T));
            var uniquePath = AssetDatabase.GenerateUniqueAssetPath(path + "/" + name + ".prefab");
            PrefabUtility.SaveAsPrefabAsset(go, uniquePath);
            Object.DestroyImmediate(go);
            BulletStormLogger.Log("Created prefab '" + name + "'.\n" + path + "/" + name);
        }
        
        /// <summary>
        /// Gets asset directory when right click on asset explorer.
        /// </summary>
        /// <param name="defaultDirectory">Returns this value if can't get directory.</param>
        /// <returns>For example: "Assets/Folder"</returns>
        private static string GetCurrentAssetDirectory(string defaultDirectory)
        {
            foreach (var obj in Selection.GetFiltered<Object>(SelectionMode.Assets))
            {
                var path = AssetDatabase.GetAssetPath(obj);
                if (string.IsNullOrEmpty(path))
                    continue;

                if (System.IO.Directory.Exists(path))
                    return path;
                if (System.IO.File.Exists(path))
                    return System.IO.Path.GetDirectoryName(path);
            }

            return defaultDirectory;
        }
    }
}