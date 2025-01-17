# CS361-Assignment-8 - Trip Itinerary PDF Export API

## Overview
The Trip Itinerary PDF Export API provides an endpoint for generating a downloadable PDF file based on the itinerary provided in the request data. The PDF is generated from markdown content, which is passed through the API in the Itinerary field. The API validates the input data, and if valid, generates and returns the PDF file stream.

## Endpoint
`POST https://myassignment8-agf6b9a7bfhdgnar.eastasia-01.azurewebsites.net/Trip/export-itinerary`
**Note**: The downloaded file name can be extracted from the `Content-Disposition` header from the API response

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
- ***Success***: Returns a 200 OK response with the generated PDF byte streams.
- Content-Type: `application/pdf`
- Content-Disposition: `attachment; filename="Itinerary_{trip.Id}.pdf"`
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

## Example calling code using C#/NET 8
```
var requestPayload = new
{
    Id = "66aa0dad7746873165167ef9",
    Country = "USA",
    StartDate = "2024-08-01T00:00:00Z",
    EndDate = "2024-08-07T00:00:00Z",
    TripType = "Culture",
    Itinerary = "Here is the itinerary: **Day 1**: Arrival in New York City..."
};

using var httpClient = new HttpClient();
var jsonContent = new StringContent(
    System.Text.Json.JsonSerializer.Serialize(requestPayload),
    Encoding.UTF8,
    "application/json");

var response = await httpClient.PostAsync("https://myassignment8-agf6b9a7bfhdgnar.eastasia-01.azurewebsites.net/Trip/export-itinerary", jsonContent);
```


## Example response-handling code using C#/NET 8
```
response.EnsureSuccessStatusCode();

// Extract filename from the Content-Disposition header
string? fileName = null;
if (response.Content.Headers.ContentDisposition != null)
{
    fileName = response.Content.Headers.ContentDisposition?.FileName?.Trim('"');
}
var pdfDataStream = await response.Content.ReadAsStreamAsync();
using (var fileStream = new FileStream(fileName, FileMode.Create, FileAccess.Write))
{
    await pdfDataStream.CopyToAsync(fileStream);
}
```

## Additional Information
- The API expects the date fields (StartDate, EndDate) to be in `YYYY-MM-DD` format.
- The Itinerary field must be a valid markdown string. This field is required and must not be empty.
- Ensure the Content-Type header in the request is set to `application/json`.

## UML diagram
![UML diagram photo](./uml-diagram.png)
