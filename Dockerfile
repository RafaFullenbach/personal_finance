FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY personal_finance.API/personal_finance.API.csproj ./personal_finance.API/
COPY personal_finance.Application/personal_finance.Application.csproj ./personal_finance.Application/
COPY personal_finance.Domain/personal_finance.Domain.csproj ./personal_finance.Domain/
COPY personal_finance.Infrastructure/personal_finance.Infrastructure.csproj ./personal_finance.Infrastructure/

RUN dotnet restore ./personal_finance.API/personal_finance.API.csproj

COPY . .

RUN dotnet publish ./personal_finance.API/personal_finance.API.csproj -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
RUN mkdir -p /data
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "personal_finance.API.dll"]
