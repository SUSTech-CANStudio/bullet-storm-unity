using System;
using System.Collections;
using BulletStorm.Util;
using UnityEngine;

namespace BulletStorm.Emitters
{
    /// <summary>
    /// Base class of all auto emitters.
    /// </summary>
    /// Auto emitters can rotate automatically when emitting bullets. But every auto emitter can only start
    /// one emission at same time.
    [DisallowMultipleComponent]
    public abstract class AutoEmitterBase : Emitter
    {
        [Header("Automation")]
        [Tooltip("Begin to emit bullets on start.")]
        public bool emitOnStart;
        [Tooltip("Auto destroy the emitter when emission finished.")]
        public bool destroyOnFinish;
        [Tooltip("Auto rotates the emitter to aim at a target.")]
        public AutoAimModule autoAim = new AutoAimModule
        {
            followRateMultiplier = 1
        };
        [Tooltip("Enables the emitter to emit towards customized direction, otherwise it will always emit forward.")]
        public AimOffsetModule aimOffset = new AimOffsetModule
        {
            curveTimeScale = 1,
            xOffsetMultiplier = 1,
            yOffsetMultiplier = 1,
            zOffsetMultiplier = 1
        };
        
        // the emission coroutine
        private ControllableCoroutine coroutine;
        // deal with aim offset module
        private Transform subEmitter;

        /// <summary>
        /// Transform used to emit bullets.
        /// </summary>
        protected Transform Emitter => aimOffset.enabled ? subEmitter : transform;

        /// <summary>
        /// Is the emitter doing an emission?
        /// </summary>
        public bool IsEmitting => coroutine.Status == CoroutineStatus.Running;
        
        public void StartEmission()
        {
            // auto aim
            if (autoAim.enabled && autoAim.aimOnEmissionStart && autoAim.CheckTarget())
            {
                transform.LookAt(autoAim.target);
            }
            
            // aim offset
            aimOffset.Reset();
            if (subEmitter is null)
            {
                subEmitter = new GameObject().transform;
                subEmitter.SetParent(transform);
                subEmitter.localPosition = Vector3.zero;
            }
            
            // start coroutine
            if (coroutine is null || coroutine.Status == CoroutineStatus.Finished)
            {
                coroutine = new ControllableCoroutine(StartEmitCoroutine(), () =>
                {
                    if (destroyOnFinish) Destroy(this);
                });
                coroutine.Start();
            }
            else
            {
                BulletStormLogger.LogWarning("Emitter is emitting now.");
            }
        }

        /// <summary>
        /// Begins after <see cref="StartEmission"/> called.
        /// </summary>
        /// <returns></returns>
        protected abstract IEnumerator StartEmitCoroutine();

        protected virtual void Start()
        {
            if (emitOnStart) StartEmission();
        }

        protected virtual void Update()
        {
            // auto aim
            if (autoAim.enabled && autoAim.CheckTarget())
            {
                var t = transform;
                var expected = autoAim.target.position - t.position;
                var forward = t.forward;
                forward = Vector3.RotateTowards(forward, expected,
                    autoAim.GetFollowRate(Vector3.Angle(forward, expected)), 0);
                transform.forward = forward;
            }

            // aim offset
            if (aimOffset.enabled)
            {
                aimOffset.Tick(Time.deltaTime);
                subEmitter.localEulerAngles = aimOffset.TotalOffset;
            }
        }

        [Serializable]
        public struct AutoAimModule
        {
            public bool enabled;
            [Tooltip("The game object should the emitter aim at.")]
            public Transform target;
            [Tooltip("When start emitting, aim at the target.")]
            public bool aimOnEmissionStart;
            
            [Header("Follow rate")]
            [Tooltip("Use a curve to describe follow rate.")]
            public bool followRateUseCurve;
            [Tooltip("Max rotation angle per second during emission to follow target")]
            public float followRateConst;
            [Tooltip("X-axis is the angle between target and current aim direction, Y-axis is rotation rate.")]
            public AnimationCurve followRateCurve;
            [Tooltip("Multiplier for follow rate curve.")]
            public float followRateMultiplier;

            public float GetFollowRate(float angleDiff) =>
                followRateUseCurve
                    ? followRateCurve.Evaluate(angleDiff) * followRateMultiplier
                    : followRateConst;

            public bool CheckTarget()
            {
                if (!(target is null)) return true;
                BulletStormLogger.LogError("Emitter aim target not set");
                return false;
            }
        }

        [Serializable]
        public struct AimOffsetModule
        {
            public bool enabled;
            [Tooltip("XYZ rotation offset when an emission starts in euler angles.")]
            public Vector3 offsetOnStart;
            [Tooltip("When using curve, the time in seconds that curve x-axis 0~1 represents.")]
            public float curveTimeScale;
            
            [Header("X-axis")]
            [Tooltip("Offset mode on x-axis, this will cause emitter aim up (negative) and down (positive).")]
            public OffsetMode xOffsetMode;
            [Tooltip("Offset on x-axis.")]
            public ParticleSystem.MinMaxCurve xOffset;
            [Tooltip("Multiplier for x offset.")]
            public float xOffsetMultiplier;

            [Header("Y-axis")]
            [Tooltip("Offset mode on y-axis, this will cause emitter aim left (negative) and right (positive).")]
            public OffsetMode yOffsetMode;
            [Tooltip("Offset on y-axis.")]
            public ParticleSystem.MinMaxCurve yOffset;
            [Tooltip("Multiplier for y offset.")]
            public float yOffsetMultiplier;

            [Header("Z-axis")]
            [Tooltip("Offset mode on z-axis, this will cause emitter aim clockwise (negative) and counterclockwise (positive).")]
            public OffsetMode zOffsetMode;
            [Tooltip("Offset on z-axis.")]
            public ParticleSystem.MinMaxCurve zOffset;
            [Tooltip("Multiplier for z offset.")]
            public float zOffsetMultiplier;

            private float time;
            public Vector3 TotalOffset { get; private set; }

            /// <summary>
            /// Call this on emission starts.
            /// </summary>
            internal void Reset()
            {
                time = 0;
                TotalOffset = offsetOnStart;
            }

            /// <summary>
            /// Tick the module to refresh <see cref="TotalOffset"/>.
            /// </summary>
            /// <param name="deltaTime">Delta time from last tick.</param>
            internal void Tick(float deltaTime)
            {
                time += deltaTime;
                TotalOffset = new Vector3(
                    GetOffset(TotalOffset.x, xOffsetMode, deltaTime, xOffset, xOffsetMultiplier),
                    GetOffset(TotalOffset.y, yOffsetMode, deltaTime, yOffset, yOffsetMultiplier),
                    GetOffset(TotalOffset.z, zOffsetMode, deltaTime, zOffset, zOffsetMultiplier));
            }

            private float GetOffset(float oldValue, OffsetMode mode, float deltaTime, ParticleSystem.MinMaxCurve curve,
                float multiplier)
            {
                var value = curve.Evaluate(time / curveTimeScale) * multiplier;
                switch (mode)
                {
                    case OffsetMode.ExactAngle:
                        return value;
                    case OffsetMode.AngularVelocity:
                        return oldValue + value * deltaTime;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
                }
            }

            public enum OffsetMode
            {
                [Tooltip("Use exactly this angle value.")]
                ExactAngle,
                [Tooltip("Rotate angle per second from emission start.")]
                AngularVelocity,
            }
        }
    }
}