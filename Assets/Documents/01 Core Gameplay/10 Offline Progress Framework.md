# 10 Offline Progress Framework

Version: 1.0
Status: Draft

---

# Purpose

This document defines how offline progress works across the entire game.

Offline Progress allows the player to continue making progress while the game is closed.

Offline Progress should support professions, combat, crafting, companion rank-up timers, consumables, inventory, achievements, statistics, and future systems.

Offline Progress is one of the most important systems in an idle RPG.

---

# Design Goals

Offline Progress should:

- Respect the player's active activity.
- Reward the player for time away.
- Feel fair and useful.
- Avoid punishing the player for closing the game.
- Connect to professions, combat, crafting, companions, inventory, and achievements.
- Be easy to understand from a return summary.
- Be reliable and safe.
- Use saved data correctly.
- Avoid creating exploits where possible.
- Be expandable for future systems.

Offline Progress should feel like the game continued naturally while the player was away.

---

# Offline Progress Philosophy

The game should respect the player's time.

If the player starts an activity and closes the game, the game should calculate what would have happened during that time.

The player should return to a clear summary of progress.

Offline Progress should not feel better than active play in every situation.

Active play may still be better for:

Manual boss encounters

Dungeon decisions

Changing activities

Managing inventory

Using relics at the right time

Reacting to dangerous combat mechanics

Offline Progress should be strong enough to feel rewarding, but not so strong that active play feels useless.

---

# Offline Progress Requirements

Offline Progress requires saved data.

The Save System must provide:

Last Save Time

Last Logout Time

Last Login Time

Current active profession activity

Current combat activity

Current crafting jobs

Assigned companions

Equipped items

Active buffs

Inventory state

Temporary Combat Loot

Companion timers

Offline Reward Claim data

Offline Progress should only calculate after the save file loads safely.

---

# Offline Progress Flow

When the player returns:

Load save file

↓

Validate save data

↓

Calculate time away

↓

Check active activities

↓

Calculate profession progress

↓

Calculate combat progress if allowed

↓

Calculate crafting progress

↓

Calculate companion timers

↓

Generate rewards

↓

Check inventory capacity

↓

Send overflow rewards to claim screen if needed

↓

Update XP, items, statistics, achievements, and collections

↓

Show Offline Progress Summary

↓

Player claims rewards

---

# Time Away Calculation

Offline Progress should calculate how long the player was away.

Time away is based on saved timestamps.

Important timestamps:

Last Save Time

Last Logout Time

Last Login Time

Last Offline Progress Calculation Time

The game should calculate:

Offline Time = Current Time - Last Offline Progress Calculation Time

Offline Time should never be negative.

If offline time is negative, the game should treat it as 0 or handle it safely.

---

# Offline Time Limits

Current design:

Offline Progress should have a maximum time limit.

Recommended starting limit:

12 hours

Future upgrades may increase this limit.

Possible upgrades:

Achievements

Companions

Relics

Account progression

Shop upgrades

Late-game unlocks

Example progression:

12 hours base

18 hours upgrade

24 hours upgrade

36 hours upgrade

48 hours upgrade

Offline time limits prevent extreme reward spikes and make balancing easier.

---

# Minimum Offline Time

Very short offline time may not need a full summary.

Recommended minimum:

1 minute

If the player was away for less than the minimum time, the game may skip the Offline Progress Summary.

Progress may still be calculated silently if needed.

---

# Offline Progress Summary

When the player returns, show a summary.

The summary should display:

Time away

Active profession progress

Combat results

Crafting completed

Companion rank-up timers completed

Items gained

XP gained

Gold gained

Rare drops found

First-time items found

Level ups

Achievements completed

Items consumed

Inventory overflow items

Deaths if offline combat occurred

The player should clearly understand what happened while they were away.

---

# Profession Offline Progress

Professions may continue offline if the player was actively doing a profession activity before leaving.

Examples:

Woodcutting continues cutting selected tree.

Mining continues mining selected rock.

Fishing continues catching selected fish.

Foraging continues gathering selected materials.

Crafting professions may continue if they use timed crafting jobs.

Profession offline progress should use:

Selected activity

Profession level

Profession XP

Equipped tool

Assigned profession companion

Active food buff if still valid

Active relic if still valid

Action time

Resource rewards

Rare drop chance

Inventory capacity

Profession progress should stop if inventory becomes full.

---

# Profession Offline Calculation

Profession offline calculation should estimate how many actions were completed.

Example:

Offline Time

÷

Action Time

=

Completed Actions

Completed Actions generate:

Profession XP

Resources

Rare drops

Statistics

Achievement progress

Collection discoveries

Exact formulas should be handled in balancing documents.

Do not hardcode final balancing numbers inside unrelated scripts.

---

# Profession Offline Inventory Rules

Profession rewards should go directly into inventory.

If inventory becomes full:

Profession offline progress should stop.

The Offline Progress Summary should show:

How long the profession worked

When inventory became full

Items gained before stopping

Items that could not be gained if relevant

Important resources should not be silently deleted.

Profession activity should not continue generating rewards when there is no inventory space.

---

# Combat Offline Progress

Combat may continue offline if the player was already in combat before leaving.

Offline combat should only apply to content that allows it.

Allowed by default:

Regular Monster Combat

Some Elite Monster Combat

Some repeatable dungeons after they become safe

Disabled by default:

Hard bosses

Manual-only encounters

Raids

Arena

Challenge encounters

Dangerous first-time dungeon attempts

Each combat activity should define whether offline combat is allowed.

---

# Offline Combat Requirements

Offline combat should use saved combat setup.

Offline combat should respect:

Equipped gear

Selected Combat Abilities

Auto-use ability settings

Assigned combat companion

Companion rank

Active food buff

Active elixirs

Available potion

Relic effects if active

Enemy stats

Combat mode

Player health

Player devotion

Offline combat should not allow the player to change setup during the calculation.

---

# Offline Combat Calculation

Offline combat should calculate:

Player damage

Companion damage

Enemy damage

Kills

Deaths

Loot gained

Combat Discipline XP from player damage

Food duration used

Elixir duration used

Potion usage

Relic duration used

Rare drops

Statistics

Achievements

Companion damage should increase kill speed.

Companion damage should not grant Combat Discipline XP unless a special companion effect explicitly allows it.

---

# Offline Combat Death Rules

If the player dies during offline combat:

Combat stops.

The player keeps XP earned from player damage before death.

The player keeps loot already earned before death.

Consumed food, potions, and elixir duration may be lost.

The Offline Progress Summary should show:

Enemy fought

Time survived

Kills before death

XP gained

Loot gained

Reason combat stopped

Death should not feel overly punishing.

---

# Offline Combat Loot Rules

Offline combat loot should go into inventory when possible.

If inventory is full:

Loot should go to Offline Reward Claim storage when possible.

Important loot should be protected from silent loss.

Important loot includes:

Unique items

Boss drops

Dungeon rewards

Rare progression materials

Relics

First-time collection items

Companion rank materials

Common low-value drops may be lost if no storage space exists, but the UI should explain this clearly.

---

# Offline Combat XP Rules

Combat Discipline XP is gained from player damage.

Player melee damage grants Warrior XP.

Player ranged damage grants Ranger XP.

Player magic or elemental damage grants Mage XP.

Companion damage does not grant Combat Discipline XP by default.

XP from a single enemy cannot exceed the enemy's maximum XP value.

Damage beyond enemy maximum health does not grant extra XP.

---

# Crafting Offline Progress

Crafting jobs may continue offline.

Crafting offline progress should use:

Crafting Recipe ID

Crafting start time

Crafting end time

Quantity being crafted

Crafting profession

Required materials

Required gold

Crafting queue if future system uses it

Current rule:

Crafting materials are consumed when crafting completes.

The Save System and Crafting System must follow the same rule.

---

# Crafting Offline Completion

When crafting completes offline:

Check required materials

Check required gold

Check inventory space

Create crafted item

Grant profession XP if applicable

Update statistics

Update achievements

Update collection discoveries

If required materials are missing, crafting should fail safely.

If inventory is full, crafted items should go to Offline Reward Claim storage when possible.

Crafted items should not be silently deleted.

---

# Companion Offline Progress

Companion rank-up timers continue while offline.

Offline companion progress should use:

Companion ID

Current rank

Target rank

Rank-up start time

Rank-up end time

Timer active status

When the player returns:

Check completed timers

Mark rank-up as complete

Allow the player to claim the rank-up

Update companion rank

Update statistics

Check achievements

Companion rank-up timers should not require the game to stay open.

---

# Companion Offline Bonuses

Assigned companions may improve offline progress.

Profession companions may affect:

Profession XP

Action speed

Resource gain

Rare drop chance

Offline profession progress

Combat companions may affect:

Companion damage

Player damage bonuses

Player defense bonuses

Rare drop chance

Offline combat speed

Utility companions may affect:

Offline time limit

Offline reward amount

Offline crafting speed

Offline rank-up time

Companion bonuses should be applied consistently online and offline.

---

# Buffs During Offline Progress

Temporary buffs may continue ticking down while offline.

Buff types:

Food buffs

Elixirs

Relic effects

Future temporary buffs

If a buff expires during offline time, only the valid portion should apply.

Example:

Player leaves for 2 hours.

Food buff lasts 5 minutes.

Only first 5 minutes of offline progress receives food buff.

Buff expiration should be shown in the Offline Progress Summary when relevant.

---

# Consumables During Offline Combat

Offline combat may consume potions if auto-use settings allow it.

Potion usage should follow combat rules.

Offline combat should track:

Potions used

Potion quantity consumed

Health restored

Time survived because of potion usage

If potions run out, combat continues only if the player can survive.

Food and elixirs should use their duration.

They should not be consumed repeatedly offline unless auto-reuse is specifically enabled.

---

# Relics During Offline Progress

Relics may affect offline progress if active when the player leaves.

Relic effects should respect:

Activation time

Duration

Cooldown

Target system

If the relic expires during offline time, only the valid portion should apply.

Relic cooldown should continue while offline.

Relics should not be automatically activated offline unless a future system explicitly allows it.

---

# Inventory During Offline Progress

Offline rewards should be added to inventory safely.

If inventory has space:

Add rewards to inventory.

If inventory is full:

Use Offline Reward Claim storage when possible.

If claim storage is unavailable:

Protect important items where possible.

Show clear warning in summary.

Inventory should not silently delete important items.

---

# Offline Reward Claim Storage

Offline Reward Claim storage is temporary protected storage for offline rewards.

It may be used when:

Inventory is full

Offline rewards contain important items

Offline rewards need player confirmation

Offline rewards are too large to add immediately

Offline Reward Claim storage should save and load.

The player should be able to claim rewards after freeing inventory space.

Offline Reward Claim storage should not become a permanent second inventory.

---

# Offline Reward Claim Rules

The player may claim rewards from the Offline Reward Claim screen.

Claim rules:

Check inventory space.

Add selected rewards.

Allow Claim All when possible.

Keep unclaimed rewards stored safely.

Do not delete unclaimed important rewards.

Show required free slots if inventory space is missing.

The player should clearly understand what still needs to be claimed.

---

# Achievements During Offline Progress

Offline progress may complete achievements.

Achievement checks may include:

Profession XP gained

Profession actions completed

Combat kills

Boss kills if allowed offline

Items obtained

Rare drops found

Crafting completed

Companion rank-ups completed

Offline time accumulated

Achievements completed offline should appear in the Offline Progress Summary.

---

# Collection During Offline Progress

Offline progress may discover new items.

When a new item is generated offline:

Mark it as discovered.

Update collection log.

Show first-time discovery in Offline Progress Summary.

Collection discovery should remain even if the item is later sold, consumed, or destroyed.

---

# Statistics During Offline Progress

Offline progress should update statistics.

Possible statistics:

Total offline time

Total offline sessions

Offline profession actions

Offline combat kills

Offline deaths

Offline items gained

Offline XP gained

Offline gold gained

Offline rare drops

Offline crafting completed

Offline companion rank-ups completed

Offline rewards lost due to full storage

Offline rewards protected from loss

Statistics should help balance and debugging.

---

# Offline Progress Limits

Offline Progress should have limits for balance.

Possible limits:

Maximum offline time

Inventory capacity

Combat death risk

Consumable availability

Buff duration

Activity requirements

Offline-disabled encounters

Companion assignment requirements

These limits should be clear to the player.

---

# Offline Disabled Activities

Some activities may not support offline progress.

Possible offline-disabled content:

Manual-only bosses

Arena fights

Raids

Special challenge encounters

Puzzle mechanics

First-time dangerous dungeon attempts

Future active events

Disabled activities should clearly explain why offline progress is not allowed.

---

# Offline Progress Abuse Prevention

The game should avoid obvious exploits.

Possible rules:

Offline time cannot be negative.

Offline time has a maximum cap.

Offline progress should use saved activity state.

Offline progress should not allow changing gear after returning before calculation.

Offline progress should not duplicate rewards.

Offline rewards should be claimed once.

Offline timers should save properly.

Do not over-focus on anti-cheat early.

This is mostly a single-player idle RPG.

---

# Offline Progress User Interface

Offline Progress Summary should display:

Time away

Activity performed

Profession XP gained

Combat Discipline XP gained

Items gained

Gold gained

Crafting completed

Companion timers completed

Level ups

Rare drops

First-time discoveries

Deaths

Inventory full warnings

Rewards waiting to be claimed

The summary should be readable and exciting.

---

# Offline Progress UI Rules

The summary should not overwhelm the player.

Important events should be highlighted.

Examples:

Level ups

Rare drops

New items

Boss kills

Companion rank-ups

Inventory full warnings

The player should be able to expand details if they want.

Simple summary first.

Detailed breakdown second.

---

# Offline Progress Notifications

The game may show notifications for important offline results.

Examples:

Profession leveled up

Combat Discipline leveled up

Rare item found

Companion rank-up completed

Crafting completed

Inventory became full

Notifications should be useful but not annoying.

---

# Offline Progress Save Rules

Offline progress should save after calculation.

After offline progress is calculated:

Update inventory

Update XP

Update companion timers

Update crafting jobs

Update combat state

Update achievements

Update collections

Update statistics

Update offline reward claim storage

Save immediately

This prevents losing offline rewards if the game closes after returning.

---

# Failure Handling

Offline Progress should fail safely.

If calculation fails:

Do not corrupt save.

Do not delete rewards.

Log error for developer.

Show safe message if needed.

Try to preserve player progress.

If one system fails, other systems should still load when possible.

---

# Technical Rules

Offline Progress should be data-driven where possible.

Activities should define whether offline progress is allowed.

Combat encounters should define whether offline combat is allowed.

Profession activities should define offline action rules.

Companions should define offline bonuses.

Buffs should define whether they apply offline.

Offline Progress should reference IDs instead of duplicating database data.

Offline Progress should not hardcode item, enemy, profession, or companion data.

---

# System Connections

Offline Progress connects to:

Save System

Inventory System

Item Database

Profession System

Combat System

Combat Discipline System

Equipment System

Companion System

Crafting System

Achievement System

Collection System

Statistics System

UI System

Offline Progress should be treated as a central system.

---

# Future Expansion

Possible additions:

Offline progress upgrades

Offline progress talents

Offline efficiency bonuses

Offline-only companions

Offline crafting queues

Offline expedition system

Offline dungeon farming unlocks

Offline loot filters

Offline reward multipliers

Cloud save offline sync

Account-wide offline bonuses

Future systems should expand the base Offline Progress Framework rather than replace it.

---

# Checklist

Every offline feature should answer:

✓ Can this activity progress offline?

✓ What data must be saved?

✓ What rewards are generated?

✓ What XP is generated?

✓ What items are consumed?

✓ What happens if inventory is full?

✓ Can rewards go to claim storage?

✓ Do companions affect it?

✓ Does equipment affect it?

✓ Do buffs affect it?

✓ Can the player die offline?

✓ Does it update achievements?

✓ Does it update collections?

✓ Does it update statistics?

✓ Does it save after calculation?

✓ Is it easy for the player to understand?

✓ Can it support future expansion?