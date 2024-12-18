Event Management System API

This API is designed to manage an event management system. It allows operations such as managing attendees, events, and registrations using endpoints for CRUD operations. Below is a comprehensive guide to the API and its functionality.

Table of Contents

Overview

Features

Endpoints

RegistrationController

EventsController

AttendeeController

Database Schema

Usage

Overview

The Event Management System API provides an interface to:

Create, read, update, and delete events.

Manage attendee records.

Register attendees to events and maintain relationships between them.

The API is built using ASP.NET Core and interacts with an SQL Server database.

Features

Separate controllers for events, attendees, and registrations.

Input validation to ensure data integrity.

Dependency on IConfiguration for database connection.

Clear error handling with HTTP responses.

Endpoints

RegistrationController

Handles operations related to event registrations.

GET /api/RegistrationController

Retrieve all registrations.

Response: List of registrations with fields RegistrationID, EventId, AttendeeId, and RegistrationDate.

POST /api/RegistrationController

Add a new registration.

Body:

{
  "EventId": 1,
  "AttendeeId": 2,
  "RegistrationDate": "2024-12-17T00:00:00"
}

Validation: Checks if EventId exists in the database.

Response:

200 OK: Registration added successfully.

400 Bad Request: Validation failed.

PUT /api/RegistrationController/{id}

Update an existing registration.

Body: Same as POST.

Response:

200 OK: Registration updated.

400 Bad Request: Update failed.

DELETE /api/RegistrationController/{id}

Delete a registration by ID.

Response:

200 OK: Registration deleted.

404 Not Found: Registration does not exist.

EventsController

Handles operations related to events.

GET /api/EventsController

Retrieve all events.

Response: List of events with fields EventID, Title, Location, Status, and Date.

POST /api/EventsController

Add a new event.

Body:

{
  "Title": "Annual Conference",
  "Location": "New York",
  "Status": "Scheduled",
  "Date": "2024-12-18T00:00:00"
}

Validation: Ensures Status is one of Cancelled, Completed, or Scheduled.

Response:

200 OK: Event added successfully.

400 Bad Request: Validation failed.

PUT /api/EventsController/{id}

Update an existing event.

Body: Same as POST.

Response:

200 OK: Event updated.

400 Bad Request: Update failed.

DELETE /api/EventsController/{id}

Delete an event by ID.

Response:

200 OK: Event deleted.

404 Not Found: Event does not exist.

AttendeeController

Handles operations related to attendees.

GET /api/AttendeesController

Retrieve all attendees.

Response: List of attendees with fields AttendeeID, Name, Email, and TicketType.

POST /api/AttendeesController

Add a new attendee.

Body:

{
  "Name": "John Doe",
  "Email": "john.doe@example.com",
  "TicketType": "VIP"
}

Validation: Ensures the email is unique.

Response:

200 OK: Attendee added successfully.

409 Conflict: Email already exists.

PUT /api/AttendeesController/{id}

Update an existing attendee.

Body: Same as POST.

Response:

200 OK: Attendee updated.

400 Bad Request: Update failed.

DELETE /api/AttendeesController/{id}

Delete an attendee by ID.

Validation: Ensures no registrations are linked to the attendee before deletion.

Response:

200 OK: Attendee deleted.

409 Conflict: Linked registrations exist.

Database Schema

The database consists of the following tables:

Events

Column

Type

EventID

INT (PK)

Title

VARCHAR(100)

Location

VARCHAR(100)

Status

VARCHAR(50)

Date

DATETIME

Attendees

Column

Type

AttendeeID

INT (PK)

Name

VARCHAR(100)

Email

VARCHAR(100)

TicketType

VARCHAR(50)

Registration

Column

Type

RegistrationID

INT (PK)

EventId

INT (FK)

AttendeeId

INT (FK)

RegistrationDate

DATETIME

Usage

Set up your SQL Server database with the above schema.

Configure the connection string in appsettings.json:

{
  "ConnectionStrings": {
    "ctc_dev_DBConnection": "YourDatabaseConnectionString"
  }
}

Start the application and use tools like Postman or curl to test the endpoints.

Ensure proper validation to maintain database integrity.

