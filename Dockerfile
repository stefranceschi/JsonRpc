FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["JsonRpc.API/JsonRpc.API.csproj", "JsonRpc.API/"]
COPY ["JsonRpc.Core/JsonRpc.Core.csproj", "JsonRpc.Core/"]
RUN dotnet restore "JsonRpc.API/JsonRpc.API.csproj"
COPY . .
WORKDIR "/src/JsonRpc.API"
RUN dotnet build "JsonRpc.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "JsonRpc.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "JsonRpc.API.dll"]