xcopy ParamFileEditor\bin\Debug\ParamFileEditor.exe \\proto-2\PAST\Software\Sequest_Param_File_Editor\ /Y /D
xcopy ParamFileEditor\bin\Debug\ParamFileEditor.xml \\proto-2\PAST\Software\Sequest_Param_File_Editor\ /Y /D
xcopy ParamFileEditor\bin\Debug\*.dll               \\proto-2\PAST\Software\Sequest_Param_File_Editor\ /Y /D
xcopy ParamFileEditor\bin\Debug\ParamFileEditorSettings.xml \\proto-2\PAST\Software\Sequest_Param_File_Editor\ /Y /D

xcopy ParamGenTest\bin\Debug\ParamGenTest.exe       \\proto-2\PAST\Software\Sequest_Param_File_Editor\ /Y /D
xcopy ParamGenTest\bin\Debug\ParamGenTest.xml       \\proto-2\PAST\Software\Sequest_Param_File_Editor\ /Y /D
xcopy ParamGenTest\bin\Debug\ParamFileGenerator.dll \\proto-2\PAST\Software\Sequest_Param_File_Editor\ /Y /D
xcopy ParamGenTest\bin\Debug\ParamFileGenerator.xml \\proto-2\PAST\Software\Sequest_Param_File_Editor\ /Y /D
xcopy ParamGenTest\bin\Debug\*.dll                  \\proto-2\PAST\Software\Sequest_Param_File_Editor\ /Y /D

xcopy ParamFileGenerator\bin\Debug\*.dll F:\Documents\Projects\DataMining\DMS_Managers\Analysis_Manager\AM_Common /Y /D

@echo off
echo.
echo Files are now at \\Proto-2\PAST\Software\Sequest_Param_File_Editor\
echo Also copied to   F:\Documents\Projects\DataMining\DMS_Managers\Analysis_Manager\AM_Common
pause