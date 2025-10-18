using System;
using System.Buffers;
using System.Collections.Generic;

namespace FLFloppa.Events
{
    /// <summary>
    /// Dispatch configurator that assembles features from a list of factories and bindings.
    /// </summary>
    public sealed class FeatureListDispatchConfigurator : IDispatchConfigurator
    {
        private readonly IDispatchFeatureFactory[] _factories;
        private readonly DispatchFeatureBinding[] _bindings;

        /// <summary>
        /// Initializes a new instance of the <see cref="FeatureListDispatchConfigurator"/> class.
        /// </summary>
        /// <param name="factories">Factories capable of producing dispatch features.</param>
        /// <param name="bindings">Binding metadata indicating which features are enabled.</param>
        public FeatureListDispatchConfigurator(
            IDispatchFeatureFactory[] factories,
            DispatchFeatureBinding[] bindings)
        {
            _factories = factories ?? Array.Empty<IDispatchFeatureFactory>();
            _bindings = bindings ?? Array.Empty<DispatchFeatureBinding>();
        }

        /// <summary>
        /// Creates a dispatch recipe by instantiating supported features for the target event type.
        /// </summary>
        /// <typeparam name="TEvent">Event payload type associated with the dispatch.</typeparam>
        /// <returns>A populated recipe when features are available; otherwise <see cref="DispatchRecipe{TEvent}.Empty"/>.</returns>
        public DispatchRecipe<TEvent> Create<TEvent>() where TEvent : struct
        {
            if (_factories.Length == 0)
            {
                return DispatchRecipe<TEvent>.Empty;
            }

            var eventType = typeof(TEvent);
            var rented = ArrayPool<IDispatchFeature<TEvent>>.Shared.Rent(_factories.Length);
            var count = 0;

            try
            {
                for (var i = 0; i < _factories.Length; i++)
                {
                    var factory = _factories[i];
                    if (factory == null || !factory.SupportsEventType(eventType))
                    {
                        continue;
                    }

                    var feature = factory.Create<TEvent>();
                    if (feature == null)
                    {
                        continue;
                    }

                    rented[count++] = feature;
                }

                if (count == 0)
                {
                    return DispatchRecipe<TEvent>.Empty;
                }

                var features = new IDispatchFeature<TEvent>[count];
                Array.Copy(rented, features, count);
                return DispatchRecipe<TEvent>.FromFeatures(features);
            }
            finally
            {
                Array.Clear(rented, 0, count);
                ArrayPool<IDispatchFeature<TEvent>>.Shared.Return(rented, clearArray: false);
            }
        }

        /// <summary>
        /// Collects the enabled dispatch feature bindings for editor visualization.
        /// </summary>
        /// <param name="bindings">Collection to populate with enabled bindings.</param>
        public void CollectFeatures(ICollection<DispatchFeatureBinding> bindings)
        {
            if (bindings == null)
            {
                throw new ArgumentNullException(nameof(bindings));
            }

            for (var i = 0; i < _bindings.Length; i++)
            {
                var binding = _bindings[i];
                if (binding.Enabled)
                {
                    bindings.Add(binding);
                }
            }
        }
    }
}
