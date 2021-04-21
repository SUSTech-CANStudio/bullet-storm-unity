using System.Collections.Generic;
using System.Threading.Tasks;

namespace CANStudio.BulletStorm.Core
{
    /// <summary>
    ///     Runtime controller of bullets.
    /// </summary>
    internal sealed class BulletController
    {
        /// <summary>
        ///     Timed actions, tuples of (action, remaining time).
        /// </summary>
        private readonly LinkedList<TimedAction> _actions;

        private readonly IBulletSystemImplementation _bulletSystem;
        private readonly BulletStormContext _context;

        /// <summary>
        ///     Permanent actions.
        /// </summary>
        private readonly List<IBulletAction> _modules;

        public BulletController(IBulletSystemImplementation bulletSystem, BulletStormContext context,
            List<IBulletAction> modules)
        {
            _bulletSystem = bulletSystem;
            _context = context;
            foreach (var action in modules) action.SetContext(context);
            _modules = modules;
            _actions = new LinkedList<TimedAction>();
            context.simulate += Simulate;
        }

        ~BulletController()
        {
            _bulletSystem.Abandon();
            _context.simulate -= Simulate;
        }

        public void ApplyAction(IBulletAction action)
        {
            action.SetContext(_context);
            _modules.Add(action);
        }

        /// <summary>
        ///     Applies an action to this controller with time limitation.
        /// </summary>
        /// <param name="action"></param>
        /// <param name="duration"></param>
        public void ApplyAction(IBulletAction action, float duration)
        {
            action.SetContext(_context);
            _actions.AddLast(new TimedAction {action = action, time = duration});
        }

        private void Simulate(float deltaTime)
        {
            if (_modules.Count == 0 && _actions.Count == 0) return;

            Parallel.For(0, _bulletSystem.BulletCount, i =>
            {
                foreach (var module in _modules) module.UpdateBullet(ref _bulletSystem.Bullet(i), deltaTime);
                foreach (var action in _actions) action.action.UpdateBullet(ref _bulletSystem.Bullet(i), deltaTime);
            });

            // clear finished actions
            for (var current = _actions.First; current != null;)
            {
                var time = current.Value.time;
                time -= deltaTime;
                current.Value = new TimedAction {action = current.Value.action, time = time};
                var temp = current;
                current = current.Next;
                if (time <= 0) _actions.Remove(temp);
            }
        }

        private struct TimedAction
        {
            public IBulletAction action;
            public float time;
        }
    }
}