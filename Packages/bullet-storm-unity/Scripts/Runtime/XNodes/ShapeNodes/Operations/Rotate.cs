using System;
using NaughtyAttributes;
using UnityEngine;

#pragma warning disable 0649

namespace CANStudio.BulletStorm.XNodes.ShapeNodes.Operations
{
    [CreateNodeMenu("BulletStorm/Shape/Operation/Rotate", Utils.OrderPositionOperation), NodeTint(Utils.ColorShapeOperation)]
    public class Rotate : ShapeOperationNode
    {
        [SerializeField, OnValueChanged("TypeChange"), AllowNesting, NodeEnum]
        private Type type;
        
        private PortRegistry<Type> registry;
        
        public override void Generate() => registry.Invoke();

        // ReSharper disable once UnusedMember.Local
        private void TypeChange() => registry.Activate(type);

        private new void OnEnable()
        {
            registry = new PortRegistry<Type>(this);
            
            registry.RegisterPorts(Type.Quaternion, (typeof(Quaternion), "rotation", true));
            registry.RegisterActions(Type.Quaternion,
                ports => SetShape(CopyInputShape().Rotate(ports[0].GetInputValue<Quaternion>())));
            
            registry.RegisterPorts(Type.Axis, (typeof(float), "angle", true),
                (typeof(Vector3), "axis", true));
            registry.RegisterActions(Type.Axis,
                ports =>
                {
                    SetShape(CopyInputShape().Rotate(ports[0].GetInputValue<float>(),
                        ports[1].GetInputValue<Vector3>()));
                });
            
            registry.RegisterPorts(Type.Euler, (typeof(Vector3), "euler", true));
            registry.RegisterActions(Type.Euler,
                ports => SetShape(CopyInputShape().Rotate(ports[0].GetInputValue<Vector3>())));
            
            registry.Activate(type);
            
            base.OnEnable();
        }

        [Serializable]
        private enum Type
        {
            Quaternion,
            [Tooltip("Rotate around axis with a given angle.")]
            Axis,
            Euler,
        }
    }
}