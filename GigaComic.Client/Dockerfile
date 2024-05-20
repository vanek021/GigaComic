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

RUN dotnet workload restore GigaComic.Client/GigaComic.Client.csproj
RUN dotnet restore "GigaComic.Client/GigaComic.Client.csproj"

COPY . .
WORKDIR "/src/GigaComic.Client"
RUN dotnet build "GigaComic.Client.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GigaComic.Client.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GigaComic.Client.dll"]