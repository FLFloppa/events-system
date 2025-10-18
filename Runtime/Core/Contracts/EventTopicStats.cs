namespace FLFloppa.Events
{
    /// <summary>
    /// Provides diagnostic information about a topic managed by the event service.
    /// </summary>
    public readonly struct EventTopicStats
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EventTopicStats"/> struct.
        /// </summary>
        /// <param name="subscriberCount">Number of active subscribers registered with the topic.</param>
        public EventTopicStats(int subscriberCount)
        {
            SubscriberCount = subscriberCount;
        }

        /// <summary>
        /// Gets the number of active subscribers for the topic.
        /// </summary>
        public int SubscriberCount { get; }
    }
}
