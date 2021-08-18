xcopy Executable\ParamFileEditor.exe \\proto-2\PAST\Software\Sequest_Param_File_Editor\ /Y /D
xcopy Executable\ParamFileEditor.xml \\proto-2\PAST\Software\Sequest_Param_File_Editor\ /Y /D
xcopy Executable\ParamFileGenerator.dll \\proto-2\PAST\Software\Sequest_Param_File_Editor\ /Y /D
xcopy Executable\ParamFileGenerator.xml \\proto-2\PAST\Software\Sequest_Param_File_Editor\ /Y /D
xcopy Executable\PRISM.dll \\proto-2\PAST\Software\Sequest_Param_File_Editor\ /Y /D
xcopy Executable\ParamGenTest.exe \\proto-2\PAST\Software\Sequest_Param_File_Editor\ /Y /D
xcopy Executable\ParamGenTest.xml \\proto-2\PAST\Software\Sequest_Param_File_Editor\ /Y /D
xcopy Executable\*.dll \\proto-2\PAST\Software\Sequest_Param_File_Editor\ /Y /D


xcopy Executable\*.dll F:\Documents\Projects\DataMining\DMS_Managers\Analysis_Manager\AM_Common /Y /D

@echo off
echo.
echo Files are now at \\Proto-2\PAST\Software\Sequest_Param_File_Editor\
echo Also copied to   F:\Documents\Projects\DataMining\DMS_Managers\Analysis_Manager\AM_Common
pause