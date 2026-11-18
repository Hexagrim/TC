# The Cabin (TC)

---

It's a game about escaping a house while being hunted by a scary monster.

<p align="center"><i>(very original idea**)</i></p>

The player gets **1 life** to find, explore and solve all the puzzles to escape the house before the monster eats them and their speaker.

## Controls

| Key | Action |
|:---:|:---|
| `WASD` | MOVE |
| `F` | interact |
| `E` | pick and throw |
| `LMB` | to use torch |
| `SHIFT` | toggle crouch |

## Tools used

-Unity 6
-Blender
-Photoshop 2026
-Assets from SketchFab and Itch
-Animations from Mixamo

** This was my first proper 3d game, so bugs are expected. There is a problem with web building being that the APV(Adaptive probe volumes) are not supported while the entire main scene depends on it. So lighing is bugged for web build.**

## Cool things i like about this game:

- I used a class derived way of handling the item classes and the interactable classes which is pretty cool
- The game uses probe volumes which support baked GI and its so cool cause it even works with moving objects
- The monster is a cool ting, first time i made smthing like this

## Problems i had?? *(turns out i need to elaborate this too)*

- Enemy movement was too choppy but it was fixed with some map building
- DRAWERS. Turns out making drawers to hold physics based items is ALMOST IMPOSSSIBLE but i fixed it by making the item move along if it triggers a container collider
- Doors. ENEMY JUST CLIPS THROUGH IT. I fixed it by just adding enemy ray check and calling a function on the door to open and to close after a time
- LIGHTING looked ass every time. Was fixed when i finally used APV to make probe lighting and with some GI it looks gooooood

## BUILD:
- just download and plug it into a unity 6 editor and just build it or modify however you want

## Installation:
Download from [itch page](https://hexagrim.itch.io/tc) and extract the zip file. Then place it in any desired directory and just use the EXE file. (THERE WILL BE A SAFESCREEN MENU about "might be harmful", you can ignore this. If you're concerned feel free to run a virus check on the zip.)

-Note

*The game doesn't include output of any GEN-AI*

*Any modifications to this project under personal and commercial use of all form are allowed without any caveats*


---
