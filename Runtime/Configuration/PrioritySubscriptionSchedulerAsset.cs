using UnityEngine;

namespace FLFloppa.Events
{
    /// <summary>
    /// ScriptableObject that creates priority schedulers with a configurable default priority.
    /// </summary>
    [CreateAssetMenu(
        fileName = "PrioritySubscriptionScheduler",
        menuName = "FLFloppa/Events/Subscription Schedulers/Priority")]
    public sealed class PrioritySubscriptionSchedulerAsset : SubscriptionSchedulerAsset
    {
        [SerializeField] private int defaultPriority = 0;

        /// <summary>
        /// Creates a provider that applies the configured priority to new subscribers.
        /// </summary>
        public override ISubscriptionSchedulerProvider CreateProvider()
        {
            return new PrioritySubscriptionSchedulerProvider(defaultPriority);
        }

        /// <summary>
        /// Gets the priority assigned to subscribers created by the provider.
        /// </summary>
        public int DefaultPriority => defaultPriority;
    }
}
