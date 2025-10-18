using UnityEngine;

namespace FLFloppa.Events
{
    /// <summary>
    /// Subscription feature that attempts to set the subscriber priority when supported by the scheduler.
    /// </summary>
    public readonly struct PrioritySubscriptionFeature<TEvent> : ISubscriptionFeature<TEvent> where TEvent : struct
    {
        private readonly int _priority;
        private readonly bool _logUnsupported;

        /// <summary>
        /// Initializes a new instance of the <see cref="PrioritySubscriptionFeature{TEvent}"/> struct.
        /// </summary>
        /// <param name="priority">Priority to assign to the subscriber.</param>
        /// <param name="logUnsupported">Whether to log when the scheduler rejects the command.</param>
        public PrioritySubscriptionFeature(int priority, bool logUnsupported)
        {
            _priority = priority;
            _logUnsupported = logUnsupported;
        }

        /// <inheritdoc />
        public void OnSubscribe(ref SubscriptionRuntimeState<TEvent> state)
        {
            var applied = state.TryApplySchedulerCommand(new SetPrioritySchedulerCommand<TEvent>(_priority));
            if (!applied && _logUnsupported)
            {
                Debug.LogWarning($"[PrioritySubscriptionFeature] Scheduler does not support priority commands for topic {state.TopicId}.");
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
