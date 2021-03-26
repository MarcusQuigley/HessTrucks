#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Frontends/Blazor.Client/Blazor.Client.csproj", "Frontends/Blazor.Client/"]
COPY ["Frontends/Blazor.App/Blazor.App.csproj", "Frontends/Blazor.App/"]
COPY ["Services/Catalog/Services.Catalog.Api/Services.Catalog.Api.csproj", "Services/Catalog/Services.Catalog.Api/"]

RUN dotnet restore "Frontends/Blazor.Client/Blazor.Client.csproj"
COPY . .
WORKDIR "/src/Frontends/Blazor.Client"
RUN dotnet build "Blazor.Client.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Blazor.Client.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Blazor.Client.dll"]
