#nullable enable
using System;

namespace FLFloppa.Events
{
    /// <summary>
    /// Represents a set of subscription features configured for a specific event type.
    /// </summary>
    public readonly struct SubscriptionRecipe<TEvent> where TEvent : struct
    {
        private readonly ISubscriptionFeature<TEvent>[]? _features;
        private readonly SubscriptionKey _key;
        private readonly bool _initialized;

        private SubscriptionRecipe(SubscriptionKey key, ISubscriptionFeature<TEvent>[] features)
        {
            _key = key;
            _features = features;
            _initialized = true;
        }

        /// <summary>
        /// Gets the key identifying the subscription configuration.
        /// </summary>
        public SubscriptionKey Key => _key;

        internal ReadOnlySpan<ISubscriptionFeature<TEvent>> Features => (_features ?? Array.Empty<ISubscriptionFeature<TEvent>>()).AsSpan();
        /// <summary>
        /// Gets a value indicating whether the recipe has been initialized with features.
        /// </summary>
        public bool IsInitialized => _initialized;

        /// <summary>
        /// Gets an empty recipe with no features assigned.
        /// </summary>
        public static SubscriptionRecipe<TEvent> Empty { get; } = new SubscriptionRecipe<TEvent>(default, Array.Empty<ISubscriptionFeature<TEvent>>());

        /// <summary>
        /// Returns the recipe if initialized, otherwise <see cref="Empty"/>.
        /// </summary>
        public SubscriptionRecipe<TEvent> Normalize() => _initialized ? this : Empty;

        /// <summary>
        /// Creates a recipe from the provided features.
        /// </summary>
        /// <param name="key">Subscription key associated with the recipe.</param>
        /// <param name="features">Feature set to attach to the recipe.</param>
        public static SubscriptionRecipe<TEvent> FromFeatures(SubscriptionKey key, params ISubscriptionFeature<TEvent>[] features)
            => new SubscriptionRecipe<TEvent>(key, features ?? Array.Empty<ISubscriptionFeature<TEvent>>());

        /// <summary>
        /// Creates a recipe using the specified configurator.
        /// </summary>
        /// <param name="configurator">Configurator that produces subscription recipes.</param>
        /// <returns>Recipe returned by the configurator or <see cref="Empty"/> when unavailable.</returns>
        public static SubscriptionRecipe<TEvent> FromConfigurator(ISubscriptionConfigurator? configurator) =>
            configurator?.Create<TEvent>() ?? Empty;
    }
}
