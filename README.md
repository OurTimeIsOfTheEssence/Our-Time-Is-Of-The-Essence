# 📦 OurTime

En enkel webbapplikation för klockor, byggd med C# och ASP.NET Core MVC. Här kan man:

* ✨ Bläddra bland olika klockmodeller
* ℹ️ Se detaljer om varje klocka
* 🛒 Lägga klockor i kundvagn
* 📝 Skriva och läsa recensioner (om man är inloggad)
* 🔐 Registrera sig och logga in

---

## 🚀 Kom igång

### 1. Klona projektet

Öppna en terminal (PowerShell / CMD) och skriv:

```bash
git clone https://github.com/OurTimeIsOfTheEssence/Our-Time-Is-Of-The-Essence.git
cd OurTime/src/OurTime.WebUI
```

### 2. Installera beroenden

Se till att du har .NET SDK installerad (minst .NET 6). Kör sedan:

```bash
dotnet restore
```

### 3. Skapa och initiera databasen

Projektet använder Entity Framework Core med SQLite (standard). För att skapa databasen och lägga in exempeldata:

```bash
dotnet ef database update
dotnet run --project ../OurTime.Infrastructure/DesignTimeDbContextFactory.csproj
```

> **OBS!** Om du vill använda SQL Server ändrar du `ConnectionStrings` i `appsettings.json`:
>
> ```json
> "ConnectionStrings": {
>   "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=OurTimeDb;Trusted_Connection=True;"
> }
> ```

### 4. Starta applikationen

I samma mapp (`OurTime.WebUI`) kör du:

```bash
dotnet run
```

Öppna sedan webbläsaren på:

- **HTTPS**: `https://localhost:<PORTNUMMER>`
- **HTTP**:  `http://localhost:<PORTNUMMER>`

(där `<PORTNUMMER>` är det som står i din konsol under “Now listening on…” när du kör `dotnet run`)


---

## 🗂️ Projektstruktur

```
OurTime/
├─ OurTime.Domain/          ← Domänmodeller (Watch, Product, User m.m.)
├─ OurTime.Application/     ← Affärslogik och gränssnitt (t.ex. IWatchRepository)
├─ OurTime.Infrastructure/  ← Databaskonfiguration och seed-data
└─ OurTime.WebUI/           ← MVC-webbprojekt (Controllers, Views, Static files)
```

* **Domain**: Definierar entiteter (Watch, Product) och värdeobjekt (Money)
* **Application**: Interface för repository och tjänster
* **Infrastructure**: Entity Framework-konfiguration, migrations och seed-metoder
* **WebUI**: MVC-kontrollers, vyer (Razor Pages), JavaScript/CSS

---

## 🔧 Teknikstack

* Språk: C#
* Framework: ASP.NET Core MVC
* Databas: SQLite (eller SQL Server om du ändrar connection string)
* ORM: Entity Framework Core
* Frontend: Razor Views, enkel CSS/JavaScript

---

## 🤝 Bidra

1. Forka detta repo
2. Skapa en feature-gren:

   ```bash
   git checkout -b feature/min-ändring
   ```
3. Committa dina ändringar:

   ```bash
   git commit -m "Lägg till ny funktion"
   ```
4. Pusha till din fork:

   ```bash
   git push origin feature/min-ändring
   ```
5. Skicka en pull request på GitHub.

---

## 📄 Licens

Detta projekt är licensierat under [MIT License](LICENSE).
