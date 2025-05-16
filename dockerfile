FROM mcr.microsoft.com/dotnet/sdk:8.0-bookworm-slim AS build
WORKDIR /source

# RUN apt update && apt install python3 -y
# RUN dotnet workload install wasm-tools

COPY Blazorex/readme.md ./Blazorex/readme.md
COPY Blazorex/src/Blazorex/*.csproj ./Blazorex/src/Blazorex/ 
COPY Router/*.csproj ./Router/
COPY Router.Wasm/*.csproj ./Router.Wasm/
RUN dotnet restore Router.Wasm

COPY Blazorex/src/Blazorex/. ./Blazorex/src/Blazorex
COPY Router/. ./Router/
COPY Router.Wasm/. ./Router.Wasm/
RUN dotnet publish Router.Wasm -c Release -p:PublishTrimmed=true -o /app

FROM nginx:alpine AS runtime
COPY --from=build /app/wwwroot /usr/share/nginx/html
COPY nginx.conf /etc/nginx/nginx.conf

EXPOSE 80
