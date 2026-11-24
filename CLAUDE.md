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
- **A living town.** Tiny people walk, carry and work, and every building feeds the next one. What I love most in city builders: how every building works together, and watching small people walk around the town and work.
- You grow a neon town on an asteroid at your own pace, upgrade every building, and build the giant ring in its centre.
- **Goal: build a Kugelblitz**, a black hole made out of light, with a ring of lasers in the middle of the colony. Tagline idea: "Build a black hole out of light."
- **Not an RTS.** No armies, no combat, no timers, no way to lose. Just the city-building part.
- **Dropped on 2026-11-24 (don't bring back):** colonist classes that level up, and a research tree (the periodic-table "Element Board"). I want buildings working together, not upgrading people.
- Inspired by cozy city builders (Let Them Trade), Minecraft Mekanism and my own Minecraft mod **Event Horizon**.

## The vibe
- **Futuristic Tokyo floating in space. Always night.** Narrow stacked buildings, warm lit windows, neon signs, steam vents.
- **The 3 key references live in `_References/`** (git-ignored: other people's images, never post them). What they have in common, and what every building should carry:
  - **Tight, tall, blocky complexes** packed together like a back alley; dark, grimy walls (near-black blue/purple).
  - **Stacked neon signs** hanging off walls, vertical sign boards with glyphs, lots of them. Upgrades add floors AND signs.
  - **Cables and pipes strung between buildings**, AC units, vents, clutter.
  - **Steam and haze** drifting through, colour fog in the distance.
  - **Wet, glossy ground** that reflects the neon.
  - Neon colours: **pink/magenta, cyan, green, warm amber/yellow**, on a dark base. Bloom on.
- Our own signs and glyphs only: no copied logos or characters (ref 2 has fan art in it).
- **Rain over the floating city.** The town sits under a clear dome (it needs air anyway); its own water cycle (ice drills, farms) condenses on the glass and rains back down. The first rain is a reward (ring part 2).
- **The little people:** tiny blocky Blockbench figures; code swings their arms and legs like Minecraft mobs, so no hand animation. Each carries a **tiny lantern**: zoomed out, glowing dots flow between the buildings.
- Sky: stars, a nebula, 1–3 **planets built in code** that slowly turn.
- Smooth and dreamy: the camera eases, nothing snaps, soft cozy music, quiet UI.
- **Camera:** angled top-down. WASD pan, scroll zoom, Q/E rotate. Zoomed far out, the colony is one glowing speck in the dark.

## How the town works (core loop)
1. Houses give colonists. Each colonist gets a job at a building with a free worker slot.
2. Workers **walk** (NavMesh) from home to work.
3. **Producers** (mines, ice drill, farm): work → carry the output to the nearest **storage depot** → repeat.
4. **Processors** (smelter, wire mill, ...): walk to the depot, pick up the inputs → carry them back → work → carry the output to the depot.
5. The **stockpile** = everything in the depots. Building and upgrading spend from it.
6. Workers carry materials to the **ring** to build it.
- **The layout is the cozy puzzle:** short walks = a faster town. Mines near depots, houses near work, so the town packs into tight alleys on its own.
- **No conveyors.** If something runs short, workers wait and buildings pause. Nothing breaks, nobody leaves.
- **Food:** farms → depot; colonists eat from the stockpile; no food = no new colonists. **Power:** generators add to one number; buildings past the first few use it.

## Buildings (February build)
- Every building has **3 levels**, and each level is its own Blockbench model (small → bigger → glowing neon). Upgrading = more worker slots and faster work. Upgrades are the main reward.
- **From the start:** landing pod, capsule house, storage depot, ice drill, hydroponics farm, iron mine, carbon pit, smelter (my furnace model), copper mine, wire mill, generator.
- **Unlocked by ring part 1:** steel mill, motor works.
- **Unlocked by ring part 2:** quartz quarry, arc furnace, chip fab.
- **Decor (vibe only):** neon signs, paper lanterns, vending machines, noodle stand, utility poles with tangled cables.
- **After February:** mass driver (trade), research lab.
- **Modelling order:** level 1 of every building first (in the order above), then all the level 2s, then the level 3s. Same footprint for all 3 levels: they grow up, never out. Reserve 4 Minecraft blocks only for neon (pink, cyan, green, amber) so exactly those can glow in Unity.
- **Grid: fine and invisible (decided 2026-11-24).** 1 cell = **1 Minecraft block** (16 Blockbench units = 1 Unity unit; check the FBX import scale), so placing feels almost free but voxel buildings always line up, pixel edge to pixel edge. Footprints can be any whole number of blocks (a 5×7 noodle shop is fine). The grid only shows while placing. Buildings rotate in 90° steps (R key; Q/E is the camera). The ghost glides to the snapped spot with `Lerp`, so snapping feels free. Decor (signs, cables, lanterns, steam) hangs over the edges to break up the boxes. Not fully free placement: voxel buildings at odd angles clash, and tight alleys need buildings flush.
- Same scale for every model, pivot at the bottom centre, export as **FBX**, textures with Filter Mode Point + no compression.

**The chains (real science):**
- Ice → water; water + power → food (grow lights really glow pink)
- Iron ore (hematite) + carbon → iron, in the smelter (carbon steals the oxygen: a real blast furnace)
- Copper ore (chalcopyrite) → copper, in the smelter (each smelter is set to iron or copper); copper → wire
- Iron + carbon → steel; steel + wire → motors
- Quartz + carbon → silicon (arc furnace); silicon + wire → circuits
- Carbon → power (generator)

## The ring (3 parts for February)
The big build in the town centre, like a Wonder. Workers carry materials to it. Each finished part changes how it looks and sounds, and unlocks new buildings.
1. **The base** (iron + copper wire): the floor lights up, a soft beat joins the music. Must finish within ~10 minutes.
2. **The laser towers** (steel + motors): the towers rise, the dome seals, **the first rain falls**.
3. **The first flash of light** (circuits + wire): a test pulse lights the sky, then fizzles. ← **the February build ends here**
- After February: capacitor bank, cryo cooling, star feed (Dyson swarm), ignition.

## Code architecture
**Data (ScriptableObjects), so new content needs no new code:**
- `ResourceData`: name, icon
- `BuildingData`: name, grid size, unlocked by (ring part), and per level: model, build cost, worker slots, power, inputs/outputs per cycle, work time

**Scripts, in the order I'll write them:**
1. `CameraController`: pan, zoom, rotate, smooth easing ✓
2. `GridManager`: mouse → grid cell, free/occupied cells (`Dictionary<Vector2Int, GameObject>`). A cell is one block; a building fills every cell under its footprint.
3. `BuildManager`: pick a building, ghost preview, place if affordable
4. `Stockpile`: amounts of each resource, fires an event when it changes (for UI)
5. `Building` (base class): level, upgrade, worker slots, pause when short
6. `ProductionBuilding`: inputs → work time → outputs (first on a plain timer, before workers exist)
7. `House`: adds colonist space
8. `ColonyManager`: colonists, food, gives each colonist a job
9. **`Worker`: the heart of the game.** A `NavMeshAgent` + a state machine (walk to work → work → carry to depot → fetch inputs → ...), the code limb swing and the lantern. I've built state machines + NavMesh before in an RTS, so build on that.
10. `RingProject`: the 3 parts, takes deliveries, what changes when each is done, unlocks
11. `PlanetVisual`: a code-built planet that slowly turns
- UI: resource bar, build menu, building info panel

## Week by week
1. Nov 24–30: the camera ✓, grid + placing/removing cubes ✓ (both done on day 1, ahead of schedule)
2. Dec 1–7: `BuildingData`, real models, ghost preview, costs ← **current** (started early)
3. Dec 8–14: stockpile, production buildings on a timer (no workers yet), resource bar
4. Dec 15–21: houses, colonists, **workers**: NavMesh walking + the state machine (to work, work, carry to the depot)
5. Dec 22 – Jan 4 (holidays, lighter): processors fetching inputs (the chains), upgrades, build menu
6. Jan 5–11: the ring: 3 parts, workers deliver to it, unlocks
7. Jan 12–18: food + power, the little people's look (blocky figures, limb swing, lanterns)
8. Jan 19–25: vibe pass (lighting, bloom, neon, rain, code planets, music)
9. Jan 26 – Feb 1: save/load, friends playtest, tuning (ring part 1 under 10 minutes)
10. Feb 2–6: polish, build, itch page, last devlog, submit
Stretch only if ahead: drones, shuttles landing with colonists, mass driver trade.

## Current task details
**Next (first task of the next session): move placing into `BuildManager`.** `GridManager` should only answer "where" and keep track of which cells are taken; `BuildManager` decides "what to build". So the click handling, `Instantiate` and `Destroy` move to `BuildManager`, and `GridManager` gets small public methods for the cells (e.g. is a cell free, mark it taken, free it). After that: `BuildingData` + the furnace model on the grid with a real footprint.
- Later, for footprints: a building whose corner cell is (x, z) and size is w × d sits at (x + w/2, 0, z + d/2), because its pivot is the bottom centre. A 90° turn swaps w and d.

**Ghost preview (done):** a `Ghost` cube lives in the scene (transparent URP `GhostMat`, **no collider** so the mouse ray ignores it, scale 1.02 against flicker, no shadows). `BuildManager.Update` asks `gridManager.TryGetMouseCell`; if it hits, the ghost shows and `Lerp`s to `gridManager.GetCellCenter(cell)` (`ghostSmoothing` 15), else `SetActive(false)`. `TryGetMouseCell` and `GetCellCenter` are public on `GridManager`; `PlaceCube` uses `GetCellCenter` too.

**GridManager (done):** `TryGetMouseCell(out Vector2Int cell)` raycasts from the camera through the mouse, nudges the hit point back along `hit.normal` by 0.01 (so clicks on a cube's side land in that cube's cell), and `Mathf.FloorToInt`s it into a cell. `cellSize` = 1 (one Minecraft block). Left click → `PlaceCube` (skips taken cells, spawns at the cell centre, `Add`s to `Dictionary<Vector2Int, GameObject>`); right click → `RemoveCell` (`TryGetValue` → `Destroy` + `Remove`).
- This project uses the **new Input System only** (`activeInputHandler: 1`): use `using UnityEngine.InputSystem;` and `Keyboard.current` / `Mouse.current` (mouse position: `Mouse.current.position.ReadValue()`). The old `Input.GetKey` throws errors here.
- Scroll gives exactly +1 / -1 per notch for one frame (no `Time.deltaTime` for scroll).

**Camera (done):** `CameraController` on an empty `CameraRig` at ground level; Main Camera is its child. Pan (WASD, normalized) and zoom (scroll) both use target + `Lerp` smoothing; Q/E rotate the rig; zoom sets the camera's `localPosition` to (0, zoom, -zoom). Later polish: pan speed scaling with zoom, smoothed rotation, and **street-level tilt**: the closer you zoom, the flatter the camera angle (45° far out → ~15° fully in), so zooming in turns into the reference alley shots.

## Notes and gotchas
- **Lunar Client saves Minecraft structures to `~\.lunarclient\profiles\cf\saves\<world>\generated\minecraft\structure\`**, not `.minecraft`. Copy them over (or teach my converter that folder).
- Minecraft-converted models come in as **one object per block with every face** (furnacev3.fbx = 1,216 cubes, ~14.6k triangles). Later task: merge into one mesh and skip faces touching another block, in my converter.
- **Save/load (week 9):** save the buildings, levels and stockpile only. On load, workers just restart their loop; never try to save a worker halfway through a walk.

## Planning docs
- Core Game Plan (current): https://claude.ai/artifact/QFXMZMtB2Z7NbGXHbjszdV
- Kugelblitz Field Manual (the path to the ring, old factory version; the science still holds): https://claude.ai/artifact/DXvF5TVmeGYxhT3n5goWDP
- Machine Map (old factory version, real-science chain still useful): https://claude.ai/artifact/UDLtQwbyotyhK2A6To7TPj

## Progress log
(Update this section at the end of each session: what got done, what's next.)
- **2026-11-24:** Planned the game and pivoted from factory tycoon to cozy space colony builder. Modelled the furnace (v3, final) in Minecraft, painted and exported it to FBX. No code yet. Then: made the Unity project `SpaceColonyGame` (6000.5.6f1, URP), Git + GitHub repo (public: https://github.com/LKarLzs0n/SpaceColonyGame), and wrote **CameraController**: WASD pan (normalized, smoothed), Q/E rotate, scroll zoom (smoothed, clamped). Learned: camera rig, `position` vs `localPosition`, `Time.deltaTime`, `normalized`, `Mathf.Clamp`, `Lerp` for ease-out. Posted devlog #1 on Hack Club (Build Mode pipeline screenshot + camera clip). Later the same day, the design became **a living town**: little people walk, carry and work (NavMesh + a worker state machine), every building feeds the next through storage depots, and the ring is a 3-part Wonder that unlocks new buildings. Classes and research were dropped; rain under the dome was added. The grid became fine and invisible (1 cell = 1 block). Then wrote **GridManager**: click to place cubes snapped to the grid, right-click to remove, side clicks fixed. Learned: raycasts, `out` parameters and the `TryGet` pattern, early `return`, `Dictionary` (`Add`, `ContainsKey`, `TryGetValue`, `Remove`), `Mathf.FloorToInt` vs `(int)` on negatives, `hit.normal`, and the if-without-braces trap. Then started **BuildManager**: a see-through ghost cube glides to the cell under the mouse. Learned: one script calling another (`public` methods + a `[SerializeField]` script slot), returning a value from a method, `SetActive`, a transparent URP material, and why visuals shouldn't have colliders. **Next:** move placing into `BuildManager` (see Current task details), then `BuildingData` + the furnace on the grid.
