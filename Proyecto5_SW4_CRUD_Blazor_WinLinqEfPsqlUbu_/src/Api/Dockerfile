# Usamos el runtime de .NET 8 sobre Alpine Linux (pesa solo ~35 MB)
FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine
WORKDIR /app

# Copiamos los archivos ya compilados
COPY publish/ .

# Configuramos el puerto
ENV ASPNETCORE_URLS=http://+:5000
EXPOSE 5000

ENTRYPOINT ["dotnet", "Api.dll"]