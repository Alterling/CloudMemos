# Cloud Memos

Cloud Memos is a test application

## Swagger

[Local](http://localhost:65432/swagger/index.html) <br/>
 
 ## Implemented:
 API application using ASP.NET Core / AWS Lambda with the functionality:
 * Create text fragments via POST /v2/CloudMemosV2 and /v1/CloudMemosV1
 * Read text fragments back via GET /v2/CloudMemosV2/{id} and /v1/CloudMemosV1/{id}
 * Sort paragraps in text fragments via PUT /v2/CloudMemosV2/{id}/Sort/{sort}
 * Obtain text statistics via /v2/CloudMemosV2/{id}/TextStatistics
 
 ## Not Implemented:
 * AWS configuration and deployment
 * Authentication
 * CI chain
 * Test and production isolation
 
 ## Current limitations
 Due to issues with deployment, the persistance was limited to in-memory storage. Switch to DynamoDB storage was planned.