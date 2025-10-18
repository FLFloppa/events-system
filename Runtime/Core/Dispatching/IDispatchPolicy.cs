namespace FLFloppa.Events
{
    /// <summary>
    /// Defines how event payloads are dispatched to topic subscribers.
    /// </summary>
    public interface IDispatchPolicy
    {
        /// <summary>
        /// Dispatches the payload to subscribers using the provided recipe and executor.
        /// </summary>
        /// <typeparam name="TEvent">Event payload type.</typeparam>
        /// <param name="topic">Topic containing subscribers.</param>
        /// <param name="payload">Payload to dispatch.</param>
        /// <param name="recipe">Dispatch recipe specifying feature hooks.</param>
        /// <param name="executor">Executor responsible for invoking subscribers.</param>
        void Dispatch<TEvent>(Topic<TEvent> topic, in TEvent payload, DispatchRecipe<TEvent> recipe, IExecutorStrategy executor) where TEvent : struct;
    }
}
