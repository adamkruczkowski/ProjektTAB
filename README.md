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

## 2FA - konfiguracja
> [!IMPORTANT]
> 2FA można włączyć lub wyłączyć statycznie w kodzie (żeby nie bawić się ciągle z mailami, polecam wyłączyć). W pliku `AuthenticationController.cs` należy zmienić flagę `_is2FAEnabled` na odpowiednio `True` albo `False`

Do 2FA potrzebujemy serwera SMTP, ja zaimplementowałem ten z [mailtrap.io](https://mailtrap.io/) dlatego bo jest darmowy i łatwo go założyć. Żeby mieć to u siebie trzeba stworzyć tam konto, stworzyć nowy projekt na środowisko sandboxowe. 
Następnie, wchodzimy w nasz projekt, zakładka 
> Integration

I kopiujemy Username i Password (pic rel)

<img width="659" alt="image" src="https://github.com/adamkruczkowski/ProjektTABbackend/assets/91903793/1c029532-2241-4add-b938-e324423005ec">

w pliku `appsettings.json` dodajemy tę konfigurację:
```
    "Smtp": {
      "Host": "sandbox.smtp.mailtrap.io",
      "Port": 2525,
      "Username": "Mailtrap_USERNAME",
      "Password": "Mailtrap_PASSWORD",
      "EnableSsl": true,
      "FromEmail": "no-reply@example.com"
    }
```
