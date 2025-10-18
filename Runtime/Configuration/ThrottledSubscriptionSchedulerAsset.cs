using System;
using UnityEngine;

namespace FLFloppa.Events
{
    /// <summary>
    /// ScriptableObject that creates throttled schedulers with a configurable cooldown duration.
    /// </summary>
    [CreateAssetMenu(
        fileName = "ThrottledSubscriptionScheduler",
        menuName = "FLFloppa/Events/Subscription Schedulers/Throttled")]
    public sealed class ThrottledSubscriptionSchedulerAsset : SubscriptionSchedulerAsset
    {
        [SerializeField] private float defaultThrottleSeconds = 0f;

        /// <summary>
        /// Creates a provider that enforces the configured throttle duration on subscribers.
        /// </summary>
        public override ISubscriptionSchedulerProvider CreateProvider()
        {
            var clamped = Mathf.Max(0f, defaultThrottleSeconds);
            return new ThrottledSubscriptionSchedulerProvider(TimeSpan.FromSeconds(clamped));
        }

        /// <summary>
        /// Gets or sets the throttle duration, in seconds, applied to created schedulers.
        /// Values below zero are clamped to zero.
        /// </summary>
        public float DefaultThrottleSeconds
        {
            get => defaultThrottleSeconds;
            set => defaultThrottleSeconds = Mathf.Max(0f, value);
        }
    }
}
