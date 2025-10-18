namespace FLFloppa.Events
{
    /// <summary>
    /// Factory that creates <see cref="DiagnosticsDispatchFeature{TEvent}"/> instances with predefined logging behavior.
    /// </summary>
    public sealed class DiagnosticsDispatchFeatureFactory : IDispatchFeatureFactory
    {
        private readonly string _label;
        private readonly bool _logBefore;
        private readonly bool _logAfter;

        /// <summary>
        /// Initializes a new instance of the <see cref="DiagnosticsDispatchFeatureFactory"/> class.
        /// </summary>
        /// <param name="label">Label prefixed to diagnostic logs.</param>
        /// <param name="logBefore">Whether generated features log before dispatch.</param>
        /// <param name="logAfter">Whether generated features log after dispatch.</param>
        public DiagnosticsDispatchFeatureFactory(string label, bool logBefore, bool logAfter)
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
        public IDispatchFeature<TEvent> Create<TEvent>() where TEvent : struct
        {
            return new DiagnosticsDispatchFeature<TEvent>(_label, _logBefore, _logAfter);
        }
    }
}
