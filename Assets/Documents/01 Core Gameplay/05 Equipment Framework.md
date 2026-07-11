# 05 Equipment Framework

Version: 1.0
Status: Draft

---

# Purpose

This document defines how equipment works across the entire game.

Equipment includes combat gear, profession tools, accessories, relics, and future special items that improve player progression.

Every equipment item should follow this framework unless explicitly designed otherwise.

---

# Design Goals

Equipment should:

- Make the player stronger or more efficient.
- Support both combat and non-combat professions.
- Create meaningful upgrade goals.
- Connect gathering, crafting, combat, and economy systems.
- Be data-driven.
- Avoid becoming useless too quickly.
- Give players long-term progression goals.

Equipment should never exist only for filler.

---

# Equipment Categories

Current equipment categories:

Weapons

Armor

Tools

Accessories

Relics

Future categories may be added later.

---

# Equipment Types

Examples:

Sword

Axe

Pickaxe

Bow

Staff

Fishing Rod

Hammer

Saw

Armor

Helmet

Gloves

Boots

Ring

Amulet

Cape

Relic

---

# Equipment Structure

Every equipment item contains:

Equipment ID

Name

Description

Icon

Category

Type

Equipment Slot

Required Level

Required Profession

Required Combat Discipline

Stats

Bonuses

Sell Value

Crafting Recipe

Upgrade Path

Can Be Equipped

Can Be Upgraded

Can Be Enchanted

Future Expansion Notes

---

# Equipment Slots

Current equipment slots:

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

Future slots may be added later.

---

# Shield Rules

The shield slot can only be used when the equipped weapon allows it.

When a two-handed weapon is equipped:

- Shield slot becomes disabled.
- Shield bonuses are not applied.
- The player should clearly see why the shield slot is unavailable.

Two-handed equipment should usually provide stronger offensive or utility bonuses to compensate for losing the shield slot.

---

# Combat Equipment

Combat equipment improves performance in combat.

Possible combat stats:

Attack Damage

Attack Speed

Accuracy

Critical Chance

Critical Damage

Defense

Health

Health Regeneration

Healing Received %

Damage Reduction

Elemental Damage

Elemental Resistance

Combat XP Bonus

Combat equipment should matter in all combat encounters.

---

# Profession Equipment

Profession equipment improves non-combat activities.

Examples:

Axes improve Woodcutting.

Pickaxes improve Mining.

Fishing Rods improve Fishing.

Knives improve Leatherworking.

Needles improve Tailoring.

Hammers improve Smithing.

Saws improve Carpentry.

Possible profession stats:

Action Speed

Resource Gain

XP Gain

Rare Drop Chance

Reduced Resource Cost

Bonus Output Chance

Profession equipment should improve efficiency, not completely replace profession levels.

---

# Accessories

Accessories provide flexible bonuses.

Examples:

Rings

Amulets

Capes

Possible bonuses:

Combat bonuses

Profession bonuses

XP bonuses

Resource bonuses

Food buff duration

Companion bonuses

Accessories should allow players to customize their build.

---

# Relics

Relics are special long-term progression equipment.

Relics can be activated for a duration and then go on cooldown.

Relics may provide:

Passive account bonuses

Profession bonuses

Combat bonuses

Unique effects

Relics should be hard to obtain, hard to upgrade, and meaningful.

---

# Active Relic Rules

Relics may provide active effects.

Every active relic should define:

Activation Effect

Duration

Cooldown

Requirement

Target System

Relics should not require constant clicking.

Active relics should feel like important strategic tools rather than mandatory spam buttons.

---

# Equipment Requirements

Equipment may require:

Player Level

Profession Level

Combat Discipline Level

Specific Profession Level

Specific Combat Discipline Level

Completed Dungeon

Unlocked Region

Crafting Requirement

Future Achievement

Requirements should always be visible before the player obtains or equips the item.

---

# Equipment Sources

Equipment may come from:

Crafting

Combat Drops

Boss Drops

Dungeon Rewards

Shops

Achievements

Future Systems

---

# Crafting Equipment

Equipment should help connect professions, combat, and progression.

Examples:

Mining

↓

Smithing

↓

Pickaxe

↓

Mining Efficiency

Woodcutting

↓

Carpentry

↓

Bow

↓

Combat

Hunting

↓

Leatherworking

↓

Armor

↓

Combat

Jewelcrafting

↓

Ring

↓

Passive Bonuses

Enchanting

↓

Equipment Bonuses

---

# Upgrade Paths

Equipment may have upgrade paths.

Example:

Runic Axe

↓

Rune Blackshard

Dropped from boss.

↓

Black Runic Axe

Upgrade paths should give players clear long-term goals.

Upgrades may require:

Base Equipment

Crafting Materials

Gold

Rare Drops

Profession Level

Combat Discipline Level

Dungeon Materials

Future Special Materials

---

# Equipment Bonuses

Equipment bonuses may affect:

Combat Damage

Combat Defense

Profession Action Speed

XP Gain

Resource Gain

Rare Drop Chance

Food Buff Strength

Food Buff Duration

Companion Effects

Unlock Requirements

Bonuses should be easy to understand.

Avoid overly complicated stat descriptions.

---

# Bonus Stacking Rules

Equipment bonuses should stack in a clear and predictable way.

Default rule:

Flat bonuses apply first.

Percentage bonuses apply after flat bonuses.

Temporary buffs apply after equipment bonuses.

Special unique effects apply last.

If multiple items give the same bonus, the UI should clearly show the total combined effect.

---

# Equipment Balance Philosophy

Equipment should improve progression without making older systems irrelevant.

Better equipment should feel exciting.

However, equipment should not remove the value of:

Profession Levels

Combat Discipline Levels

Resources

Companions

Food Buffs

Achievements

Player Decisions

Equipment should support progression, not replace it.

---

# Durability

Current design:

No durability by default.

Future option:

Durability may be added for special systems if needed.

Do not add durability unless it improves gameplay.

Avoid durability systems that feel like annoying maintenance.

---

# Enchanting

Enchanting is a future system.

Equipment may support enchantments later.

Possible enchantment effects:

More damage

More defense

More XP

Faster profession actions

Extra resources

Rare drop chance

Special passive effects

Enchanting should expand equipment, not replace normal progression.

---

# Equipment and Companions

Companions may interact with equipment.

Possible examples:

Companion requires specific gear to rank up.

Combat companions request combat gear as rank-up materials.

Profession companions request profession tools or resources as rank-up materials.

---

# Equipment and Offline Progress

Equipment bonuses should apply to offline progress unless specifically disabled.

Examples:

Axe speed should improve offline Woodcutting.

Pickaxe bonuses should improve offline Mining.

Combat gear should improve offline combat if offline combat is allowed.

Offline calculations should use currently equipped gear from the save file.

---

# Equipment Statistics

The game should track:

Times Equipped

Times Crafted

Times Upgraded

Times Sold

Highest Equipment Tier Owned

First Time Obtained

Total Resources Spent on Equipment

Combat Damage Done With Equipment

Resources Gathered With Tool

---

# Equipment User Interface

Equipment UI should display:

Icon

Name

Description

Slot

Type

Required Level

Required Profession

Required Combat Discipline

Stats

Bonuses

Source

Sell Value

Crafting Recipe

Upgrade Path

Equip Button

Upgrade Button

Compare With Current Equipment

The player should easily understand whether an item is better than currently equipped gear.

---

# Equipment Comparison

When viewing equipment, the game should compare it to currently equipped equipment.

Show:

Stat increases

Stat decreases

New bonuses

Lost bonuses

Requirement problems

Comparison should be clear and simple.

---

# Equipment Database

Every equipment item should exist inside the Equipment Database.

Equipment should never be hardcoded.

Systems should reference equipment IDs.

Equipment should connect to:

Item Database

Profession System

Combat System

Crafting System

Inventory System

Save System

Offline Progress System

---

# Technical Rules

Equipment should be data-driven.

Stats should be editable without code changes.

Bonuses should be modular.

Equipment should not duplicate item data.

Equipped items should save and load correctly.

Every equipment item should also exist as an item in the Item Database.

Combat and profession systems should read bonuses from equipped items.

---

# Future Expansion

Possible additions:

Equipment Sets

Enchantments

Item Quality

Unique Effects

Legendary Equipment

Relics

Transmog

Socketed Gems

Special Boss Gear

Companion Equipment

Endgame Upgrade Paths

---

# Checklist

Every new equipment item should answer:

✓ What category is this equipment?

✓ What type is this equipment?

✓ Which slot does it use?

✓ What are the requirements?

✓ Where does it come from?

✓ Can it be crafted?

✓ Can it be upgraded?

✓ What stats does it give?

✓ What bonuses does it give?

✓ Which profession, Combat Discipline, or system uses it?

✓ Does it remain useful later?

✓ Does it connect to other systems?

✓ Can it support future expansion?