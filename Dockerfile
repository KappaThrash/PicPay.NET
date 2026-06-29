FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build-env
WORKDIR /src


RUN apt-get update && apt-get install -y --no-install-recommends clang zlib1g-dev


COPY *.csproj ./
RUN dotnet restore -r linux-x64


COPY . ./
RUN dotnet publish -c Release -r linux-x64 -o /out --no-restore


FROM mcr.microsoft.com/dotnet/runtime-deps:9.0
WORKDIR /app


COPY --from=build-env /out/PicPay .

EXPOSE 8080

# Executa o binário diretamente
ENTRYPOINT ["./PicPay"]
