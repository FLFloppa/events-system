namespace FLFloppa.Events
{
    /// <summary>
    /// Factory that creates <see cref="DiagnosticsSubscriptionFeature{TEvent}"/> instances with predefined logging behavior.
    /// </summary>
    public sealed class DiagnosticsSubscriptionFeatureFactory : ISubscriptionFeatureFactory
    {
        private readonly string _label;
        private readonly bool _logBefore;
        private readonly bool _logAfter;

        /// <summary>
        /// Initializes a new instance of the <see cref="DiagnosticsSubscriptionFeatureFactory"/> class.
        /// </summary>
        /// <param name="label">Label prefixed to diagnostic logs.</param>
        /// <param name="logBefore">Whether generated features log before invocation.</param>
        /// <param name="logAfter">Whether generated features log after invocation.</param>
        public DiagnosticsSubscriptionFeatureFactory(string label, bool logBefore, bool logAfter)
        {
            _label = label;
            _logBefore = logBefore;
            _logAfter = logAfter;
        }

        /// <inheritdoc />
        public bool SupportsEventType(System.Type eventType)
        {
            return true;
        }

        /// <inheritdoc />
        public ISubscriptionFeature<TEvent> Create<TEvent>() where TEvent : struct
        {
            return new DiagnosticsSubscriptionFeature<TEvent>(_label, _logBefore, _logAfter);
        }
    }
}
