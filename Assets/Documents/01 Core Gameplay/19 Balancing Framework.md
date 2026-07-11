# 19 Balancing Framework

Version: 1.0  
Status: Draft  
Purpose: Global progression, combat, economy, reward, and pacing rules

---

# Purpose

This document defines how the game's systems should be balanced.

It does not provide every final number.

Instead, it defines:

Balancing principles

Progression pacing

Level curves

Activity efficiency

Combat scaling

Enemy scaling

Equipment progression

Companion contribution

Crafting costs

Resource values

Drop rates

Gold income

Gold spending

Offline efficiency

Difficulty targets

Testing procedures

Data organization

The purpose of this framework is to ensure that every Profession, Combat Discipline, enemy, item, activity, companion, and reward follows one connected balancing philosophy.

Exact values should be stored in dedicated balancing tables and game data.

They should not be scattered across scripts.

---

# Core Balancing Philosophy

The game should reward long-term planning without requiring constant attention.

The player should regularly make meaningful decisions between:

Fast XP

High resource output

Rare rewards

Gold generation

Equipment progression

Companion progression

Account progression

Safety

Efficiency

Convenience

No single activity should always be the best option for every goal.

An activity may be best for:

XP

Resources

Gold

Rare items

Crafting materials

But it should not normally dominate every category at once.

---

# Primary Balancing Goals

The game should:

- Feel rewarding during short sessions.
- Support long idle sessions.
- Provide visible progress every session.
- Avoid excessive early-game waiting.
- Avoid uncontrolled late-game speed.
- Keep older resources useful.
- Make unlocks feel meaningful.
- Make stronger content worth reaching.
- Avoid one mandatory best activity.
- Support multiple valid progression paths.
- Keep Gold useful throughout the game.
- Prevent infinite economic loops.
- Make equipment upgrades noticeable.
- Keep companions valuable without replacing the player.
- Keep offline progress useful without making active play irrelevant.
- Allow future systems to expand existing progression.
- Remain understandable without requiring external spreadsheets.

---

# Balancing Scope

This framework applies to:

Professions

Combat Disciplines

Combat Abilities

Enemies

Elite enemies

Bosses

Dungeons

Tower content

Equipment

Tools

Companions

Crafting

Runes

Food

Healing Potions

Elixirs

Relics

Items

Resources

Gold

Shops

Inventory expansion

Achievements

Collection milestones

Regions

Offline progress

Future progression systems

---

# Current Progression Caps

Current maximum levels:

Professions:

Level 100

Combat Disciplines:

Level 150

Companions:

Rank 20

Combat Abilities:

No separate Ability levels

Companions do not gain XP.

Companion advancement requires:

Items

Gold when applicable

Time

Rank requirements

---

# Progression Layers

The game contains several progression layers.

## Short-Term Progression

Measured over:

Seconds

Minutes

One play session

Examples:

Completing one action

Defeating one enemy

Receiving one reward threshold

Crafting one item

Filling one progress bar

---

## Medium-Term Progression

Measured over:

Several minutes

Hours

Several play sessions

Examples:

Gaining a Profession level

Unlocking a new activity

Crafting new equipment

Ranking up a companion

Completing a dungeon

Unlocking a new Region

---

## Long-Term Progression

Measured over:

Days

Weeks

Months

Examples:

Reaching high Profession levels

Reaching Combat Discipline level 150

Completing Collection categories

Ranking companions to Rank 20

Defeating endgame bosses

Finishing long equipment upgrade paths

Unlocking all Regions

---

# Progression Phase Definitions

For balancing purposes, progression may be divided into phases.

## Early Game

Profession Levels:

1–25

Combat Discipline Levels:

1–40

Companion Ranks:

1–5

Goals:

Teach systems

Provide frequent unlocks

Keep action times short

Keep requirements simple

Give obvious upgrades

Avoid punishing resource scarcity

---

## Mid Game

Profession Levels:

26–60

Combat Discipline Levels:

41–90

Companion Ranks:

6–12

Goals:

Connect systems

Introduce stronger equipment requirements

Create meaningful resource decisions

Introduce more difficult combat mechanics

Increase crafting complexity

Introduce Region progression

---

## Late Game

Profession Levels:

61–90

Combat Discipline Levels:

91–130

Companion Ranks:

13–18

Goals:

Require system synergy

Introduce rare materials

Increase optimization importance

Use stronger bosses and dungeons

Make upgrades require multiple systems

Create long-term Gold sinks

---

## Endgame

Profession Levels:

91–100

Combat Discipline Levels:

131–150

Companion Ranks:

19–20

Goals:

Provide long-term mastery

Use high-value rewards

Require strong preparation

Reward optimized builds

Introduce difficult bosses and Tower content

Provide prestige through completion and collection

Avoid making progress feel completely impossible without perfect play

---

# Progression Speed Philosophy

The early game should move quickly.

The player should gain several meaningful unlocks during the first sessions.

Progress should gradually slow as levels increase.

The slowdown should feel natural rather than sudden.

Avoid extreme walls where one level takes dramatically longer than the previous level without a clear reason.

---

# Expected Unlock Frequency

Suggested early progression:

A meaningful unlock every few levels

Suggested mid progression:

A meaningful unlock every 3–7 levels

Suggested late progression:

Larger unlocks every 5–10 levels

Not every level requires a major unlock.

Levels without new content may still provide:

Higher efficiency

Improved success chance

Higher damage

Higher output

Reduced action time

Access to stronger tools

Progress toward a major milestone

---

# Experience Curve Philosophy

XP requirements should increase gradually.

The curve should allow:

Fast early levels

Steady mid-game progression

Long-term high-level goals

The curve should not become so steep that new activities feel meaningless.

A new higher-level activity should generally provide enough XP to compensate for increased XP requirements.

---

# Profession XP Curve

Professions use levels 1–100.

A possible XP requirement model:

Required XP increases nonlinearly with level.

General form:

XP Required for Next Level = Base XP × Level Growth

The exact formula should be stored in balancing data.

A suitable curve may use:

Exponential growth

Power growth

Segmented growth

Or a precomputed XP table

A precomputed table is recommended because it allows precise tuning.

---

# Profession XP Targets

Suggested pacing targets are starting guidelines.

They are not final promises.

## Early Profession Levels

Levels 1–10:

Very fast

Several minutes to approximately one hour total depending on activity

Levels 11–25:

Noticeably slower but still frequent

## Mid Profession Levels

Levels 26–50:

Hours of combined activity

Levels 51–75:

Increasingly long-term progression

## Late Profession Levels

Levels 76–90:

Substantial commitment

Levels 91–100:

Mastery progression

Exact total time should be adjusted after testing the full content amount.

---

# Combat Discipline XP Curve

Combat Disciplines use levels 1–150.

Combat XP should scale from player contribution.

Current rule:

Player damage grants Combat Discipline XP.

Companion damage does not grant Combat Discipline XP by default.

Companion damage still contributes to:

Kills

Loot

Combat completion

Encounter speed

This prevents companions from becoming the main source of Combat Discipline XP.

---

# Combat XP Allocation

Combat Discipline XP should be based on player contribution.

Possible models:

XP per player damage dealt

XP per enemy defeated adjusted by player damage share

XP per successful player attack

Recommended starting model:

Combat Discipline XP is primarily based on player damage dealt.

This provides stable scaling across enemies with different Health values.

Example concept:

Player XP Earned = Player Damage Dealt × Discipline XP Coefficient

The coefficient may vary by:

Enemy type

Enemy difficulty

Region

Dungeon

Boss

Tower floor

---

# Combat XP Exploit Prevention

Combat XP should not be granted for:

Damage against invulnerable targets

Repeatedly healing an enemy and damaging it again

Damage beyond an enemy's remaining Health

Cancelled or invalid combat states

Only valid effective damage should count.

Example:

Enemy has 10 Health remaining.

Player deals 100 damage.

XP should count only the valid 10 damage unless the design intentionally uses raw damage.

Valid damage is recommended.

---

# Companion Contribution Balance

Companions directly attack enemies.

Companions:

Deal damage

Use attack timers

May use abilities

Cannot be targeted

Do not have Health

Cannot die

Do not require healing

Companion damage should be meaningful but should not completely replace the player's build.

---

# Companion Damage Target

A Combat Companion may contribute a percentage of total combat output.

Suggested starting ranges:

Low Rank:

Approximately 5–12% of total damage

Mid Rank:

Approximately 10–20%

High Rank:

Approximately 15–30%

Specialized companion builds may exceed these values temporarily.

The player should normally remain the primary damage source.

---

# Companion Rank Scaling

Companion Rank progression should feel meaningful.

Each rank may improve:

Attack Damage

Attack Speed

Accuracy

Critical Chance

Critical Damage

Ability power

Support bonus

Rank increases should not all provide identical percentage gains.

Some ranks may provide small improvements.

Milestone ranks should provide larger upgrades.

Suggested milestone ranks:

Rank 5

Rank 10

Rank 15

Rank 20

---

# Companion Rank Cost Scaling

Companion rank costs may increase through:

More materials

Higher-tier materials

More Gold

Longer waiting time

Special milestone items

Rank-up time should not increase without limit.

Very long timers should remain reasonable for an idle game.

Higher ranks should feel important, not merely delayed.

---

# Activity Balance Categories

Every activity should be balanced across several dimensions.

Possible dimensions:

XP per hour

Resources per hour

Gold value per hour

Rare reward chance

Required attention

Unlock requirement

Risk

Preparation cost

Inventory pressure

Cross-system value

An activity may excel in one or two dimensions.

It should not normally dominate all dimensions.

---

# Activity Efficiency

Every activity should have expected efficiency values.

Examples:

XP per hour

Primary resource per hour

Secondary resource per hour

Expected Gold value per hour

Rare reward chance per hour

Expected item value per hour

These values should be calculated from data.

They should not be estimated manually inside each UI card.

---

# Action Time Balance

Action time affects:

Player feedback frequency

XP rate

Reward frequency

Inventory pressure

Offline simulation cost

Perceived responsiveness

Early activities should generally have shorter action times.

Higher-level activities may have longer actions with larger rewards.

Longer action time should not automatically mean worse efficiency.

---

# Suggested Action Time Ranges

These are broad starting guidelines.

Fast actions:

1–3 seconds

Standard actions:

3–8 seconds

Long actions:

8–20 seconds

Very long actions:

20–60 seconds

Actions longer than one minute should be used carefully.

Long-duration systems such as Farming and companion rank-ups are separate timer systems.

---

# Action Speed Modifiers

Action speed bonuses may come from:

Profession level

Tools

Equipment

Companions

Relics

Food

Elixirs

Achievements

Future account upgrades

Action speed should have limits.

Avoid allowing action time to approach zero.

---

# Minimum Action Time

Every action type should define a minimum allowed action time.

Example:

Base action time:

5 seconds

Total speed bonuses reduce it.

Minimum action time:

1 second

The exact minimum may differ between systems.

This prevents:

Broken animations

Excessive reward loops

Performance problems

Extreme economic inflation

---

# Multiplicative and Additive Modifiers

Modifiers should be categorized clearly.

Additive modifiers combine before multiplication.

Multiplicative modifiers apply separately.

Example concept:

Final Value = Base Value × Additive Modifier Group × Multiplicative Modifier Group

Avoid creating too many separate multiplicative bonuses.

Too many multiplicative systems can create uncontrolled scaling.

---

# Modifier Stacking Rules

Every modifier should define:

Stat affected

Flat or percentage

Additive or multiplicative

Source category

Stacking rule

Maximum or cap if applicable

Examples:

Tool Speed Bonus

Companion Output Bonus

Food XP Bonus

Relic Rare Drop Bonus

The UI should explain final bonuses clearly.

---

# Diminishing Returns

Diminishing returns may be used for highly scalable stats.

Possible candidates:

Action Speed

Cooldown Reduction

Damage Reduction

Critical Chance

Avoidance

Drop Chance bonuses

Sell Price bonuses

Diminishing returns should be used only where necessary.

It should not be hidden.

---

# Hard Caps

Possible hard caps may include:

Critical Chance

Damage Reduction

Cooldown Reduction

Action Speed reduction

Avoidance

Accuracy

Drop multiplier

Exact caps should be defined in balancing data.

Caps should be visible in advanced stat details.

---

# Profession Activity Balance

Every Profession activity should define:

Required Level

Base Action Time

Target Durability if applicable

Base XP

Primary Rewards

Secondary Rewards

Rare Rewards

Requirements

Region

Tool Requirement

Respawn Time if applicable

Failure Chance if applicable

Estimated XP per hour

Estimated Reward value per hour

---

# Profession Tier Structure

Current Profession tier structure may use:

Tier 0 through Tier 9

Suggested level mapping:

Tier 0:

Level 1

Tier 1:

Level 10

Tier 2:

Level 20

Tier 3:

Level 30

Tier 4:

Level 40

Tier 5:

Level 50

Tier 6:

Level 60

Tier 7:

Level 70

Tier 8:

Level 80

Tier 9:

Level 90

Exact unlock levels may vary.

Level 100 may represent mastery rather than a new full tier.

---

# Higher-Tier Activity Value

Higher-tier activities should generally provide:

More XP

Higher-value resources

New crafting materials

Higher rare-reward potential

Greater progression value

However, lower-tier activities should remain useful through:

Older crafting recipes

Bulk requirements

Companion rank-ups

Equipment upgrades

Region unlocks

Shop demand

Collection goals

---

# Target Durability Balance

Target-based Professions may use durability or Health-like values.

Examples:

Trees

Ore deposits

Hunting targets

Archaeology sites

Target durability should scale with:

Required Profession level

Tool power available

Expected action duration

Reward value

Number of reward thresholds

---

# Target Damage Model

The player deals Profession damage to targets.

Possible calculation:

Profession Damage = Base Profession Power + Tool Power + Bonuses

The exact formula should remain data-driven.

Profession damage should not use Combat Discipline damage formulas.

---

# Target Completion Time

Expected target completion time should remain within intended activity pacing.

Example:

Low-tier target:

Several seconds

Mid-tier target:

Several actions

High-tier target:

Longer sustained activity

Target completion time should be calculated using expected equipment for the target's level.

Do not balance only around maximum endgame tools.

---

# Reward Threshold Balance

Target-based activities may award rewards at configured thresholds.

Common example:

75%

50%

25%

0%

Thresholds are data-driven.

Not every target must use the same thresholds.

---

# Threshold Reward Rules

Threshold rewards may:

Grant partial resources

Grant XP

Roll rare rewards

Provide completion bonuses

Current default:

Profession XP is primarily granted when the target is fully completed.

Partial thresholds may grant resources without granting full completion XP.

This prevents repeatedly farming only the first threshold for maximum XP.

---

# Threshold Exploit Prevention

The system should prevent intentional target resetting from being more efficient than completing it unless that choice is explicitly designed.

Possible protections:

Completion XP only at 0%

Rare rewards only at completion

Higher final threshold rewards

Reset penalties

No partial respawn exploitation

---

# Respawn Balance

Respawn time creates downtime.

Respawn time should be short enough that the player is not constantly waiting.

Possible ranges:

Common targets:

1–5 seconds

Rare targets:

5–20 seconds

Special targets:

Longer when justified

Respawn time should be included in XP-per-hour and reward-per-hour calculations.

---

# Gathering Profession Balance

Gathering Professions include systems such as:

Woodcutting

Mining

Fishing

Foraging

Hunting

Archaeology

Farming

Gathering balance should consider:

Action time

Target durability

Respawn time

Tool progression

Resource quantity

Rare drops

Inventory pressure

Cross-crafting demand

---

# Woodcutting Balance

Woodcutting should provide:

Logs

Wood-related crafting materials

Profession XP

Rare tree materials

Possible companion materials

Logs should support:

Carpentry

Cooking fuel if used

Equipment upgrades

Companion advancement

Future systems

Tree durability should scale with expected axe power.

---

# Mining Balance

Mining should provide:

Ore

Stone

Gems

Rare minerals

Smithing materials

Jewelcrafting materials

Mining should balance ore output against:

Smithing demand

Equipment upgrade demand

Gold value

Gem rarity

---

# Fishing Balance

Fishing should provide:

Fish

Cooking materials

Rare aquatic items

Possible companion materials

Fishing balance may use:

Fishing target difficulty

Catch time

Catch tables

Required bait

Region

Rare catch chance

---

# Foraging Balance

Foraging should provide:

Herbs

Plants

Food ingredients

Herblore materials

Rare botanical resources

Foraging may offer diverse reward tables rather than one guaranteed resource.

---

# Hunting Balance

Hunting may balance:

Target selection

Preparation items

Trap cost if used

Success chance

Action time

Animal materials

Leatherworking materials

Food materials

Rare trophies

Hunting should not duplicate Combat exactly.

---

# Archaeology Balance

Archaeology may balance:

Excavation time

Site durability

Artifact chance

Common debris

Region requirement

Tool requirement

Collection value

Rare artifacts should be exciting but not required in unreasonable quantities for basic progression.

---

# Farming Balance

Farming uses independent growth timers.

Farming balance should consider:

Seed cost

Growth duration

Harvest quantity

Failure or disease if such systems are added

Offline growth

Plot count

Crop demand

Farming should continue while another Profession is active.

Growth duration should match the value and demand of the crop.

---

# Crafting Profession Balance

Crafting Professions include:

Cooking

Smithing

Carpentry

Herblore

Tailoring

Leatherworking

Jewelcrafting

Enchanting

Runecrafting

Crafting balance should consider:

Ingredient value

Crafting time

Gold cost

Output value

XP

Equipment strength

Consumable demand

Recipe unlock requirements

---

# Crafting Reservation Rule

When crafting begins:

Required materials are reserved.

Reserved materials cannot be sold or used elsewhere.

Materials are consumed when crafting completes.

If crafting is cancelled, the cancellation rules should define whether materials are returned.

The default should avoid accidental loss unless cancellation is intentionally costly.

---

# Crafting XP Balance

Crafting XP should reflect:

Ingredient value

Recipe tier

Crafting time

Output usefulness

Unlock requirement

Crafting XP should not be based only on crafting duration.

Expensive or difficult recipes may grant more XP.

---

# Crafting Profit Balance

Crafting may create value.

However, unlimited shop-bought materials should not create infinite guaranteed profit.

The following should be checked:

Ingredient shop cost

Crafting Gold cost

Output sell value

Crafting time

Failure chance if applicable

Tool requirement

---

# Crafting Output Value

Crafted item value should reflect:

Material cost

Time cost

Profession requirement

Recipe rarity

Item usefulness

Equipment power

Demand

Sell value should normally remain lower than total player investment unless the activity is intentionally designed as a Gold-making method.

---

# Cooking Balance

Cooking creates:

Food

Combat preparation

Profession buffs

Food buffs currently last roughly:

2–5 minutes

Stronger food may provide:

Longer duration

Higher effect

More difficult ingredients

Higher Cooking requirement

Food should remain useful without requiring constant reapplication every few seconds.

---

# Herblore Balance

Herblore creates:

Healing Potions

Elixirs

Possible utility consumables

Healing Potions:

Restore Health

May use cooldown or auto-use rules

Elixirs:

Provide stronger temporary buffs

Elixirs should have meaningful duration and cost.

They should not become mandatory for routine low-level combat.

---

# Smithing Balance

Smithing should connect:

Mining

Combat equipment

Tools

Equipment upgrades

Smithing equipment should remain competitive with combat drops.

Combat drops may provide:

Unique effects

Rare upgrade materials

Specialized items

Smithing may provide:

Reliable progression

Targeted equipment creation

Tool upgrades

---

# Carpentry Balance

Carpentry should provide uses for logs.

Possible outputs:

Tools

Equipment components

Structures if added later

Containers

Utility items

Companion materials

Carpentry should prevent Woodcutting resources from becoming obsolete.

---

# Tailoring and Leatherworking Balance

Tailoring may use:

Cloth

Fibers

Special fabrics

Mage or utility equipment

Leatherworking may use:

Hides

Leather

Hunting materials

Ranger or utility equipment

These Professions should overlap only where useful.

They should retain distinct material chains and equipment identities.

---

# Jewelcrafting Balance

Jewelcrafting may create:

Rings

Amulets

Gem components

Equipment upgrade materials

Jewelry should provide meaningful specialization.

Avoid allowing one jewelry combination to dominate every build.

---

# Enchanting Balance

Enchanting may provide:

Equipment effects

Stat modification

Upgrade paths

Special materials

Enchanting should not create unlimited power stacking.

Enchantments should define:

Valid equipment types

Maximum tier

Replacement rules

Stacking rules

---

# Runecrafting Balance

Runecrafting supports:

Warrior

Ranger

Mage

The player may activate up to 5 runes before combat.

Runes are locked during combat.

Rune balance should consider:

Individual rune strength

Rune combinations

Set effects

Discipline compatibility

Crafting cost

Availability

Quantity or consumption if used later

The exact rune consumption system remains undecided.

Balance data should allow runes to support:

Permanent equipped behavior

Per-combat consumption

Per-attack consumption

Timed consumption

Without requiring major rewrites.

---

# Combat Balance Philosophy

Combat should reward preparation and progression.

The player should make decisions about:

Combat Discipline

Weapon

Armor

Jewelry

Relic

Companion

Abilities

Food

Healing Potion

Elixirs

Runes

Target selection

Combat should remain automated once started.

The player's strategy happens before and between encounters.

---

# Combat Stat Categories

Possible player combat stats:

Health

Devotion

Attack Damage

Attack Speed

Accuracy

Critical Chance

Critical Damage

Defense

Damage Reduction

Resistance

Ability Power

Cooldown Reduction

Healing Received

Life Steal if added

Avoidance if added

Loot bonuses

XP bonuses

---

# Damage Formula Philosophy

Damage should be predictable enough to understand.

Avoid formulas with many hidden random multipliers.

A possible starting model:

Final Damage = Base Damage × Ability Modifier × Offensive Modifiers × Defensive Mitigation

Random damage ranges may be used.

Example:

Weapon Damage:

10–14

Final attack rolls within that range before modifiers.

---

# Damage Type Structure

Possible damage types may include:

Physical

Ranged

Magic

Elemental types if added

True damage if added carefully

Every damage type should have a clear purpose.

Avoid adding many types without meaningful gameplay differences.

---

# Accuracy and Hit Chance

Accuracy determines whether an attack hits.

Possible inputs:

Attacker Accuracy

Target Defense or Evasion

Level difference

Ability modifier

Buffs

Debuffs

Hit chance should have minimum and maximum limits.

Example concept:

Minimum Hit Chance:

5%

Maximum Hit Chance:

95%

Exact values should be tested.

---

# Accuracy Philosophy

Low-level normal combat should not involve excessive misses.

Miss chance may become more important in:

Difficult Regions

Elites

Bosses

Tower content

Special mechanics

Accuracy should remain a meaningful stat without making combat feel random and frustrating.

---

# Critical Hits

Critical Hits may use:

Critical Chance

Critical Damage multiplier

Possible starting default:

Base Critical Chance:

Low but non-zero

Base Critical Damage:

Higher than normal damage

Exact values should be stored in balancing data.

Critical Chance should have a cap.

Critical Damage should scale more slowly than normal damage.

---

# Attack Speed

Attack Speed determines attack interval.

Possible formula:

Attack Interval = Base Interval ÷ Attack Speed Modifier

Every attacker should have:

Base Attack Interval

Minimum Attack Interval

Attack speed bonuses should not reduce intervals to unstable values.

---

# Devotion Balance

Devotion is a Combat resource.

Devotion balance should define:

Maximum Devotion

Starting Devotion

Generation method

Ability costs

Regeneration

Out-of-combat behavior

Devotion should prevent unlimited Ability use.

It should not prevent the player from using Abilities often enough to feel meaningful.

---

# Combat Ability Balance

Combat Abilities do not have separate levels.

Ability power may scale from:

Combat Discipline level

Weapon

Stats

Equipment effects

Runes

Relics

Companion bonuses

Abilities should define:

Damage multiplier

Cooldown

Devotion cost

Target rules

Effect duration

Auto-use rules

Equipment requirement

---

# Ability Trade-Offs

A strong Ability should normally have one or more costs:

Higher cooldown

Higher Devotion cost

Lower accuracy

Equipment restriction

Conditional trigger

Preparation requirement

Abilities should not all be direct damage upgrades.

Possible roles:

Burst damage

Sustained damage

Defense

Healing

Buff

Debuff

Resource generation

Execute

Control

---

# Auto-Use Balance

Auto-use conditions may affect combat efficiency.

Possible conditions:

Use when ready

Use below Health threshold

Use above Devotion threshold

Use against Boss only

Use when buff is missing

Use when enemy is below Health threshold

Auto-use should not create a separate power advantage over manual use.

It should provide convenience and strategic configuration.

---

# Enemy Balance Structure

Every enemy should define:

Enemy ID

Region

Type

Level or difficulty

Health

Attack Damage

Attack Speed

Accuracy

Defense

Critical stats if used

Abilities

Resistances

Weaknesses

Loot table

XP coefficient

Unlock requirements

---

# Normal Enemy Balance

Normal enemies should provide:

Reliable Combat XP

Basic materials

Equipment progression

Consumable materials

Region-specific items

Normal combat should be sustainable with reasonable preparation.

It should not require maximum consumables for every encounter.

---

# Elite Enemy Balance

Elite enemies should provide:

Higher difficulty

Better reward rates

Rare materials

Improved equipment chances

Stronger abilities

Elites should require more preparation than normal enemies.

They should not merely be normal enemies with excessive Health.

---

# Boss Balance

Bosses should test:

Build preparation

Equipment

Combat Discipline progression

Ability setup

Consumables

Runes

Companion choice

Bosses should include mechanics, not only high stats.

Possible mechanics:

Burst attacks

Defense phases

Resistance changes

Debuffs

Ability timing

Health thresholds

Enrage

Add summons if added later

---

# Boss Reward Balance

Bosses may reward:

Unique equipment

Upgrade materials

Companion materials

Recipes

Region unlock items

Rare consumables

Collection items

Boss rewards should remain valuable after repeated kills.

Possible long-term uses:

Crafting

Equipment upgrading

Companion rank-ups

Rune creation

Shop exchanges

---

# Dungeon Balance

Dungeons may contain:

Multiple enemies

Elite encounters

Boss

Reward chest

Entry requirements

Dungeons should be balanced around total run difficulty.

Important considerations:

Consumable use

Expected damage taken

Completion time

Failure risk

Chest value

Replay value

---

# Dungeon Reward Chests

Reward chests may use:

Guaranteed rewards

Weighted random rewards

First-clear rewards

Completion-count milestones

Rare jackpot rewards

Drop chances should be clear when the game's settings allow exact chances.

Dungeons should not require unreasonable repetition for mandatory progression items.

---

# Tower Balance

Tower content should provide increasing difficulty.

Each floor may increase:

Enemy Health

Enemy Damage

Accuracy

Defense

Ability complexity

Reward value

Tower scaling should avoid becoming only exponential stat inflation.

Higher floors should introduce:

New enemy combinations

Modifiers

Mechanics

Build challenges

---

# Enemy Health Scaling

Enemy Health should scale with expected player damage.

Expected kill time is a key balancing target.

Possible target ranges:

Weak normal enemy:

Short encounter

Normal enemy:

Moderate encounter

Elite:

Longer encounter

Boss:

Extended encounter

The exact duration depends on game speed and content type.

---

# Combat Duration Targets

Possible starting targets:

Normal enemy:

Approximately 5–20 seconds

Elite enemy:

Approximately 15–60 seconds

Boss:

Approximately 1–5 minutes

Dungeon:

Several minutes or longer

These are broad guidelines.

Idle-game combat may use longer encounters when rewards justify the duration.

---

# Combat Survival Targets

Balance should consider:

Expected player Health loss

Healing Potion use

Food buff use

Elixir use

Ability timing

The player should be able to identify when a target is too difficult.

Normal enemies should not unexpectedly kill appropriately equipped players.

Bosses may require active build preparation.

---

# Player Death or Defeat Balance

Defeat consequences should be meaningful but not excessively punishing.

Possible consequences:

Combat ends

Temporary loot remains or is partially protected according to system rules

Consumables used remain consumed

No permanent equipment loss

No Profession progress loss

No character deletion

Important rewards should never disappear silently.

---

# Temporary Combat Loot Balance

Combat rewards enter Temporary Combat Loot.

The player may:

Pick an item

Pick All

Temporary Loot capacity creates inventory management pressure.

It should not create frequent unavoidable loss.

Important rewards should use protected claim storage when necessary.

---

# Loot Table Structure

Loot tables should define:

Guaranteed drops

Common drops

Uncommon frequency drops without using rarity labels

Rare drops

Unique drops

Quantity ranges

Drop weights

Conditions

First-kill rewards

Collection status does not change drop chance unless explicitly designed.

---

# Drop Chance Philosophy

Drop chances should reflect:

Enemy difficulty

Encounter duration

Item usefulness

Item quantity required

Whether the item is mandatory

Alternative acquisition sources

Rare cosmetic or collection items may have lower chances.

Mandatory progression items should have:

Reasonable drop rates

Pity systems

Guaranteed milestones

Alternative sources

Or controlled crafting paths

---

# Expected Time to Acquire

Every important random item should have an expected acquisition time.

Expected time should include:

Encounter duration

Drop chance

Kill rate

Downtime

Player power at intended level

Do not balance a drop chance without considering how frequently the player can attempt it.

---

# Pity and Bad-Luck Protection

Bad-luck protection may be used for:

Mandatory progression items

Companion unlock items

Dungeon keys

Unique equipment

Rare recipes

Possible systems:

Increasing drop chance

Guaranteed drop after a number of attempts

Token exchange

Crafting from duplicate materials

Pity systems should be saved correctly.

---

# Equipment Balance Philosophy

Equipment should create meaningful progression.

Upgrades should be noticeable.

Equipment should support different builds.

No single item should remain best for every situation unless it is a final endgame reward.

---

# Equipment Power Budget

Every equipment tier should have a power budget.

Power may be distributed across:

Damage

Defense

Health

Accuracy

Critical stats

Attack Speed

Ability bonuses

Profession bonuses

Resource bonuses

Special effects

Items with strong special effects may need lower raw stats.

---

# Equipment Slot Power

Different slots may contribute different amounts of power.

Possible higher-impact slots:

Weapon

Chest

Relic

Possible medium-impact slots:

Helmet

Legs

Shield

Amulet

Possible lower or specialized slots:

Ring

Cape

Gloves

Boots

Exact slot budgets should be tested.

---

# Weapons

Weapons define much of offensive identity.

Weapon balance should consider:

Damage range

Attack speed

Accuracy

Combat Discipline

One-handed or two-handed

Ability compatibility

Special effect

Two-handed weapons should provide enough power to compensate for losing Shield access.

---

# Shields

Shields may provide:

Defense

Damage Reduction

Health

Resistance

Block mechanics if added

Utility effects

Shields should not be mandatory for every build.

Two-handed and dual-style builds should remain viable.

---

# Armor

Armor should provide:

Survivability

Discipline identity

Specialization

Possible categories may support:

Warrior

Ranger

Mage

Hybrid

Avoid rigid restrictions unless they create meaningful choices.

---

# Jewelry

Rings and Amulets should support specialization.

Possible bonuses:

Critical Chance

Accuracy

Resource generation

Drop bonuses

Profession bonuses

Ability effects

Jewelry should not become only flat-stat progression.

---

# Relics

Relics may provide:

Powerful timed effects

Long cooldown effects

Unique build mechanics

Account progression

Relic balance should define:

Duration

Cooldown

Activation rules

Stacking

Combat restrictions

Profession restrictions

Relics should feel powerful without being permanently active at full strength unless designed that way.

---

# Tools

Profession tools should improve:

Profession damage

Action speed

Output

Rare reward chance

Special effects

Higher-tier tools should meaningfully improve activities.

Tool progression should not make lower tools useless immediately if upgrade paths use them.

---

# Equipment Upgrade Costs

Equipment upgrades may require:

Gold

Base equipment

Profession materials

Boss materials

Dungeon materials

Region materials

Recipes

Upgrade cost should scale with power gained.

Expensive upgrades should feel noticeably impactful.

---

# Consumable Balance

Combat loadout currently supports:

1 Food

1 Healing Potion

4 Elixirs

Each consumable category should have a clear purpose.

---

# Food Balance

Food provides temporary buffs.

Current target duration:

Approximately 2–5 minutes

Food should be:

Affordable enough for regular use

Valuable enough to prepare

Not mandatory for every trivial action

Food may support:

Combat

Professions

Resource gain

XP gain

Action speed

---

# Healing Potion Balance

Healing Potions restore Health.

Balance should consider:

Healing amount

Cooldown

Auto-use threshold

Quantity cost

Crafting cost

Enemy damage

Healing should not make the player immortal.

It should extend survival and reward preparation.

---

# Elixir Balance

Elixirs provide stronger temporary buffs.

Possible categories:

Damage

Defense

Accuracy

Critical

Resource generation

Profession efficiency

Elixirs may use:

Duration

Charges

Cooldown

Mutual exclusivity

The player can equip 4 Elixirs.

Effects should be balanced around combined loadout power.

---

# Economy Balance Philosophy

Gold should remain useful throughout the game.

Gold income and Gold sinks should scale together.

The player should regularly decide between:

Buying materials

Expanding Inventory

Upgrading equipment

Ranking companions

Unlocking Regions

Buying recipes

Saving Gold

---

# Gold Income Sources

Gold may come from:

Selling items

Combat

Thieving

Dungeons

Bosses

Achievements

Offline progress

Special rewards

Gold income should be tracked by source.

This helps identify broken or useless systems.

---

# Gold Sink Categories

Major Gold sinks may include:

Inventory expansion

Shop purchases

Equipment upgrades

Companion rank-ups

Crafting fees

Recipe unlocks

Region unlocks

Loadout upgrades

Future systems

Gold sinks should feel like progression investments.

---

# Inventory Expansion Balance

Starting Inventory capacity:

100 slots

Capacity may be expanded using Gold.

Upgrade costs should increase over time.

Inventory expansion should remain attractive without becoming mandatory immediately.

Early upgrades should be achievable.

Late upgrades may become major long-term Gold sinks.

---

# Shop Price Balance

Every shop item should define:

Buy Price

Sell Price

Stock type

Requirements

Sell Price should normally be lower than Buy Price.

The economy must prevent:

Buy-low sell-high loops

Crafting guaranteed profit from unlimited shop stock without intended restrictions

Discount resale exploits

Duplicate claim exploits

---

# Resource Sell Value

Resource sell values should consider:

Acquisition rate

Crafting demand

Level requirement

Alternative sources

Inventory pressure

Selling should be a valid choice.

But crafting the item may often provide greater long-term value.

---

# Currency Inflation

Inflation occurs when Gold income rises faster than meaningful Gold sinks.

Balance testing should monitor:

Gold earned per hour

Gold spent per hour

Average saved Gold

Major purchase timing

Unused Gold accumulation

If players accumulate excessive Gold:

Add meaningful progression sinks.

Do not add arbitrary maintenance fees only to remove currency.

---

# Regional Balance

Regions should represent progression bands.

A Region may require:

Profession levels

Combat Discipline levels

Boss defeat

Dungeon completion

Gold

Items

Achievements

Region difficulty should match its unlock requirements.

---

# Region Reward Identity

Each Region should provide:

Unique resources

Combat enemies

Recipes

Equipment

Companion materials

Collection items

Region identity should prevent every Region from feeling interchangeable.

---

# Region Progression

A new Region should usually offer:

Higher-value activities

New item chains

Stronger enemies

New equipment

New progression requirements

Not every activity in a new Region must be strictly better.

Some may provide specialized resources.

---

# Offline Progress Balance

Offline progress is a core system.

Suggested offline cap:

12 hours

This remains adjustable.

Offline progress should provide meaningful value.

It should not exceed optimized active play in every category.

---

# Offline Efficiency

Possible starting offline efficiency:

100% for simple deterministic activities

Reduced efficiency for systems requiring active decisions

The exact rule may differ by system.

Examples:

Woodcutting:

May simulate near full efficiency

Farming:

Growth timers continue normally

Companion rank-up:

Timer continues normally

Crafting:

Current job continues according to system rules

Combat:

May use simulation rules with safety checks

---

# Offline Combat Balance

Offline Combat must avoid impossible outcomes.

Simulation should consider:

Player stats

Enemy stats

Healing supplies

Consumables

Runes

Companion damage

Inventory capacity

Temporary Loot

Expected kill time

Expected damage taken

If the player cannot safely continue:

Offline Combat may stop.

It should not assume infinite healing or impossible survival.

---

# Offline Reward Limits

Offline rewards may be limited by:

Time cap

Inventory capacity

Temporary Loot capacity

Consumable quantity

Activity requirements

Protected claim storage

Important rewards should not disappear silently.

---

# Active Play Advantage

Active play may provide advantages through:

Changing targets

Claiming loot

Managing Inventory

Updating loadouts

Reacting to unlocks

Using better activity choices

Active play should not receive arbitrary large multipliers only for being online.

The advantage should come mainly from better decisions.

---

# Achievement Reward Balance

Achievement rewards should be modest and meaningful.

Possible rewards:

Gold

Items

Consumables

Cosmetics

Permanent minor bonuses

Achievement Points

Avoid placing essential progression entirely behind obscure achievements.

Achievements should reward normal accomplishments rather than force inefficient play.

---

# Collection Reward Balance

Collection completion is based primarily on obtaining items.

Possible Collection rewards:

Gold

Inventory capacity

Cosmetics

Minor account bonuses

Special items

Milestone rewards

Collection bonuses should not become mandatory for basic progression.

---

# Unlock Balance

Unlock requirements should:

Communicate goals

Prevent overwhelming the player

Connect systems

Create anticipation

Avoid excessive simultaneous requirements

A new activity may require:

Level

Tool

Region

Item

Boss completion

Achievement

Companion rank

Most early unlocks should use simple requirements.

Later unlocks may combine systems.

---

# Requirement Complexity

Suggested complexity by phase:

Early Game:

One main requirement

Mid Game:

One or two requirements

Late Game:

Multiple connected requirements

Endgame:

Several meaningful progression requirements

Avoid requirements that exist only to create unnecessary delay.

---

# Power Growth Philosophy

Player power should grow through multiple systems.

Possible sources:

Combat Discipline level

Equipment

Abilities

Companion

Runes

Consumables

Relics

Achievements

Collection bonuses

No one system should provide nearly all power.

---

# Vertical and Horizontal Progression

Vertical progression increases raw power.

Examples:

Higher damage

More Health

More Defense

Horizontal progression increases options.

Examples:

New abilities

Different runes

Specialized equipment

Alternative companions

Build-specific relics

The game should contain both.

Pure vertical scaling can make old content trivial and builds uninteresting.

---

# Catch-Up Balance

Future content may need catch-up mechanics.

Possible methods:

Improved early XP

Cheaper outdated upgrades

Guaranteed early equipment

Alternative resource sources

Catch-up should not invalidate long-term players.

It should reduce unnecessary old friction.

---

# Difficulty Labels

Content may use difficulty labels.

Examples:

Easy

Appropriate

Challenging

Dangerous

Extreme

Labels should be calculated from:

Player power

Survivability

Accuracy

Expected kill time

Enemy damage

They should not rely only on Combat Discipline level.

---

# Recommended Power

Content may display recommended values.

Possible recommendations:

Combat Discipline level

Health

Accuracy

Defense

Damage

Equipment tier

Recommendations should be approximate.

They should not prevent creative builds from attempting content.

---

# Balance Data Architecture

Balancing values should be stored in data.

Possible data files:

Profession XP Table

Combat XP Table

Activity Balance Table

Enemy Balance Table

Equipment Balance Table

Companion Balance Table

Recipe Balance Table

Item Value Table

Shop Price Table

Drop Table

Region Balance Table

Offline Balance Table

---

# Stable IDs

Every balanced entry should use a stable ID.

Examples:

profession_woodcutting

activity_sproutwood_tree

enemy_grey_wolf

item_iron_ore

recipe_iron_sword

companion_ember_wolf

region_greenvale

Balance data should reference IDs rather than visible names.

---

# Data-Driven Formulas

Formulas should use configurable values.

Avoid hardcoding:

XP curves

Drop rates

Enemy scaling

Action speed caps

Shop price ratios

Companion scaling

Equipment tier multipliers

Values should be editable without rewriting gameplay scripts.

---

# Balance Tables

A balancing spreadsheet or table should contain columns such as:

ID

Name

Level Requirement

Tier

Base Action Time

Base XP

Primary Reward

Reward Quantity

Rare Reward Chance

Target Durability

Respawn Time

Expected XP per Hour

Expected Value per Hour

Notes

Combat tables may contain:

Enemy ID

Region

Health

Damage

Attack Interval

Accuracy

Defense

XP Coefficient

Expected Kill Time

Expected Damage Taken

Loot Value

---

# Expected Value Calculations

Random rewards should use expected value.

Example concept:

Expected Item Quantity = Drop Chance × Average Quantity

Expected Gold Value = Expected Quantity × Sell Value

Expected hourly value includes:

Attempt duration

Respawn time

Failure chance

Inventory limits

Consumable cost

Expected value should not replace player-facing actual rewards.

It is a balancing tool.

---

# Cost Accounting

Balance calculations should include costs.

Possible costs:

Food

Potions

Elixirs

Runes if consumed

Repair if ever added

Entry items

Crafting materials

Gold fees

Expected profit should subtract expected cost.

---

# Baseline Player Profiles

Balance testing should use standard player profiles.

Examples:

Minimum Profile:

Lowest reasonable equipment for content

Expected Profile:

Typical equipment at intended progression

Optimized Profile:

Strong but obtainable build

Maximum Profile:

Near-best available setup

Content should primarily be balanced around the Expected Profile.

---

# Profession Test Profiles

Profession testing should include:

No tool or starting tool

Expected tool for tier

Strong tool from next progression step

No companion

Expected companion

Maximum applicable bonuses

This identifies whether bonuses create extreme output.

---

# Combat Test Profiles

Combat testing should include:

Under-equipped player

Expected player

Strong player

No companion

Expected companion

High-rank companion

No consumables

Expected consumables

Maximum consumable loadout

No runes

Expected runes

Optimized runes

---

# Balance Testing Questions

Every activity should answer:

How long does one action take?

How much XP does it provide?

How much reward value does it provide?

What is the intended player level?

What equipment is expected?

What alternatives exist?

Does it remain useful later?

Can it be exploited?

---

# Combat Testing Questions

Every enemy should answer:

How long does the expected player take to defeat it?

How much damage does the player take?

How many consumables are used?

What is its XP per hour?

What is its loot value per hour?

Is the reward worth the difficulty?

Can companion damage trivialize it?

Can the enemy kill the player unexpectedly?

---

# Economy Testing Questions

Every major economic system should answer:

How much Gold does it generate?

How much Gold does it remove?

When can the player afford major upgrades?

Does selling replace crafting progression?

Can the player create infinite profit?

Does Gold remain useful late game?

---

# Progression Milestone Testing

Important milestones should be tested.

Examples:

First Profession level

First tool upgrade

First crafted equipment

First companion

First combat win

First dungeon

First Region unlock

Profession Level 50

Profession Level 100

Combat Discipline Level 100

Combat Discipline Level 150

Companion Rank 20

Each milestone should feel appropriately important.

---

# Early Game Testing

The early game should verify:

Player understands what to do

First rewards arrive quickly

First level-up happens quickly

Inventory does not fill immediately

Gold has a clear use

First upgrade is achievable

No system requires excessive waiting

---

# Mid-Game Testing

The mid game should verify:

Systems connect meaningfully

Crafting materials remain valuable

Combat and Professions support each other

Gold choices become meaningful

New Regions feel rewarding

No single activity dominates everything

---

# Late-Game Testing

The late game should verify:

Progress remains visible

Rare materials are obtainable

Equipment upgrades feel meaningful

Companions remain useful

Boss rewards remain relevant

Gold still has uses

No stat grows without control

---

# Endgame Testing

The endgame should verify:

Content remains challenging

Build choices matter

Optimized players progress faster but do not break systems

Completion goals remain achievable

Rare rewards do not require unreasonable repetition

Older systems still contribute

---

# Balance Change Rules

When changing balance:

Identify the problem.

Measure the current result.

Change the smallest relevant set of values.

Test connected systems.

Compare before and after.

Document the change.

Avoid changing many unrelated values simultaneously.

---

# Balance Versioning

Balance data should include version information.

Save data should not store derived values that can be recalculated safely.

When balance changes:

Existing player progress should remain valid.

Items should not disappear.

Levels should not reset.

Current XP should remain consistent.

Migrations should be used when necessary.

---

# Buff and Nerf Philosophy

Buff underperforming options when possible.

Nerf options when they:

Break progression

Create exploits

Remove all meaningful alternatives

Cause severe economic inflation

Trivialize major content

Nerfs should avoid destroying player investment without a strong reason.

---

# Anti-Exploit Balance Rules

The game should prevent:

Infinite buy-and-sell loops

Infinite crafting profit from unlimited shop materials

Repeated first-clear rewards

Duplicate offline rewards

Target reset XP exploits

Overkill XP exploits

Negative action times

More than 100% Critical Chance unless intentionally supported

More than maximum Damage Reduction

Free permanent Elixir effects

Companion attacks continuing outside valid combat

Loot duplication through claim actions

---

# Numerical Safety

All numerical systems should define safe limits.

Protect against:

Negative quantities

Negative prices

Negative timers

Overflow

NaN values

Infinite values

Division by zero

Extreme percentage stacking

Invalid save values

Large currencies and quantities may require:

64-bit integers

Decimals or controlled floating-point usage

---

# Randomness Philosophy

Randomness should create excitement.

It should not determine every part of progression.

Use randomness for:

Rare drops

Bonus quantities

Critical hits

Optional rewards

Avoid using heavy randomness for:

Basic activity success

Mandatory unlocks

Core equipment progression

Important save outcomes

---

# Random Seed and Save Safety

Random results should not be rerolled accidentally through:

Opening menus

Reloading UI

Changing screens

Saving and loading repeatedly

Reward generation should happen inside gameplay systems.

The UI should only display confirmed results.

---

# Player-Facing Transparency

The UI should show useful balancing information.

Possible information:

Action time

XP per action

Estimated XP per hour

Reward quantity

Drop chances when allowed

Required level

Damage range

Attack interval

Cooldown

Buff duration

Sell value

Do not expose every internal formula by default.

Advanced details may appear in tooltips or Statistics panels.

---

# Estimated Rate Accuracy

Displayed estimates should account for:

Current action speed

Tool bonuses

Companion bonuses

Respawn time

Success chance

Current modifiers

Estimates may exclude rare rewards unless clearly stated.

The UI should label estimates as:

Estimated

Average

Expected

Not guaranteed

---

# Statistics for Balancing

The game should track internal statistics such as:

Time spent per activity

XP earned per activity

Items earned per activity

Gold earned by source

Gold spent by sink

Enemies defeated

Average combat duration

Player defeats

Consumables used

Dungeon completions

Boss attempts

Crafting counts

Companion rank-up timing

Offline activity results

Player-facing statistics and development analytics may be separate.

---

# Manual Testing Before Analytics

The game should be balanced manually before relying on large-scale analytics.

Initial balancing should use:

Designer calculations

Automated simulations

Test profiles

Playtesting

Analytics can later identify unexpected player behavior.

---

# Simulation Tools

Development tools should support simulations.

Possible simulations:

10 minutes of activity

1 hour of activity

12 hours offline

1,000 combat encounters

10,000 drop rolls

Full Profession progression

Gold income and spending

Companion progression

Simulation should produce summaries.

Examples:

Average XP per hour

Average items per hour

Average Gold value

Average deaths

Average consumable use

Expected rare drops

---

# Debug Balance Controls

Development builds may include:

Change Profession level

Change Combat Discipline level

Grant equipment

Grant companion rank

Set activity speed

Force rare drop

Simulate offline time

Override enemy stats

Display expected value

Display damage calculations

Reset balance test profile

Debug controls should not exist in release builds.

---

# Balance Review Process

A balance review should examine:

Progression

Combat

Economy

Rewards

Offline systems

Inventory pressure

Player choices

The review should identify:

Dominant strategy

Useless activity

Excessive waiting

Broken reward

Impossible requirement

Inflation

Excessive grind

Trivial content

---

# Balance Documentation Rules

Every major balancing decision should be documented.

Example:

Why an activity has long action time

Why a Boss item has low drop chance

Why a shop item is limited

Why a companion rank requires a specific material

Why a stat has a cap

This prevents future changes from accidentally removing intended design.

---

# Codex Rules

Codex should:

Use data-driven balance values.

Use stable IDs.

Use shared formulas.

Use precomputed XP tables where appropriate.

Calculate expected rates through shared utilities.

Validate invalid values.

Keep UI separate from balance logic.

Support simulation and debug testing.

Codex should not:

Hardcode balance values across many scripts.

Create random XP formulas for each Profession.

Create separate damage formulas for every enemy.

Assume higher tier always means strictly better in every category.

Add item rarity without approval.

Allow companions to grant Combat Discipline XP by default.

Add companion Health.

Allow runes to be changed during combat.

Decide rune consumption rules without approval.

Silently cap stats without showing the cap in advanced information.

---

# Technical Acceptance Criteria

The Balancing System is acceptable when:

Profession XP uses a centralized curve or table.

Combat Discipline XP uses centralized rules.

Companion ranks use centralized cost and stat data.

Activity values are data-driven.

Enemy values are data-driven.

Equipment values are data-driven.

Drop tables are data-driven.

Shop prices are data-driven.

Offline efficiency is configurable.

Action speed has safe minimums.

Critical Chance and Damage Reduction can be capped.

Expected XP and reward rates can be calculated.

Combat simulations can be run.

Economic exploits are validated.

Balance changes do not require rewriting core system code.

---

# Balance Checklist

## Progression

✓ Are early levels fast enough?

✓ Does progression slow gradually?

✓ Are major unlocks spaced appropriately?

✓ Do Profession levels 1–100 feel meaningful?

✓ Do Combat Discipline levels 1–150 remain achievable?

✓ Do Companion ranks 1–20 provide noticeable improvements?

---

## Activities

✓ Does every activity have a clear purpose?

✓ Is XP per hour appropriate?

✓ Is reward value appropriate?

✓ Does action time fit the reward?

✓ Does the activity remain useful later?

✓ Is there an obvious exploit?

---

## Professions

✓ Do higher-tier activities feel rewarding?

✓ Do older resources retain uses?

✓ Do tools provide meaningful progression?

✓ Are reward thresholds balanced?

✓ Are respawn times included in rate calculations?

✓ Are independent timers handled correctly?

---

## Crafting

✓ Do ingredients justify the output?

✓ Is Crafting XP appropriate?

✓ Are materials reserved safely?

✓ Are materials consumed at completion?

✓ Does Shop material purchasing create infinite profit?

✓ Do crafted items remain competitive?

---

## Combat

✓ Is expected kill time appropriate?

✓ Is expected damage taken appropriate?

✓ Does player damage grant Combat Discipline XP?

✓ Is companion damage excluded from Discipline XP?

✓ Is companion contribution meaningful but controlled?

✓ Do abilities have meaningful costs?

✓ Are Health, Accuracy, Defense, and Attack Speed balanced together?

---

## Enemies

✓ Do normal enemies provide reliable progression?

✓ Do Elites provide better rewards?

✓ Do Bosses use mechanics?

✓ Are Dungeon rewards worth the total run?

✓ Does Tower difficulty increase meaningfully?

✓ Are mandatory items protected from extreme bad luck?

---

## Equipment

✓ Do upgrades feel meaningful?

✓ Are two-handed weapons worth losing a Shield?

✓ Do special effects use power budget?

✓ Is one item dominating every build?

✓ Are tools balanced separately from combat equipment?

---

## Consumables

✓ Do Food buffs last approximately 2–5 minutes?

✓ Do Healing Potions heal without creating immortality?

✓ Do Elixirs provide meaningful temporary buffs?

✓ Is the 1 Food, 1 Potion, and 4 Elixir loadout balanced?

✓ Are consumable costs included in combat profit?

---

## Runes

✓ Do Runes support Warrior, Ranger, and Mage?

✓ Are up to 5 active Runes supported?

✓ Are Runes locked during combat?

✓ Are set effects controlled?

✓ Is Rune consumption left configurable until decided?

---

## Economy

✓ Does Gold remain useful?

✓ Are major Gold sinks meaningful?

✓ Are Inventory upgrades priced appropriately?

✓ Are Buy Prices higher than Sell Prices?

✓ Can Crafting create an infinite shop profit loop?

✓ Does resource selling create real decisions?

---

## Drops

✓ Is expected acquisition time reasonable?

✓ Are mandatory progression items obtainable reliably?

✓ Are rare drops exciting?

✓ Are drop chances calculated with encounter time?

✓ Is bad-luck protection used where needed?

---

## Offline Progress

✓ Is the offline cap configurable?

✓ Do independent timers continue?

✓ Does offline Combat respect survival and consumables?

✓ Are Inventory and Temporary Loot limits respected?

✓ Are important rewards protected?

✓ Does active play retain an advantage through better decisions?

---

## Data

✓ Are balance values stored in centralized data?

✓ Are stable IDs used?

✓ Are formulas shared?

✓ Can values change without rewriting scripts?

✓ Are invalid values detected?

✓ Can expected rates be simulated?

---

## Testing

✓ Are Minimum, Expected, Optimized, and Maximum profiles tested?

✓ Are early, mid, late, and endgame phases tested?

✓ Are 10-minute, 1-hour, and 12-hour simulations available?

✓ Are large drop simulations available?

✓ Are economy sources and sinks tracked?

✓ Are changes documented?