# Technical Rules for Codex

This document defines the core architecture rules that every system in the project must follow. These rules take priority over individual feature implementations to ensure the project remains scalable, maintainable, and AI-friendly.

## Core Rules

* Always use **ScriptableObjects** for game data (items, skills, monsters, trees, recipes, equipment, etc.).
* Never hardcode IDs, names, or gameplay values inside scripts.
* Make every system **data-driven** so content can be added without modifying code.
* Keep the project in **one Unity scene** unless there is a compelling technical reason to create another.
* Maintain **persistent navigation** that remains available while switching between screens.
* Avoid using **Singletons** unless they are genuinely necessary for global systems.
* Implement **autosave every 30 seconds**, in addition to saving on application quit.
* Design every system as **modular and reusable**, allowing future skills and content to reuse existing code rather than duplicate it.
