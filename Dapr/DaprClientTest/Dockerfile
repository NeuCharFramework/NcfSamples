#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["../../NcfPackageSources/src/Directory.Build.props", "../../NcfPackageSources/src/"]
COPY ["DaprClientTest/DaprClientTest.csproj", "DaprClientTest/"]
COPY ["../../NcfPackageSources/src/Extensions/Senparc.Xncf.DaprClient/Senparc.Xncf.DaprClient.csproj", "../../NcfPackageSources/src/Extensions/Senparc.Xncf.DaprClient/"]
RUN dotnet restore "DaprClientTest/DaprClientTest.csproj"
COPY . .
WORKDIR "/src/DaprClientTest"
RUN dotnet build "DaprClientTest.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DaprClientTest.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DaprClientTest.dll"]