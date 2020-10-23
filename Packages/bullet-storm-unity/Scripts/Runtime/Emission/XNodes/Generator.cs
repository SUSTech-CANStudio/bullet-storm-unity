using System;
using NaughtyAttributes;
using UnityEngine;
using XNode;

#pragma warning disable 0649

namespace CANStudio.BulletStorm.Emission.XNodes
{
    [CreateNodeMenu("BulletStorm/Shape/Generator")]
    public class Generator : ShapeNode
    {
        [SerializeField]
        private Type type;

        [Tooltip("Number of bullets."), MinValue(0), AllowNesting]
        public int num;

        [Tooltip("Radius of the shape."), MinValue(0), ShowIf("ShowRadius"), AllowNesting]
        public float radius;

        [Tooltip("Length of the shape."), MinValue(0), ShowIf("ShowLength"), AllowNesting]
        public float length;

        [Tooltip("Angle of the shape."), MinValue(0), ShowIf("ShowAngle"), AllowNesting]
        public float angle;
        
        #region reflection use only
        // ReSharper disable UnusedMember.Local
        private bool ShowRadius => type == Type.FibonacciSphere ||
                                   type == Type.RandomSphere ||
                                   type == Type.Ring ||
                                   type == Type.Arc;

        private bool ShowLength => type == Type.Line;

        private bool ShowAngle => type == Type.Arc;
        // ReSharper restore UnusedMember.Local
        #endregion
        
        public override void Generate()
        {
            switch (type)
            {
                case Type.Empty:
                    shape = new Shape(num);
                    break;
                case Type.FibonacciSphere:
                    shape = Shape.FibonacciSphere(num, radius);
                    break;
                case Type.RandomSphere:
                    shape = Shape.RandomSphere(num, radius);
                    break;
                case Type.Ring:
                    shape = Shape.Ring(num, radius);
                    break;
                case Type.Line:
                    shape = Shape.Line(num, length);
                    break;
                case Type.Arc:
                    shape = Shape.Arc(num, angle, radius);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        public override object GetValue(NodePort port)
        {
            switch (port.fieldName)
            {
                case "shape":
                    return shape;
                default:
                    return null;
            }
        }

        [Serializable]
        private enum Type
        {
            Empty,
            FibonacciSphere,
            RandomSphere,
            Ring,
            Line,
            Arc,
        }
    }
}