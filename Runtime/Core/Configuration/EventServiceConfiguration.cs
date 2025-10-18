namespace FLFloppa.Events
{
    /// <summary>
    /// Immutable configuration container used to construct an <see cref="IEventService"/> instance.
    /// </summary>
    public readonly struct EventServiceConfiguration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EventServiceConfiguration"/> struct.
        /// </summary>
        /// <param name="dispatchPolicy">Policy controlling how payloads are dispatched.</param>
        /// <param name="executorStrategy">Strategy that executes subscriber delegates.</param>
        /// <param name="subscriptionConfigurator">Configurator supplying subscription recipes.</param>
        /// <param name="dispatchConfigurator">Configurator supplying dispatch recipes.</param>
        /// <param name="subscriptionSchedulerProvider">Provider capable of creating schedulers per topic.</param>
        /// <param name="diagnostics">Diagnostics provider for logging and instrumentation.</param>
        public EventServiceConfiguration(
            IDispatchPolicy dispatchPolicy,
            IExecutorStrategy executorStrategy,
            ISubscriptionConfigurator subscriptionConfigurator,
            IDispatchConfigurator dispatchConfigurator,
            ISubscriptionSchedulerProvider subscriptionSchedulerProvider,
            IEventDiagnosticsProvider diagnostics)
        {
            DispatchPolicy = dispatchPolicy;
            ExecutorStrategy = executorStrategy;
            SubscriptionConfigurator = subscriptionConfigurator;
            DispatchConfigurator = dispatchConfigurator;
            SubscriptionSchedulerProvider = subscriptionSchedulerProvider;
            Diagnostics = diagnostics;
        }

        /// <summary>
        /// Gets the policy responsible for dispatching payloads.
        /// </summary>
        public IDispatchPolicy DispatchPolicy { get; }
        /// <summary>
        /// Gets the strategy used to execute subscriber delegates.
        /// </summary>
        public IExecutorStrategy ExecutorStrategy { get; }
        /// <summary>
        /// Gets the configurator that supplies subscription recipes.
        /// </summary>
        public ISubscriptionConfigurator SubscriptionConfigurator { get; }
        /// <summary>
        /// Gets the configurator that supplies dispatch recipes.
        /// </summary>
        public IDispatchConfigurator DispatchConfigurator { get; }
        /// <summary>
        /// Gets the provider that creates subscription schedulers for topics.
        /// </summary>
        public ISubscriptionSchedulerProvider SubscriptionSchedulerProvider { get; }
        /// <summary>
        /// Gets the diagnostics provider used to surface warnings and logs.
        /// </summary>
        public IEventDiagnosticsProvider Diagnostics { get; }

        /// <summary>
        /// Gets the default configuration using built-in policies and configurators.
        /// </summary>
        public static EventServiceConfiguration Default { get; } = new EventServiceConfiguration(
            ImmediateDispatchPolicy.Instance,
            MainThreadExecutorStrategy.Instance,
            new PassthroughSubscriptionConfigurator(),
            new PassthroughDispatchConfigurator(),
            DefaultSubscriptionSchedulerProvider.Instance,
            NullEventDiagnosticsProvider.Instance);
    }
}
