using System;
using UnityEngine;

namespace BulletStorm.Util.EditorAttributes
{
    /// <summary>
    /// Field visible if function returns true.
    /// </summary>
    public class VisibleIfAttribute : PropertyAttribute
    {
        public readonly string funcName;

        public VisibleIfAttribute(string funcName)
        {
            this.funcName = funcName;
        }
    }
}