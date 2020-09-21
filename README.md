# Checkout.Com Technical Test

## Technical Details

### Architecture
I've used `Clean Architecture` and `Docker Compose` while I was working on this test. This means that you will see `UseCase` in the solution and everything was grouped and placed under its own use case folder. It's really easy to navigate. I've also used `CQRS` and `Postgres` database. You will also see tests such as `UnitTests`, `IntegrationTests` and `AcceptanceTests`. 


### Folder Structure
 Folder structure consists of four separated files, first is `src` which has source files the other one is `test` which has tests projects. There is also `cicd` and `docker` which has `docker` files folder for `Continuous Integration` and `Continuous Deployment` purpose.


### Libraries
You can find what libraries I've used the following;

- Entity Framework Core
- XUnit
- FluentAssertion
- Xbehave
- Polly
- Serilog
- Swagger

 ## Build & Run
 To Ensure you have postgres database(ie if you dont have runnning on you local system alreay) running before starting the applicaiton, you run `docker-compose -f "docker\docker-compose.yml" up -d --build`.

 To run the project, navigate to `src/PaymentGateway.Api` folder and run `dotnet run`.

 
You can use `Swagger` using the following to explore the api.

- [Go to Swagger](https://localhost:5001/swagger/index.html)

You will find the api details in `Swagger` documentation and test it.