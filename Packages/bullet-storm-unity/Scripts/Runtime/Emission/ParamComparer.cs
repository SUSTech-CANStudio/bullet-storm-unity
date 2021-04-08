using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace CANStudio.BulletStorm.Emission
{
    /// <summary>
    ///     Item to be compared.
    /// </summary>
    /// Axises are raw values of the bullet positions.
    /// <para />
    /// Angles are the euler angles from origin to bullet position, between 0 and 360.
    [Serializable]
    public enum ParamCompareItem
    {
        XAxis,
        YAxis,
        ZAxis,
        XAngle,
        YAngle,
        ZAngle
    }

    [Serializable]
    public enum ParamCompareOrder
    {
        Ascending,
        Descending
    }

    [Serializable]
    public class ParamComparer : IComparer<BulletEmitParam>
    {
        [SerializeField] [HideInInspector] private List<CompareInfo> infos;

        /// <summary>
        ///     Create a comparer for <see cref="BulletEmitParam" />. You can use operator '+' to add two comparer,
        ///     then you get a new comparer that does the first comparision, and if the first comparision result is equal,
        ///     will continue on the second comparision.
        /// </summary>
        /// <param name="compareItem">Value in the <see cref="BulletEmitParam" /> to compare</param>
        /// <param name="order">Sorting order</param>
        /// <param name="equalRange">Difference less or equal than this will be recognized as same</param>
        public ParamComparer(ParamCompareItem compareItem, ParamCompareOrder order, float equalRange = 0)
        {
            infos = new List<CompareInfo> {new CompareInfo(compareItem, order, equalRange)};
        }

        internal ParamComparer(List<CompareInfo> infos)
        {
            this.infos = infos;
        }

        public int Compare(BulletEmitParam x, BulletEmitParam y)
        {
            const float roundAngle = 360;
            foreach (var compareInfo in infos)
            {
                float xResult, yResult, result;
                switch (compareInfo.compareItem)
                {
                    case ParamCompareItem.XAxis:
                        xResult = x.position.x;
                        yResult = y.position.x;
                        break;
                    case ParamCompareItem.YAxis:
                        xResult = x.position.y;
                        yResult = y.position.y;
                        break;
                    case ParamCompareItem.ZAxis:
                        xResult = x.position.z;
                        yResult = y.position.z;
                        break;
                    case ParamCompareItem.XAngle:
                        xResult = Quaternion.LookRotation(x.position).eulerAngles.x % roundAngle;
                        if (xResult < 0) xResult += roundAngle;
                        yResult = Quaternion.LookRotation(y.position).eulerAngles.x % roundAngle;
                        if (yResult < 0) yResult += roundAngle;
                        break;
                    case ParamCompareItem.YAngle:
                        xResult = Quaternion.LookRotation(x.position).eulerAngles.y % roundAngle;
                        if (xResult < 0) xResult += roundAngle;
                        yResult = Quaternion.LookRotation(y.position).eulerAngles.y % roundAngle;
                        if (yResult < 0) yResult += roundAngle;
                        break;
                    case ParamCompareItem.ZAngle:
                        xResult = Quaternion.LookRotation(x.position).eulerAngles.z % roundAngle;
                        if (xResult < 0) xResult += roundAngle;
                        yResult = Quaternion.LookRotation(y.position).eulerAngles.z % roundAngle;
                        if (yResult < 0) yResult += roundAngle;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                switch (compareInfo.order)
                {
                    case ParamCompareOrder.Ascending:
                        result = xResult - yResult;
                        break;
                    case ParamCompareOrder.Descending:
                        result = yResult - xResult;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                if (Mathf.Abs(result) > compareInfo.equalRange) return (int) Mathf.Sign(result);
            }

            return 0;
        }

        public static ParamComparer operator +(ParamComparer first, ParamComparer second)
        {
            var infos = new List<CompareInfo>(first.infos);
            infos.AddRange(second.infos);
            return new ParamComparer(infos);
        }

        [Serializable]
        public struct CompareInfo
        {
            [Tooltip("Select which item to compare.")]
            public ParamCompareItem compareItem;

            public ParamCompareOrder order;

            [Tooltip("Difference smaller than this will be ignored.")] [MinValue(0)] [AllowNesting]
            public float equalRange;

            public CompareInfo(ParamCompareItem compareItem, ParamCompareOrder order, float equalRange)
            {
                this.compareItem = compareItem;
                this.order = order;
                this.equalRange = equalRange;
            }
        }
    }
}