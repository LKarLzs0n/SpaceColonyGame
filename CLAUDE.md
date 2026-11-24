# CLAUDE.md — Space Colony (working title)

## How to work with me (READ FIRST)
- I'm learning game programming. **I write and commit ALL the code myself.** Never write or edit my scripts.
- Your job: explain the structure, *why* it's built that way, and give me **clear numbered steps** (which script, which variable, what goes in which method, which Unity menu). Then review my code.
- Small examples to show new syntax are fine. Never the finished answer.
- When I'm stuck: give a hint first, then a bigger hint, then the answer only if I ask.
- When reviewing: point out bugs and bad patterns, explain why, and let me fix them.
- Keep explanations short and beginner-friendly. No walls of text.
- Non-code chores (copying Minecraft structures, docs, checking model files) you can do yourself.
- I program ~3 hours a day. Help me end each session on a small win and note the first task for tomorrow.
- Keep scope small. If I start adding big features before the current one works, remind me.

## Why this exists
**Hack Club entry for the Osaka, Japan trip.** Solo. ~11 hours of coding a week, tracked with **Hackatime**. Deadline: **early February 2027**. It has to be a GOOD game to get confirmed.

## The game
A **cozy space colony builder** in Unity (C#, URP), built on **real-life science**: real ores, real chemistry, real astrophysics. It should make sense.
- You grow a tiny neon town on an asteroid at your own pace, upgrade every building, and feed the giant ring in its centre.
- **Goal: build a Kugelblitz**, a black hole made out of light, with a ring of lasers in the middle of the colony. Tagline idea: "Build a black hole out of light."
- **Not an RTS.** No armies, no combat, no timers, no way to lose. Just the city-building part.
- Inspired by cozy city builders (Let Them Trade), Minecraft Mekanism and my own Minecraft mod **Event Horizon**.

## The vibe
- **Futuristic Tokyo floating in space. Always night.** Narrow stacked buildings, warm lit windows, neon signs, steam vents.
- Light comes in three colours: warm amber, neon pink, neon cyan. One bright accent per building. Bloom on.
- Sky: stars, a nebula, 1–3 **planets built in code** that slowly turn.
- Smooth and dreamy: the camera eases, nothing snaps, soft ambient music, quiet UI.
- **Camera:** angled top-down. WASD pan, scroll zoom, Q/E rotate. Zoomed far out, the colony is one glowing speck in the dark.

## Core loop
Houses bring colonists → colonists work production buildings → everything goes into **one shared stockpile** → spend it on: building and upgrading, exporting through the mass driver (credits → research), or feeding the ring.
- **No conveyors.** Buildings put output straight into the stockpile and take input from it.
- Buildings cost **materials**. Credits only buy **research** (the periodic table is the tech tree).
- If something runs short, buildings just pause. Nothing breaks.

## Buildings
- Every building has **3 levels**, and each level is its own Blockbench model (small → bigger → glowing neon). Upgrades are the main reward.
- Starter set: landing pod, capsule house, hydroponics farm, ice drill, iron mine, carbon pit, smelter (my furnace model), copper mine, wire mill, generator, storage depot, mass driver, research lab. Plus decor: neon signs, lanterns, vending machines, noodle stand.
- **Grid:** 1 cell = 4×4 Minecraft blocks (64×64 Blockbench units). Buildings take whole cells (1×1, 2×2, 3×2). Same scale for every model, pivot at the bottom centre, export as **FBX**, textures with Filter Mode Point + no compression.

## The ring (6 stages + ignition)
1. Foundation (iron plates, copper wire) — must finish within ~10 minutes
2. Laser mounts (steel, motors)
3. Emitters (circuits, wire): the first test pulse ← **the February build ends here**
4. Capacitor bank · 5. Cryo cooling · 6. Star feed (Dyson swarm) · Ignition → updates after February.

## Code architecture
**Data (ScriptableObjects), so new content needs no new code:**
- `ResourceData`: name, icon, export price
- `BuildingData`: name, grid size, and per level: model, build cost, workers, power, inputs/outputs per cycle

**Scripts, in the order I'll write them:**
1. `CameraController`: pan, zoom, rotate, smooth easing
2. `GridManager`: mouse → grid cell, free/occupied cells (`Dictionary<Vector2Int, GameObject>`)
3. `BuildManager`: pick a building, ghost preview, place if affordable
4. `Stockpile`: amounts of each resource, fires an event when it changes (for UI)
5. `Building` (base class): level, upgrade, pause when short of workers/power
6. `ProductionBuilding`: every few seconds take inputs, add outputs
7. `House`: adds colonist space
8. `ColonyManager`: colonists, food, assigning workers
9. `RingProject`: ring stages and what changes when each is done
10. `ResearchManager`: credits and unlocks
11. `PlanetVisual`: a code-built planet that slowly turns
- UI: resource bar, build menu, building info panel

## Week by week
1. Nov 24–30: the camera (rig: pan, rotate, zoom), then grid + placing a cube ← **current**
2. Dec 1–7: `BuildingData`, real models, ghost preview, costs
3. Dec 8–14: stockpile, mines + smelter producing, resource bar
4. Dec 15–21: houses, colonists, workers, food
5. Dec 22 – Jan 4 (holidays, lighter): upgrades, build menu
6. Jan 5–11: the ring, stages 1–3
7. Jan 12–18: power, mass driver credits, research
8. Jan 19–25: vibe pass (lighting, bloom, neon, code planets, sound)
9. Jan 26 – Feb 1: save/load, friends playtest, tuning
10. Feb 2–6: polish, build, itch page, last devlog, submit
Stretch only if ahead: drones, shuttles landing with colonists, one captured planet.

## Current task details
**CameraController** (on an empty `CameraRig` at ground level, Main Camera as its child at (0, 12, -12), rotated 45° down).
- Pan with WASD: move the rig along its own `transform.forward` / `transform.right`, times speed and `Time.deltaTime`.
- Rotate with Q/E: `transform.Rotate` around Y.
- Then zoom with the scroll wheel (slide the camera along its own forward, clamped), then smoothing for the dreamy feel.
- This project uses the **new Input System only** (`activeInputHandler: 1`): use `using UnityEngine.InputSystem;` and `Keyboard.current` / `Mouse.current`. The old `Input.GetKey` throws errors here.

**After the camera:** GridManager: mouse click → grid cell → spawn a cube snapped to that cell. Hints: raycast from the camera to the ground, round the hit position to the grid with `Mathf.Floor`, store occupied cells in a dictionary.

## Notes and gotchas
- **Lunar Client saves Minecraft structures to `~\.lunarclient\profiles\cf\saves\<world>\generated\minecraft\structure\`**, not `.minecraft`. Copy them over (or teach my converter that folder).
- Minecraft-converted models come in as **one object per block with every face** (furnacev3.fbx = 1,216 cubes, ~14.6k triangles). Later task: merge into one mesh and skip faces touching another block, in my converter.

## Planning docs
- Core Game Plan (current): https://claude.ai/artifact/QFXMZMtB2Z7NbGXHbjszdV
- Kugelblitz Field Manual (the path to the ring): https://claude.ai/artifact/DXvF5TVmeGYxhT3n5goWDP
- Machine Map (old factory version, real-science chain still useful): https://claude.ai/artifact/UDLtQwbyotyhK2A6To7TPj

## Progress log
(Update this section at the end of each session: what got done, what's next.)
- **2026-11-24:** Planned the game and pivoted from factory tycoon to cozy space colony builder. Modelled the furnace (v3, final) in Minecraft, painted and exported it to FBX. No code yet. **Next:** make the Unity project (Unity 6 URP, Hackatime in the code editor, Git + Unity `.gitignore`), then GridManager: a cube snapping to the grid.
