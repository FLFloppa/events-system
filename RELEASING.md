# Releasing FLFloppa Events System

This guide walks through preparing and publishing a tagged GitHub release for the package.

## 1. Pre-release checklist
* __Bump version__ – Update `package.json` and `CHANGELOG.md` with the new semantic version (e.g., `0.1.0`).
* __Verify documentation__ – Ensure `README.md`, `Docs/quick_start.md`, `Samples~/BasicSetup/README.md`, and `CONTRIBUTING.md` reflect the current feature set.
* __Review samples__ – Confirm the sample assets under `Samples~/BasicSetup/Resources/` and the scene in `Samples~/BasicSetup/Scenes/` load without errors.
* __Run tests__ – Execute edit-mode tests in `Packages/FLFloppa Events System/Tests/` (via Unity Test Runner or command line).
* __Build validation__ – Open the sample scene and play the demo (`EventServiceQuickStartSample`) to confirm event flow.

## 2. Commit and push
1. Stage all changes, including documentation and metadata updates.
2. Commit with a descriptive message (e.g., `release: prepare v0.1.0`).
3. Push to the main branch of `events-system` repository.

## 3. Tag the release
1. Create an annotated tag locally:
   ```bash
   git tag -a v0.1.0 -m "FLFloppa Events System v0.1.0"
   git push origin v0.1.0
   ```
   Replace `0.1.0` with the new version number.

## 4. Draft GitHub release notes
1. Navigate to **GitHub → Releases → Draft a new release**.
2. Choose the tag (e.g., `v0.1.0`) and set the release title to match.
3. Paste the highlights from `CHANGELOG.md` ("Added"/"Changed" sections).
4. Attach optional artifacts:
   * `.unitypackage` export (see Section 5).
   * Zip file containing the `Packages/FLFloppa Events System/` folder if needed.
5. Publish the release once content is reviewed.

## 5. Export .unitypackage (optional)
1. In Unity, open **Assets → Export Package…**.
2. Select the `Packages/FLFloppa Events System/` folder and ensure sample assets are included.
3. Export as `FLFloppa.Events.System.v0.1.0.unitypackage` and attach it to the GitHub release.

## 6. Post-release
* Update project boards or internal trackers with the new version.
* Announce the release (Discord, forums, etc.).
* Create an `UNRELEASED` section in `CHANGELOG.md` for upcoming work.

## 7. Preparing next iteration
* Branch from `main` for the next feature cycle.
* Reset sample assets if you want them pristine (duplicate as needed).

Following this checklist keeps GitHub releases consistent and ensures Unity users can install the package via Git URL or `.unitypackage` download without surprises.
