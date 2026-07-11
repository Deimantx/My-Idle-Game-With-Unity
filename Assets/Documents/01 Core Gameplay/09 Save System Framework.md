# 09 Save System Framework

Version: 1.0
Status: Draft

---

# Purpose

This document defines how saving, loading, and persistent data work across the entire game.

The Save System is responsible for preserving player progress across sessions.

Every major system should save and load through this framework unless explicitly designed otherwise.

The Save System must support professions, Combat Disciplines, inventory, equipment, companions, crafting, combat state, offline progress, achievements, collections, settings, and future systems.

---

# Design Goals

The Save System should:

- Preserve all important player progress.
- Be reliable and safe.
- Support offline progress.
- Support future updates.
- Avoid corrupted saves whenever possible.
- Be easy for Codex to expand.
- Store IDs instead of duplicated database data.
- Save automatically at important moments.
- Load safely even if some data changes later.
- Support local save first.
- Support future cloud save later.

The player should never feel afraid of losing progress.

---

# Save System Philosophy

The game is an idle RPG with long-term progression.

Because progress is permanent, saving must be treated as a core system.

The Save System should prioritize:

Reliability

Stability

Future compatibility

Clear structure

Safe loading

Easy debugging

The game should never depend only on memory for important progression.

---

# Save Location

Current design:

Use local save files.

Recommended Unity save location:

Application.persistentDataPath

Save files should not rely only on PlayerPrefs.

PlayerPrefs may be used only for small settings if needed.

Main progression data should be saved in a structured save file.

Future option:

Cloud save

Steam Cloud

Mobile cloud save

Account-based save sync

---

# Save Format

Current recommended format:

JSON

JSON is useful because it is:

Readable

Easy to debug

Easy for Codex to edit

Easy to expand

Easy to inspect during development

Future option:

Binary save format

Encrypted save format

Compressed save format

Database-style save format

Do not use complex save formats too early.

---

# Save File Structure

The main save file should contain:

Save Version

Player Data

Profession Data

Combat Discipline Data

Inventory Data

Equipment Data

Companion Data

Crafting Data

Combat State Data

Temporary Combat Loot Data

Offline Reward Claim Data

Achievement Data

Collection Data

Statistics Data

Settings Data

Timestamp Data

Future Expansion Data

Each major system should own its own save section.

---

# Save Versioning

Every save file should include a save version.

Example:

Save Version: 1

Save version is used to support future updates.

When the game changes, old saves may need to be upgraded.

The Save System should be able to:

Read save version

Detect old save data

Apply migration if needed

Load safely if possible

Warn if save is unsupported

Do not break old saves without good reason.

---

# Save Data Rules

Save data should store player-owned progress.

Save data should not duplicate static database data.

Save data should store IDs that reference databases.

Examples:

Save Item ID, not full item description.

Save Equipment ID, not full equipment database entry.

Save Companion ID, not full companion design data.

Save Profession ID, not full profession definition.

Save Enemy ID if combat state needs it.

Databases define static data.

Save files define player progress.

---

# Player Data

Player data may include:

Player Name

Account Creation Time

Total Play Time

Last Login Time

Last Save Time

Current Active Screen if needed

Tutorial Progress

Future Account Progression

Future Prestige Data

Player data should remain simple early.

---

# Profession Save Data

Every profession should save:

Profession ID

Current Level

Current XP

Unlocked Activities

Current Active Activity if applicable

Total Actions Completed

Highest Unlock Reached

Profession-specific statistics if needed

Examples:

Woodcutting Level

Woodcutting XP

Current selected tree

Total trees cut

Mining Level

Mining XP

Current selected rock

Total ores mined

Profession save data should not duplicate profession activity database data.

---

# Combat Discipline Save Data

Every Combat Discipline should save:

Combat Discipline ID

Current Level

Current XP

Unlocked Combat Abilities

Total Damage Dealt

Total Enemies Killed if relevant

Highest Unlock Reached

Current Combat Disciplines:

Warrior

Ranger

Mage

Combat Discipline data should be separate from professions.

---

# Inventory Save Data

Inventory should save:

Item ID

Quantity

Locked Status

Favorite Status

New Item Status

Inventory Capacity

Temporary Combat Loot if needed

Offline Reward Claim data if needed

Inventory should not save static item data such as item name, icon, description, or category.

Those come from the Item Database.

---

# Stackable Item Save Data

Stackable items should save:

Item ID

Quantity

Locked Status if needed

Favorite Status if needed

New Item Status if needed

Example:

Item ID: oak_log

Quantity: 5420

---

# Non-Stackable Item Save Data

Non-stackable items may need unique item instance data.

Examples:

Equipment

Relics

Future enchanted items

Future random affix items

Future durability items

Future socketed items

Non-stackable item instances should save:

Item Instance ID

Item ID

Locked Status

Favorite Status

Upgrade Level if applicable

Enchantments if applicable

Durability if future system uses it

Socketed Gems if future system uses it

Custom Data if needed

This allows two copies of the same equipment item to have different upgrade states later.

---

# Equipment Save Data

Equipment should save:

Equipped Weapon

Equipped Shield

Equipped Helmet

Equipped Chest

Equipped Legs

Equipped Gloves

Equipped Boots

Equipped Cape

Equipped Ring

Equipped Amulet

Equipped Relic

Equipped Tool

Equipment should reference item instance IDs when equipment can have unique states.

Equipment should reference Item IDs only if equipment has no unique instance data.

Equipped items must save and load correctly.

---

# Combat Loadout Save Data

Combat loadout may save:

Selected Combat Abilities

Auto-use Ability Settings

Selected Food

Selected Potion

Selected Elixirs

Selected Relic

Selected Combat Companion

Auto Combat Settings

Manual Combat Settings

This allows the player to continue with the same setup after reopening the game.

---

# Companion Save Data

Every companion should save:

Companion ID

Unlocked Status

Current Rank

Assigned Status

Assigned Activity

Current Rank-Up Timer

Rank-Up Start Time

Rank-Up End Time

Items already consumed for rank-up if applicable

Companion statistics if needed

Companion data should not save static companion descriptions or base stats.

Those come from the Companion Database.

---

# Companion Timer Save Data

Companion rank-up timers should continue while the game is closed.

Timer data should save:

Companion ID

Rank being upgraded to

Start Time

End Time

Is Timer Active

When the player returns, the game should check whether the timer completed.

Completed rank-ups should be claimable.

---

# Crafting Save Data

Crafting may save:

Active Crafting Jobs

Crafting Start Time

Crafting End Time

Crafting Recipe ID

Quantity Being Crafted

Materials Consumed Status

Crafting Profession Used

Crafting Queue if future system uses it

Crafting should clearly define whether materials are consumed at start or completion.

Current inventory framework says crafting materials are consumed when crafting completes.

Save data should match that rule.

---

# Combat State Save Data

Combat state may save if the player leaves during combat.

Combat state may include:

Is In Combat

Combat Mode

Current Enemy ID

Current Dungeon ID if applicable

Current Wave if applicable

Current Enemy Health if needed

Player Health

Player Devotion

Selected Combat Abilities

Assigned Combat Companion

Active Food Buff

Active Elixirs

Available Potion

Relic State

Combat Start Time

Last Combat Save Time

Combat state is important for offline combat.

---

# Combat Save Philosophy

Regular combat may continue offline if allowed.

Hard bosses, raids, arenas, or manual-only encounters may disable offline combat.

If offline combat is disabled, the game should safely pause or end combat when the player leaves.

Combat save rules should be clear for each combat mode.

---

# Temporary Combat Loot Save Data

Temporary Combat Loot may save:

Item ID

Quantity

Drop Source

Drop Time

Is Important

Is First-Time Drop

Is Protected From Loss

Temporary Combat Loot should save if the player closes the game during combat.

Important loot should not be silently lost.

---

# Offline Reward Claim Save Data

Offline rewards may need temporary protected storage.

Offline Reward Claim data may save:

Generated Items

Generated XP

Generated Gold

Generated Rare Drops

Generated First-Time Items

Items That Could Not Fit Inventory

Time Away

Source Activity

Claimed Status

Offline rewards should remain claimable until the player accepts them.

Important offline rewards should not be lost because inventory is full.

---

# Achievement Save Data

Achievements should save:

Achievement ID

Progress Amount

Completed Status

Claimed Status if rewards must be claimed

Completion Time

Achievement save data should reference Achievement IDs.

Do not duplicate static achievement descriptions in save files.

---

# Collection Save Data

Collection data should save:

Item ID

Discovered Status

First Discovery Time

Total Unique Items Discovered

Collection category progress if needed

Collection status should not depend on keeping the item forever.

Once discovered, it remains discovered.

---

# Statistics Save Data

Statistics should save long-term tracking data.

Examples:

Total items obtained

Total gold earned

Total enemies killed

Total bosses killed

Total dungeons completed

Total profession actions completed

Total companion damage dealt

Total player damage dealt

Total resources gathered

Total crafting completed

Statistics should be separated by system where possible.

---

# Settings Save Data

Settings may save:

Audio Volume

Music Volume

SFX Volume

Graphics Settings

Language

UI Scale

Notification Settings

Combat Log Settings

Auto Combat Settings

Accessibility Settings

Settings may be saved separately from progression if useful.

---

# Timestamp Data

The Save System must save timestamps for offline progress.

Important timestamps:

Last Save Time

Last Login Time

Last Logout Time

Last Offline Progress Calculation Time

Companion Timer Start and End Times

Crafting Timer Start and End Times

Combat Start Time

Last Combat Tick Time if needed

Offline Progress depends on accurate timestamp data.

---

# Autosave Rules

The game should autosave regularly.

Recommended autosave timing:

Every 30–60 seconds during active play

Autosave should also happen after important events.

Important autosave events:

Level up

Item obtained

Rare item obtained

Equipment changed

Companion rank-up started

Companion rank-up claimed

Crafting started

Crafting completed

Combat ended

Dungeon completed

Boss defeated

Achievement completed

Shop purchase

Inventory capacity upgraded

Game paused

Game closed

Autosave should be reliable but not cause performance problems.

---

# Manual Save

The game may include a manual save button.

Manual save should:

Save current progress immediately

Show confirmation when complete

Fail safely if saving fails

Manual save is useful during development and for player confidence.

---

# Save On Quit / Pause

The game should save when:

Application quits

Application pauses

Application loses focus

Player returns to main menu if such menu exists

On mobile, pause/save behavior is very important.

The game should assume the player can close the app at any time.

---

# Atomic Save Rules

Saving should avoid corrupted files.

Recommended save process:

Create save data in memory.

Write to temporary save file.

Verify temporary save file if possible.

Replace old save file with temporary save file.

Keep backup of previous save.

This reduces risk of corrupted saves if the game closes during saving.

---

# Backup Save Rules

The game should keep at least one backup save.

Recommended backups:

Current Save

Previous Save Backup

Optional Emergency Backup

If the main save fails to load, the game should try loading the backup.

If backup loads successfully, the player should be warned and progress should be preserved as much as possible.

---

# Corruption Handling

If a save file is corrupted, the game should not crash.

The game should:

Detect load failure

Try backup save

Show clear error message if needed

Avoid overwriting backup immediately

Allow recovery when possible

Create new save only after confirmation if recovery fails

The player should not lose progress silently.

---

# Missing Data Handling

If save data references missing database data, the game should handle it safely.

Examples:

Missing Item ID

Missing Equipment ID

Missing Companion ID

Missing Profession ID

Missing Enemy ID

Missing Achievement ID

Missing Recipe ID

The game should not crash.

Possible handling:

Hide missing item but keep raw ID

Replace with placeholder item

Skip missing entry safely

Log warning for developer

Attempt migration

Missing data should be visible during development.

---

# Save Migration Rules

When save structure changes, migration may be needed.

Migration should:

Read old save version

Convert old fields to new fields

Add missing default values

Remove deprecated values safely

Preserve player progress

Increase save version after successful migration

Example:

Old save has Skill XP.

New save has Profession XP.

Migration converts old skill IDs into profession IDs.

Migration should be tested carefully.

---

# Default Values

If a save field is missing, the game should use safe default values.

Examples:

Missing gold value becomes 0.

Missing inventory capacity becomes default capacity.

Missing profession XP becomes 0.

Missing companion rank becomes 1 if unlocked, or 0 if locked.

Missing settings use default settings.

Default values should avoid breaking player progress.

---

# New Game Save

Starting a new game should create default save data.

Default save may include:

Starting inventory capacity

Starting gold

Unlocked starting professions

Unlocked starting combat activity

Starting equipment

Starting companion if any

Tutorial state

Initial settings

Default values should be defined clearly.

---

# Reset Save Rules

The game may include a reset save option.

Reset save should require strong confirmation.

Examples:

Type DELETE

Hold button

Confirm twice

Reset should delete or archive old save.

During development, reset save may be easier to access.

In release, reset save should be protected.

---

# Multiple Save Slots

Current design:

Single save slot by default.

Future option:

Multiple save slots.

If multiple save slots are added, each slot should save:

Slot ID

Character Name

Play Time

Last Save Time

Progress Summary

Save Version

Multiple save slots are not required for v1.

---

# Cloud Save Future

Cloud save is a future system.

Cloud save may support:

Steam Cloud

Google Play cloud

Apple iCloud

Account-based sync

Cloud save should never replace local save safety.

Local save should still exist.

Cloud conflicts should be handled carefully.

Future cloud conflict options:

Use newest save

Use most progressed save

Ask player to choose

Keep both saves

---

# Anti-Cheat Philosophy

Current design:

Do not focus heavily on anti-cheat early.

This is mostly a single-player idle RPG.

Save data may be editable during development.

Future options:

Checksum

Light encryption

Cloud validation

Server-authoritative save

Anti-cheat should not slow down development too early.

---

# Debugging Tools

During development, the Save System should support debugging.

Useful debug tools:

Print save location

Open save folder

Force save

Force load

Delete save

Create test save

Add test items

Add XP

Complete companion timer

Simulate offline time

Validate save data

Debug tools should be disabled or hidden in release builds.

---

# Save Validation

The game should validate important save data after loading.

Validation should check:

Inventory quantities are not negative.

Levels are within allowed ranges.

XP is not below 0.

Companion ranks are within 1–20.

Combat Discipline levels are within 1–150.

Profession levels are within 1–100.

Equipped items exist.

Assigned companions exist.

Active crafting jobs are valid.

Temporary loot entries are valid.

Invalid data should be corrected safely when possible.

---

# Save Load Order

Systems should load in a safe order.

Recommended load order:

Load static databases.

Load save file.

Validate save version.

Run migrations if needed.

Load player data.

Load inventory data.

Load equipment data.

Load profession data.

Load Combat Discipline data.

Load companion data.

Load crafting data.

Load combat state.

Load achievements and collections.

Load statistics.

Load settings.

Calculate offline progress.

Update UI.

This prevents systems from referencing missing data.

---

# Offline Progress Connection

The Save System must support Offline Progress.

To do this, it must save:

Last save time

Last logout time

Current active profession activity

Current combat activity

Current crafting jobs

Assigned companions

Equipped items

Active buffs

Companion timers

Temporary loot if needed

Offline reward claim data

Offline Progress should be calculated only after save data loads safely.

Offline Progress should be defined in detail in the Offline Progress Framework.

---

# System Ownership

Each system should be responsible for creating its own save data section.

Examples:

Inventory System creates Inventory Save Data.

Companion System creates Companion Save Data.

Combat System creates Combat Save Data.

Profession System creates Profession Save Data.

Save Manager collects all save data into one file.

This keeps systems modular.

---

# Save Manager Responsibilities

The Save Manager should:

Create new save data

Load existing save data

Save current data

Autosave

Manual save

Backup save

Validate save data

Run migrations

Handle corrupted saves

Provide save events to other systems

Track save timestamps

The Save Manager should not contain gameplay logic.

Gameplay systems should provide their own save/load methods.

---

# Technical Rules

Save System should be data-driven where possible.

Save files should store IDs and player state, not full database definitions.

Save Manager should not hardcode every item, enemy, profession, or companion.

All save data classes should be serializable.

Save operations should be centralized.

Save/load should be safe if a system is temporarily missing data.

Do not overwrite a valid backup until the new save succeeds.

Save after important progression events.

Use clear debug logs during development.

---

# Future Expansion

Possible additions:

Cloud saves

Multiple save slots

Save import/export

Encrypted save files

Compressed save files

Save conflict resolution

Rollback saves

Character profiles

Seasonal save data

Account-wide save data

Server-based saves

Save analytics

Future save systems should expand the base Save System rather than replace it.

---

# Checklist

Every save-related feature should answer:

✓ What data needs to be saved?

✓ Which system owns this data?

✓ Does it reference IDs instead of duplicating database data?

✓ Does it save and load correctly?

✓ Does it work after closing the game?

✓ Does it work with offline progress?

✓ Does it need timestamp data?

✓ Does it need backup protection?

✓ Does it handle missing database entries?

✓ Does it handle future updates?

✓ Does it validate loaded data?

✓ Does it avoid silent progress loss?

✓ Can it support future expansion?