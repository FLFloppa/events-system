namespace FLFloppa.Events
{
    /// <summary>
    /// Factory that creates <see cref="ThrottledSubscriptionFeature{TEvent}"/> instances with a configured throttle duration.
    /// </summary>
    public sealed class ThrottledSubscriptionFeatureFactory : ISubscriptionFeatureFactory
    {
        private readonly System.TimeSpan _throttle;
        private readonly bool _logUnsupported;

        /// <summary>
        /// Initializes a new instance of the <see cref="ThrottledSubscriptionFeatureFactory"/> class.
        /// </summary>
        /// <param name="throttle">Throttle duration applied by created features.</param>
        /// <param name="logUnsupported">Whether features log when the scheduler rejects the throttle command.</param>
        public ThrottledSubscriptionFeatureFactory(System.TimeSpan throttle, bool logUnsupported)
        {
            _throttle = throttle;
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
            return new ThrottledSubscriptionFeature<TEvent>(_throttle, _logUnsupported);
        }
    }
}
