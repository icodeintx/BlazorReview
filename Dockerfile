FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app
COPY publish/ .
EXPOSE 80
EXPOSE 443
ENTRYPOINT ["dotnet", "WebsiteBlazor.dll"]