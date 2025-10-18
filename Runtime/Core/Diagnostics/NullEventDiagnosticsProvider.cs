namespace FLFloppa.Events
{
    /// <summary>
    /// Diagnostics provider that performs no operations.
    /// </summary>
    public sealed class NullEventDiagnosticsProvider : IEventDiagnosticsProvider
    {
        /// <summary>
        /// Singleton instance of the null diagnostics provider.
        /// </summary>
        public static IEventDiagnosticsProvider Instance { get; } = new NullEventDiagnosticsProvider();

        private NullEventDiagnosticsProvider()
        {
        }
    }
}
