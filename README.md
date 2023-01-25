# ArtCore Editor
ArtCore Editor is main tool to create games in ArtCore Engine. Manage assets, create scenes, and create objects. Current versions compile games only for Windows; later Linux and Android will be added. <br>
To see more about ArtCore Engine, go to the engine repository.
Demo game projects are included in the Release tab. Tutorial how to create simple game is aweable in tutorial.md<br>
<hr>
## ArtCore
ArtCore is a 2D game engine, currently for Windows, later for Android and Linux. The whole system consists of 3 components: engine, editor and compilator. For more information about engine or compilator go to their repositiries.<br>

## Compilation
ArtCore Editor is created in Visual Studio 2022. There is no cmake file to create new projects. <br>
The editor downloaded from the release tab is ready to run. For clean compilation, just clone the code and open "ArtCore Editor.sln." After first run application ask for Core.tar file.
You can download it from the release tab or create your own core file from the ArtCore Engine compilation. Core.tar contains the engine executable, the ArtScript library, and basic files like fonts or shaders, as well as ArtCompiler. <br>
Without the basic engine files, an editor can open game projects but cannot run or release them.

## Requirements
Editor requires .Net 6.0 to run. You can download it from [here](https://dotnet.microsoft.com/download/dotnet/6.0).<br>
NuGet-packed VLC is included; it`s used by sound managers.

## Run
To run the game, you need to compile it. To do this, go to the "Game" menu and select "Release" or use the arrow buttons (blue is release and red is debug).
Aditional to run game you need to have ArtCore Engine executable and compiller.All nessesary files are included in Core.tar file. 
You can download it from the release tab or create your own core file from the ArtCore Engine compilation. <br>

# Editor

## Technical and Game Archives
When the editor tries to compile a game, it creates two archives: technical and game.
The technical archive contains all files needed to run the game, like compiled scripts, the ArtScript library, shaders, fonts, and object and scene definitions.
The game archive contains all game assets. Both archives are compressed with the LZ4 algorithm.
When game is compiled, editor create folder with game name in "Releases" folder. In this folder are 2 archives and game executable.<br>
In debug mode, game archives are created in the "Output" directory.

## Main Window
The main window is divided into two parts: left and center. Left part is used to manage assets, objects and scenes, center part is used to output game console.<br>
On upper part of main window are buttons:
* Project <- new, save, load project and Update Core (Core.tar)
* Game <- tests and compilation, ArtCore settings, starting scene, and release options.
* Other buttons to create new assets, objects, and scenes
On the right side are buttons to compile and run the game in release or debug mode. <br>

## Assets
Assets are files used in games. They are divided into four categories: textures, sprites, sounds, and fonts.
* Textures are images used in game. They can be drawn on a screen.
* Sprites are animate textures. They are used to create animations. Sprites can have masks to detect collisions or click events.
* Sounds are audio files used in game. They can be played in the game or at a specific position (3D audio).
* Music are long audio files. They can be played in games.
* Fonts are used to draw text on screen. They can be used to draw text on the screen.
Prefered file types are PNG for textures/sprites, ogg/wav for sounds/music and ttf for fonts<br>
To add a new asset, go to the "Assets" menu and select "New" or right-click on the list and select "New." <br>
To edit asset right click on list and select "Edit" or double click name.<br>
To delete asset right click on list and select "Delete".<br>

## Objects (Instances) aka Entity
> Every game engine has another name for objects. In Unity, they are called "prefabs" in Unreal Engine "blueprints" in Godot "nodes," and in ArtCore "objects" <br>
Every object has a definition to create new instances. They are used to create new objects in the game called "instances." <br>
The object manager window is divided into three parts: left, center, and right. The left part is used to manage object properties like body variables or sprites.
The center part is used to add and edit events. The right part is used to manage scripts for selected events.
Scripts can be written in the ArtScript language or selected from a list of basic scripts. <br>
Behaviors are repeatable scripts that can be shared between objects. <br>
Editor placement commands like "show in..." are used to enable object placement in a scene or level editor. <br>
Variables with a defined default value are assigned when the scene is loaded. The execution order of events is described in the ArtCore Engine repository. <br>
Three buttons on the right may be confusing. Save button save object but not close window. Apply saves and close window. Cancel close window without saving.<br>

## Scenes
Scene is a collection of objects. They are used to create levels in the game. The scene manager window is divided into two parts, left and right.
The left part is used to manage scene properties like view dimensions, background, and the list of available instances. The right part is used to manage objects in the scene. <br>
To add new object to scene drag and drop it from left part to right part. To delete an object from the scene, right-click on it and select "Delete".<br>
Upper buttons are:
* Editor <- Change the grid size and show or  hide the grid. Objects are snapped to the grid by default.
* Settings <- Scene variables and scene triggers Like in objects but triggers must be called by other objects except starting trigger, that is executed on scene load.
* Scene Gui: interface system, schema, and triggers
* Levels <- These are levels for the target scene. They are used to create levels in the game. They are not required to create games. <br>
Important! Objects spawned in a scene are always spawned on the starting level, but level objects are only spawned if the target level is active.
To better understand how levels work, see the demo game project. Scene is for system objects like players, world borders, etc. Levels are for game objects like enemies, items, etc. <br>

## Compilation process
For now editor not compile entire game executable, only use compiled version so game is universal beetwen game projects.<br>
The compilation process is divided into steps:
* Project are saved.
* Editor create technical archive with all platform settings.
* Assets are compare with last compilation and only changed are added to game archive.
* Sprites are always repacked.
* Editor use ArtCompiler to compile all scripts to ArtScript bytecode.
* First objects, later scenes.
* The editor creates a game archive with all assets and compiled objects and scenes. Ready to run game.
