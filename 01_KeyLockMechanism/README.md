# Key Lock Mechanics

This challenge was suggested by "kaj-S#7493"

Time spent: 7 Hrs

Experiment and download the project zip [_01_KeyLockMechanism.zip](_01_KeyLockMechanism.zip) if you want

Unity Version: 2019.4.25f1

Final Result
![Game](Images/KeyLock.gif)


## Idea
I had a lot of time on Saturday. So, I wanted to make something playable and fun. I thought of platformer holding keys and openeing doors then I thought of making the sky rain keys.

## Execution
### The Art
For the art, I borrowed [Kenneys great asset](https://kenney.nl/assets/pixel-platformer) since it had keys in it and I initally wanted to make a platformer.

Created the map using Unity's tilemap system.

### Coding
I have been practicing lately the data orineted flow. Where the game logic is completley separated from the actual graphics. This concept (for me) can be applied to enclosed systems that can be separeted from graphics.

If you checked the [Scripts folder](Assets/Scripts) folder, you can see that there is a `GameLogic` and a `RuntimeLogic`.

The game logic is just the pure data with events (no monobehaviours) and it can be hooked to whatever graphics doing whatever they want. The logic that:
- Creates specified number of locks with random ids
- It creates a key every 0.25 seconds if the keycount is less than specified key count
- Locks will be unlcoked after "specific" number of keys are used
- There is a function that unlcoks (or test) if the key can unlock a Lock
- Keys and Locks do have ids and that's it.

In the RuntimeLogic, the GameManager creates an instance of the GameLogic and update everything else accordingly.

The RuntimeLock has a collider and when a key collides with it, it calls the check collision on the GameManager which plays the audio if the key was sucessfully used.

There are multiple events like OnGameEnded, OnKeyDestroyed, OnKeyCreated and so in the GameLogic that the other componenets listen to it in order to work.

In the current setup, I had only 9 locks places, so I made number of locks a variable that ranges from 1 to 9 that can be adjusted at the beginning of the game. (The game logic can handle as many locks and it won't matter, but the current pixel art game and the space only allows 9 places at the bottom of the screen)
![LockCount](Images/CustomLocks.jpg)


Finally, the color was supposed to be a random hue based on the id but the color combination didn't turn out good because the key was already yellow.

Feel free to use/modify the code as you like and let me know if you have any questions. (Mickey#5544 on discord)

