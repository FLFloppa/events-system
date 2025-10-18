namespace FLFloppa.Events
{
    /// <summary>
    /// Provider that creates <see cref="SequentialSubscriptionScheduler{TEvent}"/> instances.
    /// </summary>
    public sealed class DefaultSubscriptionSchedulerProvider : ISubscriptionSchedulerProvider
    {
        /// <summary>
        /// Shared singleton instance of the provider.
        /// </summary>
        public static readonly DefaultSubscriptionSchedulerProvider Instance = new DefaultSubscriptionSchedulerProvider();

        private DefaultSubscriptionSchedulerProvider()
        {
        }

        /// <inheritdoc />
        public ISubscriptionScheduler<TEvent> CreateScheduler<TEvent>() where TEvent : struct
        {
            return new SequentialSubscriptionScheduler<TEvent>();
        }
    }
}
