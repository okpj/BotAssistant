#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 2354

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["BotAssistant/BotAssistant.csproj", "BotAssistant/"]
COPY ["BotAssistant.Application.Service/BotAssistant.Application.Service.csproj", "BotAssistant.Application.Service/"]
COPY ["BotAssistant.Application.Contract/BotAssistant.Application.Contract.csproj", "BotAssistant.Application.Contract/"]
COPY ["BotAssistant.Application.Util/BotAssistant.Application.Util.csproj", "BotAssistant.Application.Util/"]
COPY ["BotAssistant.Infrastructure.Telegram/BotAssistant.Infrastructure.TelegramBot.csproj", "BotAssistant.Infrastructure.Telegram/"]
COPY ["BotAssistant.Infrastructure.Worker/BotAssistant.Infrastructure.Worker.csproj", "BotAssistant.Infrastructure.Worker/"]
COPY ["BotAssistant.Infrastructure/BotAssistant.Infrastructure.csproj", "BotAssistant.Infrastructure/"]
RUN dotnet restore "BotAssistant/BotAssistant.csproj"
COPY . .
WORKDIR "/src/BotAssistant"
RUN dotnet build "BotAssistant.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BotAssistant.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BotAssistant.dll"]