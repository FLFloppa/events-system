namespace FLFloppa.Events
{
    /// <summary>
    /// Provider that creates <see cref="SequentialSubscriptionScheduler{TEvent}"/> instances.
    /// </summary>
    public sealed class SequentialSubscriptionSchedulerProvider : ISubscriptionSchedulerProvider
    {
        /// <inheritdoc />
        public ISubscriptionScheduler<TEvent> CreateScheduler<TEvent>() where TEvent : struct
        {
            return new SequentialSubscriptionScheduler<TEvent>();
        }
    }
}
