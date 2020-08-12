using System;
using System.Collections;
using UnityEngine;
using Object = UnityEngine.Object;

namespace BulletStorm.Util
{
    /// <summary>
    /// Provides more APIs than Unity standard coroutine.
    /// </summary>
    public sealed class ControllableCoroutine
    {
        /// <summary>
        /// Status of the coroutine.
        /// </summary>
        public CoroutineStatus Status { get; private set; }

        private static readonly Lazy<MonoBehaviour> Daemon =
            new Lazy<MonoBehaviour>(() =>
            {
                var daemon = Object.Instantiate(new GameObject("CoroutineDaemon")
                    .AddComponent<CoroutineDaemon>());
                Object.DontDestroyOnLoad(daemon);
                return daemon;
            });
        private Coroutine coroutine;
        private readonly IEnumerator enumerator;
        private event Action Finish;

        /// <summary>
        /// Create a coroutine.
        /// </summary>
        /// <param name="coroutine">The coroutine function.</param>
        /// <param name="callback">Callback function on finish.</param>
        public ControllableCoroutine(IEnumerator coroutine, Action callback = null)
        {
            Status = CoroutineStatus.NotStarted;
            enumerator = coroutine;
            Finish = callback;
        }

        /// <summary>
        /// Starts or unpause the coroutine.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public void Start()
        {
            switch (Status)
            {
                case CoroutineStatus.NotStarted:
                    if (!Application.isPlaying)
                    {
                        BulletStormLogger.LogError("Can't start coroutine in editor.");
                        return;
                    }
                    Status = CoroutineStatus.Running;
                    coroutine = Daemon.Value.StartCoroutine(Wrapper());
                    break;
                case CoroutineStatus.Paused:
                    Status = CoroutineStatus.Running;
                    break;
                case CoroutineStatus.Running:
                    BulletStormLogger.Log("Coroutine already started.");
                    break;
                case CoroutineStatus.Finished:
                    BulletStormLogger.LogError("Coroutine is finished. Call 'Restart' if you want to start it again.");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// Pauses the coroutine.
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
        /// Stops the coroutine.
        /// </summary>
        public void Stop() => Stop(true);

        /// <summary>
        /// Same as <see cref="Stop"/>, but will not send finish callback.
        /// </summary>
        public void Interrupt() => Stop(false);
        
        /// <summary>
        /// Stops the coroutine.
        /// </summary>
        /// <param name="callback">Still send a callback if coroutine finished by this.</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        private void Stop(bool callback)
        {
            switch (Status)
            {
                case CoroutineStatus.Running:
                    Daemon.Value.StopCoroutine(coroutine);
                    Status = CoroutineStatus.Finished;
                    if (callback) OnFinish();
                    break;
                case CoroutineStatus.Paused:
                    Daemon.Value.StopCoroutine(coroutine);
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
            {
                if (Status == CoroutineStatus.Running)
                {
                    if (!(enumerator is null) && enumerator.MoveNext()) yield return enumerator.Current;
                    else break;
                }
                else if (Status == CoroutineStatus.Paused) yield return null;
                else throw new ArgumentOutOfRangeException();
            }
            
            Status = CoroutineStatus.Finished;
            OnFinish();
        }

        private void OnFinish() => Finish?.Invoke();
    }

    [AddComponentMenu("")]
    internal class CoroutineDaemon : MonoBehaviour
    {}

    public enum CoroutineStatus
    {
        NotStarted,
        Running,
        Paused,
        Finished
    }
}