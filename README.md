# Haunted Forest Generator

## Map Generator Design:
1. Generate the grid of the non-walkable canopy area in the forest according to the set random fill rate.
2. Use the Cellular Automata algorithm for 7 times smooth (inspired by Sebastian Lague: https://www.youtube.com/watch?v=v7yyZZjF1z4&t=739s).
3. Fill the canopy area using rule tiles with preset fill rules and collision information.
4. Repeat steps 1 and 2 to generate the ground terrain, and then fill the ground area with rule tiles of soil and grass.
5. Generate NavMesh dynamically using the stored canopy area information.

## Agents Design:
The overall use of NPBehave and NavMesh related libraries, referring to the code structure in the Lab to write the behavior tree, but did not directly use the code in the Lab.
Adventurer: Wander randomly on the map, if it is close to the Forest Spirit without getting the Treasure, it will run in the opposite direction, if it is close to the Treasure and not close to the Forest Spirit, it will go to pick up the Treasure. The Adventurer who has obtained the Treasure can attack when approaching the Forest Spirit.
Forest Spirit: Wander randomly on the map, if it is close to the Adventurer who has not obtained the Treasure, it will attack, if the other party has the Treasure, it will run in the opposite direction.

## Features:
1. Clear and correct behavior tree structure, most different types of operations are decoupled into different scripts.
2. Added color changes with state design for Agents, making behavior more visualized.
Color represents the status of each object:
Treasure - Always golden
Adventurer - usually blue, turns white when being chased, turns red after acquiring a treasure
Forest spirit - usually pink, turns white when being chased, and turns red when initiating a chase

## Demonstration Video Link:
https://www.youtube.com/watch?v=tJFm1yZGHZ0
