# 02 Experience System

Version: 1.0
Status: Draft

---

# Purpose

This document defines how experience (XP), levels, Combat Discipline progression, Companion ranks, and future account progression function throughout the game.

Every progression system should follow these rules unless explicitly stated otherwise.

---

# Design Goals

The experience system should:

- Always reward the player.
- Make every action feel meaningful.
- Provide frequent progression early game.
- Provide long-term progression in late game.
- Avoid feeling grindy without purpose.
- Support future content without redesign.

---

# Progression Types

The game currently contains multiple progression systems.

Current progression types:

Professions

Combat Disciplines

Companion Ranks

Account Progression (future)

Each progression type has its own rules.

---

# Profession Experience

Professions are non-combat progression systems.

Examples:

Woodcutting

Mining

Fishing

Cooking

Smithing

Carpentry

Herblore

Farming

Foraging

Thieving

Other non-combat systems

Each profession has its own XP and level.

Every profession levels independently.

Professions have a maximum level of 100.

Profession XP is gained by performing profession activities.

Examples:

Cutting trees

Mining ore

Fishing

Cooking food

Crafting items

Gathering resources

Processing materials

---

# Combat Discipline Experience

Combat has separate progression from professions.

Combat Disciplines:

Warrior

Ranger

Mage

Combat Disciplines have a maximum level of 150.

Combat Discipline XP is gained by dealing damage with the matching combat style.

Warrior XP comes from melee damage.

Ranger XP comes from ranged damage.

Mage XP comes from magic or elemental damage.

Combat Discipline levels may unlock:

Weapons

Armor

Combat abilities

Passive bonuses

Combat requirements

Dungeon requirements

Boss requirements

Style-specific progression

Combat Discipline progression is defined in more detail inside the Combat Framework.

---

# Companion Damage and Experience

Combat companions may deal damage during combat.

Companion damage helps defeat enemies and earn loot.

By default, companion damage does not grant Warrior, Ranger, or Mage XP.

Combat Discipline XP should come from player damage.

Special companion effects may allow limited XP sharing if explicitly defined.

---

# Companion Progression

Companions do not gain XP from actions.

Companions do not have normal levels.

Companions use ranks instead.

Current Companion Rank range:

Rank 1–20

Companion ranks progress by giving the companion required items and waiting for the required time to pass.

Examples:

Combat companions may require combat gear.

Gathering companions may require gathered resources.

Crafting companions may require crafted items.

Rare companions may require boss drops or special materials.

Companion progression is defined in more detail inside the Companion Framework.

---

# Account Progression

Account Progression is a future system.

It represents overall account growth.

Possible uses:

Achievements

Unlocks

Future prestige systems

Season rewards

Account rank

Global bonuses

Account Progression should not replace profession or Combat Discipline progression.

---

# Level Structure

Every profession and Combat Discipline should define:

Current Level

Current XP

XP Required for Next Level

Maximum Level

Statistics

Unlocks

---

# Maximum Levels

Current maximum profession level:

100

Current maximum Combat Discipline level:

150

Current maximum Companion Rank:

20

Future expansion may increase these caps if needed.

---

# XP Sources

Experience should always come from performing activities.

Profession XP sources:

Cutting trees

Mining ore

Fishing

Cooking

Crafting items

Gathering resources

Processing materials

Combat Discipline XP sources:

Dealing melee damage

Dealing ranged damage

Dealing magic or elemental damage

Using combat abilities

Damage over time effects

Weapon effects

Companion rank progress does not use XP.

---

# XP Rewards

Every successful profession action grants XP.

Combat Discipline XP is granted from damage dealt.

The amount of XP depends on:

Activity duration

Activity difficulty

Resource rarity

Enemy difficulty

Preparation required

Future modifiers

Longer or more difficult activities should generally reward more XP, but all activities should remain useful depending on the player's goal.

---

# XP Curve

Early game:

Fast progression.

Frequent level ups.

Many early unlocks.

Mid game:

Longer levels.

More meaningful unlock decisions.

Systems begin interacting more.

Late game:

Levels become progressively longer.

High-level achievements should feel prestigious.

Exact XP formulas will be defined in the Balancing documents.

Do not hardcode XP values.

---

# Level Ups

When leveling:

Increase level.

Unlock new content.

Play level-up animation.

Update statistics.

Check achievements.

Refresh UI.

Save progress.

Level ups should feel rewarding and should clearly show what was unlocked.

---

# Unlock Philosophy

Levels should unlock meaningful content.

Examples:

New trees

New ore

New fish

New equipment

New recipes

New regions

New monsters

New mechanics

New combat abilities

New passive bonuses

Avoid unlocks that only increase numbers.

---

# Experience Modifiers

Future systems may modify XP.

Examples:

Equipment

Companions

Food buffs

Temporary buffs

Area bonuses

Achievements

Relics

Events

Modifiers should multiply or add to XP after the base reward is calculated.

Modifier rules should be clear and predictable.

---

# Level Requirements

Activities may require:

Profession level

Combat Discipline level

Required equipment

Unlocked area

Completed dungeon

Specific item

Future achievement

A player must satisfy all requirements before beginning an activity.

Requirements should be clearly visible in the UI.

---

# Experience Overflow

XP should never be lost.

If the player reaches the current level cap:

Continue storing XP internally.

Future updates can convert overflow XP into new levels if the cap increases.

Overflow XP may also support future systems such as prestige, paragon, or account progression.

---

# Offline Experience

Offline progress grants XP exactly like online progress unless a system explicitly overrides it.

Offline XP should:

Increase profession XP

Increase Combat Discipline XP if offline combat is allowed

Unlock levels

Trigger achievements

Record statistics

Offline progress should never feel like a punishment.

---

# Statistics

Every profession and Combat Discipline should track:

Current Level

Current XP

Total XP Earned

Highest Level

Time Trained

Actions Completed

Level Ups

Resources Produced

Rare Drops

Combat Disciplines should also track:

Damage Dealt

Enemies Killed

Bosses Killed

Deaths

Combat Time

---

# Visual Feedback

The player should clearly see progression.

Examples:

XP popups

Floating XP numbers

Level-up animation

Progress bars

Unlock notifications

Achievement notifications

Next unlock preview

---

# XP Display Rules

Every profession and Combat Discipline should always display:

Current Level

Current XP

XP Required for Next Level

XP Progress Bar

XP Gained per Hour

Estimated Time to Next Level

Next Unlock

Players should never wonder whether they are making progress.

---

# XP Balancing Philosophy

XP rewards should reflect:

Activity duration

Activity difficulty

Resource rarity

Enemy difficulty

Preparation required

Risk

Longer or more difficult activities should generally reward more XP.

However, all activities should remain viable depending on the player's goals.

Avoid creating one activity that is always the best XP option for every situation.

---

# Reserved Systems

Possible future additions:

Prestige Levels

Paragon Levels

Seasonal XP

Guild Experience

Faction Reputation

Research Experience

Account Rank

Infinite Progression

Reserved systems should not be implemented until the base progression systems are stable.

---

# Technical Rules

XP values should never be hardcoded inside gameplay scripts.

Every activity should define its XP reward through data.

Progression should be data-driven.

Profession levels, Combat Discipline levels, Companion ranks, and Account Progression should remain separate systems.

All XP should be saved and restored by the Save System.

Experience calculations should support future modifiers without requiring major refactoring.

Combat Discipline XP allocation should follow the Combat Framework.

Companion rank progression should follow the Companion Framework.

XP formulas should be easy to adjust in balancing documents.