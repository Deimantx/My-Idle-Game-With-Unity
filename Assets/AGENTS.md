# AGENTS.md

## Project

This is a Unity Canvas/uGUI idle RPG.

Primary scene:

- Main gameplay scene containing persistent navigation and all main screens.

Primary UI technology:

- Unity Canvas/uGUI
- TextMeshPro
- Unity Input System

## Important UI Rules

- Permanent UI must exist visibly in the Unity Hierarchy before Play Mode.
- Do not regenerate permanent UI through scripts.
- Do not overwrite manually edited RectTransform values.
- Do not overwrite manually selected icons, fonts, sprites, colors, spacing, or anchors.
- Only repeated data-driven entries may be instantiated from editable prefabs.
- Inspect the existing Hierarchy and serialized references before changing UI code.
- Reuse current scene objects and prefabs.
- Do not create duplicate Canvases, EventSystems, managers, screens, or navigation buttons.
- Preserve manual prefab overrides unless they are broken and the user approves replacing them.

## Equipment Rules

- There is no dedicated Tool equipment slot.
- Profession tools use the Main-Hand slot.
- Profession support items use the Offhand slot.
- Main-Hand Combat Damage and Profession Power are separate values.
- An Axe weapon does not automatically provide Woodcutting bonuses.
- Profession capability must be explicitly defined in item data.
- Two-handed Main-Hand items disable Offhand equipment.
- Do not automatically swap equipment when entering Combat or a Profession.

## Working Rules

- Inspect before modifying.
- Do not blindly rewrite working systems.
- Preserve existing gameplay behavior unless the task explicitly changes it.
- Keep the project compiling after each implementation phase.
- Check Unity Console errors after changes.
- Report every changed scene, prefab, ScriptableObject, and script.
- Never silently delete items, save data, assets, or manually authored UI.