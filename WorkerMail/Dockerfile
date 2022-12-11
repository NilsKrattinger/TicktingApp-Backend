FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["WorkerMail/WorkerMail.csproj", "WorkerMail/"]
RUN dotnet restore "WorkerMail/WorkerMail.csproj"
COPY . .
WORKDIR "/src/WorkerMail"
RUN dotnet build "WorkerMail.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WorkerMail.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WorkerMail.dll"]
