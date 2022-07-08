FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["SceneGenerator/SceneGenerator.csproj", "SceneGenerator/"]
RUN dotnet restore "SceneGenerator/SceneGenerator.csproj"
COPY . .
WORKDIR "/src/SceneGenerator"
RUN dotnet build "SceneGenerator.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SceneGenerator.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SceneGenerator.dll"]
