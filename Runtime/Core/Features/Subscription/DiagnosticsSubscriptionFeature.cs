using UnityEngine;

namespace FLFloppa.Events
{
    /// <summary>
    /// Subscription feature that logs subscription lifecycle and invocation events for diagnostics.
    /// </summary>
    public readonly struct DiagnosticsSubscriptionFeature<TEvent> : ISubscriptionFeature<TEvent> where TEvent : struct
    {
        private readonly string _label;
        private readonly bool _logBefore;
        private readonly bool _logAfter;

        /// <summary>
        /// Initializes a new instance of the <see cref="DiagnosticsSubscriptionFeature{TEvent}"/> struct.
        /// </summary>
        /// <param name="label">Label prefixed to diagnostic logs.</param>
        /// <param name="logBefore">Whether to log before handler invocation.</param>
        /// <param name="logAfter">Whether to log after handler invocation.</param>
        public DiagnosticsSubscriptionFeature(string label, bool logBefore, bool logAfter)
        {
            _label = label;
            _logBefore = logBefore;
            _logAfter = logAfter;
        }

        /// <inheritdoc />
        public void OnSubscribe(ref SubscriptionRuntimeState<TEvent> state)
        {
            Debug.Log($"[{_label}] Subscribed handler {state.Handle.Slot} to topic {state.TopicId}");
        }

        /// <inheritdoc />
        public void OnUnsubscribe(ref SubscriptionRuntimeState<TEvent> state)
        {
            Debug.Log($"[{_label}] Unsubscribed handler {state.Handle.Slot} from topic {state.TopicId}");
        }

        /// <inheritdoc />
        public void BeforeInvoke(in TEvent payload, ref SubscriptionRuntimeState<TEvent> state, ref SubscriberExecutionContext<TEvent> context)
        {
            if (_logBefore)
            {
                Debug.Log($"[{_label}] Before invoke topic {state.TopicId} handle {context.Handle.Slot}");
            }
        }

        /// <inheritdoc />
        public void AfterInvoke(in TEvent payload, ref SubscriptionRuntimeState<TEvent> state, ref SubscriberExecutionContext<TEvent> context)
        {
            if (_logAfter)
            {
                Debug.Log($"[{_label}] After invoke topic {state.TopicId} handle {context.Handle.Slot}");
            }
        }
    }
}
