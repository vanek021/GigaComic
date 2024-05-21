FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["GigaComic.Client/GigaComic.Client.csproj", "GigaComic.Client/"]
COPY ["GigaComic.Core/GigaComic.Core.csproj", "GigaComic.Core/"]
COPY ["GigaComic.Data/GigaComic.Data.csproj", "GigaComic.Data/"]
COPY ["GigaComic.Infrastructure/GigaComic.Infrastructure.csproj", "GigaComic.Infrastructure/"]
COPY ["GigaComic.Models/GigaComic.Models.csproj", "GigaComic.Models/"]
COPY ["GigaComic.Shared/GigaComic.Shared.csproj", "GigaComic.Shared/"]
COPY ["GigaComic.Modules.GigaChat/GigaComic.Modules.GigaChat.csproj", "GigaComic.Modules.GigaChat/"]
COPY ["GigaComic.Modules.Kandinsky/GigaComic.Modules.Kandinsky.csproj", "GigaComic.Modules.Kandinsky/"]

COPY . .
WORKDIR "/src/GigaComic.Client"

RUN dotnet workload restore [/src/GigaComic.Client/GigaComic.Client.csproj]
RUN dotnet publish "GigaComic.Client.csproj" -c Release --no-restore -o /app/publish

FROM base AS final
WORKDIR /app

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "FSH.BlazorWebAssembly.Host.dll"]