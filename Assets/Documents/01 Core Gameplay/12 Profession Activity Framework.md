# 12 Profession Activity Framework

Version: 2.0  
Status: Revised Draft  
Primary Change: Profession tools now use normal equipment slots  
Supersedes: Version 1.0

---

# Purpose

This document defines how Profession activities work across the entire game.

It establishes shared rules for:

- Profession progression
- Activity structure
- Activity requirements
- Profession loadouts
- Main-hand tools and weapons
- Offhand Profession support items
- Action flow
- Target durability
- Rewards
- Experience
- Automatic repetition
- Offline progress
- Inventory interaction
- User interface
- Data architecture
- Save compatibility

Individual Profession documents may add unique mechanics, but they should follow this framework unless explicitly designed otherwise.

---

# Major Revision Summary

The previous dedicated Tool equipment slot is removed.

Profession tools now use the same equipment slots as combat equipment.

The new equipment model is:

- Main-hand or Weapon slot for tools and weapons
- Offhand or Shield slot for shields and Profession support items
- No separate Tool slot
- The same equipped item may affect both Combat and Professions
- Items may specialize in Combat, Profession bonuses, or a hybrid role

Examples:

- An Iron Axe may provide strong Woodcutting bonuses but weak Combat stats.
- An Iron Battle Axe may provide strong Combat stats but no Woodcutting bonuses.
- A Woodcutter's Logbox may be equipped in the Offhand slot and provide Woodcutting storage or output bonuses.
- A Shield may provide Combat defense but no Profession bonuses.
- A two-handed weapon or tool prevents the use of an Offhand item.

This creates meaningful equipment trade-offs between Combat power and Profession efficiency.

---

# Design Goals

Profession activities should:

- Be easy to understand.
- Provide meaningful long-term progression.
- Connect to items, equipment, Combat, crafting, companions, achievements, and collections.
- Support both active and offline progression.
- Reward higher Profession levels and intelligent equipment choices.
- Make Combat and Profession equipment compete for the same slots.
- Allow specialized, hybrid, and Combat-focused loadouts.
- Avoid requiring a separate permanent equipment system for Profession tools.
- Provide meaningful activity choices.
- Avoid becoming simple number-only progression.
- Be fully data-driven.
- Allow individual Professions to have unique identities.
- Remain expandable for future content.

Every Profession should contribute to the wider game economy.

---

# Core Terminology

## Profession

A non-combat progression system with levels, XP, activities, rewards, and unlocks.

Current maximum Profession level:

100

Examples:

- Woodcutting
- Mining
- Fishing
- Cooking
- Smithing
- Runecrafting

## Profession Activity

A selectable action performed inside a Profession.

Examples:

- Cutting a Sproutwood Tree
- Mining an Iron Deposit
- Catching River Fish
- Cooking Bread
- Crafting a Healing Potion

## Profession Tool

A Profession Tool is an equipment item that provides one or more Profession capabilities or bonuses.

A Profession Tool is not a dedicated equipment slot.

Profession Tools are equipped in the Main-Hand or Weapon slot unless the item definition explicitly states otherwise.

Examples:

- Woodcutting Axe
- Mining Pickaxe
- Fishing Rod
- Smithing Hammer
- Archaeology Pick
- Hunting Spear

A Profession Tool may also have Combat stats.

## Profession Support Item

A Profession Support Item is an Offhand item that provides Profession bonuses.

Profession Support Items use the normal Offhand or Shield slot.

Examples:

- Woodcutter's Logbox
- Miner's Ore Satchel
- Angler's Tackle Box
- Herbalist's Pouch
- Archaeologist's Field Journal
- Hunter's Trap Kit

Profession Support Items may provide little or no Combat defense.

## Profession Capability

A Profession Capability is a data tag that allows an equipped item to satisfy an activity requirement.

Examples:

- Woodcutting Tool
- Mining Tool
- Fishing Tool
- Smithing Hammer
- Archaeology Tool
- Hunting Tool

An item may have a Profession Capability without providing large bonuses.

An item may visually resemble a tool but provide no Profession Capability.

Example:

An Iron Battle Axe may have an Axe weapon tag but no Woodcutting Tool capability.

## Profession Modifier

A Profession Modifier changes the result or efficiency of a Profession activity.

Examples:

- +10 Woodcutting Power
- -5% Woodcutting Action Time
- +8% Woodcutting XP
- +10% Log Quantity
- +2% Rare Woodcutting Reward Chance
- +5 Offhand Storage Capacity for Logs

Profession Modifiers may come from:

- Main-hand equipment
- Offhand equipment
- Other equipped gear
- Companions
- Food
- Relics
- Achievements
- Profession levels
- Future account systems

---

# Profession Philosophy

Professions are non-combat progression systems.

Each Profession should provide:

- Resources
- Items
- Experience
- Unlocks
- Progression goals
- Equipment decisions
- Connections to other systems

Professions should not exist in isolation.

Examples:

- Woodcutting provides logs for Carpentry.
- Mining provides ores and gems for Smithing and Jewelcrafting.
- Fishing provides fish for Cooking.
- Herblore provides Healing Potions and Elixirs for Combat.
- Foraging and Farming provide materials for Herblore and Cooking.
- Hunting provides hides and materials for Leatherworking.
- Runecrafting provides Combat runes for Warrior, Ranger, and Mage.

The shared equipment system should reinforce these connections.

A player may equip for:

- Maximum Profession efficiency
- Maximum Combat efficiency
- A hybrid setup
- Convenience while switching between systems

---

# Profession Categories

Current Profession categories:

- Gathering Professions
- Crafting Professions
- Utility Professions

Every Profession should define one Primary Category.

A Profession may also define a Secondary Category when it uses mechanics from more than one category.

Examples:

Archaeology:

- Primary Category: Gathering
- Secondary Category: Utility

Enchanting:

- Primary Category: Crafting
- Secondary Category: Utility

---

# Gathering Professions

Gathering Professions generate raw resources.

Examples:

- Woodcutting
- Mining
- Fishing
- Foraging
- Farming
- Hunting
- Archaeology

Gathering activities should generally require:

- Profession level
- Activity target
- Appropriate equipment capability when required
- Action time or target durability
- Inventory space
- Unlocked Region when applicable

Gathering Professions should feed crafting, Combat preparation, companions, shops, and future systems.

---

# Crafting Professions

Crafting Professions convert resources into useful items.

Examples:

- Cooking
- Smithing
- Carpentry
- Herblore
- Tailoring
- Leatherworking
- Jewelcrafting
- Enchanting
- Runecrafting

Crafting activities should follow the Crafting Framework.

Crafting Professions may create:

- Equipment
- Profession Tools
- Profession Support Items
- Consumables
- Upgrade materials
- Companion materials
- Runes
- Accessories
- Progression items

---

# Utility Professions

Utility Professions provide progression through special activity types.

Examples:

- Thieving
- Archaeology
- Enchanting

Some Utility Professions may gather resources, create items, unlock content, or interact with special mechanics.

Utility Professions should follow the shared Profession structure where possible.

---

# Active Profession Limit

Current starting design:

The player may have one active Profession activity at a time.

Starting a different Profession activity should stop the current Profession activity.

Examples:

- Starting Mining stops Woodcutting.
- Starting Fishing stops Mining.
- Starting Smithing stops Fishing.

Independent timers may continue separately.

Examples:

- Companion rank-up timers
- Farming crop growth timers
- Future research timers
- Future building timers

Future systems may unlock parallel Profession activities if needed.

---

# Shared Equipment Between Combat and Professions

The same equipment loadout is used across Combat and Professions.

The game does not maintain separate hidden Combat and Profession equipment sets by default.

This means:

- Equipping an Iron Axe affects both Woodcutting and Combat.
- Equipping an Iron Battle Axe affects both Combat and Woodcutting.
- Equipping a Woodcutter's Logbox replaces a Shield.
- Equipping a Shield removes the Offhand Profession bonuses from the Logbox.
- Equipping a two-handed item disables the Offhand slot.
- Switching from a Profession to Combat does not automatically change equipment.

This creates strategic trade-offs.

Future loadout presets may allow convenient swapping, but they should still use the same equipment rules.

---

# Profession Loadout

Every Profession activity reads relevant bonuses from the player's current equipped loadout.

The standard Profession loadout includes:

- Main-Hand item
- Offhand item
- Other equipped armor and accessories
- Assigned companion
- Active food buff
- Active relic effect
- Active account bonuses

There is no dedicated Tool slot.

Profession screens should display a Profession Loadout panel rather than a Tool slot.

The Profession Loadout panel should show:

- Equipped Main-Hand item
- Equipped Offhand item
- Relevant Profession capabilities
- Relevant Profession bonuses
- Missing capability warnings
- Handedness conflicts
- Shortcut to Equipment

---

# Main-Hand Equipment Rules

The Main-Hand or Weapon slot may contain:

- Combat weapons
- Profession Tools
- Hybrid weapon-tools
- Two-handed weapons
- Two-handed Profession Tools
- Future utility weapons

Every Main-Hand item may define:

- Combat stats
- Combat Discipline compatibility
- Handedness
- Profession capabilities
- Profession modifiers
- Activity-specific modifiers
- Region-specific modifiers
- Special effects

The Main-Hand item should never require a separate hidden Tool item to function.

---

# Main-Hand Item Roles

## Specialized Profession Tool

A Specialized Profession Tool provides strong Profession bonuses and weak Combat performance.

Example:

Iron Axe

Possible properties:

- Main-Hand
- One-Handed
- Low Combat damage
- Slow or average Combat attack interval
- Woodcutting Tool capability
- High Woodcutting Power
- Woodcutting Action Speed bonus
- Possible Log Quantity bonus

This item is designed primarily for Woodcutting.

## Hybrid Weapon-Tool

A Hybrid Weapon-Tool provides moderate Combat stats and moderate Profession bonuses.

Example:

Forester's War Axe

Possible properties:

- Main-Hand
- One-Handed
- Moderate Combat damage
- Woodcutting Tool capability
- Moderate Woodcutting Power
- Small Woodcutting XP bonus

This item supports convenience and hybrid play.

## Combat Weapon

A Combat Weapon provides strong Combat stats and no Profession bonuses unless explicitly defined.

Example:

Iron Battle Axe

Possible properties:

- Main-Hand
- One-Handed or Two-Handed
- High Combat damage
- No Woodcutting Tool capability
- No Woodcutting Power
- No Log Quantity bonus
- No Woodcutting XP bonus

The item may still visually be an axe.

Visual weapon type does not automatically grant Profession capability.

---

# Base Profession Capability

A Profession may allow basic activities without a matching Profession Tool.

Recommended default:

- Basic activities can be performed using base Profession power.
- Matching Profession Tools improve efficiency.
- Advanced activities may require a specific Profession Capability.
- Profession-specific documents may override this rule.

Example:

A player may cut a basic tree while holding an Iron Battle Axe, but receives no Woodcutting equipment bonuses from it.

A high-level Ancient Ironwood Tree may require the Woodcutting Tool capability.

This allows flexible early progression while preserving meaningful advanced tool requirements.

---

# Offhand Equipment Rules

The Offhand or Shield slot may contain:

- Shields
- Profession Support Items
- Combat utility items
- Hybrid Offhand items
- Future focus items

Every Offhand item may define:

- Combat defense
- Combat resistances
- Profession capabilities
- Profession modifiers
- Storage modifiers
- Reward modifiers
- Special effects

A Profession Support Item competes directly with a Shield.

This creates a clear decision:

- Use a Shield for Combat protection.
- Use a Profession Support Item for Profession efficiency.
- Use a hybrid Offhand item for moderate benefits in both systems.

---

# Profession Support Item Examples

## Woodcutter's Logbox

Equipment Slot:

Offhand

Possible bonuses:

- Increased Woodcutting output
- Chance for bonus logs
- Additional temporary log storage
- Reduced Inventory pressure from logs
- Small Woodcutting XP bonus
- No or very low Combat defense

## Miner's Ore Satchel

Equipment Slot:

Offhand

Possible bonuses:

- Increased ore quantity
- Chance to preserve rare gems
- Additional ore storage
- Reduced Mining action time
- No or very low Combat defense

## Angler's Tackle Box

Equipment Slot:

Offhand

Possible bonuses:

- Increased Fishing success
- Increased bait preservation
- Bonus fish chance
- Increased rare catch chance
- No or very low Combat defense

---

# Two-Handed Equipment Rules

A two-handed Main-Hand item occupies both the Main-Hand and Offhand equipment capacity.

When a two-handed item is equipped:

- The Offhand slot becomes disabled.
- Offhand Profession Support bonuses do not apply.
- Shield bonuses do not apply.
- The UI should show why the Offhand slot is disabled.
- The disabled Offhand item should be safely returned to Inventory when possible.

This rule applies to:

- Two-handed Combat weapons
- Two-handed Profession Tools
- Two-handed hybrid items

A powerful two-handed Profession Tool may provide enough bonuses to compensate for losing an Offhand Profession Support Item.

---

# Equipment Trade-Off Philosophy

Equipment choices should create understandable trade-offs.

Examples:

Iron Axe plus Woodcutter's Logbox:

- Strong Woodcutting efficiency
- Weak Combat performance
- Little Combat defense

Iron Axe plus Shield:

- Strong Woodcutting Main-Hand bonus
- Better Combat defense
- No Logbox bonus

Iron Battle Axe plus Shield:

- Strong Combat performance
- No Woodcutting equipment bonuses

Two-Handed Great Axe:

- Very strong Combat damage
- No Offhand
- No Woodcutting bonus unless explicitly tagged

Two-Handed Lumber Axe:

- Very strong Woodcutting power
- Weak Combat efficiency
- No Offhand Logbox

No loadout should automatically be best for every activity.

---

# Profession Structure

Every Profession should define:

- Profession ID
- Profession Name
- Description
- Icon
- Primary Category
- Secondary Category when applicable
- Current Level
- Current XP
- Maximum Level
- Activity List
- Unlock Rules
- Supported Profession Capabilities
- Supported Main-Hand modifiers
- Supported Offhand modifiers
- Produced Items
- Consumed Items
- Companion Support
- Offline Support
- Statistics
- Achievement Connections
- Collection Connections
- Future Expansion Notes

---

# Profession Activity Structure

Every Profession activity should define:

- Activity ID
- Activity Name
- Description
- Profession ID
- Activity Type
- Required Profession Level
- Required Region when applicable
- Required Main-Hand Capability when applicable
- Required Offhand Capability when applicable
- Required Equipment Tags when applicable
- Forbidden Equipment Tags when applicable
- Required Item when applicable
- Required Unlock when applicable
- Base Action Time
- Target Health or Durability when applicable
- Reward Thresholds when applicable
- Threshold Reward Table when applicable
- XP Grant Rule
- Completion Reward
- Base XP Reward
- Primary Reward
- Reward Quantity
- Secondary Rewards
- Rare Rewards
- Resource Costs when applicable
- Failure Chance when applicable
- Can Run Offline
- Can Repeat Automatically
- Companion Support
- Future Expansion Notes

Activities should be fully data-driven.

The old `Required Tool Slot` concept is removed.

---

# Profession Activity Types

Possible activity types:

- Timed Action
- Target Durability
- Resource Conversion
- Timed Growth
- Chance-Based Action
- Exploration Activity
- Dangerous Activity
- Special Activity

Future activity types may be added later.

---

# Timed Action Activities

A Timed Action completes after a fixed amount of time.

Examples:

- Catch a fish
- Gather herbs
- Cook food
- Craft a potion
- Pickpocket a target

Activity flow:

Select activity

↓

Requirements are checked

↓

Equipped Main-Hand, Offhand, and other modifiers are read

↓

Action timer starts

↓

Action completes

↓

Resources are consumed when required

↓

Rewards and XP are granted

↓

Next action starts automatically or activity stops

Timed Actions should display a clear progress bar.

---

# Target Durability Activities

Some Gathering activities may use targets with Health or Durability.

Examples:

- Trees
- Ore deposits
- Archaeology excavation sites
- Hunting targets when applicable

The player repeatedly performs actions against the selected target.

Each action reduces target Health or Durability.

Profession damage is calculated from:

- Base Profession power
- Main-Hand Profession power
- Offhand Profession modifiers
- Other equipment modifiers
- Profession level modifiers
- Companion bonuses
- Food buffs
- Relic effects
- Future account bonuses

Combat weapon damage is not automatically converted into Profession damage.

Some targets may grant rewards at configured durability thresholds.

Example:

75% durability remaining:

Grant a reward.

50% durability remaining:

Grant a reward.

25% durability remaining:

Grant a reward.

0% durability remaining:

Grant the final reward and complete the target.

Each reward threshold can activate only once during the current target cycle.

Threshold rewards should be defined separately for every target.

Not every target must use the same thresholds.

By default, Profession XP is granted when the target reaches 0 Health or Durability.

Individual activities may grant partial XP at thresholds when explicitly defined in activity data.

When the target reaches 0 Health or Durability:

- The final reward is generated.
- Profession XP is granted.
- Statistics are updated.
- Achievements and collections are updated.
- The target enters its respawn or reset state.
- The activity resumes when the target becomes available again.

This system allows Gathering to feel similar to Combat without becoming Combat.

---

# Target Structure

Every target-based Profession entity should define:

- Target ID
- Name
- Description
- Icon
- Required Profession Level
- Maximum Health or Durability
- Respawn Time
- Base XP Reward
- Primary Reward
- Reward Quantity
- Reward Thresholds
- Threshold Rewards
- XP at Thresholds when applicable
- Completion Reward
- Rare Drop Table
- Required Main-Hand Capability
- Required Offhand Capability when applicable
- Required Equipment Tags when applicable
- Required Region
- Special Mechanics
- Future Expansion Notes

Examples:

- Tree
- Ore Deposit
- Excavation Site
- Hunting Creature
- Special Resource Node

---

# Reward Threshold Rules

Reward thresholds define when a target grants partial rewards.

Possible threshold examples:

- 75% remaining
- 50% remaining
- 25% remaining
- 0% remaining

A threshold should define:

- Trigger Percentage
- Reward Item ID
- Reward Quantity
- Bonus Reward Chance
- XP Reward when applicable
- Can Trigger More Than Once

Default rule:

A threshold can trigger only once per target cycle.

When the target respawns, all thresholds reset.

Threshold rewards should be shown in the activity UI when appropriate.

---

# Profession Action Flow

Default Profession action flow:

Player selects Profession

↓

Player selects activity

↓

Requirements are checked

↓

Current Main-Hand and Offhand equipment are validated

↓

Profession capabilities and modifiers are resolved

↓

Companion, food, relic, and other bonuses are read

↓

Action begins

↓

Progress bar or target durability updates

↓

Threshold rewards activate when applicable

↓

Action completes

↓

Resources are consumed when required

↓

XP is granted

↓

Final rewards are generated

↓

Inventory is updated

↓

Statistics, achievements, and collections are updated

↓

Activity repeats or stops

---

# Activity Start Requirements

Before an activity begins, the system should check:

- Required Profession Level
- Required Main-Hand Capability
- Required Offhand Capability when applicable
- Required Equipment Tags
- Forbidden Equipment Tags
- Required Items
- Required Region
- Required Unlocks
- Inventory Space
- Activity Availability
- Assigned Companion when required
- Two-handed and Offhand conflicts

When requirements are not met, the activity should not begin.

The UI should clearly explain what is missing.

Examples:

- Requires a Woodcutting-capable Main-Hand item.
- Requires Mining Tool capability.
- Requires an Archaeologist's Field Journal in Offhand.
- Cannot use this activity with a two-handed weapon.
- Requires Woodcutting Level 40.

---

# Equipment Changes During Active Professions

Relevant equipment changes must not be exploitable.

Default rule:

Changing the Main-Hand or Offhand item while a Profession activity is active stops the current Profession activity.

The UI should request confirmation.

Example:

Changing your Main-Hand equipment will stop Woodcutting.

Continue?

When the player confirms:

- The current action stops.
- Unfinished progress does not grant rewards.
- Earned rewards remain.
- The equipment change is applied.
- The activity must be manually restarted unless future automation says otherwise.

Changing unrelated equipment may either:

- Recalculate modifiers at the next action boundary
- Or stop the activity when the item affects that Profession

The implementation should prefer the safer rule when uncertainty exists.

---

# Profession Experience

Profession XP is gained by completing Profession activities.

XP rewards may depend on:

- Activity level
- Activity duration
- Activity difficulty
- Resources consumed
- Target durability
- Reward value
- Risk
- Special modifiers

Profession XP should be granted only to the Profession performing the activity.

Examples:

- Cutting trees grants Woodcutting XP.
- Mining ore grants Mining XP.
- Cooking food grants Cooking XP.
- Crafting armor grants Smithing, Tailoring, or Leatherworking XP depending on recipe.
- Creating runes grants Runecrafting XP.

Combat stats on an equipped Profession Tool do not grant Combat Discipline XP during a Profession activity.

Profession damage does not grant Combat Discipline XP.

---

# Profession Level Cap

Current maximum Profession level:

100

Profession levels should unlock:

- New activities
- New resources
- New targets
- New Profession Tools
- New Profession Support Items
- New equipment
- New recipes
- New Regions
- New companions
- New rare drops
- New mechanics
- New passive bonuses

Reaching Level 100 should feel like a major achievement.

Future systems may add progression beyond Level 100 without changing the base level cap.

---

# Activity Tiers

Profession activities may be divided into tiers.

Suggested mapping:

- Tier 0: Levels 1–10
- Tier 1: Levels 11–20
- Tier 2: Levels 21–30
- Tier 3: Levels 31–40
- Tier 4: Levels 41–50
- Tier 5: Levels 51–60
- Tier 6: Levels 61–70
- Tier 7: Levels 71–80
- Tier 8: Levels 81–90
- Tier 9: Levels 91–100

Exact ranges may differ by Profession.

Higher-tier activities should generally provide:

- More XP
- More valuable resources
- More useful crafting materials
- Better rare rewards
- Higher equipment requirements
- More meaningful loadout decisions
- Longer-term progression opportunities

Older activities should remain useful where possible.

---

# Activity Selection

Players should be able to choose which unlocked activity to perform.

Activity selection should show:

- Activity name
- Required level
- Action time
- XP reward
- Primary reward
- Possible threshold rewards
- Possible secondary rewards
- Rare rewards
- Required Main-Hand capability
- Required Offhand capability when applicable
- Region requirement
- Current efficiency
- Estimated XP per hour
- Estimated resources per hour

The player should be able to compare activities easily.

---

# Automatic Repetition

Most Profession activities should repeat automatically.

When an action completes:

- Rewards are granted.
- The next action begins automatically.

Automatic repetition continues until:

- Player stops activity.
- Player changes activity.
- Player changes relevant equipment.
- Inventory becomes full.
- Required resources run out.
- Required Main-Hand capability becomes unavailable.
- Required Offhand capability becomes unavailable.
- Activity becomes unavailable.
- A future stop condition occurs.

Idle progression should not require repeated clicking.

---

# Profession Equipment Bonuses

Profession equipment may provide bonuses through any normal equipment slot.

Possible bonuses:

- Faster action time
- More XP
- More resources
- Rare drop chance
- Reduced material cost
- Bonus output
- Target damage
- Respawn reduction
- Access to special activities
- Temporary Profession storage
- Inventory preservation
- Region-specific bonuses

Equipment bonuses should apply online and offline unless explicitly disabled.

Profession bonuses should be read through a shared modifier system.

Do not hardcode Profession bonuses directly into individual activity scripts.

---

# Profession Modifier Categories

Recommended modifier categories:

- Profession Power
- Action Speed
- XP Gain
- Primary Output
- Secondary Output
- Rare Reward Chance
- Bonus Output Chance
- Resource Preservation
- Resource Cost Reduction
- Respawn Reduction
- Success Chance
- Failure Reduction
- Temporary Storage
- Activity Access
- Region Bonus
- Target-Type Bonus

Every modifier should define:

- Modifier ID
- Profession ID
- Activity Type when limited
- Target Tag when limited
- Flat or percentage value
- Additive or multiplicative behavior
- Online applicability
- Offline applicability
- Stacking rule
- Cap when applicable

---

# Main-Hand Profession Modifiers

Main-Hand Profession modifiers should usually represent direct interaction with the activity.

Examples:

Woodcutting Axe:

- Woodcutting Power
- Woodcutting Action Speed
- Log Quantity
- Tree-specific effects

Mining Pickaxe:

- Mining Power
- Mining Action Speed
- Ore Quantity
- Deposit-specific effects

Fishing Rod:

- Fishing Success
- Fishing Action Speed
- Rare Catch Chance
- Bait Preservation

---

# Offhand Profession Modifiers

Offhand Profession modifiers should usually represent support, storage, preparation, or efficiency.

Examples:

Woodcutter's Logbox:

- Bonus Log Quantity
- Log Storage
- Rare Wood chance
- Reduced Inventory pressure

Miner's Ore Satchel:

- Ore Storage
- Gem preservation
- Bonus ore chance

Herbalist's Pouch:

- Herb quantity
- Herb preservation
- Rare herb chance

Offhand bonuses should not simply duplicate every Main-Hand bonus.

The two slots should feel complementary.

---

# Other Equipment Slots

Other equipment slots may also provide Profession bonuses.

Examples:

- Gloves with Gathering speed
- Boots with Region traversal bonuses
- Rings with XP bonuses
- Amulets with rare reward bonuses
- Capes with mastery bonuses
- Relics with active Profession effects

These items should use the same shared Profession modifier system.

The removal of the dedicated Tool slot does not prevent other equipment slots from supporting Professions.

---

# Profession Companions

Profession companions may support Profession activities.

Possible bonuses:

- Increase Profession XP
- Increase resource gain
- Reduce action time
- Increase rare drop chance
- Reduce resource cost
- Increase bonus output chance
- Unlock special activities
- Improve offline progress

Only the assigned Profession companion should provide bonuses.

Companion selection should be locked while the activity is active unless the player stops the activity first.

Companion bonuses stack with Main-Hand and Offhand Profession bonuses according to shared modifier rules.

---

# Profession Food Buffs

Food may provide temporary Profession bonuses.

Possible effects:

- Increase Profession XP
- Reduce action time
- Increase resource gain
- Increase rare drop chance
- Increase bonus output chance
- Reduce resource cost

Food buffs should have visible durations.

When a food buff expires, the activity continues without the bonus.

Food should not be mandatory for normal Profession progression.

---

# Profession Relics

Relics may provide active or passive Profession bonuses.

Possible effects:

- Increase action speed
- Increase XP
- Increase resources
- Increase rare rewards
- Reduce resource costs
- Unlock special activities

Active relic effects should respect duration and cooldown rules.

Relics should feel powerful but strategic.

---

# Resource Rewards

Profession activities may generate:

- Primary resources
- Secondary resources
- Rare resources
- Crafting materials
- Equipment
- Profession Tools
- Profession Support Items
- Companion rank materials
- Collectibles
- Unlock items
- Gold
- Future progression materials

Every reward should have a clear purpose.

Avoid rewards that exist only as filler.

---

# Primary Rewards

Primary rewards are the normal output of an activity.

Examples:

- Logs from Woodcutting
- Ore from Mining
- Fish from Fishing
- Herbs from Foraging
- Food from Cooking
- Bars from Smithing
- Leather from Leatherworking
- Runes from Runecrafting

Primary rewards should be predictable and clearly shown.

---

# Secondary Rewards

Secondary rewards are additional resources gained occasionally.

Examples:

- Seeds
- Gems
- Rare herbs
- Bones
- Monster parts
- Ancient fragments
- Bonus materials

Secondary rewards should connect Professions to other systems.

---

# Rare Rewards

Rare rewards should feel exciting.

Possible rare rewards:

- Unique resources
- Equipment
- Profession Tools
- Profession Support Items
- Relics
- Companion unlock items
- Rare crafting materials
- Special recipe books
- Collection items
- Region unlock items

Rare rewards should have visible drop chances when appropriate.

Hidden drop chances may be used carefully for discovery-focused content.

---

# Bonus Output

Profession activities may produce bonus output.

Possible sources:

- Main-Hand equipment
- Offhand equipment
- Other equipment
- Companions
- Food buffs
- Relics
- Achievements
- Profession level
- Future account bonuses

Bonus output should be clearly shown in activity results.

---

# Action Speed

Action speed determines how quickly a Profession activity completes.

Action speed may be affected by:

- Base activity time
- Profession level
- Main-Hand equipment
- Offhand equipment
- Other equipment
- Companion
- Food buff
- Relic
- Future modifiers

Action speed should have sensible minimum limits.

Activities should not become so fast that UI updates become unreadable or performance suffers.

---

# Profession Power and Target Damage

Target-based activities may use Profession Power.

Examples:

- Woodcutting Power against trees
- Mining Power against ore deposits
- Excavation Power against archaeology sites

Profession Power may depend on:

- Base Profession value
- Main-Hand Profession Power
- Offhand modifiers
- Profession level
- Other equipment bonuses
- Companion bonuses
- Food buffs
- Relic effects

Combat Attack Damage is separate.

A Combat weapon with high damage provides no Profession Power unless its item data explicitly includes a Profession modifier.

Profession Power does not grant Combat Discipline XP.

---

# Respawn Rules

Target-based activities may have respawn time.

When a target is completed:

- Final rewards are granted.
- XP is granted.
- The target becomes unavailable.
- Respawn timer begins.
- The activity continues automatically when the target returns, provided requirements remain valid.

Respawn time may be affected by:

- Target type
- Equipment bonuses
- Companion bonuses
- Future systems

The player should clearly see when a target is respawning.

---

# Resource Consumption

Some Profession activities consume resources.

Examples:

- Cooking consumes ingredients.
- Smithing consumes ore or bars.
- Herblore consumes herbs.
- Tailoring consumes cloth.
- Leatherworking consumes hides.
- Runecrafting consumes magical materials.
- Enchanting consumes equipment and enchantment materials.

Crafting Profession activities should follow the Crafting Framework.

Required materials are reserved when crafting begins and consumed when crafting completes.

Reserved items cannot be:

- Sold
- Destroyed
- Used by another recipe
- Used for equipment upgrades
- Used for companion rank-ups
- Used by shops

Locked items should not be consumed or reserved.

---

# Failure Rules

Most Profession activities should not fail by default.

Possible exceptions:

- Thieving
- High-risk Enchanting
- Special Hunting activities

Failure may cause:

- No reward
- Reduced reward
- Longer action time
- Temporary activity cooldown
- Minor resource loss

Failure should not create unnecessary frustration.

Basic progression activities should remain reliable.

---

# Runecrafting Activities

Runecrafting creates runes used for Combat preparation.

Runecrafting supports all Combat Disciplines:

- Warrior
- Ranger
- Mage

Runes may provide bonuses such as:

- Damage
- Defense
- Accuracy
- Critical Chance
- Critical Damage
- Attack Speed
- Devotion Regeneration
- Elemental Resistance
- Healing Received
- Rare Drop Chance
- Combat XP Gain
- Special effects from specific rune combinations

The player may activate up to 5 runes before Combat.

Runes are selected before Combat begins.

Once Combat begins, active runes cannot be changed until Combat ends.

Runecrafting activities should create meaningful rune loadout choices for every Combat style.

---

# Rune Structure

Every rune should define:

- Rune ID
- Name
- Description
- Icon
- Combat Bonus
- Supported Combat Disciplines
- Activation Requirement
- Stacking Rules
- Compatible Rune Tags
- Conflicting Rune Tags
- Set Bonus when applicable
- Duration when applicable
- Consumption Rule
- Can Apply Offline
- Future Expansion Notes

The exact Rune consumption rule remains undecided.

The data model should support future consumption rules without rewriting the Runecrafting system.

---

# Rune Loadout Rules

Current design:

- The player may activate up to 5 runes.
- Runes must be selected before Combat begins.
- Active runes are locked during Combat.
- Runes may create set bonuses when specific combinations are active.

Runes should not make every Combat build use the same loadout.

Different Combat Disciplines should have multiple useful rune choices.

---

# Farming Activities

Farming may use timed growth instead of repeated short actions.

Farming activity flow:

Select crop

↓

Check seed requirements

↓

Plant crop

↓

Consume seed

↓

Growth timer begins

↓

Crop finishes growing

↓

Player harvests crop

↓

Rewards and XP are granted

Farming plots may continue growing while another Profession is active.

Farming is an exception to the one active Profession rule because growth timers run independently.

The number of Farming plots may increase through progression.

Farming equipment bonuses may be read at:

- Planting time
- Harvest time
- Or both

Each Farming bonus must clearly define when it is evaluated.

---

# Thieving Activities

Thieving may use chance-based actions.

Every Thieving target should define:

- Required Thieving Level
- Action Time
- Success Chance
- XP Reward
- Gold Reward
- Item Rewards
- Rare Rewards
- Failure Result
- Cooldown when applicable
- Relevant Main-Hand or Offhand capability when applicable

Failure should not be excessively punishing.

Higher-level targets may provide better rewards but lower success chances.

Equipment, companions, and Profession levels may improve success chance.

---

# Archaeology Activities

Archaeology may use excavation sites.

Excavation activities may provide:

- Artifacts
- Ancient materials
- Relics
- Books
- Collection items
- Companion unlock items
- Region lore
- Rare equipment

Excavation sites may use durability similar to trees or ore deposits.

Archaeology equipment may include:

Main-Hand:

- Excavation Pick
- Brush Tool
- Survey Rod

Offhand:

- Field Journal
- Artifact Case
- Survey Map

Archaeology should connect strongly to Collections, relics, crafting, and future Region systems.

---

# Hunting Activities

Hunting may use traps, timed hunts, or target-based actions.

Hunting may require:

- Main-Hand weapon or Hunting Tool
- Offhand Trap Kit
- Bait
- Region
- Hunting level

Hunting rewards may include:

- Hides
- Meat
- Bones
- Monster parts
- Rare trophies
- Companion materials
- Leatherworking materials

Carpentry may create traps used by Hunting.

Hunting should connect to Cooking, Leatherworking, companions, and Combat preparation.

---

# Inventory Interaction

Profession rewards should be added to Inventory.

Before starting or continuing an activity, the system should check Inventory space.

When Inventory becomes full:

- The current Profession activity stops or pauses.
- The player receives a clear notification.
- Important rewards should not be silently lost.

Profession activities should follow the Inventory Framework.

Profession Support Items may provide temporary specialized storage.

Example:

A Woodcutter's Logbox may hold a limited quantity of logs.

Specialized storage rules must define:

- Supported Item Tags
- Capacity
- Whether stored items count toward Inventory capacity
- What happens when the item is unequipped
- What happens when storage is full
- Save behavior
- Offline behavior

Unequipping a storage item must never silently delete its contents.

---

# Offline Profession Progress

Most Profession activities should support offline progress.

Offline progress should use:

- Selected activity
- Time away
- Profession level
- Equipped Main-Hand item
- Equipped Offhand item
- Other equipment bonuses
- Assigned companion
- Valid food buff duration
- Valid relic duration
- Action time
- Target durability when applicable
- Inventory capacity
- Specialized storage capacity
- Resource requirements

Offline Profession progress should stop when:

- Inventory and valid specialized storage become full
- Required resources run out
- Player reaches the offline time limit
- Required equipment capability is missing
- Activity becomes unavailable
- Another stop condition occurs

---

# Offline Equipment Snapshot

Offline simulation should use the equipment that was equipped when the save was created.

The save should record:

- Main-Hand item
- Offhand item
- Other equipped items
- Relevant item instance data
- Profession modifiers
- Specialized storage contents when applicable

Equipment cannot change while the game is closed.

Offline simulation should not automatically choose a better tool or support item.

---

# Offline Target Activities

Target-based Profession activities should calculate:

- Actions performed
- Profession Power
- Target damage dealt
- Reward thresholds reached
- Targets completed
- Respawn time
- XP gained
- Resources gained
- Rare rewards
- Inventory usage
- Specialized storage usage

Threshold rewards should not trigger more than once per target cycle.

Offline calculations should produce results consistent with active gameplay.

---

# Profession Statistics

Every Profession should track:

- Current level
- Current XP
- Total XP gained
- Total actions completed
- Total time spent
- Total resources gained
- Total rare rewards gained
- Highest activity unlocked
- Highest target completed
- Total threshold rewards gained
- Total bonus output
- Total offline actions
- Time using specialized Main-Hand tools
- Time using hybrid equipment
- Time using no matching Profession capability
- Profession-specific statistics

Examples:

- Trees cut
- Ore deposits mined
- Fish caught
- Food cooked
- Items pickpocketed
- Crops harvested
- Artifacts discovered
- Animals hunted
- Runes created

---

# Profession Achievements

Profession achievements may require:

- Reach Profession levels
- Complete activity counts
- Gather resource quantities
- Find rare rewards
- Use specific Main-Hand tools
- Use Profession Support Items
- Complete activities with no Profession equipment bonus
- Complete high-level activities
- Reach Level 100
- Complete Profession Collections

Achievements should reward long-term Profession progression.

---

# Profession Collections

Profession activities may contribute to Collections.

Possible Collection entries:

- Resources
- Rare materials
- Profession Tools
- Profession Support Items
- Artifacts
- Fish
- Seeds
- Runes
- Crafted items
- Special activity rewards

First-time discoveries should update the Collection Log.

Collection discovery remains even when the item is later consumed, sold, destroyed, equipped, or unequipped.

---

# Profession User Interface

Every Profession screen should display:

- Profession icon
- Profession name
- Current level
- Current XP
- XP progress bar
- Current activity
- Activity list
- Activity requirements
- Action progress
- Target Health or Durability when applicable
- Reward thresholds when applicable
- Respawn timer when applicable
- Expected rewards
- XP per action
- Estimated XP per hour
- Estimated resources per hour
- Profession Loadout
- Equipped Main-Hand item
- Equipped Offhand item
- Relevant Main-Hand bonuses
- Relevant Offhand bonuses
- Missing capability warnings
- Assigned companion
- Active food buff
- Active relic effect
- Start button
- Stop button
- Offline support status
- Statistics button

The player should understand exactly what the Profession is doing.

Do not display a dedicated Tool slot.

---

# Profession Loadout UI

The Profession Loadout panel should contain:

## Main-Hand

Display:

- Item icon
- Item name
- Handedness
- Profession capability
- Relevant Profession bonuses
- Combat summary when useful
- Empty or no-bonus state
- Equipment shortcut

## Offhand

Display:

- Item icon
- Item name
- Shield or Profession Support role
- Relevant Profession bonuses
- Combat defense summary when useful
- Disabled-by-two-handed state
- Equipment shortcut

## Combined Profession Bonuses

Display:

- Final Profession Power
- Final Action Speed
- Final XP bonus
- Final resource bonus
- Final rare reward bonus
- Specialized storage
- Other active modifiers

The panel should clearly distinguish:

- Main-Hand bonuses
- Offhand bonuses
- Other equipment bonuses
- Temporary bonuses

---

# Equipment Tooltip Requirements

Equipment tooltips should display both Combat and Profession information.

Example Iron Axe tooltip:

Combat:

- Damage
- Attack Interval
- Accuracy
- Handedness

Woodcutting:

- Woodcutting Tool capability
- Woodcutting Power
- Action Speed
- Bonus Logs

Example Iron Battle Axe tooltip:

Combat:

- High Damage
- Attack Interval
- Accuracy
- Handedness

Woodcutting:

- No Woodcutting bonuses

Example Woodcutter's Logbox tooltip:

Combat:

- No Defense or low Defense

Woodcutting:

- Bonus Log Quantity
- Log Storage
- Rare Reward bonus

The player should never need to guess whether a weapon helps a Profession.

---

# Activity Card UI

Every activity card should display:

- Activity icon
- Activity name
- Required level
- Locked or unlocked status
- Action time
- Target Health or Durability when applicable
- Reward thresholds when applicable
- XP reward
- Primary reward
- Possible secondary rewards
- Possible rare rewards
- Required Main-Hand capability
- Required Offhand capability when applicable
- Region requirement
- Start button

Locked activities should explain their requirements.

---

# Active Activity Panel

The active activity panel should display:

- Current Profession
- Current activity
- Progress bar
- Current target
- Target Health or Durability
- Next reward threshold
- Time until action completes
- Time until respawn
- Current XP gain
- Current reward estimate
- Main-Hand bonuses
- Offhand bonuses
- Companion bonuses
- Food bonuses
- Relic bonuses
- Stop button

The panel should update clearly without excessive visual clutter.

---

# Equipment Screen Requirements

The Equipment screen should not contain a dedicated Tool slot.

Profession Tools should appear in the normal Main-Hand or Weapon slot.

Profession Support Items should appear in the normal Offhand or Shield slot.

The Equipment screen should:

- Display Combat stats
- Display relevant Profession bonuses
- Show two-handed conflicts
- Show whether an item has Profession capabilities
- Compare both Combat and Profession values
- Allow filtering by Profession compatibility
- Show which active Profession would be stopped by the change
- Require confirmation when changing relevant equipment during an active Profession

---

# Profession Notifications

Profession notifications may show:

- Activity started
- Activity stopped
- Activity stopped because equipment changed
- Inventory full
- Specialized storage full
- Required resources depleted
- Required capability missing
- Profession level gained
- New activity unlocked
- New Profession Tool unlocked
- New Profession Support Item unlocked
- Threshold reward gained
- Rare reward found
- Collection item discovered
- Achievement completed
- Food buff expired
- Relic effect expired

Notifications should be useful but not annoying.

---

# Activity Database

Every Profession activity should exist inside a Profession Activity Database.

Activities should never be hardcoded.

Systems should reference Activity IDs.

Activity data should connect to:

- Profession Database
- Item Database
- Equipment Database
- Modifier Database
- Companion Database
- Region Database
- Achievement System
- Collection System
- Inventory System
- Save System
- Offline Progress System

---

# Profession Database

Every Profession should exist inside the Profession Database.

The Profession Database should define:

- Profession ID
- Name
- Description
- Icon
- Primary Category
- Secondary Category when applicable
- Maximum Level
- Activity List
- Base Unlocks
- Supported Main-Hand capabilities
- Supported Offhand capabilities
- Supported Profession modifiers
- Supported companions
- Offline Support
- Future Expansion Notes

The Profession Database contains static Profession data.

Player levels and XP belong in save data.

---

# Equipment Data Requirements

Every equippable Main-Hand or Offhand item should support both Combat and Profession data.

Recommended equipment fields:

- Item ID
- Display Name
- Equip Slot
- Handedness
- Combat Discipline compatibility
- Combat stats
- Profession capabilities
- Profession modifiers
- Activity tags
- Target tags
- Region tags
- Specialized storage definition
- Special effects
- Offline applicability
- Future Expansion Notes

Profession capability and Profession modifier data should be independent from visual weapon category.

An axe visual does not automatically provide Woodcutting bonuses.

---

# Profession Modifier Resolver

A shared Profession Modifier Resolver should calculate the active Profession modifiers.

Inputs may include:

- Profession ID
- Activity ID
- Target tags
- Region ID
- Main-Hand item
- Offhand item
- Other equipped items
- Companion
- Food
- Relic
- Achievement bonuses
- Account bonuses

Outputs may include:

- Final Profession Power
- Final Action Time
- Final XP multiplier
- Final reward multiplier
- Final rare reward chance
- Final preservation chance
- Final success chance
- Final specialized storage
- Requirement validation result
- Modifier breakdown for UI

The UI should display the resolver result.

The UI should not calculate final Profession values independently.

---

# Save Data Changes

Save data should store:

- Main-Hand equipped item
- Offhand equipped item
- Other equipped items
- No dedicated Tool slot
- Specialized storage contents when applicable
- Active Profession
- Selected activity
- Equipment-dependent activity state
- Relevant timers
- Relevant target state
- Offline timestamp

The save system should not maintain a separate permanent Profession Tool equipment reference after migration.

---

# Migration From Dedicated Tool Slot

Existing saves or prototype data may contain a dedicated Tool slot.

A migration is required.

Recommended migration rules:

1. Read the old Tool slot item.
2. Remove the old Tool slot from the active equipment model.
3. When Main-Hand is empty, equip the old Tool item in Main-Hand when compatible.
4. When Main-Hand is occupied, move the old Tool item to Inventory.
5. When Inventory is full, move the old Tool item to protected claim storage.
6. Never silently delete the old Tool item.
7. Recalculate Profession and Combat stats.
8. Save using the new version.
9. Record that the migration has completed.

Old UI references to the Tool slot should be removed or redirected to Main-Hand.

---

# Technical Rules

Profession systems should be data-driven.

Profession activities should reference Activity IDs.

Profession rewards should reference Item IDs.

Profession requirements should reference database IDs and equipment capability tags.

Profession scripts should not hardcode:

- Activity names
- Rewards
- Requirements
- Main-Hand item names
- Offhand item names
- Profession modifiers
- Combat stats

Profession activities should work online and offline.

Reward thresholds should be defined in activity data.

Equipment, companion, food, relic, achievement, and Region modifiers should use shared modifier systems.

Adding a new Profession activity should not require rewriting Profession code.

Adding a new Profession Tool should not require creating a new equipment slot.

Adding a new Profession Support Item should not require creating a new equipment slot.

Individual Professions may use specialized activity behavior through modular activity types.

---

# Codex Implementation Rules

Codex must:

- Remove the dedicated Tool slot from Profession and Equipment UI.
- Use Main-Hand for Profession Tools.
- Use Offhand for Profession Support Items.
- Preserve Combat stats on Profession Tools.
- Preserve Profession modifiers on hybrid items.
- Allow Combat weapons to have zero Profession bonuses.
- Resolve Profession bonuses from equipped items.
- Show both Combat and Profession data in tooltips.
- Validate two-handed and Offhand conflicts.
- Stop active Profession activity when relevant equipment changes.
- Preserve all old Tool items during save migration.
- Update offline simulation to use equipped Main-Hand and Offhand items.
- Keep Profession Power separate from Combat Attack Damage.
- Keep Profession XP separate from Combat Discipline XP.
- Keep the system data-driven.

Codex must not:

- Recreate a hidden dedicated Tool slot.
- Automatically treat every axe as a Woodcutting Tool.
- Automatically treat every pick-shaped weapon as a Mining Tool.
- Grant Profession bonuses from visual weapon type alone.
- Convert Combat Damage directly into Profession Power.
- Automatically swap equipment when entering a Profession.
- Automatically swap equipment when entering Combat.
- Delete old Tool-slot items during migration.
- Allow a two-handed item and an Offhand item to provide bonuses simultaneously.
- Put final Profession calculations inside UI scripts.

---

# Balance Philosophy

Profession progression should reward long-term planning.

The player should make meaningful decisions:

- Equip a specialized Profession Tool or a stronger Combat weapon?
- Use an Offhand Profession Support Item or a Shield?
- Use a one-handed tool with an Offhand bonus or a stronger two-handed tool?
- Train a faster XP activity or gather useful resources?
- Gather basic resources or chase rare drops?
- Sell resources or use them for crafting?
- Use a food buff now or save it?
- Train a new Profession or continue a higher-level one?
- Choose a fast target or a high-durability target with more threshold rewards?

Specialized Profession equipment should clearly outperform Combat equipment inside its intended Profession.

Combat equipment should clearly outperform specialized Profession equipment inside Combat.

Hybrid equipment should provide convenience without being best at both.

Older resources and equipment should remain useful where possible.

Avoid making every new item a direct replacement for everything before it.

---

# Future Expansion

Possible additions:

- Profession loadout presets
- Automatic loadout switching outside active activities
- Profession specializations
- Profession mastery
- Profession prestige
- Parallel Profession slots
- Automation upgrades
- Advanced activity types
- Profession talent trees
- Profession contracts
- Profession events
- Regional Profession bonuses
- Guild Profession bonuses
- Profession challenges
- Shared Profession activities
- Additional reward threshold types
- More specialized Offhand support items
- Hybrid Combat and Profession equipment sets

Future systems should expand this framework rather than replace it.

---

# Checklist

## Every Profession Should Answer

✓ What category is this Profession?

✓ Does it have a Secondary Category?

✓ What resources does it produce?

✓ What resources does it consume?

✓ Which systems use its rewards?

✓ Which Main-Hand capabilities support it?

✓ Which Offhand support items support it?

✓ Which other equipment slots may provide bonuses?

✓ Which companions support it?

✓ What activities does it contain?

✓ What unlocks at each level range?

✓ Does it support offline progress?

✓ What happens when Inventory is full?

✓ Does it connect to crafting?

✓ Does it connect to Combat?

✓ Does it connect to companions?

✓ Does it update achievements?

✓ Does it update Collections?

✓ Does it save and load correctly?

✓ Can it support future expansion?

---

## Every Profession Activity Should Answer

✓ What Profession owns this activity?

✓ What type of activity is it?

✓ What level is required?

✓ Does it require a Main-Hand capability?

✓ Does it require an Offhand capability?

✓ Are any equipment tags forbidden?

✓ How long does it take?

✓ Does it use target Health or Durability?

✓ Does it use reward thresholds?

✓ What XP does it grant?

✓ What rewards does it provide?

✓ What rare rewards can appear?

✓ Can it repeat automatically?

✓ Can it run offline?

✓ What stops the activity?

✓ What happens when Main-Hand or Offhand equipment changes?

---

## Every Main-Hand Profession Item Should Answer

✓ Which equipment slot does it use?

✓ Is it one-handed or two-handed?

✓ What Combat stats does it provide?

✓ Which Profession capabilities does it provide?

✓ Which Profession modifiers does it provide?

✓ Does it support one Profession or multiple Professions?

✓ Does it work offline?

✓ Does it conflict with an Offhand item?

✓ Is it specialized, hybrid, or Combat-focused?

---

## Every Offhand Profession Support Item Should Answer

✓ Which Profession does it support?

✓ What Profession modifiers does it provide?

✓ Does it provide Combat defense?

✓ Does it provide specialized storage?

✓ What happens to stored items when unequipped?

✓ Is it disabled by two-handed equipment?

✓ Does it work offline?

✓ Does it require a specific Main-Hand capability?

---

## Every Target-Based Activity Should Answer

✓ What is the target's maximum Health or Durability?

✓ What reward thresholds does it use?

✓ What does each threshold reward?

✓ Can each threshold trigger only once?

✓ When is XP granted?

✓ What is the completion reward?

✓ How long does the target take to respawn?

✓ Which Profession Power affects it?

✓ Does it require a specific Main-Hand capability?

---

## Every Equipment Migration Should Answer

✓ Is the old Tool slot removed?

✓ Is the old Tool item preserved?

✓ Can it move safely to Main-Hand?

✓ Can it move safely to Inventory?

✓ Is protected claim storage used when Inventory is full?

✓ Are Profession and Combat stats recalculated?

✓ Is migration completion saved?

---

## Every Rune Should Answer

✓ Which Combat Disciplines does it support?

✓ What bonus does it provide?

✓ Is it consumed?

✓ Does it have a duration?

✓ Can it stack?

✓ Does it conflict with other runes?

✓ Does it contribute to a Rune set bonus?

✓ Does it work during offline Combat?
