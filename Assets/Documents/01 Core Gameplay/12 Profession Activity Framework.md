# 12 Profession Activity Framework

Version: 1.0
Status: Draft

---

# Purpose

This document defines how profession activities work across the entire game.

It establishes the shared structure, behavior, progression rules, activity flow, rewards, requirements, offline rules, and user interface used by all professions.

Individual profession documents may add unique mechanics, but they should follow this framework unless explicitly designed otherwise.

---

# Design Goals

Profession activities should:

- Be easy to understand.
- Provide meaningful long-term progression.
- Connect to items, equipment, combat, crafting, companions, achievements, and collections.
- Support both active and offline progression.
- Reward better tools, higher profession levels, and preparation.
- Provide meaningful activity choices.
- Avoid becoming simple number-only progression.
- Be fully data-driven.
- Allow individual professions to have unique identities.
- Remain expandable for future content.

Every profession should contribute to the wider game economy.

---

# Profession Philosophy

Professions are non-combat progression systems.

Professions have a maximum level of 100.

Each profession should provide:

Resources

Items

Experience

Unlocks

Progression goals

Connections to other systems

Professions should not exist in isolation.

Examples:

Woodcutting provides logs for Carpentry.

Mining provides ores and gems for Smithing and Jewelcrafting.

Fishing provides fish for Cooking.

Herblore provides potions and elixirs for combat.

Foraging and Farming provide materials for Herblore and Cooking.

Hunting provides hides and materials for Leatherworking.

Runecrafting provides combat runes for Warrior, Ranger, and Mage.

---

# Profession Categories

Current profession categories:

Gathering Professions

Crafting Professions

Utility Professions

Every profession should define one Primary Category.

A profession may also define a Secondary Category when it uses mechanics from more than one category.

Examples:

Archaeology:

Primary Category: Gathering

Secondary Category: Utility

Enchanting:

Primary Category: Crafting

Secondary Category: Utility

Some professions may combine more than one category.

---

# Gathering Professions

Gathering professions generate raw resources.

Examples:

Woodcutting

Mining

Fishing

Foraging

Farming

Hunting

Archaeology

Gathering activities should generally require:

Profession level

Activity target

Tool if applicable

Action time or target durability

Inventory space

Unlocked region if applicable

Gathering professions should feed crafting, combat preparation, companions, shops, and future systems.

---

# Crafting Professions

Crafting professions convert resources into useful items.

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

Crafting activities should follow the Crafting Framework.

Crafting professions may create:

Equipment

Tools

Consumables

Upgrade materials

Companion materials

Runes

Accessories

Progression items

---

# Utility Professions

Utility professions provide progression through special activity types.

Examples:

Thieving

Archaeology

Enchanting

Some utility professions may gather resources, create items, unlock content, or interact with special mechanics.

Utility professions should still follow the shared profession structure where possible.

---

# Active Profession Limit

Current starting design:

The player may have one active profession activity at a time.

Starting a different profession activity should stop the current profession activity.

Examples:

Starting Mining stops Woodcutting.

Starting Fishing stops Mining.

Starting Smithing stops Fishing.

Independent timers may continue separately.

Examples:

Companion rank-up timers

Farming crop growth timers

Future research timers

Future building timers

Future systems may unlock parallel profession activities if needed.

---

# Profession Structure

Every profession should define:

Profession ID

Profession Name

Description

Icon

Primary Category

Secondary Category if applicable

Current Level

Current XP

Maximum Level

Activity List

Unlock Rules

Required Equipment

Produced Items

Consumed Items

Companion Support

Offline Support

Statistics

Achievement Connections

Collection Connections

Future Expansion Notes

---

# Profession Activity Structure

Every profession activity should define:

Activity ID

Activity Name

Description

Profession ID

Activity Type

Required Profession Level

Required Region if applicable

Required Tool if applicable

Required Equipment if applicable

Required Item if applicable

Required Unlock if applicable

Base Action Time

Target Health or Durability if applicable

Reward Thresholds if applicable

Threshold Reward Table if applicable

XP Grant Rule

Completion Reward

Base XP Reward

Primary Reward

Reward Quantity

Secondary Rewards

Rare Rewards

Resource Costs if applicable

Failure Chance if applicable

Can Run Offline

Can Repeat Automatically

Companion Support

Future Expansion Notes

Activities should be fully data-driven.

---

# Profession Activity Types

Possible activity types:

Timed Action

Target Durability

Resource Conversion

Timed Growth

Chance-Based Action

Exploration Activity

Dangerous Activity

Special Activity

Future activity types may be added later.

---

# Timed Action Activities

A Timed Action completes after a fixed amount of time.

Examples:

Catch a fish

Gather herbs

Cook food

Craft a potion

Pickpocket a target

Activity flow:

Select activity

↓

Requirements are checked

↓

Action timer starts

↓

Action completes

↓

Resources are consumed if required

↓

Rewards and XP are granted

↓

Next action starts automatically or activity stops

Timed Actions should display a clear progress bar.

---

# Target Durability Activities

Some gathering activities may use targets with Health or Durability.

Examples:

Trees

Ore deposits

Archaeology excavation sites

Hunting targets if applicable

The player repeatedly performs actions against the selected target.

Each action reduces target Health or Durability.

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

By default, profession XP is granted when the target reaches 0 Health or Durability.

Individual activities may grant partial XP at thresholds if explicitly defined in activity data.

When the target reaches 0 Health or Durability:

The final reward is generated.

Profession XP is granted.

Statistics are updated.

Achievements and collections are updated.

The target enters its respawn or reset state.

The activity resumes when the target becomes available again.

This system allows gathering to feel similar to combat without becoming combat.

---

# Target Structure

Every target-based profession entity should define:

Target ID

Name

Description

Icon

Required Profession Level

Maximum Health or Durability

Respawn Time

Base XP Reward

Primary Reward

Reward Quantity

Reward Thresholds

Threshold Rewards

XP at Thresholds if applicable

Completion Reward

Rare Drop Table

Required Tool Type

Required Region

Special Mechanics

Future Expansion Notes

Examples:

Tree

Ore Deposit

Excavation Site

Hunting Creature

Special Resource Node

---

# Reward Threshold Rules

Reward thresholds define when a target grants partial rewards.

Possible threshold examples:

75% remaining

50% remaining

25% remaining

0% remaining

A threshold should define:

Trigger Percentage

Reward Item ID

Reward Quantity

Bonus Reward Chance

XP Reward if applicable

Can Trigger More Than Once

Default rule:

A threshold can trigger only once per target cycle.

When the target respawns, all thresholds reset.

Threshold rewards should be shown in the activity UI when appropriate.

---

# Profession Action Flow

Default profession action flow:

Player selects profession

↓

Player selects activity

↓

Requirements are checked

↓

Equipment, tool, companion, and bonuses are read

↓

Action begins

↓

Progress bar or target durability updates

↓

Threshold rewards activate if applicable

↓

Action completes

↓

Resources are consumed if required

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

Required Profession Level

Required Tool

Required Equipment

Required Items

Required Region

Required Unlocks

Inventory Space

Activity Availability

Assigned Companion if required

If requirements are not met, the activity should not begin.

The UI should clearly explain what is missing.

---

# Profession Experience

Profession XP is gained by completing profession activities.

XP rewards may depend on:

Activity level

Activity duration

Activity difficulty

Resources consumed

Target durability

Reward value

Risk

Special modifiers

Profession XP should be granted only to the profession performing the activity.

Examples:

Cutting trees grants Woodcutting XP.

Mining ore grants Mining XP.

Cooking food grants Cooking XP.

Crafting armor grants Smithing, Tailoring, or Leatherworking XP depending on recipe.

Creating runes grants Runecrafting XP.

---

# Profession Level Cap

Current maximum profession level:

100

Profession levels should unlock new activities, resources, tools, regions, recipes, and mechanics.

Reaching level 100 should feel like a major achievement.

Future systems may add progression beyond level 100 without changing the base level cap.

Possible future systems:

Prestige

Paragon progression

Specializations

Profession mastery

Post-level-cap unlock tracks

---

# Profession Unlocks

Profession levels may unlock:

New activities

New resources

New targets

New tools

New equipment

New recipes

New regions

New companions

New rare drops

New mechanics

New passive bonuses

Unlocks should feel meaningful.

Avoid profession levels that only increase numbers without unlocking anything interesting.

---

# Activity Tiers

Profession activities may be divided into tiers.

Example:

Tier 0:

Levels 1–10

Tier 1:

Levels 11–20

Tier 2:

Levels 21–30

Tier 3:

Levels 31–40

Tier 4:

Levels 41–50

Tier 5:

Levels 51–60

Tier 6:

Levels 61–70

Tier 7:

Levels 71–80

Tier 8:

Levels 81–90

Tier 9:

Levels 91–100

Exact tier ranges may differ by profession.

Higher-tier activities should generally provide:

More XP

More valuable resources

More useful crafting materials

Better rare rewards

Higher requirements

Longer-term progression opportunities

Older activities should remain useful where possible.

---

# Activity Selection

Players should be able to choose which unlocked activity to perform.

Activity selection should show:

Activity name

Required level

Action time

XP reward

Primary reward

Possible threshold rewards

Possible secondary rewards

Rare rewards

Tool requirement

Region requirement

Current efficiency

Estimated XP per hour

Estimated resources per hour

The player should be able to compare activities easily.

---

# Automatic Repetition

Most profession activities should repeat automatically.

When an action completes:

Rewards are granted.

The next action begins automatically.

Automatic repetition continues until:

Player stops activity

Player changes activity

Inventory becomes full

Required resources run out

Required tool becomes unavailable

Activity becomes unavailable

Future stop condition occurs

Idle progression should not require repeated clicking.

---

# Tools

Some profession activities require tools.

Examples:

Axe for Woodcutting

Pickaxe for Mining

Fishing Rod for Fishing

Knife for Leatherworking

Needle for Tailoring

Hammer for Smithing

Saw for Carpentry

Tools may affect:

Action speed

Target damage

Resource gain

XP gain

Rare drop chance

Bonus output chance

Special activity access

Tools should improve profession efficiency without replacing profession levels.

---

# Profession Equipment

Profession equipment may provide bonuses beyond the main tool.

Possible bonuses:

Faster action time

More XP

More resources

Rare drop chance

Reduced material cost

Bonus output

Access to special activities

Profession equipment should follow the Equipment Framework.

Equipment bonuses should apply online and offline unless explicitly disabled.

---

# Profession Companions

Profession companions may support profession activities.

Possible bonuses:

Increase profession XP

Increase resource gain

Reduce action time

Increase rare drop chance

Reduce resource cost

Increase bonus output chance

Unlock special activities

Improve offline progress

Only the assigned profession companion should provide bonuses.

Companion selection should be locked while the activity is active unless the player stops the activity first.

---

# Profession Food Buffs

Food may provide temporary profession bonuses.

Possible effects:

Increase profession XP

Reduce action time

Increase resource gain

Increase rare drop chance

Increase bonus output chance

Reduce resource cost

Food buffs should have visible durations.

If a food buff expires, the activity continues without the bonus.

Food should not be mandatory for normal profession progression.

---

# Profession Relics

Relics may provide active or passive profession bonuses.

Possible effects:

Increase action speed

Increase XP

Increase resources

Increase rare rewards

Reduce resource costs

Unlock special activities

Active relic effects should respect duration and cooldown rules.

Relics should feel powerful but strategic.

---

# Resource Rewards

Profession activities may generate:

Primary resources

Secondary resources

Rare resources

Crafting materials

Equipment

Companion rank materials

Collectibles

Unlock items

Gold

Future progression materials

Every reward should have a clear purpose.

Avoid rewards that exist only as filler.

---

# Primary Rewards

Primary rewards are the normal output of an activity.

Examples:

Logs from Woodcutting

Ore from Mining

Fish from Fishing

Herbs from Foraging

Food from Cooking

Bars from Smithing

Leather from Leatherworking

Runes from Runecrafting

Primary rewards should be predictable and clearly shown.

---

# Secondary Rewards

Secondary rewards are additional resources gained occasionally.

Examples:

Seeds

Gems

Rare herbs

Bones

Monster parts

Ancient fragments

Bonus materials

Secondary rewards should connect professions to other systems.

---

# Rare Rewards

Rare rewards should feel exciting.

Possible rare rewards:

Unique resources

Equipment

Relics

Companion unlock items

Rare crafting materials

Special recipe books

Collection items

Region unlock items

Rare rewards should have visible drop chances when appropriate.

Hidden drop chances may be used carefully for discovery-focused content.

---

# Bonus Output

Profession activities may produce bonus output.

Possible sources:

Tools

Equipment

Companions

Food buffs

Relics

Achievements

Profession level

Future account bonuses

Bonus output should be clearly shown in activity results.

---

# Action Speed

Action speed determines how quickly a profession activity completes.

Action speed may be affected by:

Base activity time

Profession level

Tool

Equipment

Companion

Food buff

Relic

Future modifiers

Action speed should have sensible minimum limits.

Activities should not become so fast that UI updates become unreadable or performance suffers.

---

# Target Damage

Target-based activities may use profession damage.

Examples:

Axe damage against trees

Pickaxe damage against ore deposits

Excavation power against archaeology sites

Target damage may depend on:

Tool power

Profession level

Equipment bonuses

Companion bonuses

Food buffs

Relic effects

Target damage should remain separate from combat damage.

Profession damage should not grant Combat Discipline XP.

---

# Respawn Rules

Target-based activities may have respawn time.

When a target is completed:

Final rewards are granted.

XP is granted.

Target becomes unavailable.

Respawn timer begins.

When respawn completes, activity continues automatically if still selected.

Respawn time may be affected by:

Target type

Profession bonuses

Companion bonuses

Future systems

The player should clearly see when a target is respawning.

---

# Resource Consumption

Some profession activities consume resources.

Examples:

Cooking consumes ingredients.

Smithing consumes ore or bars.

Herblore consumes herbs.

Tailoring consumes cloth.

Leatherworking consumes hides.

Runecrafting consumes magical materials.

Enchanting consumes equipment and enchantment materials.

Crafting profession activities should follow the Crafting Framework.

Required materials are reserved when crafting begins and consumed when crafting completes.

Reserved items cannot be:

Sold

Destroyed

Used by another recipe

Used for equipment upgrades

Used for companion rank-ups

Used by shops

Locked items should not be consumed or reserved.

---

# Failure Rules

Most profession activities should not fail by default.

Possible exceptions:

Thieving

High-risk Enchanting

Failure may cause:

No reward

Reduced reward

Longer action time

Temporary activity cooldown

Minor resource loss

Failure should not create unnecessary frustration.

Basic progression activities should remain reliable.

---

# Runecrafting Activities

Runecrafting creates runes used for combat preparation.

Runecrafting supports all Combat Disciplines:

Warrior

Ranger

Mage

Runes may provide bonuses such as:

Damage

Defense

Accuracy

Critical Chance

Critical Damage

Attack Speed

Devotion Regeneration

Elemental Resistance

Healing Received

Rare Drop Chance

Combat XP Gain

Special effects from specific rune combinations

The player may activate up to 5 runes before combat.

Runes are selected before combat begins.

Once combat begins, active runes cannot be changed until combat ends.

Runecrafting activities should create meaningful rune loadout choices for every combat style.

---

# Rune Structure

Every rune should define:

Rune ID

Name

Description

Icon

Combat Bonus

Supported Combat Disciplines

Activation Requirement

Stacking Rules

Compatible Rune Tags

Conflicting Rune Tags

Set Bonus if applicable

Duration if applicable

Consumption Rule

Can Apply Offline

Future Expansion Notes

Runes may support:

One Combat Discipline

Multiple Combat Disciplines

All Combat Disciplines

The UI should clearly show which bonuses come from individual runes and which bonuses come from rune combinations.

---

# Rune Loadout Rules

Current design:

The player may activate up to 5 runes.

Runes must be selected before combat begins.

Active runes are locked during combat.

Runes may create set bonuses when specific combinations are active.

Example rune set effects:

2 matching runes activate a minor set bonus.

3 matching runes activate a stronger set bonus.

5 specific runes activate a unique effect.

Exact set rules should be defined in Runecrafting data.

Runes should not make every combat build use the same loadout.

Different Combat Disciplines should have multiple useful rune choices.

---

# Rune Consumption Rules

Every rune should define how it is used.

Possible rune behavior:

Permanently active until changed

Consumed when combat begins

Consumed after a number of attacks

Consumed after a duration

Consumed after an encounter

Not consumed but limited by cooldown

The final rule may differ between rune types.

The Runecrafting document should define exact consumption behavior.

Rune consumption should be clearly shown before combat starts.

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

Farming plots may continue growing while another profession is active.

Farming is an exception to the one active profession rule because growth timers run independently.

The number of farming plots may increase through progression.

---

# Farming Plot Structure

Every Farming plot should define:

Plot ID

Unlocked Status

Unlock Requirement

Current Crop

Planting Time

Growth End Time

Ready to Harvest Status

Assigned Farming Bonus if applicable

Future Expansion Notes

Every crop should define:

Crop ID

Seed Requirement

Required Farming Level

Growth Time

Harvest Reward

Harvest Quantity

XP Reward

Possible Rare Reward

Future Expansion Notes

---

# Thieving Activities

Thieving may use chance-based actions.

Every Thieving target should define:

Required Thieving Level

Action Time

Success Chance

XP Reward

Gold Reward

Item Rewards

Rare Rewards

Failure Result

Cooldown if applicable

Failure should not be excessively punishing.

Higher-level targets may provide better rewards but lower success chances.

Equipment, companions, and profession levels may improve success chance.

---

# Thieving Failure Rules

Thieving failure may cause:

No reward

Reduced XP

Temporary cooldown

Minor gold loss

Short activity delay

Failure should not remove important items or cause harsh permanent penalties.

Success chance should always be visible to the player.

---

# Archaeology Activities

Archaeology may use excavation sites.

Excavation activities may provide:

Artifacts

Ancient materials

Relics

Books

Collection items

Companion unlock items

Region lore

Rare equipment

Excavation sites may use durability similar to trees or ore deposits.

Archaeology should connect strongly to collections, the museum, relics, crafting, and future region systems.

---

# Archaeology Site Structure

Every Archaeology site should define:

Site ID

Name

Description

Required Archaeology Level

Required Tool

Durability

Respawn Time

Artifact Table

Material Table

Rare Reward Table

Region

Collection Connections

Museum Connections

Future Expansion Notes

---

# Hunting Activities

Hunting may use traps, timed hunts, or target-based actions.

Hunting may require:

Trap

Weapon or hunting tool

Bait

Region

Hunting level

Hunting rewards may include:

Hides

Meat

Bones

Monster parts

Rare trophies

Companion materials

Leatherworking materials

Carpentry may create traps used by Hunting.

Hunting should connect to Cooking, Leatherworking, companions, and combat preparation.

---

# Hunting Activity Types

Possible Hunting activity types:

Trap Hunt

Timed Hunt

Target Durability Hunt

Baited Hunt

Tracking Activity

Special Creature Hunt

Every Hunting activity should define which activity type it uses.

---

# Inventory Interaction

Profession rewards should be added to inventory.

Before starting or continuing an activity, the system should check inventory space.

If inventory becomes full:

The current profession activity stops or pauses.

The player receives a clear notification.

Important rewards should not be silently lost.

Profession activities should follow the Inventory Framework.

---

# Offline Profession Progress

Most profession activities should support offline progress.

Offline progress should use:

Selected activity

Time away

Profession level

Equipped tool

Equipment bonuses

Assigned companion

Valid food buff duration

Valid relic duration

Action time

Target durability if applicable

Inventory capacity

Resource requirements

Offline profession progress should stop when:

Inventory becomes full

Required resources run out

Player reaches offline time limit

Activity becomes unavailable

Other stop condition occurs

---

# Offline Target Activities

Target-based profession activities should calculate:

Actions performed

Target damage dealt

Reward thresholds reached

Targets completed

Respawn time

XP gained

Resources gained

Rare rewards

Inventory usage

Threshold rewards should not trigger more than once per target cycle.

Offline calculations should produce results consistent with active gameplay.

---

# Offline Farming

Farming growth timers continue while the game is closed.

When the player returns:

Check crop growth time.

Mark completed crops as ready to harvest.

Show completed Farming plots.

Do not automatically replant crops unless a future automation system allows it.

---

# Profession Statistics

Every profession should track:

Current level

Current XP

Total XP gained

Total actions completed

Total time spent

Total resources gained

Total rare rewards gained

Highest activity unlocked

Highest target completed

Total threshold rewards gained

Total bonus output

Total offline actions

Profession-specific statistics

Examples:

Trees cut

Ore deposits mined

Fish caught

Food cooked

Items pickpocketed

Crops harvested

Artifacts discovered

Animals hunted

Runes created

---

# Profession Achievements

Profession achievements may require:

Reach profession levels

Complete activity counts

Gather resource quantities

Find rare rewards

Use specific tools

Complete high-level activities

Reach level 100

Complete profession collections

Achievements should reward long-term profession progression.

---

# Profession Collections

Profession activities may contribute to collections.

Possible collection entries:

Resources

Rare materials

Tools

Artifacts

Fish

Seeds

Runes

Crafted items

Special activity rewards

First-time discoveries should update the collection log.

Collection discovery remains even if the item is later consumed, sold, or destroyed.

---

# Profession User Interface

Every profession screen should display:

Profession icon

Profession name

Current level

Current XP

XP progress bar

Current activity

Activity list

Activity requirements

Action progress

Target Health or Durability if applicable

Reward thresholds if applicable

Respawn timer if applicable

Expected rewards

XP per action

Estimated XP per hour

Estimated resources per hour

Equipped tool

Assigned companion

Active food buff

Active relic effect

Start button

Stop button

Offline support status

Statistics button

The player should understand exactly what the profession is doing.

---

# Activity Card UI

Every activity card should display:

Activity icon

Activity name

Required level

Locked or unlocked status

Action time

Target Health or Durability if applicable

Reward thresholds if applicable

XP reward

Primary reward

Possible secondary rewards

Possible rare rewards

Required tool

Required region

Start button

Locked activities should explain their requirements.

---

# Active Activity Panel

The active activity panel should display:

Current profession

Current activity

Progress bar

Current target

Target Health or Durability

Next reward threshold

Time until action completes

Time until respawn

Current XP gain

Current reward estimate

Tool bonuses

Companion bonuses

Food bonuses

Relic bonuses

Stop button

The panel should update clearly without excessive visual clutter.

---

# Profession Notifications

Profession notifications may show:

Activity started

Activity stopped

Inventory full

Required resources depleted

Profession level gained

New activity unlocked

New tool unlocked

Threshold reward gained

Rare reward found

Collection item discovered

Achievement completed

Food buff expired

Relic effect expired

Notifications should be useful but not annoying.

---

# Activity Database

Every profession activity should exist inside a Profession Activity Database.

Activities should never be hardcoded.

Systems should reference Activity IDs.

Activity data should connect to:

Profession Database

Item Database

Equipment Database

Companion Database

Region Database

Achievement System

Collection System

Inventory System

Save System

Offline Progress System

---

# Profession Database

Every profession should exist inside the Profession Database.

The Profession Database should define:

Profession ID

Name

Description

Icon

Primary Category

Secondary Category if applicable

Maximum Level

Activity List

Base Unlocks

Supported Tools

Supported Companions

Offline Support

Future Expansion Notes

The Profession Database should contain static profession data.

Player levels and XP belong in save data.

---

# Technical Rules

Profession systems should be data-driven.

Profession activities should reference Activity IDs.

Profession rewards should reference Item IDs.

Profession requirements should reference database IDs.

Profession scripts should not hardcode activity names, rewards, or requirements.

Profession activities should work online and offline.

Reward thresholds should be defined in activity data.

Equipment, companion, food, relic, achievement, and region modifiers should use shared modifier systems.

Adding a new profession activity should not require rewriting profession code.

Individual professions may use specialized activity behavior through modular activity types.

---

# Balance Philosophy

Profession progression should reward long-term planning.

The player should make meaningful decisions:

Train a faster XP activity or gather useful resources?

Gather basic resources or chase rare drops?

Upgrade profession tool or combat equipment?

Sell resources or use them for crafting?

Use food buff now or save it?

Train a new profession or continue a higher-level one?

Choose a fast target or a high-durability target with more threshold rewards?

Older resources should remain useful where possible.

Avoid making every new activity a direct replacement for everything before it.

---

# Future Expansion

Possible additions:

Profession specializations

Profession mastery

Profession prestige

Parallel profession slots

Automation upgrades

Advanced activity types

Profession talent trees

Profession contracts

Profession events

Regional profession bonuses

Guild profession bonuses

Profession challenges

Shared profession activities

Profession loadout presets

Additional reward threshold types

Future systems should expand this framework rather than replace it.

---

# Checklist

Every profession should answer:

✓ What category is this profession?

✓ Does it have a Secondary Category?

✓ What resources does it produce?

✓ What resources does it consume?

✓ Which systems use its rewards?

✓ What tools does it use?

✓ What companions support it?

✓ What activities does it contain?

✓ What unlocks at each level range?

✓ Does it support offline progress?

✓ What happens when inventory is full?

✓ Does it connect to crafting?

✓ Does it connect to combat?

✓ Does it connect to companions?

✓ Does it update achievements?

✓ Does it update collections?

✓ Does it save and load correctly?

✓ Can it support future expansion?

Every profession activity should answer:

✓ What profession owns this activity?

✓ What type of activity is it?

✓ What level is required?

✓ What equipment or tool is required?

✓ How long does it take?

✓ Does it use target Health or Durability?

✓ Does it use reward thresholds?

✓ What XP does it grant?

✓ What rewards does it provide?

✓ What rare rewards can appear?

✓ Can it repeat automatically?

✓ Can it run offline?

✓ What stops the activity?

Every target-based activity should answer:

✓ What is the target's maximum Health or Durability?

✓ What reward thresholds does it use?

✓ What does each threshold reward?

✓ Can each threshold trigger only once?

✓ When is XP granted?

✓ What is the completion reward?

✓ How long does the target take to respawn?

Every rune should answer:

✓ Which Combat Disciplines does it support?

✓ What bonus does it provide?

✓ Is it consumed?

✓ Does it have a duration?

✓ Can it stack?

✓ Does it conflict with other runes?

✓ Does it contribute to a rune set bonus?

✓ Does it work during offline combat?