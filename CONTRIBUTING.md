# Contributing

Thanks for taking the time to contribute to the FLFloppa Events System! We welcome bug reports, feature requests, documentation improvements, and pull requests.

## Getting started

* __Fork the repository__ – Create your own fork and clone it locally.
* __Install dependencies__ – Use Unity 2022.3 LTS or newer. Install [UniTask](https://github.com/Cysharp/UniTask) (v2.3.3+) into the Unity project.
* __Open the project__ – Launch the Unity Editor and open the `Package Dev` workspace. The package lives under `Packages/FLFloppa Events System/`.

## Reporting issues

* Search existing issues before opening a new one.
* Include Unity version, package version, operating system, and reproduction steps.
* Attach screenshots, logs, or minimal repro projects when possible.

## Pull requests

1. Create a feature branch from `main` (e.g., `feature/scheduler-round-robin`).
2. Follow the coding standards outlined below.
3. Update or add documentation/tests relevant to your change.
4. Ensure `package.json` version is bumped only by maintainers prior to release.
5. Open a pull request with a clear description of the problem and solution. Reference related issues.

## Coding guidelines

* __Runtime__ – Strive for allocation-free code paths. Avoid LINQ in hot loops. Use `readonly struct` where practical.
* __Editor__ – Prefer UI Toolkit for inspectors. Keep ScriptableObject assets under `Runtime/` and editor-only tooling under `Editor/`.
* __Naming__ – Follow C# conventions (PascalCase for types/methods, camelCase for locals/fields except private serialized fields which remain `camelCase`).
* __Assemblies__ – Update `.asmdef` references when introducing new dependencies.
* __Tests__ – Add NUnit tests under `Packages/FLFloppa Events System/Tests/` when fixing bugs or adding features that affect runtime behavior.

## Documentation

* Update `Docs/quick_start.md`, `README.md`, and the sample README when behaviour changes.
* Changelog entries belong in `CHANGELOG.md` under the `UNRELEASED` section until a release tag is cut.

## Commit style

Use descriptive commit messages. Prefix with the area of impact when helpful (e.g., `runtime: add priority scheduler hooks`).

## Code of Conduct

Participation in this project is governed by the [Contributor Covenant Code of Conduct](CODE_OF_CONDUCT.md). Please review it before contributing.

Thank you for helping improve the FLFloppa Events System! 🎉
