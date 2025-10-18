<div align="center">

# FLFloppa Events System

_Event-driven architecture for Unity 2022.3+, built for zero-allocation dispatch and feature-driven customization_

[![Unity 2022.3+](https://img.shields.io/badge/unity-2022.3%2B-black.svg?logo=unity)](#requirements)
[![License: MIT](https://img.shields.io/badge/license-MIT-green.svg)](#license)
[![Issues](https://img.shields.io/badge/issues-welcome-blue.svg)](#support--questions)

</div>

The FLFloppa Events System delivers a fast, modular publish/subscribe pipeline tailored for gameplay. Topics, subscription features, and dispatch policies are assembled through ScriptableObjects, letting designers tune behaviour without touching core code. Runtime schedulers keep handler ordering deterministic—priority, throttling, and backpressure all ship out of the box.

---

## Table of contents

* [Highlights](#highlights)
* [Requirements](#requirements)
* [Installation](#installation)
* [Quick start](#quick-start)
* [Architecture overview](#architecture-overview)
* [Scheduler features](#scheduler-features)
* [Editor tooling](#editor-tooling)
* [Samples](#samples)
* [Documentation](#documentation)
* [Testing](#testing)
* [Roadmap](#roadmap)
* [Contributing](#contributing)
* [Support & questions](#support--questions)
* [License](#license)

---

## Highlights

* __Zero-allocation topics__ – Generic `Topic<TEvent>` instances reuse buffers and avoid GC when broadcasting.
* __Feature-first design__ – Compose subscription/dispatch features through ScriptableObjects; no boilerplate listeners.
* __Scheduler abstraction__ – Swap between sequential, priority, throttled, or backpressure-aware schedulers per configuration.
* __Diagnostics ready__ – Plug in `IEventDiagnosticsProvider` implementations for observability, warnings, or analytics.
* __Editor integrations__ – UI Toolkit inspectors validate configurations, surface scheduler details, and support drag-and-drop feature lists.

---

## Requirements

* Unity **2022.3 LTS** or newer.
* [UniTask](https://github.com/Cysharp/UniTask) (version `2.3.3`+) installed alongside the package.

---

## Installation

### Via Git URL (recommended)

1. Open **Window → Package Manager**.
2. Click the **+** button → **Add package from git URL…**
3. Paste the repository URL (replace the ref with the desired tag for releases):
   ```
   https://github.com/FLFloppa/events-system.git
   ```
4. Unity installs the package and registers samples.
5. Install UniTask via Git URL (`https://github.com/Cysharp/UniTask.git?path=src/UniTask/Assets/Plugins/UniTask`).

### Via GitHub release archive

1. Download the latest `.unitypackage` or `.zip` from the [GitHub releases page](https://github.com/FLFloppa/events-system/releases).
2. Import via **Assets → Import Package → Custom Package…** (assets land under `Assets/` rather than `Packages/`).
3. Install UniTask before entering Play Mode.

### Manual copy

1. Clone/download the repository.
2. Copy `Packages/FLFloppa Events System/` into your project’s `Packages/` directory.
3. Restart Unity to regenerate assembly definitions.

---

## Quick start

1. **Create an Event Service Configuration** via `Create → FLFloppa → Events → Event Service Configuration`.
2. **Assign required assets**:
   * Dispatch policy (`ImmediateDispatchPolicyAsset` or custom).
   * Executor strategy (`MainThreadExecutorStrategyAsset` or custom).
   * Subscription & dispatch configurators (feature lists, etc.).
   * Diagnostics provider (optional).
   * Subscription scheduler (Sequential, Priority, Throttled, Backpressure, or custom).
3. **Build the service in code**:
   ```csharp
   public sealed class EventsBootstrap : MonoBehaviour
   {
       [SerializeField] private EventServiceConfigurationAsset configuration;
       private IEventService _service;

       private void Awake()
       {
           _service = new EventServiceBuilder(configuration).Build();
       }

       private void OnEnable()
       {
           var handle = _service.Subscribe(SubscriptionRecipe<PlayerScored>.Empty, OnPlayerScored);
           _service.Publish(new PlayerScored { Amount = 10 });
       }

       private static void OnPlayerScored(in PlayerScored evt)
       {
           Debug.Log($"Player scored {evt.Amount}");
       }

       public readonly struct PlayerScored { public int Amount; }
   }
   ```
4. **Check `Docs/quick_start.md`** for a deeper walkthrough.

---

## Architecture overview

* __Runtime/Core__ – Topics, subscription/dispatch features, configurators, and scheduler infrastructure.
* __Runtime/Configuration__ – ScriptableObject assets that assemble runtime components.
* __Runtime/Core/Scheduling__ – Scheduler implementations + command API for priority, throttling, and backpressure.
* __Editor__ – UI Toolkit inspectors validating configurations and exposing scheduler summaries.
* __Tests__ – NUnit-based edit-mode tests (extend `Packages/FLFloppa Events System/Tests/`).

---

## Scheduler features

* __Sequential__ – Default FIFO order matches subscription sequence.
* __Priority__ – Higher priority handlers run earlier; features can mutate priorities at runtime.
* __Throttled__ – Cooldowns per subscriber prevent hot loops.
* __Backpressure__ – Tracks backlog and can pause/resume handlers when thresholds are exceeded.
* Implement custom schedulers by deriving from `ISubscriptionScheduler<TEvent>` and registering providers.

---

## Editor tooling

* __EventServiceConfigurationAssetInspector__ – Validates required references, summarizes assigned features, and displays scheduler metadata.
* __Feature list inspectors__ – Toggle and reorder subscription/dispatch feature bindings with inline validation.
* __Scheduler asset inspectors__ – Configure defaults (priority baseline, throttle interval, backlog ceilings) with contextual help.

---

## Samples

Import the **Basic Event Service Setup** sample from the Package Manager:

* `Samples~/BasicSetup/EventSystemQuickStartSample.cs` script demonstrates building an `IEventService`, subscribing, and publishing.
* Step-by-step README included to help wire configurators and schedulers.

---

## Documentation

* [`Docs/manual.md`](Docs/manual.md) – Comprehensive manual with editor setup, runtime bootstrapping, feature composition, and scheduler recipes.
* [`Docs/quick_start.md`](Docs/quick_start.md) – Quick start guide, configuration steps, and troubleshooting tips.
* Additional documentation will grow under `Documentation~/` in upcoming releases.

Documentation and changelog URLs are also exposed through `package.json` for Unity Package Manager linking.

---

## Testing

The package is `Assembly Definition` ready. Add edit-mode tests under `Packages/FLFloppa Events System/Tests/` and reference `FLFloppa.Events.Tests`. A sample NUnit fixture is provided to get you started.

---

## Roadmap

* Additional scheduler implementations (round-robin, weighted fair queuing).
* Inspector dashboards for live subscription diagnostics during Play Mode.
* More sample scenes + integration with other FLFloppa packages.
* Automated CI templates and coverage reporting.

See [`CHANGELOG.md`](CHANGELOG.md) for released milestones.

---

## Contributing

Contributions are welcome! Review [`CONTRIBUTING.md`](CONTRIBUTING.md) and adopt the [`CODE_OF_CONDUCT.md`](CODE_OF_CONDUCT.md). Bug reports and feature requests can be filed once the repository is public.

---

## Support & questions

* Open an issue on GitHub (preferred).
* Email `flfloppa@yandex.ru` for private support or partnership inquiries.

---

## License

Released under the [MIT License](LICENSE). You may use, modify, and distribute commercially as long as attribution is preserved.
