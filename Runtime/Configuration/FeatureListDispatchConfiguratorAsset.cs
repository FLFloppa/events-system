using System;
using System.Collections.Generic;
using UnityEngine;

namespace FLFloppa.Events
{
    /// <summary>
    /// ScriptableObject that produces a dispatch configurator from reusable feature bindings.
    /// </summary>
    [CreateAssetMenu(
        fileName = "FeatureListDispatchConfigurator",
        menuName = "FLFloppa/Events/Dispatch Configurators/Feature List")]
    public sealed class FeatureListDispatchConfiguratorAsset : DispatchConfiguratorAsset
    {
        [SerializeField] private DispatchFeatureCollection[] collections = Array.Empty<DispatchFeatureCollection>();
        [SerializeField] private DispatchFeatureBinding[] localBindings = Array.Empty<DispatchFeatureBinding>();

        /// <summary>
        /// Builds a configurator that instantiates features defined by the aggregated bindings.
        /// </summary>
        /// <returns>A configured <see cref="IDispatchConfigurator"/>.</returns>
        public override IDispatchConfigurator Build()
        {
            var aggregatedBindings = new List<DispatchFeatureBinding>();
            CollectActiveBindings(aggregatedBindings);
            var factories = BuildFactories(aggregatedBindings);
            var bindingsArray = aggregatedBindings.ToArray();
            return new FeatureListDispatchConfigurator(factories, bindingsArray);
        }

        /// <summary>
        /// Collects all enabled bindings from the configured collections and local list.
        /// </summary>
        /// <param name="destination">Collection to populate with active bindings.</param>
        public void CollectActiveBindings(ICollection<DispatchFeatureBinding> destination)
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

        private static IDispatchFeatureFactory[] BuildFactories(List<DispatchFeatureBinding> bindings)
        {
            if (bindings.Count == 0)
            {
                return Array.Empty<IDispatchFeatureFactory>();
            }

            var factories = new List<IDispatchFeatureFactory>(bindings.Count);
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
