namespace FLFloppa.Events
{
    /// <summary>
    /// Defines hooks executed throughout the lifecycle of a subscription and each event invocation.
    /// </summary>
    public interface ISubscriptionFeature<TEvent> where TEvent : struct
    {
        /// <summary>
        /// Called when the subscription is registered.
        /// </summary>
        /// <param name="state">Mutable subscription state shared across features.</param>
        void OnSubscribe(ref SubscriptionRuntimeState<TEvent> state);
        /// <summary>
        /// Called when the subscription is unregistered.
        /// </summary>
        /// <param name="state">Mutable subscription state shared across features.</param>
        void OnUnsubscribe(ref SubscriptionRuntimeState<TEvent> state);
        /// <summary>
        /// Called before invoking the subscriber delegate for a payload.
        /// </summary>
        /// <param name="payload">Payload that will be delivered.</param>
        /// <param name="state">Mutable subscription state shared across features.</param>
        /// <param name="context">Invocation context providing scheduler and handler details.</param>
        void BeforeInvoke(in TEvent payload, ref SubscriptionRuntimeState<TEvent> state, ref SubscriberExecutionContext<TEvent> context);
        /// <summary>
        /// Called after invoking the subscriber delegate for a payload.
        /// </summary>
        /// <param name="payload">Payload that was delivered.</param>
        /// <param name="state">Mutable subscription state shared across features.</param>
        /// <param name="context">Invocation context providing scheduler and handler details.</param>
        void AfterInvoke(in TEvent payload, ref SubscriptionRuntimeState<TEvent> state, ref SubscriberExecutionContext<TEvent> context);
    }
}
