using System.Collections.Generic;
using UnityEngine;

namespace FLFloppa.Events
{
    /// <summary>
    /// ScriptableObject that aggregates subscription feature bindings for reuse across configurators.
    /// </summary>
    [CreateAssetMenu(
        fileName = "SubscriptionFeatureCollection",
        menuName = "FLFloppa/Events/Subscription Feature Collection")]
    public sealed class SubscriptionFeatureCollection : ScriptableObject
    {
        [SerializeField] private SubscriptionFeatureBinding[] bindings = System.Array.Empty<SubscriptionFeatureBinding>();

        /// <summary>
        /// Gets the collection of configured subscription feature bindings.
        /// </summary>
        public IReadOnlyList<SubscriptionFeatureBinding> Bindings => bindings;

        /// <summary>
        /// Adds all enabled bindings to the provided collection.
        /// </summary>
        /// <param name="destination">Destination collection that receives enabled bindings.</param>
        public void CollectBindings(ICollection<SubscriptionFeatureBinding> destination)
        {
            if (bindings == null)
            {
                return;
            }

            for (var i = 0; i < bindings.Length; i++)
            {
                var binding = bindings[i];
                if (binding.Enabled)
                {
                    destination.Add(binding);
                }
            }
        }
    }
}
