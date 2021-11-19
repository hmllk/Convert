cd /data/convert/
docker build -t convertservice .

docker run  -d -p 9999:9999 --name convertservice -v /usr/share/fonts:/usr/share/fonts convertservice

docker logs convertservice
http://ip:9999/convert/wordtopdf
http://ip:9999/convert/pdftoimg

 /opt/libreoffice7.1/program/soffice --invisible --convert-to pdf  /commonservice/wwwroot/test.docx --outdir /commonservice/wwwroot/
 /opt/libreoffice7.1/program/soffice --invisible --convert-to png  /commonservice/wwwroot/test.docx --outdir /commonservice/wwwroot/
  
 
