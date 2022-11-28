
@echo prepare output dir
set outputDir="D:\projekt\ArtCore Editor\bin\Core
rmdir /Q "D:\projekt\ArtCore Editor\bin\Core\"

@echo prepare core
mkdir %outputDir%"
copy D:\projekt\ACompiler\x64\Release\ACompiler.exe  %outputDir%\ACompiler.exe"
copy D:\projekt\AScript.lib  %outputDir%\AScript.lib"

@echo prepare game exe
mkdir %outputDir%\bin_Release"
mkdir %outputDir%\bin_Debug"
	copy "D:\projekt\ArtCore\x64\Release\ArtCore.exe"  %outputDir%\bin_Release\ArtCore.exe"
	copy "D:\projekt\ArtCore\x64\Release\*.dll"  %outputDir%\bin_Release"
	
	copy "D:\projekt\ArtCore\x64\DebugEditor\ArtCore.exe"  %outputDir%\bin_Debug\ArtCore.exe"
	copy "D:\projekt\ArtCore\x64\DebugEditor\*.dll"  %outputDir%\bin_Debug"
		
@echo prepare shaders
mkdir %outputDir%\shaders"
	copy "D:\projekt\ArtCore\pack\shaders\*.*"  %outputDir%\shaders"

@echo prepare files
mkdir %outputDir%\pack"
	copy "D:\projekt\ArtCore\pack\files\*.*"  %outputDir%\pack"
	
@echo prepare gui-bulider
mkdir %outputDir%\gui-bulider"
		xcopy /E /Y "C:\Users\atrox\Desktop\TGUI-0.10\out\gui-builder\Release" %outputDir%\gui-bulider"
	
@echo prepare FileList	
cd %outputDir%"
ls -R > FileList.txt		

cd ..
tar -cf "Core.tar" "Core"