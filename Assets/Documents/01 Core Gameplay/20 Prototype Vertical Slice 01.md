# 20 Prototype Vertical Slice 01

Version: 1.0  
Status: Ready for Implementation  
Milestone Type: First Playable Vertical Slice  
Engine: Unity  
UI System: Canvas / uGUI  
Text System: TextMeshPro  
Primary Platform: PC Landscape  
Reference Resolution: 1920 × 1080

---

# Purpose

This document defines the first playable vertical slice of the game.

The purpose of this milestone is not to build the entire game.

The purpose is to prove that the core architecture, gameplay loop, persistent UI, progression, saving, Woodcutting, Combat, Inventory, Equipment, and reward systems work together correctly.

The completed prototype should allow the player to:

- Launch the game.
- Navigate between working screens.
- Select Woodcutting.
- Select one of three trees.
- Damage tree durability automatically.
- Receive logs and Woodcutting XP.
- Continue Woodcutting while browsing other screens.
- Select a combat Region.
- Select a combat activity type.
- Select a location.
- Select one of three monsters.
- Start automated combat.
- Deal and receive damage.
- Gain Warrior Combat Discipline XP.
- Receive Temporary Combat Loot.
- Claim loot into Inventory.
- Equip a dropped item.
- Save the game.
- Close and reopen the game.
- Continue with retained progress.
- Receive basic offline Woodcutting progress.

This milestone should create a reliable foundation that future systems can expand.

---

# Source Documents

Codex should use the following existing documents as supporting sources:

- 00 Master Vision
- 01 Core Gameplay
- 02 Experience System
- 03 Profession Framework
- 04 Item Framework
- 05 Equipment Framework
- 06 Combat Framework
- 07 Companion Framework
- 08 Inventory Framework
- 09 Save System Framework
- 10 Offline Progress Framework
- 12 Profession Activity Framework
- 16 UI and UX Framework
- 17 Unity UI Technical Architecture
- 18 UI Visual Style Guide
- 19 Balancing Framework

The relevant UI reference images should also be used.

---

# Source Priority

When two sources appear to conflict, use this priority:

1. This Prototype Vertical Slice document for prototype scope and prototype content.
2. Approved gameplay and system framework documents.
3. UI and UX Framework.
4. Unity UI Technical Architecture.
5. UI Visual Style Guide.
6. UI reference images.
7. Existing temporary implementation where it does not conflict with approved rules.

Reference images define intended appearance and information hierarchy.

Reference images do not replace dynamic Unity UI.

The implemented interface must remain:

- Data-driven
- Scrollable
- Responsive
- Built from reusable prefabs
- Connected to gameplay systems
- Editable inside Unity

---

# Prototype Philosophy

This milestone should prioritize:

Functionality

Reliable architecture

Clear data flow

Correct save behavior

Reusable systems

Playable progression

Simple but understandable UI

The milestone should not prioritize:

Final artwork

Perfect balancing

Advanced visual effects

Every Profession

Every Combat Discipline

Every equipment item

Every Region

Every future system

The first slice should be small enough to complete and test thoroughly.

---

# Prototype Definition of Done

The prototype is considered complete when the player can perform this full loop:

1. Launch the game.
2. Load or create a save.
3. Open Woodcutting.
4. Select Sproutwood Tree.
5. Start Woodcutting.
6. Watch tree durability decrease.
7. Receive logs at reward thresholds.
8. Receive Woodcutting XP when the tree is completed.
9. Continue Woodcutting while viewing Inventory.
10. Stop Woodcutting.
11. Open Combat.
12. Select Greenvale.
13. Select Areas.
14. Select Greenvale Forest.
15. Select one of three monsters.
16. Start Combat.
17. Watch player and enemy attacks.
18. Use the starter Combat Ability automatically or manually.
19. Defeat an enemy.
20. Gain Warrior XP from valid player damage.
21. Receive Gold and item drops.
22. See drops enter Temporary Combat Loot.
23. Pick one item.
24. Use Pick All.
25. Open Inventory.
26. Select an item.
27. Open Equipment.
28. Equip the Worn Buckler first-clear reward.
29. Save the game.
30. Close the application.
31. Reopen the application.
32. Confirm that progress, Inventory, Equipment, XP, Gold, and unlocks remain.
33. Leave Woodcutting active before closing.
34. Reopen after time has passed.
35. Receive an Offline Progress Summary.

---

# Prototype Scope

The vertical slice includes:

- One gameplay scene
- Persistent UI shell
- Top Bar
- Left Navigation
- Active Activity Bar
- Screen Manager
- Tooltip support
- Popup support
- Notification support
- Inventory
- Equipment
- Item Database
- Woodcutting
- Three trees
- Profession XP
- One Combat Discipline
- One Region
- One combat activity type
- One combat location
- Three monsters
- One starter Combat Ability
- Enemy loot
- Temporary Combat Loot
- Basic Gold
- Basic Healing Potion support
- Saving
- Loading
- Autosaving
- Basic offline Woodcutting
- Placeholder art
- Basic audio hooks if convenient

---

# Explicitly Out of Scope

Do not delay this milestone to implement:

- Mining
- Fishing
- Cooking
- Foraging
- Smithing
- Carpentry
- Herblore
- Farming
- Thieving
- Hunting
- Archaeology
- Tailoring
- Leatherworking
- Jewelcrafting
- Enchanting
- Full Runecrafting
- Ranger Combat Discipline
- Mage Combat Discipline
- Companion gameplay
- Companion rank-ups
- Dungeons
- Bosses
- Tower
- Elite Areas
- Multiple Regions
- Full Shop system
- Full Crafting system
- Achievement rewards
- Collection milestone rewards
- Full Statistics system
- Full tutorial
- Controller support
- Mobile layout
- Cloud saves
- Player trading
- Auction House
- Auto-Sell
- Buyback
- Final Rune consumption
- Final equipment balance
- Final animations
- Final icons
- Final sound effects
- Final music
- Final localization

Placeholder navigation entries for future systems may exist, but they must not require working implementations.

---

# Core Activity Rule

The prototype supports one primary active activity at a time.

Primary activities are:

Woodcutting

Combat

Starting Combat while Woodcutting is active should request confirmation.

Example:

Starting Combat will stop Woodcutting.

Continue?

Starting Woodcutting while Combat is active should require the player to quit Combat first.

The Active Activity Bar should display the current primary activity.

Future independent timers such as Farming and Companion rank-ups are outside this milestone.

---

# Prototype Runtime Scene

Use one primary gameplay scene.

Recommended scene hierarchy:

[BOOTSTRAP] GameBootstrap

[SYSTEMS] GameSystems

[UI] UI System

[EVENTSYSTEM] EventSystem

[AUDIO] Audio System

The scene must contain one active EventSystem.

Use:

Unity Input System

InputSystemUIInputModule

Do not create multiple EventSystems.

---

# Persistent UI Shell

The persistent UI shell must contain:

Top Bar

Left Navigation

Active Activity Bar

Screen Container

Dropdown Layer

Tooltip Layer

Notification Layer

Popup Layer

Modal Layer

Loading Layer

The persistent UI should not be rebuilt every time the player changes screens.

---

# Top Bar

The Top Bar displays account-wide information.

Prototype Top Bar contents:

Gold

Inventory capacity

Save status

Notification shortcut

Settings shortcut if Settings exists

Example:

Gold: 50

Inventory: 4 / 100

Saved

The Top Bar must not display the current activity.

Current activity information belongs in the Active Activity Bar.

---

# Active Activity Bar

The Active Activity Bar displays the current primary activity.

## No Activity Mode

Display:

No Active Activity

Choose Woodcutting or Combat to begin.

## Woodcutting Mode

Display:

Woodcutting icon

Woodcutting

Current tree name

Durability progress

Action progress

Stop button

Open Woodcutting button

Example:

Woodcutting — Sproutwood Tree

Durability: 15 / 25

## Combat Mode

Display:

Combat icon

Warrior

Current enemy name

Player Health

Player Devotion

Enemy Health

Quit Combat button

Open Combat button

Example:

Combat — Grey Wolf

Health: 76 / 100

Devotion: 42 / 100

The Active Activity Bar should remain compact.

It does not replace the full activity screen.

---

# Left Navigation

Prototype Left Navigation should contain:

Main

- Combat
- Inventory
- Equipment

Professions

- Woodcutting

Account

- Settings if implemented

Future systems may appear as disabled or locked placeholders.

The Navigation should:

- Be vertically scrollable.
- Highlight the selected screen.
- Display notification badges where appropriate.
- Preserve collapsed or expanded state.
- Use stable Screen IDs.
- Open screens through the Screen Manager.

Do not manually connect each button directly to unrelated GameObjects.

---

# Required Main Screens

The prototype requires:

- Woodcutting Screen
- Combat Screen
- Inventory Screen
- Equipment Screen

Optional:

- Dashboard Screen
- Settings Screen
- Debug Screen

Only one main screen should be active at a time.

---

# Screen IDs

Use stable IDs.

Required IDs:

woodcutting

combat

inventory

equipment

Optional IDs:

dashboard

settings

debug

Do not use visible display names as the only identifier.

---

# Item System Scope

The prototype requires an Item Database.

Every item should define:

Item ID

Display Name

Description

Icon

Category

Stackable

Maximum Stack

Sell Value

Can Be Sold

Can Be Destroyed

Can Be Locked

Can Be Favorited

Equipment Data if applicable

Collection Data if applicable

Future Use Notes

Prototype item definitions should be data assets.

Do not hardcode item names or values inside Woodcutting or Combat scripts.

---

# Currency

The prototype uses one main currency:

Gold

Gold:

- Does not use Inventory slots.
- Saves as account data.
- Displays in the Top Bar.
- May be gained directly from Combat.
- Is tracked separately from item stacks.

Starting Gold:

50

---

# Inventory Rules

Starting Inventory capacity:

100 slots

Prototype Inventory rules:

- Stackable items use one slot per Item ID.
- Non-stackable equipment uses one slot per individual item.
- Gold does not use a slot.
- Equipped items do not count as available Inventory stacks.
- Reserved items cannot be sold or destroyed.
- Locked items cannot be destroyed.
- Inventory changes should save.
- Inventory should support large future quantities.
- Stack quantities should use a safe integer type.

Prototype maximum stack:

999,999

Inventory should display:

Capacity

Search

Basic category filters

Sorting

Item grid

Selected item details

Available actions

---

# Starting Player State

On a new save, the player starts with:

Gold:

50

Inventory capacity:

100

Woodcutting Level:

1

Woodcutting XP:

0

Warrior Level:

1

Warrior XP:

0

Maximum Health:

100

Current Health:

100

Maximum Devotion:

100

Current Devotion:

100

Base Accuracy:

60

Base Defense:

5

Base Critical Chance:

5%

Base Critical Damage:

150%

Starting weapon:

Worn Sword

Starting shield:

None

Starting Profession tool:

Bare Hands

Starting Healing Potions:

5 Minor Healing Potions

The Worn Sword should begin equipped.

Bare Hands should appear in the Tool slot as the default Woodcutting tool.

Bare Hands should not be sellable, destroyable, or removable.

---

# Starting Equipment

## Worn Sword

Item ID:

weapon_worn_sword

Item Type:

Weapon

Combat Discipline:

Warrior

Handedness:

One-Handed

Damage Range:

6–10

Attack Interval:

2.0 seconds

Accuracy Bonus:

10

Sell Value:

10 Gold

Starting State:

Equipped

Description:

A damaged but usable sword. Better than fighting unarmed.

---

# Starting Consumable

## Minor Healing Potion

Item ID:

consumable_minor_healing_potion

Item Type:

Healing Potion

Stackable:

Yes

Starting Quantity:

5

Healing Amount:

30 Health

Cooldown:

10 seconds

Default Auto-Use Threshold:

35% Health

Sell Value:

5 Gold

Description:

Restores a small amount of Health during Combat.

The Healing Potion should never heal above Maximum Health.

---

# Prototype Experience Data

The full game caps remain:

Professions:

Level 100

Combat Disciplines:

Level 150

The prototype only needs early-level XP data populated and tested.

Use centralized XP tables.

Do not calculate different XP curves inside individual screens.

Prototype total XP thresholds:

| Level | Total XP Required |
|---:|---:|
| 1 | 0 |
| 2 | 50 |
| 3 | 125 |
| 4 | 225 |
| 5 | 350 |
| 6 | 500 |
| 7 | 700 |
| 8 | 950 |
| 9 | 1,250 |
| 10 | 1,600 |
| 11 | 2,000 |
| 12 | 2,500 |
| 13 | 3,100 |
| 14 | 3,800 |
| 15 | 4,600 |

This table may be used for Woodcutting and Warrior during the prototype.

The architecture must still support separate Profession and Combat Discipline XP tables later.

Do not permanently reduce the game maximum levels to 15.

Higher-level tables will be expanded during future balancing.

---

# Level-Up Rules

When XP reaches the next threshold:

- Increase level.
- Preserve excess XP.
- Display a level-up notification.
- Refresh locked activity and monster states.
- Refresh the screen header.
- Save progress.

If enough XP is gained for multiple levels:

- Apply every level gained.
- Show one grouped notification if appropriate.
- Do not discard excess XP.

---

# Woodcutting Prototype

Woodcutting is the first complete Profession.

Woodcutting should test:

- Profession screen structure
- Activity cards
- Locked activities
- Target durability
- Profession damage
- Reward thresholds
- Completion XP
- Respawn
- Automatic repetition
- Inventory rewards
- Active Activity Bar
- Screen switching
- Saving
- Offline progress

---

# Woodcutting Tool

## Bare Hands

Tool ID:

tool_bare_hands

Display Name:

Bare Hands

Woodcutting Power:

5

Action Interval:

1.5 seconds

Item State:

Permanent default tool

Can Be Removed:

No

Can Be Sold:

No

Can Be Destroyed:

No

Description:

The player begins by breaking weak branches and bark by hand.

Bare Hands should appear in the Tool slot.

Future axes will replace it.

---

# Woodcutting Damage

Prototype Woodcutting damage:

Profession Damage = Tool Woodcutting Power

Bare Hands damage:

5 durability damage per action

Action interval:

1.5 seconds

Woodcutting damage should be separate from Combat damage.

Do not use Warrior damage formulas for Woodcutting.

---

# Woodcutting Reward Thresholds

Prototype trees use these thresholds:

75%

50%

25%

0%

When durability crosses a threshold:

- Grant the configured threshold reward.
- Mark the threshold as completed.
- Show reward feedback.
- Prevent the threshold from rewarding more than once per tree cycle.

Woodcutting XP is granted only at 0% durability.

Rare rewards are rolled only at full completion.

After completion:

- Grant completion XP.
- Roll completion rare rewards.
- Enter respawn state.
- Restore full durability after respawn.
- Continue automatically if Woodcutting remains active.

---

# Tree 1: Sproutwood Tree

Tree ID:

tree_sproutwood

Display Name:

Sproutwood Tree

Required Woodcutting Level:

1

Maximum Durability:

25

Respawn Time:

3 seconds

Woodcutting XP on Completion:

10

Tool Requirement:

Bare Hands or better

Region:

Greenvale

Primary Item:

Sproutwood Log

Threshold rewards:

| Threshold | Reward |
|---:|---|
| 75% | 1 Sproutwood Log |
| 50% | 1 Sproutwood Log |
| 25% | 1 Sproutwood Log |
| 0% | 1 Sproutwood Log |

Total guaranteed reward per completed tree:

4 Sproutwood Logs

Rare completion reward:

Sproutwood Sap

Rare chance:

5%

Rare quantity:

1

Description:

A young and fragile tree commonly found near Greenvale.

---

# Tree 2: Mosswood Tree

Tree ID:

tree_mosswood

Display Name:

Mosswood Tree

Required Woodcutting Level:

5

Maximum Durability:

45

Respawn Time:

4 seconds

Woodcutting XP on Completion:

18

Tool Requirement:

Bare Hands or better

Region:

Greenvale

Primary Item:

Mosswood Log

Threshold rewards:

| Threshold | Reward |
|---:|---|
| 75% | 1 Mosswood Log |
| 50% | 1 Mosswood Log |
| 25% | 1 Mosswood Log |
| 0% | 2 Mosswood Logs |

Total guaranteed reward per completed tree:

5 Mosswood Logs

Rare completion reward:

Moss Resin

Rare chance:

6%

Rare quantity:

1

Description:

A damp forest tree covered with thick moss and resin.

---

# Tree 3: Ironbark Tree

Tree ID:

tree_ironbark

Display Name:

Ironbark Tree

Required Woodcutting Level:

10

Maximum Durability:

75

Respawn Time:

5 seconds

Woodcutting XP on Completion:

30

Tool Requirement:

Bare Hands or better for prototype testing

Region:

Greenvale

Primary Item:

Ironbark Log

Threshold rewards:

| Threshold | Reward |
|---:|---|
| 75% | 1 Ironbark Log |
| 50% | 1 Ironbark Log |
| 25% | 1 Ironbark Log |
| 0% | 2 Ironbark Logs |

Total guaranteed reward per completed tree:

5 Ironbark Logs

Rare completion reward:

Ironbark Knot

Rare chance:

8%

Rare quantity:

1

Description:

A dense tree with unusually hard bark.

Future balance may require an upgraded axe.

The prototype allows Bare Hands so all three targets can be tested without implementing tool crafting.

---

# Woodcutting Items

## Sproutwood Log

Item ID:

item_log_sproutwood

Category:

Resource

Stackable:

Yes

Sell Value:

1 Gold

Primary Future Uses:

Carpentry

Cooking fuel if used

Equipment components

---

## Sproutwood Sap

Item ID:

item_sproutwood_sap

Category:

Crafting Material

Stackable:

Yes

Sell Value:

8 Gold

Primary Future Uses:

Herblore

Enchanting

Companion progression

---

## Mosswood Log

Item ID:

item_log_mosswood

Category:

Resource

Stackable:

Yes

Sell Value:

2 Gold

Primary Future Uses:

Carpentry

Tool upgrades

---

## Moss Resin

Item ID:

item_moss_resin

Category:

Crafting Material

Stackable:

Yes

Sell Value:

12 Gold

Primary Future Uses:

Herblore

Enchanting

Equipment upgrades

---

## Ironbark Log

Item ID:

item_log_ironbark

Category:

Resource

Stackable:

Yes

Sell Value:

4 Gold

Primary Future Uses:

Carpentry

High-durability equipment components

Tool upgrades

---

## Ironbark Knot

Item ID:

item_ironbark_knot

Category:

Crafting Material

Stackable:

Yes

Sell Value:

20 Gold

Primary Future Uses:

Rare equipment

Companion rank-ups

High-tier Carpentry

---

# Woodcutting Screen

The Woodcutting Screen should use the relevant UI reference image.

Required elements:

- Woodcutting Screen Header
- Woodcutting icon
- Current level
- Current XP
- XP progress bar
- XP per hour estimate
- Time to next level estimate
- Next unlock
- Active target panel
- Tree activity list
- Selected tree details
- Tool slot
- Companion slot placeholder if shown
- Food slot placeholder if shown
- Relic slot placeholder if shown
- Start button
- Stop button
- Target durability bar
- Action progress bar
- Reward thresholds
- Reward preview
- Respawn state
- Locked requirements

The main Woodcutting screen should support vertical scrolling.

The Profession Header may remain fixed.

---

# Woodcutting Activity Cards

Each Tree Card should display:

Tree icon

Tree name

Required level

Locked or unlocked state

Maximum durability

Current expected completion time

Completion XP

Primary reward

Expected rewards per hour

Rare reward preview

Selected state

Active state

Start button if used

Locked cards must display the actual requirement.

Example:

Requires Woodcutting Level 5

Do not display only:

Locked

---

# Starting Woodcutting

When Start is pressed:

1. Validate that no incompatible primary activity is active.
2. Validate the tree unlock requirement.
3. Validate the required tool.
4. Validate Inventory capacity.
5. Set the tree as active.
6. Reset the tree cycle if necessary.
7. Begin the Woodcutting action timer.
8. Update the Active Activity Bar.
9. Save the active state.

Woodcutting should continue when the player opens another screen.

---

# Stopping Woodcutting

When Stop is pressed:

- Stop future Woodcutting actions.
- Preserve already-earned rewards.
- Do not grant unearned thresholds.
- Preserve current tree durability in memory during the current session if desired.
- The saved active activity becomes None.
- Update the Active Activity Bar.
- Autosave.

Prototype default:

Stopping manually resets the current tree cycle when Woodcutting is restarted.

This prevents partial-target reset exploits.

Future versions may preserve target state.

---

# Woodcutting Inventory Full Behavior

Before granting a reward:

- Check whether the item can enter an existing stack.
- Otherwise check for an available Inventory slot.

If a reward cannot be stored:

- Stop Woodcutting safely.
- Keep already-earned rewards.
- Do not delete the new reward silently.
- Place an important reward in protected claim storage if supported.
- Display an Inventory Full notification.
- Highlight Inventory Navigation.

Example:

Inventory Full

Woodcutting has stopped.

Free at least 1 Inventory slot to continue.

---

# Basic Offline Woodcutting

Offline Woodcutting is included.

Offline Combat is not included in this milestone.

Offline Woodcutting should work only when:

- Woodcutting was active when the game closed.
- The selected tree remains valid.
- The player still has the required tool.
- Save data is valid.

Offline cap:

12 hours

Offline simulation should account for:

- Tree durability
- Woodcutting damage
- Action interval
- Reward thresholds
- Completion XP
- Respawn time
- Rare completion rolls
- Inventory capacity
- Time away

If Inventory becomes full during simulation:

- Stop simulation at the point storage becomes impossible.
- Preserve all rewards that fit.
- Use protected claim storage for important rewards if available.
- Explain the result in the Offline Summary.

Do not simulate more time than the configured offline cap.

---

# Offline Progress Summary

The Offline Progress Summary should display:

Time away

Activity:

Woodcutting

Selected tree

Trees completed

Woodcutting XP gained

Levels gained

Items gained

Rare items gained

Gold gained if any

Inventory overflow

Reason activity stopped if applicable

Actions:

Claim All if protected rewards exist

Open Inventory

Open Woodcutting

Continue Activity

The player should not receive many separate reward popups.

---

# Combat Prototype

Combat should prove:

- Hierarchical combat selection
- Player auto-attacks
- Enemy auto-attacks
- Health
- Devotion
- Accuracy
- Defense
- Critical Hits
- One Combat Ability
- Healing Potion
- Warrior XP
- Enemy defeat
- Player defeat
- Enemy respawn
- Auto Repeat
- Loot generation
- Temporary Combat Loot
- Equipment rewards
- Save state

---

# Prototype Combat Discipline

Only Warrior is playable in this milestone.

Warrior maximum level remains:

150

Only early levels are expected during prototype testing.

Ranger and Mage should not be implemented yet.

They may appear as locked or future content.

---

# Player Combat Stats

Starting total stats with Worn Sword equipped:

Maximum Health:

100

Maximum Devotion:

100

Base Accuracy:

60

Worn Sword Accuracy:

+10

Total Starting Accuracy:

70

Base Defense:

5

Weapon Damage:

6–10

Attack Interval:

2.0 seconds

Critical Chance:

5%

Critical Damage:

150%

---

# Prototype Hit Chance Formula

Use a simple centralized formula.

Hit Chance:

Attacker Accuracy ÷ (Attacker Accuracy + Defender Defense)

Convert the result to a percentage.

Clamp final Hit Chance between:

10%

95%

Example:

Attacker Accuracy:

70

Defender Defense:

10

Hit Chance:

70 ÷ 80 = 87.5%

The formula should exist in the Combat System.

The UI should display the resulting chance when appropriate.

Do not calculate it separately inside every UI card.

---

# Prototype Damage Mitigation Formula

Roll damage inside the attacker Damage Range.

Apply Defense:

Final Damage = Rolled Damage × 100 ÷ (100 + Target Defense)

Round final damage to a whole number.

Minimum successful hit damage:

1

Critical Hits multiply the mitigated damage.

Critical multiplier:

150% by default

Damage beyond the target's remaining Health is overkill.

Only valid effective damage grants Warrior XP.

---

# Warrior XP

Warrior XP is granted from valid effective player damage.

Companion damage does not exist in this milestone.

Prototype rule:

Warrior XP = Valid Effective Player Damage × Enemy XP Coefficient

Examples:

Player deals 8 valid damage.

Enemy coefficient:

1.0

Warrior XP:

8

Player deals 8 valid damage.

Enemy coefficient:

1.4

Warrior XP:

11.2 before rounding

Round XP according to one centralized rule.

Recommended:

Round final encounter XP down only after accumulating fractional XP during the encounter.

Do not grant XP for:

- Misses
- Invulnerable targets
- Overkill damage
- Invalid combat states
- Damage after enemy defeat

---

# Devotion

Starting Devotion:

100

Maximum Devotion:

100

Devotion regeneration during Combat:

3 per second

Devotion should not exceed Maximum Devotion.

Devotion is used by the starter Combat Ability.

Devotion may remain full outside Combat.

---

# Starter Combat Ability

## Heavy Strike

Ability ID:

ability_warrior_heavy_strike

Display Name:

Heavy Strike

Combat Discipline:

Warrior

Required Level:

1

Damage:

150% of normal weapon damage

Cooldown:

8 seconds

Devotion Cost:

20

Target:

Current enemy

Auto-Use:

Supported

Default Auto-Use:

Enabled

Weapon Requirement:

Warrior-compatible weapon

Description:

A heavy attack that deals increased weapon damage.

Heavy Strike should:

- Use the normal Accuracy calculation.
- Use the normal Critical Hit calculation.
- Grant Warrior XP from valid effective damage.
- Consume Devotion only when activated successfully.
- Enter cooldown after use.

The player may toggle Auto-Use before or during normal prototype Combat unless the broader Combat Framework prohibits changing that setting during Combat.

The Ability loadout itself should remain locked while Combat is active.

---

# Healing Potion Rules

The player begins with 5 Minor Healing Potions.

The Combat UI should support:

Potion icon

Quantity

Cooldown

Auto-Use toggle

Auto-Use Health threshold

Default threshold:

35% Health

When Health reaches or falls below the threshold:

- Check cooldown.
- Check potion quantity.
- Consume one potion.
- Heal 30 Health.
- Start the 10-second cooldown.
- Update Inventory.
- Save through normal state events.

If no potions remain:

- Display empty state.
- Do not generate free healing.

---

# Combat Selection Structure

Prototype Combat selection flow:

Greenvale

↓

Areas

↓

Greenvale Forest

↓

Forest Rat, Grey Wolf, or Forest Bandit

↓

Enemy Details

↓

Start Combat

The Combat Selection state should use one main vertical ScrollRect.

Horizontal selector rows may exist inside it.

---

# Region

## Greenvale

Region ID:

region_greenvale

Display Name:

Greenvale

Unlock Requirement:

Available from start

Recommended Warrior Level:

1–10

Description:

A temperate frontier Region containing weak wildlife and low-level bandits.

Prototype Combat Activity Types:

Areas

Future locked types may include:

Dungeons

Bosses

Tower

Do not implement those future types in this milestone.

---

# Combat Activity Type

## Areas

Activity Type ID:

combat_type_areas

Display Name:

Areas

Unlock Requirement:

Available from start

Description:

Open combat locations containing repeatable enemies.

---

# Combat Location

## Greenvale Forest

Location ID:

location_greenvale_forest

Display Name:

Greenvale Forest

Region:

Greenvale

Activity Type:

Areas

Unlock Requirement:

Available from start

Description:

A forest containing small animals, wolves, and wandering bandits.

Enemies:

Forest Rat

Grey Wolf

Forest Bandit

---

# Enemy 1: Forest Rat

Enemy ID:

enemy_forest_rat

Display Name:

Forest Rat

Region:

Greenvale

Location:

Greenvale Forest

Required Warrior Level:

1

Maximum Health:

30

Damage Range:

2–4

Attack Interval:

2.4 seconds

Accuracy:

35

Defense:

5

Critical Chance:

0%

XP Coefficient:

1.0

Respawn Time:

2 seconds

Difficulty:

Easy

Description:

A weak forest scavenger suitable for beginning Combat training.

## Forest Rat Loot

Guaranteed Gold:

1–3

Independent item rolls:

| Item | Chance | Quantity |
|---|---:|---:|
| Raw Meat | 50% | 1 |
| Rat Tail | 25% | 1 |

First-clear reward:

1 Minor Healing Potion

---

# Enemy 2: Grey Wolf

Enemy ID:

enemy_grey_wolf

Display Name:

Grey Wolf

Region:

Greenvale

Location:

Greenvale Forest

Required Warrior Level:

2

Maximum Health:

70

Damage Range:

4–7

Attack Interval:

2.2 seconds

Accuracy:

50

Defense:

12

Critical Chance:

3%

XP Coefficient:

1.2

Respawn Time:

3 seconds

Difficulty:

Appropriate

Description:

A fast predator with greater Health and Accuracy than the Forest Rat.

## Grey Wolf Loot

Guaranteed Gold:

3–6

Independent item rolls:

| Item | Chance | Quantity |
|---|---:|---:|
| Raw Meat | 50% | 1–2 |
| Wolf Pelt | 60% | 1 |
| Wolf Fang | 10% | 1 |

First-clear reward:

20 Gold

---

# Enemy 3: Forest Bandit

Enemy ID:

enemy_forest_bandit

Display Name:

Forest Bandit

Region:

Greenvale

Location:

Greenvale Forest

Required Warrior Level:

5

Maximum Health:

120

Damage Range:

4–7

Attack Interval:

2.6 seconds

Accuracy:

55

Defense:

18

Critical Chance:

5%

Critical Damage:

150%

XP Coefficient:

1.4

Respawn Time:

4 seconds

Difficulty:

Challenging

Description:

A lightly armed bandit carrying basic equipment and stolen supplies.

## Forest Bandit Loot

Guaranteed Gold:

8–15

Independent item rolls:

| Item | Chance | Quantity |
|---|---:|---:|
| Cloth Scrap | 60% | 1–2 |
| Rusted Dagger | 10% | 1 |

First-clear reward:

Worn Buckler

The Worn Buckler should be guaranteed on the first successful defeat.

If Temporary Loot cannot accept it:

- Send it to protected claim storage.
- Do not lose it.
- Display a claim notification.

---

# Combat Items

## Raw Meat

Item ID:

item_raw_meat

Category:

Resource

Stackable:

Yes

Sell Value:

2 Gold

Future Uses:

Cooking

Companion progression

---

## Rat Tail

Item ID:

item_rat_tail

Category:

Crafting Material

Stackable:

Yes

Sell Value:

3 Gold

Future Uses:

Herblore

Companion progression

Collection

---

## Wolf Pelt

Item ID:

item_wolf_pelt

Category:

Crafting Material

Stackable:

Yes

Sell Value:

5 Gold

Future Uses:

Leatherworking

Equipment crafting

---

## Wolf Fang

Item ID:

item_wolf_fang

Category:

Crafting Material

Stackable:

Yes

Sell Value:

15 Gold

Future Uses:

Jewelry

Weapons

Companion progression

---

## Cloth Scrap

Item ID:

item_cloth_scrap

Category:

Crafting Material

Stackable:

Yes

Sell Value:

4 Gold

Future Uses:

Tailoring

Equipment repairs if added

---

# Dropped Equipment

## Rusted Dagger

Item ID:

weapon_rusted_dagger

Item Type:

Weapon

Combat Discipline:

Warrior

Handedness:

One-Handed

Damage Range:

4–7

Attack Interval:

1.5 seconds

Accuracy Bonus:

5

Sell Value:

20 Gold

Description:

A fast but weak blade taken from a Forest Bandit.

The Rusted Dagger should be a sidegrade.

It attacks faster than the Worn Sword but deals less damage per hit.

---

## Worn Buckler

Item ID:

shield_worn_buckler

Item Type:

Shield

Defense Bonus:

8

Maximum Health Bonus:

10

Sell Value:

30 Gold

First-Clear Item:

Yes

Description:

A battered wooden shield that still provides useful protection.

The Worn Buckler must be equippable with the Worn Sword and Rusted Dagger.

Equipping it should update:

Maximum Health

Current Health handling

Defense

Combat estimates

Equipment UI

Save data

When Maximum Health increases:

Current Health may increase by the same flat amount for the prototype.

When unequipped:

Current Health must not remain above the new Maximum Health.

---

# Combat Start Validation

Before Combat starts, validate:

- Selected Region is valid.
- Selected Activity Type is valid.
- Selected Location is valid.
- Selected enemy is valid.
- Enemy unlock requirement is met.
- Player has an equipped compatible weapon.
- No incompatible primary activity remains active.
- Player Health is above zero.
- Save and gameplay systems are initialized.

If Woodcutting is active:

Display confirmation that starting Combat will stop it.

---

# Active Combat Loop

When Combat begins:

1. Save the selected Combat path.
2. Stop incompatible primary activity.
3. Hide or collapse Combat Selection.
4. Show Active Combat.
5. Set player Health and Devotion to their valid current values.
6. Start player attack timer.
7. Start enemy attack timer.
8. Enable Ability processing.
9. Enable Healing Potion processing.
10. Update Active Activity Bar.
11. Autosave.

During Combat:

- Player and enemy attack automatically.
- Heavy Strike processes according to Auto-Use state.
- Healing Potion processes according to Auto-Use state.
- Warrior XP is gained from valid player damage.
- Health and Devotion update.
- Combat Log receives events.
- UI updates individual values without rebuilding full panels.

---

# Enemy Defeat

When enemy Health reaches zero:

- Stop enemy actions.
- Stop invalid pending attacks.
- Award final valid Warrior XP.
- Generate enemy loot once.
- Add loot to Temporary Combat Loot.
- Apply first-clear reward if not previously claimed.
- Increment enemy kill count.
- Mark first-clear completion.
- Save progress.
- Enter enemy respawn state.

If Auto Repeat is enabled:

- Wait for respawn time.
- Restore enemy Health.
- Begin the next encounter.

If Auto Repeat is disabled:

- End active Combat.
- Return to Combat Selection with the previous enemy selected.

---

# Player Defeat

When player Health reaches zero:

- Stop Combat.
- Stop all player and enemy attack timers.
- Do not generate enemy defeat rewards.
- Do not remove equipped items.
- Do not remove XP already earned from valid damage.
- Consumables already used remain consumed.
- Preserve Temporary Combat Loot.
- Show defeat notification.
- Return to Combat Selection.
- Restore player Health to Maximum Health after defeat.
- Restore Devotion to Maximum Devotion for the next attempt.
- Autosave.

There is no permanent item loss.

There is no XP loss.

---

# Combat Auto Repeat

Auto Repeat should be available for normal Area enemies.

Default:

Enabled

Auto Repeat stops when:

- Player is defeated.
- Player manually quits.
- Inventory or Temporary Loot creates an unsafe state.
- Required equipment becomes invalid.
- Save data becomes invalid.
- The player changes target.
- A critical system error occurs.

---

# Temporary Combat Loot

Combat drops first enter Temporary Combat Loot.

Prototype Temporary Loot capacity:

20 distinct item stacks

Stackable identical items should combine into one Temporary Loot entry.

Non-stackable equipment uses one entry per item.

Temporary Loot should display:

Item icon

Item name

Quantity

New Collection marker if supported

Important marker

Pick action

Pick All

Capacity

Full warning

---

# Temporary Loot Claiming

Clicking a loot item should request that item to be claimed.

Pick All should request all claimable items.

The gameplay system must validate:

- Item still exists in Temporary Loot.
- Inventory can accept it.
- Quantity is valid.
- The claim has not already happened.

The UI must not remove the loot visually until the gameplay system confirms success.

If Inventory cannot accept an item:

- Leave it in Temporary Loot.
- Display the reason.
- Do not delete it.

---

# Temporary Loot Full Behavior

If Temporary Loot reaches capacity:

- Existing matching stackable drops may continue stacking.
- New distinct drops that do not fit should stop Auto Repeat safely.
- Important first-clear rewards should enter protected claim storage.
- Display a high-priority warning.
- Preserve all existing loot.

Example:

Temporary Loot Full

Combat has stopped.

Claim items before continuing.

---

# Combat Log

The Combat Log should display:

Player attacks

Player misses

Enemy attacks

Enemy misses

Heavy Strike

Critical Hits

Healing Potion use

Enemy defeated

Player defeated

Gold gained

Items dropped

Rare or equipment drops

The log should use pooled rows.

Suggested retained entries:

200

When the maximum is reached:

Recycle the oldest entries.

Do not allow unlimited UI object growth.

---

# Combat Screen: Selection State

The Combat Selection state should display:

Combat Header

Selection breadcrumb

Region row

Activity Type row

Location row

Enemy row

Enemy details

Loot preview

Start Combat button

Only relevant rows should appear.

Changing Region clears:

- Activity Type
- Location
- Enemy
- Preparation details

Changing Activity Type clears:

- Location
- Enemy
- Preparation details

Changing Location clears:

- Enemy
- Preparation details

The selected option in each row should remain highlighted.

---

# Combat Breadcrumb

Examples:

Greenvale > Areas > Greenvale Forest > Forest Rat

Greenvale > Areas > Greenvale Forest > Grey Wolf

The player may click an earlier breadcrumb step.

Changing an earlier selection clears later selections.

---

# Enemy Details

Selected enemy details should display:

Enemy portrait or icon

Name

Description

Required Warrior Level

Health

Damage Range

Attack Interval

Accuracy

Defense

Difficulty

Known loot

Drop chances

First-clear reward

Start Combat button

Locked enemies should display their requirement.

Example:

Requires Warrior Level 5

---

# Active Combat Screen

The Active Combat state should display:

Player Panel

Enemy Panel

Companion placeholder only if required by the reference image

Player Health

Player Devotion

Enemy Health

Player attack progress

Enemy attack progress

Heavy Strike

Healing Potion

Active buffs and debuffs area

Temporary Combat Loot

Combat Log

Quit Combat

Auto Repeat

Critical Health, Devotion, Enemy Health, and Quit controls must remain visible without scrolling.

Do not place the complete Active Combat interface inside one large vertical ScrollRect.

Combat Log and Temporary Loot may scroll independently.

---

# Companion Prototype Rule

Companion gameplay is not implemented in this milestone.

If the reference UI contains a Companion panel:

- Display an empty placeholder.
- Label it as no Companion assigned.
- Do not create Companion Health.
- Do not create Companion targeting.
- Do not simulate Companion damage.

Future Combat Companions:

- Deal damage.
- Cannot be targeted.
- Do not have Health.
- Do not grant Combat Discipline XP through their damage.

---

# Equipment System

The Equipment Screen should support these slots:

Weapon

Shield

Helmet

Chest

Legs

Gloves

Boots

Cape

Ring

Amulet

Relic

Tool

For the prototype, only these slots require working items:

Weapon

Shield

Tool

Other slots may display empty placeholders.

---

# Equipment Screen

Required elements:

Equipment slots

Current equipped items

Current combat stats

Selected item details

Available equipment list

Comparison panel

Equip button

Unequip button

Two-handed restriction support in architecture

Runecrafting shortcut may remain disabled or hidden

The Equipment list should be scrollable.

---

# Equipping Items

When Equip is pressed:

1. Validate item ownership.
2. Validate equipment type.
3. Validate Combat Discipline requirement.
4. Validate slot compatibility.
5. Validate two-handed restrictions.
6. Unequip conflicting item safely.
7. Equip selected item.
8. Recalculate player stats through gameplay systems.
9. Update Equipment UI.
10. Update Inventory.
11. Autosave.

The UI must not directly edit final combat stats.

It should request equipment changes through the Equipment System.

---

# Unequipping Items

When Unequip is pressed:

- Validate Inventory capacity.
- Remove item from equipment slot.
- Return it to Inventory.
- Recalculate stats.
- Update UI.
- Autosave.

If Inventory is full:

- Prevent unsafe unequip.
- Display a clear reason.

---

# Equipment Comparison

When selecting Rusted Dagger or Worn Buckler, display:

Current item

Selected item

Damage change

Attack interval change

Accuracy change

Defense change

Maximum Health change

New effects

Lost effects

Use:

Up arrow

Down arrow

Plus and minus values

Color plus icon

Do not communicate comparison using color alone.

---

# Inventory Screen

Required prototype structure:

Fixed Inventory Header

Capacity display

Search

Basic filters

Sorting

Scrollable Item Grid

Selected Item Details

Item Actions

Required actions:

Equip

Unequip through Equipment screen

Consume Healing Potion only during valid context if manual use is supported

Lock

Unlock

Favorite

Unfavorite

Destroy with confirmation

View Sources where possible

Sell may remain disabled until the Shop is implemented

---

# Item Details

Selected Item Details should display:

Item icon

Name

Description

Category

Current quantity

Stackability

Sell value

Equipment stats if applicable

Primary uses

Known sources

Locked state

Favorite state

Equipped state

Collection discovery if implemented

Lifetime obtained if Collection tracking already exists

---

# Collection Tracking in Prototype

Full Collection Log implementation is not required.

However, item acquisition should be compatible with the Collection system.

At minimum, save:

- Whether the item has ever been obtained.
- Lifetime quantity obtained.

Obtaining an item from:

Woodcutting

Combat

First-clear rewards

Starting Inventory

Should update Collection-compatible data.

Starting items may count as discovered.

Selling, consuming, destroying, or equipping should not remove discovery.

---

# Basic Notifications

Required notifications:

Woodcutting started

Woodcutting stopped

Tree completed

Woodcutting level gained

Combat started

Enemy defeated

Warrior level gained

Player defeated

Item found

Equipment found

Inventory full

Temporary Loot full

First-clear reward

Save completed if shown

Save failed

Offline progress available

Notifications should be grouped when repeated.

Example:

Sproutwood Log +4

Instead of four separate notifications.

---

# Tooltip Support

Prototype tooltips should support:

Items

Equipment stats

Tree rewards

Enemy stats

Drop chances

Abilities

Healing Potion

Locked requirements

Navigation entries when collapsed

Tooltips should remain inside screen boundaries.

Important information must also be accessible without hover.

---

# Popup Support

Required popup types:

Confirmation Popup

Information Popup

Warning Popup

Error Popup

Examples:

Starting Combat will stop Woodcutting.

Destroy this item?

Quit Combat?

Inventory is full.

Save failed.

Generic Popup systems should not contain gameplay logic.

They should return the player's result to the requesting controller.

---

# Save System

The prototype must use the approved Save System Framework.

Save format:

Local structured save data

Recommended:

JSON during development

Save system should include:

Version number

Atomic write behavior

Backup save

Validation

Migration structure

Corruption handling

---

# Required Save Data

The prototype must save:

Save version

Last save timestamp

Gold

Inventory capacity

Inventory item stacks

Non-stackable equipment items

Equipped items

Woodcutting level

Woodcutting XP

Warrior level

Warrior XP

Current Health if intended

Current Devotion if intended

Selected Woodcutting tree

Active primary activity

Combat Region selection

Combat Activity Type selection

Combat Location selection

Selected enemy

Enemy first-clear states

Enemy kill counts

Temporary Combat Loot

Protected claim storage

Healing Potion quantity

Ability Auto-Use setting

Combat Auto Repeat setting

Collection-compatible item discovery

Lifetime obtained quantities

UI settings

Navigation collapsed state

Last opened screen if desired

---

# Save Timing

Autosave after meaningful actions.

Examples:

Activity started

Activity stopped

Tree completed

Level gained

Combat started

Combat ended

Enemy defeated

First-clear reward generated

Loot claimed

Equipment changed

Inventory changed significantly

Offline progress applied

Suggested timed autosave:

Every 30 seconds

Do not save every frame.

---

# Save Safety

The save process should:

1. Serialize current valid state.
2. Write to a temporary file.
3. Validate completion.
4. Replace the main save.
5. Preserve a backup.

If save fails:

- Keep the game running when safe.
- Display a critical notification.
- Do not falsely display Saved.
- Log technical details.

---

# Loading

At launch:

1. Initialize static databases.
2. Load or create save data.
3. Validate IDs.
4. Apply migrations if needed.
5. Initialize gameplay systems.
6. Initialize UI.
7. Restore Inventory and Equipment.
8. Restore progression.
9. Calculate offline progress.
10. Open default or last valid screen.
11. Show Offline Progress Summary if required.

If saved content references a missing ID:

- Ignore or replace it safely.
- Log a warning.
- Do not crash the game.

---

# New Save Creation

When no valid save exists:

Create a new save with:

50 Gold

100 Inventory slots

Worn Sword equipped

5 Minor Healing Potions

Bare Hands tool active

Woodcutting Level 1

Warrior Level 1

No active activity

No Combat first-clears

Empty Temporary Combat Loot

Default UI state

---

# Data Architecture

Use stable IDs.

Required data categories:

Item Definitions

Equipment Definitions

Profession Definition

Woodcutting Activity Definitions

Enemy Definitions

Region Definition

Combat Location Definition

Combat Activity Type Definition

Loot Tables

Ability Definition

XP Tables

Screen Definitions

Navigation Definitions

Do not use visible names as data keys.

---

# Required Stable IDs

## Screens

woodcutting

combat

inventory

equipment

## Profession

profession_woodcutting

## Trees

tree_sproutwood

tree_mosswood

tree_ironbark

## Region and Combat

region_greenvale

combat_type_areas

location_greenvale_forest

enemy_forest_rat

enemy_grey_wolf

enemy_forest_bandit

## Ability

ability_warrior_heavy_strike

## Equipment

weapon_worn_sword

weapon_rusted_dagger

shield_worn_buckler

tool_bare_hands

## Consumable

consumable_minor_healing_potion

## Woodcutting Items

item_log_sproutwood

item_sproutwood_sap

item_log_mosswood

item_moss_resin

item_log_ironbark

item_ironbark_knot

## Combat Items

item_raw_meat

item_rat_tail

item_wolf_pelt

item_wolf_fang

item_cloth_scrap

---

# UI Implementation Rules

Use:

Unity Canvas / uGUI

TextMeshPro

Reusable prefabs

Stable Screen IDs

Data-driven content

One active main screen

One EventSystem

InputSystemUIInputModule

One primary vertical ScrollRect per information-heavy screen

Event-driven refresh

Pooled repeated entries where useful

Do not use:

UI Toolkit for the main gameplay interface

Separate scenes for every screen

Legacy Unity Text

Hardcoded activity cards

Hardcoded enemy buttons

GameObject.Find during routine gameplay

One Update method on every card

Unlimited Combat Log entries

Static screenshots as functional UI

---

# Placeholder Art

Placeholder art is acceptable.

Use:

Simple silhouettes

Clearly labeled icons

Solid-color panels

Temporary symbols

Consistent placeholder frames

Do not:

Import random internet images

Use copyrighted images without approval

Mix unrelated visual styles

Treat reference screenshots as the actual interface

Final artwork should be replaceable without rewriting gameplay logic.

---

# Required Reusable UI Components

At minimum, create reusable components for:

Primary Button

Secondary Button

Danger Button

Navigation Button

Screen Header

Progress Bar

Timer Display

Item Slot

Equipment Slot

Activity Card

Enemy Card

Region Button

Requirement Row

Stat Row

Tooltip Trigger

Notification Toast

Confirmation Popup

Empty State

Combat Log Row

Temporary Loot Entry

---

# UI State Preservation

While the game remains open, preserve:

Woodcutting selected tree

Woodcutting scroll position

Combat selected Region

Combat selected Activity Type

Combat selected Location

Combat selected enemy

Combat selection scroll position

Inventory selected item

Inventory search

Inventory filter

Inventory sorting

Equipment selected item

Navigation collapsed state

Do not preserve:

Open tooltip

Pointer hover

Half-open dropdown

Animation state

---

# Prototype Implementation Order

Codex should build this milestone in phases.

Do not attempt every phase in one uncontrolled task.

---

# Phase 0: Existing Project Inspection

Before creating code:

- Inspect the current Unity project.
- Inspect existing scenes.
- Inspect existing scripts.
- Inspect existing UI.
- Inspect existing databases.
- Inspect existing save structures.
- Inspect existing prefabs.
- Reuse working systems.
- Avoid duplicate managers.
- Avoid creating a second EventSystem.
- Avoid creating a second Screen Manager.
- Report conflicts before replacing major working systems.

Acceptance:

Codex understands what already exists and identifies what can be reused.

---

# Phase 1: Project Foundation

Create or confirm:

GameBootstrap

GameSystems root

UI System root

Main gameplay scene

Input System configuration

One EventSystem

Core service initialization

Stable ID conventions

Acceptance:

The project enters Play Mode without errors.

The bootstrap initializes systems in a deterministic order.

---

# Phase 2: Persistent UI Shell

Create or confirm:

Main Canvas

Canvas Scaler

Top Bar

Main Body

Left Navigation

Content Column

Active Activity Bar

Screen Container

Overlay Layers

Popup Layer

Modal Layer

Loading Layer

Acceptance:

The interface fills 1920 × 1080 correctly.

Top Bar and Active Activity Bar are separate.

Persistent UI remains visible during screen changes.

---

# Phase 3: Screen Manager and Navigation

Create:

ScreenDefinition

UIScreenCatalog

ScreenManager

Navigation definitions

Navigation groups

Navigation buttons

Back navigation

Screen state storage

Acceptance:

Woodcutting, Combat, Inventory, and Equipment test screens can open.

Only one main screen is active.

Selected Navigation entry updates.

Back navigation works.

---

# Phase 4: Item, Inventory, and Gold Foundation

Create:

ItemDefinition

Item Database

Inventory System

Gold storage

Stackable items

Non-stackable equipment items

Inventory capacity

Inventory screen

Item Details panel

Item Slot prefab

Basic Inventory events

Acceptance:

Starting items appear.

Gold displays correctly.

Items can be added and removed safely.

Capacity updates.

Inventory survives save and load.

---

# Phase 5: Equipment Foundation

Create:

Equipment System

Equipment slots

Worn Sword

Worn Buckler

Rusted Dagger

Bare Hands Tool

Equipment screen

Stat recalculation

Equipment comparison

Acceptance:

Worn Sword starts equipped.

Worn Buckler can be equipped.

Rusted Dagger can replace Worn Sword.

Stats update correctly.

Equipment survives save and load.

---

# Phase 6: Woodcutting Vertical Slice

Create:

Woodcutting Profession data

Woodcutting progression

Tree data

Woodcutting Screen

Activity cards

Target durability

Action timer

Reward thresholds

Completion XP

Rare rewards

Respawn

Start and Stop

Active Activity Bar integration

Inventory Full handling

Acceptance:

All three trees appear.

Locked trees display requirements.

Sproutwood can be cut at Level 1.

Tree durability decreases.

Threshold rewards are granted once.

Completion XP is granted.

Tree respawns.

Woodcutting continues while Inventory is open.

---

# Phase 7: Save and Offline Woodcutting

Create:

Save data

Autosave

Manual save hooks if used

Backup save

Offline timestamp

Offline Woodcutting simulation

Offline Summary

Acceptance:

Woodcutting progress survives restart.

Offline rewards are calculated.

The 12-hour cap works.

Inventory limits are respected.

No reward is silently lost.

---

# Phase 8: Combat Selection

Create:

Greenvale Region

Areas Activity Type

Greenvale Forest

Three enemy definitions

Combat Selection state

Region row

Activity Type row

Location row

Enemy row

Breadcrumb

Enemy details

Loot preview

Start Combat validation

Acceptance:

Selections reveal progressively.

Changing an earlier selection clears later selections.

Locked enemies show required Warrior Level.

Selected enemy state is preserved.

---

# Phase 9: Active Combat

Create:

Player Combat state

Enemy Combat state

Auto-attacks

Accuracy

Defense mitigation

Critical Hits

Health

Devotion

Heavy Strike

Healing Potion

Enemy defeat

Player defeat

Respawn

Auto Repeat

Combat Log

Active Activity Bar integration

Acceptance:

Player and enemy attack correctly.

Health changes correctly.

Devotion regenerates.

Heavy Strike consumes Devotion.

Healing Potion consumes Inventory quantity.

Warrior XP is granted from valid player damage.

Player defeat is safe.

---

# Phase 10: Combat Loot

Create:

Enemy loot tables

Gold rewards

Temporary Combat Loot

Pick item

Pick All

Temporary Loot capacity

First-clear rewards

Protected claim behavior

Acceptance:

Loot generates exactly once per enemy defeat.

Items remain in Temporary Loot until claimed.

Pick and Pick All work.

Inventory Full does not delete items.

Worn Buckler is guaranteed on first Forest Bandit defeat.

---

# Phase 11: Full Save and Reload Test

Verify saving of:

Inventory

Gold

Equipment

Woodcutting progress

Warrior progress

Enemy first-clears

Temporary Combat Loot

Combat selections

Active activity

UI preferences

Acceptance:

The player can close and reopen without losing meaningful progress.

No duplicate rewards appear after loading.

No first-clear reward can be claimed twice.

---

# Phase 12: UI Reference Pass

Compare implemented screens against approved reference images.

Improve:

Panel proportions

Information hierarchy

Spacing

Text sizes

Progress-bar placement

Button placement

Card density

Scrolling

Do not change working gameplay architecture only to imitate a static screenshot.

Acceptance:

The game resembles the intended visual references while remaining dynamic.

---

# Phase 13: Testing and Cleanup

Test:

Woodcutting for extended periods

Combat for extended periods

Inventory Full

Temporary Loot Full

Player defeat

Save failure simulation

Repeated screen switching

Repeated save and load

Large item quantities

Resolution changes

UI scaling

Remove:

Duplicate managers

Unused scripts

Temporary debug hacks

Unlimited logs

Repeated scene searches

Excessive Update methods

Acceptance:

The prototype runs without repeating errors or continuously increasing UI object counts.

---

# Required Functional Tests

## New Game

✓ New save starts with correct Gold.

✓ Worn Sword is equipped.

✓ Bare Hands appears as Tool.

✓ 5 Healing Potions exist.

✓ Inventory capacity is 100.

✓ Woodcutting Level is 1.

✓ Warrior Level is 1.

---

## Woodcutting

✓ Sproutwood is unlocked.

✓ Mosswood requires Level 5.

✓ Ironbark requires Level 10.

✓ Durability decreases by 5 every 1.5 seconds.

✓ Threshold rewards occur once.

✓ XP occurs at completion.

✓ Rare reward rolls once at completion.

✓ Tree respawns.

✓ Activity continues on another screen.

✓ Stop works.

✓ Inventory Full stops safely.

---

## Combat Selection

✓ Greenvale is available.

✓ Areas is available.

✓ Greenvale Forest is available.

✓ Forest Rat is available.

✓ Grey Wolf requires Warrior Level 2.

✓ Forest Bandit requires Warrior Level 5.

✓ Breadcrumb updates.

✓ Earlier selection changes clear later selections.

---

## Active Combat

✓ Player attack timer works.

✓ Enemy attack timer works.

✓ Accuracy works.

✓ Defense mitigation works.

✓ Critical Hits work.

✓ Heavy Strike works.

✓ Devotion cost works.

✓ Devotion regeneration works.

✓ Healing Potion works.

✓ Player defeat works.

✓ Enemy defeat works.

✓ Auto Repeat works.

---

## Combat XP

✓ Only valid player damage grants Warrior XP.

✓ Misses grant no XP.

✓ Overkill grants no extra XP.

✓ Enemy defeat does not duplicate XP.

✓ Level-up preserves excess XP.

---

## Loot

✓ Gold is granted once.

✓ Independent item rolls work.

✓ Loot enters Temporary Loot.

✓ Pick works.

✓ Pick All works.

✓ Inventory Full leaves items unclaimed.

✓ Temporary Loot Full stops safely.

✓ First-clear reward occurs once.

✓ Worn Buckler cannot be lost.

---

## Equipment

✓ Worn Sword starts equipped.

✓ Rusted Dagger can be equipped.

✓ Worn Buckler can be equipped.

✓ Stats recalculate.

✓ Current Health respects new Maximum Health.

✓ Unequip checks Inventory capacity.

✓ Equipment survives save and load.

---

## Save and Load

✓ Gold persists.

✓ Inventory persists.

✓ Equipment persists.

✓ XP persists.

✓ Levels persist.

✓ First-clear state persists.

✓ Temporary Loot persists.

✓ Active Woodcutting persists.

✓ Offline time calculates once.

✓ Loading cannot duplicate rewards.

---

# Resolution Tests

Test at minimum:

1920 × 1080

2560 × 1440

Ultrawide landscape

Reduced-height window

Verify:

- Text remains readable.
- Buttons remain clickable.
- Navigation remains accessible.
- Main screens scroll correctly.
- Tooltips stay inside the Canvas.
- Popups remain visible.
- Combat critical information remains fixed.
- Inventory grid remains usable.
- Activity cards do not overlap.

---

# Performance Expectations

The prototype should:

- Open screens quickly.
- Avoid major frame spikes during routine navigation.
- Avoid rebuilding full lists for one value change.
- Avoid unlimited Combat Log growth.
- Avoid unlimited Notification growth.
- Avoid one Update method per static card.
- Pool repeated Combat Log rows.
- Pool repeated loot entries where practical.
- Cache references.
- Use gameplay events.
- Avoid repeated global searches.

---

# Debug Tools

Development-only debug controls may include:

Set Woodcutting Level

Set Warrior Level

Grant XP

Grant Gold

Grant item

Fill Inventory

Clear Inventory

Force rare tree reward

Force enemy item drop

Force first-clear reset

Start test Woodcutting

Start test Combat

Defeat current enemy

Defeat player

Simulate 1 hour offline

Simulate 12 hours offline

Open any screen

Display save path

Force save

Force load

Debug tools must not appear in release builds.

---

# Codex Working Rules

Codex should:

- Inspect the existing project first.
- Reuse working architecture.
- Implement one phase at a time.
- Keep the project compiling after every phase.
- Use stable IDs.
- Use ScriptableObjects or approved data assets.
- Use reusable prefabs.
- Keep UI separate from gameplay logic.
- Add validation.
- Add clear development logs.
- Test before moving to the next phase.
- Summarize created and modified files after each task.
- Report unfinished work honestly.

Codex should not:

- Rewrite the whole project without reason.
- Create duplicate managers.
- Create duplicate EventSystems.
- Create a separate scene per screen.
- Use UI Toolkit for main gameplay UI.
- Hardcode every tree or enemy inside UI scripts.
- Put combat formulas inside UI components.
- Put Inventory mutations directly inside buttons.
- Generate random unapproved systems.
- Add Companion Health.
- Add item rarity.
- Add Ranger or Mage early.
- Add full Dungeons, Bosses, or Tower.
- Change Rune consumption rules.
- Use static screenshots as the interface.
- Import random copyrighted artwork.
- Silently delete rewards.
- Silently reset player progress.

---

# Milestone Success Criteria

This milestone succeeds when it proves that the following systems work together:

Persistent UI

Navigation

Screen management

Data-driven content

Woodcutting

Combat

Inventory

Equipment

XP

Levels

Loot

Gold

Saving

Loading

Offline progress

The prototype does not need to look final.

It must be stable enough that future content can be added without replacing the foundation.

---

# Final Playable Test

A tester should be able to complete this sequence:

1. Start a new game.
2. See 50 Gold.
3. Open Woodcutting.
4. Cut Sproutwood Tree.
5. Receive Sproutwood Logs.
6. Gain Woodcutting XP.
7. Unlock Mosswood at Level 5.
8. Open Inventory while Woodcutting continues.
9. Stop Woodcutting.
10. Open Combat.
11. Select Greenvale.
12. Select Areas.
13. Select Greenvale Forest.
14. Fight Forest Rat.
15. Gain Warrior XP.
16. Claim Raw Meat or Rat Tail.
17. Reach Warrior Level 2.
18. Fight Grey Wolf.
19. Claim Wolf Pelt or Wolf Fang.
20. Reach Warrior Level 5.
21. Fight Forest Bandit.
22. Receive Worn Buckler first-clear reward.
23. Claim Worn Buckler.
24. Open Equipment.
25. Equip Worn Buckler.
26. Confirm Health and Defense increase.
27. Save and exit.
28. Reload the game.
29. Confirm all progress remains.
30. Start Woodcutting.
31. Close the game.
32. Return later.
33. Receive Offline Woodcutting rewards.

When this sequence works reliably, Prototype Vertical Slice 01 is complete.

---

# Final Checklist

## Foundation

✓ One primary gameplay scene exists.

✓ One EventSystem exists.

✓ Unity Input System is used.

✓ Persistent UI shell exists.

✓ Top Bar and Active Activity Bar are separate.

✓ Only one main screen is active.

---

## Data

✓ Stable IDs are used.

✓ Items are data-driven.

✓ Trees are data-driven.

✓ Enemies are data-driven.

✓ Loot tables are data-driven.

✓ XP thresholds are centralized.

✓ Combat formulas are centralized.

---

## Woodcutting

✓ Three trees exist.

✓ Level requirements work.

✓ Durability works.

✓ Reward thresholds work.

✓ Completion XP works.

✓ Respawn works.

✓ Automatic repetition works.

✓ Inventory Full handling works.

✓ Offline Woodcutting works.

---

## Combat

✓ One Region exists.

✓ One Area exists.

✓ One Location exists.

✓ Three monsters exist.

✓ Combat selection works.

✓ Auto-attacks work.

✓ Heavy Strike works.

✓ Healing Potion works.

✓ Warrior XP works.

✓ Enemy defeat works.

✓ Player defeat works.

✓ Auto Repeat works.

---

## Loot

✓ Gold rewards work.

✓ Item drops work.

✓ Temporary Loot works.

✓ Pick works.

✓ Pick All works.

✓ First-clear rewards work.

✓ Protected claim behavior works.

✓ Rewards cannot duplicate.

---

## Inventory and Equipment

✓ Inventory starts at 100 slots.

✓ Starting items exist.

✓ Item stacks work.

✓ Equipment items work.

✓ Worn Sword starts equipped.

✓ Worn Buckler can be equipped.

✓ Rusted Dagger can be equipped.

✓ Stats recalculate correctly.

---

## Saving

✓ Autosave works.

✓ Backup save works.

✓ Gold persists.

✓ Inventory persists.

✓ Equipment persists.

✓ XP and levels persist.

✓ First-clear states persist.

✓ Temporary Loot persists.

✓ Offline progress applies once.

---

## UI

✓ Left Navigation is scrollable.

✓ Woodcutting screen is scrollable.

✓ Combat Selection is scrollable.

✓ Active Combat keeps critical information fixed.

✓ Inventory grid scrolls.

✓ Equipment list scrolls.

✓ Tooltips remain in bounds.

✓ Popups block input correctly.

✓ UI matches reference direction.

---

## Safety

✓ Inventory Full never silently deletes rewards.

✓ Temporary Loot Full never silently deletes important rewards.

✓ Player defeat does not remove equipment.

✓ Invalid IDs do not crash the game.

✓ Save failure is reported.

✓ No duplicate EventSystem exists.

✓ No duplicate manager architecture exists.

✓ No core gameplay logic exists inside UI views.