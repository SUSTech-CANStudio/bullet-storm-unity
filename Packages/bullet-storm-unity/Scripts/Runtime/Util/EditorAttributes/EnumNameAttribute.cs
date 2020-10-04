using System;
using UnityEngine;

namespace CANStudio.BulletStorm.Util.EditorAttributes
{
    /// <summary>
    /// Change display enum name on GUI.
    /// </summary>
    /// Use this attribute on enum value to set display name, and use this attribute on fields to
    /// draw the enum.
    [AttributeUsage(AttributeTargets.Field)]
    public class EnumNameAttribute : PropertyAttribute
    {
        public readonly string name;
        
        /// <summary>
        /// Add on enum field to display custom enum value names. 
        /// </summary>
        public EnumNameAttribute(){}
        
        /// <summary>
        /// Add on enum value to set the name to display.
        /// </summary>
        /// <param name="name">Name to display.</param>
        public EnumNameAttribute(string name)
        {
            this.name = name;
        }
    }
}