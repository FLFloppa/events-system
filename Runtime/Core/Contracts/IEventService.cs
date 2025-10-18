using System.Threading;
using Cysharp.Threading.Tasks;

namespace FLFloppa.Events
{
    /// <summary>
    /// Defines the contract for publishing, subscribing, and monitoring event topics.
    /// </summary>
    public interface IEventService
    {
        /// <summary>
        /// Registers a handler for the specified event type using the provided subscription recipe.
        /// </summary>
        /// <typeparam name="TEvent">The struct payload type associated with the topic.</typeparam>
        /// <param name="recipe">The subscription recipe describing features and policies.</param>
        /// <param name="handler">Delegate invoked when the topic dispatches a payload.</param>
        /// <returns>A handle that can be used to unsubscribe later.</returns>
        SubscriptionHandle<TEvent> Subscribe<TEvent>(SubscriptionRecipe<TEvent> recipe, EventHandler<TEvent> handler) where TEvent : struct;
        /// <summary>
        /// Unregisters a previously subscribed handler.
        /// </summary>
        /// <typeparam name="TEvent">The struct payload type associated with the handle.</typeparam>
        /// <param name="handle">The subscription handle returned by <see cref="Subscribe"/>.</param>
        void Unsubscribe<TEvent>(SubscriptionHandle<TEvent> handle) where TEvent : struct;
        /// <summary>
        /// Publishes a payload synchronously to all subscribers of the topic.
        /// </summary>
        /// <typeparam name="TEvent">The struct payload type being dispatched.</typeparam>
        /// <param name="payload">The payload instance to broadcast.</param>
        /// <param name="recipe">Optional dispatch recipe overriding default behaviour.</param>
        void Publish<TEvent>(in TEvent payload, DispatchRecipe<TEvent> recipe = default) where TEvent : struct;
        /// <summary>
        /// Publishes a payload asynchronously, respecting the provided cancellation token.
        /// </summary>
        /// <typeparam name="TEvent">The struct payload type being dispatched.</typeparam>
        /// <param name="payload">The payload instance to broadcast.</param>
        /// <param name="recipe">Dispatch configuration overriding defaults.</param>
        /// <param name="cancellationToken">Token used to abort the asynchronous operation.</param>
        /// <returns>A UniTask that completes once the dispatch work is scheduled.</returns>
        UniTask PublishAsync<TEvent>(TEvent payload, DispatchRecipe<TEvent> recipe, CancellationToken cancellationToken = default) where TEvent : struct;
        /// <summary>
        /// Retrieves statistics about a topic if it exists and has active subscribers.
        /// </summary>
        /// <typeparam name="TEvent">The struct payload type associated with the topic.</typeparam>
        /// <param name="stats">Outputs the topic statistics.</param>
        /// <returns><c>true</c> when the topic exists and has subscribers; otherwise <c>false</c>.</returns>
        bool TryGetTopicStats<TEvent>(out EventTopicStats stats) where TEvent : struct;
        /// <summary>
        /// Provides access to diagnostics hooks for logging and instrumentation.
        /// </summary>
        IEventDiagnosticsProvider Diagnostics { get; }
    }
}
