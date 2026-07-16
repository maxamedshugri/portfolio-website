# Portfolio API (ASP.NET Core + SQL Server)

Backend-kan wuxuu u shaqeynayaa website-kaaga (`index.html`, `login.html`, `admin.html`).

## 1. Waxa aad u baahan tahay
- .NET 8 SDK ([dotnet.microsoft.com](https://dotnet.microsoft.com/download) ka soo deji)
- SQL Server (Express ama LocalDB ayaa filan) + SQL Server Management Studio (SSMS)
- Visual Studio 2022 (ikhtiyaari, laakiin waa ta ugu fudud)

## 2. Database-ka samee
Fur SSMS oo ku shaqee script-ka:
```
Database/create_database.sql
```
Kani wuxuu abuuraa database-ka `PortfolioDB` iyo tables-ka `Users` iyo `ContactMessages`. Waxaad ka arki doontaa in Users table-ku maranyahay marka hore — waa caadi, admin user-ka default-ka ah waxaa lagu abuuri doonaa tallaabada xigta.

## 3. Connection string-ga hubi
Fur `appsettings.json` oo hubi in `ConnectionStrings:DefaultConnection` uu la jaanqaadayo magaca SQL Server instance-kaaga (default-ku wuxuu isticmaalayaa `localhost` oo Windows Authentication ah).

## 4. Migrations samee oo API-ga bilaw
Terminal-ka ku fur folder-ka `PortfolioApi` oo isku day:

```bash
dotnet restore
dotnet tool install --global dotnet-ef   # hal mar oo keliya haddii aanad hore u lahayn
dotnet ef migrations add InitialCreate
dotnet ef database update
dotnet run
```

Marka aad `dotnet run` gasho, API-gu wuxuu ku shaqeyn doonaa:
```
https://localhost:5001
```
Waxaa si otomaatig ah loo abuuri doonaa admin user default ah:
- **Username:** `admin`
- **Password:** `Admin@123`

⚠️ Marka hore ee aad login-ka samayso, waxaa lagula talinayaa inaad password-ka bedasho (waxaad tan ku samayn kartaa SQL query ama endpoint dambe oo aad dari karto).

## 5. Website-ka furfur
Furfur `index.html` (adigoo isticmaalaya Live Server ama browser toos ah). Contact form-ku wuxuu fariinta u diri doonaa API-ga. `login.html` ayaad ku geli doontaa `admin` / `Admin@123`, kadibna waxaad u wareejin doontaa `admin.html` si aad u aragto fariimaha.

## Fayl-yada isbeddelay ee frontend-ka
`login.html` wuxuu lahaa laba bug — input-yada username iyo password-ka `id` attribute-kooda waa qalday (`id="admin"` iyo `id=1234`), sidaas darteed JavaScript-ku ma heli karin qiimahooda. Waxaa lagu saxay faylka cusub ee `frontend-fixed/login.html`.

## Endpoints-ka API-ga
| Method | URL | Sharaxaad |
|---|---|---|
| POST | `/api/auth/login` | Admin login, wuxuu soo celiyaa JWT token |
| POST | `/api/contact` | Fariin cusub (public, form-ka home page-ka) |
| GET | `/api/contact` | Dhammaan fariimaha (u baahan token) |
| PUT | `/api/contact/{id}/read` | Fariinta u calaamadee "read" |
| DELETE | `/api/contact/{id}` | Fariinta tirtir |
