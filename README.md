# Tile data extract


## Testing

Run the whole test suite.

```sh
dotnet test
```

### Unit testing

```sh
dotnet test --filter Category=Unit
```

### Integration testing

For integration testing you should run the following image from "https://hub.docker.com/r/kartoza/postgis/".

Default PostgreSQL user is 'docker' with password 'docker'. The integration tests have already specified the default credentials, so you don't want to worry about that.

```sh
docker run --name "postgis" -p 5432:5432 -d -t kartoza/postgis:12.4
```

Running the integration tests

```sh
dotnet test --filter Category=Integration
```
