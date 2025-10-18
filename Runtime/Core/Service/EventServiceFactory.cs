namespace FLFloppa.Events
{
    /// <summary>
    /// Convenience factory for constructing <see cref="IEventService"/> instances from configuration objects and assets.
    /// </summary>
    public static class EventServiceFactory
    {
        /// <summary>
        /// Creates an event service using the provided configuration value object.
        /// </summary>
        /// <param name="configuration">Configuration describing policies, strategies, and providers.</param>
        /// <returns>A fully constructed <see cref="IEventService"/>.</returns>
        public static IEventService Create(EventServiceConfiguration configuration)
        {
            return new EventService(
                configuration.DispatchPolicy,
                configuration.ExecutorStrategy,
                configuration.Diagnostics,
                configuration.SubscriptionConfigurator,
                configuration.DispatchConfigurator,
                configuration.SubscriptionSchedulerProvider);
        }

        /// <summary>
        /// Creates an event service from a configuration asset.
        /// </summary>
        /// <param name="asset">Asset that can build a configuration value.</param>
        public static IEventService Create(EventServiceConfigurationAsset asset)
        {
            if (asset is null) throw new System.ArgumentNullException(nameof(asset));
            return Create(asset.Build());
        }

        /// <summary>
        /// Creates an event service from individual collaborators.
        /// </summary>
        /// <param name="dispatchPolicy">Dispatch policy to employ.</param>
        /// <param name="executorStrategy">Executor strategy responsible for invoking subscribers.</param>
        /// <param name="diagnostics">Optional diagnostics provider.</param>
        /// <param name="subscriptionConfigurator">Optional subscription recipe configurator.</param>
        /// <param name="dispatchConfigurator">Optional dispatch recipe configurator.</param>
        /// <param name="subscriptionSchedulerProvider">Optional scheduler provider for topics.</param>
        /// <returns>A fully constructed <see cref="IEventService"/>.</returns>
        public static IEventService Create(
            IDispatchPolicy dispatchPolicy,
            IExecutorStrategy executorStrategy,
            IEventDiagnosticsProvider? diagnostics = null,
            ISubscriptionConfigurator? subscriptionConfigurator = null,
            IDispatchConfigurator? dispatchConfigurator = null,
            ISubscriptionSchedulerProvider? subscriptionSchedulerProvider = null)
        {
            return new EventService(dispatchPolicy, executorStrategy, diagnostics, subscriptionConfigurator, dispatchConfigurator, subscriptionSchedulerProvider);
        }
    }
}
