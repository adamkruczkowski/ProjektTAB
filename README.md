## Configuration
`application.json` must have
```
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "PolBankConnectionString": "Server=localhost\\{YOUR_SERVER};Database=PolBankDb;Trusted_Connection=True;TrustServerCertificate=True"
  }
}

```
