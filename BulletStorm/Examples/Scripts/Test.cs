using System.Xml.Linq;
using UnityEngine;

namespace BulletStorm.Examples.Scripts
{
    public class Test : MonoBehaviour
    {
        public Quaternion q;
        
        private void Start()
        {
            var root = new XElement("Root");
            var document = new XDocument(new XDeclaration("1.0", "utf-8", "yes"), root);
            root.Add(new XElement("Item", new XAttribute("src", "hahahaha"), "????"));
            root.Add(new XElement("Item", new XAttribute("src", "gagagaga"), "!!!!"));
            Debug.Log(document);
        }
    }
}