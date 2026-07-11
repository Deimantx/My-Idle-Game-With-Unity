# 08 Inventory Framework

Version: 1.0
Status: Draft

---

# Purpose

This document defines how inventory works across the entire game.

The inventory system stores items gained from professions, combat, crafting, shops, companions, achievements, dungeons, bosses, offline progress, and future systems.

Inventory should be easy to understand, scalable, data-driven, reliable, and safe.

---

# Design Goals

Inventory should:

- Store all player-owned items.
- Support resources, equipment, consumables, currencies, companion items, collectibles, and future item types.
- Be easy to search, sort, and filter.
- Clearly show item purpose.
- Connect to crafting, equipment, combat, companions, shops, and professions.
- Avoid unnecessary micromanagement.
- Support long-term collection and progression.
- Protect important items from accidental loss.
- Be safe for save/load systems.

Inventory should never feel annoying to manage.

---

# Inventory Philosophy

The inventory exists to support progression, not block it.

The player should quickly understand:

What they own

How much they own

What each item is used for

Where items come from

Whether items are important

Whether items can be sold, consumed, crafted, equipped, combined, upgraded, or used for companion progression

The inventory should become more powerful over time without becoming messy.

Inventory limits may exist, but they should create progression goals rather than constant frustration.

---

# Inventory Structure

The inventory should track:

Item ID

Quantity

Item Category

Item Type

Stackable Status

Maximum Stack Size

Obtained Status

Locked Status

Favorite Status

New Item Status

Inventory Slot Usage

Future Expansion Notes

The inventory should reference the Item Database.

The inventory should not duplicate static item data.

---

# Item Storage Rules

Items are stored by Item ID.

Stackable items increase quantity in the same inventory entry.

Non-stackable items may create separate inventory entries if needed.

Default behavior:

Resources are stackable.

Consumables are stackable.

Crafting materials are stackable.

Currencies are stackable.

Equipment is usually not stackable.

Collectibles may be stackable or non-stackable depending on design.

Future systems may override these rules.

---

# Item Categories

Inventory should support the categories defined in the Item Framework.

Current categories:

Resources

Equipment

Consumables

Crafting Materials

Companion Items

Currencies

Collectibles

Future categories may be added later.

---

# Item Types

Inventory should support item types defined in the Item Framework.

Examples:

Log

Ore

Bar

Gem

Herb

Fish

Rune

Food

Potion

Elixir

Hide

Weapon

Armor

Tool

Accessory

Relic

Material

Currency

Book

Miscellaneous

Item types should be used for filters, sorting, crafting requirements, and UI organization.

---

# Inventory Capacity

Current design:

Inventory slot limit is 100 by default.

The player can spend gold to increase this limit.

The player should not lose important progress because inventory is full too early.

Inventory capacity should matter, but it should not become annoying constant friction.

Future option:

Special systems may use limited storage if it improves gameplay.

Examples:

Dungeon chest capacity

Temporary loot bag

Temporary Combat Loot

Event storage

Companion expedition storage

Avoid inventory limits that feel like annoying mobile-game friction.

---

# Inventory Capacity Upgrade Rules

Inventory starts with 100 slots by default.

The player can spend gold to increase inventory capacity.

Inventory capacity upgrades should become more expensive over time.

Inventory upgrades should feel useful, but not mandatory every few minutes.

Example upgrade path:

100 slots

125 slots

150 slots

200 slots

250 slots

300 slots

Future expansion may allow additional capacity from:

Achievements

Companions

Relics

Shop upgrades

Account progression

Inventory capacity should create progression goals without becoming annoying.

---

# Inventory Full Rules

If inventory is full, the game should clearly explain what happens.

Profession rewards:

If inventory is full, profession activity should pause or stop.

The player should receive a clear message explaining that inventory is full.

Combat loot:

Combat loot goes into Temporary Combat Loot first.

The player can choose what to pick up.

Offline rewards:

Offline rewards should try to add items to inventory.

If inventory is full, rewards should go to an Offline Reward Claim screen when possible.

Important items should never be silently deleted.

Unique items, first-time drops, boss drops, dungeon rewards, rare progression items, relics, and companion rank materials should be protected from automatic loss whenever possible.

---

# Stack Rules

Every stackable item should define:

Current Quantity

Maximum Stack Size

Stackable Status

If maximum stack size is reached, future copies may create another stack or be handled by system rules.

Current recommendation:

Use very high stack sizes for most idle-game resources.

The player should not constantly fight stack limits.

---

# Equipment Storage

Equipment items are stored in inventory until equipped, upgraded, sold, destroyed, or used as materials.

Equipment should display:

Icon

Name

Type

Slot

Stats

Bonuses

Requirements

Upgrade Path

Comparison with currently equipped item

Equipment inventory should connect to the Equipment Framework.

---

# Consumable Storage

Consumables include:

Food

Potions

Elixirs

Other future consumables

Consumables should display:

Quantity

Effect

Duration if applicable

Combat loadout compatibility

Profession use if applicable

Consumables should connect to combat, professions, and crafting.

---

# Currency Storage

Currencies may include:

Gold

Future special currencies

Event tokens

Dungeon tokens

Currency-like items should be clearly separated from regular resources when needed.

Gold may be shown outside the main inventory UI if it is important enough.

---

# Item Locking

Players should be able to lock important items.

Locked items cannot be:

Sold

Destroyed

Used as crafting material

Used as companion rank-up material

Used as upgrade material

Unless the player manually unlocks or confirms the action.

Item locking prevents accidental loss of important resources.

---

# Favorite Items

Players may mark items as favorites.

Favorite items should be easier to find.

Favorites may appear at the top of inventory or in a separate filter.

Favorite status should save and load correctly.

---

# New Item Indicator

When the player obtains an item for the first time, it should be marked as new.

New item indicators help the player notice progression.

The new indicator should disappear when the player views the item.

First-time item discovery should also update statistics and collection systems.

---

# Item Actions

Items may support actions.

Possible item actions:

View Details

Equip

Unequip

Consume

Use

Craft With

Upgrade

Combine

Sell

Destroy

Lock

Favorite

View Source

View Recipes

View Used In

Only valid actions should appear for each item.

Do not show buttons that do nothing.

---

# Item Details Panel

Every item should have a details panel.

The details panel should show:

Icon

Name

Description

Category

Type

Quantity

Primary Use

Source

Sell Value

Requirements

Used In Recipes

Used For Upgrades

Used For Companion Rank-Ups

Can Be Sold

Can Be Destroyed

Can Be Equipped

Can Be Combined

Can Be Consumed

The player should understand why the item matters.

---

# Item Source and Usage Preview

The item details panel should help the player understand where an item comes from and where it is used.

Every item details panel should support:

View Sources

View Used In

View Related Recipes

View Related Equipment

View Related Companion Rank-Ups

View Related Unlocks

This helps the player understand why an item matters.

Example:

Iron Ore

Sources:

Mining Iron Node

Dungeon Chest

Shop

Used In:

Iron Bar

Iron Pickaxe

Iron Sword

Companion Rank-Up

---

# Sorting

Inventory should support sorting.

Possible sorting options:

Name

Quantity

Category

Type

Recently Obtained

Sell Value

Profession Source

Combat Source

Equipment Slot

Usable First

Locked First

Favorites First

Sorting should make inventory easier to manage, not more complicated.

---

# Filtering

Inventory should support filters.

Possible filters:

All Items

Resources

Equipment

Consumables

Crafting Materials

Companion Items

Currencies

Collectibles

Favorites

Locked Items

New Items

Usable Items

Profession Materials

Combat Drops

Filters should be simple and clear.

---

# Search

Inventory should support text search.

Search should match:

Item Name

Item Type

Item Category

Possible Source

Possible Use

Search is important once the game has hundreds or thousands of items.

---

# Selling Items

Items may be sellable.

Selling should grant gold or future currency.

Sell rules:

Only sellable items can be sold.

Locked items cannot be sold unless manually unlocked.

Important progression items may be unsellable.

Sell confirmation should appear for rare or important items.

Bulk selling may be supported later.

---

# Auto-Sell Rules

Auto-sell is a future quality-of-life feature.

Auto-sell may allow the player to automatically sell selected items.

Auto-sell should never apply to:

Locked items

Newly discovered items

Unique items

Progression items

Boss drops

Dungeon rewards

Companion rank-up materials

Relics

Auto-sell should require clear player confirmation.

Auto-sell rules should be optional and easy to disable.

---

# Destroying Items

Some items may be destroyable.

Destroying removes the item without reward.

Destroy rules:

Only destroyable items can be destroyed.

Locked items cannot be destroyed unless manually unlocked.

Important progression items may be non-destroyable.

Destroy confirmation should appear for valuable items.

Destroying should rarely be necessary.

---

# Crafting Interaction

Crafting systems should read inventory quantities and show requirements in UI.

Crafting should check:

Required Items

Required Quantity

Required Profession Level

Required Equipment

Required Gold

Locked Item Status

Crafting should consume materials only when crafting completes.

Crafting should not consume locked items.

If crafting uses important items, the UI should clearly warn the player.

---

# Equipment Interaction

Equipment systems should read equipment from inventory.

Equipping an item should:

Check requirements

Move item to equipped state

Apply bonuses

Update comparison UI

Save state

Unequipping should return item to inventory unless equipment rules say otherwise.

If inventory is full when unequipping, the game should prevent unequipping or provide a safe overflow rule.

Equipment should never be lost silently.

---

# Companion Interaction

Companion systems should read inventory quantities for rank-up requirements.

Companion rank-up should check:

Required items

Required quantity

Required gold

Locked item status

Rank-up timer availability

Items should be consumed when rank-up begins.

Companion-related item uses should be clearly visible in the item details panel.

---

# Combat Interaction

Combat may generate loot.

Combat loot should be added to Temporary Combat Loot after:

Enemy kill

Dungeon completion

Boss kill

Reward chest

The player can then pick up individual loot or use a Pick All button in the Temporary Combat Loot UI.

If the main inventory has enough space, picked-up loot should move into inventory.

If inventory is full, loot should remain in Temporary Combat Loot when possible.

If Temporary Combat Loot is full, common low-value items may be lost first.

Unique items, rare drops, boss drops, dungeon rewards, first-time drops, relics, and progression items should not be lost silently.

If future limited loot systems exist, the UI should clearly explain what happens to overflow.

---

# Temporary Combat Loot Rules

Combat loot first enters Temporary Combat Loot.

Temporary Combat Loot is shown in the Combat UI.

The player may:

Pick up individual items

Pick up all items

Leave items temporarily

Temporary Combat Loot should define:

Maximum item entries

Maximum stack behavior

What happens when it is full

What happens when combat ends

What happens when the player leaves combat

What happens when the player closes the game

Important loot should be protected where possible.

If Temporary Combat Loot is full, common low-value items may be lost first.

Unique, rare, boss, dungeon, first-time, or progression items should not be lost silently.

The UI should clearly warn the player before loot can be lost.

---

# Profession Interaction

Professions may generate resources.

Profession rewards should be added to inventory after each completed action.

Examples:

Woodcutting adds logs.

Mining adds ores or gems.

Fishing adds fish.

Foraging adds herbs or materials.

Profession rewards should update inventory, statistics, achievements, and collection systems.

If inventory is full, the profession should pause or stop until the player frees space.

---

# Offline Progress Interaction

Offline progress may generate items.

When the player returns, offline rewards should be added to inventory if possible.

Offline reward summary should display:

Items gained

Quantities gained

Rare drops

First-time items

Items used

Items consumed

Items that could not fit

Inventory changes from offline progress should be reliable and clearly shown.

If inventory cannot fit all offline rewards, the game should use an Offline Reward Claim screen or protected overflow system when possible.

Important offline rewards should not be silently deleted.

---

# Collection Interaction

The inventory should connect to the collection log.

When the player obtains an item for the first time:

Mark item as discovered.

Update collection progress.

Trigger collection notifications.

Check achievements.

Collection status should not depend on keeping the item forever.

Once discovered, it remains discovered, even if sold, lost, consumed, or destroyed.

---

# Shop Interaction

Shops may buy or sell items.

Shop systems should check:

Inventory quantity

Gold amount

Item sell rules

Item buy rules

Locked item status

Inventory space

Shop transactions should update inventory immediately.

If inventory is full, the player should not be able to buy items that require inventory space.

---

# Inventory Statistics

The game should track:

Total items obtained

Total unique items discovered

Total items consumed

Total items sold

Total items destroyed

Total gold earned from selling

Highest quantity owned per item

First time obtained per item

Total items crafted

Total combat loot gained

Total profession resources gained

Total items lost due to full inventory

Total items protected from loss

---

# Inventory User Interface

Inventory UI should display:

Item icon

Item name

Quantity

Category

Type

New item indicator

Locked indicator

Favorite indicator

Search bar

Sort options

Filter options

Item details panel

Action buttons

Inventory capacity

Used slots

Free slots

Inventory UI should be clear, readable, and fast to use.

---

# Inventory UI Rules

The player should never need to guess what an item does.

Important item information should be visible or one click away.

Common actions should be easy to access.

Dangerous actions such as selling rare items or destroying items should require confirmation.

Inventory should remain usable even with hundreds or thousands of items.

---

# Inventory Warning Rules

The inventory UI should warn the player when:

Inventory is almost full

Inventory is full

Temporary Combat Loot is almost full

Temporary Combat Loot is full

A rare item is about to be sold

A locked item would be consumed

A progression item would be destroyed

Offline rewards cannot fully fit

Warnings should be clear but not annoying.

---

# Inventory Database Connection

Inventory should reference the Item Database.

The Item Database defines:

Item name

Description

Category

Type

Icon

Value

Stackable status

Maximum stack size

Sellable status

Destroyable status

Primary use

Source

Inventory stores ownership and quantity.

Do not duplicate static item data inside inventory save data.

---

# Save System Rules

Inventory must save:

Item IDs

Quantities

Locked status

Favorite status

New item status

Inventory capacity

Temporary Combat Loot if needed

Offline Reward Claim data if needed

Equipped state if handled by inventory

Inventory must load safely even if item data changes in future updates.

If an item ID is missing from the database, the game should not crash.

Missing items should be handled safely.

---

# Safety Rules

Inventory should protect the player from accidental loss.

Important items should not be lost silently.

Important item types may include:

Unique equipment

Boss drops

Dungeon rewards

First-time collection items

Companion rank materials

Rare crafting materials

Relics

Future progression items

If an important item cannot be stored, the game should show a warning, temporary claim screen, or protected overflow system.

---

# Technical Rules

Inventory should be data-driven.

Inventory should reference Item IDs.

Inventory should not hardcode item behavior.

Inventory should connect to:

Item Database

Equipment System

Profession System

Combat System

Crafting System

Companion System

Shop System

Achievement System

Statistics System

Collection System

Save System

Offline Progress System

Inventory actions should be modular.

Adding a new item should not require editing inventory code.

Inventory full behavior should be consistent across professions, combat, shops, crafting, and offline progress.

---

# Future Expansion

Possible additions:

Inventory tabs

Loadout storage

Equipment presets

Consumable presets

Bulk selling

Auto-sell rules

Auto-lock rules

Item tags

Item search improvements

Material bank

Separate equipment storage

Loot filters

Collection log integration

Museum system

Trading system

Auction system

Protected overflow storage

Offline reward claim storage

Future inventory systems should expand the base framework rather than replace it.

---

# Checklist

Every inventory feature should answer:

✓ What item categories does it support?

✓ Does it use Item IDs?

✓ Does it respect locked items?

✓ Does it support stackable items?

✓ Does it support non-stackable items?

✓ Does it handle inventory full behavior?

✓ Does it protect important items?

✓ Does it connect to crafting?

✓ Does it connect to equipment?

✓ Does it connect to companions?

✓ Does it connect to combat loot?

✓ Does it connect to profession rewards?

✓ Does it save and load correctly?

✓ Does it work with offline progress?

✓ Is it easy for the player to understand?

✓ Can it support future expansion?