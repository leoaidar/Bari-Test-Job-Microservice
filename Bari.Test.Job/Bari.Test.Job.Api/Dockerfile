#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Bari.Test.Job.Api/Bari.Test.Job.Api.csproj", "Bari.Test.Job.Api/"]
COPY ["Bari.Test.Job.Domain/Bari.Test.Job.Domain.csproj", "Bari.Test.Job.Domain/"]
COPY ["Bari.Test.Job.Application/Bari.Test.Job.Application.csproj", "Bari.Test.Job.Application/"]
COPY ["Bari.Test.Job.infra.IoC/Bari.Test.Job.infra.IoC.csproj", "Bari.Test.Job.infra.IoC/"]
COPY ["Bari.Test.Job.Infra.Data/Bari.Test.Job.Infra.Data.csproj", "Bari.Test.Job.Infra.Data/"]
COPY ["Bari.Test.Job.Infra.Bus/Bari.Test.Job.Infra.Bus.csproj", "Bari.Test.Job.Infra.Bus/"]
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