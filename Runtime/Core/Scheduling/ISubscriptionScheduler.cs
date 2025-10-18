using System.Collections.Generic;

namespace FLFloppa.Events
{
    /// <summary>
    /// Provides hooks for subscription schedulers that control invocation order and context.
    /// </summary>
    public interface ISubscriptionScheduler<TEvent> where TEvent : struct
    {
        /// <summary>
        /// Called when a subscriber is added, allowing the scheduler to return a context object.
        /// </summary>
        /// <param name="slot">Slot index allocated to the subscriber.</param>
        /// <param name="handle">Subscription handle backing the subscriber.</param>
        /// <returns>A scheduler-specific context or <c>null</c> if not required.</returns>
        ISubscriptionSchedulerContext<TEvent> OnSubscriberAdded(int slot, in SubscriptionHandle<TEvent> handle);
        /// <summary>
        /// Notifies the scheduler that a subscriber has been removed.
        /// </summary>
        /// <param name="slot">Slot index previously holding the subscriber.</param>
        void OnSubscriberRemoved(int slot);
        /// <summary>
        /// Builds the execution order used when dispatching payloads.
        /// </summary>
        /// <param name="destination">List to populate with subscriber indices.</param>
        /// <param name="subscribers">Current snapshot of subscribers.</param>
        void BuildExecutionOrder(List<int> destination, List<TopicSubscriber<TEvent>> subscribers);
        /// <summary>
        /// Called after a subscriber has been invoked, providing an opportunity to update state.
        /// </summary>
        /// <param name="slot">Slot index of the invoked subscriber.</param>
        void OnSubscriberInvoked(int slot);
    }
}
