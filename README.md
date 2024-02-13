# Telecom Service API

## Introduction
This project is an assessment to demonstrate my understanding and skills in C#, ASP.NET Core, RESTful APIs. 
The main functionality includes:
- get all phone numbers
- get all phone numbers of a single customer
- add a new phone number to a customer’s account
- activate a phone number
- get all customers

## Prerequisites
- .NET Core SDK v7.0.402

## Database
- Azure SQL Database.

## Hosting
- Azure App Service: https://telecomserviceapi.azurewebsites.net/
- Swagger UI: https://telecomserviceapi.azurewebsites.net/swagger

## Assumptions
- No adding, updating, and deleting actions for Customer
- Deleting and inactiving a phone number operations are not included in the system

## Clean Architecture
### Core Layer: PhoneNumber/Customer Entity
- the innermost circle in Clean Architecture
- independent of external influences
- represents a phone number/customer within the system.
- PhoneNumber Attributes: `Id, PhoneNumberValue, CustomerId, Active`
- Customer Attributes: `Id, Name`

### Infrastructure Layer: PhoneNumberRepository (IPhoneNumberRepository)
- outside of the Core layer.
- acts as a bridge between the core business logic and data access mechanisms.
- create, read and update operations for the `PhoneNumber` entity
- implements the `IPhoneNumberRepository` interface for abstract the operations

### Application Layer: PhoneNumberService (IPhoneNumberService)
- sits between the Infrastructure and Api layers
- execute business operations and apply specific business rules
- applies necessary transformations on `PhoneNumber` data before or after it's retreived
- implements the `IPhoneNumberService` interface for abstract the operations
    
### Api Layer: PhoneNumberController
- the outermost layer, handling HTTP requests
- translate HTTP request into data and pass to PhoneNumberService (Application Layer) to proceed the next operations
- implements the `ControllerBase` interface for abstract the operations
