namespace FLFloppa.Events
{
    /// <summary>
    /// Factory that creates <see cref="PrioritySubscriptionFeature{TEvent}"/> instances with a configured priority.
    /// </summary>
    public sealed class PrioritySubscriptionFeatureFactory : ISubscriptionFeatureFactory
    {
        private readonly int _priority;
        private readonly bool _logUnsupported;

        /// <summary>
        /// Initializes a new instance of the <see cref="PrioritySubscriptionFeatureFactory"/> class.
        /// </summary>
        /// <param name="priority">Priority applied by created features.</param>
        /// <param name="logUnsupported">Whether features log when the scheduler rejects the priority command.</param>
        public PrioritySubscriptionFeatureFactory(int priority, bool logUnsupported)
        {
            _priority = priority;
            _logUnsupported = logUnsupported;
        }

        /// <inheritdoc />
        public bool SupportsEventType(System.Type eventType)
        {
            return true;
        }

        /// <inheritdoc />
        public ISubscriptionFeature<TEvent> Create<TEvent>() where TEvent : struct
        {
            return new PrioritySubscriptionFeature<TEvent>(_priority, _logUnsupported);
        }
    }
}
