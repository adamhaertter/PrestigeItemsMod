# Prestige Items Mod
<img src="https://github.com/yurijserrano/Github-Profile-Readme-Logos/blob/master/programming%20languages/c%23.svg?raw=true" height="50">
<img src="https://github.com/yurijserrano/Github-Profile-Readme-Logos/blob/master/tools/unity.png?raw=true" height=50>

## Description

This mod is currently a work in progress, but aims to add around 15 items to the game in total. You see the progress of the mod below. This is our first mod for Risk of Rain 2, so we are still learning! Some things may break or have issues, and that's why it's still in beta!

## Co-Authors
- Adam Haertter
- Brian Grammer

## Issues and Bugs

Find an issue that's not listed below? Please check out the Issues tab and create a GitHub Issue there so we can be made aware of it! Please check that the issue doesn't already exist there, though.

### Known Bugs
- Some of the items do not highlight properly in game. We are currently tracking this under [#16](https://github.com/adamhaertter/PrestigeItemsMod/issues/16)


## Items

#### Implemented
| Icon | Name | Description |
| --- | --- | --- |
| ![DevCube Icon](https://i.imgur.com/cVVPURL.png) | Dev Cube | Killing an enemy grants the holder a random status buff for 4 (+1 per stack) seconds. |

#### Planned
Mechanics subject to change throughout development. Each item is tracked in it's own GitHub Issue and branch.
| Icon | Name | Description |
| --- | --- | --- |
| ![PrestigeBleed_Alt](https://github.com/adamhaertter/PrestigeItemsMod/assets/80988984/9f6547fa-7a62-42a3-b81f-503685d3dc69) | Refined Shard | 10% (+5% per stack) chance to Cripple the enemy on hit. |
| ![PrestigeFungus_Alt](https://github.com/adamhaertter/PrestigeItemsMod/assets/80988984/78c0f7a3-5e6c-4855-8f50-cdbd3091a5e6) | Charting Fungus | Gain gold while sprinting. |
| ![PrestigeFlower_Alt](https://github.com/adamhaertter/PrestigeItemsMod/assets/80988984/dd38806d-0b00-4676-8789-a249befe084d) | Solar Flower | Apply Overheat to enemies at full health or those that already have it. | 
| ![PrestigeBear_Alt](https://github.com/adamhaertter/PrestigeItemsMod/assets/80988984/6ff04ab5-73e0-4b6b-b402-2668d155bdeb) | Higher Hopes | When dropping below full health, gain a Power Buff for a short time. Goes on cooldown. |
| ![PrestigeBauble_Alt](https://github.com/adamhaertter/PrestigeItemsMod/assets/80988984/f7874d09-f6d5-4cf8-aa2e-1666448da127) | Promobauble | Chance to promote an enemy's slowness debuffs to the next tier on hit. |
| ![PrestigeMusic_Alt](https://github.com/adamhaertter/PrestigeItemsMod/assets/80988984/e68d5841-863b-44c8-ba88-e6938372c936) | Ornate Banjo | Chance to inflict a burst of damage dealing Permanent Curse to the enemy. |
| ![PrestigeRevive_Alt](https://github.com/adamhaertter/PrestigeItemsMod/assets/80988984/1d171639-fc02-4b0b-8bd1-c179c24a6ef4) | All That Is Holy | Revives you, but sequences your items like a shrine of order. |
| ![PrestigeKey_Alt](https://github.com/adamhaertter/PrestigeItemsMod/assets/80988984/882f520c-764c-4b5e-9922-045fafbc988b) | Ethereal Key | Gives access to an interactible that gives access to a random portal on the next stage. |
| ![PrestigeSymbiote_Alt](https://github.com/adamhaertter/PrestigeItemsMod/assets/80988984/981e1135-9d36-4945-8ea4-0e92a8a6cc02) | Parasitic Symbiosis | Half your health is shield, but buffs all your stats while you have shield up. |
| ![PrestigeCell_Alt](https://github.com/adamhaertter/PrestigeItemsMod/assets/80988984/e6a04390-9d93-4121-ae52-9cd66e255e78) | Rogue Cell | Disables one skill per stage, but gives an extra charge to all other skills. |
| ![PrestigeGlasses_Alt](https://github.com/adamhaertter/PrestigeItemsMod/assets/80988984/6cfa2c21-5b64-4cb7-8f82-ed95d51be37f) | Deferred Optics | Chance to instantly remove an enemy from the field, saving them for later. |
| ![PrestigeBand_Alt](https://github.com/adamhaertter/PrestigeItemsMod/assets/80988984/fcabe679-5697-468c-9b69-2f46dc2f0133) | Multiplicity Band | Chance to trigger any band effect or any combination of band effects. Corrupts all bands. |
| ![PrestigeConvergence_Alt](https://github.com/adamhaertter/PrestigeItemsMod/assets/80988984/041b8d86-0c29-4d9e-9544-bebb0b547c3c) | Invasive Species | During the teleporter event, spawns enemies outside their native environment. Corrupts Focused Convergence. |
| ![PrestigeBalancer_Alt](https://github.com/adamhaertter/PrestigeItemsMod/assets/80988984/f3a34db2-2452-443b-950e-9faf48d48202) | The Great Equalizer | Items spawn indiscriminately of tier. Corrupts Eulogy Zero. |

## Installation
You can download the latest uploaded release from the ThunderStore on our [page](https://thunderstore.io/package/BlueBubbee/Prestige_Items_Beta/) or by using the mod manager. **This is the recommended installation.** We recommend using r2ModMan as your launcher, but the Thunderstore app probably works too.

If you prefer to manually install, ensure you have BepInEx and R2API already installed. You can download the `PrestigeItems.dll` and `prestigemodassets` files from the GitHub Releases page and place them in a folder under your `[Mod Profile]/BepInEx/plugins/` folder. These files need to be at the same folder level as each other to be able to load the sprites and models. 

## Credits
Thanks to these sources for being excellent references & baselines for learning how this all works:
- [Modding Wiki Tutorials & Boilerplate Demo](https://risk-of-thunder.github.io/R2Wiki/Mod-Creation/Getting-Started/First-Mod/)
- [Henry Tutorial](https://github.com/ArcPh1r3/HenryTutorial) 
- [KomradeSpectre's Aetherium Item Tutorial](https://www.youtube.com/watch?v=8TsF8elv_m0)
- Everyone's help in the Modding Discord as well :)
