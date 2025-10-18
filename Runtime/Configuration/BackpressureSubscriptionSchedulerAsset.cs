using UnityEngine;

namespace FLFloppa.Events
{
    /// <summary>
    /// ScriptableObject that creates backpressure schedulers with a configurable backlog limit.
    /// </summary>
    [CreateAssetMenu(
        fileName = "BackpressureSubscriptionScheduler",
        menuName = "FLFloppa/Events/Subscription Schedulers/Backpressure")]
    public sealed class BackpressureSubscriptionSchedulerAsset : SubscriptionSchedulerAsset
    {
        [SerializeField] private int defaultBacklogLimit = 0;

        /// <summary>
        /// Creates a provider that enforces a fixed backlog limit for subscribers.
        /// </summary>
        public override ISubscriptionSchedulerProvider CreateProvider()
        {
            return new BackpressureSubscriptionSchedulerProvider(defaultBacklogLimit);
        }

        /// <summary>
        /// Gets or sets the backlog limit assigned to created schedulers.
        /// Values below zero are clamped to zero.
        /// </summary>
        public int DefaultBacklogLimit
        {
            get => defaultBacklogLimit;
            set => defaultBacklogLimit = Mathf.Max(0, value);
        }
    }
}
