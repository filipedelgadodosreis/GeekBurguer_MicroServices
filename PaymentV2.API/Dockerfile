FROM mcr.microsoft.com/dotnet/core/aspnet:2.1-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.1-stretch AS build
WORKDIR /src
COPY ["PaymentV2.API/PaymentV2.API.csproj", "PaymentV2.API/"]
RUN dotnet restore "PaymentV2.API/PaymentV2.API.csproj"
COPY . .
WORKDIR "/src/PaymentV2.API"
RUN dotnet build "PaymentV2.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PaymentV2.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PaymentV2.API.dll"]