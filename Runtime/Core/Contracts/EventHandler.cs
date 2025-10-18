using System;

namespace FLFloppa.Events
{
    /// <summary>
    /// Represents a method that handles event payloads dispatched by an <see cref="IEventService"/>.
    /// </summary>
    /// <param name="payload">Payload instance delivered to the subscriber.</param>
    public delegate void EventHandler<TEvent>(in TEvent payload) where TEvent : struct;
}
