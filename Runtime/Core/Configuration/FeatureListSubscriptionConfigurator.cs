using System;
using System.Buffers;
using System.Collections.Generic;

namespace FLFloppa.Events
{
    /// <summary>
    /// Subscription configurator that instantiates features from a list of factories and bindings.
    /// </summary>
    public sealed class FeatureListSubscriptionConfigurator : ISubscriptionConfigurator
    {
        private readonly SubscriptionKey _subscriptionKey;
        private readonly ISubscriptionFeatureFactory[] _factories;
        private readonly SubscriptionFeatureBinding[] _bindings;

        /// <summary>
        /// Initializes a new instance of the <see cref="FeatureListSubscriptionConfigurator"/> class.
        /// </summary>
        /// <param name="subscriptionKey">Key used to identify the subscription recipe.</param>
        /// <param name="factories">Factories capable of producing subscription features.</param>
        /// <param name="bindings">Binding metadata indicating which features are enabled.</param>
        public FeatureListSubscriptionConfigurator(
            SubscriptionKey subscriptionKey,
            ISubscriptionFeatureFactory[] factories,
            SubscriptionFeatureBinding[] bindings)
        {
            _subscriptionKey = subscriptionKey;
            _factories = factories ?? Array.Empty<ISubscriptionFeatureFactory>();
            _bindings = bindings ?? Array.Empty<SubscriptionFeatureBinding>();
        }

        /// <summary>
        /// Creates a subscription recipe for the target event type by instantiating supported features.
        /// </summary>
        /// <typeparam name="TEvent">Event payload type associated with the subscription.</typeparam>
        /// <returns>A populated recipe when features are available; otherwise <see cref="SubscriptionRecipe{TEvent}.Empty"/>.</returns>
        public SubscriptionRecipe<TEvent> Create<TEvent>() where TEvent : struct
        {
            if (_factories.Length == 0)
            {
                return SubscriptionRecipe<TEvent>.Empty;
            }

            var eventType = typeof(TEvent);
            var rented = ArrayPool<ISubscriptionFeature<TEvent>>.Shared.Rent(_factories.Length);
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
                    return SubscriptionRecipe<TEvent>.Empty;
                }

                var features = new ISubscriptionFeature<TEvent>[count];
                Array.Copy(rented, features, count);
                return SubscriptionRecipe<TEvent>.FromFeatures(_subscriptionKey, features);
            }
            finally
            {
                Array.Clear(rented, 0, count);
                ArrayPool<ISubscriptionFeature<TEvent>>.Shared.Return(rented, clearArray: false);
            }
        }

        /// <summary>
        /// Collects the enabled feature bindings for editor visualization.
        /// </summary>
        /// <param name="bindings">Collection to populate with enabled bindings.</param>
        public void CollectFeatures(ICollection<SubscriptionFeatureBinding> bindings)
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
