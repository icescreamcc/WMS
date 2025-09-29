#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM 10.24.78.98:8080/prod/nginx:1.25.4 AS base
WORKDIR /app
# 添加nginx配置文件
ADD ./nginx/nginx.conf /etc/nginx/nginx.conf

EXPOSE 80
EXPOSE 443

# 编译前端工程
FROM 10.24.78.98:8080/prod/node:14 AS build
WORKDIR /src
ARG ENVIRONMENT=dev
ENV https_proxy="http://10.24.78.124:3128"
ENV http_proxy="http://10.24.78.124:3128"

# 复制依赖包到工作目录
COPY package*.json ./
# 安装依赖包
RUN npm install
# 复制项目得所有文件到工作目录
COPY . .
# 编译前端代码
# 根据环境，编译不同版本
RUN if [ "${ENVIRONMENT}" = "prod" ]; then \
        echo "build with prod configure"  && \
        npm run build; \
    else \
        echo "build with dev configure"  && \
        npm run build:stag; \
    fi

FROM base AS final
WORKDIR /app

COPY --from=build /src   /app 
COPY --from=build /src/dist  /usr/share/nginx/html




