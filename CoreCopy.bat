del /Q "D:\projekt\ArtCore Editor\bin\Release\Core\*.*"
del /Q "D:\projekt\ArtCore Editor\bin\Debug\Core\*.*"

mkdir "D:\projekt\ArtCore Editor\bin\Debug\Core\"
mkdir "D:\projekt\ArtCore Editor\bin\Release\Core\"

	copy D:\projekt\ACompiler\x64\Release\ACompiler.exe  "D:\projekt\ArtCore Editor\bin\Debug\Core\ACompiler.exe"
	copy D:\projekt\ACompiler\x64\Release\ACompiler.exe  "D:\projekt\ArtCore Editor\bin\Release\Core\ACompiler.exe"

	copy D:\projekt\AScript.lib  "D:\projekt\ArtCore Editor\bin\Debug\Core\AScript.lib"
	copy D:\projekt\AScript.lib  "D:\projekt\ArtCore Editor\bin\Release\Core\AScript.lib"

mkdir "D:\projekt\ArtCore Editor\bin\Debug\Core\bin\"
mkdir "D:\projekt\ArtCore Editor\bin\Release\Core\bin\"

	copy D:\projekt\ArtCore\x64\Release\*.dll  "D:\projekt\ArtCore Editor\bin\Release\Core\bin"
	copy D:\projekt\ArtCore\x64\Debug\*.dll  "D:\projekt\ArtCore Editor\bin\Debug\Core\bin"

	copy "D:\projekt\ArtCore\x64\Release\ArtCore.exe"  "D:\projekt\ArtCore Editor\bin\Release\Core\bin\ArtCore.exe"
	copy "D:\projekt\ArtCore\x64\Debug\ArtCore.exe"  "D:\projekt\ArtCore Editor\bin\Debug\Core\bin\ArtCore.exe"

mkdir "D:\projekt\ArtCore Editor\bin\Debug\Core\shaders\"
mkdir "D:\projekt\ArtCore Editor\bin\Release\Core\shaders\"

	copy D:\projekt\ArtCore\*.vert  "D:\projekt\ArtCore Editor\bin\Debug\Core\shaders\"
	copy D:\projekt\ArtCore\*.frag  "D:\projekt\ArtCore Editor\bin\Debug\Core\shaders\"
	copy D:\projekt\ArtCore\*.vert  "D:\projekt\ArtCore Editor\bin\Release\Core\shaders\"
	copy D:\projekt\ArtCore\*.frag  "D:\projekt\ArtCore Editor\bin\Release\Core\shaders\"

mkdir "D:\projekt\ArtCore Editor\bin\Debug\Core\pack\"
mkdir "D:\projekt\ArtCore Editor\bin\Release\Core\pack\"

	copy D:\projekt\ArtCore\gamecontrollerdb.txt  "D:\projekt\ArtCore Editor\bin\Debug\Core\pack\gamecontrollerdb.txt"
	copy D:\projekt\ArtCore\gamecontrollerdb.txt  "D:\projekt\ArtCore Editor\bin\Release\Core\pack\gamecontrollerdb.txt"

	copy D:\projekt\ArtCore\TitilliumWeb-Light.ttf  "D:\projekt\ArtCore Editor\bin\Debug\Core\pack\TitilliumWeb-Light.ttf"
	copy D:\projekt\ArtCore\TitilliumWeb-Light.ttf  "D:\projekt\ArtCore Editor\bin\Release\Core\pack\TitilliumWeb-Light.ttf"
	
mkdir "D:\projekt\ArtCore Editor\bin\Debug\Core\gui-bulider\"
mkdir "D:\projekt\ArtCore Editor\bin\Release\Core\gui-bulider\"

	xcopy /E /Y "C:\Users\atrox\Desktop\TGUI-0.10\out\gui-builder\Release" "D:\projekt\ArtCore Editor\bin\Debug\Core\gui-bulider"
	xcopy /E /Y "C:\Users\atrox\Desktop\TGUI-0.10\out\gui-builder\Release" "D:\projekt\ArtCore Editor\bin\Release\Core\gui-bulider"