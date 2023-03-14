#Instruction 

 1. Publish CarsDB Database Project to SQL Server
 2. Define connectionStrings in appsettings.json (connectionStrings:defaultConnection)
 3. Define secret in appsettings.json (jwtConfig:secret)
 4. Run CarsAPI Project
 5. Register User.
 6. Login.
 7. Copy JWT Token from response
 8. Add string "Bearer <JWT token>" in Authorize


Note:
Profile for launchSettings.json

```json
{
    "profiles": {
        "CarsAPI": {
            "commandName": "Project",
            "dotnetRunMessages": true,
            "launchBrowser": true,
            "launchUrl": "swagger",
            "applicationUrl": "https://localhost:7182;http://localhost:5061",
            "environmentVariables": {
                "ASPNETCORE_ENVIRONMENT": "Development"
            }
        }
    }
}
```