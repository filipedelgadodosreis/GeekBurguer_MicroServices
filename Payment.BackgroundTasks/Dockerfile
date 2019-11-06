FROM mcr.microsoft.com/dotnet/core/aspnet:2.1-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.1-stretch AS build
WORKDIR /src
COPY ["Payment.BackgroundTasks/Payment.BackgroundTasks.csproj", "Payment.BackgroundTasks/"]
RUN dotnet restore "Payment.BackgroundTasks/Payment.BackgroundTasks.csproj"
COPY . .
WORKDIR "/src/Payment.BackgroundTasks"
RUN dotnet build "Payment.BackgroundTasks.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Payment.BackgroundTasks.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Payment.BackgroundTasks.dll"]