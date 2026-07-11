# 06 Combat Framework

Version: 1.0
Status: Draft

---

# Purpose

This document defines how combat works across the entire game.

Combat is one of the main progression systems and should connect with professions, equipment, companions, food, potions, elixirs, bosses, dungeons, achievements, and future systems.

Combat should be mostly automatic, but difficult encounters may require more preparation or manual Combat Ability usage.

---

# Design Goals

Combat should:

- Be understandable early.
- Become deeper over time.
- Reward preparation more than fast clicking.
- Connect strongly with equipment and crafting.
- Support idle progression.
- Support harder active/manual encounters.
- Provide meaningful loot and progression.
- Remain expandable for bosses, dungeons, elite enemies, towers, raids, and future challenge modes.

Combat should not feel disconnected from the rest of the game.

---

# Core Combat Philosophy

Combat is real-time and automatic by default.

The player prepares before combat by choosing:

Equipment

Food Buffs

Potions

Elixirs

Companions

Combat Abilities

Relic

Enemy, Dungeon, Tower, or other combat activity

Once combat begins, the character attacks automatically.

Once combat begins, the player cannot switch equipment, companions, relics, consumables, or Combat Abilities selected for that battle.

The player can change those choices only after combat ends through Victory, Defeat, or Quit.

The player may optionally use Combat Abilities manually or set allowed abilities to auto mode.

Hard encounters may reward active play, but regular combat should work well while idle.

---

# Combat Styles

The player does not choose a permanent class.

Instead, combat style is determined by equipped weapon, Combat Abilities, gear, and build choices.

Current combat styles:

Warrior

Ranger

Mage

Future styles may be added later.

---

## Warrior

Warrior focuses on melee weapons, armor, defense, and direct physical damage.

Warrior should generally have:

Higher defense

Higher health

Strong single-target damage

Reliable accuracy

Good shield synergy

Lower ranged or elemental options

Typical Warrior weapons:

Sword

Axe

Mace

Two-handed weapon

Typical Warrior equipment:

Heavy armor

Shield

Melee accessories

Warrior should be simple and reliable for early combat.

---

## Ranger

Ranger focuses on ranged weapons, attack speed, accuracy, critical hits, and avoiding heavy defensive setups.

Ranger should generally have:

Higher attack speed

Higher critical chance

Good accuracy

Medium defense

Strong sustained damage

Typical Ranger weapons:

Bow

Longbow

Crossbow

Thrown weapons

Typical Ranger equipment:

Light armor

Ranged accessories

Ranger should feel fast, efficient, and good for farming monsters.

---

## Mage

Mage focuses on elemental damage, powerful Combat Abilities, devotion interaction, and special effects.

Mage should generally have:

Elemental damage

Strong burst damage

Lower physical defense

Higher reliance on Combat Abilities

Possible devotion synergy

Typical Mage weapons:

Staff

Wand

Orb

Typical Mage equipment:

Robes

Magic accessories

Mage should feel powerful but more dependent on preparation, resources, and Combat Ability choices.

---

# Combat Style Rules

Combat styles are not permanent classes.

The player can change combat style by changing equipment before combat starts.

Once combat begins, combat style is locked until combat ends.

Each combat style should have strengths and weaknesses.

No combat style should be best for every encounter.

Some enemies may be weak or resistant to specific styles.

Some bosses may encourage specific styles without making other styles completely useless.

---

# Combat Style Balance

Warrior should be durable and reliable.

Ranger should be fast and efficient.

Mage should be powerful and flexible but more preparation-dependent.

Every style should support idle combat.

Every style should also have room for manual Combat Ability usage in difficult encounters.

---

# Combat Discipline Progression

Combat progression is separated from regular professions.

Regular professions have a maximum level of 100.

Combat Disciplines have a maximum level of 150.

Current Combat Disciplines:

Warrior

Ranger

Mage

Combat Disciplines represent the player's long-term combat experience with each combat style.

They are not permanent classes.

The player may train all Combat Disciplines over time.

---

## Warrior Experience

Warrior XP is gained by dealing melee physical damage.

Warrior XP sources may include:

Melee auto attacks

Melee damage over time effects

Melee Combat Abilities

Melee weapon effects

Shield-based damage effects

Warrior progression should unlock:

Melee weapons

Heavy armor

Defensive abilities

Shield abilities

Melee Combat Abilities

Stronger physical damage options

---

## Ranger Experience

Ranger XP is gained by dealing ranged physical damage.

Ranger XP sources may include:

Bow attacks

Longbow attacks

Crossbow attacks

Thrown weapon attacks

Ranger damage over time effects

Ranged Combat Abilities

Ranged weapon effects

Ranger progression should unlock:

Bows

Longbows

Crossbows

Light armor

Critical hit abilities

Attack speed abilities

Ranged Combat Abilities

Better farming efficiency

---

## Mage Experience

Mage XP is gained by dealing magical or elemental damage.

Mage XP sources may include:

Staff attacks

Wand attacks

Spell damage over time effects

Spell damage

Elemental Combat Abilities

Magic weapon effects

Mage progression should unlock:

Staffs

Wands

Robes

Elemental abilities

Devotion synergy

Magic Combat Abilities

High burst damage options

---

# Combat Disciplines vs Combat Abilities

Combat Disciplines are long-term progression tracks.

Examples:

Warrior

Ranger

Mage

Combat Abilities are individual actions used during combat.

Examples:

Slash

Shield Block

Power Shot

Fireball

Frost Bolt

Combat Abilities do not replace Combat Disciplines.

Combat Abilities belong to a Combat Discipline and may unlock at specific discipline levels.

Examples:

Warrior Level 10 unlocks Shield Block.

Mage Level 15 unlocks Fireball.

Ranger Level 20 unlocks Piercing Shot.

---

# Combat XP Allocation Rules

Combat Discipline XP is granted based on the player's damage source.

Melee damage grants Warrior XP.

Ranged damage grants Ranger XP.

Magic or elemental damage grants Mage XP.

If an ability has mixed damage types, the ability data should define which Combat Discipline receives XP.

Damage over time effects grant XP to the discipline that applied the effect.

XP from a single enemy cannot exceed the enemy's maximum XP value.

Damage beyond the enemy's maximum health does not grant extra XP.

Bosses and special encounters may reduce partial XP if needed for balance.

---

# Companion Damage XP Rules

Companion damage does not grant Combat Discipline XP by default.

Combat Discipline XP comes from the player's own melee, ranged, magic, or elemental damage.

Companion damage helps the player defeat enemies faster and gain loot faster, but it should not become the main method of leveling Warrior, Ranger, or Mage.

Special companions may allow a small percentage of companion damage to grant Combat Discipline XP if explicitly defined in companion data.

This rule prevents companions from replacing player combat progression.

---

# Combat Discipline Unlocks

Combat Discipline levels may unlock:

Weapons

Armor

Combat Abilities

Passive bonuses

Dungeon requirements

Boss requirements

Relic requirements

Combat style upgrades

Special mechanics

Unlocks should feel meaningful and should not only increase numbers.

---

# Combat Discipline Level Cap

Current maximum Combat Discipline level:

150

This is intentionally higher than regular professions to support longer combat progression.

Future expansion may increase this cap if needed.

Combat Disciplines should take longer to max than regular professions.

---

# Combat Discipline UI

The combat UI should display:

Warrior Level

Ranger Level

Mage Level

Current chosen Combat Discipline highlighted

Current Combat Discipline XP

XP progress bar

Current equipped combat style

Unlocked Combat Abilities

Next unlock

The player should clearly understand which Combat Discipline they are currently training.

---

# Combat Ability Unlock Rules

Combat Abilities may require:

Combat Discipline Level

Weapon Type

Equipment Type

Completed Dungeon

Unlocked Area

Relic

Future Achievement

Abilities should be unlocked through progression and should support the identity of their Combat Discipline.

Warrior abilities should feel durable and direct.

Ranger abilities should feel fast and precise.

Mage abilities should feel powerful and effect-based.

---

# Combat Ability Leveling

Current design:

Combat Abilities do not have separate levels.

Abilities are unlocked through Combat Discipline levels and scale from player stats, equipment, buffs, and Combat Discipline level.

Future option:

Individual ability levels may be added later if combat needs deeper progression.

---

# Combat Loop

Player selects enemy, area, boss, dungeon, or other combat activity

↓

Requirements are checked

↓

Combat begins

↓

Player and enemy attack automatically

↓

Assigned combat companion attacks if available

↓

Combat Abilities activate manually or automatically for player and enemy

↓

Player damage is dealt

↓

Companion damage is dealt

↓

Enemy damage is dealt

↓

Combat Discipline XP is gained from player damage

↓

Enemy is defeated

↓

Loot is generated

↓

Statistics and achievements are updated

↓

Next enemy starts or combat stops

---

# Combat Modes

Current combat modes:

Regular Monster Combat

Dungeon Combat

Elite Monster Combat

Boss Combat

Future modes:

Tower Combat

Raid Combat

Arena Combat

World Bosses

Challenge Encounters

---

# Regular Monster Combat

Regular Monster Combat is mostly idle.

The player chooses a monster from a combat area.

The character repeatedly fights the chosen monster until:

Player stops combat

Player dies

Inventory is full

Future condition occurs

Regular monsters should provide:

Combat Discipline XP

Common loot

Crafting materials

Gold

Occasional rare drops

Regular Monster Combat should be the main idle combat farming mode.

---

# Elite Monster Combat

Elite Monsters are stronger enemies with more dangerous mechanics.

Elite Monsters may require:

Better equipment

Specific food buffs

Potions

Elixirs

Specific Combat Abilities

Manual ability usage

Dungeon completion

Area unlocks

Elite Monsters should provide:

Rare materials

Unique equipment

Upgrade materials

Gold

Better loot than regular monsters

Elite Monsters should feel like harder farming targets, not full bosses.

---

# Dungeon Combat

Dungeons are sequences of combat encounters.

The player fights wave after wave of monsters until reaching the final boss.

A dungeon may contain:

Trash monsters

Elite monsters

Mini-bosses

Final boss

Dungeon reward chest

Dungeon runs should feel more important than farming regular monsters.

Dungeons may require preparation and may not be fully idle at first.

---

# Dungeon Result Rules

Dungeon runs can end in:

Victory

Defeat

Player Quit

Victory:

Player receives dungeon chest, XP, loot, and completion progress.

Defeat:

Player keeps XP already earned from dealing damage in dungeon.

Player may lose consumed food, potions, or elixirs.

Player does not receive final dungeon chest.

Player Quit:

Combat ends immediately.

Player keeps already earned XP from dealing damage.

Player may lose consumed food, potions, or elixirs.

Player does not receive final dungeon chest.

Dungeon rewards should clearly explain what is kept and what is lost.

---

# Boss Combat

Bosses are major progression enemies.

Bosses may appear as:

Standalone bosses

Dungeon final bosses

Tower floor bosses

Raid bosses

Bosses should have stronger mechanics than normal enemies.

Bosses may require:

Better equipment

Specific food buffs

Potions

Elixirs

Manual Combat Ability usage

Relic timing

Completed dungeon

Unlocked area

Bosses should reward:

Unique equipment

Rare upgrade materials

Progression unlocks

Achievements

Large gold rewards

Future crafting materials

Bosses should introduce mechanics gradually.

---

# Combat Roles

Current player role:

Solo adventurer

Current companion role:

Secondary combat helper

Future possible systems:

Companion-assisted combat

Pets

Temporary combat helpers

The player should remain the main source of progression.

Companions may contribute meaningful damage and support, but they should not fully replace the player.

---

# Player Stats

Possible player combat stats:

Health

Devotion

Devotion Regeneration

Attack Damage

Attack Speed

Accuracy

Critical Chance

Critical Damage

Defense

Damage Reduction

Health Regeneration

Healing Received %

Elemental Damage

Elemental Resistance

Combat XP Bonus

Future stats may be added if needed.

Avoid adding too many stats too early.

---

# Devotion

Devotion acts as a combat resource similar to prayers.

Devotion may be used to activate special defensive, offensive, or utility effects.

Activating a Devotion ability drains Devotion from the player over time.

If the player runs out of Devotion, the active Devotion ability deactivates.

Devotion should define:

Maximum Devotion

Devotion Regeneration

Devotion Cost per Second

Active Devotion Effects

Devotion UI

Devotion effects may include:

Increased damage

Reduced damage taken

Increased health regeneration

Increased accuracy

Increased healing received

Protection from specific mechanics

Health leech from damage

Devotion should feel powerful but limited by regeneration and resource cost.

---

# Enemy Stats

Every enemy should define:

Enemy ID

Name

Description

Icon

Health

Attack Damage

Attack Speed

Accuracy

Defense

Critical Chance

Critical Damage

Damage Reduction

Elemental Damage

Elemental Resistance

Enemy Abilities

Loot Table

XP Value

Requirements

Region

Future Expansion Notes

---

# Damage Formula

Damage should be easy to understand.

Base player damage comes from:

Weapon

Combat stats

Equipment bonuses

Combat Discipline bonuses

Food buffs

Elixir buffs

Companion bonuses

Relic effects

Future systems

Base companion damage comes from:

Companion rank

Companion combat stats

Companion damage type

Companion abilities

Companion bonuses

Future systems

Exact formulas should be stored in balancing documents.

Do not hardcode final values inside unrelated scripts.

---

# Accuracy and Hit Chance

Not every attack must hit.

Player hit chance may depend on:

Player Accuracy

Enemy Defense

Equipment

Buffs

Debuffs

Future modifiers

Companion hit chance may depend on:

Companion Accuracy

Enemy Defense

Companion Rank

Companion bonuses

Future modifiers

Misses should be visible but not frustrating.

Early combat should have high enough hit chance to feel smooth.

---

# Critical Hits

Critical hits are stronger attacks.

Player critical hits may depend on:

Critical Chance

Critical Damage

Equipment

Combat Abilities

Companions

Food buffs

Elixir buffs

Relics

Companion critical hits may depend on:

Companion Critical Chance

Companion Critical Damage

Companion Rank

Companion bonuses

Critical hits should feel exciting but not be the only way to deal damage.

---

# Attack Speed

Attack speed controls how often attacks happen.

Player attack speed may come from:

Weapon type

Equipment bonuses

Food buffs

Elixir buffs

Combat Abilities

Companions

Relics

Companion attack speed may come from:

Companion base attack speed

Companion rank

Companion bonuses

Future systems

Attack speed should be clear in the UI.

Progress bars should be used to show attack timing.

Avoid creating attack speeds so fast that combat becomes unreadable.

---

# Health and Death

Player health determines survival.

If player health reaches 0:

Combat stops

Player is defeated

Enemy resets or encounter fails

Player keeps permanent progression

Possible future penalties:

Lost dungeon attempt

Food consumed

Potion consumed

Elixir duration lost

Cooldown before retry

Do not add harsh death penalties unless they improve gameplay.

---

# Healing

Healing may come from:

Potions

Food buffs

Elixir buffs

Equipment

Health Regeneration

Combat Abilities

Relics

Companion Effects

Future systems

Potions are the main direct healing source.

Food is currently planned as a short-duration buff, not simple one-click healing food.

Elixirs are stronger temporary buffs.

Food buffs may provide:

Health regeneration

Healing received bonus

Damage bonus

Defense bonus

Profession efficiency bonus

Combat efficiency bonus

---

# Combat Abilities

Combat Abilities are active or auto-used combat actions.

Combat Abilities may:

Deal damage

Heal the player

Increase defense

Increase attack speed

Increase critical chance

Apply buffs

Apply debuffs

Interrupt enemy abilities

Reduce incoming damage

Combat Abilities should have cooldowns.

The player may choose whether some abilities are:

Manual only

Auto-use allowed

Always passive

---

# Enemy Abilities

Enemies can use abilities automatically.

Enemy abilities should always follow cooldowns.

Most enemies should start combat with cooldowns already active so they do not instantly spam abilities at the start of combat.

Enemy abilities may:

Deal burst damage

Apply debuffs

Heal themselves

Increase defense

Charge a strong attack

Use interruptible abilities

Punish poor preparation

Hard enemies should become dangerous because of mechanics, not only bigger numbers.

---

# Companion Abilities

Combat companions may have their own abilities.

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

---

# Buffs

Buffs are positive temporary effects.

Buffs may come from:

Food

Elixirs

Equipment

Combat Abilities

Companions

Relics

Buffs may affect:

Damage

Defense

Attack speed

Accuracy

Critical chance

Health regeneration

XP gain

Resource gain

Buff duration should be visible to the player.

---

# Debuffs

Debuffs are negative temporary effects.

Debuffs may affect:

Damage

Defense

Attack speed

Accuracy

Healing received

Ability cooldowns

Debuffs should be clearly shown in combat UI.

---

# Status Effects

Possible future status effects:

Burn

Poison

Bleed

Freeze

Stun

Slow

Weakness

Vulnerability

Silence

Status effects should be added slowly.

Do not overload early combat with too many effects.

---

# Combat Consumables

Combat consumables are divided into categories.

Food:

Short-duration buffs.

Examples:

Damage bonus

Defense bonus

Health regeneration

XP gain

Profession efficiency

Potions:

Instant or short-term healing effects.

Examples:

Restore health

Restore devotion

Emergency healing

Elixirs:

Temporary stronger combat buffs.

Examples:

Increased attack speed

Increased critical chance

Increased healing received

Increased elemental resistance

Food, potions, and elixirs should have separate UI slots so the player understands their purpose.

Current combat consumable loadout:

1 Healing Potion

1 Food

4 Different Elixirs

Future systems may change these limits.

---

# Combat XP

Combat Discipline XP is gained from player damage dealt to enemies.

Player melee damage grants Warrior XP.

Player ranged damage grants Ranger XP.

Player magic or elemental damage grants Mage XP.

Companion damage helps defeat enemies faster, but does not grant Combat Discipline XP by default.

This means:

Longer fights still reward progress.

Partial boss attempts can reward XP.

Companion damage improves kill speed and loot speed.

Companions do not become the main method of leveling Warrior, Ranger, or Mage.

XP amount may depend on:

Player damage dealt

Enemy difficulty

Combat Discipline modifiers

Equipment bonuses

Food buffs

Elixir buffs

Relic effects

Special companion effects

XP from a single enemy cannot exceed the enemy's maximum XP value.

Damage beyond the enemy's maximum health does not grant extra XP.

---

# Loot

Enemies may drop:

Gold

Crafting materials

Equipment

Upgrade materials

Companion rank materials

Rare items

Dungeon keys

Future collectibles

Loot should connect combat to non-combat systems.

Example:

Boss drops upgrade material

↓

Upgrade axe

↓

Improve Woodcutting

↓

Gather better logs

↓

Craft better equipment

↓

Fight stronger boss

---

# Loot Tables

Every enemy should use a loot table.

Loot tables may contain:

Guaranteed drops

Chance drops

Rare drops

Unique drops

First-time clear rewards

Dungeon completion rewards

Loot tables should be data-driven.

Do not hardcode drops inside enemy scripts.

---

# Combat Requirements

Combat content may require:

Combat Discipline level

Equipment

Specific item

Completed dungeon

Unlocked area

Specific Profession level

Specific companion rank

Future achievement

Requirements should be visible before the player enters combat.

---

# Risk vs Reward

Harder combat should reward better progression.

Harder combat may offer:

More gold

Better loot

Rare materials

Unique equipment

Unlocks

Achievements

Cosmetics

Progression materials

The player should decide whether farming safe content or pushing harder content is better.

---

# Offline Combat

Offline combat may be allowed if the player is already in combat when leaving the game.

Offline combat should respect:

Currently equipped gear

Assigned combat companion

Current companion rank

Selected Combat Abilities

Active food buff

Active elixirs

Available potions

Relic effects if active

Offline combat should calculate:

Time away

Player damage

Companion damage

Enemy damage

Kills

Deaths

Loot gained

Player Combat Discipline XP

Food buff duration used

Potion usage

Elixir duration used

Death possibility

Companion damage should increase offline kill speed.

Companion damage should not grant Combat Discipline XP unless a special companion effect explicitly allows it.

Very difficult bosses, raids, arena encounters, or manual-only encounters may disable offline combat.

---

# Manual Combat

Manual combat is optional for most content.

Manual combat may be important for:

Bosses

Dungeons

Elite monsters

Challenge encounters

Future raids

Manual play should improve success chance but should not be required for basic progression.

Manual control may include:

Activating Combat Abilities

Timing relics

Using defensive ability

Responding to enemy ability

Manual combat should reward attention without turning the entire game into a reflex-based game.

---

# Auto Combat

Auto combat should allow players to idle normal content.

Auto combat may include:

Auto attacks

Auto Combat Abilities

Auto companion attacks

Auto companion abilities

Auto repeat enemy

Auto dungeon repeat

Auto stop on death

Auto stop when inventory is full

Auto combat settings should be simple.

---

# Combat UI

Combat UI should display:

Player health bar

Player devotion bar

Enemy health bar

Player stats

Enemy stats

Player active buffs

Enemy active buffs

Player active debuffs

Enemy active debuffs

Combat Ability buttons for player

Ability cooldowns for player and enemy

Relic button

Current food buff

Potion slot

Elixir slots

Damage numbers

XP gained

Loot drops

Combat log

Start / Stop button

Auto combat toggle

Manual ability toggle

The player should always understand what is happening.

---

# Companion Combat UI

Companion combat display should show:

Companion icon

Companion name

Companion rank

Companion attack timer

Companion damage numbers

Companion ability cooldowns

Companion damage dealt this fight

Companion total contribution

Companion health if companion health is added later

---

# Combat Log

The combat log should show important events.

Examples:

Player hits enemy

Companion hits enemy

Enemy hits player

Critical hit

Companion critical hit

Ability used

Companion ability used

Buff applied

Debuff applied

Enemy defeated

Companion lands killing blow

Loot gained

Player defeated

Rare drop found

New combat started with new enemy

Combat log should be readable with colors and icons.

Combat log should not spam too much useless information.

---

# Equipment Interaction

Combat reads bonuses from currently equipped gear.

Weapons affect:

Damage

Attack speed

Accuracy

Critical stats

Armor affects:

Defense

Health

Damage reduction

Resistance

Accessories affect:

Flexible bonuses

Relics affect:

Passive or active effects

Shield affects:

Defense

Damage reduction

Special defensive bonuses

Two-handed weapons disable shield bonuses.

---

# Companion Interaction

Companions may affect combat.

Possible effects:

Deal damage to enemies

Increase player damage

Increase player defense

Increase Combat Discipline XP gained from player damage

Increase rare drop chance

Heal over time

Reduce enemy damage

Improve food buffs

Improve potion healing

Improve elixir buffs

Improve devotion regeneration

Apply buffs

Apply debuffs

Combat companions may require combat gear as rank-up materials.

Companion damage is separate from player damage.

Companion damage helps defeat enemies but does not grant Combat Discipline XP by default.

---

# Companion Combat Damage

Combat companions may directly attack enemies.

Companion attacks are separate from player attacks.

Companion attacks may have:

Attack Damage

Attack Speed

Accuracy

Critical Chance

Critical Damage

Damage Type

Combat Style Support

Ability Cooldowns

Companion damage contributes to defeating enemies.

If a companion kills an enemy, the player still receives kill credit and loot.

Companion damage should be visible in combat logs and damage numbers.

Companion damage should be balanced so that companions support combat without replacing the player's own build, equipment, Combat Discipline levels, or preparation.

---

# Food Interaction

Food provides temporary buffs.

Food buffs may support:

Combat damage

Combat defense

Health regeneration

Healing received

Critical chance

Attack speed

XP gain

Profession efficiency

Food buffs should last for a short duration.

Current target duration:

2–5 minutes

Food should have an auto-reuse button and amount left shown in UI.

---

# Potion Interaction

Potions provide direct or short-term effects.

Potions may support:

Health restore

Devotion restore

Emergency healing

Short healing over time

Potions should be limited by the combat loadout.

Current loadout:

1 healing potion type

Future systems may add more potion slots if needed.

---

# Elixir Interaction

Elixirs provide stronger temporary buffs.

Elixirs may support:

Attack speed

Critical chance

Healing received

Elemental resistance

Damage

Defense

Devotion regeneration

Elixirs should be limited by the combat loadout.

Current loadout:

4 different elixir types

Elixir duration should be visible in combat UI.

---

# Relic Interaction

Relics may provide active or passive combat effects.

Active relics should have:

Activation effect

Duration

Cooldown

Relics should feel powerful but strategic.

Current possible relic duration range:

5 minutes to 3 hours

Current possible relic cooldown range:

1–6 hours depending on relic power and upgrades

Relics should not require constant spam.

---

# Combat Statistics

The game should track:

Total player damage dealt

Total companion damage dealt

Total damage taken

Total enemies killed

Total enemies killed by companions

Total deaths

Total bosses killed

Total dungeons completed

Highest player hit

Highest companion hit

Critical hits

Companion critical hits

Misses

Combat time

Loot gained

Rare drops found

XP gained from combat

Specific monster kills

Specific boss kills

---

# Balance Philosophy

Combat should reward preparation.

Numbers should matter, but strategy should matter too.

Better gear should feel powerful.

However, gear should not remove the value of:

Food buffs

Potions

Elixirs

Companions

Profession levels

Combat Discipline levels

Achievements

Player decisions

Hard enemies should not only be stat walls.

Bosses should introduce mechanics gradually.

Companion damage should support combat without replacing player damage.

---

# Technical Rules

Combat should be data-driven.

Enemies should be defined through data.

Combat Abilities should be defined through data.

Enemy Abilities should be defined through data.

Companion combat stats should be defined through data.

Loot tables should be defined through data.

Combat stats should be modular.

Combat should connect to:

Experience System

Item Database

Equipment Database

Inventory System

Companion System

Achievement System

Statistics System

Save System

Offline Progress System

Combat formulas should be easy to adjust in balancing documents.

---

# Future Expansion

Possible additions:

Additional Combat Styles

Elemental weaknesses

Armor sets

Dungeons

Raids

Towers

Arena

Boss phases

Elite modifiers

Unique enemy mechanics

Combat pets

Challenge modes

Manual companion abilities

Companion formations

World bosses

---

# Checklist

Every combat encounter should answer:

✓ What enemy or enemies are fought?

✓ What are the requirements?

✓ Is it idle, manual, or mixed?

✓ What stats does the enemy have?

✓ What abilities does the enemy use?

✓ Can companions deal damage in this encounter?

✓ What loot can drop?

✓ What XP is rewarded?

✓ Can this combat run offline?

✓ What makes this encounter different?

✓ Does it connect to other systems?

✓ Does it provide meaningful progression?

✓ Can it support future expansion?