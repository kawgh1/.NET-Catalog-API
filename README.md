# .NET 5 Catalog Web API for testing
- Based off project by Julio Casal https://dotnetmicroservices.com/

## Tools Used
- Docker
- MongoDB
- Postman

## Steps

- to create new web api from terminal
  - `dotnet new webapi -n Catalog`

  <br>
  
- set up certificates to run on localhost
  - This will allow Swagger to display in browser window for the API
    - `dotnet dev-certs https --trust `

  <br>

- Install MongoDB Client Driver (NuGet)
  - `dotnet add package MongoDB.Driver`

  <br>

- Run local Docker Container
  - `docker run -d --rm --name mongo -p 27017:27017 -v mongodbdata:/data/db mongo`
    - `-d` - dont attach to the process, just run it and let it go
    - `-rm` - destroy the container after you are done, dont keep it running
    - `--name` - name of the Image / Container you are creating
    - `-p` - the port
    - `-v` - the MongoDB volume, so you dont destroy the data when you stop the Docker Container
    - `mongodbdata:/data/db mongo` - store your DB data at this file path in Mongo to persist the data after the Container has closed

  <br>
- View running Docker processes
  - `docker ps`