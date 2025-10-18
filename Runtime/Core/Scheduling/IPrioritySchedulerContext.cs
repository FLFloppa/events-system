namespace FLFloppa.Events
{
    /// <summary>
    /// Scheduler context exposing priority adjustment operations for subscribers.
    /// </summary>
    internal interface IPrioritySchedulerContext<TEvent> : ISubscriptionSchedulerContext<TEvent> where TEvent : struct
    {
        /// <summary>
        /// Adjusts the current priority by the specified delta.
        /// </summary>
        /// <param name="delta">Amount to add to the existing priority.</param>
        /// <returns><c>true</c> when the priority changed; otherwise <c>false</c>.</returns>
        bool AdjustPriority(int delta);
        /// <summary>
        /// Sets the current priority to the provided value.
        /// </summary>
        /// <param name="value">Priority value to assign.</param>
        /// <returns><c>true</c> when the priority changed; otherwise <c>false</c>.</returns>
        bool SetPriority(int value);
        /// <summary>
        /// Gets the priority associated with the subscriber.
        /// </summary>
        int CurrentPriority { get; }
    }
}
