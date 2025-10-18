using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace FLFloppa.Events
{
    /// <summary>
    /// Default <see cref="IEventService"/> implementation responsible for managing topics and dispatching payloads.
    /// </summary>
    internal sealed class EventService : IEventService
    {
        private readonly IEventDiagnosticsProvider _diagnostics;
        private readonly IDispatchPolicy _dispatchPolicy;
        private readonly IExecutorStrategy _executorStrategy;
        private readonly ISubscriptionConfigurator _subscriptionConfigurator;
        private readonly IDispatchConfigurator _dispatchConfigurator;
        private readonly ISubscriptionSchedulerProvider _subscriptionSchedulerProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventService"/> class with default policies and configurators.
        /// </summary>
        public EventService()
            : this(
                ImmediateDispatchPolicy.Instance,
                MainThreadExecutorStrategy.Instance,
                NullEventDiagnosticsProvider.Instance,
                new PassthroughSubscriptionConfigurator(),
                new PassthroughDispatchConfigurator(),
                DefaultSubscriptionSchedulerProvider.Instance)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EventService"/> class using the supplied collaborators.
        /// </summary>
        /// <param name="dispatchPolicy">Policy that determines how payloads are dispatched.</param>
        /// <param name="executorStrategy">Executor responsible for running subscriber delegates.</param>
        /// <param name="diagnostics">Optional diagnostics provider for logging and instrumentation.</param>
        /// <param name="subscriptionConfigurator">Configurator that supplies default subscription recipes.</param>
        /// <param name="dispatchConfigurator">Configurator that supplies default dispatch recipes.</param>
        /// <param name="subscriptionSchedulerProvider">Factory supplying schedulers per topic.</param>
        internal EventService(
            IDispatchPolicy dispatchPolicy,
            IExecutorStrategy executorStrategy,
            IEventDiagnosticsProvider? diagnostics = null,
            ISubscriptionConfigurator? subscriptionConfigurator = null,
            IDispatchConfigurator? dispatchConfigurator = null,
            ISubscriptionSchedulerProvider? subscriptionSchedulerProvider = null)
        {
            _dispatchPolicy = dispatchPolicy ?? throw new ArgumentNullException(nameof(dispatchPolicy));
            _executorStrategy = executorStrategy ?? throw new ArgumentNullException(nameof(executorStrategy));
            _diagnostics = diagnostics ?? NullEventDiagnosticsProvider.Instance;
            _subscriptionConfigurator = subscriptionConfigurator ?? new PassthroughSubscriptionConfigurator();
            _dispatchConfigurator = dispatchConfigurator ?? new PassthroughDispatchConfigurator();
            _subscriptionSchedulerProvider = subscriptionSchedulerProvider ?? DefaultSubscriptionSchedulerProvider.Instance;
        }

        /// <inheritdoc />
        public SubscriptionHandle<TEvent> Subscribe<TEvent>(SubscriptionRecipe<TEvent> recipe, EventHandler<TEvent> handler) where TEvent : struct
        {
            if (handler is null) throw new ArgumentNullException(nameof(handler));

            var normalized = recipe.IsInitialized ? recipe : _subscriptionConfigurator.Create<TEvent>();
            normalized = normalized.Normalize();
            var topic = TopicCache<TEvent>.GetOrCreate(_subscriptionSchedulerProvider);
            var features = CloneFeatures(normalized.Features);
            return topic.AddSubscriber(features, handler);
        }

        /// <inheritdoc />
        public void Unsubscribe<TEvent>(SubscriptionHandle<TEvent> handle) where TEvent : struct
        {
            var topic = TopicCache<TEvent>.GetOrCreate(_subscriptionSchedulerProvider);
            topic.RemoveSubscriber(handle);
        }

        /// <inheritdoc />
        public void Publish<TEvent>(in TEvent payload, DispatchRecipe<TEvent> recipe = default) where TEvent : struct
        {
            var topic = TopicCache<TEvent>.GetOrCreate(_subscriptionSchedulerProvider);
            var dispatchRecipe = recipe.IsInitialized ? recipe : _dispatchConfigurator.Create<TEvent>();
            dispatchRecipe = dispatchRecipe.Normalize();
            _dispatchPolicy.Dispatch(topic, payload, dispatchRecipe, _executorStrategy);
        }

        /// <inheritdoc />
        public async UniTask PublishAsync<TEvent>(TEvent payload, DispatchRecipe<TEvent> recipe, CancellationToken cancellationToken = default) where TEvent : struct
        {
            cancellationToken.ThrowIfCancellationRequested();
            Publish(payload, recipe);
            await UniTask.CompletedTask;
        }

        /// <inheritdoc />
        public bool TryGetTopicStats<TEvent>(out EventTopicStats stats) where TEvent : struct
        {
            var topic = TopicCache<TEvent>.GetOrCreate(_subscriptionSchedulerProvider);
            stats = new EventTopicStats(topic.SubscriberCount);
            return topic.HasSubscribers;
        }

        /// <inheritdoc />
        public IEventDiagnosticsProvider Diagnostics => _diagnostics;

        /// <summary>
        /// Clones feature references from a read-only span into an array for topic registration.
        /// </summary>
        /// <typeparam name="TEvent">The struct payload type associated with the features.</typeparam>
        /// <param name="source">Span containing feature references.</param>
        /// <returns>An array that can be stored on the subscriber.</returns>
        private static ISubscriptionFeature<TEvent>[] CloneFeatures<TEvent>(ReadOnlySpan<ISubscriptionFeature<TEvent>> source) where TEvent : struct
        {
            if (source.Length == 0)
            {
                return Array.Empty<ISubscriptionFeature<TEvent>>();
            }

            var array = new ISubscriptionFeature<TEvent>[source.Length];
            for (var i = 0; i < source.Length; i++)
            {
                array[i] = source[i];
            }

            return array;
        }
    }
}
