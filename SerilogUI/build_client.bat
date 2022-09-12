@ECHO OFF

cd ../SerilogUI.Client && yarn build && xcopy ..\\SerilogUI.Client\\dist\\assets .\\assets\\ /s /y