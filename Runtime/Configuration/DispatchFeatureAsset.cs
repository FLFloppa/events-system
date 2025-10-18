using UnityEngine;

namespace FLFloppa.Events
{
    /// <summary>
    /// Base ScriptableObject that exposes dispatch feature factories for configuration assets.
    /// </summary>
    public abstract class DispatchFeatureAsset : ScriptableObject
    {
        /// <summary>
        /// Creates a factory capable of producing dispatch features at runtime.
        /// </summary>
        public abstract IDispatchFeatureFactory CreateFactory();
    }
}
