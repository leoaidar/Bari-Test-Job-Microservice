# Bari-Test-Job-Microservice

Branch develop to run on Visual Studio or VSCode.
===================================================================
Branch master to run on Docker: command: docker-compose up --build

Run both in docker and visual studio two instances to view the communication between then

Links:

Documentation View messages GET /v1/message: http://localhost:5005/swagger/index.html

Dashboard Message Queue: http://localhost:15672/#/

Dashboard Health Checks: https://localhost:5006/hc-ui#/healthchecks

Dashboard Jobs: https://localhost:5006/hangfire

Microservice Tecnologies and Software Engineering:

.NET Core 3.1

ASP.NET WebAPI

DDD 

CQRS

SOLID

Clean Code

IoC

Dependency Injection

AutoMapper

ViewModel

TDD

Unit Tests

Multiple Repositories

Docker


Background Job on Hangfire

Message Queue BUS via AMQP Protocol on RabbitMQ 

Persistence Cache NoSQL with Redis

Health Check 


***INSTRUÇÕES

A Branch master está preparada  pra funcionar no container pelo Docker com o comando: docker-composer up --build

A Branch develop está preparada pra funcionar rodando pelo Visual studio ou VScode: 

Para subir as duas instancias basta levantar pelo Docker pela branch master e levantar pelo Visual Studio a branch develop.

As duas instancias vao compartilhar os mesmos recursos e irão se comunicar atraves de mensageria.

Acima coloquei os links de acesso de cada instancia, após levantar as instancias basta clicar aqui nos links citados.


