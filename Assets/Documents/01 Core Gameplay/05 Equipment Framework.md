# 05 Equipment Framework

Version: 2.0  
Status: Revised Draft  
Primary Change: Profession tools now use normal equipment slots  
Supersedes: Version 1.0

---

# Purpose

This document defines how equipment works across the entire game.

Equipment includes:

- Combat weapons
- Profession tools
- Hybrid weapon-tools
- Shields
- Profession support items
- Armor
- Accessories
- Relics
- Future special equipment

The equipment system must support both Combat and Professions through one shared loadout.

There is no dedicated Tool equipment slot.

Every equipment item should follow this framework unless explicitly designed otherwise.

---

# Major Revision Summary

The previous dedicated Tool slot is removed.

The new slot model is:

- Main-Hand or Weapon slot
- Offhand or Shield slot
- Helmet
- Chest
- Legs
- Gloves
- Boots
- Cape
- Ring
- Amulet
- Relic

Profession tools now use the Main-Hand slot.

Profession support items now use the Offhand slot.

Examples:

- An Iron Axe may provide strong Woodcutting bonuses but weak Combat stats.
- An Iron Battle Axe may provide strong Combat stats but zero Woodcutting bonuses.
- A Forester's War Axe may provide moderate bonuses to both.
- A Woodcutter's Logbox may occupy the Offhand slot and improve Woodcutting.
- A Shield may occupy the Offhand slot and improve Combat defense.
- A two-handed item disables the Offhand slot.

The same equipped items affect both Combat and Professions.

The game should not maintain a separate hidden Profession Tool loadout.

---

# Design Goals

Equipment should:

- Make the player stronger or more efficient.
- Support both Combat and Professions.
- Create meaningful trade-offs between Combat power and Profession efficiency.
- Create meaningful upgrade goals.
- Connect gathering, crafting, Combat, companions, and the economy.
- Allow specialized, hybrid, and Combat-focused equipment.
- Use one shared equipment loadout.
- Be fully data-driven.
- Avoid becoming useless too quickly.
- Give players long-term progression goals.
- Remain understandable in the UI.
- Support online and offline calculations.
- Remain expandable for future content.

Equipment should never exist only as filler.

---

# Core Equipment Philosophy

The same equipment system is used everywhere.

This means:

- The item equipped in Main-Hand is used by Combat and Profession systems.
- The item equipped in Offhand is used by Combat and Profession systems.
- Profession bonuses are read directly from equipped item data.
- Combat stats are read directly from equipped item data.
- Equipping a Profession tool may reduce Combat power.
- Equipping a Combat weapon may reduce Profession efficiency.
- Hybrid items provide convenience but should not automatically be best in both systems.

The player should make understandable choices.

Examples:

Iron Axe plus Woodcutter's Logbox:

- Strong Woodcutting
- Weak Combat damage
- Weak Combat defense

Iron Axe plus Shield:

- Strong Woodcutting Main-Hand bonus
- Better Combat defense
- No Logbox bonus

Iron Battle Axe plus Shield:

- Strong Combat
- No Woodcutting equipment bonuses

Forester's War Axe plus Woodcutter's Logbox:

- Moderate Combat
- Moderate-to-strong Woodcutting
- Little Combat defense

---

# Equipment Categories

Current equipment categories:

- Main-Hand Equipment
- Offhand Equipment
- Armor
- Accessories
- Relics

Future categories may be added later.

The old standalone Tool category is removed as an equipment-slot category.

Items may still be described as Profession Tools through their role and capabilities.

---

# Equipment Roles

An equipment item may define one or more roles.

Possible roles:

- Combat Weapon
- Profession Tool
- Hybrid Weapon-Tool
- Shield
- Profession Support Item
- Hybrid Offhand
- Armor
- Accessory
- Relic
- Future Utility Item

Role does not determine slot by itself.

Slot, handedness, Combat stats, Profession capabilities, and modifiers must be stored explicitly.

---

# Equipment Types

Examples of equipment types:

- Sword
- Axe
- Battle Axe
- Pickaxe
- Bow
- Staff
- Fishing Rod
- Hammer
- Saw
- Knife
- Spear
- Shield
- Logbox
- Ore Satchel
- Tackle Box
- Field Journal
- Helmet
- Chest Armor
- Gloves
- Boots
- Ring
- Amulet
- Cape
- Relic

Visual type does not automatically grant Profession capability.

Example:

An item visually categorized as an Axe does not automatically count as a Woodcutting Tool.

The item must explicitly contain the Woodcutting Tool capability or Woodcutting modifiers.

---

# Equipment Structure

Every equipment item should define:

- Equipment ID
- Item ID
- Name
- Description
- Icon
- Category
- Type
- Role
- Equipment Slot
- Handedness
- Required Player Level when applicable
- Required Profession Level when applicable
- Required Combat Discipline Level when applicable
- Required Region when applicable
- Required Unlock when applicable
- Combat Discipline Compatibility
- Combat Stats
- Profession Capabilities
- Profession Modifiers
- Activity Tags
- Target Tags
- Region Tags
- Specialized Storage when applicable
- Passive Effects
- Active Effects when applicable
- Sell Value
- Crafting Recipe
- Upgrade Path
- Can Be Equipped
- Can Be Upgraded
- Can Be Enchanted
- Can Be Sold
- Can Be Destroyed
- Future Expansion Notes

Every equipment item must also exist in the Item Database.

Equipment should not duplicate shared item data unnecessarily.

---

# Equipment Slots

Current equipment slots:

- Main-Hand
- Offhand
- Helmet
- Chest
- Legs
- Gloves
- Boots
- Cape
- Ring
- Amulet
- Relic

There is no dedicated Tool slot.

Future slots may be added later only when they create meaningful gameplay.

---

# Main-Hand Slot

The Main-Hand slot may contain:

- Combat weapons
- Profession tools
- Hybrid weapon-tools
- One-handed items
- Two-handed items
- Future utility weapons

Every Main-Hand item may provide:

- Combat Damage
- Attack Interval
- Accuracy
- Critical modifiers
- Combat Ability compatibility
- Profession capabilities
- Profession Power
- Profession Action Speed
- Profession XP bonuses
- Resource bonuses
- Rare reward bonuses
- Special effects

The Main-Hand item should never require a second hidden Tool item to provide Profession functionality.

---

# Main-Hand Item Roles

## Specialized Profession Tool

A Specialized Profession Tool provides strong Profession bonuses and weak Combat performance.

Example:

Iron Axe

Possible properties:

- Main-Hand
- One-Handed
- Low Combat Damage
- Average or slow Combat Attack Interval
- Woodcutting Tool capability
- High Woodcutting Power
- Woodcutting Action Speed bonus
- Bonus Log chance

This item is primarily for Woodcutting.

---

## Hybrid Weapon-Tool

A Hybrid Weapon-Tool provides moderate Combat stats and moderate Profession bonuses.

Example:

Forester's War Axe

Possible properties:

- Main-Hand
- One-Handed
- Moderate Combat Damage
- Moderate Accuracy
- Woodcutting Tool capability
- Moderate Woodcutting Power
- Small Woodcutting XP bonus

Hybrid equipment provides convenience.

It should not normally outperform specialized equipment in both Combat and the supported Profession.

---

## Combat Weapon

A Combat Weapon provides strong Combat stats and no Profession bonuses unless explicitly defined.

Example:

Iron Battle Axe

Possible properties:

- Main-Hand
- One-Handed or Two-Handed
- High Combat Damage
- Strong Combat effects
- No Woodcutting Tool capability
- No Woodcutting Power
- No Log Quantity bonus

The item's visual appearance does not automatically make it a Profession Tool.

---

# Profession Capabilities

Profession Capabilities are explicit data tags on equipment.

They may allow an item to satisfy activity requirements.

Examples:

- Woodcutting Tool
- Mining Tool
- Fishing Tool
- Smithing Hammer
- Carpentry Saw
- Tailoring Needle
- Leatherworking Knife
- Archaeology Tool
- Hunting Tool

An item may:

- Have no Profession Capability.
- Have one Profession Capability.
- Have multiple Profession Capabilities.
- Have Profession modifiers without satisfying advanced activity requirements.
- Satisfy a requirement while providing weak bonuses.

Activities should reference capabilities rather than specific item names whenever possible.

Example:

Bad requirement:

Requires Iron Axe

Preferred requirement:

Requires Woodcutting Tool capability with Tier 2 or higher

Specific named-item requirements may still be used for special content.

---

# Base Profession Activity Without a Tool

A Profession may allow basic activities without a matching Profession Capability.

Recommended default:

- Early activities may use base Profession Power.
- Matching Profession Tools improve efficiency.
- Advanced activities may require a specific capability or capability tier.
- Individual Profession documents may override this rule.

Example:

The player may cut a Sproutwood Tree while holding an Iron Battle Axe.

The Battle Axe gives no Woodcutting bonuses.

A high-level Ancient Ironwood Tree may require a Tier 6 Woodcutting Tool capability.

---

# Offhand Slot

The Offhand slot may contain:

- Shields
- Profession Support Items
- Hybrid Offhand items
- Combat utility items
- Future focus items

Every Offhand item may provide:

- Defense
- Health
- Damage Reduction
- Resistance
- Combat utility
- Profession modifiers
- Specialized storage
- Reward bonuses
- Resource preservation
- Activity access
- Special effects

A Profession Support Item competes directly with a Shield.

---

# Offhand Item Roles

## Shield

A Shield primarily supports Combat.

Possible properties:

- Defense
- Maximum Health
- Damage Reduction
- Resistance
- Block effects
- No Profession bonuses

---

## Profession Support Item

A Profession Support Item primarily supports one or more Professions.

Possible properties:

- Bonus resource output
- Profession XP bonus
- Rare reward chance
- Resource preservation
- Specialized storage
- Activity-specific effects
- No or very low Combat defense

Example:

Woodcutter's Logbox

---

## Hybrid Offhand

A Hybrid Offhand provides moderate Combat and Profession benefits.

Example:

Reinforced Forester's Logbox

Possible properties:

- Small Defense bonus
- Small Maximum Health bonus
- Log storage
- Small Woodcutting output bonus

Hybrid Offhand items should not outperform specialized Shields and specialized Profession Support Items in their primary roles.

---

# Profession Support Item Examples

## Woodcutter's Logbox

Slot:

Offhand

Possible bonuses:

- Increased Woodcutting output
- Chance for bonus logs
- Additional specialized log storage
- Reduced Inventory pressure from logs
- Small Woodcutting XP bonus
- No or very low Combat defense

---

## Miner's Ore Satchel

Slot:

Offhand

Possible bonuses:

- Increased ore quantity
- Additional ore storage
- Rare gem preservation
- Mining Action Speed
- No or very low Combat defense

---

## Angler's Tackle Box

Slot:

Offhand

Possible bonuses:

- Increased Fishing success
- Increased bait preservation
- Bonus fish chance
- Rare catch chance
- No or very low Combat defense

---

## Archaeologist's Field Journal

Slot:

Offhand

Possible bonuses:

- Artifact identification chance
- Rare artifact chance
- Excavation XP
- Site information
- No or very low Combat defense

---

# Handedness

Every Main-Hand item must define handedness.

Possible values:

- One-Handed
- Two-Handed

Future values may be added only when necessary.

Handedness affects whether the Offhand slot can be used.

---

# One-Handed Rules

A one-handed Main-Hand item allows the Offhand slot to be used.

Examples:

- Iron Axe plus Woodcutter's Logbox
- Iron Axe plus Shield
- Sword plus Shield
- Fishing Rod plus Tackle Box when the Rod is defined as one-handed
- Hybrid weapon-tool plus Profession Support Item

One-handed items should be balanced around access to an Offhand item.

---

# Two-Handed Rules

A two-handed Main-Hand item occupies both Main-Hand and Offhand equipment capacity.

When a two-handed item is equipped:

- The Offhand slot becomes disabled.
- Shield bonuses do not apply.
- Profession Support Item bonuses do not apply.
- Hybrid Offhand bonuses do not apply.
- The UI should clearly show why the Offhand slot is disabled.
- Any previously equipped Offhand item should be returned safely to Inventory.
- If Inventory is full, the equipment change must be blocked or the old Offhand item must enter protected claim storage.
- No item may be silently deleted.

This applies to:

- Two-handed Combat weapons
- Two-handed Profession Tools
- Two-handed hybrid items

A powerful two-handed Profession Tool may provide enough power to compensate for losing an Offhand Profession Support Item.

---

# Shared Equipment Between Combat and Professions

The same loadout is active in all systems.

The game should not automatically swap equipment when:

- Starting a Profession
- Stopping a Profession
- Starting Combat
- Ending Combat
- Changing screens

This ensures equipment choices remain meaningful.

Future loadout presets may provide convenient manual or approved automatic switching outside active activities.

Even with presets, the game should still use normal equipment rules.

---

# Equipment Changes During Active Activities

Changing relevant equipment during an active activity must not create exploits.

## Active Profession

Default rule:

Changing Main-Hand or Offhand equipment stops the current Profession activity.

The UI should request confirmation.

Example:

Changing your Main-Hand equipment will stop Woodcutting.

Continue?

When confirmed:

- The current action stops.
- Incomplete progress grants no reward.
- Already-earned rewards remain.
- The equipment change is applied.
- Profession modifiers are recalculated.
- The activity must be restarted manually unless a future automation system says otherwise.

Changing another equipment slot that affects the active Profession should either:

- Stop the activity, or
- Apply only at the next action boundary

The safer rule should be used when uncertainty exists.

---

## Active Combat

Equipment loadout changes are not allowed during active Combat unless a future Combat system explicitly permits them.

The player should end Combat before changing equipment.

This includes:

- Main-Hand
- Offhand
- Armor
- Accessories
- Relic
- Profession Support Items

---

# Combat Equipment

Combat equipment improves performance in Combat.

Possible Combat stats:

- Attack Damage
- Damage Range
- Attack Interval
- Accuracy
- Critical Chance
- Critical Damage
- Defense
- Health
- Health Regeneration
- Healing Received
- Damage Reduction
- Elemental Damage
- Elemental Resistance
- Combat XP Bonus
- Devotion bonuses
- Ability bonuses
- Cooldown bonuses

Combat stats must remain separate from Profession stats.

High Combat Damage does not automatically provide Profession Power.

---

# Profession Equipment

Profession equipment improves non-combat activities.

Profession bonuses may appear on:

- Main-Hand items
- Offhand items
- Armor
- Gloves
- Boots
- Rings
- Amulets
- Capes
- Relics
- Future equipment

Possible Profession stats:

- Profession Power
- Action Speed
- Resource Gain
- XP Gain
- Rare Reward Chance
- Reduced Resource Cost
- Bonus Output Chance
- Resource Preservation
- Success Chance
- Respawn Reduction
- Specialized Storage
- Activity Access

Profession equipment should improve efficiency without replacing Profession levels.

---

# Profession Modifier Categories

Recommended Profession modifier categories:

- Profession Power
- Action Speed
- XP Gain
- Primary Output
- Secondary Output
- Rare Reward Chance
- Bonus Output Chance
- Resource Preservation
- Resource Cost Reduction
- Success Chance
- Failure Reduction
- Respawn Reduction
- Specialized Storage
- Activity Access
- Region Bonus
- Target-Type Bonus

Every Profession modifier should define:

- Modifier ID
- Supported Profession
- Supported Activity Type when limited
- Supported Target Tags when limited
- Supported Region when limited
- Flat or percentage value
- Additive or multiplicative behavior
- Stacking rule
- Maximum or cap when applicable
- Online applicability
- Offline applicability

---

# Profession Power Versus Combat Damage

Profession Power and Combat Damage are different statistics.

Examples:

Iron Axe:

- Combat Damage: 4–7
- Woodcutting Power: 20

Iron Battle Axe:

- Combat Damage: 12–18
- Woodcutting Power: 0

Forester's War Axe:

- Combat Damage: 8–12
- Woodcutting Power: 12

The Profession system should read Woodcutting Power.

The Combat system should read Combat Damage.

Neither should derive one value from the other.

Profession damage does not grant Combat Discipline XP.

---

# Armor

Armor may improve:

- Defense
- Health
- Damage Reduction
- Resistances
- Combat Discipline identity
- Profession efficiency
- Region efficiency
- Movement or activity effects
- Special mechanics

Armor slots:

- Helmet
- Chest
- Legs
- Gloves
- Boots

Profession bonuses may exist on Armor.

Examples:

- Woodcutter Gloves increase Log Quantity.
- Miner Boots reduce Mining Action Time.
- Herbalist Hood increases rare Herb chance.

Armor should not require a separate Profession equipment screen.

---

# Accessories

Accessories provide flexible bonuses.

Accessory slots:

- Ring
- Amulet
- Cape

Possible bonuses:

- Combat bonuses
- Profession bonuses
- XP bonuses
- Resource bonuses
- Food buff duration
- Companion bonuses
- Rare reward chance
- Region bonuses
- Activity-specific effects

Accessories should allow players to specialize or create hybrid builds.

---

# Relics

Relics are special long-term progression equipment.

Relics may provide:

- Passive account bonuses
- Profession bonuses
- Combat bonuses
- Active effects
- Unique mechanics

Relics should be hard to obtain, difficult to upgrade, and meaningful.

---

# Active Relic Rules

Every active Relic should define:

- Activation Effect
- Duration
- Cooldown
- Requirement
- Target System
- Valid Activity Types
- Valid Combat Types
- Offline Behavior
- Stacking Rules

Relics should not require constant clicking.

Active Relics should feel like strategic tools rather than mandatory spam buttons.

---

# Specialized Storage

Some Offhand Profession Support Items may provide specialized storage.

Examples:

- Logbox stores logs
- Ore Satchel stores ore
- Tackle Box stores bait or fish
- Artifact Case stores artifacts

Every specialized storage definition should include:

- Storage ID
- Supported Item Tags
- Capacity
- Stack rules
- Whether stored items count toward normal Inventory capacity
- Whether rewards enter storage automatically
- What happens when storage is full
- What happens when the item is unequipped
- What happens when a two-handed item is equipped
- Save behavior
- Offline behavior
- Protected overflow behavior

Unequipping a storage item must never silently delete stored items.

Recommended behavior:

1. Attempt to move stored items into Inventory.
2. When Inventory cannot accept all items, block unequip or use protected claim storage.
3. Explain the result clearly to the player.

---

# Equipment Requirements

Equipment may require:

- Player Level
- Profession Level
- Combat Discipline Level
- Specific Profession Level
- Specific Combat Discipline Level
- Completed Dungeon
- Defeated Boss
- Unlocked Region
- Crafting Requirement
- Achievement
- Item
- Future progression requirement

Requirements should always be visible before the player equips or obtains the item.

---

# Combat Discipline Compatibility

Main-Hand Combat equipment may support:

- Warrior
- Ranger
- Mage
- Multiple Combat Disciplines
- All Combat Disciplines
- No Combat Discipline

Profession Tools may still have Combat compatibility.

Example:

Iron Axe:

- Warrior compatible
- Weak Combat stats
- Strong Woodcutting bonuses

A Profession Tool may also be Combat-incompatible if that creates a clear and intentional rule.

---

# Equipment Sources

Equipment may come from:

- Crafting
- Combat Drops
- Boss Drops
- Dungeon Rewards
- Shops
- Achievements
- Collection Milestones
- Profession activities
- Companion progression
- Region rewards
- Future systems

Profession Tools and Profession Support Items should have logical acquisition paths.

---

# Crafting Equipment

Equipment should connect Professions, Combat, and progression.

Examples:

Mining

↓

Smithing

↓

Iron Axe

↓

Woodcutting efficiency and weak Combat use

Woodcutting

↓

Carpentry

↓

Woodcutter's Logbox

↓

Woodcutting output and storage

Mining

↓

Smithing

↓

Iron Battle Axe

↓

Strong Warrior Combat

Hunting

↓

Leatherworking

↓

Armor

↓

Combat and possible Profession bonuses

Jewelcrafting

↓

Ring

↓

Flexible passive bonuses

Enchanting

↓

Equipment enhancement

---

# Upgrade Paths

Equipment may have upgrade paths.

Examples:

Iron Axe

↓

Reinforced Iron Axe

↓

Runic Iron Axe

Or:

Forester's War Axe

↓

Ancient Forester's War Axe

Upgrade paths should provide clear long-term goals.

Upgrades may require:

- Base Equipment
- Crafting Materials
- Gold
- Rare Drops
- Profession Level
- Combat Discipline Level
- Dungeon Materials
- Boss Materials
- Region Materials
- Future Special Materials

---

# Upgrade Identity

Upgrades should preserve or intentionally transform an item's role.

Examples:

Specialized Profession path:

Iron Axe

↓

Master Woodcutter's Axe

Hybrid path:

Iron Axe

↓

Forester's War Axe

Combat path:

Iron Axe

↓

Iron Battle Axe

A branch that changes item identity should be explained clearly.

The player should understand whether an upgrade:

- Improves Profession bonuses
- Improves Combat stats
- Creates a hybrid item
- Changes handedness
- Changes Offhand compatibility
- Removes or adds a Profession Capability

---

# Equipment Bonuses

Equipment bonuses may affect:

- Combat Damage
- Combat Defense
- Profession Power
- Profession Action Speed
- XP Gain
- Resource Gain
- Rare Reward Chance
- Resource Preservation
- Food Buff Strength
- Food Buff Duration
- Companion Effects
- Activity Access
- Region bonuses
- Unlock Requirements
- Specialized Storage

Bonuses should be easy to understand.

Avoid overly complicated stat descriptions.

---

# Bonus Stacking Rules

Equipment bonuses should stack in a clear and predictable way.

Default order:

1. Base values
2. Flat equipment bonuses
3. Additive percentage equipment bonuses
4. Multiplicative equipment effects
5. Companion bonuses
6. Temporary Food and Elixir bonuses
7. Relic effects
8. Unique final modifiers

The exact calculation should be centralized.

When multiple items provide the same bonus, the UI should show:

- Each source
- Combined total
- Caps when applicable
- Inactive bonuses and why they are inactive

---

# Equipment Balance Philosophy

Equipment should improve progression without making older systems irrelevant.

Better equipment should feel exciting.

However, equipment should not remove the value of:

- Profession Levels
- Combat Discipline Levels
- Resources
- Companions
- Food Buffs
- Achievements
- Player Decisions

Specialized Profession equipment should be best for its intended Profession.

Combat equipment should be best for its intended Combat role.

Hybrid equipment should provide convenience without dominating both.

One-handed and two-handed items should create meaningful trade-offs.

Offhand Profession Support Items should be strong enough to compete with Shields during Profession activity.

---

# Equipment Power Budget

Every equipment item should have a power budget.

Power may be distributed across:

- Combat stats
- Profession capabilities
- Profession modifiers
- Specialized storage
- Passive effects
- Active effects
- Handedness
- Slot competition
- Requirements

An item with strong Profession bonuses may need weaker Combat stats.

An item with strong Combat stats may have no Profession bonuses.

A hybrid item should usually have reduced peak power in each role.

A two-handed item may have a higher power budget because it disables the Offhand slot.

---

# Durability

Current design:

No equipment durability by default.

Future option:

Durability may be added for special systems only when it improves gameplay.

Do not add durability as routine maintenance.

---

# Enchanting

Enchanting is a future system.

Equipment may support Enchantments later.

Possible Enchantment effects:

- More Combat Damage
- More Defense
- More Profession Power
- Faster Profession actions
- Extra resources
- Rare reward chance
- Specialized storage
- Unique passive effects

Enchanting should expand equipment rather than replace normal progression.

Enchantments should respect the item's valid roles and caps.

---

# Equipment and Companions

Companions may interact with equipment.

Possible examples:

- Companion rank-ups require specific equipment or materials.
- Combat companions request Combat equipment as rank-up materials.
- Profession companions request Profession Tools, Profession Support Items, or resources.
- A companion may increase bonuses from a supported equipment tag.

Companions do not equip player items by default.

---

# Equipment and Offline Progress

Equipment bonuses should apply to offline progress unless explicitly disabled.

Examples:

- Axe bonuses improve offline Woodcutting.
- Logbox storage affects offline Woodcutting capacity.
- Pickaxe bonuses improve offline Mining.
- Combat gear improves offline Combat if offline Combat is allowed.

Offline calculations should use the saved equipped loadout.

The game should not automatically choose better equipment while offline.

---

# Equipment Snapshot for Offline Progress

The save should record:

- Equipped Main-Hand item
- Equipped Offhand item
- Equipped Armor
- Equipped Accessories
- Equipped Relic
- Item instance data
- Specialized storage contents
- Active enchantments when added
- Relevant equipment state

Offline simulation should resolve bonuses from this saved equipment snapshot.

---

# Equipping Items

When the player equips an item:

1. Validate ownership.
2. Validate the target slot.
3. Validate requirements.
4. Validate Combat Discipline restrictions when applicable.
5. Validate Profession restrictions when applicable.
6. Validate handedness.
7. Validate Offhand conflicts.
8. Validate active activity restrictions.
9. Validate Inventory capacity for displaced items.
10. Safely unequip conflicting items.
11. Equip the new item.
12. Recalculate Combat stats.
13. Recalculate Profession modifiers.
14. Update active-system validation.
15. Update UI.
16. Autosave.

The UI should request the equipment change through the Equipment System.

The UI should not directly edit final stats.

---

# Unequipping Items

When the player unequips an item:

1. Validate Inventory capacity.
2. Validate specialized storage contents.
3. Validate active activity restrictions.
4. Remove the item from the slot.
5. Return it to Inventory.
6. Recalculate Combat stats.
7. Recalculate Profession modifiers.
8. Update UI.
9. Autosave.

When the item contains specialized storage:

- Move stored items safely.
- Block unsafe unequip when necessary.
- Never delete stored items silently.

---

# Equipping Two-Handed Items

When equipping a two-handed item:

1. Validate Main-Hand requirements.
2. Detect the equipped Offhand item.
3. Check whether the Offhand item contains specialized storage.
4. Safely move or protect stored contents.
5. Validate Inventory capacity.
6. Unequip the Offhand item.
7. Equip the two-handed item.
8. Disable the Offhand slot.
9. Recalculate all stats.
10. Update the UI.
11. Autosave.

When the Offhand item cannot be removed safely:

- Block the equipment change.
- Explain why.

---

# Equipment Statistics

The game may track:

- Times Equipped
- Times Crafted
- Times Upgraded
- Times Sold
- Highest Equipment Tier Owned
- First Time Obtained
- Total Resources Spent on Equipment
- Combat Damage Done With Equipment
- Profession Resources Gathered With Equipment
- Time Equipped During Combat
- Time Equipped During Professions
- Activities Completed With Equipment
- Targets Completed With Equipment
- Offline Progress Generated With Equipment

Statistics should use stable equipment IDs.

---

# Equipment User Interface

The Equipment screen should display:

- Equipment icon
- Name
- Description
- Slot
- Type
- Role
- Handedness
- Requirements
- Combat Discipline compatibility
- Combat stats
- Profession capabilities
- Profession modifiers
- Specialized storage
- Source
- Sell Value
- Crafting Recipe
- Upgrade Path
- Equip Button
- Unequip Button
- Upgrade Button
- Compare With Current Equipment

There should be no dedicated Tool slot.

Profession tools should appear in the Main-Hand slot.

Profession Support Items should appear in the Offhand slot.

---

# Equipment Screen Slots

Required visible slots:

- Main-Hand
- Offhand
- Helmet
- Chest
- Legs
- Gloves
- Boots
- Cape
- Ring
- Amulet
- Relic

The old Tool slot must be removed from:

- Scene Hierarchy
- Prefabs
- Screen scripts
- Equipment data
- Save data
- UI references
- Comparison logic
- Offline logic

---

# Equipment Filters

The Equipment screen should support filtering by:

- Slot
- Combat Discipline
- Profession
- Profession Capability
- Equipment Role
- One-Handed
- Two-Handed
- Specialized
- Hybrid
- Combat-focused
- Has Specialized Storage
- Can Equip
- Locked by Requirement

Examples:

- Show Woodcutting-compatible Main-Hand items.
- Show Woodcutting Offhand support items.
- Show Warrior-compatible weapons.
- Show all hybrid items.
- Show all two-handed equipment.

---

# Equipment Comparison

When viewing equipment, compare it to the item currently occupying the same slot.

The comparison should show:

## Combat Changes

- Damage
- Attack Interval
- Accuracy
- Critical stats
- Defense
- Health
- Resistances
- Ability effects

## Profession Changes

- Profession Capability gained or lost
- Profession Power
- Action Speed
- XP Gain
- Resource Gain
- Rare Reward Chance
- Resource Preservation
- Specialized Storage
- Activity Access

## Slot and Handedness Changes

- One-Handed to Two-Handed
- Offhand item lost
- Shield lost
- Profession Support Item lost
- Storage capacity lost
- New Offhand access
- Disabled slot state

## Requirement Problems

- Level too low
- Combat Discipline too low
- Profession level too low
- Region locked
- Activity currently active
- Inventory capacity insufficient
- Specialized storage not empty

Comparison should use:

- Numerical differences
- Up and down indicators
- Text labels
- Color as secondary communication only

---

# Equipment Tooltips

Equipment tooltips should display both Combat and Profession information.

Example:

## Iron Axe

Combat:

- Damage: 4–7
- Attack Interval: 2.3 seconds
- Accuracy: +4
- One-Handed

Woodcutting:

- Woodcutting Tool capability
- Woodcutting Power: +20
- Woodcutting Action Speed: +8%
- Bonus Logs: +5%

---

## Iron Battle Axe

Combat:

- Damage: 12–18
- Attack Interval: 2.8 seconds
- Accuracy: +10
- Two-Handed

Woodcutting:

- No Woodcutting capability
- No Woodcutting bonuses

---

## Woodcutter's Logbox

Combat:

- Defense: 0

Woodcutting:

- Log Output: +10%
- Log Storage: +20 stacks
- Rare Wood chance: +2%

The player should never need to guess whether an item supports a Profession.

---

# Profession Loadout Display

Profession screens should display the relevant parts of the Equipment loadout.

Required display:

Main-Hand:

- Item icon
- Name
- Handedness
- Relevant Profession Capability
- Relevant Profession modifiers

Offhand:

- Item icon
- Name
- Shield or Profession Support role
- Relevant Profession modifiers
- Disabled-by-two-handed state

Combined:

- Final Profession Power
- Final Action Speed
- Final XP bonus
- Final output bonus
- Final rare reward bonus
- Specialized storage

There should be a shortcut from the Profession screen to Equipment.

---

# Equipment Database

Every equipment item should exist inside the Equipment Database.

Equipment should never be hardcoded.

Systems should reference Equipment IDs.

Equipment should connect to:

- Item Database
- Profession System
- Combat System
- Crafting System
- Inventory System
- Save System
- Offline Progress System
- Modifier System
- Collection System
- Achievement System

---

# Equipment Database Fields

Recommended database fields:

- Equipment ID
- Item ID
- Display Name
- Description
- Icon
- Slot
- Category
- Type
- Role
- Handedness
- Requirement Set
- Combat Discipline Compatibility
- Combat Stat Block
- Profession Capability List
- Profession Modifier List
- Specialized Storage Definition
- Passive Effect List
- Active Effect List
- Upgrade Path
- Enchantment Support
- Source Information
- Sell Value
- Future Expansion Notes

Player ownership and equipped state belong in save data.

---

# Profession Modifier Resolver Integration

The Equipment System should provide equipped item data to the shared Profession Modifier Resolver.

Inputs may include:

- Profession ID
- Activity ID
- Target Tags
- Region ID
- Equipped Main-Hand
- Equipped Offhand
- Equipped Armor
- Equipped Accessories
- Equipped Relic

Outputs may include:

- Final Profession Power
- Final Action Time
- Final XP multiplier
- Final output multiplier
- Final rare reward chance
- Final preservation chance
- Final success chance
- Final specialized storage
- Requirement validation
- Modifier breakdown

The UI should display the resolver result.

The UI should not calculate final Profession values independently.

---

# Save Data

Save data should store:

- Main-Hand equipped item
- Offhand equipped item
- Helmet
- Chest
- Legs
- Gloves
- Boots
- Cape
- Ring
- Amulet
- Relic
- Item instance data
- Specialized storage contents
- Upgrade state
- Enchantment state when added
- Equipment statistics when required

Save data should not contain a dedicated Tool slot after migration.

---

# Migration From the Old Tool Slot

Existing prototype saves or scene data may contain a dedicated Tool slot.

A migration is required.

Recommended migration process:

1. Read the old Tool slot item.
2. Remove the old Tool slot from the active equipment model.
3. When Main-Hand is empty and the old Tool is compatible, equip it in Main-Hand.
4. When Main-Hand is occupied, move the old Tool to Inventory.
5. When Inventory is full, move the old Tool to protected claim storage.
6. Never silently delete the old Tool item.
7. Remove obsolete Tool-slot references.
8. Recalculate Combat stats.
9. Recalculate Profession modifiers.
10. Save using the new save version.
11. Record that migration has completed.

Scene migration should also remove:

- ToolSlot GameObject
- ToolSlot serialized references
- ToolSlot UI labels
- ToolSlot comparison logic
- ToolSlot filtering
- ToolSlot placeholder art

---

# Technical Rules

Equipment should be data-driven.

Stats should be editable without code changes.

Bonuses should be modular.

Equipment should not duplicate item data.

Equipped items should save and load correctly.

Combat and Profession systems should read bonuses from equipped items.

The Equipment System should own:

- Slot validation
- Requirement validation
- Handedness validation
- Conflict handling
- Equip and unequip operations
- Stat recalculation requests
- Save notifications

UI views should only:

- Display equipment
- Display comparisons
- Display warnings
- Request equip or unequip operations

---

# Codex Implementation Rules

Codex must:

- Remove the dedicated Tool slot.
- Use Main-Hand for Profession Tools.
- Use Offhand for Profession Support Items.
- Support Combat stats and Profession modifiers on the same item.
- Keep Combat Damage separate from Profession Power.
- Use explicit Profession Capability tags.
- Prevent visual weapon type from automatically granting Profession bonuses.
- Support specialized, hybrid, and Combat-focused equipment.
- Validate one-handed and two-handed rules.
- Safely unequip Offhand items when equipping two-handed items.
- Preserve specialized storage contents.
- Stop active Professions when relevant equipment changes.
- Prevent equipment changes during active Combat.
- Update offline calculations.
- Update Equipment comparison.
- Update tooltips.
- Update save data.
- Migrate old Tool-slot items safely.
- Keep everything data-driven.

Codex must not:

- Recreate a hidden Tool slot.
- Automatically equip a Profession Tool when entering a Profession.
- Automatically equip a Combat weapon when entering Combat.
- Treat every Axe as a Woodcutting Tool.
- Treat every Pickaxe-shaped item as a Mining Tool.
- Convert Combat Damage into Profession Power.
- Allow two-handed and Offhand bonuses simultaneously.
- Delete displaced equipment or stored resources.
- Put equipment calculations inside UI scripts.
- Hardcode Profession item names into Profession logic.

---

# Future Expansion

Possible additions:

- Equipment Sets
- Profession Equipment Sets
- Loadout Presets
- Manual Loadout Switching
- Approved Automatic Loadout Switching Outside Active Activities
- Enchantments
- Item Quality
- Unique Effects
- Relic Upgrades
- Transmog
- Socketed Gems
- Special Boss Gear
- Companion Equipment
- Endgame Upgrade Paths
- More Hybrid Weapon-Tools
- More Profession Support Items
- More Specialized Storage Types

Future systems should expand this framework rather than replace it.

---

# Acceptance Criteria

The revised Equipment System is acceptable when:

- No dedicated Tool slot exists.
- Profession Tools equip in Main-Hand.
- Profession Support Items equip in Offhand.
- Combat weapons may provide zero Profession bonuses.
- Profession Tools may still function in Combat.
- Hybrid items support both systems.
- Two-handed items disable Offhand.
- Offhand items are returned safely.
- Specialized storage cannot lose items.
- Combat stats and Profession modifiers are separate.
- Equipment changes stop active Professions when required.
- Equipment cannot change during active Combat.
- Profession screens display Main-Hand and Offhand bonuses.
- Equipment comparisons show both Combat and Profession changes.
- Tooltips show both Combat and Profession information.
- Offline progress uses the saved equipment loadout.
- Old Tool-slot items migrate safely.
- Save and load work without a Tool field.
- The system remains data-driven.

---

# Checklist

## Every Equipment Item Should Answer

✓ What category is this equipment?

✓ What type is this equipment?

✓ What role does it have?

✓ Which slot does it use?

✓ Is it one-handed or two-handed?

✓ What are the requirements?

✓ Where does it come from?

✓ Can it be crafted?

✓ Can it be upgraded?

✓ What Combat stats does it give?

✓ What Profession Capabilities does it give?

✓ What Profession modifiers does it give?

✓ Does it provide specialized storage?

✓ Which Profession, Combat Discipline, or system uses it?

✓ Is it specialized, hybrid, or Combat-focused?

✓ Does it remain useful later?

✓ Does it work offline?

✓ Does it connect to other systems?

✓ Can it support future expansion?

---

## Every Main-Hand Item Should Answer

✓ Is it one-handed or two-handed?

✓ What Combat stats does it provide?

✓ Which Combat Disciplines may use it?

✓ Does it provide a Profession Capability?

✓ What Profession Power does it provide?

✓ What Profession modifiers does it provide?

✓ Does it allow an Offhand item?

✓ Is it a Combat Weapon, Profession Tool, or Hybrid Weapon-Tool?

---

## Every Offhand Item Should Answer

✓ Is it a Shield, Profession Support Item, or Hybrid Offhand?

✓ What Combat defense does it provide?

✓ What Profession bonuses does it provide?

✓ Does it provide specialized storage?

✓ What happens to stored items when unequipped?

✓ Is it disabled by two-handed equipment?

✓ Does it work offline?

✓ Does it require a matching Main-Hand capability?

---

## Every Equipment Change Should Answer

✓ Is the player in active Combat?

✓ Is a Profession activity active?

✓ Will the change stop that activity?

✓ Will a two-handed conflict occur?

✓ Can displaced items enter Inventory?

✓ Does specialized storage contain items?

✓ Are all displaced items protected?

✓ Are Combat stats recalculated?

✓ Are Profession modifiers recalculated?

✓ Is the change saved?

---

## Every Equipment Comparison Should Answer

✓ What Combat stats increase?

✓ What Combat stats decrease?

✓ What Profession capabilities are gained?

✓ What Profession capabilities are lost?

✓ What Profession modifiers increase?

✓ What Profession modifiers decrease?

✓ Is specialized storage gained or lost?

✓ Does handedness change?

✓ Is the Offhand item lost?

✓ Are requirements met?

---

## Every Save Migration Should Answer

✓ Is the old Tool slot removed?

✓ Is the old Tool item preserved?

✓ Can it move safely to Main-Hand?

✓ Can it move safely to Inventory?

✓ Is protected claim storage used when necessary?

✓ Are old UI references removed?

✓ Are Combat stats recalculated?

✓ Are Profession modifiers recalculated?

✓ Is migration completion saved?
