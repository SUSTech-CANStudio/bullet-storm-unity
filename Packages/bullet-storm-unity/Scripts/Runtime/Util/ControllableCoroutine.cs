using System;
using System.Collections;
using JetBrains.Annotations;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CANStudio.BulletStorm.Util
{
    /// <summary>
    ///     Provides more APIs than Unity standard coroutine.
    /// </summary>
    public sealed class ControllableCoroutine
    {
        private static readonly Lazy<MonoBehaviour> Daemon =
            new Lazy<MonoBehaviour>(() =>
            {
                var daemon = Object.Instantiate(new GameObject("CoroutineDaemon")
                    .AddComponent<CoroutineDaemon>());
                Object.DontDestroyOnLoad(daemon);
                return daemon;
            });

        private readonly IEnumerator _enumerator;
        private readonly MonoBehaviour _monoBehaviour;
        private Coroutine _coroutine;

        /// <summary>
        ///     Create a coroutine.
        ///     This coroutine will run under a "CoroutineDaemon" GameObject.
        /// </summary>
        /// <param name="coroutine">The coroutine function.</param>
        /// <param name="callback">Callback function on finish.</param>
        public ControllableCoroutine(IEnumerator coroutine, Action callback = null)
        {
            Status = CoroutineStatus.NotStarted;
            _enumerator = coroutine;
            finish = callback;
        }

        /// <summary>
        ///     Create a coroutine under given MonoBehaviour.
        /// </summary>
        /// <param name="monoBehaviour"></param>
        /// <param name="coroutine"></param>
        /// <param name="callback"></param>
        public ControllableCoroutine(MonoBehaviour monoBehaviour, IEnumerator coroutine, Action callback = null) : this(
            coroutine, callback)
        {
            _monoBehaviour = monoBehaviour;
        }

        /// <summary>
        ///     Status of the coroutine.
        /// </summary>
        public CoroutineStatus Status { get; private set; }

        /// <summary>
        ///     Returns the <see cref="MonoBehaviour" /> on which coroutine runs.
        /// </summary>
        [NotNull]
        private MonoBehaviour Root => _monoBehaviour ? _monoBehaviour : Daemon.Value;

        private event Action finish;

        /// <summary>
        ///     Starts or unpause the coroutine.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public void Start()
        {
            switch (Status)
            {
                case CoroutineStatus.NotStarted:
                    if (!Application.isPlaying)
                        throw new InvalidOperationException("Can't start coroutine in editor.");

                    Status = CoroutineStatus.Running;
                    _coroutine = Root.StartCoroutine(Wrapper());
                    break;
                case CoroutineStatus.Paused:
                    Status = CoroutineStatus.Running;
                    break;
                case CoroutineStatus.Running:
                    BulletStormLogger.Log("Coroutine already started.");
                    break;
                case CoroutineStatus.Finished:
                    throw new InvalidOperationException(
                        "Coroutine is finished. Call 'Restart' if you want to start it again.");
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        ///     Pauses the coroutine.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public void Pause()
        {
            switch (Status)
            {
                case CoroutineStatus.Running:
                    Status = CoroutineStatus.Paused;
                    break;
                case CoroutineStatus.NotStarted:
                    BulletStormLogger.LogWarning("Coroutine not started.");
                    break;
                case CoroutineStatus.Paused:
                    BulletStormLogger.Log("Coroutine already paused.");
                    break;
                case CoroutineStatus.Finished:
                    BulletStormLogger.LogWarning("Coroutine is finished.");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        ///     Stops the coroutine.
        /// </summary>
        public void Stop()
        {
            Stop(true);
        }

        /// <summary>
        ///     Same as <see cref="Stop" />, but will not send finish callback.
        /// </summary>
        public void Interrupt()
        {
            Stop(false);
        }

        /// <summary>
        ///     Stops the coroutine.
        /// </summary>
        /// <param name="callback">Still send a callback if coroutine finished by this.</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        private void Stop(bool callback)
        {
            switch (Status)
            {
                case CoroutineStatus.Running:
                case CoroutineStatus.Paused:
                    Root.StopCoroutine(_coroutine);
                    Status = CoroutineStatus.Finished;
                    if (callback) OnFinish();
                    break;
                case CoroutineStatus.NotStarted:
                    BulletStormLogger.LogWarning("Coroutine not started.");
                    break;
                case CoroutineStatus.Finished:
                    BulletStormLogger.Log("Coroutine already finished.");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private IEnumerator Wrapper()
        {
            while (true)
                if (Status == CoroutineStatus.Running)
                {
                    if (!(_enumerator is null) && _enumerator.MoveNext()) yield return _enumerator.Current;
                    else break;
                }
                else if (Status == CoroutineStatus.Paused)
                {
                    yield return null;
                }
                else
                {
                    throw new ArgumentOutOfRangeException();
                }

            Status = CoroutineStatus.Finished;
            OnFinish();
        }

        private void OnFinish()
        {
            finish?.Invoke();
        }
    }

    [AddComponentMenu("")]
    internal class CoroutineDaemon : MonoBehaviour
    {
    }

    public enum CoroutineStatus
    {
        NotStarted,
        Running,
        Paused,
        Finished
    }
}