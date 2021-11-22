FROM mcr.microsoft.com/dotnet/runtime:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["MyFTPServer/MyFTPServer.csproj", "MyFTPServer/"]
RUN dotnet restore "MyFTPServer/MyFTPServer.csproj"
COPY . .
WORKDIR "/src/MyFTPServer"
RUN dotnet build "MyFTPServer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MyFTPServer.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MyFTPServer.dll"]
