# BRAZEN READ ME
## Basic Game Requirements
Brazen is a first-person Unity game where the player wanders a labryinthe in search of a key in order to escape. Within the labryinthe is the wandering Brass Minotaur, who the player must avoid or else risk death. The Player can also find spools of golden twine to leave themselves paths and indications of where they have been in the labryinthe, helping them navigate. The Player Wins by finding the key and making it back to the Gate, the Player Loses if the Minotaur catches them. 
The Minotaur wanders the labryinthe and responds to both visually seeing the player, as well as a limited range of hearing, and charges at the player when having spot them. If the Minotaur catches the Player, the Player is killed and forced to restart. 
Brazen contains music, several sound ques and sound effects for the Minotaur, items, and the player. 
## Art Requirements
Every item and texture found in Brazen was 100% created by both artists, Lee and Daelyn. This includes but is not limited to: walls and floors, pillars, the Key, balls of twine, The Minotaur, Notes, Sconces, gates, Corpses, etc. All UI assets, props and base Minotaur and Corpse Sculpts were made by Lee, all animations and major texturing work were done by Dae. The Minotaur has several animations, including: Idle, Patrol Walk, Player Spotted, Spotted Chargeup, and Charge. Dae also implemented the moving Corpse animations. 
Fire and Golden Twine Particles were made and implemented by Dae. 
## Code Requirements
All code was implemented by Jazmine Ashley. 
The Minotaur utilizes NavMesh to navigate the Labryinthe, and a detection radius in order to "hear" the Player's footsteps when they choose to sprint, as well as spot the player when in line of sight. This enables the Minotaur to stay relatively close by to the Player once out of the Starting Area, and to chase when the Player is in sight. 
The game is pausable when hitting the ESC key, which displays the Pause menu. From there the Player can navigate to Options, Quit the Game, or Restart. Within the Options menu are the abilities to enable Slow Bull Mode, and Infinite Twine Mode. 
Our main mechanic is the Golden Twine, which serves as a pickup the player can use to navigate and find their way through the maze.
## Tech Requirements
There are three distinct areas for the Player, one of which the Minotaur should never be able to enter at the beginning of the game. The Starting Area contains the Gate, which is where the Player will need to return to once aquiring the Key. The Labryinthe is the maze that both the Player and the Minotaur will wander, and then the Key Room is where the Key can be found. 
Brazen contains a Main Menu with all required elements: "Start", "About", "Options", and "Exit Game". It also contains a Pause menu with the same access to Options, How to Play, and Quitting. Brazen also contains distinct Win and Lose screens. 
UI and Animation Controllers were implemented by Luis. Level layout, prop layout, Minotaur management, Sounds, Particles, and other elements were implemented by Shawn.  
## Other Game Elements
- The team worked together creating a prototype maze and the final maze, as well as placing elements such as items, props, story elements, etc.
- The team worked together on managing level design, tracking changes, managing the github, and finding+implementing sounds, effects, inputs, packages, etc. 
