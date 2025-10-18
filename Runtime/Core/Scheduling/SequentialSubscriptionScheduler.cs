using System.Collections.Generic;

namespace FLFloppa.Events
{
    /// <summary>
    /// Scheduler that preserves subscription order (FIFO) when invoking handlers.
    /// </summary>
    internal sealed class SequentialSubscriptionScheduler<TEvent> : ISubscriptionScheduler<TEvent> where TEvent : struct
    {
        /// <inheritdoc />
        public ISubscriptionSchedulerContext<TEvent> OnSubscriberAdded(int slot, in SubscriptionHandle<TEvent> handle)
        {
            return NullSubscriptionSchedulerContext<TEvent>.Instance;
        }

        /// <inheritdoc />
        public void OnSubscriberRemoved(int slot)
        {
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
        }

        /// <inheritdoc />
        public void OnSubscriberInvoked(int slot)
        {
        }
    }
}
