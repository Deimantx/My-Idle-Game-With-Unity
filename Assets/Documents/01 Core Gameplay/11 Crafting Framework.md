# 11 Crafting Framework

Version: 1.0
Status: Draft

---

# Purpose

This document defines how crafting works across the entire game.

Crafting converts resources into useful items, equipment, consumables, upgrade materials, companion materials, and future progression items.

Crafting should connect professions, inventory, combat, equipment, companions, shops, achievements, collections, and future systems.

Crafting should never exist as an isolated system.

---

# Design Goals

Crafting should:

- Give gathered resources meaningful uses.
- Create long-term equipment goals.
- Support combat progression.
- Support profession progression.
- Support companion progression.
- Keep older resources useful when possible.
- Provide clear recipes and requirements.
- Be easy to understand.
- Be data-driven.
- Support offline progress.
- Avoid useless filler recipes.

Crafting should make the player feel like every resource has a purpose.

---

# Crafting Philosophy

Crafting is the bridge between gathering, combat, and progression.

Gathering professions create raw resources.

Crafting professions turn those resources into useful items.

Combat may provide rare materials that improve crafted gear.

Companions may require crafted items to rank up.

The economy should feel connected.

Example:

Mining

↓

Ore

↓

Smithing

↓

Sword

↓

Combat

↓

Boss Material

↓

Upgrade Sword

↓

Stronger Combat

---

# Crafting Professions

Crafting is split across crafting professions.

Examples:

Cooking

Smithing

Carpentry

Herblore

Tailoring

Leatherworking

Jewelcrafting

Enchanting

Runecrafting

Each crafting profession should have its own identity and purpose.

---

# Crafting Profession Roles

Each crafting profession should support different systems.

Cooking:

Creates food buffs.

Supports combat and professions.

Smithing:

Creates bars, weapons, armor, tools, and upgrade materials.

Supports combat and gathering + crafting professions.

Carpentry:

Creates bows, wooden tools, traps for hunting, planks, furniture, and future building materials.

Supports combat, Woodcutting, Hunting and future systems.

Herblore:

Creates potions, elixirs, oils, and future alchemy items.

Supports combat.

Tailoring:

Creates cloth armor, bags, robes, and future utility items.

Supports Mage gear and inventory expansion.

Leatherworking:

Creates leather armor, ranged gear, gloves, boots, and utility gear.

Supports Ranger gear and combat.

Jewelcrafting:

Creates rings, amulets, gems, for passive bonuses.

Supports combat and professions.

Enchanting:

Improves equipment with magical bonuses.

Supports long-term gear progression.

Runecrafting:

Creates runes, magical materials.

Supports all Combat Disciplines.


---

# Crafting Categories

Crafted items may include:

Equipment

Tools

Weapons

Armor

Accessories

Relics

Consumables

Food

Potions

Elixirs

Crafting Materials

Upgrade Materials

Companion Rank Materials

Unlock Items

Future Progression Items

---

# Crafting Recipe Structure

Every crafting recipe should define:

Recipe ID

Recipe Name

Crafting Profession

Required Profession Level

Required Materials

Required Quantities

Required Gold if applicable

Required Tool if applicable

Required Equipment if applicable

Required Unlock if applicable

Crafting Time

Output Item

Output Quantity

XP Reward

Can Craft Offline

Can Repeat

Future Expansion Notes

Recipes should be fully data-driven.

---

# Recipe Requirements

Recipes may require:

Crafting Profession Level

Item Materials

Gold

Specific Tool

Specific Equipment

Completed Dungeon

Unlocked Region

Combat Discipline Level

Companion Rank

Achievement

Future Research

Requirements should always be visible before the player can craft the item.

Locked recipes should show what is missing when possible.

---

# Material Rules

Crafting consumes materials.

Current rule:

Materials are reserved when crafting starts and consumed when crafting completes.

This means:

Starting a craft checks required materials.

Required materials become reserved and cannot be sold, destroyed, crafted with, upgraded with, or used for companion rank-ups.

When the crafting timer finishes, reserved materials are consumed.

If the craft is canceled, reserved materials are returned to normal inventory use.

If reserved materials are missing because of a bug or save issue, crafting should fail safely or pause.

This rule prevents exploits and keeps crafting consistent with the Save System, Inventory Framework, and Offline Progress Framework.

---

# Crafting Time

Crafting may take time.

Crafting time should depend on:

Recipe difficulty

Recipe tier

Crafting profession level

Crafting tool

Equipment bonuses

Companion bonuses

Food buffs

Elixirs if applicable

Relics

Future systems

Simple early recipes may craft quickly.

Important recipes may take longer.

Crafting time should feel like progression, not pointless waiting.

---

# Crafting Output Rules

Crafting creates output items.

A recipe may create:

One item

Multiple items

Random bonus output

Rare bonus output

Quality result in future systems

Output should be added to inventory when crafting completes.

If inventory has space, crafted items are added to inventory.

If inventory is full, crafting completion should pause or send crafted items to Offline Reward Claim storage when possible.

Crafted items should not be silently deleted.

If crafted items cannot be claimed because inventory is full, the UI should clearly explain how much space is needed.

---

# Crafting XP

Crafting should grant XP to the crafting profession used.

Examples:

Cooking recipe grants Cooking XP.

Smithing recipe grants Smithing XP.

Carpentry recipe grants Carpentry XP.

Herblore recipe grants Herblore XP.

Crafting XP may depend on:

Recipe tier

Crafting time

Material value

Required profession level

Output value

Difficulty

XP rewards should be defined in recipe data.

---

# Crafting Failure Rules

Current design:

Crafting does not fail by default.

If the player has the required materials and requirements, crafting should succeed.

Future systems may add special failure mechanics only if they improve gameplay.

Possible future failure systems:

Food burned while cooking

High-risk enchanting

Experimental alchemy

Rare item forging

Unstable magical crafting

Failure should not be added to basic crafting.

Avoid frustrating random failure for normal progression recipes.

---

# Crafting Success Bonus Rules

Crafting may have positive bonus results.

Possible bonuses:

Extra output

Bonus XP

Rare byproduct

Higher upgrade progress

Special item discovery

Bonus output should feel rewarding.

Bonus output should not be required for normal progression.

---

# Crafting Speed Bonuses

Crafting speed may be improved by:

Profession level

Tools

Equipment

Companions

Food buffs

Relics

Future account upgrades

Speed bonuses should be clear in the UI.

The player should understand why crafting time is reduced.

---

# Crafting Cost Reduction

Some bonuses may reduce crafting costs.

Cost reduction may affect:

Material quantity

Gold cost

Secondary material cost

Cost reduction should be balanced carefully.

Important rare materials should not be made too easy to avoid unless intentionally designed.

---

# Crafting and Inventory

Crafting reads materials from inventory.

Crafting should check:

Required item IDs

Required quantities

Locked item status

Inventory capacity

Output item space

Crafting should not consume locked items.

If the recipe would use locked items, the player should receive a warning or be blocked.

Crafting output should be added safely.

---

# Reserved Material Rules

Reserved materials are still visible in inventory, but they are marked as reserved.

Reserved materials cannot be used by other systems.

Reserved materials cannot be:

Sold

Destroyed

Used for another crafting recipe

Used for equipment upgrades

Used for companion rank-ups

Used in shops

Reserved material status should save and load correctly.

The UI should clearly show when materials are reserved for active crafting.


---

# Crafting and Equipment

Crafting creates and upgrades equipment.

Equipment crafting may create:

Weapons

Armor

Tools

Accessories

Relics

Equipment recipes should connect to the Equipment Framework.

Crafted equipment should appear in inventory.

If equipment has unique instance data, it should create a new equipment instance.

---

# Crafting and Upgrade Paths

Crafting may upgrade existing items.

Upgrade recipes may require:

Base equipment

Upgrade materials

Gold

Rare drops

Dungeon materials

Boss materials

Profession level

Combat Discipline level

Companion rank

Example:

Iron Sword

↓

Iron Sword + Wolf Fang + Gold

↓

Fanged Iron Sword

Upgrade paths should give players long-term goals.

---

# Crafting and Consumables

Crafting creates consumables.

Consumable crafting may include:

Food

Potions

Elixirs

Future consumables

Consumables should connect to combat, professions, and preparation.

Cooking and Herblore should be important consumable-producing professions.

---

# Crafting and Companions

Companions may require crafted items for rank-ups.

Examples:

Combat companion requires crafted weapon.

Profession companion requires crafted tool.

Mage companion requires crafted rune or staffs.

Rare companion requires boss material plus crafted item.

Crafting should help keep companion progression connected to the rest of the game.

---

# Crafting and Combat

Combat should provide materials for crafting.

Combat may drop:

Rare materials

Boss materials

Dungeon materials

Upgrade stones

Monster parts

Unique equipment bases

Crafting should use combat materials to create better gear and unlock new progression.

This connects combat back into professions.

---

# Crafting and Runes

Runecrafting creates runes used for combat preparation.

Runes are not only for Mage.

Runes can support Warrior, Ranger, and Mage builds.

Current design:

The player may activate up to 5 runes before combat.

Activated runes provide passive combat bonuses during combat.

Runes should be selected before combat starts.

Once combat begins, active runes cannot be changed until combat ends.

Rune bonuses may affect:

Damage

Defense

Accuracy

Critical chance

Critical damage

Attack speed

Devotion regeneration

Elemental resistance

Healing received

Rare drop chance

Combat XP gain

Runes should create meaningful preparation choices.

Examples:

Warrior may use runes that improve defense, shield bonuses, melee damage, or healing received.

Ranger may use runes that improve attack speed, accuracy, critical chance, or rare drop chance.

Mage may use runes that improve elemental damage, devotion regeneration, or spell effects.

Runes should support all Combat Disciplines without becoming mandatory for every easy encounter.

---

# Crafting and Professions

Gathering professions should feed crafting professions.

Examples:

Woodcutting provides logs for Carpentry.

Mining provides ore for Smithing.

Fishing provides fish for Cooking.

Foraging provides herbs for Herblore.

Hunting provides hides for Leatherworking.

Farming provides herbs, crops, and cooking materials.

No crafting profession should feel disconnected from resource generation.

---

# Crafting and Shops

Shops may sell:

Basic crafting materials

For Low-tier recipes

Tools

Consumable ingredients

Crafting unlocks

Shops should not replace professions.

Shops should support early progression, fill small gaps, and provide convenience.

Rare crafting materials should usually come from gameplay, not shops.

---

# Crafting and Achievements

Achievements may require:

Crafting specific items

Crafting item quantities

Unlocking recipes

Crafting high-tier equipment

Crafting legendary items

Completing profession crafting milestones

Crafting achievements should reward long-term progression.

---

# Crafting and Collections

Crafted items should update the collection log.

When the player crafts an item for the first time:

Mark item as discovered.

Update collection progress.

Trigger first-time notification.

Check achievements.

Collection discovery should remain even if the item is later sold, consumed, destroyed, or used as material.

---

# Recipe Unlocks

Recipes may unlock from:

Profession level

Dungeon completion

Boss kill

Achievement

Region unlock

Shop purchase

Quest-like future system

Research future system

Recipe unlocks should feel exciting.

Unlocking a recipe should usually open a new progression option.

---

# Hidden Recipes

Future system:

Some recipes may be hidden until discovered.

Hidden recipes may be discovered by:

Finding recipe books

Defeating bosses

Crafting related items

Completing achievements

Exploring regions

Companion rank-ups

Hidden recipes should be used carefully.

Important core progression recipes should not be hidden in frustrating ways.

---

# Crafting UI

Crafting UI should display:

Crafting profession

Profession level

Recipe list

Recipe category

Recipe requirements

Required materials

Owned materials

Missing materials

Crafting time

XP reward

Output item

Output quantity

Craft button

Current active craft

Crafting progress bar

The player should always understand what they can craft and what they are missing.

---

# Recipe Details UI

Each recipe should show:

Recipe name

Recipe description

Output item

Output amount

Required profession level

Required materials

Required gold if needed

Required unlocks

Crafting time

XP reward

Used tool if applicable

Possible bonus output

Source of missing materials

The player should be able to click missing materials to see where they come from.

---

# Crafting Filters

Crafting UI should support filters.

Possible filters:

All Recipes

Available Recipes

Missing Materials

Locked Recipes

Equipment

Tools

Weapons

Armor

Accessories

Consumables

Materials

Upgrade Recipes

Companion Items

Favorites

Filters should help the player find useful recipes quickly.

---

# Crafting Search

Crafting UI should support search.

Search should match:

Recipe name

Output item name

Required material name

Crafting profession

Item type

Recipe category

Search becomes important once the game has many recipes.

---

# Crafting Favorites

The player may favorite recipes.

Favorite recipes should be easier to find.

Favorite recipe status should save and load correctly.

Favorites are useful for repeated crafting goals.

---

# Crafting Notifications

Crafting notifications may show:

Crafting completed

Missing materials

Recipe unlocked

New recipe available

Inventory full

Rare bonus output

Profession level up

Notifications should be useful but not annoying.

---

# Offline Crafting

Crafting may continue offline.

Offline crafting should use:

Crafting start time

Crafting end time

Recipe ID

Quantity being crafted

Crafting profession

Assigned companion

Saved buffs if applicable

Inventory state

When the player returns, completed crafting should be calculated.

If materials are available and inventory has space, crafted items are created.

If inventory is full, crafted items should go to Offline Reward Claim storage when possible.

Crafted items should not be silently deleted.

---

# Crafting Statistics

The game should track:

Total items crafted

Total recipes unlocked

Total crafting time

Total crafting XP gained

Total materials consumed

Total gold spent on crafting

Total bonus outputs gained

Total equipment crafted

Total consumables crafted

Total upgrades completed

First time crafted per item

Highest tier crafted per profession

---

# Crafting Database

Every recipe should exist inside the Crafting Recipe Database.

Recipes should never be hardcoded.

Systems should reference Recipe IDs.

Crafting recipe data should connect to:

Item Database

Equipment Database

Profession System

Inventory System

Companion System

Combat System

Achievement System

Collection System

Save System

Offline Progress System

---

# Technical Rules

Crafting should be data-driven.

Recipes should be editable without code changes.

Crafting should reference Item IDs.

Crafting should reference Recipe IDs.

Crafting should not duplicate item data.

Crafting should not hardcode item names or material lists inside scripts.

Crafting should save active jobs correctly.

Crafting should load active jobs safely.

Crafting should work online and offline.

Crafting should respect locked items.

Crafting should respect inventory capacity.

Adding a new recipe should not require editing crafting code.

---

# Balance Philosophy

Crafting should feel rewarding.

Crafting should not be only a resource sink.

Crafting should create meaningful choices:

Craft tools or weapons first?

Use rare drop now or save it?

Craft consumables or gear?

Sell resources or craft with them?

Upgrade current item or craft new item?

Crafting should connect multiple systems together.

Avoid recipes that exist only to inflate item count.

---

# Future Expansion

Possible additions:

Crafting queues

Batch crafting

Recipe mastery

Item quality

Critical crafting

Crafting specializations

Crafting stations

Crafting buildings

Crafting orders

Crafting contracts

Crafting automation

Rare crafting events

Experimental recipes

Crafting talent tree

Future crafting systems should expand the base Crafting Framework rather than replace it.

---

# Checklist

Every crafting feature should answer:

✓ Which crafting profession uses it?

✓ What item does it create?

✓ What materials does it require?

✓ What requirements does it have?

✓ How long does it take?

✓ How much XP does it give?

✓ Can it work offline?

✓ Does it respect locked items?

✓ What happens if inventory is full?

✓ Does it connect to equipment?

✓ Does it connect to combat?

✓ Does it connect to companions?

✓ Does it connect to professions?

✓ Does it update achievements?

✓ Does it update collections?

✓ Does it save and load correctly?

✓ Can it support future expansion?