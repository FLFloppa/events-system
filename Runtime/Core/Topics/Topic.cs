#nullable enable
using System;
using System.Collections.Generic;

namespace FLFloppa.Events
{
    /// <summary>
    /// Represents a strongly typed event topic responsible for managing subscribers and dispatching payloads.
    /// </summary>
    public sealed class Topic<TEvent> where TEvent : struct
    {
        private readonly List<TopicSubscriber<TEvent>> _subscribers = new List<TopicSubscriber<TEvent>>();
        private readonly Stack<int> _freeSlots = new Stack<int>();
        private readonly List<int> _executionOrder = new List<int>(32);
        private readonly ISubscriptionScheduler<TEvent> _scheduler;

        /// <summary>
        /// Initializes a new instance of the <see cref="Topic{TEvent}"/> class.
        /// </summary>
        /// <param name="topicId">Unique identifier assigned to the topic.</param>
        /// <param name="scheduler">Optional scheduler responsible for ordering subscriber execution.</param>
        public Topic(ushort topicId, ISubscriptionScheduler<TEvent>? scheduler = null)
        {
            TopicId = topicId;
            _scheduler = scheduler ?? new SequentialSubscriptionScheduler<TEvent>();
        }

        /// <summary>
        /// Gets the unique identifier associated with the topic.
        /// </summary>
        public ushort TopicId { get; }

        /// <summary>
        /// Gets the count of active subscribers.
        /// </summary>
        public int SubscriberCount => _subscribers.Count - _freeSlots.Count;
        /// <summary>
        /// Gets a value indicating whether the topic currently has any active subscribers.
        /// </summary>
        public bool HasSubscribers => SubscriberCount > 0;

        /// <summary>
        /// Adds a subscriber to the topic using the provided features and handler.
        /// </summary>
        /// <param name="features">Subscription features applied to the handler lifecycle.</param>
        /// <param name="handler">Delegate invoked when payloads are dispatched.</param>
        /// <returns>A handle that can later be used to remove the subscriber.</returns>
        public SubscriptionHandle<TEvent> AddSubscriber(ISubscriptionFeature<TEvent>[] features, EventHandler<TEvent> handler)
        {
            var slot = _freeSlots.Count > 0 ? _freeSlots.Pop() : _subscribers.Count;
            var handle = CreateHandle(slot);
            var schedulerContext = _scheduler.OnSubscriberAdded(slot, handle) ?? NullSubscriptionSchedulerContext<TEvent>.Instance;
            var subscriber = new TopicSubscriber<TEvent>(handler, features, handle, schedulerContext);
            subscriber.RunSubscribe(TopicId);
            if (slot < _subscribers.Count)
            {
                _subscribers[slot] = subscriber;
            }
            else
            {
                _subscribers.Add(subscriber);
            }
            return subscriber.Handle;
        }

        /// <summary>
        /// Removes a subscriber from the topic using the provided handle.
        /// </summary>
        /// <param name="handle">Handle that identifies the subscriber to remove.</param>
        public void RemoveSubscriber(SubscriptionHandle<TEvent> handle)
        {
            if (!ValidateHandle(handle, out var index) || index >= _subscribers.Count)
            {
                return;
            }

            var subscriber = _subscribers[index];
            if (!subscriber.IsActive)
            {
                return;
            }

            subscriber.RunUnsubscribe();
            _subscribers[index] = subscriber;
            _freeSlots.Push(index);
            _scheduler.OnSubscriberRemoved(index);
        }

        /// <summary>
        /// Dispatches a payload to all active subscribers using the provided dispatch features.
        /// </summary>
        /// <param name="payload">Payload to broadcast to subscribers.</param>
        /// <param name="dispatchFeatures">Features invoked before and after dispatch.</param>
        public void Publish(in TEvent payload, ReadOnlySpan<IDispatchFeature<TEvent>> dispatchFeatures)
        {
            var dispatchState = new DispatchRuntimeState<TEvent>(TopicId);
            for (var i = 0; i < dispatchFeatures.Length; i++)
            {
                dispatchFeatures[i]?.BeforeDispatch(payload, ref dispatchState);
            }

            _executionOrder.Clear();
            _scheduler.BuildExecutionOrder(_executionOrder, _subscribers);

            for (var i = 0; i < _executionOrder.Count; i++)
            {
                var index = _executionOrder[i];
                if (index < 0 || index >= _subscribers.Count)
                {
                    continue;
                }

                var subscriber = _subscribers[index];
                if (!subscriber.IsActive)
                {
                    continue;
                }

                subscriber.Invoke(payload);
                _subscribers[index] = subscriber;
                _scheduler.OnSubscriberInvoked(index);
            }

            for (var i = dispatchFeatures.Length - 1; i >= 0; i--)
            {
                dispatchFeatures[i]?.AfterDispatch(payload, ref dispatchState);
            }
        }

        /// <summary>
        /// Creates a subscription handle for the specified slot.
        /// </summary>
        /// <param name="slot">Zero-based slot index within the subscriber list.</param>
        /// <returns>A handle referencing the slot and topic.</returns>
        private SubscriptionHandle<TEvent> CreateHandle(int slot)
        {
            return new SubscriptionHandle<TEvent>(TopicId, (uint)(slot + 1));
        }

        /// <summary>
        /// Validates the provided handle and returns the corresponding subscriber index.
        /// </summary>
        /// <param name="handle">Handle to validate.</param>
        /// <param name="index">Outputs the derived subscriber index.</param>
        /// <returns><c>true</c> when the handle is valid for this topic; otherwise <c>false</c>.</returns>
        private bool ValidateHandle(SubscriptionHandle<TEvent> handle, out int index)
        {
            index = (int)handle.Slot - 1;
            return handle.TopicId == TopicId && index >= 0;
        }
    }
}
