using System;

namespace FLFloppa.Events
{
    /// <summary>
    /// Defines how subscriber delegates are executed for dispatched payloads.
    /// </summary>
    public interface IExecutorStrategy
    {
        /// <summary>
        /// Executes all subscribers associated with the topic using the provided dispatch features.
        /// </summary>
        /// <typeparam name="TEvent">Event payload type.</typeparam>
        /// <param name="topic">Topic containing subscribers.</param>
        /// <param name="payload">Payload to deliver.</param>
        /// <param name="dispatchFeatures">Dispatch features to run before/after invocation.</param>
        void Execute<TEvent>(Topic<TEvent> topic, in TEvent payload, ReadOnlySpan<IDispatchFeature<TEvent>> dispatchFeatures) where TEvent : struct;
    }
}
