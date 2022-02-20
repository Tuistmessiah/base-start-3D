## Features

-   Layout optimal with separate file explorer
-   Pro Builder placeholder arena

## Folder organization

-   "\_3rdParty" to import/move new packages (use Package2Folder)
-   "\_Core" to place all game folders/objects
-   "Plugins", default folder for some packages (including Package2Folder)

## Project Settings/advantages

-   Player/Other Settings/Color Space: Linear
-   Editor layout with most important windows

## Plugings/Libraries

-   DebugManager

## Dependencies/Packages

-   ProBuilder
-   ProGrid
-   Package2Folder (in Plugins folder)
-   TextMeshPro (comes with Editor)

### Not installed but handy

-   Cinemachine
-   Visual Scripting
-   Editor Console Pro
-   Rainbow Folders 2
-   Search&Replace
-   MonKey
-   DOTS Character Controler !!!

## Tips

-   Execute on static on start: \[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)\] as a decorator
-   Create SO: \[CreateAssetMenu(fileName = "New Vfx Combo", menuName = "VfxManager/Vfx Combo")\] and extend ": ScriptableObject"
-   Add a menu/context menu function call: \[MenuItem("MyMenu/Do Something")\] and \[ContextMenu("Do Something")\]
-   Decorator /[System.Serializable/] on classes and structs
-   Get random value: bool variable = (Random.value > 0.5f)
-   More performant distance: (pointA - pointB).sqrMagnitude < dist \* dist
-   More performant: other.CompareTag("tagName")
-   Use structs more often (not classes)
-   \[SeriableField\] and \[HideInInspector\]
-   Style Debug.Log "<color=red>red message</color>"
-   Add shortcuts (by dragging from explorer) to unity
-   Select object + F to focus object (F two times to follow)
-   Ctrl + Shift + F: Set camera to scene view
-   Layers/tags can be subdivided by writing: Layer Folder/Layer Option
-   Shift + Space to maximize window
-   Check variable progression: public AnimationCurve plot = new AnimationCurve(); plot.addKey(Time.realtimeSinceStartup, value);
-   Wait for real seconds: yield return new WaitForSecondsRealtime(1f)
-   Never use Camera.main!
-   static readonly string bla = "bla" to avoid using memory every time
-   Inspector quality of life: \[Range(0f, 10f)\], \[Space\], \[Header("Settings")\], \[Tooltip("Some tooltip")\]
-   Loop children: foreach(Transform child in transform) {}
-   Pause editor: EditorApplication.isPaused = true;
-   Profile code part: Profiler.BeginSample("something); ... Profiler.EndSample();

## Author

Pedro Caetano
