namespace FLFloppa.Events
{
    /// <summary>
    /// Defines hooks executed before and after dispatching an event payload.
    /// </summary>
    public interface IDispatchFeature<TEvent> where TEvent : struct
    {
        /// <summary>
        /// Called prior to dispatching the payload to subscribers.
        /// </summary>
        /// <param name="payload">Payload that will be dispatched.</param>
        /// <param name="state">Mutable dispatch state shared across features.</param>
        void BeforeDispatch(in TEvent payload, ref DispatchRuntimeState<TEvent> state);
        /// <summary>
        /// Called after dispatching the payload to subscribers.
        /// </summary>
        /// <param name="payload">Payload that was dispatched.</param>
        /// <param name="state">Mutable dispatch state shared across features.</param>
        void AfterDispatch(in TEvent payload, ref DispatchRuntimeState<TEvent> state);
    }
}
