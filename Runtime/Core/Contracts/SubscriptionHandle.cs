namespace FLFloppa.Events
{
    /// <summary>
    /// Identifies a subscription registered with an <see cref="IEventService"/>.
    /// </summary>
    public readonly struct SubscriptionHandle<TEvent> where TEvent : struct
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SubscriptionHandle{TEvent}"/> struct.
        /// </summary>
        /// <param name="topicId">Identifier of the topic containing the subscription.</param>
        /// <param name="slot">Slot within the topic's subscriber list.</param>
        internal SubscriptionHandle(ushort topicId, uint slot)
        {
            TopicId = topicId;
            Slot = slot;
        }

        /// <summary>
        /// Gets the identifier of the topic associated with the subscription.
        /// </summary>
        public ushort TopicId { get; }
        /// <summary>
        /// Gets the slot index assigned to the subscription.
        /// </summary>
        public uint Slot { get; }

        /// <summary>
        /// Gets a value indicating whether the handle references a valid subscription.
        /// </summary>
        public bool IsValid => TopicId != 0 || Slot != 0;
    }
}
