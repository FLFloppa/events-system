using System;
using UnityEngine;

namespace FLFloppa.Events
{
    /// <summary>
    /// Serialized binding that references a dispatch feature asset and enables or disables it.
    /// </summary>
    [Serializable]
    public struct DispatchFeatureBinding
    {
        [SerializeField] private DispatchFeatureAsset feature;
        [SerializeField] private bool enabled;

        /// <summary>
        /// Initializes a new instance of the <see cref="DispatchFeatureBinding"/> struct.
        /// </summary>
        /// <param name="feature">Dispatch feature asset to reference.</param>
        /// <param name="enabled">Whether the feature is enabled.</param>
        public DispatchFeatureBinding(DispatchFeatureAsset feature = null, bool enabled = true)
        {
            this.feature = feature;
            this.enabled = enabled;
        }

        /// <summary>
        /// Gets the referenced dispatch feature asset.
        /// </summary>
        public DispatchFeatureAsset Feature => feature;
        /// <summary>
        /// Gets a value indicating whether the feature is enabled.
        /// </summary>
        public bool Enabled => enabled;

        /// <summary>
        /// Attempts to create a dispatch feature factory when the binding is enabled.
        /// </summary>
        /// <param name="factory">Created factory instance when successful.</param>
        /// <returns><c>true</c> when the factory was created.</returns>
        public bool TryCreateFactory(out IDispatchFeatureFactory factory)
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
