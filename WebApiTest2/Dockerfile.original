#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["WebApiTest2/WebApiTest2.csproj", "WebApiTest2/"]
RUN dotnet restore "WebApiTest2/WebApiTest2.csproj"
COPY . .
WORKDIR "/src/WebApiTest2"
RUN dotnet build "WebApiTest2.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WebApiTest2.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebApiTest2.dll"]