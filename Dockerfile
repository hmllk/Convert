FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS build-env
WORKDIR /commonservice

COPY . ./
 
RUN dotnet publish -c Release -o out

FROM  mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim
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

COPY ./SourceHanSansSC-Normal.otf /usr/share/fonts
COPY ./SourceHanSansSC-Bold.otf /usr/share/fonts
COPY ./SourceHanSansSC-Medium.otf /usr/share/fonts
COPY ./MSYH.TTC /usr/share/fonts
RUN mkfontscale && mkfontdir && fc-cache

COPY --from=build-env /commonservice/out/ .

ENTRYPOINT ["dotnet", "Convert.dll"]
