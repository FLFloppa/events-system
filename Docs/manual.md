# FLFloppa Events System Manual

> Build, extend, and troubleshoot the FLFloppa Events System in production projects.

---

## 1. Concepts recap

* **Event Service Configuration** – ScriptableObject that bundles all runtime collaborators (dispatch policy, executor strategy, feature configurators, diagnostics provider, scheduler).
* **Topics** – Strongly typed channels created implicitly from subscription recipes. Each topic is allocation-free and reuses buffers per `TEvent`.
* **Features** – ScriptableObjects that intercept subscription or dispatch flows (e.g., auto-unsubscribe, diagnostics, queueing).
* **Schedulers** – Drive the pacing/order of subscriber invocation. Implement `ISubscriptionScheduler<TEvent>` or reuse built-in assets.

---

## 2. Setting up in the editor

1. Create an `EventServiceConfigurationAsset` via `Create → FLFloppa → Events → Event Service Configuration`.
2. Populate the required fields:
   * **Dispatch Policy** – Typically `ImmediateDispatchPolicyAsset`; swap to custom policies for queued delivery.
   * **Executor Strategy** – `MainThreadExecutorAsset` keeps handlers on Unity’s main thread. Provide an async executor for background work.
   * **Subscription Configurator** – Assign a `FeatureListSubscriptionConfiguratorAsset` or bespoke configurator.
   * **Dispatch Configurator** – Mirrors subscription configurator for outbound features.
3. Optional enrichers:
   * **Diagnostics Provider** – Choose between `NullDiagnosticsProviderAsset` (no-op) or custom diagnostics.
   * **Subscription Scheduler** – Pick from sequential, priority, throttled, or backpressure assets.
4. Inspect the asset. The UI Toolkit inspector surfaces:
   * Validation messages for missing collaborators.
   * Expandable summaries of resolved features and scheduler defaults.
   * Quick action buttons to create missing assets in place.

---

## 3. Bootstrapping in code

### 3.1 MonoBehaviour bootstrapper

```csharp
using FLFloppa.Events;
using UnityEngine;

public sealed class EventsBootstrap : MonoBehaviour
{
    [SerializeField] private EventServiceConfigurationAsset configuration;
    private IEventService _service;
    private IDisposable _subscription;

    private void Awake()
    {
        _service = new EventServiceBuilder(configuration).Build();
    }

    private void OnEnable()
    {
        _subscription = _service.Subscribe(
            SubscriptionRecipe<PlayerScored>.Empty,
            OnPlayerScored);
    }

    private void OnDisable()
    {
        _subscription?.Dispose();
    }

    private static void OnPlayerScored(in PlayerScored evt)
    {
        Debug.Log($"Player scored {evt.Amount}");
    }

    public readonly struct PlayerScored
    {
        public int Amount { get; init; }
    }
}
```

### 3.2 Raising events from gameplay systems

```csharp
public sealed class ScoreSystem
{
    private readonly IEventService _events;

    public ScoreSystem(IEventService events)
    {
        _events = events;
    }

    public void AddScore(int delta)
    {
        _events.Publish(new ScoreUpdated
        {
            Total = delta
        });
    }

    public readonly struct ScoreUpdated
    {
        public int Total { get; init; }
    }
}
```

Inject `IEventService` via your DI container or pass the instance during construction.

---

## 4. Working with features

### 4.1 Composition order

Binding lists execute from top to bottom. Combine reusable feature collections with per-configurator overrides:

1. Create a `DispatchFeatureCollection` asset and fill it with commonly required dispatch features.
2. Reference the collection in `FeatureListDispatchConfiguratorAsset.collections`.
3. Add local bindings under `localBindings` to extend or override collection defaults (e.g., enable batching only in specific contexts).

### 4.2 Diagnosing pipelines

* Add `DiagnosticsSubscriptionFeatureAsset` to the subscription configurator to log before/after invocation (toggle pre/post logging independently).
* Replace `NullDiagnosticsProviderAsset` with a custom provider implementing `IEventDiagnosticsProvider` to send metrics to your analytics backend.

### 4.3 Custom features

1. Derive from `SubscriptionFeatureFactory` or `DispatchFeatureFactory` and implement `Create()` returning the runtime feature instance.
2. Wrap the factory in a ScriptableObject asset so designers can enable/disable it through feature lists.
3. Ensure the runtime feature implements `ISubscriptionFeature<TEvent>` or `IDispatchFeature<TEvent>`.

---

## 5. Scheduler scenarios

| Scheduler | Use case | Key properties |
|-----------|----------|----------------|
| `SequentialSubscriptionSchedulerAsset` | Default FIFO for deterministic behaviour | None |
| `PrioritySubscriptionSchedulerAsset` | Frame-critical systems first | `defaultPriority` (higher executes sooner) |
| `ThrottledSubscriptionSchedulerAsset` | Rate-limit expensive handlers | `defaultThrottleSeconds` |
| `BackpressureSubscriptionSchedulerAsset` | Pause when consumers lag | `defaultBacklogLimit` |

### Custom scheduler tips

* Implement `ISubscriptionScheduler<TEvent>` or derive from `SubscriptionSchedulerAsset<TEvent>`.
* Use `SubscriptionSchedulerCommands<TEvent>` to control individual subscriptions (pause, resume, reprioritize).
* Expose serialized defaults on the asset inspector for designer-friendly tweaking.

---

## 6. Testing

```csharp
using FLFloppa.Events;
using NUnit.Framework;

public sealed class PlayerScoredTests
{
    [Test]
    public void PublishesAndReceivesEvent()
    {
        var configuration = ScriptableObject.CreateInstance<EventServiceConfigurationAsset>();
        // configure collaborators for the test here...

        var service = new EventServiceBuilder(configuration).Build();
        var received = 0;

        using var handle = service.Subscribe(
            SubscriptionRecipe<ScoreUpdated>.Empty,
            evt => received = evt.Total);

        service.Publish(new ScoreUpdated { Total = 42 });

        Assert.AreEqual(42, received);
    }

    private readonly struct ScoreUpdated
    {
        public int Total { get; init; }
    }
}
```

Tips:

* Use in-memory ScriptableObject instances for isolated tests.
* Dispose subscriptions after each test to keep topics clean.

---

## 7. Troubleshooting

* **No handlers firing** – Confirm the subscription configurator includes bindings and the scheduler is not paused (backpressure).
* **Unexpected allocations** – Ensure events are structs or immutable records; avoid capturing lambdas that allocate closure objects.
* **Diagnostics spam** – Disable logging hooks in `DiagnosticsSubscriptionFeatureAsset` or switch to `NullDiagnosticsProviderAsset` in release builds.
* **Scheduler stalls** – Inspect the expandable lists in the configuration inspector for backlog thresholds or throttle intervals that are too aggressive.

---

## 8. Additional resources

* `Docs/quick_start.md` – step-by-step setup.
* `Samples~/BasicSetup` – runnable sample with configured assets.
* `Runtime/Configuration/` – reference implementations for all ScriptableObject collaborators.

Contributions and improvements to this manual are welcome—submit pull requests or open issues with suggestions.
