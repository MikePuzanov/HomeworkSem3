FROM mcr.microsoft.com/dotnet/runtime:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Test1.1/Test1.1.csproj", "Test1.1/"]
RUN dotnet restore "Test1.1/Test1.1.csproj"
COPY . .
WORKDIR "/src/Test1.1"
RUN dotnet build "Test1.1.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Test1.1.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Test1.1.dll"]
