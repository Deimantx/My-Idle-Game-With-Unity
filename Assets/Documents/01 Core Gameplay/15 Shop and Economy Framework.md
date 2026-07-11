# 15 Shop and Economy Framework

Version: 1.0  
Status: Draft

---

# Purpose

This document defines how shops, Gold, buying, selling, prices, unlocks, and the wider game economy work.

The economy connects:

Professions

Crafting

Combat

Inventory

Equipment

Companions

Consumables

Runes

Achievements

Collections

Regions

Dungeons

Bosses

Future systems

The economy should give resources and items meaningful value without allowing shops to replace normal gameplay progression.

---

# Design Goals

The Shop and Economy System should:

- Give Gold meaningful long-term uses.
- Give unwanted items a useful purpose through selling.
- Support professions without replacing them.
- Support combat without selling the best rewards directly.
- Provide useful progression unlocks.
- Keep old resources valuable when possible.
- Avoid excessive inflation.
- Avoid mandatory daily purchases.
- Be easy to understand.
- Be fully data-driven.
- Support future regions, currencies, and shops.
- Respect inventory capacity and item-locking rules.

The economy should reward playing the game rather than bypassing it.

---

# Economy Philosophy

Gold is a universal progression resource.

The player earns Gold through:

Selling items

Combat rewards

Profession activities

Dungeon rewards

Boss rewards

Achievements

Shops and future systems

The player spends Gold on:

Items

Materials

Equipment

Consumables

Recipes

Inventory capacity

Companion progression

Equipment upgrades

Region unlocks

Profession services

Future systems

Gold should remain useful throughout the entire game.

---

# Main Currency

Current main currency:

Gold

Gold should be used across most general systems.

Gold may be displayed permanently in the main UI.

Gold should not use inventory slots.

Gold should save and load as account progression data.

---

# Future Currencies

Future systems may introduce additional currencies.

Examples:

Dungeon Tokens

Arena Currency

Collection Currency

Achievement Points

Region Currency

Special currencies should have clear purposes.

Avoid adding too many currencies.

A new currency should only exist when Gold cannot support the system cleanly.

---

# Currency Rules

Every currency should define:

Currency ID

Name

Description

Icon

Maximum Amount if applicable

Primary Sources

Primary Uses

Can Be Purchased

Can Be Sold

Can Be Converted

Counts as Inventory Item

Future Expansion Notes

Gold should not normally have a maximum amount.

Special currencies may have limits if necessary.

---

# Shop Categories

Possible shop categories:

General Shop

Profession Shop

Combat Shop

Equipment Shop

Consumable Shop

Recipe Shop

Companion Shop

Rune Shop

Region Shop

Dungeon Shop

Boss Token Shop

Achievement Shop

Event Shop

Future categories may be added later.

---

# General Shop

The General Shop provides basic useful items.

Possible stock:

Basic profession tools

Low-tier materials

Basic food

Basic potions

Low-tier equipment

Inventory upgrades

Simple recipes

General utility items

The General Shop should help early progression.

It should not sell rare or endgame rewards.

---

# Profession Shops

Profession shops support specific professions.

Examples:

Woodcutting Shop

Mining Shop

Fishing Shop

Cooking Shop

Smithing Shop

Herblore Shop

Farming Shop

Hunting Shop

Archaeology Shop

Runecrafting Shop

Profession shops may sell:

Basic tools

Basic materials

Seeds

Bait

Containers

Low-tier recipes

Profession unlocks

Convenience items

Profession shops should support progression without replacing profession activities.

---

# Combat Shop

The Combat Shop may sell:

Basic weapons

Basic armor

Food

Potions

Elixirs

Combat preparation items

Entry-level runes

Basic equipment upgrade materials

The Combat Shop should not sell the strongest boss or dungeon equipment directly.

Combat should remain the main source of powerful combat rewards.

---

# Recipe Shop

The Recipe Shop may sell:

Low-tier recipes

Basic profession recipes

Regional recipes

Utility recipes

Consumable recipes

Recipe unlocks should cost Gold or another relevant currency.

Important high-tier recipes should usually come from:

Profession progression

Bosses

Dungeons

Achievements

Rare drops

Regions

Collections

Recipe shops should fill gaps without replacing exploration and progression.

---

# Companion Shop

The Companion Shop may sell:

Basic companion unlock items

Common companion rank materials

Companion-related crafting materials

Companion utility items

The Companion Shop should not sell every companion.

Rare companions should still come from meaningful gameplay.

Companion rank progression should continue requiring items and time.

---

# Rune Shop

The Rune Shop may sell:

Basic runes

Low-tier Runecrafting materials

Rune recipes

Rune loadout unlocks if needed

The Rune Shop should not replace Runecrafting.

Most strong runes should be crafted or earned through progression.

---

# Regional Shops

Regions may contain unique shops.

Regional shops may sell:

Region-specific materials

Regional recipes

Local equipment

Consumables

Region unlock items

Profession tools

Cosmetic items

Regional shop stock may require the region to be unlocked.

Some items may require additional progression.

---

# Shop Structure

Every shop should define:

Shop ID

Name

Description

Icon

Shop Category

Region if applicable

Unlock Requirements

Currency Used

Stock List

Buy Rules

Sell Rules

Restock Rules

Price Modifiers

Future Expansion Notes

Shops should be fully data-driven.

---

# Shop Entry Structure

Every shop entry should define:

Shop Entry ID

Shop ID

Item ID

Buy Price

Sell Price Override if applicable

Purchase Limit if applicable

Stock Amount if applicable

Restock Time if applicable

Unlock Requirement

Required Profession Level if applicable

Required Combat Discipline Level if applicable

Required Region if applicable

Required Achievement if applicable

Required Dungeon Completion if applicable

Required Companion Rank if applicable

Future Expansion Notes

---

# Buying Items

When buying an item, the system should check:

Shop is unlocked

Item is available

Player has enough currency

Player has enough inventory space

Purchase limit has not been reached

Requirements are met

If all requirements are met:

Remove currency.

Add item to inventory.

Update collection discovery if the item is new.

Update statistics.

Check achievements.

Save progress.

---

# Buying and Inventory Capacity

Items cannot be purchased if inventory has no space.

Stackable items may still be purchased if they fit into an existing stack.

Non-stackable items require an available inventory slot.

The Shop UI should show:

Required inventory space

Current free slots

Maximum affordable quantity

Maximum quantity that fits

Items should never be lost because the player purchased them with a full inventory.

---

# Buying Quantities

For stackable items, the player may choose a purchase quantity.

Possible quantity controls:

Buy 1

Buy 10

Buy 100

Buy Maximum

Custom Amount

The UI should show:

Price per item

Total price

Current owned quantity

Inventory space required

Maximum affordable quantity

Maximum storable quantity

---

# Selling Items

The player may sell eligible items for Gold or another defined currency.

Selling should check:

Item is sellable

Item is not locked

Quantity is available

Item is not currently equipped

Item is not reserved for crafting

Item is not used by an active companion rank-up

Item is not protected by another system

Selling should remove the item and grant the correct currency.

---

# Selling Quantities

For stackable items, the player may choose:

Sell 1

Sell 10

Sell 100

Sell All

Custom Amount

The UI should show:

Sell value per item

Total sale value

Current owned quantity

Remaining quantity

Locked or reserved quantity

The player should not accidentally sell all copies of an important item.

---

# Sell Confirmation Rules

Confirmation should be required when selling:

Equipment

Relics

Rare items

Boss drops

Dungeon rewards

Companion rank materials

Progression items

Items used in upgrades

Large quantities

Items marked as favorites

Locked items cannot be sold until unlocked.

Common low-value resources may be sold without repeated confirmation.

---

# Item Locking and Shops

Locked items cannot be sold.

Locked items should be excluded from Sell All.

Reserved crafting materials cannot be sold.

Equipped items cannot be sold.

Items used in active systems should be protected.

The Shop UI should clearly show why an item cannot be sold.

---

# Buy Price Rules

Every purchasable item should define a Buy Price.

Buy Price may depend on:

Base item value

Item tier

Item usefulness

Item rarity classification if used

Region

Shop type

Progression level

Limited stock

Future modifiers

Prices should be defined in shop data or balancing data.

Do not hardcode prices inside shop scripts.

---

# Sell Price Rules

Every sellable item should define a Sell Value.

Default rule:

Sell Price should be lower than Buy Price.

This prevents unlimited buy-and-sell loops.

Example:

Buy Price:

100 Gold

Sell Price:

20 Gold

Exact ratios may vary by item type.

---

# Base Item Value

The Item Framework defines an item's base Value.

Base Value may be used for:

Sell prices

Shop prices

Crafting cost calculations

Equipment upgrade costs

Achievement statistics

Economic balancing

Base Value should not automatically become the final shop price.

Shops may apply their own price modifiers.

---

# Price Modifiers

Prices may be modified by:

Shop type

Region

Achievements

Companions

Relics

Future reputation systems

Limited stock

Special events

Account progression

Price modifiers should be limited and clearly shown.

Avoid stacking so many discounts that prices become meaningless.

---

# Buy Price Formula

A possible starting formula:

Final Buy Price = Base Buy Price × Shop Modifier × Account Modifier

Exact formulas should be stored in balancing documents.

The UI should show the final price.

If a discount is active, the UI may show:

Original Price

Discount Amount

Final Price

---

# Sell Price Formula

A possible starting formula:

Final Sell Price = Base Sell Value × Sell Modifier

Sell modifiers may come from:

Achievements

Companions

Relics

Future economy upgrades

Selling bonuses should remain modest.

Players should not earn more by buying and reselling the same item.

---

# Price Floor

Prices should never become negative.

Gold costs should normally have a minimum value of 1 when a purchase requires Gold.

Discounts should not reduce important items to free unless explicitly designed.

---

# Shop Stock Rules

Shops may use different stock systems.

Possible stock types:

Unlimited Stock

Limited Stock

One-Time Purchase

Account-Limited Stock

Timed Restock

Progression-Based Stock

Event Stock

Most basic materials should use Unlimited Stock.

Rare or valuable items may use limited or one-time stock.

---

# Unlimited Stock

Unlimited Stock items may be purchased repeatedly.

Examples:

Basic materials

Basic food

Basic potions

Low-tier tools

Unlimited stock should only be used for items that do not damage progression balance.

---

# Limited Stock

Limited Stock items have a maximum available quantity.

Limited stock should define:

Maximum Stock

Current Stock

Restock Time

Restock Amount

Purchase Limit

The player should clearly see when stock returns.

Avoid using limited stock only to create unnecessary waiting.

---

# One-Time Purchases

One-time purchases may include:

Inventory capacity upgrades

Permanent shop upgrades

Region unlocks

Recipe unlocks

Additional loadout slots

Quality-of-life upgrades

Once purchased, one-time upgrades should remain unlocked permanently.

---

# Restock Rules

Shops may restock over time.

Restock timers may continue while the game is closed.

Every restocking entry should define:

Restock Interval

Restock Amount

Maximum Stock

Offline Restock Allowed

Restock timers should save and load correctly.

Restocking should not require the player to stay online.

---

# Inventory Capacity Purchases

Inventory capacity can be increased by spending Gold.

Current starting inventory capacity:

100 slots

Capacity upgrades may follow a progression such as:

125 slots

150 slots

200 slots

250 slots

300 slots

Each upgrade should define:

Upgrade ID

Required Gold

Current Capacity Requirement

New Capacity

Purchase Limit

Inventory capacity upgrades should become more expensive over time.

They should remain valuable long-term Gold sinks.

---

# Gold Sources

Gold may come from:

Selling items

Regular combat

Elite combat

Bosses

Dungeons

Profession activities

Achievements

Collection rewards

Companion bonuses

Offline progress

Future systems

No single Gold source should completely replace all others.

---

# Selling as a Gold Source

Selling items should be an important source of Gold.

However, selling should create meaningful decisions.

Examples:

Sell logs now or save them for Carpentry?

Sell ore or use it for Smithing?

Sell boss material or save it for an equipment upgrade?

Sell food or use it for combat?

Sell companion materials or save them for rank-ups?

These decisions create economic depth.

---

# Direct Gold Rewards

Some activities may reward Gold directly.

Possible sources:

Combat

Thieving

Dungeon completion

Boss rewards

Achievements

Shops and economy events

Direct Gold rewards should be balanced against item-selling income.

---

# Gold Sinks

Gold sinks remove Gold from the economy.

Important Gold sinks may include:

Buying items

Inventory capacity upgrades

Crafting costs

Equipment upgrades

Companion rank-ups

Recipes

Region unlocks

Profession unlocks

Rune loadout upgrades

Future research

Future buildings

Gold sinks help Gold remain useful.

---

# Gold Sink Philosophy

Gold sinks should feel like progression investments.

Good Gold sinks:

Unlock new options.

Improve convenience.

Support equipment progression.

Support companions.

Support crafting.

Expand inventory.

Open regions.

Bad Gold sinks:

Constant meaningless fees.

Punishing maintenance costs.

Repeated charges for basic actions.

Systems that exist only to remove Gold.

Gold spending should usually feel rewarding.

---

# Crafting Costs

Crafting recipes may require Gold.

Gold costs may represent:

Crafting services

Fuel

Tool maintenance

Special materials not represented as items

Complex production

Crafting Gold costs should not make basic crafting feel overly expensive.

High-tier equipment and upgrades may require larger Gold investments.

---

# Equipment Upgrade Costs

Equipment upgrades may require:

Gold

Base equipment

Crafting materials

Boss materials

Dungeon materials

Profession levels

Combat Discipline levels

Upgrade costs should increase with equipment power.

Gold should be one part of an upgrade, not always the only requirement.

---

# Companion Rank-Up Costs

Companion rank-ups may require Gold.

Gold costs should increase at higher ranks.

A rank-up may require:

Items

Gold

Time

Special materials

The player should clearly see all requirements before starting the rank-up.

Companion rank-up Gold costs should support long-term progression.

---

# Region Unlock Costs

Some regions may require Gold to unlock.

Region unlocks may also require:

Profession level

Combat Discipline level

Item

Dungeon completion

Boss defeat

Achievement

Region unlock costs should feel like meaningful progression gates.

Gold alone should not unlock every region.

---

# Shop Unlock Requirements

A shop may require:

Region unlocked

Profession level

Combat Discipline level

Dungeon completion

Boss defeat

Achievement

Companion rank

Item

Gold payment

Future progression system

Locked shops should show their unlock requirements when appropriate.

Secret shops may remain hidden.

---

# Shop Stock Unlocks

Individual shop items may unlock separately from the shop.

Examples:

Better tools unlock at higher Profession levels.

Combat equipment unlocks at higher Combat Discipline levels.

Regional materials unlock after entering a region.

Recipes unlock after dungeon completion.

Companion items unlock after finding a companion.

Shop progression should reflect player progression.

---

# Shop and Professions

Shops should support professions without replacing them.

Shops may sell:

Basic resources

Low-tier tools

Seeds

Bait

Containers

Low-tier recipes

Convenience materials

Shops should not provide unlimited access to the best profession resources.

High-tier and rare materials should come primarily from profession activities.

---

# Shop and Crafting

Shops may provide crafting support.

Possible stock:

Basic ingredients

Low-tier materials

Recipe unlocks

Tools

Containers

Fuel-like resources

Rare crafting components should usually come from:

Combat

Bosses

Dungeons

Professions

Achievements

Collections

Crafting should remain connected to gameplay.

---

# Shop and Combat

Combat provides Gold and items that may be sold.

Shops provide preparation items such as:

Food

Potions

Elixirs

Basic equipment

Basic runes

Combat shops should help the player prepare.

They should not allow the player to buy the strongest combat progression without engaging with combat.

---

# Shop and Equipment

Equipment may be bought and sold.

Shops may sell:

Starting equipment

Low-tier weapons

Low-tier armor

Basic tools

Utility accessories

Strong equipment should mainly come from:

Crafting

Boss drops

Dungeon rewards

Equipment upgrades

Rare discoveries

---

# Shop and Companions

Shops may sell common companion materials.

Rare companion materials should come from their connected gameplay systems.

Examples:

Combat companion materials from combat.

Profession companion materials from professions.

Rare rank materials from bosses and dungeons.

Shops may help fill small material gaps.

---

# Shop and Runes

Basic runes and Runecrafting materials may be sold.

Strong runes and rune sets should require:

Runecrafting

Recipes

Rare materials

Combat progression

Region progression

Shops should not replace the player's rune crafting progression.

---

# Shop and Collections

Buying an item for the first time should discover it in the Collection Log.

Viewing an item in a shop does not discover it.

The item must be successfully purchased and enter player ownership or protected claim storage.

Selling an item does not remove Collection discovery.

Shop purchases should update Lifetime Obtained statistics.

---

# Shop and Achievements

Shop-related achievements may track:

Gold earned

Gold spent

Items sold

Items purchased

Inventory capacity upgrades

Recipes purchased

Regional shops unlocked

Special shop items purchased

Achievements should not encourage wasting Gold.

Normal economic activity should provide achievement progress.

---

# Shop and Offline Progress

Shop restock timers may continue offline.

Offline progress should not automatically buy or sell items unless a future automation system explicitly allows it.

When the player returns:

Update shop stock.

Update restock timers.

Show important stock changes if needed.

Shops should not perform economic actions without player permission.

---

# Buyback System

Future option:

Recently sold items may appear in a Buyback section.

Buyback may help recover accidentally sold items.

Buyback rules may include:

Limited number of recent items

Higher repurchase price

Temporary availability

No buyback for consumable quantities after certain conditions

Locked items should not need buyback because they cannot be sold.

A Buyback system is useful but not required for v1.

---

# Bulk Selling

The player may sell multiple items at once.

Bulk selling should support:

Selected item quantities

Sell All Unlocked

Sell by category

Sell by filter

Future Auto-Sell rules

Bulk selling should never include:

Locked items

Equipped items

Reserved items

Protected progression items

Favorites without confirmation

The total Gold received should be shown before confirming.

---

# Auto-Sell

Auto-Sell is a future quality-of-life system.

Auto-Sell may allow the player to automatically sell selected items.

Auto-Sell rules may use:

Specific Item IDs

Item categories

Item types

Minimum owned quantity

Maximum kept quantity

Acquisition source

Auto-Sell should never apply automatically to:

New discoveries

Locked items

Unique items

Relics

Boss equipment

Dungeon equipment

Progression items

Companion unlock items

Important rank materials

Auto-Sell should be optional and clearly configured.

---

# Keep Quantity Rules

Future selling options may allow the player to keep a minimum quantity.

Example:

Sell all Oak Logs above 1,000.

Keep Quantity:

1,000

Sell Quantity:

Current Quantity - Keep Quantity

Keep Quantity rules are useful for idle resource management.

They should not be required for the base game.

---

# Price Information UI

Every shop item should display:

Item icon

Item name

Description

Current owned quantity

Buy price

Sell value

Available stock

Purchase limit

Requirements

Source

Primary use

Collection status

The player should understand whether the purchase is useful.

---

# Shop User Interface

The Shop UI should display:

Shop name

Shop icon

Shop category

Current currency

Shop stock

Item prices

Owned quantities

Stock amounts

Restock timers

Unlock requirements

Search

Filters

Sorting

Buy controls

Sell controls

Buyback if available

Inventory space

The Shop UI should be clear and fast to use.

---

# Shop Filters

Possible filters:

All Items

Resources

Equipment

Weapons

Armor

Tools

Accessories

Consumables

Food

Potions

Elixirs

Runes

Recipes

Companion Items

Available Purchases

Locked Purchases

Affordable Items

New Collection Items

Filters should help players find useful items quickly.

---

# Shop Sorting

Possible sorting options:

Name

Price

Category

Type

Profession

Combat Discipline

Required Level

Owned Quantity

Available Stock

Affordable First

Unlocked First

Sorting should make shop navigation easier.

---

# Shop Search

Shop search should match:

Item name

Item category

Item type

Profession

Combat Discipline

Primary use

Recipe

Shop category

Search should respect hidden shop stock.

---

# Purchase Preview

Before confirming a purchase, the UI may show:

Item

Quantity

Price per item

Total price

Currency remaining

Inventory slots required

Current owned quantity

Collection discovery status

Requirements

This is especially useful for expensive or one-time purchases.

---

# Sale Preview

Before confirming a sale, the UI may show:

Item

Quantity being sold

Sell value per item

Total Gold received

Quantity remaining

Whether item is used in recipes

Whether item is used for upgrades

Whether item is used for companion rank-ups

Whether item is part of a favorite or locked group

Important sales should be clearly explained.

---

# Economy Statistics

The game should track:

Current Gold

Total Gold Earned

Total Gold Spent

Total Gold Earned from Selling

Total Gold Earned from Combat

Total Gold Earned from Professions

Total Gold Earned Offline

Total Items Purchased

Total Items Sold

Total Shop Transactions

Most Expensive Purchase

Most Valuable Sale

Inventory Capacity Upgrades Purchased

Recipes Purchased

Gold Spent on Equipment

Gold Spent on Crafting

Gold Spent on Companions

Gold Spent on Regions

---

# Price Balance

Prices should reflect:

Progression stage

Item usefulness

Acquisition difficulty

Crafting cost

Drop rate

Profession level

Combat difficulty

Long-term demand

Prices should not rely only on item tier.

An old low-tier resource may remain valuable if used in many recipes or companion rank-ups.

---

# Resource Value Preservation

Older resources should remain economically useful where possible.

Possible uses for older resources:

Higher-tier crafting recipes

Equipment upgrades

Companion rank-ups

Shop exchanges

Runecrafting

Consumable production

Region unlocks

Collection or Museum systems

Future buildings

Avoid making early resources completely worthless.

---

# Economic Exploit Prevention

The economy should prevent simple infinite Gold loops.

Examples to avoid:

Buying an item for less than its sell value.

Crafting an item from shop materials and selling it for guaranteed infinite profit without meaningful progression limits.

Discount stacking that allows profitable resale.

Duplicate reward claims.

Repeated one-time purchases.

The game should validate prices and transactions.

Do not over-focus on complex anti-cheat early.

---

# Transaction Safety

Every transaction should happen safely.

Purchase flow:

Validate requirements.

Validate currency.

Validate inventory space.

Remove currency.

Add item.

Update systems.

Save.

Sale flow:

Validate item.

Validate quantity.

Remove item.

Add currency.

Update systems.

Save.

If a transaction fails, it should not partially remove items or currency.

---

# Shop Database

Every shop should exist inside the Shop Database.

Shops should never be hardcoded.

Shop data should connect to:

Item Database

Inventory System

Equipment System

Profession System

Crafting System

Combat System

Companion System

Achievement System

Collection System

Region System

Save System

Offline Progress System

Systems should reference Shop IDs and Shop Entry IDs.

---

# Economy Database

Economic balancing data may define:

Base item values

Buy price modifiers

Sell price modifiers

Currency definitions

Inventory upgrade prices

Region unlock costs

Crafting Gold costs

Equipment upgrade costs

Companion Gold costs

Restock rules

Economic data should be editable without changing code.

---

# Save Data

The Shop and Economy System should save:

Current currencies

Purchased one-time upgrades

Current limited stock

Restock timestamps

Purchase limits

Unlocked shops

Unlocked shop entries

Buyback items if used

Auto-Sell settings if used

Keep Quantity settings if used

Static shop stock definitions should remain in shop data.

---

# Technical Rules

The Shop and Economy System should be data-driven.

Items should be referenced by Item ID.

Shops should be referenced by Shop ID.

Currencies should be referenced by Currency ID.

Prices should be stored in shop or balancing data.

Shop scripts should not hardcode item names, prices, or requirements.

Transactions should be atomic and safe.

Shop purchases should respect inventory capacity.

Selling should respect locking and reservation rules.

Purchases should update Collections, Achievements, Statistics, and Save data.

Restock timers should work online and offline.

Adding a new shop should not require rewriting shop code.

---

# Balance Philosophy

The economy should create meaningful decisions.

The player should frequently consider:

Should I sell this item or save it?

Should I buy a tool or craft one?

Should I spend Gold on inventory capacity?

Should I rank up a companion?

Should I upgrade equipment?

Should I unlock a region?

Should I buy materials or gather them?

Should I spend Gold now or save for a larger unlock?

Gold should be valuable without being constantly scarce.

Shops should provide convenience and progression options without replacing gameplay.

---

# Future Expansion

Possible additions:

Buyback

Auto-Sell

Keep Quantity rules

Player trading

Auction House

Regional price differences

Merchant reputation

Limited travelling merchants

Black market shop

Collection shop

Achievement Point shop

Dungeon Token shop

Boss Token shop

Seasonal shops

Event shops

Shop discounts

Dynamic economy

Crafting orders

Player market

Future economy systems should expand the base framework rather than replace it.

---

# Checklist

Every shop should answer:

✓ What type of shop is it?

✓ What currency does it use?

✓ How is it unlocked?

✓ What items does it sell?

✓ Does it buy items?

✓ Does it use limited stock?

✓ Does stock restock offline?

✓ Are any purchases one-time?

✓ Does it respect inventory capacity?

✓ Does it connect to Collections?

✓ Does it connect to Achievements?

✓ Does it save and load correctly?

✓ Can it expand later?

Every shop entry should answer:

✓ What Item ID does it reference?

✓ What is the Buy Price?

✓ What is the Sell Price?

✓ Is stock unlimited or limited?

✓ Does it have a purchase limit?

✓ What requirements does it have?

✓ Does it require inventory space?

✓ Can it be sold back?

✓ Does buying it discover the item?

✓ Can future systems modify its price?

Every currency should answer:

✓ What is its purpose?

✓ Where does it come from?

✓ What is it spent on?

✓ Is it necessary as a separate currency?

✓ Does it use inventory space?

✓ Does it have a maximum amount?

✓ Can it be converted?

✓ Does it save and load correctly?

Every transaction should answer:

✓ Are requirements validated?

✓ Is currency validated?

✓ Is inventory space validated?

✓ Are locked and reserved items protected?

✓ Does the transaction update statistics?

✓ Does it update Collection progress?

✓ Does it update Achievement progress?

✓ Does it save safely?

✓ Can it fail without losing items or currency?