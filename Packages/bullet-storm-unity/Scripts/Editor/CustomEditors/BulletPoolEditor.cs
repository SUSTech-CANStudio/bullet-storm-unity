using System.Collections.Generic;
using System.Linq;
using CANStudio.BulletStorm.BulletSystem;
using UnityEditor;
using UnityEngine;

namespace CANStudio.BulletStorm.Editor.CustomEditors
{
    [CustomEditor(typeof(BulletPool))]
    internal class BulletPoolEditor : UnityEditor.Editor
    {
        private string bullets;
        private string inheritedBullets;
        private BulletPool self;

        private void OnEnable()
        {
            self = target as BulletPool;
            if (self == null) return;
            bullets = BulletsToString(self.bullets);
            inheritedBullets = InheritedBulletsToString(self);
        }

        protected override bool ShouldHideOpenButton()
        {
            return true;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button(new GUIContent("Detect", "Detect bullets in the folder.")))
            {
                Detect(self);
                bullets = BulletsToString(self.bullets);
                inheritedBullets = InheritedBulletsToString(self);
            }

            if (bullets != "")
            {
                EditorGUILayout.LabelField("Bullets in this pool");
                EditorGUILayout.HelpBox(bullets, MessageType.Info);
            }

            if (inheritedBullets != "")
            {
                EditorGUILayout.LabelField("Inherited bullets in this pool");
                EditorGUILayout.HelpBox(inheritedBullets, MessageType.Info);
            }
        }

        private static void Detect(BulletPool bulletPool)
        {
            bulletPool.bullets.Clear();
            var selfPath = AssetDatabase.GetAssetPath(bulletPool);
            var lastIndex = selfPath.LastIndexOf('/');
            var guidList = AssetDatabase.FindAssets("", new[] {selfPath.Substring(0, lastIndex)});
            foreach (var guid in guidList)
            {
                var assetPath = AssetDatabase.GUIDToAssetPath(guid);
                var prefab = AssetDatabase.LoadMainAssetAtPath(assetPath) as GameObject;
                if (prefab is null) continue;
                var type = PrefabUtility.GetPrefabAssetType(prefab);
                if (type != PrefabAssetType.Regular && type != PrefabAssetType.Variant) continue;
                if (prefab.TryGetComponent(out IBullet bulletSystem))
                    bulletPool.bullets.Add(bulletSystem.Name, bulletSystem);
            }
        }

        private static string BulletsToString(IReadOnlyDictionary<string, IBullet> bulletSystems)
        {
            var names = new List<string>(bulletSystems.Keys);
            if (names.Count == 0) return "";
            names.Sort();
            return names.Aggregate((current, add) => current + "\n" + add);
        }

        private static string InheritedBulletsToString(BulletPool bulletPool)
        {
            if (!bulletPool.parentPool || bulletPool.parentPool == bulletPool) return "";
            var names = new List<string>(AllBulletNames(bulletPool.parentPool));
            if (names.Count == 0) return "";
            names.Sort();
            return names.Aggregate((current, add) => current + "\n" + add);
        }

        private static IEnumerable<string> AllBulletNames(BulletPool bulletPool)
        {
            var result = new HashSet<string>(bulletPool.bullets.Keys);
            if (bulletPool.parentPool && bulletPool.parentPool != bulletPool)
                result.UnionWith(AllBulletNames(bulletPool.parentPool));

            return result;
        }
    }
}