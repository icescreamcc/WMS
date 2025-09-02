#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["WebApi/WebApi.csproj", "WebApi/"]
COPY ["Model/Model.csproj", "Model/"]
COPY ["Logic/LogicDependencyInjection/LogicDependencyInjection.csproj", "Logic/LogicDependencyInjection/"]
COPY ["Logic/Sys/Sys.csproj", "Logic/Sys/"]
COPY ["Logic/LogicCommon/LogicCommon.csproj", "Logic/LogicCommon/"]
COPY ["External/Aliyun/Aliyun.csproj", "External/Aliyun/"]
COPY ["External/Common/Common.csproj", "External/Common/"]
COPY ["Logic/LogicBase/LogicBase.csproj", "Logic/LogicBase/"]
COPY ["External/Cache/Cache.csproj", "External/Cache/"]
COPY ["DbRepository/Repository/Repository.csproj", "DbRepository/Repository/"]
COPY ["Logic/Purchase/Purchase.csproj", "Logic/Purchase/"]
COPY ["Logic/Inventory/Inventory.csproj", "Logic/Inventory/"]
COPY ["Logic/Authentication/Authentication.csproj", "Logic/Authentication/"]
COPY ["Logic/BaseInfo/BaseInfo.csproj", "Logic/BaseInfo/"]
COPY ["Logic/Quality/Quality.csproj", "Logic/Quality/"]
RUN dotnet restore "WebApi/WebApi.csproj"
COPY . .
WORKDIR "/src/WebApi"
RUN dotnet build "WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebApi.dll"]