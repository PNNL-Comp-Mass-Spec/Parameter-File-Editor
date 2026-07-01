# Parameter File Generator

The parameter file generator DLL was historically used by the [Analysis Manager](https://github.com/PNNL-Comp-Mass-Spec/DMS-Analysis-Manager) 
in DMS to generate SEQUEST parameter files using param file settings stored in the DMS database. 
* The current iteration of the DLL copies the parameter file for a given analysis job from the parameter file Windows file share to the local working directory.

# DMS Parameter File Generator

When the parameter file generator is compiled using Visual Studio, a program for manually creating or obtaining parameter files is created; 
see `ParamGenTest.exe` in directory `ParamGenTest\bin`.
* The DMS Param File Generator has a GUI interface for selecting the parameter file type (MS-GF+, MaxQuant, FragPipe, etc.), the parameter file to create or retrieve, and the output directory.

![DMS Parameter File Generator](https://github.com/PNNL-Comp-Mass-Spec/Parameter-File-Editor/raw/master/docs/DMS_Param_File_Generator.png)

# Contacts

Written by Matthew Monroe for the Department of Energy (PNNL, Richland, WA) \
E-mail: matthew.monroe@pnnl.gov or proteomics@pnnl.gov \
Website: https://github.com/PNNL-Comp-Mass-Spec/ or https://www.pnnl.gov/integrative-omics

## License

The Peptide Hit Results Processor is licensed under the 2-Clause BSD License;
you may not use this program except in compliance with the License. You may obtain
a copy of the License at https://opensource.org/licenses/BSD-2-Clause

Copyright 2026 Battelle Memorial Institute
