using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

#pragma warning disable 0649

namespace BulletStorm.BulletSystem
{
    /// <summary>
    /// A useful tool to manage bullets.
    /// </summary>
    /// Put this <see cref="ScriptableObject"/> into the folder you store your bullet system prefabs,
    /// and it can auto register all bullet systems in the folder and sub folders.
    /// Then you can use <see cref="Find"/> to get a bullet.
    [CreateAssetMenu(menuName = "BulletStorm/BulletPool")]
    public sealed class BulletPool : ScriptableObject
    {
        private readonly Dictionary<string, IOriginBulletSystem> bullets = new Dictionary<string, IOriginBulletSystem>();
        
        [Tooltip("Bullet pool can inherit bullets from other pool.")]
        [SerializeField] private BulletPool parentPool;
        
        [NotNull]
        private IEnumerable<string> AllBulletNames
        {
            get
            {
                var result = new HashSet<string>(bullets.Keys);
                if (parentPool && parentPool != this)
                {
                    result.UnionWith(parentPool.AllBulletNames);
                }

                return result;
            }
        }
        
        /// <summary>
        /// Finds a bullet system in this pool, will also find in parent pools.
        /// </summary>
        /// <param name="bulletName">Name of the bullet (bullet system prefab name)</param>
        /// <returns>Null if not found</returns>
        public IOriginBulletSystem Find(string bulletName)
        {
            if (bullets.TryGetValue(bulletName, out var bullet)) return bullet;
            if (parentPool && parentPool != this) return parentPool.Find(bulletName);
            return null;
        }
        
#if UNITY_EDITOR
        /// <summary>
        /// Register all bullets in the same folder or in subfolders into the pool.
        /// </summary>
        public void Refresh()
        {
            bullets.Clear();
            var selfPath = AssetDatabase.GetAssetPath(this);
            var lastIndex = selfPath.LastIndexOf('/');
            var guidList = AssetDatabase.FindAssets("", new[] {selfPath.Substring(0, lastIndex)});
            foreach (var guid in guidList)
            {
                var assetPath = AssetDatabase.GUIDToAssetPath(guid);
                var prefab = AssetDatabase.LoadMainAssetAtPath(assetPath) as GameObject;
                if (prefab is null) continue;
                var type = PrefabUtility.GetPrefabAssetType(prefab);
                if (type != PrefabAssetType.Regular && type != PrefabAssetType.Variant) continue;
                if (prefab.TryGetComponent(out IOriginBulletSystem bulletSystem))
                {
                    bullets.Add(bulletSystem.Name, bulletSystem);
                }
            }
        }
        
        public string BulletsToString()
        {
            var names = new List<string>(bullets.Keys);
            if (names.Count == 0) return "";
            names.Sort();
            return names.Aggregate((current, add) => current + "\n" + add);
        }

        public string InheritedBulletsToString()
        {
            if (!parentPool || parentPool == this) return "";
            var names = new List<string>(parentPool.AllBulletNames);
            if (names.Count == 0) return "";
            names.Sort();
            return names.Aggregate((current, add) => current + "\n" + add);
        }

        private void OnEnable()
        {
            Refresh();
        }
#endif
    }
}

