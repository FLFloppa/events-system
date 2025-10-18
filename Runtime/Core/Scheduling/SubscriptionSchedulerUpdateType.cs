namespace FLFloppa.Events
{
    /// <summary>
    /// Singleton context used when a scheduler does not provide a specialized context.
    /// </summary>
    internal sealed class NullSubscriptionSchedulerContext<TEvent> : ISubscriptionSchedulerContext<TEvent> where TEvent : struct
    {
        /// <summary>
        /// Shared instance that can be reused across subscribers.
        /// </summary>
        public static readonly NullSubscriptionSchedulerContext<TEvent> Instance = new NullSubscriptionSchedulerContext<TEvent>();

        private NullSubscriptionSchedulerContext()
        {
        }
    }
}
