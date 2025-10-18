using System;
using System.Collections.Generic;

namespace FLFloppa.Events
{
    /// <summary>
    /// Scheduler that enforces a cooldown window between invocations for each subscriber.
    /// </summary>
    internal sealed class ThrottledSubscriptionScheduler<TEvent> : ISubscriptionScheduler<TEvent> where TEvent : struct
    {
        private readonly List<ThrottleSlot> _slots = new List<ThrottleSlot>();
        private readonly TimeSpan _defaultThrottle;

        /// <summary>
        /// Initializes a new instance of the <see cref="ThrottledSubscriptionScheduler{TEvent}"/> class.
        /// </summary>
        /// <param name="defaultThrottle">Default cooldown applied to new subscribers.</param>
        public ThrottledSubscriptionScheduler(TimeSpan? defaultThrottle = null)
        {
            _defaultThrottle = defaultThrottle.GetValueOrDefault(TimeSpan.Zero);
            if (_defaultThrottle < TimeSpan.Zero)
            {
                _defaultThrottle = TimeSpan.Zero;
            }
        }

        /// <inheritdoc />
        public ISubscriptionSchedulerContext<TEvent> OnSubscriberAdded(int slot, in SubscriptionHandle<TEvent> handle)
        {
            EnsureCapacity(slot);
            _slots[slot] = new ThrottleSlot(TimeSpan.Zero, DateTime.MinValue);
            return new ThrottleSchedulerContext(this, slot);
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
            var now = DateTime.UtcNow;

            for (var i = 0; i < subscribers.Count; i++)
            {
                if (i >= _slots.Count)
                {
                    destination.Add(i);
                    continue;
                }

                var slot = _slots[i];
                if (!slot.IsThrottled(now))
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

            var slotData = _slots[slot];
            if (slotData.Throttle == TimeSpan.Zero)
            {
                return;
            }

            _slots[slot] = new ThrottleSlot(slotData.Throttle, DateTime.UtcNow);
        }

        private void EnsureCapacity(int slot)
        {
            while (_slots.Count <= slot)
            {
                _slots.Add(new ThrottleSlot(_defaultThrottle, DateTime.MinValue));
            }
        }

        private readonly struct ThrottleSlot
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="ThrottleSlot"/> struct.
            /// </summary>
            /// <param name="throttle">Cooldown duration for the slot.</param>
            /// <param name="lastInvoked">Timestamp of the most recent invocation.</param>
            public ThrottleSlot(TimeSpan throttle, DateTime lastInvoked)
            {
                Throttle = throttle < TimeSpan.Zero ? TimeSpan.Zero : throttle;
                LastInvoked = lastInvoked;
            }

            /// <summary>
            /// Gets the throttle duration applied to the slot.
            /// </summary>
            public TimeSpan Throttle { get; }
            /// <summary>
            /// Gets the timestamp of the most recent invocation.
            /// </summary>
            public DateTime LastInvoked { get; }

            /// <summary>
            /// Determines whether the slot is currently throttled based on <paramref name="now"/>.
            /// </summary>
            /// <param name="now">Reference time.</param>
            /// <returns><c>true</c> when the throttle window has not yet elapsed.</returns>
            public bool IsThrottled(DateTime now)
            {
                if (Throttle == TimeSpan.Zero)
                {
                    return false;
                }

                return now - LastInvoked < Throttle;
            }
        }

        private sealed class ThrottleSchedulerContext : IThrottleSchedulerContext<TEvent>
        {
            private readonly ThrottledSubscriptionScheduler<TEvent> _scheduler;
            private readonly int _slot;

            /// <summary>
            /// Initializes a new instance of the <see cref="ThrottleSchedulerContext"/> class.
            /// </summary>
            /// <param name="scheduler">Scheduler managing throttle state.</param>
            /// <param name="slot">Subscriber slot associated with the context.</param>
            public ThrottleSchedulerContext(ThrottledSubscriptionScheduler<TEvent> scheduler, int slot)
            {
                _scheduler = scheduler;
                _slot = slot;
            }

            /// <inheritdoc />
            public bool SetThrottle(TimeSpan duration)
            {
                _scheduler.EnsureCapacity(_slot);
                var clamped = duration < TimeSpan.Zero ? TimeSpan.Zero : duration;
                var current = _scheduler._slots[_slot];
                if (current.Throttle == clamped)
                {
                    return false;
                }

                _scheduler._slots[_slot] = new ThrottleSlot(clamped, current.LastInvoked);
                return true;
            }

            /// <inheritdoc />
            public TimeSpan CurrentThrottle
            {
                get
                {
                    _scheduler.EnsureCapacity(_slot);
                    return _scheduler._slots[_slot].Throttle;
                }
            }
        }
    }
}
