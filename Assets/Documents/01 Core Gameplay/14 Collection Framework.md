# 14 Collection Framework

Version: 1.0
Status: Draft

---

# Purpose

This document defines how the Collection Log works across the entire game.

The Collection Log primarily tracks items discovered by the player.

Items may come from:

Professions

Crafting

Combat drops

Boss drops

Dungeon rewards

Shops

Achievements

Regions

Future systems

When the player obtains an item for the first time, that item becomes permanently discovered in the Collection Log.

The Collection Log should also display how many copies of each item the player has obtained over the lifetime of the account.

---

# Design Goals

The Collection Log should:

- Track item discoveries.
- Make first-time item drops feel meaningful.
- Show permanent account-wide discovery progress.
- Display lifetime item quantities.
- Connect professions, crafting, combat, shops, and progression.
- Help players understand where items come from.
- Provide long-term completion goals.
- Work with active and offline progress.
- Be fully data-driven.
- Avoid unnecessary filler entries.
- Avoid requiring the player to permanently keep items.

The Collection Log should focus on discovering items rather than tracking kills, bosses, or repeated activities.

---

# Collection Philosophy

The Collection Log tracks what the player has obtained at least once.

Inventory tracks what the player currently owns.

These are separate systems.

Example:

The player obtains an Iron Sword.

The Iron Sword becomes discovered in the Collection Log.

The player later sells or upgrades the Iron Sword.

The Iron Sword remains discovered permanently.

Collection discovery should not be removed when an item is:

Sold

Consumed

Destroyed

Used for crafting

Used for equipment upgrades

Used for companion rank-ups

Donated to a future Museum

Lost through another system

Once an item is discovered, it remains discovered permanently.

---

# Core Collection Rule

The main Collection Log is based on items.

An item entry is completed the first time the player successfully obtains that item.

The Collection Log should not require:

Defeating an enemy a certain number of times

Killing a boss

Completing a dungeon

Reaching a profession level

Reaching a companion rank

Completing repeated activities

These accomplishments belong in the Achievement System.

Enemies, bosses, dungeons, professions, and regions may appear as item sources, but they are not Collection Log entries by default.

---

# Discovery Rules

A collection entry becomes discovered when the player meets its discovery condition.

Possible discovery conditions:

Obtain item

Craft item

Create rune

Excavate artifact

Receive equipment

Receive combat drop

Receive dungeon reward

Receive boss drop

Purchase item

Receive achievement reward

Receive companion reward

Find rare reward

Every collection entry should define its exact discovery condition.

The most common discovery condition should be:

Obtain the item for the first time.

---

# Successful Item Acquisition

An item should count as obtained when it successfully enters player ownership.

Possible ownership locations:

Main Inventory

Equipment Inventory

Temporary protected reward storage

Offline Reward Claim storage

Achievement Reward Claim storage

Other protected claim storage

An item should not count as obtained if:

The item was generated but permanently lost before being claimed.

The item appeared only as a preview.

The player viewed it in a shop but did not purchase it.

The player viewed it in a recipe but did not craft or obtain it.

The item appeared in a loot table but did not drop.

---

# First-Time Discovery

When the player obtains an item for the first time:

Mark the item as discovered.

Record the discovery time.

Record the discovery source.

Increase lifetime obtained quantity.

Update the relevant collection category.

Update total collection completion.

Check collection milestone rewards.

Check related achievements.

Show a first-time discovery notification.

Save collection progress immediately.

The first-time discovery notification should feel exciting without interrupting gameplay.

---

# Collection Entry Completion

Every normal item entry has two main states:

Undiscovered

Discovered

The entry becomes completed after the player obtains at least one copy.

Obtaining additional copies does not change the entry's completion status.

Additional copies increase lifetime statistics.

Example:

Oak Log obtained for the first time:

Collection Entry: Discovered

Lifetime Obtained: 1

Oak Log obtained again:

Collection Entry: Still Discovered

Lifetime Obtained: 2

---

# Lifetime Item Tracking

Every collection entry should track how many copies the player has obtained over the lifetime of the account.

Lifetime Obtained should increase whenever the player successfully obtains the item.

Possible acquisition sources:

Gathering

Crafting

Combat

Bosses

Dungeons

Shops

Achievements

Companions

Offline Progress

Future systems

Lifetime Obtained should never decrease.

Selling, consuming, destroying, or using an item should not reduce Lifetime Obtained.

---

# Current Ownership

Collection entries may also display the player's current owned quantity.

Current Owned Quantity comes from the Inventory System.

Example:

Lifetime Obtained:

10,000 Oak Logs

Currently Owned:

725 Oak Logs

This allows the player to compare permanent collection history with current inventory ownership.

Current Owned Quantity may increase or decrease.

Lifetime Obtained only increases.

---

# Highest Quantity Owned

Collection entries may track the highest quantity of an item owned at one time.

Example:

Lifetime Obtained:

10,000

Currently Owned:

725

Highest Quantity Owned:

3,450

This statistic is optional for UI display but should be available for statistics and achievements.

---

# Acquisition Source Statistics

The game may track how an item was obtained.

Possible source totals:

Total Gathered

Total Crafted

Total Dropped from Combat

Total Received from Bosses

Total Received from Dungeons

Total Purchased

Total Received from Achievements

Total Received from Offline Progress

Total Received from Other Sources

Not every item needs every source statistic.

The Collection Log may show a simplified source breakdown when useful.

---

# Collection Categories

Collection entries should be organized using item categories.

Current collection categories:

Resources

Crafting Materials

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

Runes

Companion Items

Artifacts

Currencies

Books

Special Items

Future categories may be added later.

Collection categories should use categories and types from the Item Framework.

---

# Profession Collections

Items may also be grouped by the profession that produces or primarily uses them.

Possible Profession Collection groups:

Woodcutting

Mining

Fishing

Cooking

Foraging

Smithing

Herblore

Farming

Carpentry

Thieving

Hunting

Archaeology

Tailoring

Leatherworking

Jewelcrafting

Enchanting

Runecrafting

An item may appear in both its item category and its related profession group.

This should reference the same collection entry instead of creating duplicate discovery data.

---

# Profession Item Discovery

Profession activities may discover items.

Examples:

Woodcutting discovers logs and rare tree materials.

Mining discovers ores, gems, and rare minerals.

Fishing discovers fish and rare catches.

Foraging discovers herbs, seeds, and materials.

Hunting discovers hides, meat, bones, and trophies.

Archaeology discovers artifacts and ancient materials.

Profession rewards should update the Collection Log when the item is successfully obtained.

---

# Crafted Item Discovery

Crafting may discover items.

Examples:

Smithing creates weapons, armor, bars, and tools.

Cooking creates food.

Herblore creates potions and elixirs.

Carpentry creates bows, traps, tools, and planks.

Tailoring creates cloth equipment.

Leatherworking creates leather equipment.

Jewelcrafting creates rings, amulets, and gems.

Enchanting creates or modifies enchanted equipment.

Runecrafting creates runes.

A crafted item becomes discovered when crafting successfully completes and the output enters player ownership or protected reward storage.

---

# Combat Item Discovery

Combat may discover items through loot.

Possible combat item sources:

Regular enemies

Elite enemies

Bosses

Dungeons

Towers

Raids

Reward chests

Combat Collection entries should still represent items, not enemies.

Example:

Collection Entry:

Wolf Fang

Source:

Grey Wolf

Collection Entry:

Blackshard

Source:

Blackstone Guardian Boss

Collection Entry:

Ancient Chestplate

Source:

Ancient Crypt Dungeon Chest

Defeating the enemy alone does not complete the item entry.

The item must actually be obtained.

---

# Boss and Dungeon Item Sources

Bosses and dungeons are not Collection Log entries by default.

They may appear as item sources.

A Boss source display may show:

Boss name

Region

Drop chance

Required Combat Discipline level

Dungeon requirement

A Dungeon source display may show:

Dungeon name

Reward chest

Enemy source

Final boss

Drop chance

Completion requirement

Boss and Dungeon completion should be tracked through Achievements and combat statistics.

---

# Equipment Collection

The Equipment Collection tracks equipment obtained by the player.

Possible equipment groups:

Weapons

Armor

Tools

Accessories

Relics

Warrior Equipment

Ranger Equipment

Mage Equipment

Profession Tools

Boss Equipment

Dungeon Equipment

Crafted Equipment

Equipment becomes discovered when obtained.

The player does not need to equip the item for it to count.

---

# Equipment Instance Rules

Multiple copies of the same equipment may have different instance data.

Examples:

Upgrade level

Enchantments

Socketed gems

Future item quality

Collection discovery should be based on the base Item ID.

Example:

The player obtains three Iron Swords.

Collection Entry:

Iron Sword

Discovered: Yes

Lifetime Obtained: 3

Individual equipment instances remain managed by the Inventory and Equipment Systems.

---

# Equipment Upgrade Collection Rules

An upgraded item should count as a separate collection entry when it has its own unique Item ID.

Example:

Runic Axe

Black Runic Axe

Ancient Black Runic Axe

Each version may have its own collection entry.

The Collection Log should show upgrade relationships when possible.

Example:

Runic Axe

↓

Black Runic Axe

↓

Ancient Black Runic Axe

---

# Consumable Collection

The Consumable Collection tracks:

Food

Potions

Elixirs

Future consumables

Consumables become discovered when obtained or crafted for the first time.

The player does not need to consume the item for it to count.

Consumable entries may display:

Effect

Duration

Source

Recipe

Required profession level

Current owned quantity

Lifetime obtained quantity

Lifetime consumed quantity

---

# Rune Collection

Runes are items created through Runecrafting.

A rune becomes discovered when it is created or otherwise obtained for the first time.

Rune entries may display:

Rune icon

Rune name

Description

Combat bonus

Supported Combat Disciplines

Rune tags

Set bonus connections

Consumption rule

Required Runecrafting level

Recipe

Current owned quantity

Lifetime obtained quantity

The player does not need to activate the rune for basic discovery.

Rune activation and rune set accomplishments should be tracked through Achievements.

---

# Artifact Collection

Artifacts are item-based Archaeology discoveries.

An artifact becomes discovered when successfully excavated or otherwise obtained.

Artifact entries may display:

Artifact icon

Name

Description

Excavation site

Region

Artifact set

Museum status if applicable

Current owned quantity

Lifetime obtained quantity

Artifacts remain discovered even if donated to a future Museum.

---

# Relic Collection

Relics are special equipment items.

A relic becomes discovered when obtained for the first time.

Relic entries may display:

Relic icon

Name

Description

Passive effect

Active effect

Duration

Cooldown

Upgrade path

Source

Current owned quantity

Lifetime obtained quantity

Relics should feel like important Collection Log discoveries.

---

# Companion Items

Companion-related items may appear in the Collection Log.

Examples:

Companion unlock items

Companion rank materials

Companion equipment

Special companion resources

Obtaining a companion item discovers the item entry.

Unlocking a companion itself should be tracked in the Companion System.

A separate Companion Collection tab may exist later, but companion unlocks should not be mixed into the main Item Collection completion percentage by default.

---

# Recipe Unlocks

Recipes are not physical items unless represented by an item such as a recipe book.

Recipe unlocks should normally be tracked in the Crafting System.

Recipe books may appear in the Item Collection because they are items.

Example:

Ancient Sword Recipe Book

The book becomes discovered when obtained.

The unlocked Ancient Sword recipe is tracked separately in the Crafting System.

A future Recipe Collection tab may exist without affecting the main Item Collection percentage.

---

# Region Unlocks

Regions are not Item Collection entries.

Region discovery and completion should be tracked in the Region System.

Regions may appear as item source information.

Example:

Item:

Frostleaf

Source Region:

Frozen Highlands

Region unlocks should not affect the main Item Collection percentage unless a future separate Region Collection is created.

---

# Hidden Collection Entries

Some item entries may remain hidden until discovered or partially revealed.

Possible hidden display styles:

Completely hidden

Question mark

Item silhouette

Unknown name

Known name but hidden source

Possible hidden item types:

Secret artifacts

Secret relics

Hidden rune combinations

Rare boss drops

Secret crafted equipment

Special event items

Core progression items should not be hidden in frustrating ways.

---

# Revealed but Undiscovered Entries

Some item entries may be visible before discovery.

A revealed entry may show:

Item name

Icon or silhouette

Description

Possible source

Required profession level

Required region

Drop chance if applicable

This helps the player set collection goals.

Every entry should define:

Hidden Until Discovered

Source Hidden Until Discovered

Name Hidden Until Discovered

Icon Hidden Until Discovered

---

# Item Source Information

Every Collection Log entry should help the player understand where the item comes from.

Possible sources:

Profession Activity

Crafting Recipe

Regular Enemy

Elite Enemy

Boss

Dungeon

Reward Chest

Shop

Achievement

Companion

Archaeology Site

Region

Future system

An item may have multiple sources.

The Collection Log should display every known source when appropriate.

---

# Source Unlock Rules

Some item sources may remain hidden until the player unlocks related content.

Example:

A boss item source remains hidden until the boss is discovered.

A region resource remains hidden until the region is unlocked.

A recipe source remains hidden until the recipe is discovered.

The Collection Log should avoid revealing major secret content too early.

---

# Drop Chance Display

Random-drop items may display their drop chance.

Possible display rules:

Exact percentage

Approximate rarity

Unknown until first discovery

Hidden permanently

Recommended rule:

Show exact drop chances for normal combat and profession items.

Secret items may hide their drop chance until discovery.

Drop chances should come directly from the relevant loot table.

---

# Collection Entry Structure

Every Collection Log item entry should define:

Collection Entry ID

Item ID

Collection Category

Profession Group if applicable

Discovered Status

Discovery Time

First Discovery Source

Current Owned Quantity

Lifetime Obtained Quantity

Highest Quantity Owned

Lifetime Consumed if applicable

Lifetime Sold if applicable

Lifetime Crafted if applicable

Lifetime Gathered if applicable

Lifetime Dropped from Combat if applicable

Hidden Until Discovered

Counts Toward Completion

Optional Entry

Event Entry

Related Item Sources

Related Achievement IDs

Future Expansion Notes

Static item information should come from the Item Database.

---

# Item Database Connection

The Collection Log should reference the Item Database.

The Item Database provides:

Item ID

Name

Description

Icon

Category

Type

Primary Use

Source

Sell Value

Stackable Status

Equipment information if applicable

Consumable information if applicable

The Collection Log should not duplicate static item data.

It should store discovery and lifetime statistics.

---

# Collection Completion

An item entry is complete after the item has been obtained at least once.

A Collection category is complete when every required item entry in that category has been discovered.

Every entry should define:

Counts Toward Completion

Optional Entry

Event Entry

Future Entry

Unavailable Entry

Optional and temporary entries should not block permanent collection completion.

---

# Collection Percentage

Collection percentage should calculate:

Discovered required item entries

÷

Total required item entries

×

100

The UI should show:

Items discovered

Items remaining

Required entries

Optional entries

Hidden entries discovered

Category completion percentage

Overall Item Collection completion percentage

Lifetime quantities should not affect completion percentage.

Obtaining one copy is enough to complete the entry.

---

# Collection Milestones

Collection categories may have milestone rewards.

Example milestones:

10% Complete

25% Complete

50% Complete

75% Complete

100% Complete

Milestones should use number of discovered items, not lifetime quantity.

Possible milestone rewards:

Gold

Crafting materials

Inventory capacity

Consumables

Cosmetics

Achievement Points

Small passive bonuses

Rare items

Milestone rewards should support progression without becoming mandatory.

---

# Collection Reward Philosophy

Collection rewards should feel useful but should not replace normal progression.

The main reasons to complete the Collection Log should be:

Discovery

Completion

Long-term goals

Account history

Finding rare items

Exploring connected systems

Small permanent bonuses are acceptable.

Large mandatory combat or profession bonuses should be avoided.

---

# Reward Claim Rules

Collection milestone rewards may be:

Granted automatically

Manually claimed

Every reward should define:

Claim Required

If inventory is full:

Item rewards should remain claimable.

Important Collection rewards should never be silently deleted.

Claim status should save and load correctly.

---

# Duplicate Items

Obtaining an already discovered item should not create a new discovery.

It should update:

Lifetime Obtained

Current Owned Quantity

Highest Quantity Owned

Source-specific statistics

Achievements if applicable

Duplicate items should not repeatedly trigger the first-time discovery notification.

Optional smaller notifications may appear for extremely rare duplicate items.

---

# Lifetime Quantity Rules

Lifetime Obtained should increase based on quantity successfully acquired.

Example:

The player gathers 5 Oak Logs from one reward.

Lifetime Obtained increases by 5.

The player crafts 10 Healing Potions.

Lifetime Obtained increases by 10.

The player receives 2 Iron Bars from combat.

Lifetime Obtained increases by 2.

For non-stackable equipment:

Each individual item obtained increases Lifetime Obtained by 1.

---

# Selling and Consuming Items

Selling or consuming an item should not reduce collection discovery or Lifetime Obtained.

The Collection Log may display:

Lifetime Sold

Lifetime Consumed

Lifetime Destroyed

Lifetime Used for Crafting

Lifetime Used for Upgrades

These values should come from shared item statistics when available.

---

# Collection and Inventory

Inventory tracks current ownership.

The Collection Log tracks permanent discovery and lifetime acquisition.

When an item enters inventory or protected claim storage:

Check whether it is already discovered.

Mark it discovered if needed.

Increase Lifetime Obtained.

Update Current Owned Quantity when appropriate.

When an item leaves inventory:

Update Current Owned Quantity.

Do not reduce Lifetime Obtained.

Do not remove discovery.

---

# Collection and Professions

Profession rewards should update the Collection Log.

Examples:

Logs from Woodcutting

Ore from Mining

Fish from Fishing

Herbs from Foraging

Crops from Farming

Artifacts from Archaeology

Hides from Hunting

Profession rewards obtained offline should also update the Collection Log.

---

# Collection and Crafting

Crafted outputs should update the Collection Log.

When crafting completes:

Create the output item.

Increase Lifetime Obtained.

Increase Lifetime Crafted.

Mark the item discovered if it is new.

Update category completion.

Check achievements.

Crafting should use the same item discovery event as every other system.

---

# Collection and Combat

Combat loot should update the Collection Log after the item is successfully claimed.

Possible combat item sources:

Regular enemies

Elite enemies

Bosses

Dungeons

Reward chests

Companion-assisted combat

Combat should increase:

Lifetime Obtained

Lifetime Dropped from Combat

Boss or dungeon source totals if tracked

Enemies and bosses themselves should not become main Collection Log entries.

---

# Collection and Shops

Purchased items should update the Collection Log.

When an item is successfully purchased:

Increase Lifetime Obtained.

Increase Lifetime Purchased if tracked.

Mark the item discovered if it is new.

Viewing an item in a shop does not discover it.

The player must obtain the item.

---

# Collection and Achievements

Achievements and the Collection Log are separate systems.

The Collection Log tracks item discovery.

Achievements track accomplishments.

Examples:

Collection Log:

Obtain an Oak Log.

Achievement:

Cut 10,000 trees.

Collection Log:

Obtain every type of fish.

Achievement:

Catch 100,000 fish.

Collection Log:

Obtain a boss weapon.

Achievement:

Defeat the boss 100 times.

Achievements should read Collection Log progress when an achievement requires item discoveries.

---

# Collection and Offline Progress

Offline progress may discover items.

Possible offline discoveries:

Profession resources

Rare profession rewards

Crafted items

Combat loot

Artifacts

Runes

When the player returns:

Process successfully obtained items.

Update Lifetime Obtained quantities.

Mark new entries discovered.

Update Collection completion.

Show new discoveries in the Offline Progress Summary.

Items lost because they could not be stored should not count as obtained unless they entered protected claim storage.

---

# Retroactive Collection Progress

Collection progress may update retroactively when reliable saved statistics prove that the player previously obtained an item.

Example:

A Collection Log feature is added after release.

Saved statistics show the player previously obtained 500 Iron Ore.

Iron Ore should become discovered.

Lifetime Obtained should use the saved total when available.

If accurate historical data does not exist, the game should not invent a lifetime quantity.

Future item systems should track Lifetime Obtained from the beginning.

---

# Collection Notifications

When a new item is discovered, show a notification.

The notification may display:

Item icon

Item name

Collection category

Discovery source

Current category progress

Lifetime Obtained

Example:

New Collection Item Discovered

Blackshard

Boss Drop

Equipment Collection: 42 / 120

Multiple discoveries may be grouped.

Example:

5 New Collection Items Discovered

---

# Collection User Interface

The Collection Log UI should display:

Overall completion percentage

Total items discovered

Total required items

Collection categories

Category completion percentages

Item entries

Search

Filters

Sorting

Recently discovered items

Milestone rewards

Unclaimed rewards

The Collection Log should be easy to use even with thousands of items.

---

# Collection Entry UI

Each discovered item entry should display:

Item icon

Item name

Description

Category

Type

Primary use

First discovery source

Discovery time

Current owned quantity

Lifetime obtained quantity

Highest quantity owned

Lifetime crafted if applicable

Lifetime gathered if applicable

Lifetime dropped from combat if applicable

Lifetime consumed if applicable

Lifetime sold if applicable

Known sources

Related recipe

Related profession

Related region

Related enemy, boss, or dungeon source

Upgrade path if applicable

---

# Undiscovered Entry UI

An undiscovered entry may display:

Unknown icon or silhouette

Unknown name or visible name

Possible source

Required profession

Required region

Related recipe

Drop chance if visible

Whether it counts toward completion

Hidden information should follow the entry's hidden display rules.

---

# Collection Filters

Possible filters:

All Items

Discovered

Undiscovered

Recently Discovered

Resources

Equipment

Weapons

Armor

Tools

Accessories

Relics

Consumables

Food

Potions

Elixirs

Runes

Artifacts

Companion Items

Profession Items

Combat Drops

Crafted Items

Boss Drops

Dungeon Rewards

Optional Entries

Filters should help the player find specific item goals.

---

# Collection Sorting

Possible sorting options:

Category

Name

Discovered First

Undiscovered First

Recently Discovered

Lifetime Obtained

Current Owned Quantity

Profession

Region

Source

Item Type

Sorting should make the Collection Log easy to navigate.

---

# Collection Search

Collection search should match:

Item name

Item type

Category

Profession

Region

Enemy source

Boss source

Dungeon source

Recipe

Primary use

Search should respect hidden item rules.

Hidden item names should not appear before discovery unless explicitly allowed.

---

# Item Tracking

The player may track specific undiscovered items.

Tracked item information may display:

Item name or silhouette

Known sources

Required profession level

Required region

Required recipe

Drop chance

Current owned quantity

Suggested starting limit:

5 tracked items

Tracked item goals may appear in a small UI panel.

---

# Collection Completion Summary

The Collection Log should display:

Total items discovered

Total required items

Overall completion percentage

Category completion percentages

Profession group completion

Recently discovered items

Rarest discovered items

Highest lifetime item quantities

Available milestone rewards

Unclaimed rewards

The summary should make long-term account progress easy to understand.

---

# Collection Statistics

The game should track:

Total unique items discovered

Total item entries

Overall collection percentage

Items discovered by category

Items discovered from professions

Items discovered from crafting

Items discovered from combat

Items discovered from bosses

Items discovered from dungeons

Items discovered from shops

Items discovered offline

First item discovered

Most recent item discovered

Rarest item discovered

Highest lifetime quantity for an item

---

# Collection Save Data

Collection save data should include:

Collection Entry ID or Item ID

Discovered Status

Discovery Time

First Discovery Source

Lifetime Obtained Quantity

Highest Quantity Owned

Source-specific quantities if not stored elsewhere

Claimed Milestone Rewards

Claimed Completion Rewards

Tracked Status

Static item definitions should remain in the Item Database.

Current Owned Quantity should be read from Inventory instead of duplicated when possible.

---

# Collection Database

Collection categories and special entry rules should exist inside the Collection Database.

Standard item entries may be generated from the Item Database.

Collection data should connect to:

Item Database

Inventory System

Equipment System

Profession System

Crafting System

Combat System

Enemy Database

Boss Database

Dungeon Database

Companion System

Achievement System

Statistics System

Save System

Offline Progress System

Shop System

Region System

---

# Technical Rules

The Collection Log should be data-driven.

Item IDs should remain stable.

Every collectible item should define whether it counts toward Collection completion.

Item acquisition systems should send one shared item-obtained event.

The shared event should include:

Item ID

Quantity

Acquisition Source

Source ID if applicable

Timestamp

The Collection System should use this event to:

Mark discovery

Increase Lifetime Obtained

Record the first discovery source

Update completion

Trigger notifications

Systems should not implement separate discovery logic for individual items.

Adding a new item should not require rewriting Collection System code.

Missing Item IDs should not crash the game.

Collection progress should save immediately after first discovery.

---

# Items Excluded From Collection

Some items may be excluded from the main Collection Log.

Possible exclusions:

Developer test items

Temporary debug items

Placeholder items

Removed items

Unavailable future items

Internal system tokens

Temporary combat-only objects

Every item should define:

Counts Toward Collection

Excluded items should not affect completion percentage.

---

# Optional and Event Items

Optional items may appear in the Collection Log without affecting main completion.

Event items should define:

Event ID

Availability

Counts Toward Main Completion

Permanent Availability

Event items should not normally block permanent Collection completion after an event ends.

They may appear in a separate Event Items category.

---

# Balance Philosophy

The Collection Log should encourage players to explore all item-producing systems.

The player should find items through:

Professions

Crafting

Combat

Bosses

Dungeons

Shops

Achievements

Companions

Rare discoveries

The Collection Log should not require unreasonable quantities.

Obtaining one copy should normally complete an item entry.

Lifetime quantity should be informative and useful for achievements, not required for basic discovery completion.

Rare entries should feel exciting.

Avoid adding items only to inflate the Collection Log.

---

# Rare Item Protection

Future option:

Extremely rare items that block Collection completion may use bad-luck protection.

Possible systems:

Increasing drop chance after repeated failures

Guaranteed drop after a large number of attempts

Boss token exchange

Duplicate item conversion

Collection currency

Rare item protection should be used carefully.

The goal is to prevent one extremely unlucky drop from blocking completion forever.

---

# Future Expansion

Possible additions:

Museum donations

Item lore

Collection titles

Collection badges

Player profile showcase

Rarest item showcase

Collection milestone shop

Regional item collections

Item set collections

Duplicate conversion

Collection currency

Trading

Seasonal item collections

Event item collections

Account-wide Collection Log

Future collection systems should expand the item-based Collection Log rather than replace it.

---

# Checklist

Every Collection Log item entry should answer:

✓ What Item ID does it reference?

✓ How is the item obtained?

✓ What discovers the entry?

✓ Does it count toward completion?

✓ Is it hidden before discovery?

✓ What source information is visible?

✓ Does it track Lifetime Obtained?

✓ Does it display Current Owned Quantity?

✓ Does it track source-specific quantities?

✓ Can it be obtained offline?

✓ Does it remain discovered permanently?

✓ Does it connect to achievements?

✓ Does it save and load correctly?

✓ Can it complete retroactively?

Every item-producing system should answer:

✓ Does it send the shared item-obtained event?

✓ Does it include the correct Item ID?

✓ Does it include the obtained quantity?

✓ Does it include the acquisition source?

✓ Does it update Lifetime Obtained?

✓ Does it trigger first-time discovery?

✓ Does it work during offline progress?

✓ Does it avoid counting items that were permanently lost?

Every collection category should answer:

✓ Which item types belong to it?

✓ Which entries count toward completion?

✓ Are any entries optional?

✓ Are event items separated?

✓ Does it have milestone rewards?

✓ Can it expand when new items are added?