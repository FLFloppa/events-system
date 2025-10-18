using System;

namespace FLFloppa.Events
{
    /// <summary>
    /// Factory abstraction for creating dispatch features per event type.
    /// </summary>
    public interface IDispatchFeatureFactory
    {
        /// <summary>
        /// Determines whether the factory supports the specified event type.
        /// </summary>
        /// <param name="eventType">Event payload type to evaluate.</param>
        /// <returns><c>true</c> when the type is supported.</returns>
        bool SupportsEventType(Type eventType);
        /// <summary>
        /// Creates a dispatch feature for the specified event type.
        /// </summary>
        /// <typeparam name="TEvent">Event payload type.</typeparam>
        /// <returns>An instance of <see cref="IDispatchFeature{TEvent}"/>.</returns>
        IDispatchFeature<TEvent> Create<TEvent>() where TEvent : struct;
    }
}
