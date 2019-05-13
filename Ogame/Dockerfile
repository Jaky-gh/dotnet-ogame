FROM microsoft/dotnet:2.1-sdk AS build
COPY . .
WORKDIR /Ogame
RUN ["dotnet", "restore"]
RUN ["dotnet", "build"]
EXPOSE 5000-5001
CMD dotnet ef database update && dotnet run
