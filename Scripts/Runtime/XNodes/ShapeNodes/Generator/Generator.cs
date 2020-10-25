using System;
using CANStudio.BulletStorm.Emission;
using NaughtyAttributes;
using UnityEngine;

#pragma warning disable 0649

namespace CANStudio.BulletStorm.XNodes.ShapeNodes
{
    [CreateNodeMenu("BulletStorm/Shape/Generator/Generator")]
    public class Generator : ShapeNode
    {
        [SerializeField, NodeEnum]
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
                    SetShape(new Shape(num));
                    break;
                case Type.FibonacciSphere:
                    SetShape(Shape.FibonacciSphere(num, radius));
                    break;
                case Type.RandomSphere:
                    SetShape(Shape.RandomSphere(num, radius));
                    break;
                case Type.Ring:
                    SetShape(Shape.Ring(num, radius));
                    break;
                case Type.Line:
                    SetShape(Shape.Line(num, length));
                    break;
                case Type.Arc:
                    SetShape(Shape.Arc(num, angle, radius));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
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