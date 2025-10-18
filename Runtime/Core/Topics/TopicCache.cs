using System.Collections.Generic;

namespace FLFloppa.Events
{
    /// <summary>
    /// Provides cached access to topics per event type and scheduler provider.
    /// </summary>
    internal static class TopicCache<TEvent> where TEvent : struct
    {
        private static readonly object Sync = new object();
        private static readonly Dictionary<ISubscriptionSchedulerProvider, Topic<TEvent>> Topics = new();

        /// <summary>
        /// Retrieves an existing topic for the provider or creates one if none exists.
        /// </summary>
        /// <param name="provider">Scheduler provider used to configure the topic.</param>
        /// <returns>A cached topic instance keyed by <paramref name="provider"/>.</returns>
        public static Topic<TEvent> GetOrCreate(ISubscriptionSchedulerProvider? provider)
        {
            provider ??= DefaultSubscriptionSchedulerProvider.Instance;

            lock (Sync)
            {
                if (!Topics.TryGetValue(provider, out var topic))
                {
                    var scheduler = provider.CreateScheduler<TEvent>() ?? new SequentialSubscriptionScheduler<TEvent>();
                    topic = new Topic<TEvent>(EventTypeIndex.GetId<TEvent>(), scheduler);
                    Topics[provider] = topic;
                }

                return topic;
            }
        }

        /// <summary>
        /// Gets the default topic instance using the default scheduler provider.
        /// </summary>
        public static Topic<TEvent> Instance => GetOrCreate(DefaultSubscriptionSchedulerProvider.Instance);
    }
}
