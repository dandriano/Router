FROM mcr.microsoft.com/dotnet/sdk:8.0-bookworm-slim AS build
WORKDIR /source
COPY . .

RUN apt update && apt install python3 -y

RUN dotnet workload install wasm-tools
RUN dotnet publish Router.Wasm -c Release -p:PublishTrimmed=true -o /app

FROM busybox:stable as runtime
WORKDIR /www
COPY --from=build /app/wwwroot .

EXPOSE 80
CMD ["httpd", "-f", "-p", "80", "-h", "/www"]