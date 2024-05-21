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

RUN dotnet workload restore GigaComic.Client.csproj
RUN dotnet restore "GigaComic.Client.csproj"
RUN dotnet publish "GigaComic.Client.csproj" -c Release --no-restore -o /app/publish

FROM nginx:alpine AS final
WORKDIR /usr/share/nginx/html
COPY --from=publish /app/publish/wwwroot .

COPY nginx.conf /etc/nginx/nginx.conf

EXPOSE 80
ENTRYPOINT ["nginx", "-g", "daemon off;"]