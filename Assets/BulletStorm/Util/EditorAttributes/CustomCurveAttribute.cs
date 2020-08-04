using System;
using UnityEngine;

namespace BulletStorm.Util.EditorAttributes
{
    public class CustomCurveAttribute : PropertyAttribute
    {
        public readonly Rect range;
        public readonly Color color;

        public CustomCurveAttribute(float x, float y, float w, float h)
        {
            range = new Rect(x, y, w, h);
            color = Color.green;
        }

        public CustomCurveAttribute(float x, float y, float w, float h, float r, float g, float b, float a = 1)
        {
            range = new Rect(x, y, w, h);
            color = new Color(r, g, b, a);
        }
    }
}