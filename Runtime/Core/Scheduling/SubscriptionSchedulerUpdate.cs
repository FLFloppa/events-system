using System;

namespace FLFloppa.Events
{
    /// <summary>
    /// Adjusts the subscriber priority by a relative delta.
    /// </summary>
    public sealed class AdjustPrioritySchedulerCommand<TEvent> : ISubscriptionSchedulerCommand<TEvent> where TEvent : struct
    {
        private readonly int _delta;

        /// <summary>
        /// Initializes a new instance of the <see cref="AdjustPrioritySchedulerCommand{TEvent}"/> class.
        /// </summary>
        /// <param name="delta">Amount to add to the current priority.</param>
        public AdjustPrioritySchedulerCommand(int delta)
        {
            _delta = delta;
        }

        /// <inheritdoc />
        public bool TryApply(ISubscriptionSchedulerContext<TEvent> context)
        {
            return context is IPrioritySchedulerContext<TEvent> priorityContext && priorityContext.AdjustPriority(_delta);
        }
    }

    /// <summary>
    /// Sets the subscriber priority to an absolute value.
    /// </summary>
    public sealed class SetPrioritySchedulerCommand<TEvent> : ISubscriptionSchedulerCommand<TEvent> where TEvent : struct
    {
        private readonly int _value;

        /// <summary>
        /// Initializes a new instance of the <see cref="SetPrioritySchedulerCommand{TEvent}"/> class.
        /// </summary>
        /// <param name="value">Priority value to assign.</param>
        public SetPrioritySchedulerCommand(int value)
        {
            _value = value;
        }

        /// <inheritdoc />
        public bool TryApply(ISubscriptionSchedulerContext<TEvent> context)
        {
            return context is IPrioritySchedulerContext<TEvent> priorityContext && priorityContext.SetPriority(_value);
        }
    }

    /// <summary>
    /// Sets the throttle duration for a subscriber.
    /// </summary>
    public sealed class SetThrottleSchedulerCommand<TEvent> : ISubscriptionSchedulerCommand<TEvent> where TEvent : struct
    {
        private readonly TimeSpan _duration;

        /// <summary>
        /// Initializes a new instance of the <see cref="SetThrottleSchedulerCommand{TEvent}"/> class.
        /// </summary>
        /// <param name="duration">Throttle duration between invocations.</param>
        public SetThrottleSchedulerCommand(TimeSpan duration)
        {
            _duration = duration < TimeSpan.Zero ? TimeSpan.Zero : duration;
        }

        /// <inheritdoc />
        public bool TryApply(ISubscriptionSchedulerContext<TEvent> context)
        {
            return context is IThrottleSchedulerContext<TEvent> throttleContext && throttleContext.SetThrottle(_duration);
        }
    }

    /// <summary>
    /// Pauses a subscriber managed by a backpressure scheduler.
    /// </summary>
    public sealed class PauseSubscriptionSchedulerCommand<TEvent> : ISubscriptionSchedulerCommand<TEvent> where TEvent : struct
    {
        /// <inheritdoc />
        public bool TryApply(ISubscriptionSchedulerContext<TEvent> context)
        {
            return context is IBackpressureSchedulerContext<TEvent> backpressureContext && backpressureContext.Pause();
        }
    }

    /// <summary>
    /// Resumes a subscriber previously paused by a backpressure scheduler.
    /// </summary>
    public sealed class ResumeSubscriptionSchedulerCommand<TEvent> : ISubscriptionSchedulerCommand<TEvent> where TEvent : struct
    {
        /// <inheritdoc />
        public bool TryApply(ISubscriptionSchedulerContext<TEvent> context)
        {
            return context is IBackpressureSchedulerContext<TEvent> backpressureContext && backpressureContext.Resume();
        }
    }

    /// <summary>
    /// Sets the backlog limit for a subscriber managed by a backpressure scheduler.
    /// </summary>
    public sealed class SetBacklogLimitSchedulerCommand<TEvent> : ISubscriptionSchedulerCommand<TEvent> where TEvent : struct
    {
        private readonly int _value;

        /// <summary>
        /// Initializes a new instance of the <see cref="SetBacklogLimitSchedulerCommand{TEvent}"/> class.
        /// </summary>
        /// <param name="value">Maximum backlog allowed.</param>
        public SetBacklogLimitSchedulerCommand(int value)
        {
            _value = value < 0 ? 0 : value;
        }

        /// <inheritdoc />
        public bool TryApply(ISubscriptionSchedulerContext<TEvent> context)
        {
            return context is IBackpressureSchedulerContext<TEvent> backpressureContext && backpressureContext.SetBacklogLimit(_value);
        }
    }

    /// <summary>
    /// Increments the backlog counter for a backpressure-managed subscriber.
    /// </summary>
    public sealed class IncrementBacklogSchedulerCommand<TEvent> : ISubscriptionSchedulerCommand<TEvent> where TEvent : struct
    {
        /// <inheritdoc />
        public bool TryApply(ISubscriptionSchedulerContext<TEvent> context)
        {
            return context is IBackpressureSchedulerContext<TEvent> backpressureContext && backpressureContext.IncrementBacklog();
        }
    }

    /// <summary>
    /// Resets the backlog counter for a backpressure-managed subscriber.
    /// </summary>
    public sealed class ResetBacklogSchedulerCommand<TEvent> : ISubscriptionSchedulerCommand<TEvent> where TEvent : struct
    {
        /// <inheritdoc />
        public bool TryApply(ISubscriptionSchedulerContext<TEvent> context)
        {
            if (context is IBackpressureSchedulerContext<TEvent> backpressureContext)
            {
                backpressureContext.ResetBacklog();
                return true;
            }

            return false;
        }
    }
}
