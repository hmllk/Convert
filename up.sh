app=commonservice-api
docker build -f Dockerfile -t $app .
docker stop $app
docker rm $app
docker run -d --name $app --restart always \
    -p 9999:9999 \
    $app

