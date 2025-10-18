namespace FLFloppa.Events
{
    /// <summary>
    /// Runtime context provided to subscription features during subscriber invocation.
    /// </summary>
    public readonly struct SubscriberExecutionContext<TEvent> where TEvent : struct
    {
        internal SubscriberExecutionContext(SubscriptionHandle<TEvent> handle)
        {
            Handle = handle;
        }

        /// <summary>
        /// Gets the handle identifying the subscriber being invoked.
        /// </summary>
        public SubscriptionHandle<TEvent> Handle { get; }
    }
}
