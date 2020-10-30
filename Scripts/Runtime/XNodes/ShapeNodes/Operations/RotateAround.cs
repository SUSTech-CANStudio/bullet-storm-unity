using System;
using NaughtyAttributes;
using UnityEngine;

#pragma warning disable 0649

namespace CANStudio.BulletStorm.XNodes.ShapeNodes.Operations
{
    [CreateNodeMenu("BulletStorm/Shape/Operation/Rotate Around", Utils.OrderPositionOperation), NodeTint(Utils.ColorShapeOperation)]
    public class RotateAround : ShapeOperationNode
    {
        [SerializeField, OnValueChanged("TypeChange"), AllowNesting, NodeEnum]
        private Type type;

        [Input(connectionType = ConnectionType.Override, typeConstraint = TypeConstraint.Inherited)]
        public Vector3 point;
        
        private PortRegistry<Type> registry;
        
        public override void Generate() => registry.Invoke();

        // ReSharper disable once UnusedMember.Local
        private void TypeChange() => registry.Activate(type);
        
        private new void OnEnable()
        {
            registry = new PortRegistry<Type>(this);
            
            registry.RegisterPorts(Type.Quaternion, (typeof(Quaternion), "rotation", true));
            registry.RegisterActions(Type.Quaternion,
                ports => SetShape(CopyInputShape()
                    .RotateAround(GetInputValue(nameof(point), point), ports[0].GetInputValue<Quaternion>())));
            
            registry.RegisterPorts(Type.Axis, (typeof(float), "angle", true), (typeof(Vector3), "axis", true));
            registry.RegisterActions(Type.Axis,
                ports => SetShape(CopyInputShape().RotateAround(GetInputValue(nameof(point), point),
                    ports[0].GetInputValue<float>(), ports[1].GetInputValue<Vector3>())));
            
            registry.Activate(type);
            
            base.OnEnable();
        }
        
        [Serializable]
        private enum Type
        {
            Quaternion,
            [Tooltip("Rotate around axis with a given angle.")]
            Axis,
        }
    }
}