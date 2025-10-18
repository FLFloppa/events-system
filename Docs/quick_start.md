# FLFloppa Events System – Quick Start

## Installing the package
* __Add package__ – Copy the `Packages/FLFloppa Events System/` folder into your Unity project (or add it as a Git submodule / local package).
* __Assembly reference__ – Ensure your gameplay assemblies reference `FLFloppa.Events.Runtime` so you can access the runtime API.

## Creating a configuration asset
* __Create asset__ – Right-click in the Project window and choose `Create ► FLFloppa ► Events ► Event Service Configuration` to make an `EventServiceConfigurationAsset`.
* __Assign policies__ – Populate the required fields (dispatch policy, executor strategy, subscription/dispatch configurators, diagnostics provider). The inspector highlights missing assignments.
* __Choose scheduler__ – Pick one of the provided `SubscriptionSchedulerAsset` instances (Sequential, Priority, Throttled, Backpressure) to control subscription ordering behaviour.

## Building the service
* __Instantiate__ – Use `var service = new EventServiceBuilder(myAsset).Build();` to create an `IEventService` at runtime.
* __Reuse__ – Build once and cache the service (e.g., in a MonoBehaviour, ScriptableObject, or service locator).

## Subscribing to events
* __Define event struct__ – Create a `public struct PlayerScored { public int Amount; }` (events must be structs).
* __Create recipe__ – Configure subscription features through the selected `ISubscriptionConfigurator`. Feature assets (priority, throttled, backpressure) can emit scheduler commands.
* __Subscribe__ – `service.Subscribe(PlayerSubscriptionRecipe, OnPlayerScored);` where `OnPlayerScored` matches `EventHandler<PlayerScored>`.

## Publishing events
* __Synchronous__ – `service.Publish(new PlayerScored { Amount = 5 });`.
* __Asynchronous__ – `await service.PublishAsync(new PlayerScored { Amount = 5 }, DispatchRecipe<PlayerScored>.Empty, cancellationToken);`.

## Using scheduler-aware features
* __Priority__ – Add `PrioritySubscriptionFeature` via configurator to ensure higher priority handlers run first.
* __Throttled__ – Apply throttling to limit invocation frequency (uses `SetThrottleSchedulerCommand`).
* __Backpressure__ – Attach features that pause/resume handlers when backlog thresholds are exceeded.

## Diagnostics
* __Stats__ – Call `service.TryGetTopicStats(out var stats);` to inspect subscriber counts.
* __Diagnostics provider__ – Provide a custom `IEventDiagnosticsProvider` to capture warnings, metrics, or logs for your project.

## Testing the system
* __Sample script__ – See `Assets/Scripts/EventSystemQuickStart.cs` for a ready-to-use MonoBehaviour that wires up the service, subscribes, and publishes sample events.
