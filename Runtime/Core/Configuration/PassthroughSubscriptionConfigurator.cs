using System.Collections.Generic;

namespace FLFloppa.Events
{
    /// <summary>
    /// Subscription configurator that returns empty recipes, effectively disabling additional features.
    /// </summary>
    public sealed class PassthroughSubscriptionConfigurator : ISubscriptionConfigurator
    {
        /// <summary>
        /// Returns an empty subscription recipe for the specified event type.
        /// </summary>
        /// <typeparam name="TEvent">Event payload type associated with the subscription.</typeparam>
        public SubscriptionRecipe<TEvent> Create<TEvent>() where TEvent : struct
        {
            return SubscriptionRecipe<TEvent>.Empty;
        }

        /// <summary>
        /// No-op implementation since no features are configured.
        /// </summary>
        /// <param name="bindings">Collection of bindings (unused).</param>
        public void CollectFeatures(ICollection<SubscriptionFeatureBinding> bindings)
        {
        }
    }
}
