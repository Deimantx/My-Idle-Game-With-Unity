# 18 UI Visual Style Guide

Version: 1.0  
Status: Draft  
Primary Platform: PC Landscape  
Reference Resolution: 1920 × 1080  
UI System: Unity Canvas / uGUI  
Text System: TextMeshPro

---

# Purpose

This document defines how the game's User Interface should look.

The UI and UX Framework defines how the interface behaves.

The Unity UI Technical Architecture defines how the interface is constructed inside Unity.

This Visual Style Guide defines:

Visual identity

Color palette

Typography

Panel appearance

Button appearance

Navigation appearance

Progress bars

Cards

Item slots

Combat presentation

Profession presentation

Tooltips

Popups

Notifications

Spacing

Icons

Animations

Responsive visual rules

Placeholder asset rules

Visual consistency rules for Codex

The purpose of this document is to prevent every screen from looking like it belongs to a different game.

---

# Visual Direction

The game should use a modern dark-fantasy RPG interface.

The style should combine:

Dark fantasy atmosphere

Modern interface readability

MMORPG-style information density

Clean idle-game navigation

Subtle metallic and carved-material details

Strong visual hierarchy

The interface should feel polished and immersive without becoming difficult to read.

The UI should not look like:

A generic mobile application

A bright cartoon interface

A futuristic science-fiction dashboard

A plain Unity prototype

A direct copy of Melvor Idle

A collection of unrelated asset packs

The game may take inspiration from other idle RPG and MMORPG interfaces, but its shapes, colors, icons, borders, and layout identity should remain original.

---

# Visual Style Pillars

The visual identity should follow five main pillars.

## Dark Fantasy

The interface should use dark backgrounds, subdued materials, bronze or gold accents, and fantasy-inspired icons.

## Modern Clarity

Text, values, buttons, and progress should remain easy to understand.

Decoration should never make information difficult to read.

## Controlled Detail

Panels may use subtle textures, borders, highlights, and shadows.

Avoid excessive ornamentation around every element.

## Strong Hierarchy

The player should immediately understand:

What screen is open

What is selected

What is active

What is locked

What can be clicked

What information is most important

## Visual Consistency

Buttons, tabs, cards, item slots, panels, progress bars, and notifications should use shared styles across the entire game.

---

# Overall Interface Mood

The interface should feel:

Mysterious

Ancient

Strategic

Powerful

Organized

Readable

Long-term progression should feel important.

The UI should support the feeling that the player is managing an expanding fantasy account rather than controlling one character moment by moment.

---

# Main Color Philosophy

The interface should primarily use:

Dark blue-black backgrounds

Dark slate panels

Muted steel borders

Warm bronze or gold accents

Off-white text

Controlled system-specific accent colors

Recommended visual distribution:

70% dark neutral backgrounds

20% raised panels and borders

10% accents, highlights, and status colors

Bright colors should be reserved for important information.

---

# Base Color Palette

The following colors are starting recommendations.

Exact colors may be adjusted after reference images and UI testing.

## Deep Background

Purpose:

Main screen background

Behind large panels

Unused screen space

Suggested color:

`#0D1117`

---

## Main Background

Purpose:

Main UI shell

Screen backgrounds

Navigation background

Suggested color:

`#121923`

---

## Standard Panel

Purpose:

Normal cards

Information panels

Lists

Suggested color:

`#1A2430`

---

## Raised Panel

Purpose:

Selected item details

Important information

Popups

Active panels

Suggested color:

`#233140`

---

## Hovered Panel

Purpose:

Hovered cards

Interactive rows

Suggested color:

`#2A3949`

---

## Primary Border

Purpose:

Normal panel borders

Card outlines

Dividers

Suggested color:

`#344455`

---

## Strong Border

Purpose:

Selected panels

Important frames

Active content

Suggested color:

`#53677C`

---

## Primary Accent

Purpose:

Important selected states

Major buttons

High-value highlights

Suggested color:

`#D0A44B`

This should appear as muted gold or bronze rather than bright yellow.

---

## Secondary Accent

Purpose:

Interactive highlights

Links

Information icons

Selected secondary controls

Suggested color:

`#3C9CB5`

This should appear as muted teal or blue.

---

# Text Colors

## Primary Text

Purpose:

Main labels

Important values

Titles

Suggested color:

`#EDF2F7`

---

## Secondary Text

Purpose:

Descriptions

Secondary values

Subtitles

Suggested color:

`#B2BEC9`

---

## Muted Text

Purpose:

Inactive labels

Minor descriptions

Unimportant statistics

Suggested color:

`#788696`

---

## Disabled Text

Purpose:

Disabled buttons

Unavailable actions

Suggested color:

`#53606D`

---

## Highlighted Text

Purpose:

Important rewards

Selected titles

Major progression information

Suggested color:

`#E2BE68`

---

# Status Colors

Status colors should remain consistent throughout the game.

## Positive

Purpose:

Requirements met

Stat increases

Successful actions

Suggested color:

`#4FB879`

Use with:

Checkmark

Up arrow

Success label

---

## Negative

Purpose:

Missing requirements

Damage

Stat decreases

Errors

Suggested color:

`#D45B63`

Use with:

Cross icon

Down arrow

Error label

---

## Warning

Purpose:

Inventory nearly full

Important confirmation

Limited resources

Suggested color:

`#D89A3C`

Use with:

Warning triangle

Warning text

---

## Information

Purpose:

Help

Tooltips

Neutral system messages

Suggested color:

`#4D8FD1`

---

## Locked

Purpose:

Locked content

Unavailable systems

Suggested color:

`#68727D`

Locked content should also use a lock icon and explanatory text.

Color alone should not represent the locked state.

---

# Progress and Resource Colors

Different progression systems should use consistent colors.

## Health

Suggested color:

`#B9414E`

Health bars should use deep red rather than bright neon red.

Low Health may become brighter or pulse gently.

---

## Devotion

Suggested color:

`#7762C9`

Devotion should use violet or indigo.

It should remain clearly different from Health and XP.

---

## Profession XP

Suggested color:

`#D0A94F`

Profession XP bars should use muted gold.

---

## Combat Discipline XP

Suggested color:

`#4C86C6`

Combat Discipline XP may use blue.

---

## Target Durability

Suggested color:

`#B96E3D`

Target durability should use warm orange or brown.

It should remain visually distinct from enemy Health.

---

## Crafting Progress

Suggested color:

`#3B9B92`

Crafting progress may use teal.

---

## Companion Rank Progress

Suggested color:

`#A26BC4`

Companion rank progression may use purple.

---

## Farming Growth

Suggested color:

`#62A55C`

Farming growth should use green.

---

## Cooldowns

Suggested color:

`#657384`

Cooldown overlays should use dark desaturated blue or grey.

---

# Combat Discipline Colors

Combat Discipline colors should be used as accents, not as full-screen backgrounds.

## Warrior

Suggested accent:

Deep red or bronze-red

Suggested color:

`#B0524E`

Possible visual elements:

Sword icon

Heavy border accents

Angular shapes

---

## Ranger

Suggested accent:

Forest green

Suggested color:

`#4E9B61`

Possible visual elements:

Arrow icon

Light leather textures

Sharp but thin borders

---

## Mage

Suggested accent:

Arcane blue or violet

Suggested color:

`#626BD1`

Possible visual elements:

Rune icon

Soft glow

Arcane line details

Combat Discipline colors should not replace standard status colors.

Health remains red regardless of Combat Discipline.

---

# Profession Colors

Each Profession may use a small identifying accent.

These colors may appear in:

Profession icons

Screen Header accent

Selected navigation line

XP bar details

Activity card highlights

They should not recolor the entire screen.

Suggested starting accents:

Woodcutting:

Warm green-brown

Mining:

Steel grey-blue

Fishing:

Deep ocean blue

Cooking:

Warm orange

Foraging:

Herbal green

Smithing:

Iron grey with ember orange

Herblore:

Emerald green

Farming:

Field green

Carpentry:

Golden brown

Thieving:

Muted violet

Hunting:

Dark forest green

Archaeology:

Sandstone gold

Tailoring:

Soft indigo

Leatherworking:

Dark brown

Jewelcrafting:

Cyan or gem blue

Enchanting:

Arcane purple

Runecrafting:

Blue-violet

Exact Profession colors should be finalized after Profession icons are created.

---

# Item Rarity Rules

The game currently does not use an item rarity system.

Codex must not automatically create:

Common rarity

Uncommon rarity

Rare rarity

Epic rarity

Legendary rarity

Rarity-colored borders

Rarity-colored item names

Rarity-colored backgrounds

Item slot frames should use category, selection, equipment, lock, favorite, or discovery states instead.

A rarity system may be added later through a separate design decision.

---

# Background Style

Screen backgrounds should remain dark and quiet.

Possible background elements:

Subtle stone texture

Dark brushed metal

Faded parchment shapes

Soft fantasy patterns

Very subtle environmental silhouettes

Backgrounds should not compete with text or panels.

Avoid:

Bright illustrations directly behind information

Strong repeating textures

High-contrast noise

Constant animated backgrounds

The main screen background may use a subtle gradient.

Suggested direction:

Darker at the edges

Slightly lighter behind main content

---

# Material Style

The UI may use a combination of:

Dark metal

Carved stone

Aged wood

Leather

Bronze

Dark glass

Materials should be used as subtle inspiration rather than photorealistic textures on every panel.

Recommended material usage:

Navigation:

Dark metal or wood

Standard panels:

Dark slate

Selected panels:

Raised dark metal

Buttons:

Metal, stone, or leather

Important borders:

Bronze

Tooltips:

Dark leather or dark glass

Profession-specific decorative details may use related materials.

Examples:

Woodcutting may use subtle wood accents.

Smithing may use iron and ember accents.

Archaeology may use sandstone accents.

---

# Panel Styles

The interface should use a small number of shared panel styles.

## Background Panel

Used for:

Screen sections

Large containers

Lists

Appearance:

Dark background

Minimal border

Low contrast

Little or no shadow

---

## Standard Panel

Used for:

Information blocks

Activity details

Item details

Appearance:

Standard panel color

Thin border

Small corner radius

Subtle inner highlight

---

## Raised Panel

Used for:

Selected content

Important details

Popups

Appearance:

Lighter panel background

Stronger border

Soft shadow

Clear separation from background

---

## Active Panel

Used for:

Current activity

Current enemy

Selected target

Appearance:

Strong accent border

Subtle accent glow

Selected marker

Do not use intense pulsing except for urgent warnings.

---

## Warning Panel

Used for:

Inventory full

Missing requirements

Dangerous actions

Appearance:

Standard dark panel

Warning-colored border

Warning icon

Clear explanation

---

# Panel Borders

Borders should remain relatively thin.

Recommended normal border thickness:

1–2 pixels at 1920 × 1080

Recommended selected border thickness:

2–3 pixels

Important decorative frames may be slightly thicker.

Avoid using thick borders around every small element.

Border hierarchy:

No border:

Background containers

Subtle border:

Normal cards

Strong border:

Selected cards

Accent border:

Active content

Warning border:

Danger or missing requirements

---

# Corner Style

The interface should use slightly rounded or subtly cut corners.

Recommended direction:

Small 4–8 pixel corner radius

Or subtle fantasy cut-corner shapes

Avoid extremely rounded mobile-style pills for every panel.

Pill shapes may be used for:

Small status labels

Filters

Notification badges

Tags

---

# Shadows

Shadows should be subtle.

Use shadows to distinguish:

Popups

Raised panels

Dropdowns

Tooltips

Modals

Avoid large soft shadows behind every card.

Recommended direction:

Low-opacity black

Small downward offset

Moderate blur

Shadows should support depth rather than decoration.

---

# Dividers

Dividers should separate information without becoming dominant.

Use:

Thin lines

Low-contrast borders

Small spacing gaps

Section labels

Recommended divider color:

A muted version of the Primary Border color

Avoid bright white divider lines.

---

# Spacing System

The interface should use a consistent spacing scale.

Suggested base spacing unit:

4 pixels at 1920 × 1080

Recommended spacing values:

4 pixels:

Very small internal spacing

8 pixels:

Icon-to-text spacing

12 pixels:

Compact card padding

16 pixels:

Standard panel padding

24 pixels:

Large section spacing

32 pixels:

Major screen separation

Use consistent multiples rather than random spacing values.

---

# Standard Padding

Suggested starting values:

Small button:

8–12 pixels horizontal padding

Standard button:

12–20 pixels horizontal padding

Card:

12–16 pixels

Standard panel:

16–20 pixels

Large details panel:

20–24 pixels

Popup:

24–32 pixels

Exact values may be adjusted during reference image creation.

---

# Typography Philosophy

The UI should use no more than two primary font families.

Recommended structure:

One title or display font

One highly readable interface font

The title font may have fantasy character.

The interface font should prioritize readability.

Avoid using decorative fantasy fonts for:

Long descriptions

Item statistics

Combat logs

Small buttons

Requirement lists

All text should use TextMeshPro.

---

# Font Licensing Rule

Final fonts must have appropriate licensing for commercial game use.

Codex should not import random fonts from unknown sources.

Until final fonts are selected, Codex should use approved project placeholder fonts.

Placeholder fonts should later be replaceable through shared TextMeshPro font assets and theme settings.

---

# Text Hierarchy

Suggested starting text sizes at 1920 × 1080:

## Major Screen Title

Size:

30–36

Use for:

Combat

Woodcutting

Inventory

Collection Log

---

## Section Title

Size:

22–28

Use for:

Current Target

Available Activities

Equipment Stats

---

## Card Title

Size:

18–22

Use for:

Tree names

Enemy names

Recipe names

Items

---

## Standard Text

Size:

15–18

Use for:

Descriptions

Requirements

Statistics

---

## Secondary Text

Size:

13–16

Use for:

Minor values

Source information

Cooldown text

---

## Compact Label

Size:

11–14

Use for:

Badges

Small timers

Minor card information

Text should not become smaller simply to force more content onto one screen.

Scrolling should be used instead.

---

# Text Weight

Recommended hierarchy:

Bold or semi-bold:

Major titles

Selected content

Important values

Medium:

Button labels

Card titles

Normal:

Descriptions

Secondary statistics

Muted:

Minor information

Avoid using bold text for every label.

---

# Text Alignment

Recommended alignment:

Screen titles:

Left aligned

Card names:

Left aligned

Numeric values:

Right aligned when displayed in rows

Progress-bar text:

Centered or left aligned depending on bar size

Buttons:

Centered

Long descriptions:

Left aligned

Avoid centered long paragraphs.

---

# Text Overflow

Compact UI elements may use:

Ellipsis

Tooltip expansion

Detail-panel expansion

Long descriptions should use wrapping.

Do not reduce font size drastically to fit a long item name.

---

# Button Philosophy

Buttons should clearly look interactive.

Buttons should have:

Readable label

Clear border

Visible hover response

Visible pressed response

Visible disabled state

Consistent internal padding

Buttons should not depend entirely on glowing effects.

---

# Primary Button

Used for:

Start activity

Start combat

Craft

Buy

Claim Reward

Confirm major action

Appearance:

Primary Accent background or strong accent border

Bright readable text

Stronger visual weight than surrounding controls

There should normally be only one dominant Primary Button in a small decision area.

---

# Secondary Button

Used for:

Open details

View sources

Change selection

Cancel non-dangerous actions

Appearance:

Raised dark panel

Standard border

Secondary Accent on hover or selection

---

# Danger Button

Used for:

Destroy item

Reset save

Quit dangerous encounter

Confirm permanent loss

Appearance:

Dark red background or red border

Negative status color

Clear destructive label

Danger buttons should not visually resemble Primary confirmation buttons.

---

# Icon Button

Used for:

Settings

Help

Close

Favorite

Lock

Search

Appearance:

Square or compact

Clear icon

Tooltip on hover

Visible hover background

Icon-only buttons should always have a tooltip.

---

# Button States

Every standard button should support:

Normal

Hovered

Pressed

Selected

Disabled

Locked

Loading

## Normal

Standard border and background.

## Hovered

Slightly brighter background.

Stronger border.

Small optional glow.

## Pressed

Darker background.

Slight visual depression.

## Selected

Persistent accent border.

Optional selected marker.

## Disabled

Reduced contrast.

Muted text.

No misleading hover response.

## Locked

Lock icon.

Requirement explanation.

## Loading

Loading spinner or progress indicator.

Prevent repeated activation.

---

# Button Animation

Button feedback should be fast.

Suggested effects:

Small brightness change

Very small scale change

Short border transition

Subtle sound

Avoid:

Large bouncing

Long glow animations

Buttons moving far from their original position

Routine button interaction should feel immediate.

---

# Tab Style

Tabs should visually connect to their content.

Selected tab:

Raised appearance

Accent line or border

Brighter text

Unselected tab:

Darker background

Muted text

Hovered tab:

Slight highlight

Tabs should not all resemble large primary buttons.

Horizontal tab rows may scroll when many tabs exist.

---

# Filter Style

Filters may use:

Compact buttons

Toggle chips

Dropdowns

Checkbox rows

Active filters should remain clearly visible.

Suggested active filter appearance:

Secondary Accent border

Small checkmark

Brighter label

The player should be able to identify active filters at a glance.

---

# Persistent Left Navigation Style

The Left Navigation should establish the game's main visual identity.

Appearance:

Dark metal, stone, or wood-inspired background

Clear separation from main content

Subtle vertical border on its right edge

Scrollable content

Consistent icon placement

Navigation entries should display:

Icon

Text label

Selected state

Lock state

Notification badge

---

# Navigation Group Headers

Group headers should appear visually different from navigation entries.

Possible appearance:

Smaller uppercase or semi-bold text

Muted accent line

Expandable arrow

Darker background

Examples:

COMBAT

GATHERING

CRAFTING

PROGRESSION

Group headers should not compete with selected screen entries.

---

# Navigation Selected State

The selected navigation entry should use:

Accent-colored vertical line

Raised background

Brighter icon

Brighter text

Optional soft glow

Do not rely only on changing text color.

---

# Navigation Hover State

Hovered entries should use:

Slightly lighter background

Brighter border or icon

Fast transition

Hover should not look identical to selected state.

---

# Navigation Locked State

Locked entries should use:

Muted icon

Muted text

Lock icon

Requirement tooltip

Do not hide every future system automatically.

Some locked systems may be visible to show future progression.

---

# Navigation Notification Badges

Notification badges may show:

New

Number

Claim

Ready

Warning icon

Badge colors should reflect meaning.

Examples:

Claimable reward:

Gold accent

Warning:

Orange

Critical:

Red

New content:

Blue or teal

Avoid placing red badges everywhere.

---

# Collapsed Navigation Style

Collapsed Navigation should show:

Icons

Selected marker

Notification badges

Tooltips on hover

The collapsed state should preserve the same visual theme.

Do not create a completely different style for collapsed Navigation.

---

# Top Bar Style

The Top Bar should appear as a persistent global information strip.

It may contain:

Gold

Inventory capacity

Notifications

Save status

Settings shortcut

Appearance:

Dark raised strip

Subtle lower border

Compact information groups

Consistent icon and value pairs

The Top Bar should not contain current activity information.

---

# Active Activity Bar Style

The Active Activity Bar should visually communicate that gameplay continues while browsing.

Appearance:

Raised panel below the Top Bar

Strong activity icon

Current activity name

Current target or recipe

Progress bar

Stop or Quit control

The bar may use a small Profession or Combat accent.

It should remain visually distinct from the Top Bar.

---

# Active Activity Bar Modes

## No Activity

Muted appearance

Text:

No Active Activity

Optional small suggestion

---

## Profession Activity

Profession accent

Profession icon

Action progress

Target or recipe name

---

## Combat Activity

Combat accent

Health

Devotion

Enemy name

Quit control

The Combat mode should not become a replacement for the full Combat screen.

---

# Screen Header Style

A Screen Header should clearly identify the active system.

Possible elements:

Large icon

Screen name

Level

XP bar

Short subtitle

Help button

Statistics button

Appearance:

Dark raised strip or panel

System-specific accent line

Consistent height

Screen Headers should not use completely different structures for every Profession.

---

# Scrollable Content Style

Scrollable content should appear naturally connected to the screen.

Use:

Consistent panel spacing

Clear section headings

Subtle scrollbars

Expandable cards

Avoid:

Large empty gaps

Multiple visible vertical scrollbars beside one another

Tiny scrollable boxes inside larger scrollable boxes

The player should easily understand which area is currently scrollable.

---

# Scrollbar Style

Scrollbars should be visible but subtle on PC.

Scrollbar track:

Very dark

Low contrast

Scrollbar handle:

Muted steel

Brighter on hover

Accent when actively dragged if desired

Suggested width:

8–14 pixels

Avoid extremely thin scrollbars that are difficult to click.

---

# Card Style

Cards should be the main reusable content unit.

Cards may represent:

Activities

Enemies

Regions

Dungeons

Recipes

Items

Companions

Achievements

Shop entries

Standard Card appearance:

Dark panel

Thin border

Icon area

Title

Important values

Clear selection state

Card layouts should remain consistent within the same content type.

---

# Card Density

Cards may use three density levels.

## Comfortable Card

Used for:

Important selections

Region cards

Dungeon cards

Companions

Contains:

Large icon

Description

Multiple statistics

---

## Standard Card

Used for:

Activities

Enemies

Recipes

Achievements

Contains:

Medium icon

Title

Important statistics

---

## Compact Row

Used for:

Requirements

Stats

Sources

Combat Log entries

Contains:

Small icon

One or two text lines

Values

A screen should not randomly mix density levels without purpose.

---

# Selected Card State

Selected cards should use:

Accent border

Raised background

Selected marker or check

Brighter title

Optional subtle glow

Selection should remain visible even when the cursor is no longer hovering.

---

# Locked Card State

Locked cards should use:

Reduced saturation

Muted background

Lock icon

Visible requirement

The card should remain readable.

Do not place an opaque black overlay over all information unless content is intentionally secret.

---

# Activity Card Style

Profession Activity Cards should display:

Activity icon

Activity name

Required level

Action time

Primary reward

XP

Locked or active state

Start button if used

Active Activity Card:

Profession accent border

Active marker

Progress indicator

Locked Activity Card:

Lock icon

Requirement text

Muted controls

---

# Region Selection Style

Combat Region cards should feel like major location choices.

Possible elements:

Region illustration or icon

Region name

Recommended level

Completion information

Locked state

Appearance:

Wider cards

Strong image area

Subtle location-specific accent

Selected Region:

Accent frame

Selected marker

Expanded lower selection content

---

# Activity Type Selection Style

Combat Activity Type buttons may represent:

Areas

Elite Areas

Dungeons

Bosses

Tower

Appearance:

Large icon button or compact card

Consistent width

Clear selected state

Visible lock state

They should feel more important than ordinary filters but less important than Region cards.

---

# Enemy Card Style

Enemy Cards should display:

Enemy icon or portrait

Enemy name

Difficulty

Health

Damage type

Loot preview button

Locked state

Selected Enemy:

Stronger border

Expanded details

Visible Start Combat path

Enemy Cards should not look identical to Inventory item slots.

---

# Dungeon Card Style

Dungeon Cards should display:

Dungeon illustration or icon

Dungeon name

Difficulty

Completion count

Entry requirement

Reward chest icon

Dungeon Cards should feel larger and more important than regular enemy cards.

---

# Boss Card Style

Boss Cards should use:

Larger portrait area

Strong border

Distinct warning or challenge accent

Difficulty information

Loot preview

Boss presentation should feel threatening without using constant flashing.

---

# Active Combat Visual Style

The Active Combat screen should prioritize clarity.

The visual center should be:

Player

Enemy

Attack progress

Health

Devotion

Abilities

Combat controls

The screen may use a darker combat background than normal Profession screens.

The player and enemy panels should appear visually opposed.

Example direction:

Player panel aligned left

Enemy panel aligned right

Companion panel near Player

Combat Abilities below Player

Enemy abilities below Enemy

---

# Player Combat Panel Style

Player panel should display:

Portrait or character icon

Health

Devotion

Combat Discipline

Attack progress

Equipment summary

Buffs

Debuffs

Appearance:

Raised dark panel

Combat Discipline accent

Strong Health and Devotion bars

---

# Enemy Combat Panel Style

Enemy panel should display:

Enemy image

Name

Health

Attack progress

Abilities

Buffs

Debuffs

Appearance:

Raised dark panel

Enemy or danger accent

Clear warning effects

Bosses may use a stronger decorative frame.

---

# Companion Combat Panel Style

The Companion panel should be smaller than the Player panel.

It should display:

Companion icon

Name

Rank

Attack progress

Ability cooldown

Damage

The Companion panel should not contain:

Health bar

Death icon

Target icon

Companion visual treatment should reinforce support and contribution rather than survival.

---

# Combat Attack Progress Bars

Player, Enemy, and Companion attack bars should use a consistent shape.

They may use different accents.

Player:

Combat Discipline accent

Enemy:

Red or hostile accent

Companion:

Purple or companion accent

Attack bars should display:

Progress fill

Optional time remaining

Attack icon

Bars should be visually distinct from Health bars.

---

# Combat Ability Button Style

Ability buttons should resemble compact action slots.

Each button should display:

Ability icon

Cooldown overlay

Remaining cooldown

Auto-use state

Unavailable state

Ready state

Suggested shape:

Square or slightly rectangular

Strong border

Dark background

Cooldown overlay filling downward or radially

Ready abilities may use a subtle glow.

Avoid constant flashing.

---

# Auto-Use Indicator

Auto-enabled abilities should display:

Small Auto label

Toggle icon

Accent border

The indicator should remain visible during cooldown.

---

# Consumable Slot Style

Food, Healing Potion, and Elixir slots should remain visually distinct.

Food:

Warm brown or orange accent

Healing Potion:

Red accent

Elixir:

Blue, green, purple, or effect-based accent

Each slot should display:

Icon

Quantity

Duration or cooldown

Active state

Empty state

---

# Rune Summary Style

Active Runes should appear as small arcane slots.

Appearance:

Dark background

Arcane border

Rune symbol

Set-connection indicator when relevant

The Combat screen only displays the active Rune summary.

Rune selection belongs inside Runecrafting.

---

# Buff and Debuff Icons

Buff and Debuff icons should use:

Square icons

Small border

Duration overlay

Tooltip

Buffs:

Positive or neutral border

Debuffs:

Negative border

Avoid making every buff icon a different frame shape.

---

# Temporary Combat Loot Style

Temporary Loot should resemble a protected reward tray.

Each loot entry should display:

Item icon

Quantity

New Collection marker

Important marker

Claim action through clicking

The panel should include:

Capacity

Pick All button

Full warning

New Collection items should use a discovery marker without adding rarity colors.

---

# Combat Log Style

Combat Log rows should be compact.

Possible visual indicators:

Player attack icon

Enemy attack icon

Companion icon

Ability icon

Critical marker

Loot icon

Suggested colors:

Player action:

Light blue or neutral

Enemy damage:

Muted red

Companion action:

Purple

Healing:

Green

Loot:

Gold

The log should avoid bright rainbow text.

---

# Profession Screen Style

Profession screens should feel calmer than Combat.

They may use:

Profession-specific accent

Target illustration

Activity cards

Progress bars

Tool slot

Companion slot

Rewards

The overall structure should remain shared across Professions.

Profession identity should come from:

Icon

Accent color

Small material details

Background illustration

Not from completely different layouts.

---

# Target-Based Profession Style

Trees, ore deposits, excavation sites, and similar targets should display:

Target image or icon

Target name

Durability

Reward thresholds

Profession damage

Respawn state

Target durability should not use the same red Health presentation as enemies.

---

# Reward Threshold Style

Threshold markers may appear along the durability bar.

Example thresholds:

75%

50%

25%

0%

Completed threshold:

Checkmark

Accent highlight

Claimed marker

Upcoming threshold:

Visible marker

Reward preview in tooltip

Thresholds should be generated from data.

Do not visually assume every target uses the same thresholds.

---

# Crafting Screen Style

Crafting should visually connect:

Recipe

Materials

Output

Time

XP

Recommended direction:

Recipe list on one side

Selected recipe details on the other

Material flow leading toward output

Materials should show:

Owned

Required

Reserved

Missing

Missing materials should use warning or negative indicators.

---

# Inventory Style

The Inventory should be dense but readable.

Main areas:

Search and filters

Scrollable item grid

Selected item details

Actions

Inventory slots should use consistent square frames.

The Inventory background should remain neutral so item icons are easy to see.

---

# Item Slot Style

Every Item Slot should support:

Normal

Hovered

Selected

New

Locked

Favorite

Equipped

Reserved

Empty

Standard appearance:

Dark square background

Thin border

Centered icon

Quantity in lower corner

Status icons in corners

Do not use item rarity-colored frames.

---

# Item Slot Status Placement

Suggested placement:

Quantity:

Bottom-right

Lock:

Top-left

Favorite:

Top-right

New:

Small accent marker near top

Equipped:

Bottom-left

Reserved:

Small chain or reservation icon

Avoid placing multiple large badges over the item icon.

---

# Selected Item Slot

Selected item slot should use:

Accent border

Raised background

Optional small glow

Selected slots should remain visually distinct from merely hovered slots.

---

# New Collection Item Marker

A newly discovered item may display:

Small star

Book icon

Collection icon

Short New label

The marker should disappear after the discovery has been viewed according to UI rules.

---

# Equipment Slot Style

Equipment slots should display:

Slot icon when empty

Equipped item icon

Slot name in tooltip

Disabled state

Two-handed conflict state

Empty slots should remain understandable.

Disabled Shield slot should display:

Shield silhouette

Disabled overlay

Two-handed explanation

---

# Equipment Comparison Style

Stat changes should use:

Up arrow

Down arrow

Plus or minus value

Text label

Positive and Negative colors

New effects may use:

New label

Small star icon

Lost effects may use:

Removed label

Cross icon

Color alone should not communicate the comparison.

---

# Companion Card Style

Companion cards should display:

Portrait

Name

Rank

Category

Assignment

Ready state

Locked state

Combat Companions and Profession Companions may use different small category icons.

They should still share the same card frame.

---

# Companion Rank-Up Style

Rank-Up panel should emphasize:

Current rank

Next rank

Required items

Required Gold

Required time

New bonus

Start Rank-Up button

The next rank benefit should remain visually important.

Completed timers should use a clear Ready or Claim state.

---

# Shop Style

The Shop should feel like a merchant interface without sacrificing clarity.

Possible visual details:

Subtle wood or cloth accents

Merchant icon

Gold accent

Main areas:

Shop stock

Owned quantities

Prices

Purchase preview

Sell list

The Shop should use standard item cards and slots rather than unique controls for every shop.

---

# Price Display

Gold cost should display:

Gold icon

Exact value

Affordable or unaffordable state

Affordable:

Primary text or gold

Unaffordable:

Negative indicator

Do not reduce text opacity so much that the price becomes unreadable.

---

# Achievement Style

Achievement cards should display:

Icon

Name

Description

Progress

Reward

Completion state

Completed achievements may use:

Gold accent

Checkmark

Completion date

Incomplete achievements should remain readable.

Hidden achievements should follow their configured visibility rules.

---

# Collection Log Style

The Collection Log should focus on item discovery.

Entries should display:

Item icon or silhouette

Discovered state

Current owned quantity

Lifetime obtained quantity

Known source

Category

Undiscovered entries may use:

Silhouette

Question mark

Muted border

Known source information when allowed

Enemies, Bosses, and Dungeons should appear only as item sources.

---

# Collection Completion Style

Collection progress should use:

Overall completion percentage

Category completion

Discovered item count

Milestone rewards

The Collection Log should feel like a long-term archive.

Possible decorative theme:

Dark library

Archive

Codex

Museum-like frame accents

Do not sacrifice readability for parchment effects.

---

# Tooltip Style

Tooltips should use:

Dark raised panel

Strong border

Small shadow

Clear title

Optional icon

Readable description

Stat rows

Requirement rows

Tooltips should not use transparent backgrounds that make text difficult to read.

Tooltips should visually match Popups but remain more compact.

---

# Tooltip Width

Tooltips may use different maximum widths.

Compact Tooltip:

Stats and short descriptions

Standard Tooltip:

Items and abilities

Wide Tooltip:

Equipment comparison or complex effects

Long tooltips should wrap text.

They should remain inside screen boundaries.

---

# Popup Style

Popups should use:

Darkened backdrop when blocking input

Raised central panel

Strong border

Clear title

Focused content

Clear buttons

Popup sizes should match content.

Avoid making every popup nearly full screen.

---

# Modal Style

Important modals should use:

Dark background overlay

Strong raised frame

Warning or system accent

Clear action hierarchy

Dangerous confirmation:

Danger button

Secondary Cancel button

The safe option should remain easy to identify.

---

# Notification Style

Notifications should appear as compact raised cards.

Possible contents:

Icon

Title

Short message

Reward amount

Progress

Notification types may use small accent borders.

Examples:

Achievement:

Gold

Collection Discovery:

Teal or blue

Error:

Red

Warning:

Orange

Profession Level:

Profession accent

Notifications should not fill large parts of the screen.

---

# Notification Badge Style

Badges should remain compact.

Possible badge forms:

Small circle

Rounded rectangle

Icon marker

Badges may contain:

Number

Exclamation mark

Checkmark

New label

Avoid oversized badges covering navigation icons.

---

# Empty State Style

Empty states should display:

Simple icon

Short title

Helpful explanation

Optional action

Example:

No Items Match Your Filters

Clear Filters

Empty states should feel intentional rather than unfinished.

---

# Loading State Style

Loading states may use:

Small spinner

Progress bar

Status text

Loading states should match the dark-fantasy interface.

Avoid bright default Unity loading spinners.

---

# Error State Style

Error states should display:

Error icon

Clear message

Whether progress is safe

Suggested next action

Technical error codes should not dominate the player-facing interface.

---

# Inventory Full Warning Style

Inventory Full should use:

Warning panel

Inventory icon

Current capacity

Clear consequence

Direct Inventory shortcut

Example:

Inventory Full

100 / 100 Slots Used

Woodcutting has stopped.

The warning should be noticeable without covering the entire screen permanently.

---

# Offline Progress Summary Style

The Offline Progress Summary should feel rewarding.

Recommended structure:

Time Away

Main results

Level-ups

Rare rewards

Collection discoveries

Achievements

Overflow warnings

Claim actions

Possible visual direction:

Large summary popup

Scrollable details

Strong reward icons

Grouped results

Avoid showing dozens of separate popups after returning.

---

# Icon Style

Icons should share one visual language.

Recommended icon direction:

Fantasy-themed

Readable silhouettes

Moderate detail

Strong contrast

Consistent viewing angle

Consistent line thickness

Consistent lighting

Avoid mixing:

Flat vector icons

Photorealistic icons

Pixel-art icons

Highly rendered 3D icons

Cartoon emoji-style icons

Unless the entire game deliberately uses that mixture.

---

# Icon Backgrounds

Icons may use transparent backgrounds.

Item icons should normally not include their own unrelated decorative frame.

The UI slot provides the frame.

This allows the same icon to appear inside:

Inventory

Collection Log

Crafting

Shop

Loot

Requirements

---

# Missing Icon Style

Missing assets should display one approved placeholder.

Suggested placeholder:

Dark square

Question mark

Subtle warning border

Codex should not create a different temporary icon for every missing asset.

Missing icons should also produce a development warning.

---

# Character and Enemy Images

Combat portraits should use a consistent presentation.

Possible format:

Bust portrait

Creature portrait

Framed illustration

Images should use:

Similar crop

Similar lighting

Similar background treatment

Player and enemy art should not use completely different proportions.

---

# Region Artwork

Region cards may use landscape illustrations.

Artwork should remain dark enough for text overlays when necessary.

Prefer placing text in a separate panel rather than directly over detailed art.

---

# Placeholder Art Rules for Codex

Until final artwork exists, Codex may use placeholders.

Approved placeholder types:

Solid-color panel

Simple silhouette

Letter or symbol icon

Simple geometric shape

Clearly labeled temporary image

Codex should not:

Import random copyrighted images

Use unrelated internet images

Generate inconsistent art styles for each screen

Use default Unity sprites without adapting them to the interface

Create permanent-looking assets without approval

Placeholder assets should be easy to replace.

---

# Reference Image Rules

Future UI reference images should guide:

Layout

Visual hierarchy

Panel proportions

Color usage

Spacing

Component appearance

Reference images should not become hardcoded screenshots inside the game.

The implemented UI must remain dynamic and data-driven.

Documents 16, 17, and 18 remain the structural source of truth.

Reference images refine visual presentation.

When a reference image conflicts with gameplay rules:

Gameplay and system documents take priority.

When a reference image conflicts only with spacing or appearance:

The approved reference image may guide the final visual result.

---

# Animation Philosophy

Animations should support information.

Good animation uses:

Hover response

Selection response

Panel opening

Progress completion

Reward reveal

Level-up

Rare item discovery

Bad animation uses:

Constant movement

Long delays

Large bouncing panels

Flashing backgrounds

Animations that prevent clicking

---

# Default Animation Timing

Suggested starting durations:

Button hover:

0.08–0.15 seconds

Button press:

0.05–0.10 seconds

Panel fade:

0.12–0.20 seconds

Popup opening:

0.15–0.25 seconds

Notification arrival:

0.15–0.25 seconds

Screen transition:

0.15–0.30 seconds

Exact timings may be adjusted after testing.

---

# Reduced Motion

Reduced Motion should disable or shorten:

Large panel sliding

Scale bounce

Screen shake

Flashing highlights

Repeated reward movement

Important state changes should remain visible through:

Color

Icons

Text

Simple fades

---

# Hover Effects

Hover effects should be subtle.

Possible effects:

Slight brightness increase

Stronger border

Small icon highlight

Tooltip appearance

Avoid:

Large scale increases

Continuous animation

Strong bloom

Hovered elements should not shift surrounding layout.

---

# Selected Effects

Selected elements should use persistent visual signals.

Possible signals:

Accent border

Selected indicator

Raised background

Brighter title

Checkmark

Do not use only a temporary animation to communicate selection.

---

# Disabled Effects

Disabled controls should use:

Muted background

Muted text

No active glow

Disabled icon

Requirement explanation

Disabled buttons should still remain readable.

---

# Locked Effects

Locked content should use:

Lock icon

Muted saturation

Visible requirement

Optional silhouette

Avoid completely hiding normal progression information unless the content is intended to remain secret.

---

# Active Effects

Currently active content should use:

Active label

Accent border

Progress animation

Current activity icon

Possible small glow

Active content should not rely on pulsing alone.

---

# Visual Information Density

The game contains large amounts of information.

Use layering:

Primary information:

Always visible

Secondary information:

Visible inside cards or details panels

Advanced information:

Tooltips, expandable sections, or Statistics panels

Do not place every possible statistic directly on every card.

---

# Data Table Style

Stat tables and requirement lists should use:

Alternating subtle row backgrounds if useful

Left-aligned labels

Right-aligned values

Consistent row height

Thin dividers

Positive and negative indicators

Avoid bright spreadsheet-style grids.

---

# Chart and Graph Style

Future charts should use:

Dark backgrounds

Clear axes

Limited accent colors

Readable labels

Tooltips

No unnecessary 3D effects

Charts should follow the same status and system color rules.

---

# Responsive Visual Rules

At smaller landscape widths:

Navigation may collapse.

Details panels may move into overlays.

Cards may become more compact.

Multi-column layouts may become one column.

Font size should not fall below comfortable readability.

At larger widths:

Main content may use a maximum width.

Panels should not stretch excessively.

Additional empty width may be used for spacing or secondary panels.

---

# Ultrawide Rules

On ultrawide displays:

Keep Left Navigation at controlled width.

Keep Top Bar stretched.

Center or limit main content width when appropriate.

Do not stretch item cards or text lines across the entire screen.

Secondary information panels may use extra space.

---

# Future Mobile Visual Rules

Future mobile layouts may use:

Bottom Navigation

Navigation drawer

Full-screen panels

Larger buttons

Larger item slots

Reduced simultaneous information

The same colors, fonts, icons, panel style, and component identity should remain recognizable.

Mobile should look like the same game, not a different product.

---

# Accessibility Visual Rules

The interface should support:

High contrast

Color-blind friendly indicators

Larger UI scale

Larger text

Reduced motion

Exact number display

Important information should never depend only on color.

Examples:

Positive:

Green plus up arrow

Negative:

Red plus down arrow

Locked:

Grey plus lock icon

Selected:

Accent plus border and marker

---

# Visual Theme Data

Visual values should be centralized where practical.

Possible shared theme values:

Background colors

Panel colors

Border colors

Text colors

Status colors

Spacing values

Corner sizes

Standard animation durations

Standard sprites

Standard font assets

Changing a shared style should update multiple screens consistently.

Codex should avoid hardcoding unrelated colors into every screen script.

---

# Style Token Naming

Suggested token names:

ColorBackgroundDeep

ColorBackgroundMain

ColorPanelStandard

ColorPanelRaised

ColorBorderStandard

ColorBorderStrong

ColorAccentPrimary

ColorAccentSecondary

ColorTextPrimary

ColorTextSecondary

ColorTextMuted

ColorPositive

ColorNegative

ColorWarning

ColorHealth

ColorDevotion

ColorProfessionXP

ColorCombatXP

SpacingSmall

SpacingStandard

SpacingLarge

CornerStandard

AnimationFast

AnimationStandard

Exact implementation belongs in the Unity UI Technical Architecture.

---

# Asset Naming Conventions

Suggested asset names:

UI_BG_Main

UI_BG_Panel

UI_Frame_Standard

UI_Frame_Selected

UI_Button_Primary

UI_Button_Secondary

UI_Button_Danger

UI_Slot_Item

UI_Slot_Equipment

UI_Slot_Rune

UI_Icon_Lock

UI_Icon_Favorite

UI_Icon_New

UI_Icon_Warning

UI_Icon_Unknown

UI_Bar_Health

UI_Bar_Devotion

UI_Bar_XP

UI_Bar_Durability

Assets should use clear names.

Avoid names such as:

NewButton

PanelFinal

Image2

TestFrame

RealFinalIcon

---

# Codex Visual Rules

Codex should follow these rules when constructing UI.

Use shared components.

Use shared colors.

Use shared fonts.

Use shared panel styles.

Use shared button states.

Use shared spacing.

Use shared icon conventions.

Do not invent a completely different visual style for each screen.

Do not assign random colors to new systems.

Do not add item rarity colors.

Do not add excessive glow.

Do not use default Unity styling as final UI.

Do not reduce text until it becomes difficult to read.

Use scrolling when information does not fit.

Do not hide requirements behind color alone.

Do not add permanent artwork without a clear replacement path.

Do not copy another game's exact UI.

---

# Visual Acceptance Criteria

The visual design is acceptable when:

The game has a recognizable dark-fantasy identity.

Text remains easy to read.

The current screen is immediately clear.

Selected content is immediately clear.

Active content is immediately clear.

Locked content explains its requirements.

Buttons have consistent states.

Cards have consistent structure.

Progress systems use consistent colors.

Combat and Profession screens feel related but distinct.

Item slots do not use invented rarity colors.

Navigation feels persistent and organized.

The Top Bar and Active Activity Bar remain visually separate.

Tooltips, Popups, and Notifications share the same design language.

The interface remains usable at 1920 × 1080 and 2560 × 1440.

Future reference images can be reproduced using the same visual rules.

---

# Reference Images to Create Later

The following reference images should eventually be created.

## Main UI Shell

Should show:

Top Bar

Left Navigation

Active Activity Bar

Main Screen area

Overlay placement

---

## Combat Selection Screen

Should show:

Region row

Activity Type row

Location selection

Enemy selection

Breadcrumb

Enemy details

Loot preview

---

## Active Combat Screen

Should show:

Player panel

Enemy panel

Companion panel

Health

Devotion

Attack progress bars

Abilities

Consumables

Active Runes

Temporary Loot

Combat Log

---

## Standard Profession Screen

Woodcutting is the recommended first example.

Should show:

Profession Header

Level and XP

Activity list

Selected tree

Target durability

Reward thresholds

Tool

Companion

Active bonuses

Start or Stop control

---

## Inventory Screen

Should show:

Search

Filters

Item grid

Selected item

Item details

Item actions

Capacity

---

## Equipment Screen

Should show:

Equipment slots

Character or equipment layout

Current stats

Equipment comparison

Available item list

---

## Crafting Screen

Should show:

Recipe list

Selected recipe

Materials

Reserved quantities

Output

Crafting time

XP

Craft control

---

## Companion Screen

Should show:

Companion list

Selected companion

Rank

Bonuses

Rank-up materials

Rank-up timer

Assignment controls

---

## Shop Screen

Should show:

Shop stock

Currency

Item prices

Owned quantities

Purchase preview

Sell tab

---

## Achievement Screen

Should show:

Categories

Completion

Achievement list

Progress bars

Rewards

Tracked achievements

---

## Collection Log Screen

Should show:

Item categories

Discovered and undiscovered items

Current quantity

Lifetime obtained

Item sources

Completion milestones

---

# Final Checklist

## Identity

✓ Does the UI look like a dark-fantasy RPG?

✓ Does it remain modern and readable?

✓ Does it avoid directly copying another game?

✓ Do all screens feel like the same game?

---

## Colors

✓ Are dark neutral backgrounds used consistently?

✓ Are accent colors controlled?

✓ Are status colors consistent?

✓ Are Combat Discipline colors used only as accents?

✓ Are Profession colors used only as accents?

✓ Are item rarity colors absent?

---

## Typography

✓ Are titles visually distinct?

✓ Is standard text readable?

✓ Are descriptions left aligned?

✓ Are values aligned consistently?

✓ Are decorative fonts avoided for small text?

---

## Panels

✓ Are panel styles reused?

✓ Are selected panels clearly visible?

✓ Are borders not excessively thick?

✓ Are shadows subtle?

✓ Is padding consistent?

---

## Buttons

✓ Does every button look interactive?

✓ Are Hovered and Selected states different?

✓ Are Disabled states readable?

✓ Are Dangerous actions visually distinct?

✓ Do icon-only buttons have tooltips?

---

## Navigation

✓ Is the selected screen clear?

✓ Are groups visually organized?

✓ Are notification badges controlled?

✓ Does collapsed Navigation remain understandable?

---

## Progress

✓ Is Health always visually distinct?

✓ Is Devotion distinct from Health?

✓ Is XP distinct from Durability?

✓ Are attack progress bars distinct from Health bars?

✓ Is progress readable without relying only on bar width?

---

## Cards and Slots

✓ Are cards consistent within each content type?

✓ Are item slots readable?

✓ Are status icons positioned consistently?

✓ Are selected and hovered slots different?

✓ Are rarity frames not used?

---

## Combat

✓ Are Player and Enemy states clear?

✓ Is Companion visually present without Health UI?

✓ Are abilities readable?

✓ Are Temporary Loot and Combat Log clearly separated?

✓ Are warnings noticeable without excessive flashing?

---

## Professions

✓ Do Profession screens share a structure?

✓ Does each Profession have a recognizable accent?

✓ Is Target Durability distinct from enemy Health?

✓ Are reward thresholds visible?

---

## Overlays

✓ Do Tooltips remain readable?

✓ Do Popups use a shared visual language?

✓ Do Modals clearly block interaction?

✓ Do Notifications remain compact?

---

## Accessibility

✓ Is information communicated with icons and text as well as color?

✓ Does Reduced Motion preserve understanding?

✓ Does UI scaling preserve layouts?

✓ Does high contrast remain possible?

---

## Codex

✓ Are shared components reused?

✓ Are colors not randomly hardcoded?

✓ Are placeholder assets clearly temporary?

✓ Are missing icons handled safely?

✓ Can final art replace placeholders?

✓ Can reference images guide the implementation without replacing dynamic UI?