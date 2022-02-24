## Description

The prefab "DebugManager", when placed in the scene, will create a singleton "Config". After this happens (on Awake), the static methods inside namespace DebugManager will become available to use in the project.
The contents are "Lines", "Prisms", "Spheres" and "Logging", each with a collection of overloads to make debugging easier and less code intrusive.

The "Test" script can be (de)attached to the scene to show some display examples (by default, it is attached already to the prefab DebugManager but it can be removed).

## How to

Drag prefab DebugManager to scene and import, by "using DebugManager;", in the script you need, to have access to the namespace methods. In your dragged prefab, change the parameters of Config to your liking.

## Author

Pedro Caetano
