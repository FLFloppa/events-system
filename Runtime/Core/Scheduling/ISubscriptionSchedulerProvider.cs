namespace FLFloppa.Events
{
    /// <summary>
    /// Provides subscription schedulers for event topics.
    /// </summary>
    public interface ISubscriptionSchedulerProvider
    {
        /// <summary>
        /// Creates a scheduler for the specified event type.
        /// </summary>
        /// <typeparam name="TEvent">Event payload type associated with the scheduler.</typeparam>
        /// <returns>An <see cref="ISubscriptionScheduler{TEvent}"/> instance.</returns>
        ISubscriptionScheduler<TEvent> CreateScheduler<TEvent>() where TEvent : struct;
    }
}
