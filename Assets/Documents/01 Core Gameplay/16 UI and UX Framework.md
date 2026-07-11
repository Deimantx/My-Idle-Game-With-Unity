# 16 UI and UX Framework

Version: 1.0  
Status: Draft

---

# Purpose

This document defines how the game's user interface behaves from the player's perspective.

The game is primarily controlled through:

Menus

Buttons

Tabs

Cards

Lists

Scrollable panels

Progress bars

Loadouts

Tooltips

Popups

Information screens

Because the player does not directly control the character, the User Interface is one of the game's most important systems.

The UI should make complex progression easy to understand without overwhelming the player.

This framework defines:

Navigation

Persistent interface elements

Main screen structure

Scrolling behavior

Screen selection flows

Information hierarchy

Interaction rules

Popups

Notifications

Tooltips

Progress displays

Responsive layouts

Accessibility

Future mobile support

A separate Unity UI Technical Architecture document should define exactly how Codex constructs these systems inside Unity.

A separate UI Visual Style Guide should define how the interface looks.

---

# UI Philosophy

The interface should help the player make decisions quickly.

The player should always understand:

What activity is currently active

What progress is being made

What rewards are being earned

What requirements are missing

What can be upgraded

What has been unlocked

What will happen after pressing a button

The interface should prioritize clarity over decoration.

Important information should be visible without requiring unnecessary clicks.

Detailed information may appear inside:

Tooltips

Expandable sections

Side panels

Detail panels

Popups

Tabs

The player should not need an external wiki for basic gameplay information.

---

# Design Goals

The User Interface should:

- Be easy to understand.
- Support short check-ins and long idle sessions.
- Display progression clearly.
- Keep important navigation permanently accessible.
- Allow browsing without stopping active gameplay.
- Support large amounts of content.
- Use reusable UI components.
- Be data-driven where possible.
- Support mouse and keyboard.
- Support future touch controls.
- Scale across different screen resolutions.
- Use scrolling instead of shrinking information excessively.
- Avoid unnecessary popups.
- Avoid excessive animation.
- Avoid hiding important requirements.
- Provide clear feedback for every action.
- Remain usable with hundreds or thousands of items, activities, recipes, and enemies.

The UI should feel like one connected game interface rather than a collection of unrelated menus.

---

# Target Platforms

Current primary platform:

PC

Current primary layout:

Landscape

Current reference resolution:

1920 × 1080

Future platforms may include:

Steam Deck

Android phones

Android tablets

iPhone

iPad

The PC interface should be designed first.

The UI structure should remain flexible enough to support future mobile layouts without rewriting gameplay systems.

---

# Unity UI Technology

Current UI technology:

Unity Canvas / uGUI

Text system:

TextMeshPro

The main gameplay interface should not use UI Toolkit.

All major interface elements should use reusable uGUI prefabs.

Examples:

Buttons

Navigation entries

Tabs

Panels

Activity cards

Item slots

Equipment slots

Progress bars

Stat rows

Tooltips

Requirement rows

Notifications

Popups

Modal windows

---

# Scene Philosophy

The main game interface should exist inside one primary gameplay scene.

Opening a different system should not require loading another Unity scene.

Examples:

Inventory

Combat

Professions

Equipment

Companions

Crafting

Shop

Achievements

Collection Log

Settings

Screen changes should happen by showing and hiding UI panels.

Gameplay systems should continue operating while the player browses other screens.

Examples:

Woodcutting continues while viewing Inventory.

Combat continues while viewing Equipment information.

Crafting continues while viewing Achievements.

Companion rank-up timers continue while viewing the Shop.

Farming timers continue regardless of the active screen.

---

# Main Canvas Structure

Recommended high-level hierarchy:

[UI] Main Canvas

[PERSISTENT] Left Navigation

[PERSISTENT] Top Bar

[PERSISTENT] Active Activity Bar

[SCREENS] Screen Container

[OVERLAYS] Dropdown Layer

[OVERLAYS] Tooltip Layer

[OVERLAYS] Notification Layer

[POPUPS] Popup Layer

[MODALS] Modal Layer

[LOADING] Loading Layer

Recommended display order:

Main Screens

Persistent Interface

Dropdowns

Tooltips

Notifications

Popups

Modals

Loading Screen

Modals and Loading Screens should appear above all normal interface elements.

---

# Screen Manager

A central Screen Manager should control main screen visibility.

Screen Manager responsibilities:

Register main screens

Open a screen

Close the current screen

Remember the previous screen

Return to the previous screen

Prevent multiple main screens from appearing at once

Preserve screen state when appropriate

Update navigation selection

Support direct navigation from other systems

Only one main content screen should normally be active at a time.

Persistent navigation, the Top Bar, the Active Activity Bar, notifications, and tooltips remain independent from the selected main screen.

---

# Main Screen Rule

Only one primary screen should normally be visible inside the Screen Container.

Examples:

Dashboard Screen

Combat Screen

Woodcutting Screen

Mining Screen

Inventory Screen

Equipment Screen

Companion Screen

Crafting Screen

Shop Screen

Achievements Screen

Collection Log Screen

Settings Screen

Popups and detail windows may appear above the active screen.

Opening a popup does not replace the current main screen.

---

# Screen State Preservation

Main screens should preserve useful state when the player leaves and returns.

Possible preserved state:

Selected tab

Selected activity

Selected item

Selected companion

Selected recipe

Selected enemy

Selected dungeon

Selected region

Search text

Active filters

Sort option

Scroll position

Expanded sections

Temporary screen state does not always need to be written to the save file.

Important UI preferences may be saved between sessions.

---

# Persistent Left Navigation

The main navigation should remain accessible while browsing the game.

Current PC design:

Persistent left-side navigation panel

The navigation panel should provide access to the game's major systems.

Possible main entries:

Dashboard

Combat

Inventory

Equipment

Companions

Shop

Achievements

Collection Log

Settings

Individual professions should also be accessible from the left navigation.

The currently selected screen should remain visually highlighted.

---

# Navigation Scrolling

The left navigation will contain many buttons and categories.

The Navigation content should therefore use a vertical scrolling panel.

The fixed parts of the Navigation may include:

Game logo or title

Collapse button

Optional search button

The scrollable Navigation content may include:

Main system buttons

Combat buttons

Profession categories

Individual profession buttons

Progression systems

Account systems

The player should be able to access every navigation entry without making the buttons too small.

---

# Profession Navigation

Professions should be directly accessible from the persistent navigation.

The player should not need to open a general Skills screen before selecting a profession.

Professions may be grouped inside expandable categories.

Example:

Gathering

Woodcutting

Mining

Fishing

Foraging

Farming

Hunting

Archaeology

Crafting

Cooking

Smithing

Carpentry

Herblore

Tailoring

Leatherworking

Jewelcrafting

Enchanting

Runecrafting

Utility

Thieving

Profession groups may be collapsed or expanded.

Their expanded state may be saved.

---

# Navigation Groups

Possible navigation groups:

Main

Combat

Gathering Professions

Crafting Professions

Utility Professions

Progression

Account

Example:

Main:

Dashboard

Inventory

Equipment

Combat:

Combat

Dungeons if separate later

Tower if separate later

Gathering Professions:

Woodcutting

Mining

Fishing

Foraging

Farming

Hunting

Archaeology

Crafting Professions:

Cooking

Smithing

Carpentry

Herblore

Tailoring

Leatherworking

Jewelcrafting

Enchanting

Runecrafting

Utility Professions:

Thieving

Progression:

Companions

Achievements

Collection Log

Account:

Shop

Settings

Frequently used screens should remain easy to access.

---

# Navigation Entry Structure

Every navigation entry should define:

Navigation ID

Display Name

Icon

Target Screen ID

Category

Sort Order

Unlock Requirement

Notification Badge

Selected State

Hidden State

Future Expansion Notes

Locked entries may:

Remain hidden until relevant

Appear locked with visible requirements

The correct behavior may differ between systems.

---

# Navigation Button States

Navigation buttons should support:

Normal

Hovered

Pressed

Selected

Disabled

Locked

Notification Available

The selected entry should remain clearly highlighted.

Possible notification badges:

New

Claim

Level Up

Completed

Inventory Full

Crafting Complete

Companion Ready

The meaning of each badge should remain consistent throughout the game.

---

# Collapsible Navigation

The left navigation may support expanded and collapsed states.

Expanded state may show:

Icon

Text label

Category headings

Notification badge

Collapsed state may show:

Icon

Tooltip on hover

Notification badge

The player should be able to choose the preferred navigation width.

Navigation width and collapsed state should save between sessions.

---

# Top Bar

The Top Bar displays important account-wide information.

The Top Bar should remain separate from the Active Activity Bar.

Possible Top Bar information:

Gold

Inventory usage

Notifications

Settings shortcut

Save status

Additional important account currency if necessary

The Top Bar should not display the current activity because that information belongs in the Active Activity Bar.

The Top Bar should not become overcrowded.

Less important currencies should remain inside their related screens.

---

# Active Activity Bar

A persistent Active Activity Bar should display what the player is currently doing.

Possible displayed information:

Activity icon

Profession or Combat name

Current target or recipe

Activity progress bar

Time remaining

Stop button

Open Activity button

When combat is active, it may additionally display:

Player Health

Player maximum Health

Devotion

Maximum Devotion

Enemy name

Quit Combat button

Examples:

Woodcutting — Sproutwood Tree

Smithing — Iron Bar

Combat — Grey Wolf

Health: 49 / 50

Devotion: 550 / 1,000

The player should be able to click the Active Activity Bar to open the related screen.

The Active Activity Bar should remain compact and should not cover important main content.

---

# Active Activity Bar Rules

The Active Activity Bar should update whenever the active activity changes.

Starting a different profession activity should update the display.

Entering combat should change the display into Combat mode.

When no activity is active, the bar may display:

No Active Activity

Choose an activity to begin.

The Active Activity Bar should not contain detailed system information that belongs inside the main screen.

---

# Multiple Timer Display

Independent timers may continue while another main activity is active.

Examples:

Companion rank-up

Farming crop growth

Crafting job

Relic cooldown

Future research

The UI may provide a compact Timer Drawer or Timer Summary.

Possible display:

1 Craft Completed

2 Crops Ready

Companion Rank-Up: 12m Remaining

The player should not need to visit every system constantly to know when something is ready.

---

# Dashboard Screen

The Dashboard is an optional account overview.

It may display:

Current active activity

Recent offline progress

Profession overview

Combat Discipline overview

Companion timers

Crafting status

Farming status

Inventory capacity

Recent achievements

Recent Collection discoveries

Suggested progression goals

Claimable rewards

Dashboard sections should deep-link to their related screens.

The Dashboard should summarize information without replacing dedicated system screens.

---

# Standard Main Screen Structure

Most main screens should use a consistent structure.

Recommended structure:

Fixed Screen Header

Scrollable Main Content

Optional Fixed Action Bar

The Screen Header may contain:

Screen icon

Screen name

Level and XP if applicable

Short description

Screen-specific currency

Help button

The Scrollable Main Content contains most information and selection controls.

The Fixed Action Bar may contain important actions that should remain accessible.

Examples:

Start Combat

Stop Combat

Craft

Buy

Sell

Claim Reward

---

# Main Screen Scrolling Philosophy

Most information-heavy screens should support vertical scrolling.

The game will contain too many activities, items, regions, enemies, recipes, and information panels to fit everything into one fixed screen.

The UI should not attempt to solve this by:

Making text extremely small

Making buttons extremely small

Removing important information

Overcrowding panels

Instead, screens should use controlled scrolling.

Most main screens should have one primary vertical scroll area.

---

# Main Screen Scrolling Architecture

Persistent UI elements should never be inside a main screen ScrollRect.

These include:

Left Navigation

Top Bar

Active Activity Bar

Notification Layer

Tooltip Layer

Popup Layer

Modal Layer

Recommended structure:

Fixed Screen Header

↓

Scrollable Main Content

↓

Optional Fixed Action Bar

The Screen Header should remain visible when useful.

Important actions may remain fixed when constant access is important.

---

# Single Scroll Owner Rule

Each main screen should normally have one primary vertical scroll owner.

Avoid:

Vertical ScrollRect inside another Vertical ScrollRect

Horizontal ScrollRect inside another Horizontal ScrollRect

Allowed when useful:

Horizontal selector row inside a vertical main ScrollRect

Scrollable Combat Log beside fixed Combat panels

Scrollable Inventory grid beside a fixed Item Details panel

Scrollable loot area inside the fixed Combat interface

When several panels contain long information, prefer:

One main ScrollRect

Expandable sections

Tabs

Detail panels

Popups

Side panels

This is usually better than adding many separate vertical scrollbars.

---

# Fixed and Scrollable Information

Information that should usually remain fixed:

Screen title

Current Health during combat

Current Devotion during combat

Important combat controls

Start and Stop buttons

Search and filters when constantly used

Confirmation buttons

Information that may scroll:

Activity lists

Recipes

Items

Achievements

Collection entries

Regions

Enemy selections

Dungeon information

Loot tables

Statistics

Long descriptions

Requirements

---

# Dynamic Content Height

Main screen content should be able to expand vertically.

When a new section appears:

The content height should update.

The ScrollRect should update.

Previous selections should remain stable.

The screen should not jump to the top unnecessarily.

The UI may automatically scroll enough to reveal newly opened content.

Example:

Selecting a Region reveals the Activity Type row.

Selecting a Dungeon reveals its information panel.

Automatic scrolling should be gentle and should not move the player unnecessarily.

---

# Scroll Position Preservation

Major screens may preserve their scroll position while the game remains open.

Examples:

Combat selection

Profession activity list

Inventory

Shop stock

Achievement list

Collection Log

Changing a filter or category may reset scroll position when the previous position is no longer meaningful.

Leaving and returning to a screen should not always return the player to the top.

---

# Scrollbar Rules

Long PC lists should normally provide a visible or clearly understandable scrollbar.

A scrollbar helps the player understand:

That more content exists

Current position

Approximate content length

Scrollbars may be visually subtle.

A scrollbar may be hidden when:

The content is short

Scrolling is already obvious

The panel is primarily designed for touch

Another position indicator exists

---

# Standard Screen Header

A standard Screen Header may display:

Screen icon

Title

Subtitle or description

Current level

Current XP

XP progress bar

Screen-specific currency

Statistics button

Help button

Do not place unrelated actions inside the Screen Header.

The header should help the player identify the current system immediately.

---

# Tabs

Tabs may separate closely related content.

Examples:

Inventory:

All Items

Equipment

Consumables

Resources

Companions:

All

Profession

Combat

Crafting:

Recipes

Active Craft

Favorites

Tabs should not be used when a simple filter is enough.

The active tab should remain clearly highlighted.

Tab selection may be preserved when returning to a screen.

---

# Standard Profession Screen

Every profession screen should use a shared layout where possible.

Recommended structure:

Fixed Profession Header

Scrollable Profession Content

Optional Fixed Start or Stop Bar

Main profession content may include:

Active Activity Panel

Activity List

Selected Activity Details

Tool Slot

Companion Slot

Food Buff Slot

Relic Slot

Statistics section

Requirements

Rewards

---

# Profession Header

The Profession Header should display:

Profession icon

Profession name

Current level

Current XP

XP required for next level

XP progress bar

XP per hour

Estimated time to next level

Next unlock

The player should immediately understand profession progression.

---

# Profession Activity List

The Activity List should display available activities.

Every Activity Card may display:

Activity icon

Activity name

Required level

Locked or unlocked state

Base action time

Current action time

XP per completion

Estimated XP per hour

Primary reward

Estimated rewards per hour

Required tool

Region

Start button

Activities may appear as:

Cards

Rows

Compact list entries

The list should support vertical scrolling when necessary.

---

# Locked Profession Activities

Locked activities should explain why they are locked.

Possible requirements:

Profession Level

Tool

Region

Item

Achievement

Dungeon Completion

Companion Rank

The UI should not show only:

Locked

It should show the actual requirement.

Examples:

Requires Woodcutting Level 20

Requires Iron Axe

Requires Evergreen Region

Multiple missing requirements should be shown clearly.

---

# Active Profession Panel

The Active Profession Panel should display:

Current activity

Current target

Progress bar

Target Health or Durability

Next reward threshold

Current tool

Assigned companion

Active bonuses

XP gained

Rewards gained

Respawn timer

Stop button

For target-based professions, reward thresholds should be visible.

Example:

75%

50%

25%

0%

Completed thresholds should be visually marked.

---

# Profession Target Display

Target-based professions may display:

Target icon or image

Target name

Maximum Health or Durability

Current Health or Durability

Profession damage

Action timer

Reward thresholds

Respawn status

The panel may feel similar to combat but should remain visually distinct.

Profession damage should not be confused with Combat Discipline damage.

---

# Crafting Screen

The Crafting Screen should display:

Crafting profession

Profession level

Recipe list

Recipe filters

Search

Selected recipe

Required materials

Owned quantities

Reserved quantities

Missing materials

Crafting time

XP reward

Output item

Output quantity

Craft button

Active crafting job

Crafting progress

The Recipe List and details may exist inside the main scrolling area.

Important Craft or Cancel controls may remain fixed when appropriate.

---

# Crafting Material Display

Required materials should show:

Item icon

Item name

Required quantity

Owned quantity

Reserved quantity

Missing quantity

Clicking a material should open item information or source navigation.

Possible status indicators:

Checkmark for sufficient materials

Warning icon for missing materials

Reservation icon for reserved materials

Color should not be the only indicator.

---

# Combat Screen

Combat is one of the most information-heavy systems.

The Combat Screen has two major states:

Combat Selection State

Active Combat State

Combat Selection State allows the player to choose:

Region

Activity Type

Location or Content

Enemy, Dungeon, Boss, or Tower Floor

Active Combat State displays:

Player

Enemy

Companion

Abilities

Consumables

Runes

Relic

Effects

Loot

Combat Log

Combat controls

---

# Combat Selection Screen

Before combat begins, the Combat Screen should use a hierarchical selection interface.

Default flow:

Select Region

↓

Select Activity Type

↓

Select Location or Content

↓

Select Target or View Content Details

↓

Start Combat or Enter Content

Each selection row should appear after the required previous selection is made.

The complete Combat Selection interface should use one primary vertical ScrollRect.

---

# Combat Selection Breadcrumb

A compact breadcrumb should display the player's current selection.

Area example:

Greenvale > Areas > Forest > Grey Wolf

Dungeon example:

Greenvale > Dungeons > Overgrown Crypt

Tower example:

Ancient Kingdom > Tower > Floor 15

The player may click an earlier breadcrumb step to return to that level.

Changing an earlier selection clears later selections.

---

# Combat Region Row

The first row displays available Regions.

Every Region button may display:

Region icon

Region name

Locked or unlocked state

Recommended Combat Discipline level

Completion information if applicable

New-content badge

The Region row may scroll horizontally when many Regions exist.

Selecting a different Region clears:

Activity Type

Location

Enemy

Dungeon

Boss

Tower Floor

Combat preparation details

---

# Combat Activity Type Row

After selecting a Region, the Activity Type row appears.

Possible activity types:

Areas

Elite Areas

Dungeons

Bosses

Tower

Future combat content

Every Activity Type button may display:

Activity icon

Activity name

Locked or unlocked state

Available content count

Completion information

Selecting a different Activity Type clears all later selections.

---

# Combat Area Selection

When the player selects Areas, show locations inside the selected Region.

Examples:

Forest

Graveyard

Cave

Ruins

Swamp

Area selection flow:

Select Region

↓

Select Areas

↓

Select Location

↓

Select Enemy

↓

View Enemy Details

↓

Start Combat

The Location row may use horizontally scrollable cards or a wrapped card layout.

---

# Combat Target Row

After selecting a normal combat location, display available enemies.

Every Enemy Card may display:

Enemy icon

Enemy name

Combat level or difficulty

Health

Primary damage type

Known resistances

New item indicator if applicable

Loot preview button

Locked or unlocked state

Selecting an enemy should open its combat preparation details.

---

# Enemy Details Panel

Enemy details may display:

Enemy icon or image

Enemy name

Description

Health

Attack damage

Attack speed

Combat Abilities

Known resistances

Known weaknesses

Possible loot

Drop chances

Recommended preparation

Start Combat button

A separate information button may open a more detailed explanation of enemy abilities.

---

# Dungeon Selection

When the player selects Dungeons, display available Dungeons instead of normal locations.

Selecting a Dungeon should display:

Dungeon name

Dungeon description

Entry requirements

Recommended Combat Discipline level

Number of encounters

Enemy information

Final boss

Special mechanics

Completion count

Best completion time

Reward chest

Possible chest rewards

Item drop chances

Start Dungeon button

The player should be able to click the Reward Chest to inspect all possible rewards and their chances.

Dungeons do not require a normal Enemy Target row unless individual encounter selection becomes part of the design.

---

# Boss Selection

When the player selects Bosses, display available standalone Boss encounters.

Each Boss Card may display:

Boss icon

Boss name

Region

Difficulty

Entry requirements

Recommended Combat Discipline level

Completion count

Loot preview

Locked or unlocked state

Selecting the Boss should open its detailed preparation panel.

---

# Tower Selection

When the player selects Tower, display:

Tower name

Unlocked floors

Highest floor completed

Selected floor

Floor enemies

Floor modifiers

Possible rewards

Entry requirements

Start Floor button

Tower floors may use a vertical list when many floors exist.

The Tower floor list may exist inside the main Combat Selection ScrollRect.

Avoid nesting another vertical ScrollRect unless necessary.

---

# Progressive Selection Rules

Only relevant rows should be displayed.

Changing an earlier selection should close and clear later rows.

Example:

Changing Region clears:

Selected Activity Type

Selected Location

Selected Target

Selected Dungeon

Selected Boss

Selected Tower Floor

Combat preparation details

The selected option in each row should remain highlighted.

The screen may automatically scroll enough to reveal the next newly opened row.

---

# Combat Selection State Preservation

The Combat Screen may remember:

Selected Region

Selected Activity Type

Selected Location

Selected Enemy

Selected Dungeon

Selected Boss

Selected Tower Floor

Scroll position

Returning to the Combat Screen should restore the previous valid selection where possible.

---

# Starting Combat

When combat begins:

Collapse or hide the large Combat Selection interface.

Display a compact breadcrumb.

Show the Active Combat interface.

Keep important player and enemy information visible.

Lock setup options that cannot be changed during combat.

The player may still inspect enemy, dungeon, or boss information.

When combat ends, the Combat Selection interface should restore the previous selection.

---

# Active Combat Layout

Recommended Active Combat sections:

Player Panel

Enemy Panel

Player and Enemy Stat Areas

Companion Panel

Combat Ability Bar

Consumable Loadout

Active Rune Summary

Relic Control

Buff and Debuff Area

Temporary Loot Panel

Combat Log

Combat Controls

Important Health, Devotion, Enemy Health, and combat controls should remain visible without scrolling.

Less important information may exist in scrollable side panels or expandable sections.

---

# Player Combat Panel

The Player Combat Panel should display:

Player Health

Maximum Health

Health bar

Devotion

Maximum Devotion

Devotion bar

Combat Discipline

Equipped weapon

Equipped shield if applicable

Equipped armor

Equipped jewelry

Relic

Attack timer as a progress bar

Auto-attack damage range

Defense

Active buffs

Active debuffs

Important combat values should remain visible throughout combat.

---

# Enemy Combat Panel

The Enemy Combat Panel should display:

Enemy icon or image

Enemy name

Enemy Health

Enemy maximum Health

Health bar

Attack timer as a progress bar

Combat Abilities

Ability cooldowns

Active buffs

Active debuffs

Known resistances

Loot preview button

Information button for skills and mechanics

Boss mechanics should have clear visual warnings.

---

# Combat Companion Panel

The Companion Panel should display:

Companion icon

Companion name

Companion rank

Attack timer as a progress bar

Attack Damage

Damage Type

Companion Ability

Ability cooldown

Damage dealt this fight

Companions do not currently have:

Health

Targetable state

Death state

The UI should not display companion Health unless that system is added later.

---

# Combat Ability Bar

Combat Ability buttons should display:

Ability icon

Ability name in tooltip

Cooldown overlay

Remaining cooldown time

Manual or Auto state

Resource requirement

Unavailable state

Weapon requirement

Ability states should include:

Ready

On Cooldown

Disabled

Auto-Enabled

Manual-Only

Blocked by Equipment

Blocked by Devotion

The player should be able to configure auto-use before combat.

Combat Ability loadout should be locked after combat begins.

---

# Combat Consumable Display

Current combat consumable loadout:

1 Food

1 Healing Potion

4 Elixirs

Each slot should display:

Item icon

Quantity remaining

Effect

Duration

Auto-use state if applicable

Current active state

Food duration

Elixir duration

Potion cooldown if used

Food, Healing Potions, and Elixirs should remain visually distinct.

---

# Active Rune Summary

Rune selection belongs inside the Runecrafting screen.

The Equipment screen should provide a shortcut to Rune selection.

The Combat Screen should display a compact summary of currently selected runes.

The summary should display:

Up to 5 active rune icons

Individual rune tooltips

Active rune set bonuses

Total rune effects

Rune quantities if consumable

Runecrafting shortcut

Runes cannot be changed while combat is active.

When combat is not active, the shortcut opens the Rune selection section of Runecrafting.

---

# Combat Controls

Combat controls may include:

Start Combat

Stop Combat

Quit Encounter

Auto Repeat

Auto Ability Toggle

Combat Speed Display if added later

Change Enemy

Combat setup should be locked during active combat.

The UI should explain which settings require combat to stop before they can be changed.

---

# Temporary Combat Loot

Combat loot first enters Temporary Combat Loot.

The interface should display:

Loot item icon

Item name

Quantity

Collection status

New discovery marker

Important item marker

Pick All button

Temporary storage capacity

Full warning

Clicking an item icon should collect that item when safe.

Important items should be visually prioritized.

The player should be warned before Temporary Loot can be lost.

---

# Combat Log

The Combat Log may use its own vertical scrolling panel beside the fixed Combat interface.

Possible events:

Player attack

Enemy attack

Companion attack

Critical hit

Ability used

Buff applied

Debuff applied

Potion used

Enemy defeated

Loot dropped

Rare item found

Player defeated

The Combat Log should support:

Color-coded event types

Icons

Auto-scroll

Pause auto-scroll

Filtering

Clear button

Text size setting

Repeated basic attacks may be grouped or simplified.

---

# Inventory Screen

Recommended Inventory structure:

Fixed Inventory Header

Fixed Search and Filter Controls

Scrollable Item Grid or Item List

Selected Item Details Panel

Item Actions

The Item Grid should be the primary scrolling area.

The Selected Item Details Panel may remain fixed on wider screens.

On smaller screens, Item Details may open as a separate overlay or panel.

---

# Inventory Item Slot

Every slot should display:

Item icon

Quantity

New indicator

Locked indicator

Favorite indicator

Equipped indicator if applicable

Reserved indicator if applicable

Detailed information belongs in the Item Details Panel or tooltip.

---

# Inventory Details Panel

Selected Item Details should display:

Item icon

Item name

Description

Category

Type

Current quantity

Lifetime obtained quantity

Primary use

Sources

Sell value

Requirements

Recipe uses

Equipment upgrade uses

Companion rank-up uses

Available actions

The details panel should explain why the item matters.

---

# Inventory Actions

Possible actions:

Equip

Unequip

Consume

Use

Upgrade

Combine

Craft With

Sell

Destroy

Lock

Unlock

Favorite

Unfavorite

View Sources

View Recipes

Only valid actions should appear.

Dangerous actions should require confirmation.

---

# Equipment Screen

The Equipment Screen should display:

Character equipment layout

Equipment slots

Current stats

Selected equipment details

Comparison panel

Scrollable available equipment list

Combat loadout shortcut

Profession tool shortcut

Runecrafting rune-selection shortcut

Equipment slots:

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

---

# Equipment Comparison

When selecting equipment, show:

Current item

Selected item

Stat increases

Stat decreases

New bonuses

Lost bonuses

Requirement problems

Two-handed restrictions

Comparison indicators may include:

Up arrow

Down arrow

New effect icon

Removed effect icon

Color should not be the only indicator.

---

# Two-Handed Equipment UI

When a two-handed weapon is equipped or selected:

The Shield slot becomes unavailable.

The Shield slot should explain why.

Example:

Unavailable while using a two-handed weapon.

The player should be warned that equipping the weapon will unequip the current Shield.

Unequipped equipment should return safely to Inventory.

---

# Companion Screen

The Companion Screen should display:

Companion categories

Scrollable Companion list

Unlocked and locked companions

Selected companion details

Rank

Rank bonuses

Next rank bonus

Rank-up requirements

Rank-up timer

Assignment controls

Combat stats if applicable

Profession support if applicable

---

# Companion List Entry

Every Companion entry should display:

Icon

Name

Category

Current rank

Assignment state

Rank-up-ready indicator

Locked or unlocked state

Combat or Profession icon

Completed rank-up timers should create a clear notification badge.

---

# Combat Companion Details

Combat Companion details should display:

Attack Damage

Attack Speed

Accuracy

Critical Chance

Critical Damage

Damage Type

Combat Discipline support

Companion Ability

Ability cooldown

Current bonuses

Damage dealt statistics

Companions should not display Health because they cannot currently be attacked or killed.

---

# Companion Rank-Up UI

The Rank-Up interface should display:

Current rank

Target rank

Required items

Owned quantities

Required Gold

Required time

New bonus

Major milestone reward

Start Rank-Up button

Rank-up timer

Claim Rank button

Required items should be clickable to view their sources.

Items should be consumed when the rank-up begins.

---

# Shop Screen

Recommended Shop structure:

Fixed Shop Header

Fixed currency display

Fixed Buy and Sell tabs

Search and filters

Scrollable Shop Stock or Sell List

Selected Item Details

Purchase or Sale Preview

Important transaction controls may remain fixed.

The player should understand the full transaction before confirming it.

---

# Achievement Screen

The Achievement Screen should display:

Achievement categories

Completion percentage

Achievement Points

Scrollable Achievement list

Progress bars

Rewards

Claim status

Search

Filters

Sorting

Tracked achievements

Recently completed achievements

Claimable rewards should be easy to locate.

---

# Collection Log Screen

The Collection Log focuses primarily on items.

Recommended structure:

Fixed Collection Header

Overall item completion

Category selection

Search and filters

Scrollable Collection Item Grid or List

Selected Collection Entry Details

The Collection Log should display:

Discovered state

Undiscovered state

Current owned quantity

Lifetime obtained quantity

First discovery source

Known item sources

Milestone rewards

Enemies, Bosses, and Dungeons may appear as item sources but are not main Collection entries.

---

# Collection Entry Display

A discovered Collection entry should display:

Item icon

Item name

Description

Category

Type

Current owned quantity

Lifetime obtained quantity

First discovery source

Known sources

Related profession

Related recipe

Drop chance if applicable

An undiscovered entry may display:

Silhouette

Question mark

Known name

Known source

Locked information

Hidden rules should come from Collection data.

---

# Settings Screen

The Settings Screen may contain tabs or expandable sections.

Possible categories:

Audio

Graphics

UI

Gameplay

Combat

Notifications

Accessibility

Save Options

Possible UI settings:

UI Scale

Tooltip Delay

Navigation Width

Combat Log Text Size

Number Format

Animation Amount

Notification Duration

Color-Blind Support

Show Exact Drop Chances

Confirmation Settings

Settings should apply immediately where possible.

---

# Popup Types

Possible popup types:

Information Popup

Confirmation Popup

Selection Popup

Reward Popup

Warning Popup

Error Popup

Popups should remain focused on one purpose.

---

# Modal Rules

A modal blocks interaction with the interface underneath it.

Use modals for:

Destructive confirmations

Important irreversible actions

Save reset

Large purchases

Selling important equipment

Quitting dangerous combat

Major reward selection

Do not use modals for routine notifications.

Most modals should support:

Close button

Cancel button

Escape key

Clicking outside may close simple non-dangerous modals.

Clicking outside should never confirm a destructive action.

---

# Confirmation Rules

Confirmation should be required for:

Selling important equipment

Selling favorite items

Destroying items

Resetting save data

Starting expensive upgrades

Canceling long timers when resources may be lost

Quitting important combat

Consuming rare progression materials

Routine actions should not require repeated confirmation.

---

# Tooltip System

Tooltips may display:

Name

Description

Current value

Effect

Requirement

Source

Cooldown

Duration

Calculation summary

Keyboard shortcut

Tooltips should support:

Items

Stats

Abilities

Buffs

Debuffs

Runes

Companions

Equipment bonuses

Profession modifiers

Requirements

---

# Tooltip Rules

Tooltips should:

Appear after a short hover delay

Remain inside screen boundaries

Avoid covering the selected object when possible

Support long descriptions

Support icons and formatted values

Disappear when the cursor leaves

Support press-and-hold on future mobile layouts

Important information should not exist only in a tooltip.

---

# Item Source Navigation

Items should support direct source navigation.

Examples:

Iron Ore opens Mining.

Wolf Fang opens the Grey Wolf Combat selection.

A potion recipe opens Herblore.

A Boss material opens the related Boss selection.

A Back action should return the player to the original screen and selection.

---

# Deep Linking

Systems should be able to open a specific screen and focus a specific entry.

Examples:

Open Inventory and select Iron Ore.

Open Smithing and select Iron Bar.

Open Combat and select Grey Wolf.

Open Collection Log and select Blackshard.

Open Companions and select Ember Wolf.

Deep links should use stable IDs.

Possible deep-link data:

Target Screen ID

Target Entry ID

Target Tab

Return Screen ID

---

# Back Navigation

PC Back controls may include:

Escape key

Back button

Recommended order:

Close open tooltip if necessary.

Close dropdown.

Close popup.

Close modal if allowed.

Return from detail view.

Return to previous main screen.

Back navigation should never unexpectedly close the game.

---

# Search Rules

Large lists should support search.

Possible screens:

Inventory

Crafting

Shop

Achievements

Collection Log

Companions

Profession activity lists

Combat targets if needed

Search should update quickly and match relevant fields.

Examples:

Item name

Item type

Profession

Source

Recipe

Combat Discipline

Search should respect hidden-content rules.

---

# Filter Rules

Filters should:

Use consistent controls

Clearly show active filters

Provide Clear All

Preserve selections where appropriate

Show result count

Avoid excessive nested filter menus

Common filters may use visible category buttons.

Advanced filters may use a Filter popup.

---

# Sorting Rules

Every sortable list should display the current sort method.

Possible sorts:

Name

Level

Quantity

Recently obtained

Recently unlocked

Category

Type

Price

Progress

Source

Locked or unlocked

Ascending or descending state should be visible.

---

# General Scroll Behavior

Scrollable panels should use directional behavior consistently.

PC users should be able to use:

Mouse wheel

Scrollbar

Click and drag when supported

Future touch users should be able to swipe.

Scroll position should remain stable when content updates.

Lists should not jump back to the top unnecessarily.

Only the intended direction should scroll.

---

# Drag and Drop

Drag and drop is optional.

Essential actions should not require it.

Primary interaction should use:

Click or tap

Select action

Confirm when necessary

Future drag-and-drop uses may include:

Equipping items

Changing rune slots

Reordering loadouts

Removing items from loadouts

Every drag-and-drop action should have a button-based alternative.

---

# Button Rules

Buttons should clearly communicate their action.

Good labels:

Start Woodcutting

Stop Activity

Craft Iron Sword

Equip

Upgrade

Claim Reward

Buy

Sell

Avoid vague labels:

Do It

Action

Continue

Buttons should support:

Normal

Hovered

Pressed

Disabled

Selected

Loading

Disabled buttons should explain why they are disabled.

---

# Disabled Action Feedback

A disabled action should display its missing requirement.

Possible methods:

Tooltip

Requirement list

Inline warning

Disabled reason text

Example:

Craft Iron Sword

Requires 5 Iron Bars.

Owned: 3

The player should not need to guess why an action is unavailable.

---

# Requirement Display

Requirements should use a consistent format.

Possible states:

Requirement Met

Requirement Missing

Requirement Locked

Requirement Partially Met

Every Requirement Row may display:

Icon

Requirement name

Required value

Current value

Status icon

Examples:

Woodcutting Level 20 — Current: 18

Iron Axe — Owned

Evergreen Region — Locked

Color should not be the only indicator.

---

# Progress Bars

Progress bars may represent:

XP

Health

Devotion

Profession actions

Player attacks

Enemy attacks

Companion attacks

Crafting

Companion rank-ups

Target durability

Respawn

Ability cooldowns

Buff durations

Relic cooldowns

Farming growth

Progress bars should clearly indicate what they represent.

---

# Progress Bar Information

A progress bar may display:

Current value

Maximum value

Percentage

Time remaining

Rate

Label

Examples:

Woodcutting XP: 7,250 / 10,000

Companion Rank-Up: 2h 14m Remaining

Tree Durability: 320 / 500

The player should not need to estimate progress only from the bar width.

---

# Timer Display

Timer formatting should remain consistent.

Examples:

Under one minute:

42s

Under one hour:

12m 34s

Over one hour:

2h 14m

Over one day:

1d 5h

Long timers:

3d 12h

---

# Number Formatting

Possible display modes:

Exact:

1,245,820

Abbreviated:

1.25M

The player may select:

Exact Numbers

Abbreviated Numbers

Abbreviated values should show exact values in tooltips.

Purchases, crafting requirements, and precise costs should remain exact.

---

# Stat Display

Stats should use consistent names and formats.

Examples:

Attack Damage

Attack Speed

Accuracy

Critical Chance

Critical Damage

Defense

Damage Reduction

Resource Gain

Action Speed

XP Gain

Flat and percentage bonuses should remain distinct.

Examples:

+10 Attack Damage

+10% Attack Damage

---

# Positive and Negative Changes

Stat comparisons should use:

Up arrow

Down arrow

Plus and minus symbols

Text labels where needed

Color should not be the only method of communication.

---

# Notification Types

Possible notifications:

Toast Notifications

Banner Notifications

Navigation Badges

Activity Messages

Reward Notifications

Warning Notifications

Achievement Notifications

Collection Discovery Notifications

Level-Up Notifications

---

# Notification Priority

Low:

Common item obtained

Routine activity completed

Medium:

Profession level gained

Crafting completed

Companion timer completed

High:

Rare item found

Achievement completed

New Collection item

Inventory full

Critical:

Save failure

Important reward cannot be stored

Corrupted data warning

Higher-priority notifications should remain visible longer.

---

# Notification Grouping

Repeated notifications should be grouped.

Instead of:

Oak Log +1

Oak Log +1

Oak Log +1

Display:

Oak Log +3

Multiple discoveries may display:

5 New Collection Items

Repeated combat drops may also be summarized.

---

# Notification History

A Notification History panel may store:

Recent unlocks

Rare drops

Level-ups

Completed crafting

Companion timers

Achievements

Collection discoveries

Warnings

The player should be able to review important messages that disappeared.

---

# New Content Badges

New-content badges may appear on:

Navigation entries

Tabs

Items

Recipes

Activities

Companions

Achievements

Collection entries

Badges should disappear after the related content is viewed.

Avoid permanent unnecessary notification dots.

---

# Empty States

Screens and lists should explain empty content.

Examples:

No items match your filters.

No crafting recipe selected.

No companion assigned.

No active activity.

No achievements completed yet.

No Temporary Combat Loot.

Avoid blank panels without explanations.

---

# Loading States

Normal screen navigation inside the main scene should be immediate.

When loading takes noticeable time, show:

Spinner

Progress bar

Loading text

Skeleton placeholders

Do not use unnecessary loading screens for ordinary panel switching.

---

# Error States

Errors should explain:

What failed

Whether progress is safe

What the player can do next

Example:

Purchase Failed

Your Gold was not spent.

Free one Inventory slot and try again.

Technical details should be logged for development rather than displayed to the player.

---

# Inventory Full UX

When Inventory becomes full:

Show a clear warning.

Stop or pause affected profession activities.

Prevent unsafe Shop purchases.

Keep protected rewards claimable.

Highlight Inventory navigation.

Show used and maximum slots.

Example:

Inventory Full

Woodcutting has stopped.

Free at least 1 Inventory slot to continue.

---

# Offline Progress Summary

The Offline Progress Summary should appear after loading when meaningful offline progress occurred.

It may display:

Time away

Active activity

XP gained

Items gained

Gold gained

Combat kills

Companion damage

Crafting completed

Farming completed

Companion timers completed

Level-ups

Rare drops

Collection discoveries

Achievements

Inventory overflow

Important information should appear first.

Detailed sections may be expandable or scrollable.

---

# Offline Summary Actions

Possible actions:

Claim All

View Items

View Combat Results

View Profession Results

View Collection Discoveries

Open Inventory

Continue Activity

The player should not be forced through many separate reward popups.

---

# First-Time Tutorials

Tutorials should be contextual.

Examples:

First time opening Combat

First time equipping an item

First time starting a profession

First time assigning a companion

First time selecting runes

First time Inventory becomes full

Tutorials should:

Be short

Highlight relevant UI

Allow skipping

Avoid repeated blocking

Remain available through Help

---

# Help System

Screens may include a Help button.

Help may explain:

System purpose

Core loop

Important terminology

Requirements

Common questions

Detailed formulas may appear in advanced tooltips or future guides.

---

# PC Input

Primary PC input:

Mouse

Keyboard

Mouse interactions:

Left Click:

Select or activate

Right Click:

Optional quick-action menu

Mouse Wheel:

Scroll

Hover:

Show tooltip

Keyboard interactions may include:

Escape:

Back or close

Enter:

Confirm

Tab:

Move between controls

Arrow Keys:

Navigate supported lists

Keyboard shortcuts may be added later.

---

# Future Touch Input

Future mobile input should support:

Tap

Swipe

Press and hold

Optional drag

Touch targets should remain large enough to press reliably.

Hover-only information must have a touch alternative.

Possible behavior:

Tap item:

Select item.

Tap selected item again:

Open details.

Press and hold:

Show tooltip.

Swipe:

Scroll.

Mobile and PC layouts may differ while sharing gameplay logic.

---

# Responsive Layout

Recommended Canvas Scaler setup:

Scale With Screen Size

Reference Resolution:

1920 × 1080

Screen Match Mode:

Match Width Or Height

Starting Match value:

Approximately 0.5

Final scaling should be tested on multiple resolutions.

---

# Supported PC Resolutions

The UI should be tested at minimum on:

1920 × 1080

2560 × 1440

Ultrawide displays should also be considered.

Main content should not stretch excessively on very wide screens.

Maximum content widths may be used.

---

# Small-Screen Behavior

When horizontal space becomes limited:

Left navigation may collapse.

Secondary panels may become tabs.

Detail panels may open as overlays.

Multi-column layouts may become single-column.

Text should remain readable.

Buttons should not overlap.

Important information should not be removed only to fit the screen.

---

# Future Mobile Layout

Future mobile layouts may use:

Navigation drawer

Bottom navigation for major screens

Separate Profession selection screen

Full-screen detail panels

Larger touch targets

Reduced simultaneous information

The same gameplay data and systems should be reused.

---

# Safe Area Support

Future mobile versions should respect:

Camera notches

Rounded corners

System navigation areas

Important buttons and text should remain inside the safe area.

---

# UI Scale Setting

Possible UI Scale options:

Small

Default

Large

Very Large

Or a percentage slider.

UI scaling should not break layouts.

Text, icons, buttons, and panels should scale consistently.

---

# Accessibility

Possible accessibility settings:

UI Scale

Text Size

High Contrast Mode

Color-Blind Friendly Indicators

Reduced Motion

Combat Log Text Size

Longer Notification Duration

Disable Screen Shake

Disable Flashing Effects

Exact Number Display

Tooltip Delay

Important information should not rely only on:

Color

Sound

Animation

Icons without text

---

# Reduced Motion

Reduced Motion may reduce or disable:

Panel sliding

Button bounce

Screen shake

Flashing highlights

Repeated item animations

Large number animations

State changes should remain understandable without motion.

---

# Color Accessibility

Color should support information rather than replace it.

Examples:

Positive stat:

Green plus up arrow.

Negative stat:

Red plus down arrow.

Locked requirement:

Dimmed state plus lock icon and requirement text.

Ready ability:

Bright state plus Ready indicator.

---

# Audio Feedback

Possible UI sounds:

Button click

Purchase complete

Sale complete

Crafting started

Crafting completed

Level-up

Achievement completed

Rare item found

Error

Inventory full

UI sounds should remain subtle.

The player should be able to adjust or disable UI sound volume.

---

# Animation Rules

Good animation uses:

Panel opening

Progress completion

Item reward arrival

Level-up

Rare item reveal

Button feedback

Avoid:

Long delays before actions

Excessive bouncing

Constant flashing

Animations that block interaction

Routine actions should remain fast.

---

# UI Performance Expectations

The interface should remain responsive with large amounts of data.

Performance expectations:

Do not rebuild entire lists when one value changes.

Update only affected components.

Reuse repeated UI entries.

Avoid unnecessary UI refreshes every frame.

Large lists may later use pooling or virtualization.

Scrolling should remain smooth.

Opening screens should feel immediate.

---

# UI and Gameplay Separation

UI scripts should:

Read gameplay data.

Display gameplay data.

Send player commands.

UI scripts should not:

Contain combat formulas.

Generate profession rewards.

Directly modify XP.

Create items outside the Inventory System.

Calculate offline progress.

Example:

The Crafting UI requests:

StartCraft(recipeID)

The Crafting System validates and performs the action.

The UI displays the result.

---

# Reusable UI Components

Reusable components should be created for:

Primary Button

Secondary Button

Danger Button

Icon Button

Navigation Button

Tab Button

Item Slot

Equipment Slot

Companion Card

Activity Card

Recipe Card

Enemy Card

Dungeon Card

Region Button

Requirement Row

Stat Row

Progress Bar

Timer Display

Buff Icon

Ability Button

Rune Slot

Notification Toast

Confirmation Popup

Tooltip

Search Bar

Filter Button

Dropdown

Empty State

Loading Indicator

---

# Naming Conventions

Recommended hierarchy prefixes:

[UI]

[SCREEN]

[PERSISTENT]

[PANEL]

[POPUP]

[MODAL]

[LIST]

[CARD]

[BUTTON]

[TEXT]

[ICON]

[SLOT]

[SCROLL]

Examples:

[UI] MainCanvas

[PERSISTENT] LeftNavigation

[PERSISTENT] ActiveActivityBar

[SCREEN] CombatScreen

[PANEL] EnemyDetails

[SCROLL] CombatSelectionScroll

[LIST] ActivityList

[CARD] ProfessionActivityCard

[BUTTON] StartCombat

[SLOT] RuneSlot01

---

# Recommended UI Hierarchy

Example:

[UI] MainCanvas

├── [PERSISTENT] TopBar

├── [PERSISTENT] LeftNavigation

├── [PERSISTENT] ActiveActivityBar

├── [SCREENS] ScreenContainer

│   ├── [SCREEN] Dashboard

│   ├── [SCREEN] Combat

│   ├── [SCREEN] Inventory

│   ├── [SCREEN] Equipment

│   ├── [SCREEN] Companions

│   ├── [SCREEN] Shop

│   ├── [SCREEN] Achievements

│   ├── [SCREEN] CollectionLog

│   ├── [SCREEN] Settings

│   └── [SCREENS] ProfessionScreens

│       ├── [SCREEN] Woodcutting

│       ├── [SCREEN] Mining

│       ├── [SCREEN] Fishing

│       ├── [SCREEN] Cooking

│       └── Additional Professions

├── [OVERLAYS] DropdownLayer

├── [OVERLAYS] TooltipLayer

├── [OVERLAYS] NotificationLayer

├── [POPUPS] PopupLayer

├── [MODALS] ModalLayer

└── [LOADING] LoadingLayer

---

# Dynamic List Rules

Lists should generate entries from database data.

Examples:

Profession activities from the Activity Database

Inventory entries from Inventory data

Recipes from the Recipe Database

Companions from the Companion Database

Achievements from the Achievement Database

Collection entries from Item and Collection data

Shop stock from the Shop Database

Combat Regions from the Region Database

Combat Activity Types from Combat content data

Enemies from the Enemy Database

Dungeons from the Dungeon Database

Do not manually create every list entry inside the Unity scene.

---

# Screen Unlock Rules

A screen may be:

Available from the start

Visible but locked

Hidden until unlocked

When a new screen unlocks:

Show a notification.

Update Navigation.

Highlight the new entry.

Provide a short explanation when needed.

The interface should not overwhelm the player with every system immediately.

---

# UI Save Data

The UI may save:

Last active screen

Navigation collapsed state

Navigation width

Expanded Navigation categories

Selected tabs

Sort settings

Filter settings

Tracked achievements

Tracked Collection items

Selected Combat Region

Selected Combat Activity Type

Selected Combat Location

Selected enemy or Dungeon

Scroll positions where useful

UI Scale

Tooltip delay

Notification settings

Combat Log settings

Temporary hover states do not need to be saved.

---

# UI Debug Tools

Development UI tools may include:

Open any screen

Unlock every screen

Refresh current screen

Show test notification

Show test popup

Fill Inventory

Clear Inventory

Simulate locked activity

Simulate level-up

Simulate Collection discovery

Simulate Achievement completion

Simulate Combat selection

Change resolution

Change UI scale

Display safe area

Show UI object names

Debug tools should be disabled in release builds.

---

# Technical Rules

The User Interface should use Unity Canvas / uGUI.

Text should use TextMeshPro.

The main Navigation should remain persistent.

The Top Bar should remain separate from the Active Activity Bar.

Only one main screen should normally be active.

Active gameplay should continue while browsing.

Most information-heavy screens should support vertical scrolling.

Each main screen should normally use one primary vertical ScrollRect.

Avoid nested scrolling in the same direction.

Horizontal selector rows may exist inside a vertical ScrollRect.

Important active Combat information should remain visible without scrolling.

Screens should use reusable prefabs.

UI should remain separate from gameplay logic.

Lists should be generated from databases.

Deep links should use stable IDs.

Tooltips should stay inside screen boundaries.

Popups should use a shared popup system.

Notifications should use a shared notification system.

UI settings should save and load correctly.

Adding a new screen should not require rewriting the Screen Manager.

Adding new Regions, enemies, items, recipes, activities, or companions should not require rewriting shared UI systems.

---

# UX Philosophy

The interface should help the player compare:

Activities

Rewards

Equipment

Recipes

Companions

Combat targets

Dungeons

Shop purchases

Rune loadouts

The interface should provide:

Clear basic information

Optional advanced detail

Fast navigation

Reliable feedback

Minimal unnecessary friction

Scrolling should allow the interface to contain enough information without making text and buttons too small.

---

# Future Expansion

Possible additions:

Mobile-specific layout

Controller support

Steam Deck layout

UI themes

Customizable Dashboard

Movable panels

Resizable windows

Saved screen layouts

Multiple equipment loadouts

Profession presets

Combat presets

Rune loadout presets

Advanced item comparison

Detailed damage meters

UI mod support

Additional accessibility settings

Future UI systems should expand this framework rather than replace it.

---

# Checklist

Every main screen should answer:

✓ What system does the screen represent?

✓ What is the primary player action?

✓ Which information must remain fixed?

✓ Which content should scroll?

✓ Does it have one clear primary ScrollRect?

✓ Does it avoid nested scrolling in the same direction?

✓ Does it preserve useful state?

✓ Can it be opened through Navigation?

✓ Can other systems deep-link to it?

✓ Does it support Search, Filtering, or Sorting when needed?

✓ Does it handle locked content?

✓ Does it handle empty states?

✓ Does it handle errors?

✓ Does it scale to smaller resolutions?

✓ Can future systems expand it?

Every UI action should answer:

✓ What happens when the player presses it?

✓ Are requirements shown?

✓ Is confirmation necessary?

✓ Is feedback immediate?

✓ Can the action fail safely?

✓ Is the disabled reason visible?

✓ Does the action work with mouse?

✓ Can it support touch later?

Every scrolling screen should answer:

✓ What is the primary scroll owner?

✓ Which content remains fixed?

✓ Which content expands vertically?

✓ Does newly opened content remain visible?

✓ Is scroll position preserved?

✓ Does the screen avoid unnecessary nested ScrollRects?

✓ Is the scrollbar visible when useful?

Every Combat selection level should answer:

✓ What previous selection unlocks it?

✓ What data creates its buttons or cards?

✓ What happens when its selection changes?

✓ Which later selections are cleared?

✓ Does the selected option remain highlighted?

✓ Can the player return through the breadcrumb?

✓ Does the screen remember the last valid selection?

Every reusable component should answer:

✓ Can it display data from multiple systems?

✓ Does it support normal, selected, disabled, and locked states?

✓ Does it use consistent text and icons?

✓ Does it support tooltips?

✓ Does it scale correctly?

✓ Can it be reused without hardcoded gameplay logic?

Every popup should answer:

✓ Why is the popup required?

✓ Does it block the interface?

✓ Can it be closed safely?

✓ Is confirmation necessary?

✓ What happens when Escape is pressed?

✓ Does it clearly explain the result?

Every responsive layout should answer:

✓ Does text remain readable?

✓ Do buttons remain usable?

✓ Does Navigation remain accessible?

✓ Do panels overlap?

✓ Can lists scroll correctly?

✓ Does the layout support PC landscape?

✓ Can it adapt to future mobile screens?