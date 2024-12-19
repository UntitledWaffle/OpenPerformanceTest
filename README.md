# Event Management System API

This API is designed to manage an event management system, providing functionality for managing attendees, events, and registrations through various endpoints for CRUD operations.

---

## Table of Contents

- [Overview](#overview)
- [Features](#features)
- [Endpoints](#endpoints)
  - [RegistrationController](#registrationcontroller)
  - [EventsController](#eventscontroller)
  - [AttendeeController](#attendeecontroller)
- [Database Schema](#database-schema)
- [Usage](#usage)

---

## Overview

The Event Management System API enables the following operations:

- **Events**: Create, read, update, and delete events.
- **Attendees**: Manage attendee records.
- **Registrations**: Register attendees to events and maintain their relationships.

**Tech Stack**:
- Built using ASP.NET Core
- Interacts with an SQL Server database

---

## Features

- Separate controllers for events, attendees, and registrations.
- Input validation to ensure data integrity.
- Uses `IConfiguration` for database connections.
- Comprehensive error handling with meaningful HTTP responses.

---

## Endpoints

### RegistrationController

Manages event registrations.

- **GET** `/api/RegistrationController`
  - **Description**: Retrieve all registrations.
  - **Response**: List of registrations (fields: `RegistrationID`, `EventId`, `AttendeeId`, `RegistrationDate`).

- **POST** `/api/RegistrationController`
  - **Description**: Add a new registration.
  - **Request Body**:
    ```json
    {
      "EventId": 1,
      "AttendeeId": 2,
      "RegistrationDate": "2024-12-17T00:00:00"
    }
    ```
  - **Validation**: Ensures `EventId` exists in the database.
  - **Responses**:
    - `200 OK`: Registration added successfully.
    - `400 Bad Request`: Validation failed.

- **PUT** `/api/RegistrationController/{id}`
  - **Description**: Update an existing registration.
  - **Request Body**: Same as POST.
  - **Responses**:
    - `200 OK`: Registration updated.
    - `400 Bad Request`: Update failed.

- **DELETE** `/api/RegistrationController/{id}`
  - **Description**: Delete a registration by ID.
  - **Responses**:
    - `200 OK`: Registration deleted.
    - `404 Not Found`: Registration does not exist.

---

### EventsController

Handles event-related operations.

- **GET** `/api/EventsController`
  - **Description**: Retrieve all events.
  - **Response**: List of events (fields: `EventID`, `Title`, `Location`, `Status`, `Date`).

- **POST** `/api/EventsController`
  - **Description**: Add a new event.
  - **Request Body**:
    ```json
    {
      "Title": "Annual Conference",
      "Location": "New York",
      "Status": "Scheduled",
      "Date": "2024-12-18T00:00:00"
    }
    ```
  - **Validation**: Ensures `Status` is one of `Cancelled`, `Completed`, or `Scheduled`.
  - **Responses**:
    - `200 OK`: Event added successfully.
    - `400 Bad Request`: Validation failed.

- **PUT** `/api/EventsController/{id}`
  - **Description**: Update an existing event.
  - **Request Body**: Same as POST.
  - **Responses**:
    - `200 OK`: Event updated.
    - `400 Bad Request`: Update failed.

- **DELETE** `/api/EventsController/{id}`
  - **Description**: Delete an event by ID.
  - **Responses**:
    - `200 OK`: Event deleted.
    - `404 Not Found`: Event does not exist.

---

### AttendeeController

Manages attendee-related operations.

- **GET** `/api/AttendeesController`
  - **Description**: Retrieve all attendees.
  - **Response**: List of attendees (fields: `AttendeeID`, `Name`, `Email`, `TicketType`).

- **POST** `/api/AttendeesController`
  - **Description**: Add a new attendee.
  - **Request Body**:
    ```json
    {
      "Name": "John Doe",
      "Email": "john.doe@example.com",
      "TicketType": "VIP"
    }
    ```
  - **Validation**: Ensures the email is unique.
  - **Responses**:
    - `200 OK`: Attendee added successfully.
    - `409 Conflict`: Email already exists.

- **PUT** `/api/AttendeesController/{id}`
  - **Description**: Update an existing attendee.
  - **Request Body**: Same as POST.
  - **Responses**:
    - `200 OK`: Attendee updated.
    - `400 Bad Request`: Update failed.

- **DELETE** `/api/AttendeesController/{id}`
  - **Description**: Delete an attendee by ID.
  - **Validation**: Ensures no linked registrations exist.
  - **Responses**:
    - `200 OK`: Attendee deleted.
    - `409 Conflict`: Linked registrations exist.

---

## Database Schema

The database schema includes the following tables:

### Events Table
| Column    | Type         |
|-----------|--------------|
| EventID   | INT (PK)     |
| Title     | VARCHAR(100) |
| Location  | VARCHAR(100) |
| Status    | VARCHAR(50)  |
| Date      | DATETIME     |

### Attendees Table
| Column      | Type         |
|-------------|--------------|
| AttendeeID  | INT (PK)     |
| Name        | VARCHAR(100) |
| Email       | VARCHAR(100) |
| TicketType  | VARCHAR(50)  |

### Registration Table
| Column          | Type         |
|------------------|--------------|
| RegistrationID   | INT (PK)     |
| EventId          | INT (FK)     |
| AttendeeId       | INT (FK)     |
| RegistrationDate | DATETIME     |

---

## Usage

1. **Set up the Database**: Configure your SQL Server database using the schema above.
2. **Database Connection**: Update the `appsettings.json` file with your connection string:
   ```json
   {
     "ConnectionStrings": {
       "ctc_dev_DBConnection": "YourDatabaseConnectionString"
     }
   }
