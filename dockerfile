FROM mcr.microsoft.com/dotnet/sdk:8.0-bookworm-slim AS build
WORKDIR /source

# RUN apt update && apt install python3 -y
# RUN dotnet workload install wasm-tools

COPY Router/*.csproj ./Router/
COPY Router.Wasm/*.csproj ./Router.Wasm/
RUN dotnet restore Router.Wasm

COPY Router/. ./Router/
COPY Router.Wasm/. ./Router.Wasm/
RUN dotnet publish Router.Wasm -c Release -p:PublishTrimmed=true -o /app

FROM busybox:stable as runtime
WORKDIR /www
COPY --from=build /app/wwwroot .

EXPOSE 80
CMD ["httpd", "-f", "-p", "80", "-h", "/www"]