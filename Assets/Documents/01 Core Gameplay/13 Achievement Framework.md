# 13 Achievement Framework

Version: 1.0
Status: Draft

---

# Purpose

This document defines how achievements work across the entire game.

Achievements reward the player for reaching progression milestones, exploring systems, completing difficult content, collecting items, and experimenting with different gameplay strategies.

Achievements should connect to professions, combat, equipment, companions, crafting, inventory, collections, offline progress, and future systems.

Achievements should provide meaningful long-term goals without becoming mandatory daily chores.

---

# Design Goals

Achievements should:

- Reward meaningful progression.
- Encourage players to explore different systems.
- Provide short-term and long-term goals.
- Recognize difficult accomplishments.
- Support completion-focused players.
- Connect to existing gameplay systems.
- Provide useful but balanced rewards.
- Be fully data-driven.
- Track progress automatically.
- Work during active and offline progress.
- Avoid frustrating or overly repetitive requirements.
- Avoid mandatory daily tasks.

Achievements should celebrate progression rather than interrupt it.

---

# Achievement Philosophy

Achievements should encourage the player to interact with the full game.

Good achievements may reward:

Progression milestones

Profession levels

Combat victories

Boss kills

Dungeon completions

Companion progression

Crafting accomplishments

Collection discoveries

Rare drops

Equipment upgrades

Rune loadouts

Long-term statistics

Interesting challenges

Achievements should not exist only to increase achievement count.

Every achievement should represent a meaningful accomplishment.

---

# Achievement Categories

Current achievement categories:

General Achievements

Profession Achievements

Combat Achievements

Dungeon Achievements

Boss Achievements

Equipment Achievements

Crafting Achievements

Companion Achievements

Inventory Achievements

Collection Achievements

Economy Achievements

Challenge Achievements

Future categories may be added later.

---

# Achievement Structure

Every achievement should define:

Achievement ID

Name

Description

Icon

Category

Requirement Type

Target ID if applicable

Required Amount

Current Progress

Completed Status

Completion Time

Hidden Status

Reward Type

Reward Amount

Reward Item ID if applicable

Passive Bonus if applicable

Claim Required

Repeatable Status

Future Expansion Notes

Achievements should be fully data-driven.

---

# Achievement Requirement Types

Possible requirement types:

Reach Level

Gain XP

Complete Activity

Complete Activity Count

Obtain Item

Obtain Item Quantity

Craft Item

Craft Item Quantity

Equip Item

Upgrade Equipment

Defeat Enemy

Defeat Enemy Count

Defeat Boss

Complete Dungeon

Complete Dungeon Count

Unlock Companion

Reach Companion Rank

Deal Damage

Deal Companion Damage

Use Combat Discipline

Use Rune Loadout

Discover Collection Item

Earn Gold

Spend Gold

Increase Inventory Capacity

Complete Offline Progress

Survive Challenge

Custom Requirement

Future requirement types may be added later.

---

# Achievement Progress

Achievements should track progress automatically.

Examples:

Cut 1,000 trees.

Current progress:

723 / 1,000

Defeat a specific boss.

Current progress:

0 / 1

Reach Woodcutting Level 100.

Current progress:

83 / 100

Unlock 10 companions.

Current progress:

7 / 10

Progress should update immediately when the related action occurs.

---

# Achievement Completion

When an achievement requirement is completed:

Mark achievement as completed.

Store completion time.

Trigger achievement notification.

Grant or prepare rewards.

Update achievement statistics.

Update related completion systems.

Save progress immediately.

Achievements should only complete once unless explicitly marked as repeatable.

---

# Permanent Achievements

Current design:

Most achievements are permanent and non-repeatable.

Permanent achievements should remain completed forever.

Progress should never reset unless a future prestige or seasonal system explicitly requires separate achievement tracking.

Permanent achievements should form the main achievement system.

---

# Repeatable Achievements

Current design:

Repeatable achievements are not required for the base game.

Future repeatable achievements may exist for:

Special events

Seasonal content

Challenge modes

Endgame contracts

Repeatable achievements should not become mandatory daily chores.

Daily and weekly achievement systems should not be added unless they genuinely improve gameplay.

---

# Hidden Achievements

Some achievements may be hidden.

Hidden achievements may conceal:

Name

Description

Requirement

Reward

Possible hidden achievement sources:

Secret boss interactions

Rare item combinations

Unusual rune loadouts

Special companion actions

Hidden profession discoveries

Unusual combat victories

Hidden achievements should feel surprising rather than unfair.

Important progression achievements should not be hidden.

---

# Achievement Difficulty

Achievements may define a difficulty classification.

Possible classifications:

Basic

Intermediate

Advanced

Difficult

Endgame

Secret

Difficulty should describe the expected accomplishment.

Difficulty should not directly control rewards unless explicitly designed.

The UI may use difficulty to help organize achievements.

---

# Achievement Chains

Some achievements may form progression chains.

Example:

Woodcutter I:

Cut 100 trees.

Woodcutter II:

Cut 1,000 trees.

Woodcutter III:

Cut 10,000 trees.

Master Woodcutter:

Reach Woodcutting Level 100.

Achievement chains should show clear progression.

Later achievements in a chain may remain hidden or locked until earlier achievements are completed.

Achievement chains should not create unnecessary duplicates.

---

# Achievement Rewards

Achievements may grant:

Gold

Items

Equipment

Consumables

Companion rank materials

Companion unlock items

Inventory capacity

Recipes

Regions

Combat Abilities

Runes

Relics

Passive bonuses

Cosmetics

Achievement Points

Future rewards

Rewards should match the achievement.

Example:

A Smithing achievement may reward a Smithing tool or crafting material.

A boss achievement may reward unique equipment or an upgrade material.

A companion achievement may reward companion rank materials.

---

# Reward Philosophy

Achievement rewards should feel useful.

Rewards should not completely replace normal progression.

Achievements should not provide such powerful bonuses that players feel forced to complete every achievement immediately.

Good achievement rewards include:

Small permanent bonuses

Useful resource bundles

Unique cosmetic rewards

Special recipes

Quality-of-life unlocks

Rare but balanced equipment

Companion progression materials

Achievement rewards should support progression without becoming mandatory.

---

# Achievement Points

Achievements may grant Achievement Points.

Achievement Points represent total achievement completion.

Achievement Points may be used for:

Completion score

Achievement milestones

Future cosmetic unlocks

Future account rewards

Future achievement shop

Current recommendation:

Achievement Points should initially function as a completion score.

Do not create a mandatory Achievement Point economy too early.

---

# Passive Achievement Bonuses

Some achievements may provide passive bonuses.

Possible passive bonuses:

Profession XP

Combat Discipline XP

Resource gain

Gold gain

Inventory capacity

Rare drop chance

Crafting speed

Companion rank-up speed

Offline time limit

Combat damage

Combat defense

Passive bonuses should be small and clearly displayed.

Avoid stacking too many achievement bonuses into mandatory power.

Major gameplay power should still come from professions, equipment, Combat Disciplines, companions, consumables, runes, and relics.

---

# Reward Claim Rules

Achievements may either grant rewards automatically or require manual claiming.

Every achievement should define:

Claim Required

Current recommendation:

Simple rewards such as Gold or Achievement Points may be granted automatically.

Items, equipment, relics, and important progression rewards may require claiming.

If inventory is full:

Achievement item rewards should remain available to claim.

Important achievement rewards should never be silently deleted.

Claimable rewards should save and load correctly.

---

# General Achievements

General achievements may reward overall account progression.

Examples:

Create the account.

Play for 1 hour.

Play for 100 hours.

Reach a total profession level milestone.

Reach a total Combat Discipline level milestone.

Unlock multiple regions.

Increase inventory capacity.

Discover multiple game systems.

General achievements should represent broad account progression.

---

# Profession Achievements

Every profession may have its own achievements.

Possible requirements:

Reach profession levels.

Complete profession actions.

Gather specific resource quantities.

Find rare profession rewards.

Use specific tools.

Complete high-level activities.

Reach Level 100.

Complete profession collections.

Trigger target reward thresholds.

Profession achievements should encourage progression without forcing inefficient activities.

---

# Woodcutting Achievement Examples

Possible Woodcutting achievements:

Cut the first tree.

Cut 100 trees.

Cut 10,000 trees.

Collect a rare log.

Use a high-tier axe.

Complete an Ancient Tree.

Reach Woodcutting Level 50.

Reach Woodcutting Level 100.

Individual profession documents should define exact achievements later.

---

# Crafting Achievements

Crafting achievements may require:

Craft the first item.

Craft specific item quantities.

Craft equipment.

Craft consumables.

Craft a legendary item.

Unlock recipes.

Complete equipment upgrade paths.

Create rare bonus output.

Reach crafting profession levels.

Crafting achievements should connect gathering resources to useful progression.

---

# Runecrafting Achievements

Runecrafting achievements may require:

Create the first rune.

Create specific rune quantities.

Activate 5 runes at once.

Activate a rune set bonus.

Use a Warrior-focused rune loadout.

Use a Ranger-focused rune loadout.

Use a Mage-focused rune loadout.

Complete combat using a special rune combination.

Reach Runecrafting Level 100.

Runecrafting achievements should encourage experimenting with different rune combinations.

---

# Farming Achievements

Farming achievements may require:

Plant the first crop.

Harvest crop quantities.

Unlock Farming plots.

Grow rare crops.

Harvest multiple plots.

Reach Farming Level 100.

Farming achievements should respect the independent Farming timer system.

---

# Thieving Achievements

Thieving achievements may require:

Complete successful pickpockets.

Steal from specific targets.

Obtain rare stolen items.

Complete consecutive successful actions.

Reach Thieving Level 100.

Achievements should not encourage excessive punishment or failure farming.

---

# Archaeology Achievements

Archaeology achievements may require:

Excavate sites.

Discover artifacts.

Complete artifact sets.

Find relics.

Donate items to the museum if added.

Discover rare excavation rewards.

Reach Archaeology Level 100.

Archaeology achievements should connect strongly to collections and the museum.

---

# Hunting Achievements

Hunting achievements may require:

Complete hunts.

Use specific traps.

Capture or hunt specific creatures.

Obtain rare trophies.

Gather hides and meat.

Complete special creature hunts.

Reach Hunting Level 100.

Hunting achievements should connect to Carpentry, Cooking, Leatherworking, and companions.

---

# Combat Achievements

Combat achievements may require:

Defeat monsters.

Deal damage.

Reach Combat Discipline levels.

Use Combat Abilities.

Survive difficult encounters.

Use specific equipment styles.

Complete fights without potions.

Complete fights with specific runes.

Win with specific companions.

Combat achievements should encourage different builds and preparation strategies.

---

# Combat Discipline Achievements

Each Combat Discipline should have achievements.

Warrior examples:

Deal melee damage.

Use shield abilities.

Defeat enemies using two-handed weapons.

Reach Warrior Level 100.

Reach Warrior Level 150.

Ranger examples:

Deal ranged damage.

Land critical hits.

Defeat enemies using bows or crossbows.

Reach Ranger Level 100.

Reach Ranger Level 150.

Mage examples:

Deal elemental damage.

Apply magical status effects.

Use Devotion effects.

Reach Mage Level 100.

Reach Mage Level 150.

Achievements should not force the player to permanently choose one Combat Discipline.

---

# Companion Combat Achievements

Because combat companions deal damage, achievements may track companion contribution.

Possible requirements:

Deal companion damage.

Defeat enemies with companion attacks.

Land companion critical hits.

Complete a dungeon with a specific companion.

Defeat a boss with a companion landing the final hit.

Deal a combined amount of companion damage.

Companion damage achievements should not grant Combat Discipline XP.

They should reward companion use and progression.

---

# Boss Achievements

Boss achievements may require:

Defeat a boss for the first time.

Defeat a boss multiple times.

Defeat a boss using a specific Combat Discipline.

Defeat a boss with a specific companion.

Defeat a boss without using a potion.

Defeat a boss within a time limit.

Defeat a boss while using a special rune combination.

Boss achievements should recognize meaningful challenges.

Avoid creating requirements that depend entirely on random luck.

---

# Dungeon Achievements

Dungeon achievements may require:

Complete a dungeon.

Complete a dungeon multiple times.

Complete every dungeon in a region.

Complete a dungeon without dying.

Complete a dungeon without using potions.

Complete a dungeon using a specific Combat Discipline.

Complete a dungeon with a specific companion.

Complete a dungeon within a time limit.

Dungeon achievements should encourage mastery after the player understands the dungeon.

---

# Equipment Achievements

Equipment achievements may require:

Equip the first item.

Equip every equipment slot.

Craft a full equipment set.

Equip a high-tier weapon.

Equip a relic.

Complete an upgrade path.

Upgrade specific equipment.

Equip a two-handed weapon.

Use a shield build.

Equipment achievements should connect crafting and combat progression.

---

# Companion Achievements

Companion achievements may require:

Unlock a companion.

Unlock multiple companions.

Reach specific companion ranks.

Reach Rank 20.

Reach Rank 20 with multiple companions.

Deal damage with combat companions.

Complete profession actions with profession companions.

Complete content with specific companions.

Companion achievements should reward long-term companion progression.

---

# Inventory Achievements

Inventory achievements may require:

Obtain items.

Discover unique items.

Increase inventory capacity.

Lock an item.

Favorite an item.

Own a large quantity of one resource.

Fill many inventory slots.

Inventory achievements should not encourage intentionally frustrating inventory management.

---

# Collection Achievements

Collection achievements may require:

Discover item quantities.

Complete item categories.

Complete profession collections.

Complete equipment collections.

Complete artifact sets.

Discover every rune.

Discover every companion.

Complete the full collection log.

Collection achievements should reward discovery, not permanent ownership.

Once an item is discovered, it remains counted.

---

# Economy Achievements

Economy achievements may require:

Earn Gold.

Spend Gold.

Sell items.

Buy inventory capacity.

Purchase shop unlocks.

Craft high-value equipment.

Economy achievements should not encourage players to waste important resources.

Progress should represent normal economic activity.

---

# Offline Progress Achievements

Offline Progress achievements may require:

Complete an offline session.

Gain profession XP offline.

Gain Combat Discipline XP offline.

Complete crafting offline.

Complete companion timers offline.

Accumulate total offline time.

Find rare rewards offline.

Offline achievements should reward using the idle systems naturally.

---

# Challenge Achievements

Challenge achievements should represent unusual or difficult accomplishments.

Examples:

Defeat a boss without taking damage.

Complete a dungeon without potions.

Defeat an enemy using only companion damage after the player has contributed required minimum damage.

Complete combat using five specific runes.

Reach a target reward threshold with a weak tool.

Complete a profession activity under special conditions.

Challenge achievements should be optional.

They may provide cosmetics, titles, Achievement Points, or modest rewards.

Core progression should not depend on extremely difficult challenge achievements.

---

# Retroactive Achievement Progress

Achievements should track progress from the moment the relevant system exists.

When new achievements are added in future updates, the game should complete them retroactively when reliable saved statistics already prove the requirement.

Examples:

The player already killed 10,000 monsters.

A new achievement requires 5,000 monster kills.

The achievement should complete after the update.

Retroactive completion should only happen when accurate data exists.

Do not guess progress that was never tracked.

---

# Missable Achievements

Current design:

Achievements should not normally be permanently missable.

If an achievement depends on a one-time event, the player should have another way to attempt it later when possible.

Avoid achievements that require knowledge the player could not reasonably have.

Secret achievements may be surprising, but they should not punish normal play.

---

# Achievement Dependencies

Some achievements may require earlier achievements.

Every dependency-based achievement should define:

Required Achievement ID

Visible Before Unlock

Progress Tracking Before Unlock

Recommended default:

Progress may still track before the achievement becomes visible.

This prevents the player from repeating completed work unnecessarily.

---

# Achievement Notifications

When an achievement completes, show a notification.

Notification should display:

Achievement icon

Achievement name

Description

Reward summary

Achievement Points if applicable

Claim button if required

Notifications should feel satisfying but should not cover important gameplay information.

Multiple achievements completed together should be grouped when appropriate.

---

# Achievement User Interface

The Achievement UI should display:

Achievement categories

Achievement name

Achievement icon

Description

Requirement

Current progress

Required progress

Progress bar

Completed status

Completion time

Reward

Claim status

Hidden status

Achievement Points

Search

Filters

Completion percentage

The player should easily understand what they have completed and what they can work toward next.

---

# Achievement Filters

Possible Achievement filters:

All Achievements

Incomplete

Completed

Claimable Rewards

General

Professions

Combat

Bosses

Dungeons

Equipment

Crafting

Companions

Collections

Hidden Achievements Discovered

Challenge Achievements

Filters should make large achievement lists easy to navigate.

---

# Achievement Sorting

Possible sorting options:

Category

Completion Status

Progress Closest to Completion

Recently Completed

Difficulty

Achievement Points

Name

Sorting should help the player identify useful goals.

---

# Achievement Tracking

The player may track selected achievements.

Tracked achievements may appear in a small UI panel.

The tracking panel may show:

Achievement name

Current progress

Required progress

Progress bar

Tracked achievement limit should remain small to avoid UI clutter.

Suggested starting limit:

3 tracked achievements

Future progression may increase this limit if useful.

---

# Achievement Completion Summary

The Achievement screen should display:

Total achievements

Achievements completed

Completion percentage

Achievement Points earned

Achievement Points available

Category completion

Recently completed achievements

Unclaimed rewards

Completion summary should make long-term progress easy to understand.

---

# Offline Achievement Progress

Achievements may progress and complete while the game is closed.

Offline progress may update achievements for:

Profession actions

Profession XP

Combat kills

Combat damage

Companion damage

Crafting completion

Items obtained

Gold gained

Companion timers

Collection discoveries

Achievements completed offline should appear in the Offline Progress Summary.

Rewards should remain safe if inventory is full.

---

# Achievement and Inventory Rules

Item rewards should use the Inventory System.

If inventory has space:

Add reward to inventory.

If inventory is full:

Keep the achievement reward claimable.

Important achievement rewards should never be silently deleted.

Locked inventory items should not affect achievement reward claiming.

---

# Achievement and Collection Rules

Achievements and collections are separate systems.

Collections track discovery.

Achievements track accomplishments.

Example:

Collection:

Discover every type of fish.

Achievement:

Catch 10,000 fish.

Completing a collection may also complete an achievement.

Achievements should reference collection progress instead of duplicating collection discovery logic.

---

# Achievement and Statistics Rules

Achievements should read progress from shared statistics whenever possible.

Examples:

Total trees cut

Total enemies defeated

Total companion damage

Total items crafted

Total Gold earned

Total dungeon completions

Using shared statistics prevents duplicate tracking.

Achievement progress should not maintain a separate count if an accurate statistic already exists.

---

# Achievement Statistics

The game should track:

Total achievements completed

Achievement Points earned

Achievements completed by category

Hidden achievements discovered

Challenge achievements completed

Achievement rewards claimed

Achievement rewards unclaimed

First achievement completed

Most recent achievement completed

Completion percentage

---

# Achievement Database

Every achievement should exist inside the Achievement Database.

Achievements should never be hardcoded.

Systems should reference Achievement IDs.

Achievement data should connect to:

Profession System

Combat System

Combat Discipline System

Equipment System

Crafting System

Companion System

Inventory System

Collection System

Statistics System

Save System

Offline Progress System

Region System

Dungeon System

Boss System

---

# Achievement Save Data

Achievement save data should include:

Achievement ID

Current Progress if not read from shared statistics

Completed Status

Completion Time

Claimed Status

Hidden Discovery Status

Tracked Status

Static achievement definitions should remain in the Achievement Database.

Save data should only store player progress.

---

# Technical Rules

Achievements should be data-driven.

Achievement requirements should use modular requirement types.

Achievement rewards should use modular reward types.

Achievement scripts should not hardcode individual achievement names or requirements.

Achievement progress should use shared events and statistics.

Adding a new achievement should not require rewriting achievement code.

Achievement completion should save immediately.

Offline progress should be able to complete achievements.

Achievement rewards should respect inventory capacity.

Missing Achievement IDs should not crash the game.

Achievement IDs should remain stable after release.

---

# Balance Philosophy

Achievements should reward normal progression and optional challenges.

Achievements should not require excessive repetitive grinding without meaningful purpose.

Achievement rewards should be useful but not mandatory.

Players should be encouraged to explore different systems.

Difficult achievements should feel prestigious.

Completion should take time.

The player should always have:

Several nearby achievements

Several medium-term achievements

Several long-term achievements

Several difficult optional achievements

Avoid making every achievement a simple quantity increase.

---

# Future Expansion

Possible additions:

Achievement titles

Profile badges

Achievement showcase

Achievement Point shop

Seasonal achievements

Event achievements

Guild achievements

Prestige achievements

Speedrun achievements

Difficulty-specific achievements

Platform achievements

Steam achievements

Mobile platform achievements

Future achievement systems should expand this framework rather than replace it.

---

# Checklist

Every achievement should answer:

✓ What category does it belong to?

✓ What accomplishment does it recognize?

✓ What requirement type does it use?

✓ What system provides its progress?

✓ Is progress automatically tracked?

✓ Can it complete offline?

✓ Is it hidden?

✓ Is it permanently missable?

✓ Does it have dependencies?

✓ What reward does it provide?

✓ Is the reward automatic or claimable?

✓ What happens if inventory is full?

✓ Does it use shared statistics?

✓ Does it save and load correctly?

✓ Can it complete retroactively?

✓ Does it encourage meaningful gameplay?

✓ Can future systems expand it?

Every achievement reward should answer:

✓ Is the reward useful?

✓ Does it match the achievement?

✓ Does it replace normal progression?

✓ Does it require inventory space?

✓ Can it remain claimable?

✓ Does it provide permanent power?

✓ Is the permanent power balanced?