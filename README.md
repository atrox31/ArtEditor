# ArtCore Editor
## This is an editor to create games working in ArtCore Engine.
# How to run this?
Download release ArtCoreEditor_binx64, unzip and run editor<br>
Or<br>
1. Clone ArtEditor
2. Download Core.zip, unpack to Core.tar (do not unpack future) this is binary pack from compatible ArtCore release
<br>Or run script from ArtCore repository to create new Core.tar.
3. Run ArtCore Editor
4. Select Core.tar when Editor ask to
<br>
Create new project or download and unzip test_game.zip <- open 'Project.acg' with ArtEditor<br>Click this button to play game in debug mode.
<img src="https://user-images.githubusercontent.com/5814733/206944317-78709c1e-a0ef-43c6-9b44-aeecc667482e.png" /><br>

# Features
* Add custom assets like:
* texture <- png image, can be draw into screen
* sprite <- animate set of textures, can be transparent, have collision mask, custom image center
* music <- long files for music in game (wav or ogg)
* sounds <- short wav files for sound in game
* fonts <- custom fonts to draw on screen
* scenes <- scene is a playground for Your objects(instances) to play. For now You can place, move or delete objects from the scene. Change background to solid color or texture
* instances <- this is the main game object. Create, add sprite, body (for collision) and custom variables used in events.
## Event system
Every living instance receives events like OnMouseDown, OnCollision, OnDraw etc... When an event is hitted code from this instance is executed. How to code? First way is to click in the Script editor, just select the category and answer questions by clicking on links. When You do, proper code is inserted to the event. You can write custom code too. Game engine uses ArtCode, a custom scripting language (ArtCompiller translates this to a binary file and can tell when and where You make a mistake in code).
