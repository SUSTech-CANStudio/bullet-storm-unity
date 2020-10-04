using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using CANStudio.BulletStorm.Util.EditorAttributes;
using UnityEditor;
using UnityEngine;

#pragma warning disable 0649

namespace CANStudio.BulletStorm.Editor
{
    /// <summary>
    /// Editor settings of bullet storm.
    /// </summary>
    public class BulletStormEditorSettings : ScriptableObject
    {
        public const string Path = "/Config/preferences.asset";

        internal static BulletStormEditorSettings Instance => Lazy.Value;
        private static readonly Lazy<BulletStormEditorSettings> Lazy = new Lazy<BulletStormEditorSettings>(() =>
        {
            var basePath = BulletStormEditorUtil.GetBasePath();
            var asset = AssetDatabase.LoadAssetAtPath<BulletStormEditorSettings>(basePath + Path);
            if (asset) return asset;
            
            asset = CreateInstance<BulletStormEditorSettings>();
            AssetDatabase.CreateAsset(asset, basePath + Path);
            return asset;
        });
        
        [SerializeField]
        [EnumName]
        private BulletStormLanguage language;
        public static BulletStormLanguage Language => Instance.language;
    }
    
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public enum BulletStormLanguage
    {
        [EnumName("English")]
        en,
        [EnumName("简体中文")]
        zh_CN,
    }
    
    internal class BulletStormEditorSettingsProvider : SettingsProvider
    {
        private readonly SerializedObject settings;

        private BulletStormEditorSettingsProvider(string path, SettingsScope scopes = SettingsScope.User,
            IEnumerable<string> keywords = null) : base(path, scopes, keywords)
        {
            settings = new SerializedObject(BulletStormEditorSettings.Instance);
        }

        public override void OnGUI(string searchContext)
        {
            settings.UpdateIfRequiredOrScript();
            var property = settings.GetIterator();
            property.Next(true);
            property.NextVisible(false);
            while (property.NextVisible(false))
            {
                EditorGUILayout.PropertyField(property);
            }

            settings.ApplyModifiedProperties();
        }

        [SettingsProvider]
        public static SettingsProvider GetProvider()
        {
            var provider = new BulletStormEditorSettingsProvider("Preferences/BulletStorm");
            provider.keywords = GetSearchKeywordsFromSerializedObject(provider.settings);
            return provider;
        }
    }
}