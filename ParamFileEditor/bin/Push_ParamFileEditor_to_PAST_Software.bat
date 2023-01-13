xcopy Release\ParamFileEditor.exe \\proto-2\PAST\Software\Sequest_Param_File_Editor\ /Y /D
xcopy Release\ParamFileEditor.xml \\proto-2\PAST\Software\Sequest_Param_File_Editor\ /Y /D
xcopy Release\ParamFileGenerator.dll \\proto-2\PAST\Software\Sequest_Param_File_Editor\ /Y /D
xcopy Release\ParamFileGenerator.xml \\proto-2\PAST\Software\Sequest_Param_File_Editor\ /Y /D
xcopy Release\PRISM.dll \\proto-2\PAST\Software\Sequest_Param_File_Editor\ /Y /D
xcopy Release\*.dll \\proto-2\PAST\Software\Sequest_Param_File_Editor\ /Y /D

@echo off
echo.
echo Files are now at \\Proto-2\PAST\Software\Sequest_Param_File_Editor\
pause