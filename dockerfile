FROM mcr.microsoft.com/dotnet/sdk:8.0-jammy AS build
WORKDIR /source
COPY . .
RUN dotnet publish Router.Wasm -c Release -p:PublishTrimmed=true -o /app

FROM busybox:stable as runtime
WORKDIR /www
COPY --from=build /app/wwwroot .

EXPOSE 80
CMD ["httpd", "-f", "-p", "80", "-h", "/www"]