#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM 10.24.78.98:8080/prod/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM 10.24.78.98:8080/prod/dotnet-sdk:7.0 AS build

# 设置默认值
ARG ENVIRONMENT=Development
WORKDIR /src
# RUN dotnet restore "WebApi/WebApi.csproj"
COPY . .

WORKDIR /src/WebApi
# 根据环境，编译不同版本

RUN if [ "${ENVIRONMENT}" = "Production" ]; then \
        echo "build with prod configure"  && \
        dotnet publish -c Release -o /app/publish; \
    else \
        echo "build with dev configure"  &&  \
        dotnet publish -c Debug -o /app/publish; \
    fi

RUN cat WebApi.csproj

FROM base AS final
WORKDIR /app
# 设置默认值
ARG ENVIRONMENT=Development

ENV ASPNETCORE_ENVIRONMENT=${ENVIRONMENT}

COPY --from=build /app/publish .

# CMD cat "appsettings.Development.json" && cat "appsettings.Production.json"

RUN mkdir  Files

CMD ["dotnet", "WebApi.dll", "--urls", "http://0.0.0.0:80"]