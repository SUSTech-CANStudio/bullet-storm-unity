using System;
using NaughtyAttributes;
using UnityEngine;
using XNode;

#pragma warning disable 0649

namespace CANStudio.BulletStorm.XNodes.Math.Noun
{
    [CreateNodeMenu("BulletStorm/Math/Noun/Quaternion"), NodeTint(Utils.ColorMathNoun)]
    public class Quaternion : Node
    {
        [Output]
        public UnityEngine.Quaternion value;

        [SerializeField, NodeEnum]
        private Type type;
        
        [Tooltip("Define upward direction.")]
        public bool setUpward;
        
        [Input(connectionType = ConnectionType.Override), Label(""), AllowNesting]
        public UnityEngine.Vector3 vector0;

        [Input(connectionType = ConnectionType.Override), Label(""), AllowNesting]
        public UnityEngine.Vector3 vector1;

        #region reflection use only
        // ReSharper disable UnusedMember.Local
        private bool ShowEuler => type == Type.Euler;
        private bool ShowForward => type == Type.LookRotation;
        private bool ShowFromToDirection => type == Type.FromToRotation;
        // ReSharper restore UnusedMember.Local
        #endregion

        public override object GetValue(NodePort port)
        {
            if (port.fieldName == nameof(value)) return value;
            return null;
        }

        private void OnValidate()
        {
            switch (type)
            {
                case Type.Euler:
                    value = UnityEngine.Quaternion.Euler(GetInputValue(nameof(vector0), vector0));
                    break;
                case Type.LookRotation:
                    if (vector0 == UnityEngine.Vector3.zero) vector0 = UnityEngine.Vector3.forward;
                    value = setUpward
                        ? UnityEngine.Quaternion.LookRotation(GetInputValue(nameof(vector0), vector0),
                            GetInputValue(nameof(vector1), vector1))
                        : UnityEngine.Quaternion.LookRotation(GetInputValue(nameof(vector0), vector0));
                    break;
                case Type.FromToRotation:
                    value = UnityEngine.Quaternion.FromToRotation(GetInputValue(nameof(vector0), vector0),
                        GetInputValue(nameof(vector1), vector1));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            this.NotifyChange();
        }
        
        [Serializable]
        private enum Type
        {
            Euler = 0,
            LookRotation = 1,
            FromToRotation = 2,
        }
    }
}