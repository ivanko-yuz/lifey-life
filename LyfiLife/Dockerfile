FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["LyfiLife/LyfiLife.csproj", "LyfiLife/"]
RUN dotnet restore "LyfiLife/LyfiLife.csproj"
COPY . .
WORKDIR "/src/LyfiLife"
RUN dotnet build "LyfiLife.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "LyfiLife.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LyfiLife.dll"]
