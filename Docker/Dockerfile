FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["LifeyLife/LifeyLife.csproj", "LifeyLife/"]
RUN dotnet restore "LifeyLife/LifeyLife.csproj"
COPY . .
WORKDIR "/src/LifeyLife"
RUN dotnet build "LifeyLife.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "LifeyLife.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LifeyLife.dll"]
