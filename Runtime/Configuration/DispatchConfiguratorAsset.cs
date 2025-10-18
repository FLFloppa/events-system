using UnityEngine;

namespace FLFloppa.Events
{
    /// <summary>
    /// Base ScriptableObject that produces an <see cref="IDispatchConfigurator"/> for runtime use.
    /// </summary>
    public abstract class DispatchConfiguratorAsset : ScriptableObject
    {
        /// <summary>
        /// Builds a dispatch configurator instance.
        /// </summary>
        public abstract IDispatchConfigurator Build();
    }
}
