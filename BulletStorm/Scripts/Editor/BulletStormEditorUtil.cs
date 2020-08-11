using System;
using System.Linq;
using BulletStorm.Util;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;
using Object = UnityEngine.Object;

namespace BulletStorm.Editor
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

        /// <summary>
        /// Gets an attribute on an enum field value
        /// </summary>
        /// <param name="enumVal">The enum value</param>
        /// <typeparam name="T">The type of the attribute you want to retrieve</typeparam>
        /// <returns>The attribute of type T that exists on the enum value</returns>
        /// <example><![CDATA[string desc = myEnumVariable.GetAttributeOfType<DescriptionAttribute>().Description;]]></example>
        public static T GetAttributeOfType<T>(Enum enumVal) where T : Attribute
        {
            var type = enumVal.GetType();
            var memInfo = type.GetMember(enumVal.ToString());
            var attributes = memInfo[0].GetCustomAttributes(typeof(T), false);
            return attributes.Length > 0 ? (T) attributes[0] : null;
        }

        /// <summary>
        /// Load default asset in "BulletStorm/Config/DefaultItems".
        /// </summary>
        /// <param name="name">Asset name, including extension.</param>
        /// <typeparam name="T">Asset type.</typeparam>
        /// <returns></returns>
        public static T LoadDefaultAsset<T>(string name) where T : Object =>
            AssetDatabase.LoadAssetAtPath<T>(GetBasePath() + "/Config/DefaultItems/" + name);
    }
}
