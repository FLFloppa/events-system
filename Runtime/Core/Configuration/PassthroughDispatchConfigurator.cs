using System.Collections.Generic;

namespace FLFloppa.Events
{
    /// <summary>
    /// Dispatch configurator that returns empty recipes, bypassing additional dispatch features.
    /// </summary>
    public sealed class PassthroughDispatchConfigurator : IDispatchConfigurator
    {
        /// <summary>
        /// Returns an empty dispatch recipe for the specified event type.
        /// </summary>
        /// <typeparam name="TEvent">Event payload type associated with the dispatch.</typeparam>
        public DispatchRecipe<TEvent> Create<TEvent>() where TEvent : struct
        {
            return DispatchRecipe<TEvent>.Empty;
        }

        /// <summary>
        /// No-op implementation since no dispatch features are configured.
        /// </summary>
        /// <param name="bindings">Collection of bindings (unused).</param>
        public void CollectFeatures(ICollection<DispatchFeatureBinding> bindings)
        {
        }
    }
}
