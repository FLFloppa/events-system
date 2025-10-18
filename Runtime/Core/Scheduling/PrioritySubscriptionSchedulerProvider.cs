namespace FLFloppa.Events
{
    /// <summary>
    /// Provider that creates <see cref="PrioritySubscriptionScheduler{TEvent}"/> instances.
    /// </summary>
    public sealed class PrioritySubscriptionSchedulerProvider : ISubscriptionSchedulerProvider
    {
        private readonly int _defaultPriority;

        /// <summary>
        /// Initializes a new instance of the <see cref="PrioritySubscriptionSchedulerProvider"/> class.
        /// </summary>
        /// <param name="defaultPriority">Priority assigned to new subscribers.</param>
        public PrioritySubscriptionSchedulerProvider(int defaultPriority)
        {
            _defaultPriority = defaultPriority;
        }

        /// <inheritdoc />
        public ISubscriptionScheduler<TEvent> CreateScheduler<TEvent>() where TEvent : struct
        {
            return new PrioritySubscriptionScheduler<TEvent>(_defaultPriority);
        }
    }
}
