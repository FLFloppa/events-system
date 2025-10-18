namespace FLFloppa.Events
{
    /// <summary>
    /// Provider that creates <see cref="BackpressureSubscriptionScheduler{TEvent}"/> instances.
    /// </summary>
    public sealed class BackpressureSubscriptionSchedulerProvider : ISubscriptionSchedulerProvider
    {
        private readonly int _defaultBacklogLimit;

        /// <summary>
        /// Initializes a new instance of the <see cref="BackpressureSubscriptionSchedulerProvider"/> class.
        /// </summary>
        /// <param name="defaultBacklogLimit">Default backlog threshold applied to new subscribers.</param>
        public BackpressureSubscriptionSchedulerProvider(int defaultBacklogLimit)
        {
            _defaultBacklogLimit = defaultBacklogLimit < 0 ? 0 : defaultBacklogLimit;
        }

        /// <inheritdoc />
        public ISubscriptionScheduler<TEvent> CreateScheduler<TEvent>() where TEvent : struct
        {
            return new BackpressureSubscriptionScheduler<TEvent>(_defaultBacklogLimit);
        }
    }
}
