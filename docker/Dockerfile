FROM mcr.microsoft.com/mssql/server:2019-CU10-ubuntu-20.04
USER root

EXPOSE 1433

# Install dos2unix
RUN apt-get -y update && \
    apt-get install -y dos2unix

RUN mkdir -p /usr/src/autobarn
WORKDIR /usr/src/autobarn

COPY . /usr/src/autobarn

# Grant permissions for the import-data script to be executable
RUN chmod +x /usr/src/autobarn/import-data.sh
COPY . /usr/src/autobarn

RUN dos2unix *
RUN dos2unix csv-data/*

USER mssql

ENTRYPOINT /bin/bash ./entrypoint.sh
