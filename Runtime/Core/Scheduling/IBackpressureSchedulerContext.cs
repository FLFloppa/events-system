namespace FLFloppa.Events
{
    /// <summary>
    /// Scheduler context exposing backpressure operations for subscribers.
    /// </summary>
    internal interface IBackpressureSchedulerContext<TEvent> : ISubscriptionSchedulerContext<TEvent> where TEvent : struct
    {
        /// <summary>
        /// Pauses the subscriber from receiving further payloads.
        /// </summary>
        /// <returns><c>true</c> when state changed; otherwise <c>false</c>.</returns>
        bool Pause();
        /// <summary>
        /// Resumes the subscriber if previously paused.
        /// </summary>
        /// <returns><c>true</c> when state changed; otherwise <c>false</c>.</returns>
        bool Resume();
        /// <summary>
        /// Sets the maximum backlog allowed for the subscriber.
        /// </summary>
        /// <param name="value">Maximum queued payload count.</param>
        /// <returns><c>true</c> when the limit changed; otherwise <c>false</c>.</returns>
        bool SetBacklogLimit(int value);
        /// <summary>
        /// Increments the backlog counter for the subscriber.
        /// </summary>
        /// <returns><c>true</c> when the backlog was incremented.</returns>
        bool IncrementBacklog();
        /// <summary>
        /// Resets the backlog counter to zero.
        /// </summary>
        void ResetBacklog();
        /// <summary>
        /// Gets a value indicating whether the subscriber is currently paused.
        /// </summary>
        bool IsPaused { get; }
        /// <summary>
        /// Gets the current backlog count for the subscriber.
        /// </summary>
        int Backlog { get; }
        /// <summary>
        /// Gets the maximum backlog threshold for the subscriber.
        /// </summary>
        int MaxBacklog { get; }
    }
}
