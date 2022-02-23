## Description

Wow kind of camera + controls for player. Consists of 4 types of Camera and 4 types of Camera adjustment mode (commented in CameraController script).
Features:

-   Movement
-   Jumping
-   Speed correction on ramps
-   Slope sliding down
-   Keybinds
-   Camera Controls with mouse
-   Smooth Camera movement
-   Camera collisions with environment

To be added:

-   Swimming
-   Flying
-   Ladder Climbing
-   Wall jumping

## Uses/Usage

### How To

-   (Easy: Using Prefabs)

    -   Drag Player Prefab to scene and CameraRig (Player should have a ref to camera and vice-versa) and disable any other existing camera in the scene.
    -   Attach references between the two dragged prefabs (in their scripts)

-   (Custom: Attaching Scripts) To adapt scripts to already existing Camera and Player gamobjects:
    -   Attach CameraController to the Camera gameobject and create a nested gameobject with the exact same transform to be the 'Tilt' ref in the script.
    -   Attach PlayerController to the Player gameobject. Create two nested gameobjects for 'GroundDirection' and 'FallDirection' and attach them to script. (Optional) Attach camera ref (previously created/existing one). Set up the 'Controls'.
    -   Reference Player gameobject in CameraController.

### Behaviour

-   All scripts are wrapped inside namespace BaseCameraControlsWOW
-   Usage of Control and ControlBinding classes to map inputs is required. Alternatively, you can use your own input configuration.
-   (!) CameraController script searches for Player script in scene if no ref is given to it.

## Dependencies

-   ProBuilder

## Exported

-   Package 22/11/21, following tutorial collection until [video 10](https://www.youtube.com/watch?v=Rohul_FU1vQ&list=PLffw8tfWPU2PZvX4o4r-u4O9purAbgcfQ&index=11) of Soupertrooper.
