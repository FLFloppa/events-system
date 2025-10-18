# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [0.1.0] - 2025-10-17
### Added
* Core event service (`EventService`, `Topic<TEvent>`, `SubscriptionRecipe`, `DispatchRecipe`).
* Scheduler abstraction with sequential, priority, throttled, and backpressure implementations.
* Subscription feature commands for priority and throttling.
* ScriptableObject configurators and inspectors for subscription/dispatch feature collections and schedulers.
* Diagnostics provider hook and topic statistics reporting.
* Sample scene assets (`Samples~/BasicSetup`) and `EventServiceQuickStartSample` MonoBehaviour.
* Quick start documentation (`Docs/quick_start.md`) and package README.

### Changed
* Initial public release.

[0.1.0]: https://github.com/FLFloppa/events-system/releases/tag/v0.1.0
