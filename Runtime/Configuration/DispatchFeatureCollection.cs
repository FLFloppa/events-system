using System.Collections.Generic;
using UnityEngine;

namespace FLFloppa.Events
{
    /// <summary>
    /// ScriptableObject that aggregates dispatch feature bindings for reuse across configurators.
    /// </summary>
    [CreateAssetMenu(
        fileName = "DispatchFeatureCollection",
        menuName = "FLFloppa/Events/Dispatch Feature Collection")]
    public sealed class DispatchFeatureCollection : ScriptableObject
    {
        [SerializeField] private DispatchFeatureBinding[] bindings = System.Array.Empty<DispatchFeatureBinding>();

        /// <summary>
        /// Gets the collection of configured dispatch feature bindings.
        /// </summary>
        public IReadOnlyList<DispatchFeatureBinding> Bindings => bindings;

        /// <summary>
        /// Adds all enabled bindings to the provided collection.
        /// </summary>
        /// <param name="destination">Destination collection that receives enabled bindings.</param>
        public void CollectBindings(ICollection<DispatchFeatureBinding> destination)
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
