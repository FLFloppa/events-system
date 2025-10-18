using UnityEngine;

namespace FLFloppa.Events
{
    /// <summary>
    /// Base ScriptableObject that exposes subscription feature factories for configuration assets.
    /// </summary>
    public abstract class SubscriptionFeatureAsset : ScriptableObject
    {
        /// <summary>
        /// Creates a factory capable of producing subscription features at runtime.
        /// </summary>
        public abstract ISubscriptionFeatureFactory CreateFactory();
    }
}
