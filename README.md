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
- Stop running Docker process
  - `docker stop {name}`
- View Docker Volumes
  - `docker volume ls`
- Remove Docker Volume
  - `docker volume rm {volume name}`

  <br>

- Run local Docker Container with environment variables (DB username, password, etc.)
  - `docker run -d --rm --name mongo -p 27017:27017 -v mongodbdata:/data/db -e MONGO_INITDB_ROOT_USERNAME=mongoadmin -e MONGO_INITDB_ROOT_PASSWORD=password1 mongo`
    - `-d` - dont attach to the process, just run it and let it go
    - `-rm` - destroy the container after you are done, dont keep it running
    - `--name` - name of the Image / Container you are creating
    - `-p` - the port
    - `-v` - the MongoDB volume, so you dont destroy the data when you stop the Docker Container
    - `mongodbdata:/data/db mongo` - store your DB data at this file path in Mongo to persist the data after the Container has closed
      - So at this point, our DB knows about requiring user authentication we have set, but our Services do not know about it
        - Set these in `appsettings.json` and the `.NET Secret Manager`

- ### .NET Secret Manager 
  - #### Generally dont use this in Prod
  - `dotnet user-secrets init`
    - `Set UserSecretsId to '7ee7c261-9b12-4f9e-9fc0-de16f4fd7384' for MSBuild project '/Users/j/Desktop/Catalog/Catalog.csproj'.`
  - to add a secret
    - `dotnet user-secrets set MongoDbSettings:Password password1`
      - `Successfully saved MongoDbSettings:Password = password1 to the secret store.`
  - Finally
    - Set the username and password in the MongoDB Connection string in `settings/mongodbsettings.cs`
      - `return $"mongodb://{User}:{Password}@{Host}:{Port}";`

  <br>

- ### Health Checks
  - A health check is an endpoint like `https://localhost:5001/health` that will tell if an API is able to receive requests and able to communicate with the DB or other services it depends on
  - Install Open Source MongoDB library to check MongoDB connection status as part of Health Check
    - `dotnet add package AspNetCore.HealthChecks.MongoDb`
  - This can be configured in `startup.cs`
    - `services.AddHealthChecks().AddMongoDb(mongoDbSettings.ConnectionString, name: "mongodb", timeout: TimeSpan.FromSeconds(3));`
    - `app.UseEndpoints(endpoints =>
      {
      endpoints.MapControllers();
      endpoints.MapHealthChecks("/health");
      });`
  - Testing Health Check can be done by sending a GET request to https://localhost:5001/health 
    - it should return `200` `Healthy`
    - Now stop the docker instance of the Mongo container `docker stop mongo`
    - Send another GET request to https://localhost:5001/health
    - Verify it returns `503` `Unhealthy`
  - #### Note: I had to add another package to get the MongoDB Health Check package to work properly
    - `dotnet add package Microsoft.Bcl.AsyncInterfaces`
  - Health Checks can be customized and configured to return more detailed information as a JSON object like this:
    - `{
        "status": "Healthy",
            "checks": [
                {
                "name": "mongodb",
                "status": "Healthy",
                "exception": "none",
                "duration": "00:00:00.0923151"
                }
            ]
      }`
    - More customization options can be seen here https://github.com/Xabaril/AspNetCore.Diagnostics.HealthChecks for all kinds or BackEnd stuff including AWS, Kafka, RabbitMQ, SendGrid, Kubernetes, Azure, etc.
  
    <br>
  
- ## Docker
  - Problems Docker helps solve / simplify
    - What kind of machine is my server going to run on?
      - Physical machine? Virtual Machine?
        - Linux? Windows? Be sure to pick the right OS version for your needs
    - How are we getting files to and from our server?
    - What if DB requires different version of OS or dependencies?
    - What if we want to move to a new version of .NET?
    - How do we quickly start the REST API on the machine?
    - What if one instance of the API or service is not enough to handle the load?

  - ### Dockerfile
    - The Dockerfile can set configurations for:
      - OS
      - .NET / ASP.NET Core Runtime
      - Dependencies
      - Where to place the files in the file system
      - How to star the REST API