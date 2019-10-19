FROM mcr.microsoft.com/dotnet/core/aspnet:2.1-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.1-stretch AS build
WORKDIR /src
COPY ["Payment.API/Payment.API.csproj", "Payment.API/"]
RUN dotnet restore "Payment.API/Payment.API.csproj"
COPY . .
WORKDIR "/src/Payment.API"
RUN dotnet build "Payment.API.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Payment.API.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Payment.API.dll"]