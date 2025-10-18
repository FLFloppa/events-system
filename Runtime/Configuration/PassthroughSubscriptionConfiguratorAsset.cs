using UnityEngine;

namespace FLFloppa.Events
{
    /// <summary>
    /// ScriptableObject that builds a passthrough subscription configurator returning empty recipes.
    /// </summary>
    [CreateAssetMenu(
        fileName = "PassthroughSubscriptionConfigurator",
        menuName = "FLFloppa/Events/Subscription Configurators/Passthrough")]
    public sealed class PassthroughSubscriptionConfiguratorAsset : SubscriptionConfiguratorAsset
    {
        /// <summary>
        /// Creates a configurator that yields empty subscription feature sets.
        /// </summary>
        public override ISubscriptionConfigurator Build()
        {
            return new PassthroughSubscriptionConfigurator();
        }
    }
}
