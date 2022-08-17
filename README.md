#JSON-RPC over HTTP application
Author: Stefano Franceschi
Description: Backend developer assignment

The repository contains a .NET Solution, with the following C# projects:

1. JsonRpc.API: is a .NET6 WebApi application which will expose the Json-Rpc endpoint

2. JsonRpc.Core: a library project containing the business logic for the application including the interfaces and implementations to fetch data form the external service

3. JsonRpc.Tests: is the project containing unit and integration tests (using MSTest and Moq)

## Getting Started with Docker image

once downloaded the source code or cloned the repository the application can be started with the included script to create a Docke image as follows:

-open a terminal window in the root directory and run:

#### `docker build -t dockerwebapi -f Dockerfile .`

-run the following command to start the webapi on port 8080:

#### `docker run -ti --rm -p 8080:80 dockerwebapi`

-now you can start sending requests to http://localhost:8080/api/JsonRpc
-for testing purposes a swagger UI is available at http://localhost:8080/swagger/index.html

## JSON-RPC exposed endpoint

### POST /api/JsonRpc

is the only endpoint available, requests need mandatory fields are: id,jsonrpc,method and params, params at the moment have only one schema, but when implementing more methods could change their schema.

## Request body

```json
{
  "id": 1,
  "jsonrpc": "2.0",
  "method": "GetOilPriceTrend",
  "params": {
    "startDateISO8601": "2022-08-01",
    "endDateISO8601": "2022-08-05"
  }
}
```

## Responses

### 200 - OK when succesfully retrieved requested data (if any for the requested dates)

```json
{
  "id": 1,
  "jsonrpc": "2.0",
  "result": {
    "prices": [
      {
        "dateISO8601": "2022-08-01",
        "price": 106.09
      },
      {
        "dateISO8601": "2022-08-02",
        "price": 106.51
      },
      {
        "dateISO8601": "2022-08-03",
        "price": 101.82
      },
      {
        "dateISO8601": "2022-08-04",
        "price": 97.99
      },
      {
        "dateISO8601": "2022-08-05",
        "price": 100.31
      }
    ]
  }
}
```

### 400 - Bad Request the request is not well formatted (ex. missing mandatory parameters or wrong values)
