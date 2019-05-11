FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["Ogame/Ogame.csproj", "Ogame/"]
RUN dotnet restore "Ogame/Ogame.csproj"
COPY . .
WORKDIR "/src/Ogame"
RUN dotnet build "Ogame.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Ogame.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Ogame.dll"]