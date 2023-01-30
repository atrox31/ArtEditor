# How to create simple game in ArtCore Engine
Assets used in project: [assets.zip](https://github.com/atrox31/ArtEditor/files/10538369/assets.zip)
<hr>

Game created in this tutorial is: bounce the ball off the tiles and knock them all off the board. When the title is destroyed they can give a bonus or new ball.<br>
![screen](https://user-images.githubusercontent.com/5814733/215568193-9790483c-38b7-465d-9db9-e607d87b9d86.png)

## Create new project
1. Open editor, first click on "Create new Project" or select last opened project if exists
2. Select path of new project, last empty folder are project name
3. Select target platform, Windows
4. Select "Import standard main menu"
5. Click "Create"
6. Now in the project settings do not change anything. Options with AC are engine settings, SDL_ is sdl2 settings flags and SDL_GL is opengl settings

## Adding assets
1. Unzip assets.zip or prepare own assets.
2. In upper menu click Sprite->New
3. In the name type "spr_ball". Best practice is to write an index in front of the asset type.
4. Click Import and select ball.png
5. If Editor ask for clean image list type Yes
6. Now select the sprite center as Center, this is the pivot point where sprites are rendered on object
7. Sprite mask circle: 28 Radius.
8. You can select "Show" to see where is center of sprite and how big is mask 
9. Click Apply
10. Now add sprite for player
11. Name "spr_player", image: "player.png", Center: center, Mask: Rect 128x30 ->Apply
12. Name "spr_star", image: "star.png", Center: center. Mask: Circle: 25 Radius
13. Now for titles: Name "spr_title", but on Import select files from "title.png" to "title6.png" to add 6 frames of animation, this will be used for title variations.
14. Center: center, Mask: Rect (square) 62x62
> Why mask is always smaller than sprite?<br>
> Because sprites have faded to opacity effect on borders and to make better visual collisions we crop some of the image.

## Adding music and sound
All files starting with "snd_" add as sound, "mus_main_menu" add as music
If You not type any asset name Editor put there filename without extension so just add new assets
> Right-Click on asset name in left list to add new element faster or delete if needed.

## Instances
We need: titles, ball, player, bonus and walls.<br>
1. First let's create invisible walls so the ball doesn't fall out of the screen.
2. Create "obj_wall_vertical"
3. Sprite set as <default> then means no sprite.
4. Editor placement: Show in scene. This instance is always present, regardless of the level.
5. Body type: do not select anything, We create body from code.
6. Now Events, click on "Add Event" and select "EvOnCreate".
Select: Code and type:
```
//EvOnCreate
// move instance to vertical scene center
move_instant(new_point(get_pos_x(), math_div(convert_int_to_float(scene_get_height()), 2)))
// set the body to cover the entire edge of the screen
instance_set_body_as_rect(32, scene_get_height())
```
Hit Apply, next is "obj_wall_horizontal". All properties is the same but code is slightly different:
```
//EvOnCreate
// move instance to horizontal  scene center
move_instant(new_point(math_div(convert_int_to_float(scene_get_height()),2),get_pos_y()))
// set the body to cover the entire edge of the screen
instance_set_body_as_rect(scene_get_width(), 32)
```
> Why move_instant? Why I can not place it myself?<br>
> We can place instance 'by hand' but when scene have different size or we miss place wall not be covered all of scene edge. This is a safe option.
Next is "obj_player":
1. Set Sprite as spr_player
2. Do not check placement, the player will be spawned by script
3. Check "Have body" and select "Sprite mask" <- this will copy body data from sprite mask, as the player grows from the bonuses his collision mask will always coincide with the picture.
4. Add variable: name: "scale", Type: "VTypeFloat", Default: "1.0" <- this will grow when the player collects bonuses
5. Add Events:
EvOnCreate: 
  <br>
  WORK IN PROGRESS
