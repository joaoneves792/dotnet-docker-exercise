FROM microsoft/dotnet:2.2-sdk
EXPOSE 8080
RUN apt-get update && apt-get install -y unzip && \
    curl -sSL https://aka.ms/getvsdbgsh | bash /dev/stdin -v vs2017u5 -l /vsdbg/
USER 999:999
ENV HOME /app
COPY --chown=999:999 . /app
WORKDIR /app
RUN dotnet build -c Debug -o .
CMD ["/bin/sh", "-c", "exec dotnet dotnet_exercise.dll"]
