## Copilot / Agent Instructions for this repo

Purpose: concise, repo-specific guidance so AI coding agents can be immediately productive working on this Unity project.

1) Big picture
- Unity game project — open with the Editor version in [ProjectSettings/ProjectVersion.txt](ProjectSettings/ProjectVersion.txt). Core content lives in `Assets/` (Scenes, Prefabs, Scripts, SciptableObjects).
- Rendering/Input/nav: project uses URP and the Unity Input System (see `Packages/manifest.json`) and has custom nav code under `NavMeshPlus/` and `NavMeshComponents/` at repo root.

2) Key places to inspect (quickly)
- Gameplay code: `Assets/Scripts/` — example: `Assets/Scripts/Misc/FlashBlick.cs` shows MonoBehaviour patterns and serialized fields.
- Data/config: `Assets/SciptableObjects/` (note spelling) — ScriptableObjects live here and drive many systems.
- Scenes & wiring: `Assets/Scenes/`, `Assets/Prefabs/` — prefabs and scene instances are the canonical runtime wiring points.
- Project/meta: `ProjectSettings/`, `Packages/manifest.json`, and root csproj files (`Assembly-CSharp.csproj`, `NavMeshPlus.csproj`).

3) Build / run / test workflows
- Primary: use the Unity Editor (match ProjectVersion.txt) for Play Mode, scene iteration, and regenerating IDE files (opening the project regenerates `Assembly-CSharp.csproj`).
- Headless / CLI (template):
```
"C:\Path\To\Unity.exe" -batchmode -projectPath "<repo-root>" -quit -executeMethod <Method> -buildTarget Win64 -logFile build.log
```
- Look for a `BuildScript` before calling `-executeMethod`; otherwise run builds from the Editor.
- Tests: uses `com.unity.test-framework`. Run EditMode/PlayMode tests via the Unity Test Runner in the Editor.

4) Project conventions & patterns
- Prefab-first wiring: most components are referenced via serialized fields on MonoBehaviours. Inspect prefab instances and scene objects rather than assuming runtime DI.
- Script layout: `Assets/Scripts/` with subfolders. Follow nearby files' naming and namespace style.
- ScriptableObjects: used for project data/config; changing their fields can break serialized references in scenes/prefabs.

5) Integration points & dependencies to be aware of
- URP: `com.unity.render-pipelines.universal` — rendering assets under `UniversalRenderPipelineGlobalSettings.asset`.
- Input System: `com.unity.inputsystem` and `PlayerInputActions/` generated assets.
- Navigation: `NavMeshComponents/`, `NavMeshPlus/` and `com.unity.ai.navigation` — editing nav code may require regening nav assets in Editor.

6) Editing safety checklist (critical)
- Do NOT rename or change types of serialized fields on MonoBehaviours without also updating prefabs/scenes in the Editor.
- If you add/remove public/serialized fields, open affected scenes/prefabs in the Editor to reserialize and save.
- Prefer small, incremental changes and validate via Play Mode in the Editor.

7) Quick searches & useful entrypoints
- Engine version: [ProjectSettings/ProjectVersion.txt](ProjectSettings/ProjectVersion.txt)
- Packages: [Packages/manifest.json](Packages/manifest.json)
- ScriptableObjects: [Assets/SciptableObjects/](Assets/SciptableObjects/)
- Example script: [Assets/Scripts/Misc/FlashBlick.cs](Assets/Scripts/Misc/FlashBlick.cs)
- Nav projects: [NavMeshPlus/](NavMeshPlus/)

8) Merge guidance for agents
- If this file exists, merge rather than overwrite—preserve repository-specific notes and append new, verifiable findings.

If you'd like, I can expand any section (example: a short runbook for headless builds, a checklist for safe serialized-field refactors, or a list of high-priority scripts to review). Please tell me which section to expand.
