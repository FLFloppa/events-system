using UnityEngine;

namespace FLFloppa.Events
{
    /// <summary>
    /// ScriptableObject that creates sequential schedulers which preserve subscription order.
    /// </summary>
    [CreateAssetMenu(
        fileName = "SequentialSubscriptionScheduler",
        menuName = "FLFloppa/Events/Subscription Schedulers/Sequential")]
    public sealed class SequentialSubscriptionSchedulerAsset : SubscriptionSchedulerAsset
    {
        /// <summary>
        /// Creates a provider that dispatches subscribers in registration order.
        /// </summary>
        public override ISubscriptionSchedulerProvider CreateProvider()
        {
            return new SequentialSubscriptionSchedulerProvider();
        }
    }
}
