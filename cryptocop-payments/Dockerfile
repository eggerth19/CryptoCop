FROM mcr.microsoft.com/dotnet/core/sdk:3.1

LABEL maintainer="Eggert Orri Hermannsson"

ENV PRODUCTION_ENV=rabbitmq-container

COPY .  /app
WORKDIR /app/PaymentService
RUN dotnet restore
ENTRYPOINT [ "dotnet", "run" ]  