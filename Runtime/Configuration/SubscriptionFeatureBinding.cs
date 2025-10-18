using System;
using UnityEngine;

namespace FLFloppa.Events
{
    /// <summary>
    /// Serialized binding that references a subscription feature asset and enables or disables it.
    /// </summary>
    [Serializable]
    public struct SubscriptionFeatureBinding
    {
        [SerializeField] private SubscriptionFeatureAsset feature;
        [SerializeField] private bool enabled;

        /// <summary>
        /// Initializes a new instance of the <see cref="SubscriptionFeatureBinding"/> struct.
        /// </summary>
        /// <param name="feature">Subscription feature asset to reference.</param>
        /// <param name="enabled">Whether the feature is enabled.</param>
        public SubscriptionFeatureBinding(SubscriptionFeatureAsset feature = null, bool enabled = true)
        {
            this.feature = feature;
            this.enabled = enabled;
        }

        /// <summary>
        /// Gets the referenced subscription feature asset.
        /// </summary>
        public SubscriptionFeatureAsset Feature => feature;
        /// <summary>
        /// Gets a value indicating whether the feature is enabled.
        /// </summary>
        public bool Enabled => enabled;

        /// <summary>
        /// Attempts to create a subscription feature factory when the binding is enabled.
        /// </summary>
        /// <param name="factory">Created factory instance when successful.</param>
        /// <returns><c>true</c> when the factory was created.</returns>
        public bool TryCreateFactory(out ISubscriptionFeatureFactory factory)
        {
            if (!enabled || feature == null)
            {
                factory = default!;
                return false;
            }

            factory = feature.CreateFactory();
            return factory != null;
        }
    }
}
