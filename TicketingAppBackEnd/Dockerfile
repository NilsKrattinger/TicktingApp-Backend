FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["TicketingAppBackEnd/TicketingAppBackEnd.csproj", "TicketingAppBackEnd/"]
RUN dotnet restore "TicketingAppBackEnd/TicketingAppBackEnd.csproj"
COPY . .
WORKDIR "/src/TicketingAppBackEnd"
RUN dotnet build "TicketingAppBackEnd.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TicketingAppBackEnd.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TicketingAppBackEnd.dll"]
