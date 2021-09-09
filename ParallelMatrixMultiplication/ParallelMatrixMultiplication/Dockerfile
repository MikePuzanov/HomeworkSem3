FROM mcr.microsoft.com/dotnet/runtime:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["ParallelMatrixMultiplication/ParallelMatrixMultiplication.csproj", "ParallelMatrixMultiplication/"]
RUN dotnet restore "ParallelMatrixMultiplication/ParallelMatrixMultiplication.csproj"
COPY . .
WORKDIR "/src/ParallelMatrixMultiplication"
RUN dotnet build "ParallelMatrixMultiplication.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ParallelMatrixMultiplication.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ParallelMatrixMultiplication.dll"]
