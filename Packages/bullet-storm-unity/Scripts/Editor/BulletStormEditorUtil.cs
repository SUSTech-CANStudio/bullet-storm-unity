using System;
using System.Linq;
using CANStudio.BulletStorm.Util;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;
using Object = UnityEngine.Object;

namespace CANStudio.BulletStorm.Editor
{
    internal static class BulletStormEditorUtil
    {
        private static string _basePath;
        
        /// <summary>
        /// Calculate total height all child properties.
        /// </summary>
        /// <param name="property">The parent property.</param>
        /// <param name="except">Don't calculate properties in these names.</param>
        /// <returns>Total height.</returns>
        public static float ChildPropertiesHeight(SerializedProperty property, params string[] except)
        {
            var refProperty = property.Copy();
            var nextProperty = property.Copy();
            var height = 0f;
            var shouldBreak = nextProperty.NextVisible(false);
            if (refProperty.NextVisible(true)) do
            {
                if (shouldBreak && refProperty.propertyPath == nextProperty.propertyPath) break;
                if (except.Contains(refProperty.name)) continue;
                height += EditorGUI.GetPropertyHeight(refProperty) + EditorGUIUtility.standardVerticalSpacing;
            } while (refProperty.NextVisible(false));
            
            return height;
        }

        /// <summary>
        /// Draw all child properties of a given property.
        /// </summary>
        /// <param name="position">Position of the property.</param>
        /// <param name="property">The parent property.</param>
        /// <param name="except">Don't draw properties in these names.</param>
        public static void DrawChildProperties(Rect position, SerializedProperty property, params string[] except)
        {
            var refProperty = property.Copy();
            var nextProperty = property.Copy();
            var shouldBreak = nextProperty.NextVisible(false);
            if (refProperty.NextVisible(true)) do
            {
                if (shouldBreak && refProperty.propertyPath == nextProperty.propertyPath) break;
                if (except.Contains(refProperty.name)) continue;
                EditorGUI.PropertyField(GetPropertyPosition(ref position, refProperty), refProperty);
            } while (refProperty.NextVisible(false));
        }

        /// <summary>
        /// Get position for a property and move current rect forward.
        /// </summary>
        /// <param name="current">Current position rect</param>
        /// <param name="property">Property to draw</param>
        /// <returns>Position for the property</returns>
        public static Rect GetPropertyPosition(ref Rect current, SerializedProperty property)
        {
            var result = new Rect(current) {height = EditorGUI.GetPropertyHeight(property, false)};
            current.yMin += result.height + EditorGUIUtility.standardVerticalSpacing;
            // if (current.height < 0) BulletStormLogger.LogWarning("Property position exceeded: " + current.height);
            return result;
        }

        /// <summary>
        /// Gets path of bullet storm folder.
        /// </summary>
        /// <returns></returns>
        public static string GetBasePath()
        {
            if (!(_basePath is null) && _basePath.StartsWith("Assets/")) return _basePath;
            
            var assets = AssetDatabase.FindAssets("BulletStorm t:asmdef");
            Assert.IsNotNull(assets);

            var selected = assets.ToList().ConvertAll(AssetDatabase.GUIDToAssetPath)
                .Where(path => path.EndsWith("/BulletStorm.asmdef")).ToList();

            if (selected.Count == 1)
            {
                _basePath = selected[0].Substring(0, selected[0].LastIndexOf('/'));
                return _basePath;
            }
            
            BulletStormLogger.LogError("Can't locate bullet storm assembly, find " + (selected.Count > 0
                ? selected.Count + " items:\n" + selected.Aggregate((current, next) => current + "\n" + next)
                : "0 item."));
            _basePath = "Assets/BulletStorm";
            return _basePath;
        }
    }
}
