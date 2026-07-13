# 21 Unity UI Hierarchy Authoring and Editable UI Refactor

Version: 1.0  
Status: Required Refactor  
Engine: Unity  
UI System: Canvas / uGUI  
Text System: TextMeshPro  
Primary Platform: PC Landscape  
Reference Resolution: 1920 × 1080

---

# Purpose

This document defines how the existing Unity UI must be refactored so that the majority of the interface exists visibly inside the Unity Hierarchy and can be edited through the Inspector.

The current implementation creates too much permanent UI through scripts.

This causes problems because:

- Navigation buttons are not visible in the Hierarchy before Play Mode.
- Major panels cannot be repositioned easily.
- Text, images, spacing, anchors, and colors are difficult to edit.
- The interface is difficult to understand visually.
- Small UI changes require modifying code.
- Runtime-created objects are harder for a non-programmer to manage.
- Reference images are harder to reproduce manually.
- Existing UI layout is hidden until the game runs.

The new approach must make Unity function as a visual UI editor.

Scripts should control:

- Behavior
- Data
- Screen switching
- Button actions
- Dynamic list contents
- Gameplay updates
- Progress values
- Runtime state

Scripts should not construct the permanent interface.

---

# Main Goal

When the game is not running, the user must be able to open the primary gameplay scene and see the full UI structure in the Hierarchy.

The user must be able to select and edit:

- Top Bar
- Gold display
- Inventory display
- Left Navigation
- Navigation group headers
- Combat Navigation button
- Inventory Navigation button
- Equipment Navigation button
- Woodcutting Navigation button
- Active Activity Bar
- Woodcutting screen
- Combat screen
- Inventory screen
- Equipment screen
- Screen headers
- Main panels
- Scroll Views
- Fixed buttons
- Detail panels
- Progress bars
- Popup roots
- Tooltip roots
- Notification roots

The user should be able to change:

- Width
- Height
- Position
- Anchors
- Padding
- Spacing
- Text size
- Font
- Text alignment
- Colors
- Images
- Borders
- Button transitions
- Scroll View size
- Panel proportions

These changes must remain after entering and leaving Play Mode.

---

# Source Priority

This document overrides earlier UI architecture rules where they conflict with editable Hierarchy authoring.

Priority for UI implementation:

1. This document
2. 20 Prototype Vertical Slice 01
3. 17 Unity UI Technical Architecture
4. 16 UI and UX Framework
5. 18 UI Visual Style Guide
6. Approved UI reference images
7. Existing implementation

The previous recommendation to instantiate major screens when first opened is replaced for the current project.

For the vertical slice:

- Main screens should already exist under `ScreenContainer`.
- Navigation buttons should already exist under `LeftNavigation`.
- Persistent UI should already exist under `MainCanvas`.
- Major screen panels should already exist before Play Mode.

Scripts may enable and disable these objects.

Scripts should not create them from nothing at runtime.

---

# Core UI Authoring Principle

Use a hybrid authoring model.

## Hierarchy-Authored UI

Permanent and major UI must be created manually in the scene or placed as visible prefab instances.

Examples:

- Main Canvas
- Persistent shell
- Top Bar
- Left Navigation
- Navigation buttons
- Active Activity Bar
- Screen Container
- Main screen roots
- Screen headers
- Fixed panels
- Main Scroll Views
- Detail panels
- Permanent action buttons
- Equipment slot layout
- Combat player panel
- Combat enemy panel
- Combat companion placeholder
- Ability bar
- Consumable panel
- Combat controls
- Popup layers
- Tooltip layer
- Notification layer

## Runtime-Generated UI

Only repeated or database-sized content may be created at runtime.

Examples:

- Inventory item slots
- Woodcutting tree cards
- Enemy cards
- Region cards
- Loot entries
- Combat Log rows
- Notification toasts
- Requirement rows
- Stat rows
- Reward rows
- Future recipe cards
- Future achievement cards
- Future collection entries

Runtime-generated entries must use editable prefabs.

The prefab itself must be editable through the Project window.

---

# Static Versus Dynamic UI Rule

Use this decision:

If the element always exists as part of the screen layout, it belongs in the Hierarchy.

If the number of elements depends on game data, it may be generated from a prefab.

Examples:

| UI Element | Authoring Method |
|---|---|
| Top Bar | Hierarchy |
| Gold icon and text | Hierarchy |
| Inventory capacity text | Hierarchy |
| Left Navigation panel | Hierarchy |
| Combat Navigation button | Hierarchy |
| Inventory Navigation button | Hierarchy |
| Equipment Navigation button | Hierarchy |
| Woodcutting Navigation button | Hierarchy |
| Active Activity Bar | Hierarchy |
| Woodcutting screen root | Hierarchy |
| Combat screen root | Hierarchy |
| Inventory screen root | Hierarchy |
| Equipment screen root | Hierarchy |
| Screen headers | Hierarchy |
| Scroll View containers | Hierarchy |
| Start and Stop buttons | Hierarchy |
| Combat Quit button | Hierarchy |
| Pick All button | Hierarchy |
| Equipment slots | Hierarchy |
| Tree cards | Runtime prefab instances |
| Enemy cards | Runtime prefab instances |
| Inventory item slots | Runtime prefab instances |
| Combat Log rows | Runtime prefab instances |
| Temporary Loot entries | Runtime prefab instances |
| Notifications | Runtime prefab instances |

---

# Editing Experience Requirement

The scene should remain understandable without entering Play Mode.

When viewing the Hierarchy, the user should be able to understand:

- Which objects belong to persistent UI
- Which objects belong to each screen
- Which panels are scrollable
- Which panels are fixed
- Which buttons perform permanent actions
- Which objects are runtime content containers
- Which objects are popup or overlay layers

Objects should not use unclear names such as:

- Panel
- Panel 1
- Button
- Button 2
- New Game Object
- Image
- Image 4
- Text
- TMP Text
- Content 2

Use descriptive names.

---

# Required Scene Hierarchy

The primary gameplay scene should contain the following structure.

```text
[BOOTSTRAP] GameBootstrap

[SYSTEMS] GameSystems

[UI] UI System
├── [UI] MainCanvas
│   ├── [PERSISTENT] TopBar
│   │   ├── [LAYOUT] LeftSection
│   │   │   └── [TEXT] GameTitle
│   │   ├── [LAYOUT] CenterSection
│   │   └── [LAYOUT] RightSection
│   │       ├── [DISPLAY] GoldDisplay
│   │       │   ├── [ICON] GoldIcon
│   │       │   └── [TEXT] GoldValue
│   │       ├── [DISPLAY] InventoryCapacityDisplay
│   │       │   ├── [ICON] InventoryIcon
│   │       │   └── [TEXT] InventoryCapacityValue
│   │       ├── [DISPLAY] SaveStatusDisplay
│   │       │   ├── [ICON] SaveStatusIcon
│   │       │   └── [TEXT] SaveStatusText
│   │       ├── [BUTTON] NotificationsButton
│   │       └── [BUTTON] SettingsButton
│   │
│   ├── [LAYOUT] MainBody
│   │   ├── [PERSISTENT] LeftNavigation
│   │   │   ├── [HEADER] NavigationHeader
│   │   │   │   ├── [TEXT] NavigationTitle
│   │   │   │   └── [BUTTON] CollapseNavigationButton
│   │   │   │
│   │   │   └── [SCROLL] NavigationScrollView
│   │   │       ├── Viewport
│   │   │       │   └── Content
│   │   │       │       ├── [NAV GROUP] MainGroup
│   │   │       │       │   ├── [HEADER] MainGroupHeader
│   │   │       │       │   ├── [NAV BUTTON] CombatButton
│   │   │       │       │   ├── [NAV BUTTON] InventoryButton
│   │   │       │       │   └── [NAV BUTTON] EquipmentButton
│   │   │       │       │
│   │   │       │       ├── [NAV GROUP] ProfessionsGroup
│   │   │       │       │   ├── [HEADER] ProfessionsGroupHeader
│   │   │       │       │   └── [NAV BUTTON] WoodcuttingButton
│   │   │       │       │
│   │   │       │       └── [NAV GROUP] AccountGroup
│   │   │       │           ├── [HEADER] AccountGroupHeader
│   │   │       │           └── [NAV BUTTON] SettingsNavigationButton
│   │   │       │
│   │   │       └── VerticalScrollbar
│   │   │
│   │   └── [LAYOUT] ContentColumn
│   │       ├── [PERSISTENT] ActiveActivityBar
│   │       │   ├── [STATE] NoActivityState
│   │       │   │   ├── [ICON] NoActivityIcon
│   │       │   │   └── [TEXT] NoActivityText
│   │       │   │
│   │       │   ├── [STATE] ProfessionActivityState
│   │       │   │   ├── [ICON] ProfessionIcon
│   │       │   │   ├── [TEXT] ProfessionNameText
│   │       │   │   ├── [TEXT] ProfessionTargetText
│   │       │   │   ├── [BAR] ProfessionProgressBar
│   │       │   │   ├── [BUTTON] OpenProfessionButton
│   │       │   │   └── [BUTTON] StopProfessionButton
│   │       │   │
│   │       │   └── [STATE] CombatActivityState
│   │       │       ├── [ICON] CombatIcon
│   │       │       ├── [TEXT] CombatDisciplineText
│   │       │       ├── [TEXT] CombatEnemyText
│   │       │       ├── [BAR] CompactPlayerHealthBar
│   │       │       ├── [BAR] CompactDevotionBar
│   │       │       ├── [BAR] CompactEnemyHealthBar
│   │       │       ├── [BUTTON] OpenCombatButton
│   │       │       └── [BUTTON] QuitCombatButton
│   │       │
│   │       └── [SCREENS] ScreenContainer
│   │           ├── [SCREEN] WoodcuttingScreen
│   │           ├── [SCREEN] CombatScreen
│   │           ├── [SCREEN] InventoryScreen
│   │           ├── [SCREEN] EquipmentScreen
│   │           └── [SCREEN] SettingsScreen
│   │
│   ├── [OVERLAY] DropdownLayer
│   ├── [OVERLAY] TooltipLayer
│   │   └── [TOOLTIP] SharedTooltip
│   ├── [OVERLAY] NotificationLayer
│   │   └── [CONTAINER] NotificationContainer
│   ├── [OVERLAY] PopupLayer
│   │   └── [POPUP] SharedPopupRoot
│   ├── [OVERLAY] ModalLayer
│   │   ├── [IMAGE] ModalBlocker
│   │   └── [CONTAINER] ModalContainer
│   └── [OVERLAY] LoadingLayer
│       ├── [IMAGE] LoadingBlocker
│       ├── [TEXT] LoadingText
│       └── [BAR] LoadingProgress
│
└── [UI MANAGERS] UIManagers
    ├── ScreenManager
    ├── NavigationController
    ├── TooltipManager
    ├── PopupManager
    ├── NotificationManager
    └── UIStateManager

[EVENTSYSTEM] EventSystem

[AUDIO] AudioSystem
```

---

# Prefab Instance Rule

Major reusable elements may be prefab instances, but the instances must already be placed in the scene.

Examples:

- Navigation buttons may be instances of NavigationButton.prefab.
- Screen headers may be instances of ScreenHeader.prefab.
- Standard buttons may be prefab instances.
- Progress bars may be prefab instances.
- Equipment slots may be prefab instances.
- Main screens may be prefab instances under ScreenContainer.

The user must be able to select these instances in the Hierarchy.

Do not instantiate permanent screen prefabs only after the player presses a Navigation button.

---

# Prefab Editing Philosophy

Use prefabs to make repeated components consistent.

A prefab should contain its visual structure.

Example Navigation Button prefab:

```text
[NAV BUTTON] NavigationButton
├── [IMAGE] Background
├── [IMAGE] SelectedMarker
├── [ICON] Icon
├── [TEXT] Label
├── [ICON] LockIcon
└── [BADGE] NotificationBadge
    └── [TEXT] BadgeText
```

The user should be able to edit the prefab to change every Navigation button.

Per-instance overrides may be used for:

- Label
- Icon
- Target Screen ID
- Default selected state
- Lock state
- Notification support

---

# Navigation Authoring Rule

The current four Navigation buttons must exist visibly in the Hierarchy:

- Combat
- Inventory
- Equipment
- Woodcutting

Do not generate these buttons from a database at runtime for the current prototype.

Each button should have a NavigationButtonBinding component.

Suggested serialized fields:

- Screen ID
- Button reference
- Icon reference
- Label reference
- Background reference
- Selected marker reference
- Lock icon reference
- Notification badge reference
- Normal state visuals
- Selected state visuals
- Disabled state visuals

The component should report clicks to NavigationController.

It should not search for the target screen.

---

# Future Navigation Entries

Future Navigation buttons may be added manually as prefab instances.

To add a new Profession later:

- Duplicate an existing Navigation button.
- Rename it.
- Change its label.
- Change its icon.
- Change its Screen ID.
- Place it in the correct Navigation group.
- Assign its corresponding screen.

This should not require writing a new navigation script.

An Editor tool may later help add entries, but the generated result must remain visible and editable in the scene.

---

# Screen Authoring Rule

These screen roots must exist under ScreenContainer before Play Mode:

- WoodcuttingScreen
- CombatScreen
- InventoryScreen
- EquipmentScreen

Each screen should contain its major layout in the Hierarchy.

The Screen Manager should only:

- Enable the requested screen.
- Disable the previous screen.
- Update Navigation selection.
- Preserve screen state.
- Handle Back navigation.
- Refresh screen data.

The Screen Manager must not instantiate these screen roots at runtime.

---

# Screen Root Component

Every screen root should have a UIScreenReference or equivalent component.

Suggested fields:

- Screen ID
- Root GameObject
- Default Selected Object
- Preserve State
- Allow Back Navigation

The Screen Manager should receive the screen references through serialized Inspector assignments.

Do not use scene-wide searches to locate screens.

---

# Screen Registry

The Screen Manager should contain a serialized list.

Example:

```text
Screens
├── woodcutting → WoodcuttingScreen
├── combat → CombatScreen
├── inventory → InventoryScreen
├── equipment → EquipmentScreen
└── settings → SettingsScreen
```

The user must be able to see and change these references in the Inspector.

A UIScreenCatalog may still exist for metadata.

The actual scene GameObject references should remain serialized and visible.

---

# Screen Initial Visibility

Only one main screen should be enabled when Play Mode begins.

Other screens may be disabled.

They must still remain visible in the Hierarchy as disabled GameObjects.

Recommended default:

- WoodcuttingScreen: Enabled
- CombatScreen: Disabled
- InventoryScreen: Disabled
- EquipmentScreen: Disabled
- SettingsScreen: Disabled

The UI Bootstrapper may apply a saved last screen after initialization.

---

# Woodcutting Screen Hierarchy

Recommended editable structure:

```text
[SCREEN] WoodcuttingScreen
├── [HEADER] WoodcuttingHeader
│   ├── [ICON] ProfessionIcon
│   ├── [TEXT] ProfessionName
│   ├── [TEXT] ProfessionLevel
│   ├── [BAR] ProfessionXPBar
│   │   ├── Background
│   │   ├── Fill
│   │   └── [TEXT] XPText
│   ├── [TEXT] XPPerHourText
│   ├── [TEXT] TimeToLevelText
│   ├── [TEXT] NextUnlockText
│   ├── [BUTTON] StatisticsButton
│   └── [BUTTON] HelpButton
│
├── [SCROLL] WoodcuttingMainScroll
│   ├── Viewport
│   │   └── Content
│   │       ├── [SECTION] ActiveTargetSection
│   │       │   ├── [HEADER] ActiveTargetHeader
│   │       │   ├── [IMAGE] TargetImage
│   │       │   ├── [TEXT] TargetName
│   │       │   ├── [BAR] TargetDurabilityBar
│   │       │   ├── [BAR] ActionProgressBar
│   │       │   ├── [CONTAINER] ThresholdContainer
│   │       │   ├── [TEXT] RespawnText
│   │       │   └── [TEXT] ActiveStatusText
│   │       │
│   │       ├── [SECTION] ActivitySelectionSection
│   │       │   ├── [HEADER] ActivitySelectionHeader
│   │       │   └── [DYNAMIC CONTENT] TreeCardContainer
│   │       │
│   │       ├── [SECTION] SelectedTreeDetailsSection
│   │       │   ├── [IMAGE] SelectedTreeIcon
│   │       │   ├── [TEXT] SelectedTreeName
│   │       │   ├── [TEXT] SelectedTreeDescription
│   │       │   ├── [CONTAINER] SelectedTreeStats
│   │       │   ├── [CONTAINER] SelectedTreeRewards
│   │       │   └── [CONTAINER] SelectedTreeRequirements
│   │       │
│   │       └── [SECTION] WoodcuttingLoadoutSection
│   │           ├── [SLOT] ToolSlot
│   │           ├── [SLOT] CompanionSlot
│   │           ├── [SLOT] FoodSlot
│   │           └── [SLOT] RelicSlot
│   │
│   └── VerticalScrollbar
│
└── [ACTIONS] WoodcuttingActionBar
    ├── [BUTTON] StartWoodcuttingButton
    └── [BUTTON] StopWoodcuttingButton
```

Only the repeated Tree Cards may be generated at runtime.

The surrounding sections must already exist.

---

# Combat Screen Hierarchy

The Combat Screen should contain both major states in the Hierarchy.

```text
[SCREEN] CombatScreen
├── [HEADER] CombatHeader
│
├── [STATE] CombatSelectionState
│   ├── [SCROLL] CombatSelectionScroll
│   │   ├── Viewport
│   │   │   └── Content
│   │   │       ├── [PANEL] CombatBreadcrumb
│   │   │       ├── [SECTION] RegionSelectionSection
│   │   │       │   └── [DYNAMIC CONTENT] RegionCardContainer
│   │   │       ├── [SECTION] ActivityTypeSection
│   │   │       │   └── [DYNAMIC CONTENT] ActivityTypeContainer
│   │   │       ├── [SECTION] LocationSelectionSection
│   │   │       │   └── [DYNAMIC CONTENT] LocationCardContainer
│   │   │       ├── [SECTION] EnemySelectionSection
│   │   │       │   └── [DYNAMIC CONTENT] EnemyCardContainer
│   │   │       └── [SECTION] EnemyDetailsSection
│   │   │           ├── [IMAGE] EnemyPortrait
│   │   │           ├── [TEXT] EnemyName
│   │   │           ├── [TEXT] EnemyDescription
│   │   │           ├── [CONTAINER] EnemyStatsContainer
│   │   │           ├── [CONTAINER] EnemyLootPreview
│   │   │           ├── [TEXT] EnemyRequirementText
│   │   │           └── [BUTTON] StartCombatButton
│   │   └── VerticalScrollbar
│
└── [STATE] ActiveCombatState
    ├── [LAYOUT] CombatMainArea
    │   ├── [PANEL] PlayerCombatPanel
    │   │   ├── [IMAGE] PlayerPortrait
    │   │   ├── [TEXT] PlayerName
    │   │   ├── [TEXT] CombatDiscipline
    │   │   ├── [BAR] PlayerHealthBar
    │   │   ├── [BAR] PlayerDevotionBar
    │   │   ├── [BAR] PlayerAttackProgressBar
    │   │   ├── [CONTAINER] PlayerStats
    │   │   ├── [CONTAINER] PlayerEquipmentSummary
    │   │   └── [CONTAINER] PlayerEffects
    │   │
    │   ├── [PANEL] CompanionCombatPanel
    │   │   ├── [ICON] CompanionIcon
    │   │   ├── [TEXT] CompanionName
    │   │   └── [TEXT] NoCompanionText
    │   │
    │   └── [PANEL] EnemyCombatPanel
    │       ├── [IMAGE] EnemyPortrait
    │       ├── [TEXT] EnemyName
    │       ├── [BAR] EnemyHealthBar
    │       ├── [BAR] EnemyAttackProgressBar
    │       ├── [CONTAINER] EnemyStats
    │       ├── [CONTAINER] EnemyAbilities
    │       └── [CONTAINER] EnemyEffects
    │
    ├── [PANEL] CombatAbilityPanel
    │   ├── [BUTTON] HeavyStrikeButton
    │   └── [TOGGLE] HeavyStrikeAutoUseToggle
    │
    ├── [PANEL] CombatConsumablesPanel
    │   ├── [SLOT] FoodSlot
    │   ├── [SLOT] HealingPotionSlot
    │   ├── [SLOT] ElixirSlot01
    │   ├── [SLOT] ElixirSlot02
    │   ├── [SLOT] ElixirSlot03
    │   └── [SLOT] ElixirSlot04
    │
    ├── [PANEL] ActiveRuneSummaryPanel
    │   ├── [SLOT] RuneSlot01
    │   ├── [SLOT] RuneSlot02
    │   ├── [SLOT] RuneSlot03
    │   ├── [SLOT] RuneSlot04
    │   └── [SLOT] RuneSlot05
    │
    ├── [PANEL] TemporaryLootPanel
    │   ├── [HEADER] TemporaryLootHeader
    │   ├── [TEXT] TemporaryLootCapacityText
    │   ├── [SCROLL] TemporaryLootScroll
    │   │   └── Viewport
    │   │       └── [DYNAMIC CONTENT] TemporaryLootContainer
    │   └── [BUTTON] PickAllButton
    │
    ├── [PANEL] CombatLogPanel
    │   ├── [HEADER] CombatLogHeader
    │   ├── [SCROLL] CombatLogScroll
    │   │   └── Viewport
    │   │       └── [DYNAMIC CONTENT] CombatLogContainer
    │   └── [BUTTON] ClearCombatLogButton
    │
    └── [ACTIONS] CombatActionBar
        ├── [TOGGLE] AutoRepeatToggle
        └── [BUTTON] QuitCombatButton
```

The two state roots should already exist.

The Combat Controller should only switch their active state.

---

# Inventory Screen Hierarchy

```text
[SCREEN] InventoryScreen
├── [HEADER] InventoryHeader
│   ├── [ICON] InventoryIcon
│   ├── [TEXT] InventoryTitle
│   ├── [TEXT] InventoryCapacityText
│   └── [BUTTON] InventoryHelpButton
│
├── [CONTROLS] InventoryControls
│   ├── [INPUT] InventorySearchInput
│   ├── [BUTTON] AllItemsFilter
│   ├── [BUTTON] ResourcesFilter
│   ├── [BUTTON] EquipmentFilter
│   ├── [BUTTON] ConsumablesFilter
│   └── [DROPDOWN] InventorySortDropdown
│
├── [LAYOUT] InventoryMainLayout
│   ├── [PANEL] InventoryGridPanel
│   │   └── [SCROLL] InventoryGridScroll
│   │       ├── Viewport
│   │       │   └── [DYNAMIC CONTENT] InventoryItemContainer
│   │       └── VerticalScrollbar
│   │
│   └── [PANEL] ItemDetailsPanel
│       ├── [IMAGE] ItemIcon
│       ├── [TEXT] ItemName
│       ├── [TEXT] ItemCategory
│       ├── [TEXT] ItemQuantity
│       ├── [TEXT] ItemDescription
│       ├── [CONTAINER] ItemStatsContainer
│       ├── [CONTAINER] ItemSourcesContainer
│       ├── [BUTTON] EquipItemButton
│       ├── [BUTTON] LockItemButton
│       ├── [BUTTON] FavoriteItemButton
│       ├── [BUTTON] ViewSourcesButton
│       └── [BUTTON] DestroyItemButton
│
└── [PANEL] InventoryEmptyState
```

Only the item slot contents should be generated dynamically.

The controls and Item Details panel must already exist.

---

# Equipment Screen Hierarchy

```text
[SCREEN] EquipmentScreen
├── [HEADER] EquipmentHeader
│   ├── [ICON] EquipmentIcon
│   ├── [TEXT] EquipmentTitle
│   └── [BUTTON] EquipmentHelpButton
│
├── [LAYOUT] EquipmentMainLayout
│   ├── [PANEL] EquippedItemsPanel
│   │   ├── [SLOT] HelmetSlot
│   │   ├── [SLOT] CapeSlot
│   │   ├── [SLOT] AmuletSlot
│   │   ├── [SLOT] WeaponSlot
│   │   ├── [SLOT] ChestSlot
│   │   ├── [SLOT] ShieldSlot
│   │   ├── [SLOT] GlovesSlot
│   │   ├── [SLOT] LegsSlot
│   │   ├── [SLOT] BootsSlot
│   │   ├── [SLOT] RingSlot
│   │   ├── [SLOT] RelicSlot
│   │   └── [SLOT] ToolSlot
│   │
│   ├── [PANEL] CurrentStatsPanel
│   │   └── [CONTAINER] CurrentStatsContainer
│   │
│   ├── [PANEL] EquipmentInventoryPanel
│   │   └── [SCROLL] EquipmentInventoryScroll
│   │       ├── Viewport
│   │       │   └── [DYNAMIC CONTENT] EquipmentItemContainer
│   │       └── VerticalScrollbar
│   │
│   └── [PANEL] EquipmentComparisonPanel
│       ├── [TEXT] ComparisonTitle
│       ├── [IMAGE] CurrentItemIcon
│       ├── [TEXT] CurrentItemName
│       ├── [IMAGE] SelectedItemIcon
│       ├── [TEXT] SelectedItemName
│       ├── [CONTAINER] ComparisonStatsContainer
│       ├── [BUTTON] EquipSelectedButton
│       └── [BUTTON] UnequipCurrentButton
│
└── [PANEL] EquipmentEmptyState
```

All twelve Equipment slots should exist in the Hierarchy.

Do not generate permanent Equipment slots through code.

---

# Inspector-Driven References

Scripts should use serialized references.

Example:

```csharp
public sealed class TopBarView : MonoBehaviour
{
    [SerializeField] private TMP_Text goldValueText;
    [SerializeField] private TMP_Text inventoryCapacityText;
    [SerializeField] private TMP_Text saveStatusText;
    [SerializeField] private Button notificationsButton;
    [SerializeField] private Button settingsButton;
}
```

This is preferred over creating the text and buttons in code.

The Inspector should visibly show which UI elements are connected.

---

# Forbidden Static UI Creation

Codex must remove or stop using runtime code that creates permanent UI through patterns such as:

```csharp
new GameObject("CombatButton");
gameObject.AddComponent<Button>();
gameObject.AddComponent<Image>();
gameObject.AddComponent<TextMeshProUGUI>();
Instantiate(combatScreenPrefab, screenContainer);
```

These patterns are forbidden for permanent UI in the vertical slice.

They remain allowed for dynamic repeated entries when using approved prefabs.

---

# Allowed Runtime Instantiation

Runtime instantiation is allowed for:

```csharp
Instantiate(treeCardPrefab, treeCardContainer);
Instantiate(enemyCardPrefab, enemyCardContainer);
Instantiate(itemSlotPrefab, inventoryItemContainer);
Instantiate(combatLogRowPrefab, combatLogContainer);
Instantiate(lootEntryPrefab, temporaryLootContainer);
Instantiate(notificationToastPrefab, notificationContainer);
```

Every instantiated prefab must be editable through the Project window.

---

# Scripts Must Not Overwrite Layout

Scripts should not routinely overwrite:

- Anchors
- Pivot
- Anchored position
- Width
- Height
- Padding
- Spacing
- Font size
- Text alignment
- Panel color
- Border color
- Image sprite
- Button transition settings

These values belong in the scene, prefab, or shared visual configuration.

Scripts may modify visual state when necessary.

Examples:

- Selected state
- Locked state
- Progress fill
- Warning color
- Active state
- Current icon
- Dynamic text

Scripts must not rebuild the visual layout during Awake, Start, or Update.

---

# Layout Editing Persistence

Changes made in the Unity Inspector must remain authoritative.

Example:

The user changes Left Navigation width from 260 to 300.

Entering Play Mode must not reset it to 260 through script.

The user changes the Combat button height.

Entering Play Mode must not recreate it with the old size.

The user changes Woodcutting Header spacing.

Entering Play Mode must not overwrite the spacing.

Any script currently forcing these values should be removed or made optional.

---

# Visual State Components

Reusable components may control visual states.

Example states:

- Normal
- Hovered
- Pressed
- Selected
- Disabled
- Locked
- Active
- Warning

The default visual values should be assigned in the Inspector.

The component may switch between them at runtime.

It should not invent colors or dimensions through hardcoded values unless the value is a documented fallback.

---

# Navigation Button Binding

Recommended component responsibilities:

```text
NavigationButtonBinding
├── Stores Screen ID
├── Receives Button click
├── Requests ScreenManager.OpenScreen
├── Displays selected state
├── Displays locked state
├── Displays notification badge
└── Displays tooltip when navigation is collapsed
```

It should not:

- Instantiate the destination screen.
- Search the scene for the destination screen.
- Destroy other screens.
- Rebuild Navigation.
- Set permanent button layout values.

---

# Screen Manager Refactor

The Screen Manager should use existing screen objects.

Recommended serialized structure:

```csharp
[Serializable]
public sealed class ScreenSceneEntry
{
    [SerializeField] private string screenId;
    [SerializeField] private UIScreenController screenController;

    public string ScreenId => screenId;
    public UIScreenController ScreenController => screenController;
}
public sealed class ScreenManager : MonoBehaviour
{
    [SerializeField] private List<ScreenSceneEntry> screens;
    [SerializeField] private string defaultScreenId = "woodcutting";
}
```

The exact implementation may differ.

The important rule is that references are visible and assignable in the Inspector.

---

# Screen Opening Behavior

When opening a screen:

- Validate the Screen ID.
- Find the serialized scene entry.
- Close the currently active screen.
- Disable its root.
- Enable the destination screen root.
- Restore its state.
- Refresh its data.
- Update Navigation selected state.
- Update screen history.

Do not instantiate or destroy the main screen.

---

# Runtime Content Containers

All dynamic content areas should be clearly named.

Use names such as:

- TreeCardContainer
- EnemyCardContainer
- InventoryItemContainer
- TemporaryLootContainer
- CombatLogContainer
- NotificationContainer
- RequirementRowContainer
- StatRowContainer
- RewardRowContainer

Do not use only:

- Content
- Content 1
- Items
- List

Where Unity's Scroll View requires an object named Content, the parent should have a descriptive name.

Example:

```text
InventoryGridScroll
└── Viewport
    └── InventoryItemContainer
```

---

# Runtime Template Prefabs

Required dynamic prefabs:

- TreeActivityCard.prefab
- RegionCard.prefab
- CombatActivityTypeButton.prefab
- LocationCard.prefab
- EnemyCard.prefab
- InventoryItemSlot.prefab
- EquipmentInventoryItem.prefab
- TemporaryLootEntry.prefab
- CombatLogRow.prefab
- NotificationToast.prefab
- RequirementRow.prefab
- StatRow.prefab
- RewardRow.prefab

Each prefab should expose editable child objects.

Example:

```text
[PREFAB] TreeActivityCard
├── [IMAGE] Background
├── [IMAGE] SelectedBorder
├── [IMAGE] ActiveBorder
├── [IMAGE] TreeIcon
├── [TEXT] TreeName
├── [TEXT] RequiredLevel
├── [TEXT] Durability
├── [TEXT] CompletionXP
├── [TEXT] PrimaryReward
├── [ICON] LockIcon
├── [TEXT] LockRequirement
└── [BUTTON] SelectButton
```

---

# Placeholder Data in Edit Mode

Major UI fields should contain readable placeholder text in Edit Mode.

Examples:

```text
Gold: 50
Inventory: 4 / 100
Woodcutting Level 1
XP: 0 / 50
Sproutwood Tree
Durability: 25 / 25
No Active Activity
Forest Rat
Health: 30 / 30
```

This helps the user understand the intended layout before Play Mode.

Runtime scripts may replace placeholder values.

Scripts should not leave every field blank in Edit Mode.

---

# No Automatic Hierarchy Destruction

Codex must not create scripts that delete and recreate the permanent UI when Play Mode starts.

Avoid systems such as:

```text
Destroy Existing UI
Generate Complete UI
Generate Navigation
Generate All Screens
Generate Top Bar
```

Any old UIBuilder, RuntimeUIGenerator, CreateUIOnStart, or similar system should be inspected.

If it creates permanent UI:

- Disable it.
- Refactor it.
- Remove its static-generation responsibility.
- Preserve any useful binding or data logic separately.

Do not delete useful gameplay logic simply because it exists in the same class.

Separate the responsibilities first.

---

# Editor Tools

Editor tools are allowed when they improve workflow.

Examples:

- Validate missing references
- Add a Navigation button prefab instance
- Add a standard screen prefab instance
- Select missing references
- Open the relevant prefab
- Preview screen states
- Validate duplicate Screen IDs

Editor tools must not secretly regenerate and overwrite user-edited UI.

Any tool that modifies the Hierarchy should require an explicit button press.

It should not run automatically when entering Play Mode.

---

# Runtime and Edit-Time Separation

Runtime scripts control data and behavior.

Edit-time scene objects control layout.

Example:

The Woodcutting Controller may set:

Tree name
Current durability
XP
Locked state
Active state
Progress fill

It should not set:

- Where the Target panel is placed
- Target panel width
- Target panel height
- Header height
- Card spacing
- Scroll View size
- Font family
- Permanent panel background

---

# Refactor Strategy

The existing UI must be converted carefully.

Do not delete the whole current implementation immediately.

Use the following phases.

---

# Phase 0: Backup and Audit

Before changing UI:

- Confirm the project enters Play Mode.
- Record current Console errors.
- Commit the project to Git.
- Create a backup commit.

Suggested commit:

Before editable hierarchy UI refactor

Audit:

- Which UI objects are created at runtime
- Which scripts create them
- Which scripts contain useful behavior
- Which scripts contain gameplay logic
- Which scripts contain screen switching
- Which scripts assign button events
- Which prefabs already exist
- Which objects already exist in the scene

Acceptance:

Codex produces a list of UI-generation scripts and their responsibilities.

---

# Phase 1: Create Editable Persistent Shell

Create or repair in the scene:

- Main Canvas
- Top Bar
- Main Body
- Left Navigation
- Navigation Scroll View
- Navigation groups
- Four required Navigation buttons
- Content Column
- Active Activity Bar
- Screen Container
- Overlay layers

Do not connect gameplay yet if doing so risks breaking the project.

Acceptance:

The full persistent shell is visible in the Hierarchy before Play Mode.

---

# Phase 2: Convert Navigation

Create visible Navigation button instances:

- CombatButton
- InventoryButton
- EquipmentButton
- WoodcuttingButton

Assign:

- Label
- Icon placeholder
- Target Screen ID
- Selected state references
- Locked state references
- Notification badge references

Connect through NavigationButtonBinding.

Disable the old runtime Navigation generation.

Acceptance:

All four buttons are visible and editable in the Hierarchy.

Clicking them changes screens.

No duplicate runtime buttons appear.

---

# Phase 3: Convert Screen Roots

Create visible screen roots:

- WoodcuttingScreen
- CombatScreen
- InventoryScreen
- EquipmentScreen

Assign them to Screen Manager.

Disable runtime screen instantiation.

Acceptance:

Every screen root exists under ScreenContainer before Play Mode.

Only one is enabled during gameplay.

No duplicate screens are instantiated.

---

# Phase 4: Convert Screen Layouts

Move major screen structure into the Hierarchy.

For each screen:

- Create fixed Header.
- Create fixed action areas.
- Create Scroll View.
- Create major panels.
- Create dynamic content containers.
- Create permanent buttons.
- Assign controller references.

Do not yet focus on final visual polish.

Acceptance:

The screen layout can be edited without changing code.

---

# Phase 5: Preserve Dynamic Lists

Keep data-driven generation for:

- Tree cards
- Enemy cards
- Inventory slots
- Equipment inventory entries
- Loot
- Combat Log
- Notifications

Ensure each system uses editable prefabs.

Acceptance:

Lists populate at runtime without recreating the surrounding screen.

---

# Phase 6: Remove Static Generation Code

After the scene-authored UI works:

- Remove unused permanent UI construction code.
- Remove duplicate object creation.
- Remove unused prefab loading.
- Remove layout values hardcoded in scripts.
- Remove scene searches used only for generated UI.
- Keep gameplay logic intact.
- Keep dynamic list generation intact.

Acceptance:

No permanent duplicate UI appears after entering Play Mode.

---

# Phase 7: Connect Gameplay Data

Assign Inspector references for:

- Top Bar
- Active Activity Bar
- Woodcutting UI
- Combat UI
- Inventory UI
- Equipment UI

Use event-driven updates.

Acceptance:

Existing gameplay remains functional.

Only the presentation architecture changed.

---

# Phase 8: Reference Image Adjustment

After the refactor works:

- Compare against approved UI references.
- Adjust panel positions in the Inspector.
- Adjust spacing.
- Adjust Header proportions.
- Adjust Navigation width.
- Adjust card sizes through prefab editing.
- Adjust text sizes.
- Adjust Scroll Views.
- Adjust Combat panel proportions.

These changes should require little or no code editing.

Acceptance:

The user can visually improve the UI through Unity alone.

---

# Phase 9: Cleanup and Validation

Check:

- No duplicate EventSystem
- No duplicate Canvas
- No duplicate Screen Manager
- No duplicate Navigation
- No duplicate screens
- No missing Inspector references
- No permanent runtime-created UI
- No invisible raycast blockers
- No broken buttons
- No missing prefabs
- No Console errors

Acceptance:

The UI remains stable through repeated screen switching.

---

# Gameplay Preservation Rules

This refactor is primarily a presentation and authoring change.

Do not rewrite working gameplay systems unnecessarily.

Preserve:

- Woodcutting activity logic
- Tree durability
- Reward thresholds
- Profession XP
- Inventory
- Equipment
- Combat
- Warrior XP
- Temporary Loot
- Saving
- Loading
- Offline progress
- Stable IDs

The UI should continue to call the existing systems through their public interfaces.

---

# Button Event Rules

Permanent buttons may connect listeners in code.

Example:

```csharp
private void Awake()
{
    startButton.onClick.AddListener(OnStartPressed);
}
```

The Button itself must already exist in the Hierarchy.

Do not create the Button in Awake.

Listeners should be removed when necessary.

---

# UnityEvent Inspector Option

Simple UI actions may also use Inspector UnityEvents when appropriate.

Examples:

- Open Settings popup
- Close popup
- Toggle panel
- Open help panel

Core gameplay actions should still pass through controllers or gameplay systems.

Avoid putting complex gameplay flow entirely inside Inspector UnityEvents.

---

# RectTransform Rules

RectTransforms for major UI should be configured in the Inspector.

Use anchors and Layout Groups.

Do not store every screen position in script constants.

Major panel anchors should remain visible and editable.

Recommended:

- Top Bar stretches horizontally.
- Main Body stretches below Top Bar.
- Navigation stretches vertically.
- Content Column fills remaining width.
- Active Activity Bar stays at top of Content Column.
- Screen Container fills remaining Content Column.
- Screen roots stretch to Screen Container.
- Main Scroll Views stretch within their screen.

---

# Layout Group Rules

Layout Groups are allowed and encouraged where useful.

Use:

- Horizontal Layout Group
- Vertical Layout Group
- Grid Layout Group
- Layout Element
- Content Size Fitter

Their values should be editable in the Inspector.

Do not use scripts to set spacing and padding every time the scene starts.

---

# Scroll View Rules

Main Scroll Views must already exist.

Scripts may:

- Add runtime entries to Content.
- Restore scroll position.
- Scroll to selected content.
- Enable or disable sections.

Scripts should not create the entire Scroll View structure.

The user must be able to edit:

- Viewport size
- Scrollbar width
- Content padding
- Scroll sensitivity
- Scrollbar visibility
- Section spacing

---

# Canvas Rules

Use one primary Canvas.

Do not create new root Canvases for every screen.

Additional Canvases may only be used where deliberately required.

The Canvas should be visible in the scene before Play Mode.

Required Canvas configuration:

```text
Render Mode: Screen Space - Overlay
Canvas Scaler: Scale With Screen Size
Reference Resolution: 1920 × 1080
Screen Match Mode: Match Width Or Height
Match: 0.5 starting value
```

---

# EventSystem Rule

The scene must contain exactly one EventSystem.

It must use:

```text
InputSystemUIInputModule
```

Do not allow a UI-generation script to create another EventSystem.

---

# Naming Rules

Use prefixes to make the Hierarchy readable.

Recommended prefixes:

```text
[UI]
[PERSISTENT]
[LAYOUT]
[SCREEN]
[STATE]
[HEADER]
[SECTION]
[PANEL]
[SCROLL]
[CONTAINER]
[DYNAMIC CONTENT]
[NAV GROUP]
[NAV BUTTON]
[BUTTON]
[TOGGLE]
[INPUT]
[DROPDOWN]
[TEXT]
[IMAGE]
[ICON]
[BAR]
[SLOT]
[OVERLAY]
[POPUP]
[TOOLTIP]
[ACTIONS]
```

These labels are organizational conventions.

They do not need to be included in runtime player-facing names.

---

# Folder Structure

Recommended UI prefab folders:

```text
Assets/Game/UI/
├── Prefabs/
│   ├── Persistent/
│   │   ├── TopBar.prefab
│   │   ├── LeftNavigation.prefab
│   │   └── ActiveActivityBar.prefab
│   ├── Screens/
│   │   ├── WoodcuttingScreen.prefab
│   │   ├── CombatScreen.prefab
│   │   ├── InventoryScreen.prefab
│   │   └── EquipmentScreen.prefab
│   ├── Navigation/
│   │   ├── NavigationButton.prefab
│   │   └── NavigationGroupHeader.prefab
│   ├── Cards/
│   │   ├── TreeActivityCard.prefab
│   │   ├── RegionCard.prefab
│   │   ├── LocationCard.prefab
│   │   └── EnemyCard.prefab
│   ├── Slots/
│   │   ├── InventoryItemSlot.prefab
│   │   ├── EquipmentSlot.prefab
│   │   ├── ConsumableSlot.prefab
│   │   └── RuneSlot.prefab
│   ├── Rows/
│   │   ├── StatRow.prefab
│   │   ├── RequirementRow.prefab
│   │   ├── RewardRow.prefab
│   │   └── CombatLogRow.prefab
│   └── Overlays/
│       ├── Tooltip.prefab
│       ├── NotificationToast.prefab
│       └── ConfirmationPopup.prefab
```

---

# Prefab Ownership

A prefab should own its visual children.

Example:

InventoryItemSlot.prefab owns:

Background
Item icon
Quantity text
Lock icon
Favorite icon
New marker
Equipped marker
Selected border

The Inventory Screen should own:

- Search
- Filters
- Sort
- Item grid panel
- Item details panel

The Inventory controller should not create these permanent controls.

---

# Handling Current Generated UI

Codex should inspect the current generated interface before changing it.

For each generated object, decide:

## Convert to Scene Object

Use when the object is permanent.

Examples:

- Navigation buttons
- Screen roots
- Top Bar
- Active Activity Bar
- Screen headers
- Start and Stop buttons
- Permanent Combat panels

## Convert to Prefab

Use when the object repeats based on data.

Examples:

- Tree card
- Enemy card
- Item slot
- Loot entry
- Combat Log row

## Keep as Script Behavior

Use when the code performs:

- Data binding
- Button handling
- State updates
- Screen switching
- List population
- Gameplay requests

## Remove

Use when the code:

- Duplicates existing objects
- Creates permanent UI unnecessarily
- Resets user-edited layout
- Creates duplicate managers
- Creates duplicate EventSystems
- Is no longer referenced

---

# No Blind Rewrite Rule

Codex must not delete all existing UI scripts and rebuild the entire project without inspection.

Before replacing a script, Codex must identify:

- What gameplay behavior it contains
- What UI-generation behavior it contains
- What references depend on it
- What save data depends on it
- What can be separated safely

Prefer separating responsibilities over replacing everything.

---

# Required Codex Audit Output

Before performing the refactor, Codex should report:

1. Every script that creates permanent UI.
2. Every script that creates dynamic list entries.
3. Every script that controls screen switching.
4. Every script that changes RectTransform values.
5. Every script that applies colors or font sizes.
6. Every permanent UI prefab currently instantiated at runtime.
7. Every duplicate or conflicting manager.
8. Which existing gameplay logic will remain unchanged.
9. Which objects will be created in the scene.
10. Which scripts will be modified or replaced.

---

# Required Codex Refactor Prompt

Use this instruction when beginning the work:

```text
Refactor the current Unity UI so that the majority of the interface is visible and editable in the scene Hierarchy before Play Mode.

Read and follow “21 Unity UI Hierarchy Authoring and Editable UI Refactor.md”.

The current implementation creates too much permanent UI through scripts. Replace that authoring approach without breaking working gameplay.

Required result:

- Top Bar exists in the Hierarchy.
- Left Navigation exists in the Hierarchy.
- Combat, Inventory, Equipment, and Woodcutting Navigation buttons exist in the Hierarchy.
- Active Activity Bar exists in the Hierarchy.
- Woodcutting, Combat, Inventory, and Equipment screen roots exist under ScreenContainer in the Hierarchy.
- Major screen panels, headers, Scroll Views, detail panels, and permanent buttons exist in the Hierarchy.
- Only repeated database-driven entries are instantiated at runtime.
- Dynamic entries use editable prefabs.
- ScreenManager enables and disables existing screen objects instead of instantiating them.
- User-edited RectTransform, layout, color, font, and spacing values must not be overwritten when Play Mode starts.

Before changing anything:

1. Inspect the existing project.
2. Identify every script that generates UI.
3. Separate permanent UI generation from useful gameplay and data-binding logic.
4. Report the proposed conversion plan.
5. Preserve one EventSystem and InputSystemUIInputModule.
6. Preserve working Woodcutting, Combat, Inventory, Equipment, Save, and Offline systems.

Do not blindly rewrite working gameplay systems.

Do not create duplicate screens, managers, Canvases, or EventSystems.

Keep runtime generation only for repeated entries such as Tree Cards, Enemy Cards, Inventory Slots, Temporary Loot, Combat Log rows, and Notifications.

At completion:

- Enter Play Mode.
- Test every Navigation button.
- Confirm only one main screen is active.
- Confirm no duplicate runtime UI appears.
- Confirm the user can edit permanent UI in the Hierarchy.
- Confirm Inspector layout changes remain after entering Play Mode.
- Check the Console.
- List all created, modified, and removed files.
- List all created and modified scene objects.
- Report any remaining generated permanent UI.
```

---

# Manual User Editing Test

After the refactor, perform this test:

- Exit Play Mode.
- Select LeftNavigation.
- Change its width.
- Select CombatButton.
- Change its height.
- Change the Combat label font size.
- Move the selected marker.
- Select ActiveActivityBar.
- Change its height.
- Select WoodcuttingScreen.
- Change the size of its Header.
- Enter Play Mode.
- Switch between all screens.
- Exit Play Mode.

Expected result:

- All changes remain.
- Navigation still works.
- Screens still switch.
- No duplicate buttons appear.
- No script resets the layout.

---

# Required Acceptance Criteria

The refactor is complete when:

- The Main Canvas exists visibly in the scene.
- The Top Bar exists visibly in the scene.
- Left Navigation exists visibly in the scene.
- Required Navigation buttons exist visibly in the scene.
- Active Activity Bar exists visibly in the scene.
- Required main screen roots exist visibly in the scene.
- Major screen panels exist visibly in the scene.
- Permanent action buttons exist visibly in the scene.
- Equipment slots exist visibly in the scene.
- Active Combat panels exist visibly in the scene.
- Scripts do not construct permanent UI.
- Scripts do not overwrite user-edited layout.
- Dynamic lists still populate correctly.
- Dynamic elements use editable prefabs.
- ScreenManager uses serialized scene references.
- Only one main screen is active.
- Only one EventSystem exists.
- Gameplay continues while browsing screens.
- Saving and loading remain functional.
- Offline progress remains functional.
- No duplicate UI appears during Play Mode.
- The project enters Play Mode without errors.

---

# Final Rules Codex Must Not Break

- Do not create permanent Navigation buttons at runtime.

- Do not instantiate main screens when they are opened.

- Do not construct the Top Bar in code.

- Do not construct the Active Activity Bar in code.

- Do not construct permanent screen Headers in code.

- Do not construct main Scroll Views in code.

- Do not construct permanent Combat panels in code.

- Do not generate permanent Equipment slots in code.

- Do not reset Inspector layout values during startup.

- Do not create another EventSystem.

- Do not create another Main Canvas.

- Do not create duplicate Screen Managers.

- Do not delete working gameplay systems merely to change UI authoring.

- Do not replace dynamic data-driven lists with manually hardcoded entries.

- Do not place gameplay calculations in UI views.

- Do not use static screenshots as the functional interface.

- Do not leave runtime-generated permanent objects after the refactor.

---

# Final Checklist

## Hierarchy

- ✓ Is the entire persistent shell visible before Play Mode?

- ✓ Are Navigation buttons visible before Play Mode?

- ✓ Are main screens visible as disabled or enabled scene objects?

- ✓ Are major panels clearly named?

- ✓ Are dynamic containers clearly identified?

## Editability

- ✓ Can Navigation width be edited in the Inspector?

- ✓ Can button size be edited in the Inspector?

- ✓ Can text size be edited in the Inspector?

- ✓ Can panel spacing be edited in the Inspector?

- ✓ Can Scroll View sizes be edited in the Inspector?

- ✓ Do changes survive Play Mode?

## Navigation

- ✓ Do Combat, Inventory, Equipment, and Woodcutting buttons exist in the scene?

- ✓ Does each button have a serialized Screen ID?

- ✓ Does ScreenManager use existing scene screens?

- ✓ Does selected state work?

- ✓ Does Back navigation work?

## Screens

- ✓ Does WoodcuttingScreen exist under ScreenContainer?

- ✓ Does CombatScreen exist under ScreenContainer?

- ✓ Does InventoryScreen exist under ScreenContainer?

- ✓ Does EquipmentScreen exist under ScreenContainer?

- ✓ Does only one main screen remain enabled?

## Dynamic Content

- ✓ Are Tree Cards generated from an editable prefab?

- ✓ Are Enemy Cards generated from an editable prefab?

- ✓ Are Inventory Slots generated from an editable prefab?

- ✓ Are Temporary Loot entries generated from an editable prefab?

- ✓ Are Combat Log rows generated from an editable prefab?

- ✓ Are Notifications generated from an editable prefab?

## Scripts

- ✓ Do scripts control behavior instead of permanent layout construction?

- ✓ Are UI references serialized?

- ✓ Are scene-wide searches avoided?

- ✓ Are hardcoded RectTransform values removed?

- ✓ Are permanent new GameObject UI calls removed?

- ✓ Are permanent AddComponent UI calls removed?

## Safety

- ✓ Does Woodcutting still work?

- ✓ Does Combat still work?

- ✓ Does Inventory still work?

- ✓ Does Equipment still work?

- ✓ Does saving still work?

- ✓ Does offline progress still work?

- ✓ Is there exactly one EventSystem?

- ✓ Are there no duplicate screens or buttons?

The most important distinction for Codex is: **major screen structure belongs in the Hierarchy; only repeated database content belongs to runtime prefab generation.**