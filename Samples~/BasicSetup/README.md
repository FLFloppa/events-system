# Basic Event Service Setup Sample

This sample demonstrates how to configure and use the FLFloppa Events System inside a Unity project. Import it from **Window → Package Manager → FLFloppa Events System → Samples → Basic Event Service Setup**.

## Contents
* `Resources/` – Example assets for dispatch policy, executor strategy, configurators, diagnostics, and subscription scheduler.
* `Scripts/EventServiceQuickStartSample.cs` – MonoBehaviour that builds an `IEventService`, subscribes to an event, and publishes sample payloads.
* `Scenes/BasicEventService.unity` – Scene referencing the sample configuration and script.

## Setup steps
1. Open the sample scene (`Scenes/BasicEventService.unity`).
2. Inspect the `EventSystemQuickStartSample` GameObject:
   * The `EventServiceQuickStartSample` component references the sample `EventServiceConfigurationAsset`.
   * Enter Play Mode to see console logs when the `PlayerScored` event is published.
3. Modify scheduler behaviour by swapping the `SubscriptionScheduler` asset in the configuration and observe how handler invocation order changes.
4. Attach additional subscription features from `Resources/SubscriptionFeatures/` to experiment with priority and throttling commands.

## Extending the sample
* Duplicate the configuration asset and assign your own subscription/dispatch features.
* Implement custom scheduler providers by inheriting from `ISubscriptionSchedulerProvider` and referencing them in the configuration.
* Add more event structs and handlers to explore multi-topic setups.
