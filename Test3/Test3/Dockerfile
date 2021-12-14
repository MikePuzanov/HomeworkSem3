FROM mcr.microsoft.com/dotnet/runtime:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Test3/Test3.csproj", "Test3/"]
RUN dotnet restore "Test3/Test3.csproj"
COPY . .
WORKDIR "/src/Test3"
RUN dotnet build "Test3.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Test3.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Test3.dll"]
