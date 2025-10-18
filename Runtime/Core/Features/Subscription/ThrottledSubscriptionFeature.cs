using System;
using UnityEngine;

namespace FLFloppa.Events
{
    /// <summary>
    /// Subscription feature that applies a throttle duration to the subscriber when supported by the scheduler.
    /// </summary>
    public readonly struct ThrottledSubscriptionFeature<TEvent> : ISubscriptionFeature<TEvent> where TEvent : struct
    {
        private readonly TimeSpan _throttle;
        private readonly bool _logUnsupported;

        /// <summary>
        /// Initializes a new instance of the <see cref="ThrottledSubscriptionFeature{TEvent}"/> struct.
        /// </summary>
        /// <param name="throttle">Throttle duration to request from the scheduler.</param>
        /// <param name="logUnsupported">Whether to log when the scheduler rejects the throttle command.</param>
        public ThrottledSubscriptionFeature(TimeSpan throttle, bool logUnsupported)
        {
            _throttle = throttle < TimeSpan.Zero ? TimeSpan.Zero : throttle;
            _logUnsupported = logUnsupported;
        }

        /// <inheritdoc />
        public void OnSubscribe(ref SubscriptionRuntimeState<TEvent> state)
        {
            var applied = state.TryApplySchedulerCommand(new SetThrottleSchedulerCommand<TEvent>(_throttle));
            if (!applied && _logUnsupported)
            {
                Debug.LogWarning($"[ThrottledSubscriptionFeature] Scheduler does not support throttle commands for topic {state.TopicId}.");
            }
        }

        /// <inheritdoc />
        public void OnUnsubscribe(ref SubscriptionRuntimeState<TEvent> state)
        {
        }

        /// <inheritdoc />
        public void BeforeInvoke(in TEvent payload, ref SubscriptionRuntimeState<TEvent> state, ref SubscriberExecutionContext<TEvent> context)
        {
        }

        /// <inheritdoc />
        public void AfterInvoke(in TEvent payload, ref SubscriptionRuntimeState<TEvent> state, ref SubscriberExecutionContext<TEvent> context)
        {
        }
    }
}
