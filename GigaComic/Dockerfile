FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["GigaComic/GigaComic.csproj", "GigaComic/"]
COPY ["GigaComic.Core/GigaComic.Core.csproj", "GigaComic.Core/"]
COPY ["GigaComic.Core.Server/GigaComic.Core.Server.csproj", "GigaComic.Core.Server/"]
COPY ["GigaComic.Data/GigaComic.Data.csproj", "GigaComic.Data/"]
COPY ["GigaComic.Infrastructure/GigaComic.Infrastructure.csproj", "GigaComic.Infrastructure/"]
COPY ["GigaComic.Models/GigaComic.Models.csproj", "GigaComic.Models/"]
COPY ["GigaComic.Shared/GigaComic.Shared.csproj", "GigaComic.Shared/"]
COPY ["GigaComic.Modules.GigaChat/GigaComic.Modules.GigaChat.csproj", "GigaComic.Modules.GigaChat/"]
COPY ["GigaComic.Modules.Kandinsky/GigaComic.Modules.Kandinsky.csproj", "GigaComic.Modules.Kandinsky/"]
COPY ["GigaComic.Modules.ComicRenderer/GigaComic.Modules.ComicRenderer.csproj", "GigaComic.Modules.ComicRenderer/"]


RUN dotnet restore "GigaComic/GigaComic.csproj"
COPY . .
WORKDIR "/src/GigaComic"
RUN dotnet build "GigaComic.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GigaComic.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GigaComic.dll"]