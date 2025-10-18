using System;
using System.Collections.Generic;
using UnityEngine;

namespace FLFloppa.Events
{
    /// <summary>
    /// ScriptableObject that produces a subscription configurator from reusable feature bindings.
    /// </summary>
    [CreateAssetMenu(
        fileName = "FeatureListSubscriptionConfigurator",
        menuName = "FLFloppa/Events/Subscription Configurators/Feature List")]
    public sealed class FeatureListSubscriptionConfiguratorAsset : SubscriptionConfiguratorAsset
    {
        [SerializeField] private string subscriptionKey;
        [SerializeField] private SubscriptionFeatureCollection[] collections = Array.Empty<SubscriptionFeatureCollection>();
        [SerializeField] private SubscriptionFeatureBinding[] localBindings = Array.Empty<SubscriptionFeatureBinding>();

        /// <summary>
        /// Builds a configurator that instantiates features defined by the aggregated bindings.
        /// </summary>
        /// <returns>A configured <see cref="ISubscriptionConfigurator"/>.</returns>
        public override ISubscriptionConfigurator Build()
        {
            var aggregatedBindings = new List<SubscriptionFeatureBinding>();
            CollectActiveBindings(aggregatedBindings);
            var factories = BuildFactories(aggregatedBindings);
            var bindingsArray = aggregatedBindings.ToArray();
            var key = new SubscriptionKey(string.IsNullOrWhiteSpace(subscriptionKey) ? null : subscriptionKey);
            return new FeatureListSubscriptionConfigurator(key, factories, bindingsArray);
        }

        /// <summary>
        /// Collects all enabled bindings from the configured collections and local list.
        /// </summary>
        /// <param name="destination">Collection to populate with active bindings.</param>
        public void CollectActiveBindings(ICollection<SubscriptionFeatureBinding> destination)
        {
            if (destination == null)
            {
                throw new ArgumentNullException(nameof(destination));
            }

            if (collections != null)
            {
                for (var i = 0; i < collections.Length; i++)
                {
                    collections[i]?.CollectBindings(destination);
                }
            }

            if (localBindings == null)
            {
                return;
            }

            for (var i = 0; i < localBindings.Length; i++)
            {
                var binding = localBindings[i];
                if (binding.Enabled)
                {
                    destination.Add(binding);
                }
            }
        }

        private static ISubscriptionFeatureFactory[] BuildFactories(List<SubscriptionFeatureBinding> bindings)
        {
            if (bindings.Count == 0)
            {
                return Array.Empty<ISubscriptionFeatureFactory>();
            }

            var factories = new List<ISubscriptionFeatureFactory>(bindings.Count);
            for (var i = 0; i < bindings.Count; i++)
            {
                var binding = bindings[i];
                if (binding.TryCreateFactory(out var factory))
                {
                    factories.Add(factory);
                }
            }

            return factories.ToArray();
        }
    }
}
