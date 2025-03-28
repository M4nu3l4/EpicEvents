# EpicEvents API 🎫🎤

EpicEvents è un'API RESTful sviluppata in **ASP.NET Core 8**, con **Entity Framework Core**, **ASP.NET Identity**, autenticazione **JWT**, logging con **Serilog**, **refresh token**, gestione utenti/ruoli, eventi, biglietti e artisti.

---

## 🔧 Requisiti

- .NET 8 SDK
- SQL Server o LocalDB
- Visual Studio o VS Code
- NuGet packages installati (vedi sotto)

---

## 📦 NuGet Packages principali

- Microsoft.EntityFrameworkCore.SqlServer
- Microsoft.AspNetCore.Identity.EntityFrameworkCore
- Microsoft.AspNetCore.Authentication.JwtBearer
- Serilog.AspNetCore
- Serilog.Sinks.File
- Serilog.Sinks.MSSqlServer
- Swashbuckle.AspNetCore

---

## 🚀 Avvio progetto

1. Configura la connection string in `appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=EpicEventsDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
}
```

2. Configura la chiave JWT in `appsettings.json`:

```json
"Jwt": {
  "Key": "chiave-segreta-jwt",
  "Issuer": "EpicEventsAPI",
  "Audience": "EpicEventsClient"
}
```

3. Esegui le migrazioni:

```bash
Add-Migration Init
Update-Database
```

4. Avvia il progetto con F5 o `dotnet run`.

---

## 👤 Utente di default

| Email                 | Password     | Ruolo           |
|----------------------|--------------|------------------|
| admin@epicevents.com | Admin123!    | Amministratore   |

Creato automaticamente tramite `SeedData`.

---

## 🔐 Autenticazione

- Login: `POST /api/account/login`
- Registrazione: `POST /api/account/register`
- Refresh Token: `POST /api/account/refresh`
- Claims attivi: `GET /api/account/me`

📌 **Swagger** supporta il login JWT con `Authorize 🔒`.

---

## 🛂 Ruoli & Policy

| Ruolo           | Permessi                                                                 |
|----------------|--------------------------------------------------------------------------|
| **Amministratore** | CRUD su eventi e artisti, gestione biglietti                          |
| **Utente**         | Acquisto e visualizzazione biglietti personali                        |

Policy usata: `"AdminOnly"` per controller `Evento`, `Artista`.

---

## 🔄 Refresh Token

- I token sono salvati nel DB (tabella `RefreshTokens`)
- Validità: 7 giorni
- Access token: ~10 minuti
- Endpoint: `POST /api/account/refresh`

---

## 📋 API principali

### 🎤 Artisti

- `GET /api/artista`
- `POST /api/artista`
- `PUT /api/artista/{id}`
- `DELETE /api/artista/{id}`

### 🎫 Eventi

- `GET /api/evento`
- `POST /api/evento`
- `PUT /api/evento/{id}`
- `DELETE /api/evento/{id}`

### 🎟 Biglietti

- `POST /api/biglietti` → acquisto biglietti (utente loggato)
- `GET /api/biglietti/miei` → visualizza biglietti propri

---

## 🪵 Logging

- Middleware custom:
  - `RequestLoggingMiddleware.cs` → log richieste utente, endpoint, tempo, IP
  - `ExceptionHandlingMiddleware.cs` → log errori globali, 401/403
- Serilog scrive su:
  - 📁 `Logs/log.txt`
  - 🗃 Tabella SQL `Logs`

---

## 📁 Struttura del progetto

```
EpicEvents/
├── Controllers/
│   ├── AccountController.cs
│   ├── ArtistaController.cs
│   └── EventoController.cs
│
├── Middleware/
│   ├── RequestLoggingMiddleware.cs
│   └── ExceptionHandlingMiddleware.cs
│
├── Models/
│   ├── ApplicationUser.cs
│   ├── Artista.cs
│   ├── Evento.cs
│   ├── Biglietto.cs
│   └── RefreshToken.cs
│
├── Data/
│   ├── ApplicationDbContext.cs
│   └── SeedData.cs
│
├── DTO/
│   ├── LoginDto.cs
│   ├── RegisterDto.cs
│   └── RefreshTokenDto.cs
│
├── Program.cs
└── appsettings.json
```

---

## ✅ Requisito EXTRA completato

✔ Tutti i metodi sensibili includono `try/catch` con logging Serilog nel `catch`  
✔ Log salvati sia su file che su DB (`Logs`)

---

## 📌 Note finali

🔐 Tutti gli endpoint sono testabili via Swagger con autenticazione JWT  
🧪 Logging avanzato per debug e sicurezza  
🔄 Refresh token sicuro e persistente  
