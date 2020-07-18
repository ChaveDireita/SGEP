echo off
cls
cd SGEP
..\dotnet-windows-noadmin\dotnet.exe run -- --use-local-db
pause