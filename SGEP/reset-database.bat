erase Migrations /q 
rmdir Migrations
dotnet ef database drop -f
dotnet ef migrations add Migracao
dotnet ef database update
