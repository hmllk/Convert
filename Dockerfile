FROM registry-docker.pamirs.com/dotnetsdk-yarn:latest AS build-env
WORKDIR /commonservice

COPY . ./
 
RUN dotnet publish -c Release -o out

FROM  registry-docker.pamirs.com/aspnet:3.1
WORKDIR /commonservice

ADD sources.list  /etc/apt/

RUN apt-get update \
        && apt-get install apt-file -y \
        && apt-get install libxinerama1 -y \
        && apt-get install libdbus-1-3 -y \
        && apt-get install libglib2.0-0 -y \
        && apt-get install libcairo2 -y \
        && apt-get install libcups2 -y \
        && apt-get install libsm6 -y  \
        && apt-get install libx11-xcb1 -y \
        && apt-get install ghostscript -y
        
ADD .  /commonservice
RUN tar -xzvf LibreOffice_7.1.4_Linux_x86-64_deb.tar.gz  \
       && dpkg -i  LibreOffice_7.1.4.2_Linux_x86-64_deb/DEBS/*.deb

#RUN ln -s /opt/libreoffice7.1

COPY --from=build-env /commonservice/out/ .

ENTRYPOINT ["dotnet", "Convert.dll"]