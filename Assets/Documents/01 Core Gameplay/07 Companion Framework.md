# 07 Companion Framework

Version: 1.0
Status: Draft

---

# Purpose

This document defines how companions work across the entire game.

Companions are long-term progression helpers that can support professions, combat, equipment, consumables, loot, offline progress, and future systems.

Companions should feel meaningful, collectible, and useful without replacing the player's own progression.

---

# Design Goals

Companions should:

- Provide long-term progression goals.
- Support professions and combat.
- Connect to items, equipment, bosses, dungeons, and achievements.
- Give players meaningful choices.
- Be useful both online and offline.
- Be data-driven.
- Avoid becoming mandatory for every activity.
- Avoid replacing profession levels, Combat Discipline levels, equipment, or player decisions.

Companions should feel like powerful helpers, not the main source of progression.

---

# Companion Progression

Companions do not gain XP from actions.

Companions do not have normal levels.

Companions use ranks instead.

Current rank range:

Rank 1–20

A companion increases rank when the player gives it required items and waits for the required time to pass.

Companion ranks should feel like long-term investment.

---

# Companion Rank Rules

Every companion rank should define:

Current Rank

Maximum Rank

Required Items

Required Gold if applicable

Required Time

Unlocked Bonus

Future Expansion Notes

Rank-up requirements should match the companion's identity.

Examples:

Combat companions may require combat gear, monster drops, boss materials, or dungeon materials.

Woodcutting companions may require logs, axes, planks, or rare tree drops.

Mining companions may require ores, bars, gems, or pickaxes.

Crafting companions may require crafted items or profession materials.

Rare companions may require unique materials from difficult content.

---

# Companion Categories

Current companion categories:

Profession Companions

Combat Companions

Future categories may include:

Hybrid Companions

Utility Companions

Legendary Companions

Seasonal Companions

---

# Profession Companions

Profession companions improve non-combat professions.

Possible effects:

Increase profession XP

Increase resource gain

Reduce action time

Increase rare drop chance

Reduce resource cost

Increase bonus output chance

Improve offline profession progress

Unlock special profession activities

Profession companions should support specific professions or profession groups.

Examples:

Woodcutting Companion

Mining Companion

Fishing Companion

Cooking Companion

Smithing Companion

Carpentry Companion

Herblore Companion

---

# Combat Companions

Combat companions improve combat performance.

Combat companions may directly attack enemies.

Possible effects:

Deal damage to enemies

Increase player damage

Increase player defense

Increase Combat Discipline XP gained from player damage

Increase rare drop chance

Improve food buffs

Improve potion healing

Improve elixir buffs

Heal the player over time

Reduce enemy damage

Improve devotion regeneration

Apply buffs

Apply debuffs

Combat companions should support combat without replacing equipment, Combat Discipline levels, or player preparation.

---

# Companion Combat Participation

Combat companions may directly participate in combat by attacking enemies.

A combat companion may:

Deal damage

Use basic attacks

Use companion abilities

Apply buffs

Apply debuffs

Heal the player

Support devotion regeneration

Improve loot chance

Improve rare drop chance

Companion damage should make combat faster, but companions should not replace the player's own damage, equipment, Combat Discipline levels, or preparation.

The player should remain the main source of combat progression.

---

# Companion Combat Safety Rules

Current design:

Companions do not have health.

Companions cannot die.

Companions cannot be targeted by enemies.

Companions cannot be removed from combat by enemy attacks.

The player does not need to heal companions.

This keeps companion combat simple and avoids turning companions into another character management system.

Future systems may add companion health or targeting only if it improves gameplay.

---

# Companion Structure

Every companion contains:

Companion ID

Name

Description

Icon

Category

Current Rank

Maximum Rank

Rank Requirements

Current Bonuses

Unlock Source

Assigned Activity

Can Support Combat

Can Support Professions

Can Work Offline

Future Expansion Notes

---

# Combat Companion Structure

Every combat companion should define:

Companion Attack Damage

Companion Attack Speed

Companion Accuracy

Companion Critical Chance

Companion Critical Damage

Damage Type

Combat Style Supported

Companion Abilities

Ability Cooldowns

Scaling Rules

Can Deal Damage Offline

Future Expansion Notes

Companion combat stats should scale mainly from companion rank.

Some companions may also scale from player equipment, Combat Discipline level, relics, or future systems.

---

# Companion Damage Rules

Combat companions can deal damage to enemies.

Companion damage counts toward defeating enemies.

If a companion lands the killing blow, the player still receives loot and kill credit.

Companion damage does not grant Combat Discipline XP by default.

Combat Discipline XP should come from the player's own damage.

Special companions may allow a small percentage of companion damage to grant Combat Discipline XP if explicitly defined in companion data.

Companion damage should be shown separately in combat UI and combat logs.

Companion damage should be balanced so that companions support combat without replacing the player's own build, equipment, Combat Discipline levels, or preparation.

---

# Companion Ability Rules

Combat companions may have abilities.

Companion abilities may:

Deal damage

Apply buffs

Apply debuffs

Heal the player

Improve devotion regeneration

Reduce enemy damage

Increase loot chance

Increase rare drop chance

Companion abilities should have cooldowns.

Companion abilities should usually activate automatically.

Future systems may allow manual companion ability usage if needed.

Companion abilities should be data-driven.

---

# Companion Sources

Companions may come from:

Profession unlocks

Dungeon rewards

Boss drops

Achievements

Shops

Special crafting

Future systems

Rare companions should feel exciting to unlock.

---

# Companion Assignment

Companions may need to be assigned before they provide bonuses.

Possible assignment types:

Assigned to a profession

Assigned to combat

Current design:

A companion must be assigned before an activity starts.

If combat begins, the chosen combat companion is locked until combat ends.

Companions should not be freely swapped during active combat.

The player can change companions only after combat ends through Victory, Defeat, or Quit.

---

# Companion Limits

The game should define how many companions can be active at once.

Suggested starting design:

1 active profession companion

1 active combat companion

Future possible slots:

1 active utility companion

Additional companion slots from future progression

Companion limits create meaningful choices.

Avoid allowing every companion bonus to be active at the same time.

---

# Companion Bonuses

Companion bonuses may affect:

Profession XP

Combat Discipline XP from player damage

Resource Gain

Action Speed

Rare Drop Chance

Gold Gain

Player Combat Damage

Companion Combat Damage

Player Defense

Food Buff Strength

Food Buff Duration

Potion Healing

Elixir Strength

Devotion Regeneration

Offline Progress

Rank-up Time

Bonuses should be clear and easy to understand.

---

# Companion Balance Philosophy

Companions should improve progression without becoming mandatory.

A companion should never completely replace:

Profession Levels

Combat Discipline Levels

Equipment

Food Buffs

Potions

Elixirs

Relics

Achievements

Player Decisions

Higher rank companions should feel stronger, but lower rank companions should still feel useful.

Combat companions may deal meaningful damage, but the player should remain the main source of Combat Discipline XP and combat progression.

---

# Companion Rank-Up Design

Rank-ups should provide meaningful improvements.

Examples:

Rank 1:

Small basic bonus.

Rank 5:

Improved main bonus.

Rank 10:

Unlock secondary bonus.

Rank 15:

Unlock stronger special bonus.

Rank 20:

Unlock final signature bonus.

Not every rank needs a new mechanic, but major milestones should feel exciting.

---

# Companion Rank-Up Example

Example companion:

Oakling

Category:

Profession Companion

Supports:

Woodcutting

Rank bonuses:

Rank 1:

+2% Woodcutting XP

Rank 5:

+3% Woodcutting resource gain

Rank 10:

+5% chance for bonus logs

Rank 15:

Small chance to find rare seeds while Woodcutting

Rank 20:

Unlocks special Ancient Grove activity

Rank-up materials may include:

Logs

Planks

Rare tree drops

Woodcutting tools

Gold

Time

---

# Combat Companion Example

Example companion:

Ember Wolf

Category:

Combat Companion

Supports:

Combat

Damage Type:

Fire / Melee

Rank bonuses:

Rank 1:

Companion attacks enemies for small fire damage.

Rank 5:

Increases companion attack speed.

Rank 10:

Adds small chance to apply Burn.

Rank 15:

Improves player fire damage.

Rank 20:

Unlocks Ember Howl companion ability.

Rank-up materials may include:

Combat gear

Monster drops

Boss materials

Dungeon materials

Gold

Time

---

# Companion and Items

Companions should strongly connect to the Item Framework.

Items may be used for:

Unlocking companions

Ranking up companions

Feeding companions

Crafting companion items

Unlocking companion bonuses

Companion progression should help keep older resources useful.

---

# Companion and Equipment

Companions may require equipment as rank-up materials.

Examples:

Combat companion requests swords, armor, shields, bows, staves, or boss gear.

Profession companion requests tools such as axes, pickaxes, fishing rods, hammers, or saws.

Companions may also improve specific equipment types.

Examples:

Warrior companion improves shield bonuses.

Ranger companion improves bow attack speed.

Mage companion improves staff elemental damage.

Mining companion improves pickaxe efficiency.

---

# Companion and Combat

Combat companions may affect:

Damage

Defense

Healing

Devotion

Loot

Rare drops

Combat Discipline XP gained from player damage

Combat companions should be selected before combat starts.

Once combat begins, combat companion choice is locked until combat ends.

Combat companions should not allow players to ignore combat preparation.

Companion damage is separate from player damage.

Companion damage helps defeat enemies but does not grant Combat Discipline XP by default.

---

# Companion and Professions

Profession companions may affect:

Profession XP

Action speed

Resource output

Rare drops

Offline progress

Bonus materials

Profession companions should connect clearly to specific professions or profession groups.

Examples:

Woodcutting companion supports trees and logs.

Mining companion supports ores and gems.

Fishing companion supports fish and rare catches.

Crafting companion supports resource conversion.

---

# Companion and Offline Progress

Companion bonuses should apply to offline progress unless specifically disabled.

Offline calculations should use:

Assigned companion

Current companion rank

Current companion bonuses

Currently active profession or combat activity

Saved equipment

Saved buffs if applicable

Some companions may specialize in offline bonuses.

---

# Companion Offline Combat Damage

If offline combat is allowed, assigned combat companion damage should be included in offline calculations.

Offline combat should calculate:

Player damage

Companion damage

Enemy damage

Kills

Deaths

Loot

Combat Discipline XP from player damage

Food, potion, and elixir usage

Companion rank and bonuses

Companion damage should improve offline kill speed.

Companion damage should not create Combat Discipline XP unless specifically allowed.

---

# Companion and Achievements

Achievements may require:

Unlocking companions

Ranking companions

Using companions

Completing content with specific companions

Reaching rank 20 with specific companions

Collecting all companions

Dealing damage with combat companions

Defeating enemies with companion damage

Companion achievements should reward long-term progression.

---

# Companion User Interface

Companion UI should display:

Icon

Name

Description

Category

Current Rank

Maximum Rank

Current Bonus

Next Rank Bonus

Required Items

Required Time

Unlock Source

Assign Button

Rank Up Button

Assigned Activity

Progress Timer

Companion UI should make it clear:

What the companion does

How to rank it up

What the next rank unlocks

Where to get required materials

---

# Combat Companion User Interface

For combat companions, the UI should also display:

Attack Damage

Attack Speed

Accuracy

Critical Chance

Critical Damage

Damage Type

Combat Style Supported

Current Combat Bonus

Companion Abilities

Ability Cooldowns

Damage Done This Fight

Damage Done Total

Combat companion stats should be visible enough that the player understands how much the companion contributes.

---

# Companion Rank-Up Timer

Rank-ups may take real time.

Rank-up timer rules:

The player gives required items.

Rank-up timer starts.

Items are consumed when the rank-up begins.

When timer completes, companion rank increases.

The player can claim the completed rank-up.

Future systems may reduce rank-up time.

Rank-up timers should not feel like annoying mobile game waiting mechanics.

They should support long-term planning.

---

# Companion Statistics

The game should track:

Companions unlocked

Highest companion rank

Rank 20 companions

Total items spent on companions

Total time spent ranking companions

Total profession resources gained from companions

Total player damage increased by companions

Total companion damage dealt

Total enemies killed by companions

Highest companion hit

Total rare drops gained with companions

Total offline progress helped by companions

---

# Companion Database

Every companion should exist inside the Companion Database.

Companions should never be hardcoded.

Systems should reference Companion IDs.

Companion data should connect to:

Item Database

Profession System

Combat System

Equipment System

Inventory System

Achievement System

Statistics System

Save System

Offline Progress System

---

# Technical Rules

Companions should be data-driven.

Companion bonuses should be modular.

Companion combat stats should be data-driven.

Companion abilities should be data-driven.

Rank requirements should be editable without code changes.

Companion rank timers should save and load correctly.

Assigned companions should save and load correctly.

Companion damage should apply consistently online and offline.

Companion bonuses should apply consistently online and offline.

Companion effects should not be hardcoded inside individual profession or combat scripts.

Combat companion damage should not grant Combat Discipline XP unless explicitly enabled in companion data.

---

# Future Expansion

Possible additions:

Companion Evolution

Companion Traits

Companion Gear

Companion Bond

Companion Expeditions

Companion Synergy

Legendary Companions

Seasonal Companions

Companion Collection Log

Companion Passive Tree

Manual Companion Abilities

Companion Formations

Future companion systems should expand the base companion framework rather than replace it.

---

# Checklist

Every new companion should answer:

✓ What category is this companion?

✓ What does this companion support?

✓ Where does this companion come from?

✓ How is this companion unlocked?

✓ What are its rank requirements?

✓ What bonuses does each major rank provide?

✓ Can it support professions?

✓ Can it support combat?

✓ Can it deal damage?

✓ Can it work offline?

✓ Which items does it consume?

✓ Which systems does it connect to?

✓ Does it remain useful later?

✓ Can future systems expand this companion?