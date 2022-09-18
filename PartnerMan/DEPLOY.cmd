cd /D "%~dp0"
cd PartnerMng
echo Telepítés...
docker network create -d bridge z-net
docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=zUl33@111" --name z-sql -p 1433:1433 --network z-net -d mcr.microsoft.com/mssql/server:2022-latest
docker build . -t partner_manager:dev -f PartnerMan\Dockerfile
docker run -d -p 8080:80 --network z-net --name zulee partner_manager:dev

start http://localhost:8080