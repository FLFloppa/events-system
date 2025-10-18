using System;

namespace FLFloppa.Events
{
    /// <summary>
    /// Scheduler context exposing throttle configuration for subscribers.
    /// </summary>
    internal interface IThrottleSchedulerContext<TEvent> : ISubscriptionSchedulerContext<TEvent> where TEvent : struct
    {
        /// <summary>
        /// Sets the throttle duration applied to the subscriber.
        /// </summary>
        /// <param name="duration">Cooldown duration between invocations.</param>
        /// <returns><c>true</c> when the throttle changed; otherwise <c>false</c>.</returns>
        bool SetThrottle(TimeSpan duration);
        /// <summary>
        /// Gets the current throttle duration applied to the subscriber.
        /// </summary>
        TimeSpan CurrentThrottle { get; }
    }
}
