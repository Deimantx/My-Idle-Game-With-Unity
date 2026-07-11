# 03 Profession Framework

Version: 1.0
Status: Draft

---

# Purpose

This document defines the standard structure and rules that every non-combat profession in the game must follow.

Professions include systems such as Woodcutting, Mining, Fishing, Cooking, Smithing, Carpentry, Herblore, Farming, and other non-combat progression systems.

Every profession should follow this framework unless explicitly designed otherwise.

The purpose of this document is to ensure consistency, scalability, and maintainability across all current and future professions.

---

# Terminology

The game separates progression systems into different types.

Professions:

Woodcutting

Mining

Fishing

Cooking

Smithing

Carpentry

Herblore

Farming

Other non-combat systems

Professions have a maximum level of 100.

Combat Disciplines:

Warrior

Ranger

Mage

Combat Disciplines have a maximum level of 150 and are defined in the Combat Framework.

Combat Abilities:

Slash

Shield Block

Fireball

Power Shot

Other active or auto-used combat abilities

Combat Abilities are defined in the Combat Framework.

Do not use the word “skill” when referring to professions unless speaking generally.

---

# Design Goals

Every profession should:

- Have a clear purpose.
- Connect to at least one other system.
- Feel rewarding to level.
- Provide meaningful progression.
- Remain useful throughout the game.
- Support future expansions.
- Be enjoyable both online and offline.

---

# Profession Categories

Professions are divided into categories.

Current categories:

Gathering

Crafting

Future categories may be added without changing existing professions.

---

# Profession Structure

Every profession contains:

Profession Name

Description

Current Level

Current XP

Maximum Level

Available Activities

Unlock Requirements

Produced Resources

Consumed Resources

Equipment

Statistics

Achievements

Offline Support

Future Expansion Notes

---

# Profession Purpose

Every profession must answer:

Why does this profession exist?

What does it produce?

What other systems use its resources?

What makes this profession unique?

No profession should exist simply because another game has it.

---

# Activities

A profession can contain one or more activities.

Examples:

Woodcutting

• Cut Trees

Mining

• Mine Ore  
• Mine Gems

Fishing

• Catch Fish  
• Deep Sea Fishing

Cooking

• Cook Food  
• Prepare Buff Meals

Smithing

• Smelt Bars  
• Forge Equipment

Carpentry

• Process Logs  
• Craft Wooden Equipment

---

# Profession Progression

Players improve professions by performing activities.

Leveling should unlock:

New Activities

New Resources

Better Efficiency

New Equipment

New Recipes

New Areas

New Mechanics

Levels should unlock meaningful gameplay rather than only increasing numbers.

---

# Resources

Every profession should define:

Resources Produced

Resources Consumed

Resources Required

Resources Unlocked

Every resource should have a purpose somewhere else in the game.

Avoid creating resources that become permanently useless.

---

# Equipment

Each profession should define:

Required Equipment

Optional Equipment

Equipment Bonuses

Future Equipment

Equipment should improve efficiency rather than replace progression.

Examples:

Better axe speeds up Woodcutting.

Better pickaxe speeds up Mining.

Better fishing rod improves Fishing.

Better hammer improves Smithing.

Better saw improves Carpentry.

---

# Companion Interaction

Each profession should define whether companions interact with it.

Examples:

Increase XP

Increase Resource Gain

Reduce Action Time

Increase Rare Drop Chance

Unlock Special Activities

Some professions may not support companions.

---

# Buff Interaction

Professions may be affected by:

Food Buffs

Equipment Bonuses

Achievement Bonuses

Companion Bonuses

Future systems should easily integrate with professions.

---

# Statistics

Every profession should track:

Current Level

Current XP

Total XP Earned

Time Active

Actions Completed

Resources Produced

Resources Consumed

Rare Drops

---

# Standard Action Flow

Player selects activity

↓

Requirements checked

↓

Activity begins

↓

Timer or progress advances

↓

Rewards generated

↓

XP granted

↓

Statistics updated

↓

Achievements checked

↓

Repeat until stopped

---

# Failure Rules

By default, professions cannot fail.

Individual professions may introduce failure mechanics only if appropriate.

Failure should not feel random or unfair.

Examples of possible future failure systems:

High-risk crafting

Thieving

Dangerous gathering areas

Unstable magical crafting

Combat failure is handled separately in the Combat Framework.

---

# Offline Progress

Every profession must define:

Can it run offline?

Can rewards be earned offline?

Can achievements progress offline?

Can statistics progress offline?

Offline progression should behave consistently unless a profession specifically overrides it.

---

# User Interface

Every profession screen should contain:

Profession Icon

Profession Name

Profession Level

Experience Bar

Activity Selection

Activity Information

Requirements

Rewards

Current Equipment

Companion (if supported)

Statistics Button

Start / Stop Activity

---

# Unlock Rules

Activities may require:

Profession Level

Equipment

Completed Activity

Completed Dungeon

Region Unlock

Requirements should be clearly visible to the player.

---

# Balance Philosophy

Every profession should:

Reward player time.

Offer meaningful upgrades.

Avoid becoming obsolete.

Remain valuable throughout progression.

Provide resources useful to other systems.

No profession should dominate every aspect of progression.

---

# Technical Rules

Every profession should be data-driven.

Gameplay values should not be hardcoded.

Activities should be defined through data.

Resources should use the Item Database.

Equipment should use the Equipment Database.

Profession progression should use the Experience System.

Statistics should automatically integrate with the Statistics System.

Achievements should automatically integrate with the Achievement System.

Offline progression should integrate with the Offline Progress System.

---

# Activity Structure

Every activity inside a profession should define:

Activity Name

Required Level

Required Equipment

Duration / Action Speed

XP Reward

Resource Rewards

Rare Rewards

Special Mechanics (optional)

Future Expansion Notes

---

# Profession States

Every profession can exist in one of the following states:

Locked

Available

Active

Paused

Completed (activity only)

Unavailable (requirements not met)

Offline Active

---

# Future Expansion

Professions should support future additions without redesign.

Possible additions:

Specializations

Profession Prestige

Elite Activities

Legendary Resources

Seasonal Activities

Mini-Games

World Events

New Equipment

New Companion Interactions

New Regions

---

# Checklist

Every new profession should answer the following:

✓ What is the purpose of the profession?

✓ What category does it belong to?

✓ What activities does it contain?

✓ What resources does it produce?

✓ What resources does it consume?

✓ What equipment affects it?

✓ How does it level?

✓ What unlocks does it have?

✓ How does it interact with companions?

✓ Which buffs affect it?

✓ Can it run offline?

✓ Which statistics are tracked?

✓ Which achievements use this profession?

✓ How can this profession expand in future updates?