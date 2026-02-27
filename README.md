# Frontline TD

Frontline TD is a 2D tower defense game built with Unity.

The player places military-themed units on build plots, defends the path from multiple enemy types, and survives increasingly difficult waves. The project includes menu flow, map selection, shop/tower placement, wave spawning, projectile combat, and game-over handling.

## Tech Stack

- Engine: Unity `6000.3.9f1`
- Language: C#
- Render Pipeline: Universal Render Pipeline (URP)
- Input: Unity Input System (`com.unity.inputsystem`)

## Repository Structure

- `Frontline TD/`: Unity project root
- `Frontline TD/Assets/Scenes/`: Main scenes (`MainMenu`, `MapSelector`, `SampleScene`, `Map 2`)
- `Frontline TD/Assets/Scripts/`: Core gameplay scripts
  - `Mangers/`: player, level, build, and wave/spawn managers
  - `Units/`: tower behavior
  - `Enemy/`: enemy movement/types
  - `Projectiles/`: projectile and explosion logic
  - `UI/`: menus, settings, scene transitions

## Gameplay Systems (High-Level)

- **Building**: `BuildManager` tracks money and selected tower.
- **Plot placement**: `Plot` handles hover, click-to-build, and range visualization.
- **Waves**: `EnemySpawner` supports intro waves (`WaveDefinition`) and infinite procedural scaling.
- **Combat**: `Unit`-based towers detect targets and fire projectiles.
- **Progression pressure**: enemy count, HP, speed, and spawn cadence increase over time.
- **Failure state**: `PlayerManager` tracks lives and triggers game over through UI.

## Local Development Setup

### Prerequisites

1. Install **Unity Hub**.
2. Install Unity Editor version **`6000.3.9f1`** in Unity Hub.
3. Install a Git client.

### Clone and Open

```bash
git clone <your-repo-url>
cd frontline-git
```

In Unity Hub:

1. Click **Add** (or **Add project from disk**).
2. Select the `Frontline TD` folder inside this repo.
3. Open the project with Unity `6000.3.9f1`.

Unity will resolve packages from `Packages/manifest.json` on first open.

## Run Locally (Editor)

1. Open one of the gameplay scenes:
   - `Assets/Scenes/SampleScene.unity`
   - `Assets/Scenes/Map 2.unity`
2. Press **Play** in the Unity Editor.

Current build scene order is configured in `ProjectSettings/EditorBuildSettings.asset`:

1. `MainMenu`
2. `MapSelector`
3. `SampleScene`
4. `Map 2`

## Deploy to Local Machines (Standalone Build)

Use this when you want a runnable build for your own PC or another local machine.

### Build Steps (Unity GUI)

1. Open the project in Unity.
2. Go to **File > Build Profiles** (or **Build Settings**, depending on Unity UI).
3. Select a target platform:
   - Windows (`Windows x86_64`)
   - macOS (`macOS`)
   - Linux (`Linux x86_64`)
4. Confirm the required scenes are included in build.
5. Click **Build** and choose an output folder.

### Run on the Target Machine

- **Windows**: run the generated `.exe` with its data folder.
- **macOS**: run the generated `.app` bundle.
- **Linux**: run the generated executable (ensure execute permission if needed: `chmod +x <binary>`).

### Local Distribution Notes

- Keep the executable/app and generated data folders together.
- If audio/settings seem wrong, delete old local saves/config and relaunch.
- For best compatibility, build on the same OS you plan to run, or validate cross-platform behavior carefully.

## Common Issues

- **Wrong Unity version**: open with `6000.3.9f1` to avoid serialization/package issues.
- **Missing packages/compile errors on first launch**: close Unity, reopen project, and let package restore complete.
- **Scenes not loading in build**: verify scene list/order in Build Profiles/Build Settings.

## License

This repository includes an MIT license file at the root: `LICENSE`.
