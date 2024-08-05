# CS361-Assignment-8 - Trip Itinerary PDF Export API

## Overview
The Trip Itinerary PDF Export API provides an endpoint for generating a downloadable PDF file based on the itinerary provided in the request data. The PDF is generated from markdown content, which is passed through the API in the Itinerary field. The API validates the input data, and if valid, generates and returns the PDF file.

## Endpoint
`POST /trip/export-itinerary`
## Description
This endpoint accepts a JSON payload containing details about a trip and generates a PDF document based on the provided itinerary. The generated PDF is returned as a downloadable file.

## Request Body
The request body must be a JSON object that conforms to the following structure:
```
{
    "Id": "string",            // Optional: Unique identifier for the trip
    "Country": "string",       // Optional: The country where the trip is taking place
    "StartDate": "string",     // Optional: The start date of the trip (format: YYYY-MM-DD)
    "EndDate": "string",       // Optional: The end date of the trip (format: YYYY-MM-DD)
    "TripType": "string",      // Optional: The type of trip (e.g., Culture, Adventure, etc.)
    "Itinerary": "string"      // Required: The itinerary of the trip in markdown format
}
```

## Validation Rules
- Id: Optional, string.
- Country: Optional, string.
- StartDate: Optional, must be a valid date (format: YYYY-MM-DD).
- EndDate: Optional, must be a valid date (format: YYYY-MM-DD).
- TripType: Optional, string.
- Itinerary: Required, string, must not be empty.

## Response
- ***Success***: Returns a 200 OK response with the generated PDF file.
- Content-Type: `application/pdf`
- Content-Disposition: `attachment; filename="Itinerary_{trip.Id}.pdf"`
- A binary PDF file will be downloaded.
- ***Validation Error***: Returns a 400 Bad Request response with details of the validation errors.
- Example Failure Response:
```
{
    "Message": "Validation failed.",
    "Errors": [
        "The Itinerary field is required.",
        "Invalid start date format."
    ]
}
```

- Server Error: Returns a 500 Internal Server Error response if something goes wrong during PDF generation.
```
{
    "Message": "An error occurred while generating the PDF: [Error details]"
}
```

## Example Request
### Using curl
Here is an example of how to send a POST request to the API using curl:
```
curl -X POST http://localhost:5000/trip/export-itinerary \
     -H "Content-Type: application/json" \
     -d '{
           "Id": "123456",
           "Country": "USA",
           "StartDate": "2024-08-01",
           "EndDate": "2024-08-07",
           "TripType": "Culture",
           "Itinerary": "Here is the itinerary: **Day 1**: Arrival in New York City..."
         }' \
     -o "Itinerary_123456.pdf"
```

## Additional Information
- The API expects the date fields (StartDate, EndDate) to be in YYYY-MM-DD format.
- The Itinerary field must be a valid markdown string. This field is required and must not be empty.
- Ensure the Content-Type header in the request is set to application/json.
