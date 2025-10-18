namespace FLFloppa.Events
{
    /// <summary>
    /// Represents a subscription entry that wraps a handler, features, and scheduler context.
    /// </summary>
    public struct TopicSubscriber<TEvent> where TEvent : struct
    {
        private EventHandler<TEvent>? _handler;
        private ISubscriptionFeature<TEvent>[]? _features;
        private SubscriptionRuntimeState<TEvent> _runtimeState;
        private SubscriberExecutionContext<TEvent> _context;
        private bool _isActive;

        /// <summary>
        /// Initializes a new instance of the <see cref="TopicSubscriber{TEvent}"/> struct.
        /// </summary>
        /// <param name="handler">Delegate invoked when the topic emits a payload.</param>
        /// <param name="features">Feature array applied to subscription lifecycle events.</param>
        /// <param name="handle">Handle that identifies the subscription.</param>
        /// <param name="schedulerContext">Context provided by the scheduler for issuing commands.</param>
        public TopicSubscriber(EventHandler<TEvent> handler, ISubscriptionFeature<TEvent>[] features, SubscriptionHandle<TEvent> handle, ISubscriptionSchedulerContext<TEvent>? schedulerContext)
        {
            _handler = handler;
            _features = features;
            _runtimeState = new SubscriptionRuntimeState<TEvent>(handle.TopicId, handle, schedulerContext);
            _context = new SubscriberExecutionContext<TEvent>(handle);
            _isActive = true;
        }

        /// <summary>
        /// Gets the handle associated with this subscriber.
        /// </summary>
        public SubscriptionHandle<TEvent> Handle => _context.Handle;
        /// <summary>
        /// Gets a value indicating whether the subscriber is active and has a handler assigned.
        /// </summary>
        public bool IsActive => _isActive && _handler is not null;

        /// <summary>
        /// Executes subscription hooks on the associated features.
        /// </summary>
        /// <param name="topicId">Identifier for the topic being subscribed to.</param>
        public void RunSubscribe(ushort topicId)
        {
            if (_features is null)
            {
                return;
            }

            for (var i = 0; i < _features.Length; i++)
            {
                _features[i]?.OnSubscribe(ref _runtimeState);
            }
        }

        /// <summary>
        /// Executes unsubscription hooks and releases references.
        /// </summary>
        public void RunUnsubscribe()
        {
            if (_features is null)
            {
                return;
            }

            for (var i = 0; i < _features.Length; i++)
            {
                _features[i]?.OnUnsubscribe(ref _runtimeState);
            }

            _handler = null;
            _features = null;
            _isActive = false;
        }

        /// <summary>
        /// Invokes the subscriber handler and runs feature hooks before and after invocation.
        /// </summary>
        /// <param name="payload">Event payload to deliver.</param>
        public void Invoke(in TEvent payload)
        {
            if (_handler is null)
            {
                return;
            }

            if (_features is { Length: > 0 })
            {
                for (var i = 0; i < _features.Length; i++)
                {
                    _features[i]?.BeforeInvoke(payload, ref _runtimeState, ref _context);
                }
            }

            _handler.Invoke(payload);

            if (_features is { Length: > 0 })
            {
                for (var i = _features.Length - 1; i >= 0; i--)
                {
                    _features[i]?.AfterInvoke(payload, ref _runtimeState, ref _context);
                }
            }
        }
    }
}
