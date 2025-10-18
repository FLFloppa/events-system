using UnityEngine;

namespace FLFloppa.Events
{
    /// <summary>
    /// Base ScriptableObject used to expose subscription scheduler providers through the editor.
    /// </summary>
    public abstract class SubscriptionSchedulerAsset : ScriptableObject
    {
        /// <summary>
        /// Creates a provider capable of instantiating schedulers at runtime.
        /// </summary>
        /// <returns>An <see cref="ISubscriptionSchedulerProvider"/> instance.</returns>
        public abstract ISubscriptionSchedulerProvider CreateProvider();
    }
}
