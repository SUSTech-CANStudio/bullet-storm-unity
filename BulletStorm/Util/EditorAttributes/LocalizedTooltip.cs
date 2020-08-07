using UnityEngine;

namespace BulletStorm.Util.EditorAttributes
{
    public class LocalizedTooltip : TooltipAttribute
    {
        public LocalizedTooltip(string tooltip) : base(Localization.Tooltip(tooltip))
        {
        }
    }
}