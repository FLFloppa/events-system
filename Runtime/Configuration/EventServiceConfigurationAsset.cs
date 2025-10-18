#nullable enable
using System;
using UnityEngine;

namespace FLFloppa.Events
{
    /// <summary>
    /// ScriptableObject asset that builds an <see cref="EventServiceConfiguration"/> using serialized collaborators.
    /// </summary>
    [CreateAssetMenu(
        fileName = "EventServiceConfiguration",
        menuName = "FLFloppa/Events/Event Service Configuration")]
    public sealed class EventServiceConfigurationAsset : ScriptableObject, IBuildable<EventServiceConfiguration>
    {
        [SerializeField] private DispatchPolicyAsset? dispatchPolicy;
        [SerializeField] private ExecutorStrategyAsset? executorStrategy;
        [SerializeField] private SubscriptionConfiguratorAsset? subscriptionConfigurator;
        [SerializeField] private DispatchConfiguratorAsset? dispatchConfigurator;
        [SerializeField] private DiagnosticsProviderAsset? diagnosticsProvider;
        [SerializeField] private SubscriptionSchedulerAsset? subscriptionScheduler;

        /// <summary>
        /// Builds a configuration instance using the serialized assets.
        /// </summary>
        /// <returns>An initialized <see cref="EventServiceConfiguration"/>.</returns>
        public EventServiceConfiguration Build()
        {
            ValidateOrThrow();
            var diagnostics = diagnosticsProvider != null
                ? diagnosticsProvider.Build()
                : NullEventDiagnosticsProvider.Instance;
            return new EventServiceConfiguration(
                dispatchPolicy!.Build(),
                executorStrategy!.Build(),
                subscriptionConfigurator!.Build(),
                dispatchConfigurator!.Build(),
                subscriptionScheduler != null
                    ? subscriptionScheduler.CreateProvider()
                    : DefaultSubscriptionSchedulerProvider.Instance,
                diagnostics);
        }

        /// <summary>
        /// Validates the asset contents and returns whether they are complete.
        /// </summary>
        /// <param name="validationMessage">Message describing why validation failed.</param>
        /// <returns><c>true</c> when required references are assigned.</returns>
        public bool TryValidate(out string validationMessage)
        {
            if (dispatchPolicy == null)
            {
                validationMessage = "Dispatch policy asset is not assigned.";
                return false;
            }

            if (executorStrategy == null)
            {
                validationMessage = "Executor strategy asset is not assigned.";
                return false;
            }

            if (subscriptionConfigurator == null)
            {
                validationMessage = "Subscription configurator asset is not assigned.";
                return false;
            }

            if (dispatchConfigurator == null)
            {
                validationMessage = "Dispatch configurator asset is not assigned.";
                return false;
            }

            validationMessage = string.Empty;
            return true;
        }

        /// <summary>
        /// Ensures required collaborators are assigned, throwing when validation fails.
        /// </summary>
        internal void ValidateOrThrow()
        {
            if (!TryValidate(out var message))
            {
                throw new InvalidOperationException(message);
            }
        }

        /// <summary>
        /// Gets the dispatch policy resolved from the assigned asset or default instance.
        /// </summary>
        public IDispatchPolicy DispatchPolicy => dispatchPolicy != null ? dispatchPolicy.Build() : ImmediateDispatchPolicy.Instance;
        /// <summary>
        /// Gets the executor strategy resolved from the assigned asset or default instance.
        /// </summary>
        public IExecutorStrategy ExecutorStrategy => executorStrategy != null ? executorStrategy.Build() : MainThreadExecutorStrategy.Instance;
        /// <summary>
        /// Gets the subscription configurator resolved from the assigned asset or default.
        /// </summary>
        public ISubscriptionConfigurator SubscriptionConfigurator => subscriptionConfigurator != null ? subscriptionConfigurator.Build() : new PassthroughSubscriptionConfigurator();
        /// <summary>
        /// Gets the dispatch configurator resolved from the assigned asset or default.
        /// </summary>
        public IDispatchConfigurator DispatchConfigurator => dispatchConfigurator != null ? dispatchConfigurator.Build() : new PassthroughDispatchConfigurator();
        /// <summary>
        /// Gets the diagnostics provider resolved from the assigned asset or default instance.
        /// </summary>
        public IEventDiagnosticsProvider DiagnosticsProvider => diagnosticsProvider != null ? diagnosticsProvider.Build() : NullEventDiagnosticsProvider.Instance;
        /// <summary>
        /// Gets the subscription scheduler asset reference, if any.
        /// </summary>
        public SubscriptionSchedulerAsset? SubscriptionScheduler => subscriptionScheduler;
        /// <summary>
        /// Gets the scheduler provider resolved from the asset or default instance.
        /// </summary>
        public ISubscriptionSchedulerProvider SubscriptionSchedulerProvider => subscriptionScheduler != null
            ? subscriptionScheduler.CreateProvider()
            : DefaultSubscriptionSchedulerProvider.Instance;
    }
}
