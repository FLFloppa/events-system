#nullable enable
using System;

namespace FLFloppa.Events
{
    /// <summary>
    /// Represents a set of dispatch features configured for a specific event type.
    /// </summary>
    public readonly struct DispatchRecipe<TEvent> where TEvent : struct
    {
        private readonly IDispatchFeature<TEvent>[]? _features;
        private readonly bool _initialized;

        private DispatchRecipe(IDispatchFeature<TEvent>[] features)
        {
            _features = features;
            _initialized = true;
        }

        internal ReadOnlySpan<IDispatchFeature<TEvent>> Features => (_features ?? Array.Empty<IDispatchFeature<TEvent>>()).AsSpan();
        /// <summary>
        /// Gets a value indicating whether the recipe has been initialized with features.
        /// </summary>
        public bool IsInitialized => _initialized;

        /// <summary>
        /// Gets an empty recipe with no dispatch features assigned.
        /// </summary>
        public static DispatchRecipe<TEvent> Empty { get; } = new DispatchRecipe<TEvent>(Array.Empty<IDispatchFeature<TEvent>>());

        /// <summary>
        /// Returns the recipe if initialized, otherwise <see cref="Empty"/>.
        /// </summary>
        public DispatchRecipe<TEvent> Normalize() => _initialized ? this : Empty;

        /// <summary>
        /// Creates a recipe from the provided dispatch features.
        /// </summary>
        /// <param name="features">Feature set to attach to the recipe.</param>
        public static DispatchRecipe<TEvent> FromFeatures(params IDispatchFeature<TEvent>[] features)
            => new DispatchRecipe<TEvent>(features ?? Array.Empty<IDispatchFeature<TEvent>>());

        /// <summary>
        /// Creates a recipe using the specified configurator.
        /// </summary>
        /// <param name="configurator">Configurator that produces dispatch recipes.</param>
        /// <returns>Recipe returned by the configurator or <see cref="Empty"/> when unavailable.</returns>
        public static DispatchRecipe<TEvent> FromConfigurator(IDispatchConfigurator? configurator)
            => configurator?.Create<TEvent>() ?? Empty;
    }
}
