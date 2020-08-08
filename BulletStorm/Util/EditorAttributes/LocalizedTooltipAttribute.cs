using UnityEngine;

namespace BulletStorm.Util.EditorAttributes
{
    public class LocalizedTooltipAttribute : TooltipAttribute
    {
        public LocalizedTooltipAttribute(string tooltip) : base(Localization.Tooltip(tooltip))
        {
        }
    }
}