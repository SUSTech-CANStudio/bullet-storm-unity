using XNode;

namespace CANStudio.BulletStorm.XNodes.Math.Noun
{
    [CreateNodeMenu("BulletStorm/Math/Noun/Vector3"), NodeTint(Utils.ColorMathNoun)]
    public class Vector3 : Node
    {
        [Output(ShowBackingValue.Always)]
        public UnityEngine.Vector3 value;

        public override object GetValue(NodePort port)
        {
            if (port.fieldName == nameof(value)) return value;
            return null;
        }

        private void OnValidate()
        {
            this.NotifyChange();
        }
    }
}