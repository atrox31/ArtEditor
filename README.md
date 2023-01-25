# ArtCore Editor
ArtCore Editor is main tool to create games in ArtCore Engine. Manage assets, create scenes and objects. Current wersion compile games only for Windows, later Linux and Android.<br>
To see more about ArtCore Engine go to engine repository.
<hr>
## ArtCore
ArtCore is 2D game engine, currently for Windows, later Android and Linux. The whole system consists of 3 components: engine, editor and compilator. For more information about engine or compilator go to their repositiries.<br>

## Compilation
ArtCore Editor is created in Visual Studio 2022. There is no cmake file to create new projects.<br>
Editor downloaded from release tab is ready to run. For clean compilation just clone code and open "ArtCore Editor.sln". After first run application ask for Core.tar file.
You can download it from release tab or create own core file from ArtCore Engine compilation. Core.tar contains engine executable, ArtScript libray and basic files like font or shaders and ArtCompiler.<br>
Without basic engine files editor can open game projects but can not run or release it.
