@echo off
echo "============================ GENERATE C# classes ============================"

echo "Generate classes for info.xsd"
"C:\Program Files (x86)\Microsoft SDKs\Windows\v8.1A\bin\NETFX 4.5.1 Tools\xsd.exe" /nologo info.xsd /c /n:Arkivverket.Arkade.ExternalModels.Info
copy /y info.cs ..\Info.cs
del info.cs

echo "Generate classes for addml.xsd"
"C:\Program Files (x86)\Microsoft SDKs\Windows\v8.1A\bin\NETFX 4.5.1 Tools\xsd.exe" /nologo addml.xsd /c /n:Arkivverket.Arkade.ExternalModels.Addml
copy /y Addml.cs ..\Addml.cs
del addml.cs


pause