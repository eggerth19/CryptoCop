FROM mcr.microsoft.com/dotnet/core/sdk:3.1

LABEL maintainer="Eggert Orri Hermannsson"

ENV ASPNETCORE_URLS=http://+:5000
ENV PRODUCTION_ENV=rabbitmq-container

COPY .  /app
WORKDIR /app/Cryptocop.Software.API
RUN dotnet restore
ENTRYPOINT [ "dotnet", "run" ]  