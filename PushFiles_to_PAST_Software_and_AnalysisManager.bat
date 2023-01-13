xcopy ParamGenTest\bin\Release\ParamFileGenerator.dll \\proto-2\PAST\Software\Sequest_Param_File_Editor\ /Y /D
xcopy ParamGenTest\bin\Release\ParamFileGenerator.xml \\proto-2\PAST\Software\Sequest_Param_File_Editor\ /Y /D
xcopy ParamGenTest\bin\Release\PRISM.dll \\proto-2\PAST\Software\Sequest_Param_File_Editor\ /Y /D
xcopy ParamGenTest\bin\Release\ParamGenTest.exe \\proto-2\PAST\Software\Sequest_Param_File_Editor\ /Y /D
xcopy ParamGenTest\bin\Release\ParamGenTest.xml \\proto-2\PAST\Software\Sequest_Param_File_Editor\ /Y /D
xcopy ParamGenTest\bin\Release\*.dll \\proto-2\PAST\Software\Sequest_Param_File_Editor\ /Y /D


xcopy ParamFileGenerator\bin\Release\*.dll F:\Documents\Projects\DataMining\DMS_Managers\Analysis_Manager\AM_Common /Y /D

@echo off
echo.
echo Files are now at \\Proto-2\PAST\Software\Sequest_Param_File_Editor\
echo Also copied to   F:\Documents\Projects\DataMining\DMS_Managers\Analysis_Manager\AM_Common
pause