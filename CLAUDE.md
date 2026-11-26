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
- **Art time counts too, up to 30% of the project's total.** Track modelling/art with **Lapse** (lapse.hackclub.com), attached to this game's Hackatime project. So every 7 h of code lets ~3 h of art count (11 h code a week ≈ 4.7 h of art). Art beyond that doesn't count, so code stays the main thing.

## The game
A **cozy space colony builder** in Unity (C#, URP), built on **real-life science**: real ores, real chemistry, real astrophysics. It should make sense.
- **A living town.** Tiny people walk, carry and work, and every building feeds the next one. What I love most in city builders: how every building works together, and watching small people walk around the town and work.
- You grow a neon town on an asteroid at your own pace, upgrade every building, and build the giant ring in its centre.
- **Goal: build a Kugelblitz**, a black hole made out of light, with a ring of lasers in the middle of the colony. Tagline idea: "Build a black hole out of light."
- **Not an RTS.** No armies, no combat, no timers, no way to lose. Just the city-building part.
- **It's YOUR city (decided 2026-11-26).** Chill, and you build it from scratch your way. No rules on where things go: no zones, no required roads, no "must be next to X". The only hard rules are overlap and cost. Optimal spots exist, but only through **walking distance**, which you *see* (workers taking the long way), never as a number, a warning or a red X. Boulders and ice lying around the pod give natural spots to build near without forcing anything. Decor from the start and R-rotate are part of making it yours.
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

## The big picture (locked 2026-11-26)
**The heartbeat, running all game:** place a building → workers walk and carry → the depot fills → spend it on more town. **The layout is the puzzle** (short walks = a faster town). **The ring is why you keep going:** each part needs a longer chain, and pays you back with something you see and hear.

One playthrough (times are guesses, tune them at the playtest):
| Time | What you do | What you get |
|---|---|---|
| 0 min | The pod lands; 2 people grab crates and rocks | The first houses, more people move in |
| ~5 min | The town wakes up: ice drill, farm, food | A rusty boulder: iron ore, the mine unlocks |
| ~10 min | First chains: iron, carbon, copper, wire | **Ring part 1, the base:** the floor glows, a beat joins the music |
| ~30 min | Steel and motors, upgrades, tighter alleys | **Ring part 2, the laser towers:** the dome seals, the first rain |
| ~1 h | Silicon and chips: quartz, arc furnace, chip fab | **Ring part 3, the first flash:** a pulse lights the sky. The February build ends here |

## How the town works (core loop)
1. Houses give colonists. Each colonist gets a job at a building with a free worker slot.
2. Workers **walk** (NavMesh) from home to work.
3. **Producers** (mines, ice drill, farm): work → carry the output to the nearest **storage depot** → repeat.
4. **Processors** (smelter, wire mill, ...): walk to the depot, pick up the inputs → carry them back → work → carry the output to the depot.
5. The **stockpile** = everything in the depots. Building and upgrading spend from it.
6. Workers carry materials to the **ring** to build it.
- **The layout is the cozy puzzle:** short walks = a faster town. Mines near depots, houses near work, so the town packs into tight alleys on its own.
- **Home and rest (locked 2026-11-26).** The house that spawns a colonist is its **home**. After a few deliveries (start with 5) a worker gets tired, walks home, **goes inside** (hidden) and rests ~10 s, then walks back to work. Everyone rests on their **own clock**, never all at once: there's no day/night (the town is always night), so the town never freezes and some lanterns are always moving. A house far from work = long commutes = a slower town, which you see, never as a number. Phase 5: a house's windows light up while someone is inside.
- **No conveyors.** If something runs short, workers wait and buildings pause. Nothing breaks, nobody leaves.
- **Food:** farms → depot; colonists eat from the stockpile; no food = no new colonists. **Power:** generators add to one number; buildings past the first few use it. The landing pod gives a little starting power, enough for the first farm before the generator unlocks.

## The opening (the first ~10 minutes)
- The landing pod touches down in the dark with **2 colonists**. Around it lie **cargo crates** (food + scrap metal), **grey boulders** (rock) and **ice chunks**. No trees: it's an asteroid.
- **You name your colony as the pod lands (decided 2026-11-26).** One quiet text box, nothing else. The name lights up on the pod's neon sign and goes into the save, so the city is *yours* from second one. Built in phase 5 with the UI.
- You click things to **mark** them; idle colonists walk over and carry them to the pod (the first storage).
- Rock + scrap → the first capsule house → more colonists → a depot and an ice drill → water → the farm.
- Someone hauls in a **rusty red boulder**: "Iron ore discovered!" and the iron mine unlocks. From there the chains take over.
- Gathering by hand needs workers, so it gets built in phase 3 together with the `Worker`.

## Unlocks: discover it, unlock it
- **At the start:** the landing pod (already standing, gives a little power), capsule house, storage depot, ice drill, and all the decor.
- **The first time a material reaches a depot, every building that uses it unlocks:** water → hydroponics farm; iron ore (hematite, rusty red) → iron mine + smelter; carbon (black) → carbon pit + generator; copper ore (chalcopyrite, brassy) → copper mine; copper → wire mill.
- New ores first show up as boulders lying around the colony, in their real colours, so you find them by looking around.
- **The ring's parts unlock the big tiers:** part 1 → steel mill + motor works; part 2 → quartz quarry, arc furnace, chip fab.

## Buildings (February build)
- Every building has **3 levels**, and each level is its own Blockbench model (small → bigger → glowing neon). Upgrading = more worker slots and faster work. Upgrades are the main reward.
- **Base buildings (some at the start, the rest discovered, see Unlocks):** landing pod, capsule house, storage depot, ice drill, hydroponics farm, iron mine, carbon pit, smelter (my furnace model), copper mine, wire mill, generator.
- **Unlocked by ring part 1:** steel mill, motor works.
- **Unlocked by ring part 2:** quartz quarry, arc furnace, chip fab.
- **Decor (vibe only):** neon signs, paper lanterns, vending machines, noodle stand, utility poles with tangled cables. Available from the start and cheap, so the town looks cozy early.
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
- `BuildingData`: name, grid size, placeholder colour, unlocked by (a discovered resource or a ring part), and per level: model, build cost, worker slots, power, inputs/outputs per cycle, work time

**Scripts, in the order I'll write them:**
1. `CameraController`: pan, zoom, rotate, smooth easing ✓
2. `GridManager`: mouse → grid cell, free/occupied cells (`Dictionary<Vector2Int, GameObject>`). A cell is one block; a building fills every cell under its footprint. ✓
3. `BuildManager`: pick a building, ghost preview, place if affordable ✓
4. `Stockpile`: amounts of each resource ✓; later: fires an event when it changes (for UI), and remembers which resources it has ever seen (for unlocks)
5. `Building` (base class): data, footprint, stockpile ✓; later: level, upgrade, worker slots, pause when short
6. `ProductionBuilding`: inputs → work time → outputs, on a plain timer ✓ (workers take over in phase 3)
7. `House`: adds colonist space
8. `ColonyManager`: colonists, food, gives each colonist a job
9. **`Worker`: the heart of the game.** A `NavMeshAgent` + a state machine (walk to work → work → carry to depot → fetch inputs → ...), the code limb swing and the lantern. Walking, depots, jobs and the work-and-carry state machine ✓; fetching inputs, limb swing and lantern later.
10. `Gatherable`: crates, boulders and ice chunks for the opening; click to mark, idle workers carry them in
11. `RingProject`: the 3 parts, takes deliveries, what changes when each is done, unlocks
12. `PlanetVisual`: a code-built planet that slowly turns
- UI: resource bar, build menu, building info panel

## Roadmap: mechanics first (locked 2026-11-26)
**Rules:**
- Every building is a **coloured placeholder cube** (one colour per type) until phase 5.
- **No real UI until phase 5:** debug text on screen is enough.
- Models get made alongside the code in Blockbench (tracked in Lapse, art ≤30%), but they only go into the game in phase 5.
- Goal: by early January the whole playthrough (see The big picture) plays from start to end with ugly cubes.

**Phases:**
1. **Building** (Nov 24 – Dec 3): the camera ✓, grid + placing/removing ✓, ghost ✓, `BuildManager` takes over placing ✓, `BuildingData` (name, size, colour) ✓, footprints ✓, number keys ✓, R rotate ✓, centred under the mouse ✓. **Done on Nov 26, a week early.** Costs moved to phase 2 (they need the `Stockpile`).
2. **Economy** (Dec 4–14): `ResourceData` ✓, `Stockpile` ✓, costs + full refunds ✓, `ProductionBuilding` on a plain timer ✓, debug text ✓, the first chain (ice drill → water → farm → food) ✓. **Done on Nov 26 too, ~1.5 weeks early.**
3. **The living town** (Dec 15 – Jan 4, holidays lighter; can start now) ← **current**: `House`, `ColonyManager`, **`Worker`** (NavMesh + state machine), depots, processors fetching inputs (the chains), the opening (`Gatherable`: marking crates and boulders), discover-to-unlock.
4. **The goal** (Jan 5–11): `RingProject` (3 parts, deliveries, unlocks), upgrades, food + power.
5. **Make it beautiful** (Jan 12–25): real models, build menu, resource bar, naming your colony, house windows that light up when someone's home, the little people's look (blocky figures, limb swing, lanterns), neon, bloom, rain, code planets, music.
6. **Ship it** (Jan 26 – Feb 6): save/load, friends playtest, tuning (ring part 1 under 10 minutes), itch page, last devlog, submit.

Stretch only if ahead: drones, shuttles landing with colonists, mass driver trade.

## Current task details
**Done (2026-11-26):** `BuildManager` does all the placing (clicks, `Instantiate`, `Destroy`). `GridManager` only answers questions: `TryGetMouseCell`, `GetCellCenter`, `GetFootprintCenter(corner, size)`, `IsCellFree` / `IsAreaFree`, `OccupyCell` / `OccupyArea` (every cell of a footprint points at the same building), `TryGetBuilding`, `FreeCell`. `BuildingData` cards in `Assets/Data` (Capsule House 2×2 amber, Storage Depot 3×3 cyan, Ice Drill 2×3 pink; public fields `buildingName`, `size`, `color`) give each placed cube its name, colour and footprint: the cube is scaled to (size.x, 1, size.y) and the ghost matches (× 1.02).

**Centred placement (done):** in `Update`, `corner = cell - GetFootprintSize() / 2` (integer division on purpose), so the building sits centred under the mouse; the ghost and `PlaceBuilding(corner)` both use that corner. `RemoveBuilding` still gets the real mouse `cell`.

**Footprints round 2 (done):** a small `Building` component on the `Test Building` prefab remembers its `data`, `corner` and `size` (it grows into the real `Building` class later). `PlaceBuilding` fills it in right after `Instantiate`; `RemoveBuilding` asks it for `corner` + `size`, calls `GridManager.FreeArea`, then `Destroy`s it, so right-click anywhere on a building frees all its cells.

**Number keys (done):** `BuildManager` has `[SerializeField] BuildingData[] buildingCards` (Capsule House, Storage Depot, Ice Drill) and a code-only `selectedBuilding`. `SelectBuilding(int index)` guards with `index >= buildingCards.Length`, picks the card and `Debug.Log`s it; `Start` calls `SelectBuilding(0)`; keys 1–3 at the top of `Update` call it. They're a debug shortcut: the phase 5 build menu's buttons will call the same `SelectBuilding`.

**R-rotate (done):** `private int rotationSteps` (0–3, R does `(rotationSteps + 1) % 4`). `GetFootprintSize()` returns the card's size, swapped when `rotationSteps % 2 == 1`; `GetRotation()` returns `Quaternion.Euler(0, rotationSteps * 90, 0)`. Rule: **grid things** (area check, centre, `OccupyArea`, `b.size`) use `GetFootprintSize()`; **the shape** (cube and ghost `localScale`) uses the card's `selectedBuilding.size`, and the rotation turns it. The cube spawns with `GetRotation()`; the ghost `Slerp`s to it, so it turns smoothly.

**Phase 2, the economy (done 2026-11-26):**
- `ResourceData` cards in `Assets/Data`: Rock, Scrap Metal, Water, Food (`resourceName`, `icon`).
- `Stockpile` (on a `Stockpile` object in the scene): `Dictionary<ResourceData, int> amounts`; `GetAmount` (TryGetValue, else 0), `Add` (`amounts[r] = GetAmount(r) + amount`, negative = subtract), `CanAfford(ResourceAmount[])` (same shape as `IsAreaFree`), `Spend`, `AddAll`. Debug: **G** gives 10 of each `debugResources` card; `OnGUI` lists every pair with `GUILayout.Label` (font 24). No change event yet: add it when the phase 5 UI needs one.
- `ResourceAmount` = a `[System.Serializable]` plain class (`public ResourceData resource; public int amount;`).
- `BuildingData` now also has `cost`, `prefab`, `workTime`, `input`, `output` (singular names). Costs: Capsule House 10 Rock + 5 Scrap, Storage Depot 15 Rock, Ice Drill 10 Scrap + 5 Rock, Hydroponics Farm (3×3 green, card 4, key 4) 10 Rock + 5 Scrap. Ice Drill: no input → 1 Water / 3 s. Farm: 2 Water → 1 Food / 5 s.
- `BuildManager` checks `CanAfford` after `IsAreaFree`, spends at the end of `PlaceBuilding`, refunds the **full cost** on remove (`AddAll(b.data.cost)` before `Destroy`: moving buildings never punishes you). It spawns `selectedBuilding.prefab` and hands the building `b.stockpile`.
- `Building` has `data`, `corner`, `size`, `stockpile`. **`ProductionBuilding : Building`** (inheritance; `GetComponent<Building>()` still finds it): a timer; when it reaches `workTime` and the stockpile can afford the input, spend input, `AddAll` output, timer = 0; otherwise it waits (an empty input array = always affordable). Prefabs: `Test Building` (plain `Building`, house + depot) and `Test Production Building` (only `ProductionBuilding`, drill + farm).

**Phase 3 so far (2026-11-26):**
- NavMesh: `NavMeshSurface` on `Testing Ground` (Use Geometry = Physics Colliders, so the collider-less ghost is ignored; baked). Every building prefab has a `NavMeshObstacle` with **Carve**, so walkers go round buildings.
- 3 `Worker` capsules in the scene (scale 0.4, y 0.4, **no collider** so the mouse ray ignores them, `NavMeshAgent`). `Worker` grabs its agent in `Awake`; it has `colony` + `stockpile` slots and a private `workplace`.
- Depots: `StorageDepot : Building` signs up in `Start` (`stockpile.AddDepot(this)`; Start, not Awake, because BuildManager sets `b.stockpile` after `Instantiate`) and signs out in `OnDestroy`. `Stockpile` has `List<StorageDepot> depots` + `GetNearestDepot(from)` (the find-the-smallest pattern; null when there are none). Prefab `Test Depot` (only `StorageDepot`), used by the Storage Depot card. `Building.GetNearestPoint(from)` = `GetComponent<Collider>().ClosestPoint(from)`: workers walk to a building's nearest wall, never its centre (that's inside the carved hole).
- Jobs: `ColonyManager` (an object in the scene) has `List<ProductionBuilding> workplaces`, `AddWorkplace` / `RemoveWorkplace` and `FindFreeWorkplace(from)` (nearest one whose `worker` is null; `continue` skips taken ones). `ProductionBuilding` has `public Worker worker`, signs up in `Start`, signs out in `OnDestroy`. `Building` has `colony`, handed over by BuildManager like `stockpile`. A worker with no `workplace` finds the nearest free one, claims it (`workplace.worker = this`) and walks there; when its building is removed, `workplace` reads as null and it looks again.
- **The work-and-carry loop (done):** `Worker` is a state machine: `enum State { Idle, GoingToWork, Working, GoingToDepot }` + a `switch` in `Update` calling `UpdateIdle` / `UpdateGoingToWork` / `UpdateWorking` / `UpdateGoingToDepot`; `Update` first sets Idle when `workplace == null` (building removed). `HasArrived()` = `!agent.pathPending && agent.remainingDistance <= 0.5f`. Idle: claim the nearest free workplace, walk there. GoingToWork: arrived → wait until the stockpile can afford `data.input` → `Spend` it (inputs come straight from the stockpile for now, no fetch walk) → Working. Working: `workTimer` up to `data.workTime`, nearest depot (none → wait), `carrying = data.output`, walk there. GoingToDepot: depot gone → back to Working (its full timer re-picks a depot at once); arrived → `AddAll(carrying)`, walk back. Each method: "not ready → `return`", then the action, and the last line switches the state.
- `ProductionBuilding` has no timer any more (just `worker`, sign-up, sign-out): **nothing is made unless a worker walks it to a depot.** Drill and farm both run through workers.

- **Houses spawn the workers (done):** `Worker` is a prefab (`Assets/Prefabs/Worker`); no workers sit in the scene any more. Its `colony`, `stockpile` and `home` are public fields. `House : Building` (prefab `Test House`, used by the Capsule House card) has `[SerializeField] Worker workerPrefab` + `colonists = 2`; in `Start` a `for` loop does `Instantiate(workerPrefab, GetDoorPoint(), Quaternion.identity)` (a `Worker`-typed slot makes `Instantiate` return the `Worker` directly) and hands it `colony`, `stockpile` and `home = this`. `GetDoorPoint()` = the wall point nearest a spot 5 in front (`transform.forward`, so R turns the door) + half a step out, so the agent starts outside the carved hole. Removing a house leaves its workers working (homeless) for now.

**Next (first task of the next session): home + rest** (see How the town works; the steps were already given, not started):
- `Worker` fields: `[SerializeField] int tripsBeforeRest = 5`, `[SerializeField] float restTime = 10f`, `int trips`, `float restTimer`, `Renderer body` (grabbed in `Awake`). Enum + switch get `GoingHome` and `Resting`.
- `UpdateGoingToDepot`: after `AddAll`, `trips++`; at `tripsBeforeRest` with a `home` → reset trips, walk home, GoingHome, `return`.
- `UpdateGoingHome`: home gone → back to work; arrived → `body.enabled = false`, restTimer = 0, Resting. `UpdateResting`: after `restTime` → `body.enabled = true`, walk to work, GoingToWork.
- Safety: the `workplace == null` block in `Update` also sets `body.enabled = true` (no invisible job hunters). Never `SetActive(false)`: it would stop `Update` and the worker would never wake.
- Test with Trips Before Rest = 2 on the Worker prefab, then back to 5.
- Then: food (colonists eat; no food = no new colonists), processors walking to fetch inputs, the opening (`Gatherable`), discover-to-unlock.
- When real models come: a model's pivot is its bottom centre, so it sits at (x + w/2, 0, z + d/2). The placeholder cube's pivot is its middle, hence y = 0.5 for now.

**Ghost preview (done):** a `Ghost` cube lives in the scene (transparent URP `GhostMat`, **no collider** so the mouse ray ignores it, scale 1.02 against flicker, no shadows). `BuildManager.Update` asks `gridManager.TryGetMouseCell`; if it hits, the ghost shows and `Lerp`s to `gridManager.GetCellCenter(cell)` (`ghostSmoothing` 15), else `SetActive(false)`. `TryGetMouseCell` and `GetCellCenter` are public on `GridManager`; `PlaceCube` uses `GetCellCenter` too.

**GridManager (done):** `TryGetMouseCell(out Vector2Int cell)` raycasts from the camera through the mouse, nudges the hit point back along `hit.normal` by 0.01 (so clicks on a cube's side land in that cube's cell), and `Mathf.FloorToInt`s it into a cell. `cellSize` = 1 (one Minecraft block). Left click → `PlaceCube` (skips taken cells, spawns at the cell centre, `Add`s to `Dictionary<Vector2Int, GameObject>`); right click → `RemoveCell` (`TryGetValue` → `Destroy` + `Remove`).
- This project uses the **new Input System only** (`activeInputHandler: 1`): use `using UnityEngine.InputSystem;` and `Keyboard.current` / `Mouse.current` (mouse position: `Mouse.current.position.ReadValue()`). The old `Input.GetKey` throws errors here.
- Scroll gives exactly +1 / -1 per notch for one frame (no `Time.deltaTime` for scroll).

**Camera (done):** `CameraController` on an empty `CameraRig` at ground level; Main Camera is its child. Pan (WASD, normalized) and zoom (scroll) both use target + `Lerp` smoothing; Q/E rotate the rig; zoom sets the camera's `localPosition` to (0, zoom, -zoom). Later polish: pan speed scaling with zoom, smoothed rotation, and **street-level tilt**: the closer you zoom, the flatter the camera angle (45° far out → ~15° fully in), so zooming in turns into the reference alley shots.

## Notes and gotchas
- **Lunar Client saves Minecraft structures to `~\.lunarclient\profiles\cf\saves\<world>\generated\minecraft\structure\`**, not `.minecraft`. Copy them over (or teach my converter that folder).
- Minecraft-converted models come in as **one object per block with every face** (furnacev3.fbx = 1,216 cubes, ~14.6k triangles). Later task: merge into one mesh and skip faces touching another block, in my converter.
- **Save/load (week 9):** save the colony name, the buildings, levels and stockpile only. On load, workers just restart their loop; never try to save a worker halfway through a walk.

## Planning docs
- Core Game Plan (current): https://claude.ai/artifact/QFXMZMtB2Z7NbGXHbjszdV
- Kugelblitz Field Manual (the path to the ring, old factory version; the science still holds): https://claude.ai/artifact/DXvF5TVmeGYxhT3n5goWDP
- Machine Map (old factory version, real-science chain still useful): https://claude.ai/artifact/UDLtQwbyotyhK2A6To7TPj

## Progress log
(Update this section at the end of each session: what got done, what's next.)
- **2026-11-24:** Planned the game and pivoted from factory tycoon to cozy space colony builder. Modelled the furnace (v3, final) in Minecraft, painted and exported it to FBX. No code yet. Then: made the Unity project `SpaceColonyGame` (6000.5.6f1, URP), Git + GitHub repo (public: https://github.com/LKarLzs0n/SpaceColonyGame), and wrote **CameraController**: WASD pan (normalized, smoothed), Q/E rotate, scroll zoom (smoothed, clamped). Learned: camera rig, `position` vs `localPosition`, `Time.deltaTime`, `normalized`, `Mathf.Clamp`, `Lerp` for ease-out. Posted devlog #1 on Hack Club (Build Mode pipeline screenshot + camera clip). Later the same day, the design became **a living town**: little people walk, carry and work (NavMesh + a worker state machine), every building feeds the next through storage depots, and the ring is a 3-part Wonder that unlocks new buildings. Classes and research were dropped; rain under the dome was added. The grid became fine and invisible (1 cell = 1 block). Then wrote **GridManager**: click to place cubes snapped to the grid, right-click to remove, side clicks fixed. Learned: raycasts, `out` parameters and the `TryGet` pattern, early `return`, `Dictionary` (`Add`, `ContainsKey`, `TryGetValue`, `Remove`), `Mathf.FloorToInt` vs `(int)` on negatives, `hit.normal`, and the if-without-braces trap. Then started **BuildManager**: a see-through ghost cube glides to the cell under the mouse. Learned: one script calling another (`public` methods + a `[SerializeField]` script slot), returning a value from a method, `SetActive`, a transparent URP material, and why visuals shouldn't have colliders. **Next:** move placing into `BuildManager` (see Current task details), then `BuildingData` + the furnace on the grid.
- **2026-11-26:** Wrote devlog #2 and set up Lapse for art time. Locked the big picture: the heartbeat loop, one playthrough from the pod to the ring's first flash, the opening (2 colonists gather crates and boulders), discover-to-unlock, and the mechanics-first roadmap (6 phases, placeholder cubes until phase 5). Added the pod's starting power and free decor. Then coded: moved placing into `BuildManager` (`GridManager` only answers questions now), the first ScriptableObject `BuildingData` (3 cards with name, size, colour), and footprints round 1 (big cubes fill every cell they cover, the ghost matches). Learned: refactoring, `!` to flip a bool, passing an `out` straight through, variable scope (`cell` only exists after the line that makes it), arrays count from 0, ScriptableObjects + `[CreateAssetMenu]`, `GetComponent<Renderer>()`, colour alpha, default field values, nested `for` loops, integer division (always `/ 2f`), and Vector3 slot order (x, height, z). Then footprints round 2: each building remembers its own footprint in a `Building` component, and right-click removes the whole building. Learned: a component as an object's memory, `GetComponent` on your own script, and asking an object for its data before you `Destroy` it. Then number keys 1–3 pick a card from an array through `SelectBuilding(int index)`. Learned: `Keyboard.current.digit1Key`, `Start`, `.Length`, the off-by-one trap (`>=`, not `>`), and `Debug.Log` with `+`. Then R-rotate: 4 quarter turns, the grid uses the swapped footprint, the shape keeps the card's size and the rotation turns it, and the ghost turns smoothly. Learned: `%` (remainder, wrap-around and odd/even), `Quaternion`, `Quaternion.Euler`, `Quaternion.Slerp`. Then centred placement (the building sits under the mouse; integer division on purpose), which finished phase 1 a week early. Also decided: **it's YOUR city** (no placement rules beyond overlap and cost; the best spots only show through walking distance), and you **name your colony** when the pod lands. Then **all of phase 2 in the same session**: `ResourceData` cards, the `Stockpile` with debug text and a G give-key, building costs with full refunds, and `ProductionBuilding` on a timer; the ice drill makes water and the first chain runs (drills → water → hydroponics farm → food, the farm waits when water runs low). Learned: the dictionary indexer (`amounts[key] = ...`, safe to write, crashes to read a missing key), `TryGetValue` inside an `if` (an `int` can never be `null`), `foreach` over arrays and over a dictionary (`KeyValuePair`, `.Key`, `.Value`), `OnGUI` + `GUILayout.Label`, `[System.Serializable]` plain classes, `void` vs `bool` returns, fields are private unless `public`, check-everything-then-spend, negative numbers through `Add`, a card picking its own prefab, handing scene objects to spawned prefabs, and **inheritance** (`ProductionBuilding : Building`). Also: Rider's auto-import added `using Mono.Cecil;` for a wrong type (editor-only, breaks builds): watch for strange `using` lines. Then **phase 3 started**: NavMesh (a baked surface, buildings carving holes), a capsule worker that walks round buildings, depots that sign up in a list so the worker finds the nearest one, and jobs: a `ColonyManager` workplace list, and the closest jobless worker walks to each new drill or farm by itself. Learned: `NavMeshSurface` / `NavMeshObstacle` (Carve) / `NavMeshAgent` + `SetDestination`, `Awake` vs `Start` timing, `OnDestroy`, `this`, `&&` (stops early), `List` (`Add`, `Remove`), the find-the-nearest pattern (`Mathf.Infinity`, `Vector3.Distance`), `continue`, null checks, `Collider.ClosestPoint`, `[SerializeField]` vs `GetComponent` (pick one), and that a destroyed object reads as `null`. Then **the work-and-carry loop**: the worker became a state machine (Idle → walk to work → take inputs → work → carry to the nearest depot → walk back), the old production timer is gone, and water and food only arrive when someone carries them in; a depot closer to the drill makes the loop visibly faster (the layout puzzle works). Learned: `enum`, `switch` / `case` / `break`, one small method per state, the "not ready → return, act, switch state" shape, and `=` (set) vs `==` (compare): `if (workplace = null)` compiled (Unity objects work as true/false) but erased the workplace every frame. Also locked: **home and rest**: a colonist's house is its home; after a few deliveries it walks home, goes inside and rests, each on its own clock (no day/night, the town never freezes), and in phase 5 the windows light up while someone's home. Then **houses spawn the workers**: the worker became a prefab, and a Capsule House sends 2 colonists out of its front door (it turns with R), handing them the colony, the stockpile and their home. Learned: a prefab slot typed as a script (`Instantiate` returns that script), `Quaternion.identity`, `transform.forward`, and filling in a fresh object's fields between its `Awake` and its first `Update`. Wrote devlog #3 (the economy + the living town). **Next:** home + rest (see Current task details).
