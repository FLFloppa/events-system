using System;

namespace FLFloppa.Events
{
    /// <summary>
    /// Executor strategy that publishes payloads on the current thread via the topic.
    /// </summary>
    public sealed class MainThreadExecutorStrategy : IExecutorStrategy
    {
        /// <summary>
        /// Singleton instance of the main-thread executor strategy.
        /// </summary>
        public static IExecutorStrategy Instance { get; } = new MainThreadExecutorStrategy();

        private MainThreadExecutorStrategy()
        {
        }

        /// <inheritdoc />
        public void Execute<TEvent>(Topic<TEvent> topic, in TEvent payload, ReadOnlySpan<IDispatchFeature<TEvent>> dispatchFeatures) where TEvent : struct
        {
            topic.Publish(payload, dispatchFeatures);
        }
    }
}
