# Project-Hogsmeade
2.5D wizard platformer

## Project Plan

## Instructions for prototype 
•	A goes left.
•	D goes right.
•	Space bar does jump and double jump.
•	E key is projectile (shoot) spell.
•	F key is teleport spell.
•	R key is shield spell.
•	Shift + direction key(A or D) is dash.


### Prototype



- [x] player script, which holds the different properties for a player and proides helper methods for interacting with the player.
- [x] player movement script, with custom logic for jumping, double jumping, and wall jumping.
- [x] spell system, casting different spells. Including validation, cooldowns and sync it to the players mana.
- [x] script for pickups and prefabs for potions.
- [x] simple HUD, which displays the players health and mana.
- [x] simple enemy AI, which targets the player and throws projectiles at them.
- [x] basic menu system, with start game, options and quit buttons.
- [ ] simple tilemap system for creating levels 
- [ ] different types of terrains that affect the player, i.e by damaging, slowing or trapping them.
- [ ] xp and level system for the player 
- [ ] build at least 2 levels, whcih include transitions from one level to the next.
- [ ] add win/lose states
- [ ] build a save/load state and menu
- [ ] add a simple instructions scene


### Full Game

- [ ] Write lore
- [ ] Implement tilemap rules
- [ ] Spell slot system
- [ ] Implement full stat system
- [ ] add more spells, enemies, terrains and pickups
- [ ] Create entity sprites and animations
- [ ] Create terrain sprites
- [ ] Create spectial fx sprites and animations
- [ ] Create sound fx and music
- [ ] Create lore dialogues
- [ ] Create lore cutscenes
- [ ] add boss battle
- [ ] add more settings to the options menu such as button mapping and diffictuly
- [ ] Add a win state
 
## Contrubution Guidelines


### Creating a branch

When adding a new feature / working on anything to do with the project, please create a new branch. This can be done by running the following command in the terminal:

```sh
git checkout -b <branch-name>
```

This will create a new branch and switch to it. You can then push this branch to github by running:

```sh
git push -u origin <branch-name>
```

### Commiting changes

From here, you can start working on your changes. Please commit often, and use proper commit messages when doing so.

**NOTE**: Always make sure you are on the correct branch before commiting. You can check which branch you are on by running:

```sh
git branch
```

When you are done working on your changes, you can push them to github by running:

```sh
git push
```

### Creating a pull request

Once you have pushed your changes, you can create a pull request on [github](https://github.com).

When creating a pull request, please make sure to add a description of what you have changed, and what you have added. If you are adding a new feature, ideally you should add a screenshot or gif of the feature in action.

if you know what you are doing, you can merge the main branch into your branch to resolve any conflicts. If you are unsure, please assign the review to me. 

Once it has been reviewed, it can be merged into the main branch.



