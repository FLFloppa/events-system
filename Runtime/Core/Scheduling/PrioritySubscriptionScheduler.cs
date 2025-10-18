using System;
using System.Collections.Generic;

namespace FLFloppa.Events
{
    /// <summary>
    /// Scheduler that orders subscribers by priority, falling back to slot order for ties.
    /// </summary>
    internal sealed class PrioritySubscriptionScheduler<TEvent> : ISubscriptionScheduler<TEvent> where TEvent : struct
    {
        private readonly List<int> _priorities = new List<int>();
        private readonly int _defaultPriority;

        /// <summary>
        /// Initializes a new instance of the <see cref="PrioritySubscriptionScheduler{TEvent}"/> class.
        /// </summary>
        /// <param name="defaultPriority">Priority assigned to new subscribers when none is specified.</param>
        public PrioritySubscriptionScheduler(int defaultPriority = 0)
        {
            _defaultPriority = defaultPriority;
        }

        /// <inheritdoc />
        public ISubscriptionSchedulerContext<TEvent> OnSubscriberAdded(int slot, in SubscriptionHandle<TEvent> handle)
        {
            EnsureCapacity(slot);
            _priorities[slot] = _defaultPriority;
            return new PrioritySchedulerContext(this, slot);
        }

        /// <inheritdoc />
        public void OnSubscriberRemoved(int slot)
        {
            if (slot < _priorities.Count)
            {
                _priorities[slot] = int.MinValue;
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
                destination.Add(i);
            }

            destination.Sort(CompareSlots);
        }

        /// <inheritdoc />
        public void OnSubscriberInvoked(int slot)
        {
        }

        private int CompareSlots(int left, int right)
        {
            var leftPriority = GetPriority(left);
            var rightPriority = GetPriority(right);
            var comparison = rightPriority.CompareTo(leftPriority);
            return comparison != 0 ? comparison : left.CompareTo(right);
        }

        private int GetPriority(int slot)
        {
            return slot < _priorities.Count ? _priorities[slot] : int.MinValue;
        }

        private void EnsureCapacity(int slot)
        {
            while (_priorities.Count <= slot)
            {
                _priorities.Add(_defaultPriority);
            }
        }

        private sealed class PrioritySchedulerContext : IPrioritySchedulerContext<TEvent>
        {
            private readonly PrioritySubscriptionScheduler<TEvent> _scheduler;
            private readonly int _slot;

            /// <summary>
            /// Initializes a new instance of the <see cref="PrioritySchedulerContext"/> class.
            /// </summary>
            /// <param name="scheduler">Scheduler managing priority state.</param>
            /// <param name="slot">Subscriber slot associated with the context.</param>
            public PrioritySchedulerContext(PrioritySubscriptionScheduler<TEvent> scheduler, int slot)
            {
                _scheduler = scheduler;
                _slot = slot;
            }

            /// <inheritdoc />
            public bool AdjustPriority(int delta)
            {
                _scheduler.EnsureCapacity(_slot);
                var current = _scheduler._priorities[_slot];
                var updated = current + delta;
                _scheduler._priorities[_slot] = updated;
                return updated != current;
            }

            /// <inheritdoc />
            public bool SetPriority(int value)
            {
                _scheduler.EnsureCapacity(_slot);
                var current = _scheduler._priorities[_slot];
                if (current == value)
                {
                    return false;
                }

                _scheduler._priorities[_slot] = value;
                return true;
            }

            /// <inheritdoc />
            public int CurrentPriority
            {
                get
                {
                    _scheduler.EnsureCapacity(_slot);
                    return _scheduler._priorities[_slot];
                }
            }
        }
    }
}
