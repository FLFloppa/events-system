using System.Collections.Generic;

namespace FLFloppa.Events
{
    /// <summary>
    /// Scheduler that manages backlog thresholds and pause/resume semantics per subscriber.
    /// </summary>
    internal sealed class BackpressureSubscriptionScheduler<TEvent> : ISubscriptionScheduler<TEvent> where TEvent : struct
    {
        private readonly List<BackpressureSlot> _slots = new List<BackpressureSlot>();
        private readonly int _defaultMaxBacklog;

        /// <summary>
        /// Initializes a new instance of the <see cref="BackpressureSubscriptionScheduler{TEvent}"/> class.
        /// </summary>
        /// <param name="defaultMaxBacklog">Maximum backlog allowed before subscribers are skipped.</param>
        public BackpressureSubscriptionScheduler(int defaultMaxBacklog = 0)
        {
            _defaultMaxBacklog = defaultMaxBacklog < 0 ? 0 : defaultMaxBacklog;
        }

        /// <inheritdoc />
        public ISubscriptionSchedulerContext<TEvent> OnSubscriberAdded(int slot, in SubscriptionHandle<TEvent> handle)
        {
            EnsureCapacity(slot);
            _slots[slot] = new BackpressureSlot(false, 0, _defaultMaxBacklog);
            return new BackpressureSchedulerContext(this, slot);
        }

        /// <inheritdoc />
        public void OnSubscriberRemoved(int slot)
        {
            if (slot < _slots.Count)
            {
                _slots[slot] = default;
            }
        }

        /// <inheritdoc />
        public void BuildExecutionOrder(List<int> destination, List<TopicSubscriber<TEvent>> subscribers)
        {
            if (destination.Capacity < subscribers.Count)
            {
                destination.Capacity = subscribers.Count;
            }

            for (var i = 0; i < subscribers.Count; i++)
            {
                if (i >= _slots.Count)
                {
                    destination.Add(i);
                    continue;
                }

                var slot = _slots[i];
                if (!slot.IsPaused && slot.Backlog <= slot.MaxBacklog)
                {
                    destination.Add(i);
                }
            }
        }

        /// <inheritdoc />
        public void OnSubscriberInvoked(int slot)
        {
            if (slot >= _slots.Count)
            {
                return;
            }

            var data = _slots[slot];
            if (data.Backlog > 0)
            {
                _slots[slot] = new BackpressureSlot(data.IsPaused, data.Backlog - 1, data.MaxBacklog);
            }
        }

        private void EnsureCapacity(int slot)
        {
            while (_slots.Count <= slot)
            {
                _slots.Add(new BackpressureSlot(false, 0, _defaultMaxBacklog));
            }
        }

        private readonly struct BackpressureSlot
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="BackpressureSlot"/> struct.
            /// </summary>
            /// <param name="isPaused">Whether the subscriber is currently paused.</param>
            /// <param name="backlog">Number of queued payloads for the subscriber.</param>
            /// <param name="maxBacklog">Maximum backlog allowed before pausing.</param>
            public BackpressureSlot(bool isPaused, int backlog, int maxBacklog)
            {
                IsPaused = isPaused;
                Backlog = backlog < 0 ? 0 : backlog;
                MaxBacklog = maxBacklog < 0 ? 0 : maxBacklog;
            }

            /// <summary>
            /// Gets a value indicating whether the subscriber is paused.
            /// </summary>
            public bool IsPaused { get; }
            /// <summary>
            /// Gets the current backlog count.
            /// </summary>
            public int Backlog { get; }
            /// <summary>
            /// Gets the maximum backlog allowed.
            /// </summary>
            public int MaxBacklog { get; }
        }

        private sealed class BackpressureSchedulerContext : IBackpressureSchedulerContext<TEvent>
        {
            private readonly BackpressureSubscriptionScheduler<TEvent> _scheduler;
            private readonly int _slot;

            /// <summary>
            /// Initializes a new instance of the <see cref="BackpressureSchedulerContext"/> class.
            /// </summary>
            /// <param name="scheduler">Scheduler managing backlog state.</param>
            /// <param name="slot">Subscriber slot associated with the context.</param>
            public BackpressureSchedulerContext(BackpressureSubscriptionScheduler<TEvent> scheduler, int slot)
            {
                _scheduler = scheduler;
                _slot = slot;
            }

            /// <inheritdoc />
            public bool Pause()
            {
                return UpdateSlot(state => new BackpressureSlot(true, state.Backlog, state.MaxBacklog), state => state.IsPaused);
            }

            /// <inheritdoc />
            public bool Resume()
            {
                return UpdateSlot(state => new BackpressureSlot(false, state.Backlog, state.MaxBacklog), state => !state.IsPaused);
            }

            /// <inheritdoc />
            public bool SetBacklogLimit(int value)
            {
                var clamped = value < 0 ? 0 : value;
                return UpdateSlot(state => new BackpressureSlot(state.IsPaused, state.Backlog, clamped), state => state.MaxBacklog == clamped);
            }

            /// <inheritdoc />
            public bool IncrementBacklog()
            {
                return UpdateSlot(state => new BackpressureSlot(state.IsPaused, state.Backlog + 1, state.MaxBacklog), state => false);
            }

            /// <inheritdoc />
            public void ResetBacklog()
            {
                _scheduler.EnsureCapacity(_slot);
                var current = _scheduler._slots[_slot];
                _scheduler._slots[_slot] = new BackpressureSlot(current.IsPaused, 0, current.MaxBacklog);
            }

            /// <inheritdoc />
            public bool IsPaused
            {
                get
                {
                    _scheduler.EnsureCapacity(_slot);
                    return _scheduler._slots[_slot].IsPaused;
                }
            }

            /// <inheritdoc />
            public int Backlog
            {
                get
                {
                    _scheduler.EnsureCapacity(_slot);
                    return _scheduler._slots[_slot].Backlog;
                }
            }

            /// <inheritdoc />
            public int MaxBacklog
            {
                get
                {
                    _scheduler.EnsureCapacity(_slot);
                    return _scheduler._slots[_slot].MaxBacklog;
                }
            }

            private bool UpdateSlot(System.Func<BackpressureSlot, BackpressureSlot> updater, System.Func<BackpressureSlot, bool> isNoOp)
            {
                _scheduler.EnsureCapacity(_slot);
                var state = _scheduler._slots[_slot];
                if (isNoOp(state))
                {
                    return false;
                }

                var updated = updater(state);
                _scheduler._slots[_slot] = updated;
                return true;
            }
        }
    }
}
