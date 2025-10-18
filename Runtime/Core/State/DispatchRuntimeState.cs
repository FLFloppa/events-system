namespace FLFloppa.Events
{
    /// <summary>
    /// Runtime state exposed to dispatch features when processing a payload.
    /// </summary>
    public readonly struct DispatchRuntimeState<TEvent> where TEvent : struct
    {
        internal DispatchRuntimeState(ushort topicId)
        {
            TopicId = topicId;
        }

        /// <summary>
        /// Gets the identifier of the topic currently dispatching the payload.
        /// </summary>
        public ushort TopicId { get; }
    }
}
