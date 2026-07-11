# 17 Unity UI Technical Architecture

Version: 1.0  
Status: Draft  
Engine: Unity  
UI System: Canvas / uGUI  
Text System: TextMeshPro  
Primary Platform: PC Landscape  
Reference Resolution: 1920 × 1080

---

# Purpose

This document defines how Codex should technically construct the game's User Interface inside Unity.

It converts the player-facing rules from the UI and UX Framework into:

Unity hierarchy rules

Canvas configuration

Screen architecture

Navigation architecture

ScrollRect architecture

Reusable prefab architecture

Dynamic list generation

Popup and tooltip systems

UI event handling

Screen state preservation

Responsive layout rules

Performance rules

Testing requirements

Implementation order

The UI and UX Framework defines what the interface should do.

This document defines how Codex should build it.

The UI Visual Style Guide should separately define:

Colors

Fonts

Borders

Panel appearance

Button appearance

Icons

Animations

Visual identity

---

# Technical Goals

The Unity UI architecture should:

- Use Canvas / uGUI.
- Use TextMeshPro for all normal text.
- Use one primary gameplay scene.
- Keep persistent UI visible while screens change.
- Allow only one main screen to be active at a time.
- Support hundreds or thousands of data entries.
- Generate lists from game databases.
- Avoid manually creating every item or activity entry.
- Use reusable UI prefabs.
- Keep gameplay logic separate from UI scripts.
- Use events rather than constant polling.
- Support vertical screen scrolling.
- Avoid conflicting nested ScrollRects.
- Preserve important screen state.
- Support direct navigation to specific content.
- Support future mobile layouts.
- Remain understandable inside the Unity Hierarchy.
- Allow Codex to add content without rewriting core UI systems.

---

# Core Technical Decisions

The following decisions should be treated as current project rules.

UI System:

Unity Canvas / uGUI

Text:

TextMeshPro

Primary Orientation:

Landscape

Primary Reference Resolution:

1920 × 1080

Main Runtime Scene:

One primary gameplay scene

Screen Changes:

Show, hide, or instantiate UI screen prefabs inside one Screen Container

Persistent Navigation:

Left-side Navigation

Persistent Global Information:

Top Bar

Persistent Current Activity Information:

Active Activity Bar

Main Screen Scrolling:

One primary vertical ScrollRect per information-heavy screen

Main Screen Visibility:

Only one main screen active at a time

List Content:

Generated dynamically from database data

UI Logic:

Separated from gameplay calculations

Mobile Support:

Future requirement, not the initial layout target

---

# Project Folder Structure

Recommended Unity folders:

Assets

Game

UI

Core

Screens

Persistent

Navigation

Popups

Tooltips

Notifications

Components

Lists

Scroll

Animations

Debug

Prefabs

Screens

Persistent

Cards

Slots

Buttons

Popups

Tooltips

Notifications

Data

ScreenDefinitions

NavigationDefinitions

UISettings

Sprites

Icons

Backgrounds

Scripts

UI

Core

Screens

Navigation

Lists

Scroll

Popups

Tooltips

Notifications

State

Debug

Tests

UI folder example:

Assets/Game/UI/

Core/

Screens/

Persistent/

Navigation/

Popups/

Tooltips/

Notifications/

Components/

Lists/

Scroll/

Prefabs/

Data/

Tests/

Do not place all UI scripts inside one large folder.

Scripts should be grouped by responsibility.

---

# Runtime Scene Structure

The primary gameplay scene should contain a UI shell.

Recommended scene hierarchy:

[BOOTSTRAP] GameBootstrap

[SYSTEMS] GameSystems

[UI] UI System

[EVENTSYSTEM] EventSystem

[AUDIO] Audio System

The UI System should contain:

[UI] MainCanvas

[UI] UIManagers

The MainCanvas should contain the persistent shell and screen layers.

---

# Main Canvas Hierarchy

Recommended hierarchy:

[UI] MainCanvas

├── [PERSISTENT] TopBar

├── [LAYOUT] MainBody

│   ├── [PERSISTENT] LeftNavigation

│   └── [LAYOUT] ContentColumn

│       ├── [PERSISTENT] ActiveActivityBar

│       └── [SCREENS] ScreenContainer

├── [OVERLAYS] DropdownLayer

├── [OVERLAYS] TooltipLayer

├── [OVERLAYS] NotificationLayer

├── [POPUPS] PopupLayer

├── [MODALS] ModalLayer

└── [LOADING] LoadingLayer

The Top Bar should remain separate from the Active Activity Bar.

The Top Bar should contain account-wide information.

The Active Activity Bar should contain the current profession, crafting, or combat activity.

---

# Main Layout Structure

Recommended visual structure:

Top Bar

↓

Main Body

Left Navigation | Content Column

Content Column contains:

Active Activity Bar

Main Screen Container

The Top Bar stretches across the full width.

The Main Body fills the remaining height.

The Left Navigation occupies a fixed or collapsible width.

The Content Column fills the remaining width.

The Active Activity Bar remains fixed at the top of the Content Column.

The Screen Container fills the remaining area beneath the Active Activity Bar.

---

# Starting Layout Measurements

These values are starting recommendations and may be adjusted during visual testing.

Top Bar Height:

Approximately 56–72 pixels at 1920 × 1080

Active Activity Bar Height:

Approximately 64–88 pixels

Expanded Navigation Width:

Approximately 240–300 pixels

Collapsed Navigation Width:

Approximately 64–88 pixels

Standard Screen Header Height:

Approximately 72–96 pixels

Fixed Action Bar Height:

Approximately 64–88 pixels

Exact dimensions belong in the UI Visual Style Guide.

The architecture should not depend on one exact pixel value.

---

# Main Canvas Configuration

Main Canvas settings:

Render Mode:

Screen Space - Overlay

Canvas Scaler:

Scale With Screen Size

Reference Resolution:

1920 × 1080

Screen Match Mode:

Match Width Or Height

Match:

0.5 as a starting value

Reference Pixels Per Unit:

100 unless the art requires another value

Graphic Raycaster:

Enabled

Pixel Perfect:

Optional and normally disabled unless the art style requires it

The final Match value should be tested on:

1920 × 1080

2560 × 1440

Ultrawide resolutions

Lower landscape resolutions if later supported

---

# Canvas Rules

Use one main root Canvas by default.

Do not create a separate Canvas for every panel or card.

Additional nested Canvases may be used only when necessary for:

Independent sorting

Frequently changing interface groups

Special animation isolation

Performance optimization confirmed through profiling

Too many Canvases can increase rebuild and draw costs.

Use hierarchy order for most overlay sorting.

Use `overrideSorting` only when necessary.

---

# Overlay Order

Recommended order from lowest to highest:

Screen Container

Persistent UI

Dropdown Layer

Tooltip Layer

Notification Layer

Popup Layer

Modal Layer

Loading Layer

Recommended sorting order values if separate Canvases are used:

Main UI:

0

Dropdowns:

100

Tooltips:

200

Notifications:

300

Popups:

400

Modals:

500

Loading:

1000

These numbers are organizational guidelines.

---

# RectTransform Rules

All major UI elements should use anchors correctly.

Avoid positioning major panels using only absolute coordinates.

Top Bar:

Anchor Min:

0, 1

Anchor Max:

1, 1

Pivot:

0.5, 1

Stretch horizontally

Fixed height

Main Body:

Anchor Min:

0, 0

Anchor Max:

1, 1

Offset top equal to Top Bar height

Left Navigation:

Anchor vertically from bottom to top of Main Body

Fixed width

Content Column:

Stretch to remaining Main Body area

Active Activity Bar:

Stretch horizontally

Anchor to top of Content Column

Fixed height

Screen Container:

Stretch to remaining Content Column area beneath Active Activity Bar

---

# Layout Component Rules

Use these components deliberately:

Horizontal Layout Group

Vertical Layout Group

Grid Layout Group

Layout Element

Content Size Fitter

Aspect Ratio Fitter

Do not combine Layout Groups and Content Size Fitters carelessly.

Avoid putting a Content Size Fitter on an object whose size is already controlled by a parent Layout Group unless the relationship is intentionally designed.

Use Layout Element for:

Minimum width

Preferred width

Flexible width

Minimum height

Preferred height

Flexible height

Use Content Size Fitter mainly for dynamic content roots that need to grow according to their children.

---

# Avoiding Layout Rebuild Problems

Codex should avoid:

Deep chains of nested Layout Groups

Multiple Content Size Fitters controlling each other

Changing layout values every frame

Calling `LayoutRebuilder.ForceRebuildLayoutImmediate` repeatedly

Rebuilding every card when one text value changes

When an immediate layout rebuild is genuinely required:

Call it once after dynamic content has been created or expanded.

Do not call it from `Update`.

---

# UI Manager Hierarchy

Recommended manager objects:

[UI MANAGER] UIBootstrapper

[UI MANAGER] ScreenManager

[UI MANAGER] NavigationManager

[UI MANAGER] PopupManager

[UI MANAGER] ModalManager

[UI MANAGER] TooltipManager

[UI MANAGER] NotificationManager

[UI MANAGER] UIInputManager

[UI MANAGER] UIStateManager

[UI MANAGER] UIAudioManager

[UI MANAGER] UIThemeManager

These may be separate components on one UIManagers GameObject or separate child GameObjects.

Do not place gameplay systems inside UI managers.

---

# UI Bootstrapper

The `UIBootstrapper` initializes the UI.

Responsibilities:

Validate required references

Initialize UI services

Initialize the Screen Manager

Initialize Navigation

Open the default screen

Connect persistent UI to game systems

Apply saved UI settings

Apply UI scale

Restore Navigation state

Restore last screen when allowed

The UI Bootstrapper should run after core game databases and save data are available.

---

# UI Initialization Order

Recommended initialization order:

Load static databases

↓

Load or create save data

↓

Initialize gameplay systems

↓

Initialize UI services

↓

Build Navigation

↓

Initialize Persistent UI

↓

Open default or saved screen

↓

Display Offline Progress Summary if needed

The UI should not attempt to display player data before save and gameplay systems are initialized.

---

# Dependency Access

UI scripts should not repeatedly use:

`GameObject.Find`

`FindObjectOfType`

`FindFirstObjectByType`

Scene-wide searches during normal gameplay

Core dependencies should be assigned through:

Serialized references

Initialization methods

A composition root

A controlled service registry

Example:

`UIBootstrapper` may provide references to:

Inventory System

Combat System

Profession System

Crafting System

Companion System

Save System

Databases

UI Managers

Dependencies should be validated during initialization.

---

# Screen Architecture

Every main screen should be implemented as a reusable prefab.

Examples:

DashboardScreen.prefab

CombatScreen.prefab

InventoryScreen.prefab

EquipmentScreen.prefab

CompanionScreen.prefab

ShopScreen.prefab

AchievementScreen.prefab

CollectionLogScreen.prefab

WoodcuttingScreen.prefab

MiningScreen.prefab

The persistent shell remains in the scene.

Screen prefabs are instantiated inside Screen Container.

---

# Screen Loading Strategy

Recommended starting strategy:

Instantiate a screen prefab the first time it is opened.

Cache the screen instance.

Hide it when another screen opens.

Reuse it when opened again.

This provides:

Cleaner scene hierarchy

Faster initial scene setup

Lower initial UI object count

Preserved screen state

No repeated prefab loading

For very lightweight prototypes, some screens may initially exist in the scene.

The architecture should still support prefab-based screens.

---

# Screen Definition

Every screen should have a data definition.

Recommended `ScreenDefinition` fields:

Screen ID

Display Name

Icon

Screen Prefab

Default Navigation Group

Default Sort Order

Available From Start

Hidden Until Unlocked

Required Unlock ID if applicable

Preserve State

Allow Back Navigation

Future Mobile Prefab if applicable

Screen IDs should remain stable.

Do not use the visible screen name as the only internal identifier.

Example Screen IDs:

dashboard

combat

inventory

equipment

companions

shop

achievements

collection_log

profession_woodcutting

profession_mining

profession_runecrafting

---

# Screen Catalog

Create a `UIScreenCatalog` ScriptableObject.

The catalog should contain every `ScreenDefinition`.

The Screen Manager should use the catalog to resolve Screen IDs.

The catalog should support:

Finding a screen definition by ID

Validating duplicate IDs

Returning available screens

Returning locked screens

Adding a new screen without modifying Screen Manager code

---

# Base Screen Controller

Every main screen should inherit from or implement a shared screen contract.

Suggested class:

`UIScreenController`

Possible interface:

`IUIScreen`

Required properties:

Screen ID

IsInitialized

IsVisible

Required lifecycle methods:

Initialize

Open

Close

Refresh

HandleBack

SaveScreenState

RestoreScreenState

Suggested method outline:

```csharp
public abstract class UIScreenController : MonoBehaviour
{
    public abstract string ScreenId { get; }

    public bool IsInitialized { get; private set; }
    public bool IsVisible { get; private set; }

    public virtual void Initialize(UIScreenContext context) { }

    public virtual void Open(UIScreenOpenRequest request) { }

    public virtual void Close() { }

    public virtual void Refresh() { }

    public virtual bool HandleBack() => false;

    public virtual UIScreenState CaptureState() => null;

    public virtual void RestoreState(UIScreenState state) { }
}
```

The exact code may differ, but every screen should follow one consistent lifecycle.

---

# Screen Lifecycle

First opening:

Instantiate prefab.

Initialize references.

Connect to required gameplay systems.

Restore state if available.

Display screen.

Refresh visible data.

Later opening:

Restore existing instance.

Apply deep-link request if present.

Refresh changed data.

Display screen.

Closing:

Save temporary screen state.

Unsubscribe from unnecessary active-only events.

Hide screen.

Do not destroy the screen unless memory management requires it.

---

# Screen Visibility

Inactive main screens should normally have their root GameObject disabled.

Use:

```csharp
gameObject.SetActive(false)
```

This prevents invisible screens from:

Receiving input

Running unnecessary Update methods

Participating in layout calculations

Blocking raycasts

A screen transition may temporarily use a CanvasGroup.

After the transition completes, the inactive screen should be disabled.

---

# Screen Manager Responsibilities

The Screen Manager should:

Open screens by stable Screen ID

Instantiate missing screens

Cache screen instances

Close the current screen

Prevent multiple main screens from being active

Update selected Navigation entry

Maintain screen history

Handle deep-link requests

Restore screen state

Support default screen opening

Support screen unlocks

Handle invalid Screen IDs safely

The Screen Manager should not contain screen-specific gameplay logic.

---

# Open Screen Request

A screen-open request may contain:

Target Screen ID

Target Entry ID

Target Tab ID

Target Subsection ID

Return Screen ID

Preserve Current Screen In History

Force Refresh

Custom Payload if necessary

Suggested structure:

```csharp
public sealed class UIScreenOpenRequest
{
    public string ScreenId;
    public string TargetEntryId;
    public string TargetTabId;
    public string TargetSubsectionId;
    public bool AddCurrentToHistory = true;
    public bool ForceRefresh;
}
```

This supports direct source navigation.

---

# Screen History

The Screen Manager should maintain a Back stack.

Example:

Inventory

↓

Iron Ore details

↓

Mining screen

↓

Iron Deposit activity

Pressing Back should return through the previous navigation path.

The Back stack should store enough state to restore:

Screen ID

Selected entry

Selected tab

Scroll position if necessary

The stack should have a reasonable maximum size.

Example:

20–50 entries

Duplicate consecutive entries should not be added unnecessarily.

---

# Back Navigation Order

The UI Input Manager should process Back or Escape in this order:

Close temporary tooltip if necessary.

Close open dropdown.

Close top popup.

Close modal if closing is allowed.

Close detail overlay.

Allow active screen to handle Back.

Return to previous screen from Screen history.

Do nothing or open Pause menu.

Escape should not unexpectedly close the application.

---

# Navigation Architecture

The Left Navigation should be generated from data.

Create a NavigationEntryDefinition.

Recommended fields:

Navigation ID

Display Name

Icon

Target Screen ID

Navigation Group ID

Sort Order

Visible From Start

Hidden Until Unlocked

Unlock Requirement ID

Supports Notification Badge

Future Expansion Notes

Create a NavigationGroupDefinition.

Recommended fields:

Group ID

Display Name

Icon if needed

Sort Order

Collapsible

Expanded By Default

Visible

Entry List or category filter

---

# Navigation Construction

At UI initialization:

Load Navigation definitions.

Check unlock states.

Sort groups.

Sort entries.

Create Navigation Group prefabs.

Create Navigation Button prefabs.

Bind each button to its Screen ID.

Restore expanded group state.

Restore collapsed Navigation state.

Highlight the active screen.

Navigation entries should not be manually wired one by one in the scene.

---

# Navigation Prefabs

Recommended prefabs:

NavigationGroupHeader.prefab

NavigationEntryButton.prefab

NavigationLockedEntry.prefab if separate

NavigationBadge.prefab

Navigation button controller responsibilities:

Display icon

Display label

Display lock state

Display selected state

Display notification badge

Open target screen

Show tooltip when Navigation is collapsed

Navigation buttons should not directly control unrelated screens.

They should request navigation through the Screen Manager.

---

# Navigation Scrolling

The Left Navigation should use:

One vertical ScrollRect

A fixed optional header

A scrollable Content area

The Navigation Content should use:

Vertical Layout Group

Content Size Fitter with vertical preferred size

Top-aligned pivot

The Navigation ScrollRect should preserve its position during ordinary screen changes.

Expanding or collapsing a group should update content height.

---

# Navigation Collapse

Expanded and collapsed Navigation should use the same Navigation data.

Recommended approach:

Change preferred width through a Layout Element.

Hide text labels when collapsed.

Keep icons and badges visible.

Show tooltips for collapsed entries.

Save:

Collapsed state

Expanded width if adjustable

Expanded Navigation groups

Do not create two completely separate Navigation systems.

---

# Top Bar Architecture

The Top Bar should be persistent.

Recommended components:

Gold display

Inventory capacity display

Notification button

Save status display

Settings shortcut

Optional important currency display

The Top Bar should subscribe to relevant events.

Examples:

Gold Changed

Inventory Capacity Changed

Save Started

Save Completed

Save Failed

The Top Bar should not poll these values every frame.

The Top Bar should not display current activity details.

---

# Active Activity Bar Architecture

The Active Activity Bar should have display modes.

Suggested modes:

None

Profession

Crafting

Combat

Special Timer if added later

Recommended controller:

ActiveActivityBarController

Responsibilities:

Listen for active activity changes

Switch between display modes

Display progress

Display target or recipe

Display Health and Devotion during combat

Open the related main screen

Request Stop or Quit through gameplay systems

The Active Activity Bar must not directly stop systems by changing internal values.

It should call public gameplay commands.

Examples:

ProfessionSystem.StopActiveProfession()

CombatSystem.RequestQuitCombat()

CraftingSystem.CancelActiveCraft()

---

# Active Activity Bar Refresh

Do not rebuild the whole Active Activity Bar every frame.

Use events for major changes:

Activity Started

Activity Stopped

Combat Started

Combat Ended

Target Changed

Use a controlled timer or progress updater for:

Progress-bar fill

Time remaining

Health display

Devotion display

High-frequency values may update several times per second rather than every frame if smooth animation is not required.

---

# Standard Screen Prefab Structure

Recommended screen prefab hierarchy:

[SCREEN] ExampleScreen

├── [HEADER] ScreenHeader

├── [CONTENT] ScreenContentRoot

│ └── [SCROLL] MainScrollRect

│ ├── Viewport

│ │ └── Content

│ └── Scrollbar Vertical

└── [ACTIONS] FixedActionBar

The Header and Fixed Action Bar are optional.

The Main ScrollRect should fill the available space between fixed sections.

---

# Screen Header Prefab

Create a reusable ScreenHeader prefab.

Possible fields:

Icon

Title

Subtitle

Level

XP text

XP progress bar

Screen-specific resource

Statistics button

Help button

The Screen Header controller should allow unused sections to be disabled.

Do not create an entirely different header implementation for every profession.

---

# Main Screen Scrolling Rule

Most information-heavy screens should have one primary vertical ScrollRect.

The primary ScrollRect owns the screen's vertical movement.

Examples:

Combat Selection

Profession activity screen

Achievement list

Collection Log

Shop stock

Long settings page

Avoid adding a separate vertical ScrollRect to every subsection.

---

# Standard Vertical ScrollRect Setup

Recommended hierarchy:

[SCROLL] MainScrollRect

├── Viewport

│ └── Content

└── Scrollbar Vertical

ScrollRect settings:

Horizontal:

Disabled

Vertical:

Enabled

Movement Type:

Clamped or Elastic depending on visual style

Inertia:

Enabled

Scroll Sensitivity:

Test for mouse wheel comfort

Viewport:

Use RectMask2D

Content RectTransform:

Anchor Min:

0, 1

Anchor Max:

1, 1

Pivot:

0.5, 1

Content should stretch horizontally and grow downward.

Content may use:

Vertical Layout Group

Content Size Fitter

Vertical Fit:

Preferred Size

---

# RectMask2D Rule

Use RectMask2D for normal rectangular scrolling viewports.

Prefer it over Mask where no custom shaped mask is required.

Benefits include:

Lower UI cost

No additional mask material

Simpler setup

Use Mask only when the visible shape is not rectangular.

---

# Single Scroll Owner Rule

Avoid:

Vertical ScrollRect inside another vertical ScrollRect

Horizontal ScrollRect inside another horizontal ScrollRect

Allowed combinations:

Horizontal Region row inside vertical Combat Selection ScrollRect

Horizontal Activity Type row inside vertical Combat Selection ScrollRect

Scrollable Combat Log beside fixed Combat panels

Scrollable Inventory grid beside fixed Item Details

Scrollable Temporary Loot panel beside fixed Combat information

When the same-direction nested scrolling seems necessary, reconsider the layout first.

Possible alternatives:

Expandable sections

Tabs

Detail popup

Side panel

One larger parent ScrollRect

Virtualized list

---

# Nested Horizontal Scroll Handling

The Combat Selection screen may contain horizontal selector rows inside a vertical ScrollRect.

Unity can produce drag conflicts between nested ScrollRects.

Codex should implement a shared scroll-routing component.

Suggested component:

NestedScrollRouter

Behavior:

When horizontal drag movement is stronger than vertical movement:

Send drag to the child horizontal ScrollRect.

When vertical drag movement is stronger:

Forward drag to the parent vertical ScrollRect.

Mouse wheel behavior:

Default mouse wheel should scroll the parent vertical screen.

Shift plus mouse wheel may scroll the horizontal row if supported.

Touch drag should use direction locking after a small movement threshold.

Do not implement separate custom logic for every horizontal selector row.

---

# Scroll Position Preservation

Each major screen may store:

Normalized vertical position

Selected entry ID

Expanded section IDs

Horizontal selector positions where useful

State should normally be preserved while the application is open.

Some state may also be persisted between sessions.

Examples worth persisting:

Navigation scroll position

Last Combat selection

Inventory filter

Collection category

Examples usually not worth permanently saving:

Temporary tooltip position

Hover state

Open dropdown

---

# Scroll Position Restoration

Restore scroll position only after:

Dynamic entries have been generated.

Layout has rebuilt.

Content height is valid.

Possible restoration flow:

Generate entries.

Wait until end of frame or rebuild layout once.

Restore normalized position.

Do not restore scroll position before the Content RectTransform has its final height.

---

# Dynamic Content Expansion

When a section is revealed:

Enable or instantiate the section.

Bind its data.

Allow layout to rebuild.

Optionally scroll the new section into view.

Example:

Select Region.

Reveal Activity Type row.

Select Dungeon.

Reveal Dungeon Details panel.

Automatic scrolling should reveal the new section without forcing it to the exact top unless appropriate.

---

# Scroll To Element Utility

Create a reusable utility:

ScrollRectFocusUtility

Responsibilities:

Scroll a target RectTransform into view

Support vertical parent ScrollRect

Support optional padding

Avoid scrolling when the element is already fully visible

Support animated or immediate movement

This utility may be used for:

Combat selection rows

Validation errors

Newly unlocked activities

Deep-linked entries

Selected crafting recipes

Tracked Collection items

---

# Dynamic List Architecture

Large lists should be generated from data.

Examples:

Inventory items

Collection entries

Achievements

Recipes

Shop stock

Enemies

Regions

Companions

Profession activities

The system should not require one manually created GameObject per possible database entry inside the scene.

---

# List Controller Pattern

Each dynamic list should have a controller.

Suggested generic responsibilities:

Receive data list

Apply filter

Apply sorting

Create or reuse entry views

Bind data to entry views

Release unused views

Restore selection

Restore scroll position

Display empty state

Possible class concept:

```csharp
PooledListController<TData, TView>
```

Individual systems may use typed controllers.

Examples:

InventoryListController

AchievementListController

CollectionListController

EnemyListController

RecipeListController

---

# Object Pooling

Reusable list entries should use object pooling.

Recommended for:

Inventory slots

Collection entries

Achievement cards

Recipe cards

Shop entries

Enemy cards

Loot entries

Notification toasts

Pooling flow:

Request view from pool.

Set parent.

Enable view.

Bind data.

When no longer needed:

Unbind events.

Disable view.

Return to pool.

Do not destroy and recreate the entire list every time a filter changes.

---

# Virtualization

For lists with hundreds or thousands of entries, use virtualization.

Virtualization means:

Only visible entries and a small buffer are instantiated.

Entry positions represent the full list.

Views are reused as the player scrolls.

Recommended candidates:

Inventory

Collection Log

Achievements

Large Recipe lists

Large Shop lists

Virtualization may be introduced after the basic pooled list works.

However, the architecture should not depend on all entries permanently existing.

---

# List Refresh Types

Use different refresh levels.

Full Rebuild:

Data set changed significantly.

Filter changed.

Sort changed.

Category changed.

Partial Refresh:

One item quantity changed.

One achievement progressed.

One Collection entry was discovered.

One shop stock amount changed.

Visual Refresh:

Selection changed.

Hover state changed.

Notification badge changed.

Do not perform a Full Rebuild for every small value update.

---

# Selection State

Lists that support selection should track selection by stable ID.

Examples:

Selected Item ID

Selected Recipe ID

Selected Companion ID

Selected Enemy ID

Do not track selection only by list index.

Filtering or sorting may change indexes.

If the selected entry becomes hidden by filters:

Clear selection or show a clear state.

Do not silently select an unrelated entry.

---

# Reusable UI Prefabs

Required reusable prefab categories:

Buttons

Cards

Slots

Rows

Progress displays

Overlays

Lists

Recommended reusable prefabs:

PrimaryButton

SecondaryButton

DangerButton

IconButton

NavigationButton

TabButton

FilterButton

DropdownButton

ActivityCard

EnemyCard

DungeonCard

BossCard

RegionButton

RecipeCard

CompanionCard

AchievementCard

CollectionItemCard

ShopItemCard

ItemSlot

EquipmentSlot

RuneSlot

ConsumableSlot

RequirementRow

StatRow

RewardRow

SourceRow

ProgressBar

TimerDisplay

BuffIcon

AbilityButton

NotificationToast

Tooltip

ConfirmationPopup

SelectionPopup

EmptyState

LoadingIndicator

---

# Prefab Binding Pattern

Every reusable view should expose a Bind method.

Example:

```csharp
public void Bind(ItemViewData data, ItemViewContext context)
{
    iconImage.sprite = data.Icon;
    nameText.text = data.DisplayName;
    quantityText.text = data.QuantityText;
    lockIcon.SetActive(data.IsLocked);
}
```

A view should not search global databases itself unless that responsibility is explicitly assigned.

Prefer creating view data before binding.

---

# View Data Objects

Use view-specific data structures where helpful.

Examples:

ItemSlotViewData

ActivityCardViewData

EnemyCardViewData

AchievementCardViewData

CollectionEntryViewData

Benefits:

UI receives already prepared display information.

Formatting logic stays centralized.

Gameplay database objects are not modified by UI.

Views remain simple.

Testing becomes easier.

---

# Presenter or Controller Pattern

Recommended separation:

Gameplay System:

Owns gameplay state and rules.

Screen Controller or Presenter:

Reads gameplay state and prepares UI data.

View Components:

Display data and send interaction requests.

Example:

Inventory System:

Owns item stacks.

Inventory Screen Controller:

Requests filtered item data.

Item Slot View:

Displays one item and reports clicks.

Avoid placing all responsibilities inside one large MonoBehaviour.

---

# UI and Gameplay Separation

UI may request actions.

Gameplay systems decide whether actions are valid.

Example Crafting flow:

Player presses Craft.

↓

Crafting Screen calls Crafting System.

↓

Crafting System validates materials and requirements.

↓

Crafting System starts the craft or returns an error.

↓

UI displays the result.

The Crafting button should not directly remove items.

The Equipment UI should not directly modify equipment lists.

The Combat UI should not directly damage enemies.

---

# Command Result Pattern

Gameplay commands should return clear results.

Example:

```csharp
public readonly struct GameActionResult
{
    public bool Success { get; }
    public string ErrorCode { get; }
    public string Message { get; }
}
```

Possible commands:

StartProfessionActivity

StopProfessionActivity

StartCombat

QuitCombat

CraftRecipe

EquipItem

BuyItem

SellItem

StartCompanionRankUp

The UI should display player-friendly errors.

---

# UI Event Architecture

Gameplay systems should publish events when relevant data changes.

Examples:

GoldChanged

InventoryChanged

ItemQuantityChanged

ProfessionXPChanged

ProfessionLevelChanged

ActiveProfessionChanged

CombatStarted

CombatEnded

CombatHealthChanged

EnemyChanged

CompanionRankChanged

CraftingStarted

CraftingCompleted

AchievementProgressChanged

AchievementCompleted

CollectionItemDiscovered

ShopStockChanged

SaveStateChanged

UI screens subscribe only to events they need.

---

# Event Subscription Rules

Subscribe when:

Screen initializes for permanent global listeners

Screen opens for active-only listeners

Component enables

Unsubscribe when:

Screen closes if listener is active-only

Component disables

Component is destroyed

Do not leave event subscriptions attached to destroyed objects.

Avoid anonymous event handlers when they cannot be easily unsubscribed.

---

# Event Frequency

Do not publish expensive broad events excessively.

Prefer:

```csharp
ItemQuantityChanged(itemId, newQuantity)
```

over:

```csharp
InventoryChanged()
```

every frame

Prefer:

```csharp
AchievementProgressChanged(achievementId)
```

over rebuilding all achievements.

Broad refresh events may still exist for:

Load completion

Save restoration

Major database changes

Debug refresh

---

# Polling Rules

Avoid using Update for every UI component.

Acceptable high-frequency updates:

Progress bars

Attack timers

Countdown timers

Animated numeric values

Cursor-following tooltip positioning

These should use centralized or limited updating.

Static cards should update only when their data changes.

---

# UI Update Scheduler

A shared UI update scheduler may handle timed refreshes.

Suggested update groups:

Every Frame:

Only smooth visual animation when required

10 times per second:

Combat Health

Attack progress

Activity progress

1 time per second:

Long timers

Companion rank timers

Crafting remaining time

Farming timers

On Event:

Gold

Inventory quantity

XP

Unlocks

Collection discoveries

This reduces unnecessary work.

---

# Formatting Services

Create shared formatting utilities.

Examples:

NumberFormatter

TimeFormatter

PercentageFormatter

StatFormatter

RequirementFormatter

ItemQuantityFormatter

Do not implement different number formatting on every screen.

Example formats:

1,245,820

1.25M

42s

12m 34s

2h 14m

+10 Attack Damage

+10% Attack Damage

---

# Tooltip Architecture

Use one shared Tooltip Manager.

Recommended tooltip flow:

UI element sends Tooltip Request.

Tooltip Manager selects a tooltip prefab or layout.

Tooltip content is bound.

Tooltip is positioned.

Tooltip is clamped inside screen bounds.

Tooltip closes when the pointer leaves or request is cancelled.

Tooltip requests should support:

Title

Icon

Description

Stat rows

Requirements

Source information

Cooldown

Duration

Optional comparison

---

# Tooltip Trigger Component

Create reusable component:

UITooltipTrigger

Possible fields:

Tooltip Type

Static Tooltip ID

Dynamic Tooltip Provider

Hover Delay

Allow Touch Hold

The trigger should notify Tooltip Manager on:

Pointer Enter

Pointer Exit

Pointer Move if needed

Pointer Down for touch

Pointer Up

Do not create a separate Tooltip object for every UI element.

---

# Tooltip Positioning

Tooltips should be positioned using Canvas-space coordinates.

Use:

```csharp
RectTransformUtility.ScreenPointToLocalPointInRectangle
```

Tooltip positioning should:

Prefer appearing beside the target

Avoid covering the target

Flip left or right near screen edges

Flip above or below when necessary

Remain within Canvas bounds

Support variable tooltip size

---

# Popup Architecture

Use one shared Popup Manager.

Popup types may include:

Information

Confirmation

Selection

Reward

Warning

Error

The Popup Manager should:

Instantiate or reuse popup prefabs

Maintain popup stack

Block input when needed

Return results through callbacks, tasks, or events

Close popups safely

Support Escape behavior

---

# Modal Architecture

Modal windows should use:

A darkened input-blocking backdrop

CanvasGroup or raycast-blocking panel

One focused modal window

No interaction with screens underneath

Modal stack should remain limited.

Avoid opening multiple destructive confirmation modals over each other.

---

# Popup Result Pattern

Confirmation popup should return a result.

Example:

Confirmed

Cancelled

Closed

Do not place gameplay actions directly inside a generic Popup Manager.

The caller should decide what happens after confirmation.

Example:

Shop Screen requests confirmation.

Popup returns Confirmed.

Shop Screen calls Shop System.

---

# Notification Architecture

Use one Notification Manager.

Notification categories:

Low

Medium

High

Critical

Achievement

Collection Discovery

Level Up

Rare Reward

Error

Notification Manager responsibilities:

Queue notifications

Group repeated messages

Limit visible notifications

Display priority correctly

Store Notification History

Play optional UI sound

Return toast views to pool

---

# Notification Grouping

Notifications should be grouped by a key.

Example grouping key:

Item ID plus acquisition source

Instead of creating ten separate Oak Log messages:

Update one active notification to:

Oak Log +10

Grouping should use a short time window.

Important unique notifications should not be grouped into common messages.

---

# Dropdown Architecture

Dropdowns should appear inside Dropdown Layer.

Do not allow dropdown content to be clipped by a parent ScrollRect.

Possible approach:

When opened, move or spawn the dropdown menu under Dropdown Layer.

Position it relative to the source control.

Close it when:

Selection is made

Click occurs outside

Escape is pressed

Source screen closes

---

# Input Blocking

Each overlay should clearly define whether it blocks input.

Tooltips:

Normally do not block unrelated input

Dropdowns:

Block clicks behind their menu area

Notifications:

Normally do not block input except their own buttons

Popups:

May block part or all of the screen

Modals:

Block all underlying input

Loading Layer:

Blocks all input when game state is unsafe to interact with

Use CanvasGroup:

Interactable

Blocks Raycasts

Alpha

Avoid invisible panels that still block raycasts.

---

# EventSystem

The scene should contain one EventSystem.

Recommended input system:

Unity Input System

Input System UI Input Module

Do not create multiple active EventSystems.

The UI should support:

Mouse

Keyboard

Future controller navigation

Future touch input

---

# Focus Management

When a Modal opens:

Move focus to the default Modal control.

When it closes:

Return focus to the previous control if possible.

When a screen opens:

Optionally focus the first meaningful control for keyboard or controller use.

Mouse users should not be forced into visible focus outlines unless relevant.

---

# Persistent UI State

Persistent UI should retain:

Navigation selected state

Navigation collapse state

Top Bar values

Active Activity state

Notification badges

Persistent elements should not be rebuilt when main screens change.

---

# Screen State Data

Create a base state type:

UIScreenState

Possible screen-specific states:

CombatScreenState

InventoryScreenState

CollectionScreenState

ShopScreenState

ProfessionScreenState

Example Combat state:

Selected Region ID

Selected Activity Type ID

Selected Location ID

Selected Enemy ID

Selected Dungeon ID

Selected Boss ID

Selected Tower Floor

Vertical Scroll Position

Horizontal Region Scroll Position

Example Inventory state:

Selected Item ID

Search text

Filter IDs

Sort mode

Grid Scroll Position

---

# UI State Manager

The UI State Manager should:

Store temporary screen state

Restore screen state

Store persistent UI preferences

Clear invalid state when content is removed

Handle save version changes

Temporary states may remain memory-only.

Important preferences may be included in save data or a separate UI preferences file.

---

# Persistent UI Preferences

Possible saved UI settings:

Last opened screen

Navigation collapsed state

Navigation width

Expanded Navigation groups

UI scale

Text size

Tooltip delay

Notification duration

Number format

Combat Log filters

Combat Log text size

Reduced motion

Color accessibility option

Selected default sort modes

Do not mix large gameplay data into UI preference data.

---

# Combat Selection Technical Architecture

The Combat Selection screen uses a progressive hierarchical selection flow.

Selection levels:

Region

Activity Type

Location or Content

Enemy, Dungeon, Boss, or Tower Floor

Preparation Details

The complete selection interface should use one main vertical ScrollRect.

Horizontal rows may exist inside it.

---

# Combat Screen States

The Combat Screen should have two root states:

[STATE] CombatSelection

[STATE] ActiveCombat

Only one state should be visible at a time.

Combat Selection:

Visible before combat

Active Combat:

Visible while combat is running

When combat begins:

Save selection state.

Hide or collapse Combat Selection.

Show Active Combat.

When combat ends:

Hide Active Combat.

Restore Combat Selection.

Restore previous selection and scroll state.

---

# Combat Selection Hierarchy

Recommended prefab hierarchy:

[SCREEN] CombatScreen

├── [HEADER] CombatHeader

├── [STATE] CombatSelection

│ └── [SCROLL] CombatSelectionScroll

│ └── Viewport

│ └── Content

│ ├── [PANEL] SelectionBreadcrumb

│ ├── [SECTION] RegionSection

│ │ └── [SCROLL-H] RegionRow

│ ├── [SECTION] ActivityTypeSection

│ │ └── [SCROLL-H] ActivityTypeRow

│ ├── [SECTION] LocationSection

│ │ └── LocationContent

│ ├── [SECTION] TargetSection

│ │ └── TargetContent

│ └── [SECTION] PreparationDetails

└── [STATE] ActiveCombat

├── [PANEL] PlayerPanel

├── [PANEL] EnemyPanel

├── [PANEL] CompanionPanel

├── [PANEL] AbilityBar

├── [PANEL] Consumables

├── [PANEL] ActiveRuneSummary

├── [PANEL] BuffsAndDebuffs

├── [PANEL] TemporaryLoot

├── [PANEL] CombatLog

└── [PANEL] CombatControls

---

# Combat Selection Controller

Recommended controller:

CombatSelectionController

Responsibilities:

Load available Regions

Handle Region selection

Load available Activity Types

Handle Activity Type selection

Load Areas, Dungeons, Bosses, or Tower data

Handle target selection

Update breadcrumb

Clear invalid later selections

Display requirements

Open preparation details

Restore previous selection

Scroll newly revealed sections into view

The controller should not calculate combat damage.

---

# Selection Clearing Rules

Changing Region clears:

Activity Type

Location

Enemy

Dungeon

Boss

Tower Floor

Preparation details

Changing Activity Type clears:

Location

Enemy

Dungeon

Boss

Tower Floor

Preparation details

Changing Location clears:

Enemy

Preparation details

Changing Dungeon or Boss replaces existing preparation details.

The controller should centralize these rules.

Do not duplicate clearing logic inside every button.

---

# Region Row Generation

Region entries should come from Region Database.

For each available Region:

Request RegionButton from pool.

Bind Region view data.

Set selected state.

Set locked state.

Attach selection callback.

The Region row should not contain manually assigned buttons.

---

# Activity Type Row

Combat Activity Types may be represented by data definitions.

Examples:

areas

elite_areas

dungeons

bosses

tower

The available Activity Types may differ by Region.

The row should display only valid or intentionally locked Activity Types.

---

# Location and Target Layout

Normal Areas may use:

Horizontal location cards

Wrapped location card grid

Vertical location list

Enemy target cards may use:

Horizontal row

Wrapped grid

Compact list

The screen's main vertical ScrollRect remains the vertical scroll owner.

---

# Dungeon Details

Dungeon details should be one expandable or replaceable panel.

It may contain:

Requirements

Recommended level

Encounter count

Enemy list

Final boss

Special mechanics

Reward chest

Drop chances

Completion statistics

Start Dungeon button

Long Dungeon information should expand the main Content height.

Do not create a separate vertical ScrollRect for basic Dungeon details unless the panel becomes extremely large.

---

# Reward Chest Popup

Clicking the Dungeon Reward Chest may open an Information Popup.

Popup may display:

Chest icon

Chest name

Possible items

Quantities

Drop chances

Collection status

Current owned quantities

Source details

The popup may contain its own vertical ScrollRect because it is a separate overlay, not nested inside the Combat Selection scroll.

---

# Active Combat Layout Rules

During active combat, these should remain visible:

Player Health

Player Devotion

Enemy Health

Player attack progress

Enemy attack progress

Companion attack progress

Combat controls

Critical warnings

Other panels may scroll independently when appropriate:

Combat Log

Temporary Loot

Long buff list

Do not place the entire active combat interface inside one large vertical ScrollRect.

---

# Active Combat Update Strategy

High-frequency visual values:

Health

Devotion

Attack timers

Ability cooldowns

Buff durations

Update through:

Events for immediate value changes

Centralized progress refresh for smooth bars

Do not rebuild Player Panel or Enemy Panel each frame.

Update individual labels and fills.

---

# Combat Log Architecture

Combat Log should use:

Pooled log row prefab

Maximum retained entry count

Optional filters

Auto-scroll toggle

Clear action

Suggested maximum retained entries:

Several hundred, configurable

When maximum is exceeded:

Remove or recycle oldest entries.

Do not retain an unlimited number of active UI objects.

---

# Temporary Combat Loot Architecture

Temporary Loot should use a pooled list or grid.

Every loot entry should track:

Item ID

Quantity

Collection discovery status

Important status

Claimable status

Clicking one entry should request collection through Combat Loot System.

Pick All should request a batch claim.

The UI should not remove loot until the gameplay system confirms success.

---

# Profession Screen Technical Architecture

Profession screens should share a common base prefab where practical.

Possible base structure:

Profession Header

Active Profession Panel

Activity List

Selected Activity Details

Tool Slot

Companion Slot

Food Buff Slot

Relic Slot

Statistics section

Start or Stop controls

Different Professions may provide specialized modules.

Examples:

Farming Plot section

Thieving success chance section

Archaeology excavation section

Runecrafting Rune selection section

---

# Shared Profession Screen Controller

A shared controller may handle:

Profession level

Profession XP

Active activity

Tool

Companion

Food

Relic

Activity list generation

Locked requirements

Start and Stop commands

Profession-specific controllers may extend it.

Avoid copying the full screen controller for every Profession.

---

# Profession Activity Cards

Activity cards should be generated from Profession Activity Database.

Each card should bind:

Activity ID

Icon

Name

Required level

Action time

XP

Primary reward

Estimated hourly values

Locked state

Requirement state

Active state

Selection callback

Start callback if included

The card should not calculate its own estimated XP formula independently.

---

# Target-Based Profession UI

Target-based professions should update:

Target Health or Durability

Profession damage

Reward thresholds

Respawn timer

Active state

Threshold markers should be generated from target data.

Do not hardcode exactly four thresholds into shared UI.

The current design may commonly use:

75%

50%

25%

0%

But targets should support different configured thresholds.

---

# Crafting Screen Technical Architecture

Crafting screens should use:

Recipe list controller

Selected recipe presenter

Material requirement list

Craft progress display

Reserved item display

Craft command controls

Recipe list should be generated from Recipe Database.

Material rows should bind:

Item ID

Required quantity

Owned quantity

Reserved quantity

Available quantity

Missing quantity

Status

Clicking a material should create a deep-link request.

---

# Inventory Screen Technical Architecture

Recommended layout:

Fixed Header

Fixed search, filters, and sort controls

Scrollable Inventory grid

Fixed Item Details panel on wide layouts

The Inventory grid should use:

Pooling

Virtualization when item count becomes large

Selection by Item ID

Partial quantity refresh

The Item Details panel updates only when:

Selection changes

Selected item quantity changes

Selected item metadata changes

---

# Equipment Screen Technical Architecture

Equipment slots should be reusable EquipmentSlotView prefabs.

Each slot binds:

Slot type

Equipped Item ID

Icon

Empty state

Disabled state

Two-handed conflict state

Click callback

Equipment comparison should use a prepared comparison model.

The UI should not calculate final combat stats independently.

It should request stat comparison from Equipment or Stat System.

---

# Companion Screen Technical Architecture

Companion list should use pooled Companion Cards.

Companion details should bind:

Companion ID

Rank

Category

Assignment

Bonuses

Combat stats

Profession support

Rank-up requirements

Rank-up timer

Because companions currently have no Health:

Do not create Health UI fields.

Do not create death-state UI.

Do not create targeting UI.

---

# Shop Screen Technical Architecture

The Shop screen should use:

Shop stock list controller

Sell list controller

Selected item details

Quantity selector

Purchase preview

Sale preview

Transaction command result

The UI should wait for Shop System confirmation before:

Removing currency visually

Removing sold items visually

Adding purchased items visually

Normal system events should update those values after a successful transaction.

---

# Achievement Screen Technical Architecture

Achievement list should support:

Category filter

Completion filter

Search

Sort

Tracked state

Claimable state

Achievement cards should update partially when one achievement changes.

Do not rebuild the whole list every time one progress value increases.

---

# Collection Log Technical Architecture

The Collection Log is primarily item-based.

Standard entries may be generated from Item Database plus Collection data.

Each entry should display:

Discovery state

Current quantity

Lifetime obtained quantity

Category

Source information

Selection state

Large Collection lists should use virtualization or strong pooling.

The Collection controller should not create enemy, boss, or dungeon entries as main Collection entries.

Those systems are displayed only as item sources.

---

# Deep-Link Architecture

Create a UIDeepLinkService.

Deep-link examples:

Item source to Profession activity

Item source to Enemy

Recipe ingredient to Item details

Collection entry to Boss source

Equipment item to upgrade recipe

Suggested deep-link request:

Target Screen ID

Target Tab ID

Target Entry ID

Target Parent ID

Return Context

Example:

Target Screen:

combat

Target Region:

greenvale

Target Activity Type:

areas

Target Location:

forest

Target Entry:

grey_wolf

The destination screen should validate the request.

If a target is locked or missing:

Open the nearest valid screen state.

Display a clear message.

Do not crash.

---

# Responsive Layout Architecture

The initial layout targets PC landscape.

Use anchored layouts instead of fixed absolute positioning.

Wide layout may use:

List on left

Details on right

Narrow layout may use:

List screen

Details overlay

Or:

List tab

Details tab

Responsive switching may be based on:

Canvas width

Aspect ratio

Configured breakpoint

Do not use screen resolution alone without considering Canvas scaling.

---

# Layout Breakpoints

Possible initial breakpoint logic:

Wide:

Details panel shown beside list

Medium:

Narrower Navigation and details panel

Small landscape:

Collapsed Navigation and overlay details

Future portrait mobile:

Dedicated mobile screen prefab or layout configuration

Exact breakpoint values should be tested later.

---

# Future Mobile Architecture

Gameplay systems should remain independent from the UI layout.

Future mobile may use:

Different persistent shell

Navigation drawer

Bottom Navigation

Full-screen details

Larger touch targets

The same screen controllers or presenters may feed different view prefabs.

Avoid gameplay code that assumes a left Navigation panel exists.

---

# UI Scale Implementation

UI scale should modify a controlled root or Canvas Scaler setting.

Do not individually resize hundreds of objects.

Possible method:

Adjust Canvas Scaler reference scale factor or root UI scale multiplier.

After changing UI scale:

Rebuild affected layouts once.

Validate navigation width.

Validate tooltips.

Validate popup boundaries.

Validate screen scroll areas.

---

# Safe Area Architecture

Future mobile Safe Area should be applied at the shell level.

Recommended hierarchy:

MainCanvas

└── SafeAreaRoot

└── Persistent and Screen layout

A SafeAreaController may update offsets based on Screen.safeArea.

Desktop should use the full screen.

---

# Accessibility Implementation

Accessibility settings should control shared UI services.

Examples:

Reduced Motion:

Animation service reduces durations or skips transitions.

Exact Numbers:

NumberFormatter changes output mode.

Tooltip Delay:

Tooltip Manager changes hover delay.

Longer Notifications:

Notification Manager changes duration.

High Contrast:

Theme Manager selects accessible style values.

Color information should also use icons or labels.

---

# UI Animation Architecture

Create a shared animation service or reusable transition components.

Possible transitions:

Fade

Short slide

Scale reveal

Progress completion pulse

Do not hardcode long custom animations into each screen controller.

Animation rules:

Routine screen opening should remain fast.

Input should not be blocked longer than necessary.

Reduced Motion should disable or shorten movement.

Gameplay actions should not wait for cosmetic animation to finish.

---

# Screen Transition Rules

Recommended default:

Fade out old screen quickly.

Disable old screen.

Enable new screen.

Restore state and refresh.

Fade in new screen.

Total duration should remain short.

Screen Manager should prevent overlapping transition requests.

Rapid Navigation clicks should:

Queue only the latest valid request

Or ignore input until the short transition finishes

Do not instantiate multiple copies of the same screen.

---

# UI Audio Architecture

Use one UIAudioManager.

Possible methods:

PlayButtonClick

PlayConfirm

PlayCancel

PlayError

PlayPurchase

PlayCraftComplete

PlayAchievement

PlayRareReward

UI components should request semantic sounds.

Do not directly reference unique AudioClips from every button unless necessary.

UI sound volume should be controlled separately.

---

# Error Handling

UI command failures should display clear messages.

Examples:

Not enough Gold

Inventory full

Missing Profession level

Item reserved

Combat setup locked

Rune cannot be changed during combat

Error handling flow:

Gameplay System returns failure result.

UI maps error code to localized player-facing text.

Notification or inline error is displayed.

No partial UI state change remains.

Technical exception details should be logged separately.

---

# Loading State Architecture

Normal screen changes should not require Loading screens.

Loading Layer may be used for:

Initial save load

Database initialization

Major content loading

Save migration

Cloud save synchronization in future

Loading Layer should:

Block input

Display progress when known

Display meaningful status

Handle failure safely

---

# Empty State Architecture

Every large list controller should support an Empty State prefab.

Possible messages:

No items match your filters.

No activities are unlocked.

No recipes are available.

No Temporary Combat Loot.

No achievements match this category.

Empty states should be controlled by the list controller.

Do not leave an unexplained blank area.

---

# UI Performance Rules

Avoid:

One Update method on every card

Rebuilding full lists for one quantity change

Unlimited Combat Log objects

Unlimited Notification objects

Excessive nested Layout Groups

Multiple unnecessary Canvases

Repeated database lookups every frame

Repeated GetComponent calls during updates

Repeated scene searches

Prefer:

Cached references

Event-driven updates

Pooling

Virtualization

Partial refresh

Shared formatters

Prepared view data

Centralized timed updates

---

# TextMeshPro Rules

Use TextMeshPro for normal visible text.

Create shared font assets and material presets inside the project.

Do not use legacy UnityEngine.UI.Text.

Text fields should support:

Auto sizing only when carefully bounded

Overflow settings appropriate to the field

Ellipsis for compact cards when useful

Wrapping for descriptions

Consistent alignment

Do not rely on extreme auto-size ranges to make layouts fit.

Fix the layout instead.

---

# Sprite and Icon Rules

UI should reference sprites through data definitions.

Examples:

Item icon from Item Database

Profession icon from Profession Database

Enemy icon from Enemy Database

Screen icon from Screen Definition

Do not hardcode icon asset paths inside screen scripts.

Missing icon behavior:

Display a safe placeholder icon.

Log a warning.

Do not crash the UI.

---

# Localization Preparation

Even if the game initially uses one language, UI text architecture should avoid combining sentences through fragile string concatenation.

Prefer formatted templates.

Example:

Requires {0} Level {1}

Store stable display strings separately from IDs.

UI layouts should allow moderate text expansion.

Buttons should not depend on one exact English label width.

---

# Editor Validation

Create validation tools or OnValidate checks where useful.

Validate:

Duplicate Screen IDs

Duplicate Navigation IDs

Missing Screen prefabs

Missing icons

Missing required UI references

Invalid Navigation target Screen IDs

Invalid deep-link targets

Missing Item IDs

Missing database references

Invalid sorting values

Validation should report clear Unity Console messages.

---

# Debug UI Tools

Development-only UI tools may include:

Open Screen by ID

Open Deep Link

Unlock every screen

Rebuild Navigation

Refresh active screen

Simulate Inventory Full

Simulate Rare Item drop

Simulate Collection discovery

Simulate Achievement completion

Start test combat

Stop test combat

Change resolution

Change UI scale

Display current UI state

Show active screen stack

Show pooled object counts

Show UI refresh counts

Debug UI should be disabled or excluded from release builds.

---

# UI Logging

Use structured development logs.

Examples:

[UI][ScreenManager] Opened screen: combat

[UI][Navigation] Invalid target screen: tower

[UI][Tooltip] Missing provider for item: iron_ore

[UI][List] Rebuilt Inventory list: 425 entries

Avoid logging every progress-bar update.

Verbose logging should be configurable.

---

# Testing Strategy

UI should be tested through:

Edit Mode tests

Play Mode tests

Manual resolution testing

Large data stress testing

Keyboard and mouse testing

Offline result testing

---

# Required Screen Manager Tests

Test:

Default screen opens.

Opening a screen closes the previous screen.

Only one main screen remains active.

Opening the same screen does not duplicate it.

Back returns to previous screen.

Invalid Screen ID fails safely.

Deep link opens the correct entry.

Screen state restores correctly.

Navigation highlight updates.

---

# Required Scroll Tests

Test:

Main screen scroll works with mouse wheel.

Scrollbar works.

Horizontal selector row works.

Vertical drag inside horizontal row reaches parent when appropriate.

Scroll position remains after leaving and returning.

Dynamic sections expand Content height.

Deep-linked entry scrolls into view.

Changing filter resets or preserves position correctly.

No vertical nested ScrollRect conflict exists.

---

# Required Dynamic List Tests

Test with:

0 entries

1 entry

50 entries

500 entries

Several thousand entries where relevant

Verify:

No duplicate cards

Selection remains correct

Pooling releases unused entries

Sort works

Filter works

Search works

Quantity updates do not rebuild everything

Empty state appears correctly

---

# Required Resolution Tests

Test:

1920 × 1080

2560 × 1440

Ultrawide

Reduced-height landscape window

Different UI Scale options

Verify:

Text remains readable

Buttons remain clickable

Navigation remains accessible

No important panels overlap

Tooltips remain inside screen

Popups remain centered and visible

Scroll areas remain usable

---

# Required Combat UI Tests

Test:

Region selection reveals Activity Type.

Activity Type reveals correct content.

Changing Region clears later selections.

Area selection reveals locations.

Location selection reveals enemies.

Dungeon selection opens Dungeon details.

Reward Chest opens reward popup.

Combat starts from selected target.

Combat Selection hides or collapses.

Active Combat displays correct information.

Runes cannot be changed during combat.

Combat ending restores previous selection.

Temporary Loot claims correctly.

Combat Log pooling works.

---

# Required Profession UI Tests

Test:

Profession Activity list is database-driven.

Locked activities show requirements.

Starting an activity updates Active Activity Bar.

Changing profession stops previous active profession according to gameplay rules.

Target durability updates.

Threshold markers update.

Respawn state displays.

Inventory Full stops or pauses the activity and shows feedback.

Offline results update the screen correctly.

---

# Coding Standards

UI classes should have one clear responsibility.

Avoid extremely large screen scripts.

Recommended separation:

Screen Controller

List Controller

Details Presenter

Individual View Components

Popup Controller

State Model

Use clear names.

Examples:

CombatScreenController

CombatSelectionController

EnemyCardView

InventoryListController

ItemDetailsPresenter

Avoid names such as:

Manager2

NewUI

TempScript

ButtonHandlerFinal

---

# Serialized Field Rules

Use:

```csharp
[SerializeField] private Button startButton;
```

Prefer private serialized fields over public mutable fields.

Validate required references.

Do not call GetComponent repeatedly when the component can be cached.

Use descriptive field names.

---

# UnityEvent Rules

Unity button onClick may call a small view method.

Avoid building the entire application flow through Inspector UnityEvents.

Recommended:

Button calls local handler.

Local handler sends request to controller or system.

Example:

```csharp
startButton.onClick.AddListener(OnStartPressed);
```

Do not manually wire hundreds of content buttons in the Inspector.

Dynamic entries should bind callbacks through code.

---

# Cancellation and Destruction

When screens or views are disabled:

Cancel running UI-only asynchronous operations.

Stop screen-specific coroutines.

Unsubscribe events.

Hide tooltips owned by the screen.

Close dropdowns owned by the screen.

Return pooled elements when rebuilding lists.

Gameplay operations should continue independently.

Closing the Crafting screen should not cancel crafting.

---

# Save and Load Rules

UI preferences may be saved separately from core gameplay state.

Save:

Navigation state

UI scale

Settings

Last screen

Selected Combat path if desired

Filters and sort preferences

Tracked items and achievements

Do not save:

Temporary hover state

Animation progress

Open tooltip

Pointer position

Half-open dropdown

On load:

Validate saved Screen IDs and entry IDs.

Ignore or replace missing data safely.

---

# Technical Acceptance Criteria

The UI architecture is acceptable when:

One main gameplay scene contains the persistent UI shell.

Top Bar and Active Activity Bar are separate.

Left Navigation is data-driven and vertically scrollable.

Only one main screen is active at a time.

Screens can be opened through stable IDs.

Screens support Back navigation.

Screens support deep links.

Most information-heavy screens use one primary vertical ScrollRect.

Horizontal selectors work inside vertical screens.

Dynamic lists are generated from databases.

Large lists use pooling.

UI scripts do not contain core gameplay calculations.

Gameplay continues while other screens are open.

Popups use a shared Popup Manager.

Tooltips use a shared Tooltip Manager.

Notifications use a shared Notification Manager.

Screen state is preserved where useful.

The Combat selection hierarchy works correctly.

Active Combat keeps critical information visible.

UI scales correctly at supported resolutions.

Adding new items, enemies, activities, recipes, and Regions does not require rewriting shared UI systems.

---

# Codex Implementation Order

Codex should build the UI architecture in phases.

Do not attempt to build every final screen simultaneously.

---

# Phase 1: Persistent UI Shell

Create:

Main Canvas

Canvas Scaler

Top Bar

Main Body

Left Navigation container

Content Column

Active Activity Bar

Screen Container

Overlay layers

Popup Layer

Modal Layer

Loading Layer

EventSystem

Acceptance:

The layout fills 1920 × 1080 correctly.

Persistent elements remain visible.

Screen Container resizes correctly.

---

# Phase 2: Core UI Managers

Create:

UIBootstrapper

ScreenManager

NavigationManager

UIStateManager

PopupManager

TooltipManager

NotificationManager

UIInputManager

Acceptance:

A test screen can be opened by Screen ID.

Opening another screen closes the first.

Back navigation works.

A test popup, tooltip, and notification can display.

---

# Phase 3: Screen Definitions and Navigation Data

Create:

ScreenDefinition

UIScreenCatalog

NavigationEntryDefinition

NavigationGroupDefinition

Navigation data assets

Navigation group prefab

Navigation entry prefab

Acceptance:

Navigation is generated from data.

Navigation opens target screens.

Selected state updates.

Locked entries display correctly.

Navigation can collapse.

---

# Phase 4: Standard Screen Template

Create:

Base UIScreenController

Screen Header prefab

Main Screen Scroll template

Fixed Action Bar template

Empty State prefab

Loading State prefab

Acceptance:

A test screen supports:

Header

Scrollable Content

Fixed Action Bar

State preservation

Deep linking

---

# Phase 5: Dynamic List System

Create:

Reusable pool

Pooled list controller

Selection model

Search support

Filter support

Sort support

Partial refresh support

Acceptance:

Test list works with:

0 items

50 items

500 items

No duplicate views appear.

Selection remains correct after sorting.

---

# Phase 6: Shared UI Components

Create:

Buttons

Tabs

Item Slot

Activity Card

Enemy Card

Region Button

Requirement Row

Stat Row

Progress Bar

Timer Display

Tooltip Trigger

Notification Toast

Confirmation Popup

Acceptance:

Components support normal, selected, disabled, and locked states.

Components bind data without hardcoded gameplay logic.

---

# Phase 7: Profession Screen Prototype

Implement Woodcutting as the first Profession screen.

Include:

Profession Header

Activity list

Target panel

Durability bar

Reward thresholds

Tool display

Companion display

Start and Stop controls

Active Activity Bar integration

Acceptance:

Woodcutting continues while another screen is open.

Locked trees show requirements.

Target durability updates.

Reward thresholds display.

Inventory Full feedback works.

---

# Phase 8: Combat Selection Prototype

Implement:

Region row

Activity Type row

Location row

Enemy row

Breadcrumb

Enemy details

Start Combat

Main vertical ScrollRect

Nested horizontal scroll handling

Acceptance:

Changing earlier selections clears later selections.

New sections appear correctly.

The screen scrolls to revealed sections.

Selection state is restored.

---

# Phase 9: Active Combat Prototype

Implement:

Player panel

Enemy panel

Companion panel

Attack progress bars

Health and Devotion

Abilities

Consumables

Active Rune summary

Temporary Loot

Combat Log

Combat controls

Acceptance:

Critical information remains visible without scrolling.

Companion has no Health UI.

Temporary Loot can be claimed.

Combat Log uses pooling.

Rune loadout cannot be changed during combat.

---

# Phase 10: Inventory and Equipment

Implement:

Inventory pooled grid

Search

Filters

Sorting

Item details

Equipment slots

Comparison panel

Two-handed restrictions

Deep links

Acceptance:

Large Inventory remains responsive.

Quantity changes update only affected entries.

Equipment comparison uses gameplay stat results.

---

# Phase 11: Remaining Screens

Implement:

Companions

Crafting

Shop

Achievements

Collection Log

Settings

Dashboard if retained

Each screen should reuse the existing architecture.

Do not create a new list, popup, or screen-management system for each screen.

---

# Phase 12: Responsive and Accessibility Pass

Implement:

UI Scale

Reduced Motion

Number Format

Tooltip Delay

Notification Duration

Resolution testing

Collapsed Navigation

Narrow-layout behavior

Acceptance:

The interface remains usable on all supported PC resolutions.

Accessibility settings affect shared systems consistently.

---

# Phase 13: Performance and Cleanup

Profile:

Canvas rebuilds

Layout rebuilds

List creation

Combat Log

Notifications

Screen opening

Large Inventory

Collection Log

Remove:

Unnecessary Update methods

Repeated GetComponent calls

Repeated database searches

Unbounded UI object growth

Acceptance:

Screen switching feels immediate.

Large lists scroll smoothly.

Long Combat sessions do not continually increase UI object count.

---

# Rules Codex Must Not Break

Do not use UI Toolkit for the main gameplay UI.

Do not load a separate scene for every screen.

Do not place core gameplay calculations inside UI scripts.

Do not manually create one scene object for every item, enemy, or activity.

Do not allow multiple main screens to remain active.

Do not duplicate current activity information in both Top Bar and Active Activity Bar.

Do not use nested vertical ScrollRects without a strong reason.

Do not put the whole Active Combat interface inside a vertical ScrollRect.

Do not show Companion Health.

Do not allow Runes to be changed during active Combat.

Do not rebuild every large list after every small value change.

Do not use GameObject.Find for routine UI behavior.

Do not leave invisible panels blocking raycasts.

Do not silently ignore failed actions.

Do not destroy player progress because a UI action failed.

---

# Final Checklist

## Canvas

✓ Main Canvas uses Canvas / uGUI.

✓ Text uses TextMeshPro.

✓ Canvas Scaler uses 1920 × 1080 reference resolution.

✓ Persistent layers remain outside Screen Container.

✓ Overlay order is correct.

## Screens

✓ Screens use stable Screen IDs.

✓ Screen prefabs are loaded or instantiated through Screen Manager.

✓ Only one main screen is active.

✓ Screens implement a consistent lifecycle.

✓ Back navigation works.

✓ Deep links work.

✓ Useful state is preserved.

## Navigation

✓ Navigation is generated from data.

✓ Navigation groups are collapsible.

✓ Navigation is vertically scrollable.

✓ Selected entry is highlighted.

✓ Locked entries show requirements.

✓ Collapse state is saved.

## Scrolling

✓ Each information-heavy screen has one primary vertical ScrollRect.

✓ Horizontal selector rows route input correctly.

✓ Same-direction nested ScrollRects are avoided.

✓ Scroll positions restore correctly.

✓ Dynamic sections update content height.

✓ Newly revealed content can be focused.

## Lists

✓ Lists are data-driven.

✓ Repeated views use pooling.

✓ Large lists can support virtualization.

✓ Selection uses stable IDs.

✓ Partial updates are supported.

✓ Empty states are displayed.

## Persistent UI

✓ Top Bar displays account-wide information.

✓ Active Activity Bar displays the current activity.

✓ Activity Bar changes mode during Combat.

✓ Persistent UI does not rebuild during every screen change.

## Combat

✓ Combat selection uses Region → Type → Content → Target.

✓ Changing an earlier selection clears later selections.

✓ Breadcrumb works.

✓ Active Combat keeps critical information visible.

✓ Combat Log is pooled.

✓ Temporary Loot is system-controlled.

✓ Companion has no Health UI.

✓ Active Runes are shown but cannot be changed during Combat.

## Architecture

✓ UI and gameplay logic are separated.

✓ UI actions call gameplay systems.

✓ Gameplay systems return success or failure.

✓ UI updates through events.

✓ References are cached.

✓ Runtime scene searches are avoided.

✓ Shared managers control popups, tooltips, and notifications.

## Performance

✓ No unlimited UI object growth.

✓ No Update method on every card.

✓ No complete list rebuild for one changed value.

✓ Layout rebuilds are controlled.

✓ Screens open quickly.

✓ Large lists remain responsive.

## Expansion

✓ New screens can be added through Screen Definitions.

✓ New Navigation entries can be added through data.

✓ New items automatically appear in data-driven lists.

✓ New Regions and enemies automatically populate Combat selection.

✓ New Profession activities automatically populate Profession screens.

✓ Future mobile layouts can reuse the same gameplay systems.
