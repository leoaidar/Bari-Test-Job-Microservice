#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Bari.Test.Job.Api/Bari.Test.Job.Api.csproj", "Bari.Test.Job.Api/"]
RUN dotnet restore "Bari.Test.Job.Api/Bari.Test.Job.Api.csproj"
COPY . .
WORKDIR "/src/Bari.Test.Job.Api"
RUN dotnet build "Bari.Test.Job.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Bari.Test.Job.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Bari.Test.Job.Api.dll"]