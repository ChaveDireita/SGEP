FROM mcr.microsoft.com/dotnet/core/sdk:2.1 AS build-env

WORKDIR /app

COPY *.csproj ./
RUN dotnet restore

COPY . .
RUN dotnet build -c Release -o out

RUN dotnet ef database update

ENTRYPOINT [ "dotnet", "run" ]

EXPOSE 55555