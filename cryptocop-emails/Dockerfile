FROM python:3.9.0

LABEL maintainer="Eggert Orri Hermannsson"

ADD email_service.py /
ENV PRODUCTION_ENV=rabbitmq-container

RUN pip install requests pika

CMD [ "python", "./email_service.py" ]