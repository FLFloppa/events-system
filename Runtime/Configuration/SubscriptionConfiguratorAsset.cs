using UnityEngine;

namespace FLFloppa.Events
{
    /// <summary>
    /// Base ScriptableObject that builds an <see cref="ISubscriptionConfigurator"/> instance.
    /// </summary>
    public abstract class SubscriptionConfiguratorAsset : ScriptableObject, IBuildable<ISubscriptionConfigurator>
    {
        /// <summary>
        /// Creates a subscription configurator for runtime use.
        /// </summary>
        public abstract ISubscriptionConfigurator Build();
    }
}
