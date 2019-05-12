################################ NEED ###################################
 - dotnet
 - docker (for mac and linux)
################################ Config #################################

in Ogame/Ogame/appsettings.json
- comment or uncomment the line corresponding to your distribution


################################ Launch server ##########################

on windows :
open with visual studio
in nugget console run
- Update-Database
then run with visual studio

on mac :
- sudo docker run -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=TESTtest123' -p 1433:1433 --name db -d mcr.microsoft.com/mssql/server:2017-latest
- cd Ogame
- dotnet restore
- dotnet build
- cd Ogame
- dotnet ef database update
- dotnet run
