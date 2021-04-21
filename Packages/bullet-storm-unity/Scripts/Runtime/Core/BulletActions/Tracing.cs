using System;
using System.Numerics;

namespace CANStudio.BulletStorm.Core.BulletActions
{
    public class Tracing : IBulletAction
    {
        private readonly int _target;
        private readonly float _tracingRate;
        private readonly Func<float, float> _tracingRateCurve;
        private BulletStormContext _context;

        /// <summary>
        ///     Creates a tracing action with constant tracing rate.
        /// </summary>
        /// <param name="target">name of a vector variable in <see cref="VariableTable" />, representing a position.</param>
        /// <param name="tracingRate">max rotating angle (in degree) per second.</param>
        public Tracing(string target, float tracingRate)
        {
            _target = BulletStormContext.String2Hash(target);
            _tracingRate = tracingRate;
            _tracingRateCurve = null;
        }

        /// <summary>
        ///     Creates a tracing action with curve.
        /// </summary>
        /// <param name="target">name of a vector variable in <see cref="VariableTable" />, representing a position.</param>
        /// <param name="tracingRateCurve">
        ///     input is the angle (in degree) between bullet's velocity and bullet to target direction,
        ///     output is the tracing rate.
        /// </param>
        public Tracing(string target, Func<float, float> tracingRateCurve)
        {
            _target = BulletStormContext.String2Hash(target);
            _tracingRateCurve = tracingRateCurve;
        }

        public void SetContext(BulletStormContext context)
        {
            _context = context;
        }

        public void UpdateBullet(ref BulletParams @params, float deltaTime)
        {
            var direction = Vector3.Transform(Vector3.UnitZ, @params.rotation);
            var aimDirection = _context.GetVector(_target) - @params.position;
            var axis = Vector3.Cross(direction, aimDirection);
            var dAngle = _tracingRateCurve?.Invoke(MathUtil.AngleBetween(direction, aimDirection)) ?? _tracingRate;
            if (dAngle < 0)
                dAngle = 0;
            @params.rotation = Quaternion.CreateFromAxisAngle(axis, dAngle) * @params.rotation;
        }
    }
}