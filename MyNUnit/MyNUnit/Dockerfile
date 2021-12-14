FROM mcr.microsoft.com/dotnet/runtime:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["MyNUnit/MyNUnit.csproj", "MyNUnit/"]
RUN dotnet restore "MyNUnit/MyNUnit.csproj"
COPY . .
WORKDIR "/src/MyNUnit"
RUN dotnet build "MyNUnit.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MyNUnit.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MyNUnit.dll"]
