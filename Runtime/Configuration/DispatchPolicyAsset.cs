using UnityEngine;

namespace FLFloppa.Events
{
    /// <summary>
    /// Base ScriptableObject that builds an <see cref="IDispatchPolicy"/> instance.
    /// </summary>
    public abstract class DispatchPolicyAsset : ScriptableObject, IBuildable<IDispatchPolicy>
    {
        /// <summary>
        /// Creates a dispatch policy for runtime use.
        /// </summary>
        public abstract IDispatchPolicy Build();
    }
}
