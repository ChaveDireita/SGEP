echo off
cls
cd SGEP
erase Migrations /q 
rmdir Migrations
..\dotnet-windows-noadmin\dotnet.exe ef database drop -f
..\dotnet-windows-noadmin\dotnet.exe ef migrations add Migracao
..\dotnet-windows-noadmin\dotnet.exe ef database update
pause