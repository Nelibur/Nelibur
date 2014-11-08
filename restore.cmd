@echo off

md packages
.nuget\nuget restore Solution.sln -PackagesDirectory packages
.nuget\nuget install Tools\packages.config -OutputDirectory Tools -ExcludeVersion -NonInteractive

