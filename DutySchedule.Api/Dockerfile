#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["DutySchedule.Api/DutySchedule.Api.csproj", "DutySchedule.Api/"]
RUN dotnet restore "DutySchedule.Api/DutySchedule.Api.csproj"
COPY . .
WORKDIR "/src/DutySchedule.Api"
RUN dotnet build "DutySchedule.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DutySchedule.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DutySchedule.Api.dll"]