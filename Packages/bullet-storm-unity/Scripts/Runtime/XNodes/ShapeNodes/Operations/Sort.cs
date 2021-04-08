using System.Collections.Generic;
using CANStudio.BulletStorm.Emission;
using UnityEngine;

#pragma warning disable 0649

namespace CANStudio.BulletStorm.XNodes.ShapeNodes.Operations
{
    [CreateNodeMenu("BulletStorm/Shape/Operation/Sort", Utils.OrderSpecialOperation)]
    [NodeTint(Utils.ColorShapeOperationSpecial)]
    public class Sort : ShapeOperationNode
    {
        [SerializeField] private List<ParamComparer.CompareInfo> comparers;

        public override void Generate()
        {
            SetShape(CopyInputShape().Sort(new ParamComparer(comparers)));
        }
    }
}