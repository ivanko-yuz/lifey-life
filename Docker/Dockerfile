FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["LifeyLife.App/LifeyLife.App.csproj", "LifeyLife.App/"]
COPY ["LifeyLife.Api/LifeyLife.Api.csproj", "LifeyLife.Api/"]
COPY ["LifeyLife.Core/LifeyLife.Core.csproj", "LifeyLife.Core/"]
COPY ["LifeyLife.Data/LifeyLife.Data.csproj", "LifeyLife.Data/"]
RUN dotnet restore "LifeyLife.App/LifeyLife.App.csproj"
COPY . .
WORKDIR "/src/LifeyLife.App"
RUN dotnet build "LifeyLife.App.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "LifeyLife.App.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LifeyLife.App.dll"]
