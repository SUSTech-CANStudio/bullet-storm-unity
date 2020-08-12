using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using BulletStorm.Editor;
using UnityEditor;
using UnityEngine;

namespace BulletStorm.Util
{
    /// <summary>
    /// Deal with string localization.
    /// This class translates strings using xml files.
    /// </summary>
    /// XML file format:
    /// <code>
    /// &lt;root&gt;
    ///     &lt;item src="(source_string_0)"&gt;(translated_string_0)&lt;item/&gt;
    ///     &lt;item src="(source_string_1)"&gt;(translated_string_1)&lt;item/&gt;
    /// &lt;root/&gt;
    /// </code>
    internal class Localization
    {
        /// <summary>
        /// Singleton instance.
        /// </summary>
        public static Localization Instance => Lazy.Value;
        
        private static readonly Lazy<Localization> Lazy = new Lazy<Localization>(() => new Localization());
        
        private Dictionary<string, string> tooltips;

        private Localization()
        {
#if UNITY_EDITOR
            LoadDictionaries(BulletStormEditorSettings.Language.ToString(),
                BulletStormEditorUtil.GetBasePath() + "/Config");
#endif
        }

        public static string Tooltip(string tooltip) => Translate(tooltip, Instance.tooltips);
        
        private static string Translate(string origin, IReadOnlyDictionary<string, string> dictionary)
        {
            return dictionary is null || !dictionary.TryGetValue(origin, out var translate) || translate is null ||
                   translate.Trim().Length == 0
                ? origin
                : translate;
        }
        
#if UNITY_EDITOR
        private void LoadDictionaries(string language, string path)
        {
            const string extension = ".xml";
            LoadDictionary(path + "/Tooltips/" + language + extension, out tooltips);
        }

        private static bool LoadDictionary(string assetPath, out Dictionary<string, string> dictionary)
        {
            var text = AssetDatabase.LoadAssetAtPath<TextAsset>(assetPath);
            if (text == null)
            {
                dictionary = null;
                return false;
            }

            var xml = XDocument.Parse(text.text);
            if (xml.Root == null)
            {
                dictionary = null;
                return false;
            }

            dictionary = xml.Root.Elements()
                .ToDictionary(item => item.Attribute("src")?.Value ?? string.Empty, item => item.Value.Trim());
            return true;
        }

        // public static void WriteDictionary(string resourceFolderPath, string resourcePath, IEnumerable<string> keys)
        // {
        //     var dic = LoadDictionary(resourcePath, out var old)
        //         ? keys.ToDictionary(key => key, key => old.TryGetValue(key, out var value) ? value : "")
        //         : keys.ToDictionary(key => key, key => "");
        //     WriteDictionary(resourceFolderPath + resourcePath, dic);
        // }
        //
        // private static void WriteDictionary(string assetPath, Dictionary<string, string> dictionary)
        // {
        //     var root = new XElement("root");
        //     foreach (var pair in dictionary)
        //     {
        //         root.Add(new XElement("item", new XAttribute("src", pair.Key), pair.Value));
        //     }
        //     var doc = new XDocument(new XDeclaration("1.0", "utf-8", "yes"), root);
        //     AssetDatabase.CreateAsset(new TextAsset(doc.ToString()), assetPath);
        // }
#endif
    }
}