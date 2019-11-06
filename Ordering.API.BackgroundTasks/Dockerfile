FROM mcr.microsoft.com/dotnet/core/aspnet:2.1-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.1-stretch AS build
WORKDIR /src
COPY ["Ordering.API.BackgroundTasks/Ordering.API.BackgroundTasks.csproj", "Ordering.API.BackgroundTasks/"]
RUN dotnet restore "Ordering.API.BackgroundTasks/Ordering.API.BackgroundTasks.csproj"
COPY . .
WORKDIR "/src/Ordering.API.BackgroundTasks"
RUN dotnet build "Ordering.API.BackgroundTasks.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Ordering.API.BackgroundTasks.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Ordering.API.BackgroundTasks.dll"]