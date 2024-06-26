## Configuration
Nalezy pobrac baze danych Sql Server 2022 od Microsoft

Trzeba stworzyc plik `appsettings.json`w glownym katalogu i wkleic ponizsza zawartosc WRAZ z wypelnionym polem YOUR_SERVER.
Do pola nalezy wkleic wartosc "Server name" z pola logowania do bazy SSMS (konfigurowalo sie podczas instalacji)
Domyslnie jest chyba "localhost\MSSQLSERVER01"
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
    "PolBankConnectionString": "Server={YOUR_SERVER_NAME_};Database=PolBankDb;Trusted_Connection=True;TrustServerCertificate=True"
  }
}

UWAZAC NA PORT ORAZ CZY JEST HTTP CZY HTTPS
```
