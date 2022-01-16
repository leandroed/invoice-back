FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine AS publish
WORKDIR /src
COPY DbLib/DbLib.csproj ./DbLib/
COPY InvoiceApi/InvoiceApi.csproj ./InvoiceApi/

RUN dotnet restore "DbLib/DbLib.csproj" --runtime alpine-x64
RUN dotnet restore "InvoiceApi/InvoiceApi.csproj" --runtime alpine-x64
COPY . .

WORKDIR /src/DbLib
RUN dotnet publish "DbLib.csproj" -c Release -o /app/publish \
    --runtime alpine-x64 \
    --self-contained true \
    /p:PublishTrimmed=true \
    /p:PublishSingleFile=false

WORKDIR /src/InvoiceApi
RUN dotnet publish "InvoiceApi.csproj" -c Release -o /app/publish \
    --runtime alpine-x64 \
    --self-contained true \
    /p:PublishTrimmed=true \
    /p:PublishSingleFile=true

FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine as base
WORKDIR /app

EXPOSE 80
COPY --from=publish /app/publish .

ENTRYPOINT ["./InvoiceApi"]
