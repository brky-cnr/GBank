This is a solution for a simple online banking API app.

### Technologies
* .NET 5 WebAPI<br/>
* .NET Native DI<br/>
* MongoDB<br/>
* MediatR<br/>
* JWT Auth Bearer<br/>
* FluentValidator<br/>
* Xunit, FluentAssertions<br/>
* Swagger<br/>
* Docker<br/>

### Design Patterns
* OOP<br/>
* Rest Api<br/>
* Domain Driven Design<br/>
* Domain Events<br/>
* Domain Notification<br/>
* CQRS<br/>
* Inversion of Control / Dependency injection<br/>
* Mediator<br/>
* Health Check<br/>
* Middleware<br/>
* Response wrapper<br/>
* Error Handling, Global Exception<br/>

### Application
This layer contains all application logic. It's the layer where business process flows are handled. The capabilities of the application can be observed in this layer.<br/>
### Domain
This layer contains all entities, documents, enums, exceptions, interfaces, types and logic specific to the domain layer.<br/>
### Infrastructure
This layer contains classes for accessing external resources such as MongoDB, file systems, web services, smtp, and so on. These classes should be based on interfaces defined within the domain layer.<br/>

#### Local Setup
In order to build and run the dockerized API application, execute `docker-compose up --build` from the root of the solution where you find the docker-compose.yml file.<br/>
http://localhost:8080/swagger/index.html
#### Test 
http://ec2-3-85-84-172.compute-1.amazonaws.com:8080/swagger/index.html<br/>
Also, 27017 port is open for MongoDB connection. (`mongodb://3.85.84.172:27017`)<br/>

If you are having problems, please let me know.
 
