using System;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

namespace BulletStorm.Util
{
    /// <summary>
    /// Deal with string localization.
    /// This class translates strings using xml files.
    /// </summary>
    /// XML file format:
    /// <code>
    /// &lt;(dictionary_name)&gt;
    ///     &lt;item src="(source_string_0)"&gt;(translated_string_0)&lt;item/&gt;
    ///     &lt;item src="(source_string_1)"&gt;(translated_string_1)&lt;item/&gt;
    /// &lt;(dictionary_name)/&gt;
    /// </code>
    public class Localization
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
        private void LoadDictionaries(string language)
        {
            const string configFolder = "BulletStormConfig/";
            const string tooltipFolder = "Tooltips/";
            LoadDictionary(configFolder + tooltipFolder + language, out tooltips);
        }

        private static void LoadDictionary(string path, out Dictionary<string, string> dictionary)
        {
            var text = Resources.Load(path) as TextAsset;
            if (text == null)
            {
                dictionary = null;
                return;
            }
            var xml = XElement.Parse(text.text);

            dictionary = new Dictionary<string, string>();
            foreach (var element in xml.Elements())
            {
                
            }
        }
#endif
    }
}