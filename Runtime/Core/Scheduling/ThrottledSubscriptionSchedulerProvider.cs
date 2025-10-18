using System;

namespace FLFloppa.Events
{
    /// <summary>
    /// Provider that creates <see cref="ThrottledSubscriptionScheduler{TEvent}"/> instances.
    /// </summary>
    public sealed class ThrottledSubscriptionSchedulerProvider : ISubscriptionSchedulerProvider
    {
        private readonly TimeSpan _defaultThrottle;

        /// <summary>
        /// Initializes a new instance of the <see cref="ThrottledSubscriptionSchedulerProvider"/> class.
        /// </summary>
        /// <param name="defaultThrottle">Default throttle duration applied to new subscribers.</param>
        public ThrottledSubscriptionSchedulerProvider(TimeSpan defaultThrottle)
        {
            _defaultThrottle = defaultThrottle < TimeSpan.Zero ? TimeSpan.Zero : defaultThrottle;
        }

        /// <inheritdoc />
        public ISubscriptionScheduler<TEvent> CreateScheduler<TEvent>() where TEvent : struct
        {
            return new ThrottledSubscriptionScheduler<TEvent>(_defaultThrottle);
        }
    }
}
