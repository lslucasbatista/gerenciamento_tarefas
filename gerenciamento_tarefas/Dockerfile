# ================================
#  Etapa 1: Build
# ================================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia os arquivos de projeto
COPY ["GerenciamentoProjeto.API/GerenciamentoProjeto.API.csproj", "GerenciamentoProjeto.API/"]
COPY ["GerenciamentoProjeto.Application/GerenciamentoProjeto.Application.csproj", "GerenciamentoProjeto.Application/"]
COPY ["GerenciamentoProjeto.Domain/GerenciamentoProjeto.Domain.csproj", "GerenciamentoProjeto.Domain/"]
COPY ["GerenciamentoProjeto.Infrastructure/GerenciamentoProjeto.Infrastructure.csproj", "GerenciamentoProjeto.Infrastructure/"]
COPY ["GerenciamentoProjeto.Tests/GerenciamentoProjeto.Tests.csproj", "GerenciamentoProjeto.Tests/"]

# Restaura dependências
RUN dotnet restore "GerenciamentoProjeto.API/GerenciamentoProjeto.API.csproj"

# Copia todo o código
COPY . .

# Compila em modo Release
WORKDIR "/src/GerenciamentoProjeto.API"
RUN dotnet publish "GerenciamentoProjeto.API.csproj" -c Release -o /app/publish

# ================================
#  Etapa 2: Runtime
# ================================
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Copia o build da etapa anterior
COPY --from=build /app/publish .

# Porta exposta
EXPOSE 8080
EXPOSE 443

# Variável de ambiente
ENV ASPNETCORE_URLS=http://+:8080

# Comando de inicialização
ENTRYPOINT ["dotnet", "GerenciamentoProjeto.API.dll"]