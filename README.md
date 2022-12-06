# ArtCore Editor
## This is editor to create games working in ArtCore Engine.
# How to run this?
1. Clone Editor code, compile / Download ArtCoreEditor_binx64
2. Download Core.tar [!] do not unpack this file <- this is binary pack from ArtCore release 0.6.0
3. Run ArtCore Editor
4. Create new project or download and unzip test_game.zip <- open 'Project.acg' with ArtEditor
5. Click  ![plbtn](https://user-images.githubusercontent.com/5814733/205962203-646bf87c-2777-4d2d-b783-6e951fad7b4d.png) to play game in debug mode.

# Features
* Add custom assets like:
* texture <- png image, can be draw into screen
* sprite <- animate set of textures, can be transparent, have collision mask, custom image center
* music <- long files for music in game (vaw or ogg)
* sounds <- short wav files for sound in game
* fonts <- custom fonts to draw on screen
* scenes <- scene is playground for Your objects(instances) to play. For now You can place, move or delete objects from scene. Change background to solid color or texture
* instances <- ths is main game objects. Create, add sprite, body (for collision) and custom variables used in events.
## Event system
In scene Every living instance recive events like OnMouseDown, OnCollision, EnDraw etc... When event is hiited code from this instance is executed. How to code? First way is click in Script editor, just select category and answer questions by click on links. When You done proper code is insert to event. You can write custom code too. Game engine use ArtCode, custom scripting language (ArtCompiller translate this to binary file and can tell when and where You make mistake in code).
