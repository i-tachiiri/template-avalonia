# 📝 Avalonia Desktop Template — **AI Scaffold Specification**

> **Audience** – An AI pair‑coding tool that generates a GitHub template repository.
> **Goal** – Produce a ready‑to‑fork scaffold for a single‑developer desktop app with cloud backup & update delivery, minimising cost and complexity.
> **Version** – 2025‑06‑14 (initial)

---

## 1 📌 High‑Level Requirements

| #   | Requirement                                                                   | Notes                                              |
| --- | ----------------------------------------------------------------------------- | -------------------------------------------------- |
| R1  | **Avalonia UI (.NET 8)** with CommunityToolkit.MVVM boiler‑plate              | Cross‑platform (Win/macOS/Linux).                  |
| R2  | **Local DB = SQLite** via EF Core                                             | Migrations included.                               |
| R3  | **Cloud backup** to *Azure SQL Serverless* via JSON diff sync                 | Trigger = app exit + Timer (03:00 JST).            |
| R4  | **Azure Functions (Isolated Worker, .NET 8)** handles backup API & bulk‐merge | Consumption plan only.                             |
| R5  | **Logging** with *Serilog*—RollingFile (always) + optional Windows EventLog   | File retention = 7 days.                           |
| R6  | **Auto‑update** using *Squirrel.Azure* (or NetSparkle)\*                      | Feed hosted in Blob `$web/updates/`.               |
| R7  | **CI/CD** via GitHub Actions                                                  | Build, test, sign, upload installers + IaC deploy. |
| R8  | **Infra as Code** with Bicep modules                                          | Common (VNet, Log) + svc‑specific.                 |
| R9  | **Local dev containers** + `docker‑compose` for SQL Server & Azurite          | Match prod connection strings.                     |
| R10 | **README / Docs** link back to this spec so AI tools stay aligned             | Markdown only.                                     |

\* *AI may choose NetSparkleUpdater if Squirrel issues arise on macOS.*

---

## 2 🚀 Repository Layout

```text
/ (root)
├─ .github/workflows/           # CI pipelines
├─ infra/                       # Bicep IaC
│   ├─ common/                  # Shared resources
│   └─ svc-backup/              # Functions, SQL, Storage
├─ src/
│   ├─ App/                     # Avalonia UI project
│   ├─ BackupService/           # SQLite ↔ Azure sync library
│   └─ Functions/               # Azure Functions project
├─ docker-compose.yml           # sqlserver + azurite
└─ spec/avalonia-desktop-template-spec.md
```

---

## 3 🛠  Generation Steps (AI)

1. **Create solution** `MyTemplate.sln`.
2. Generate **Avalonia** project (`src/App`) with:

   * DI via `Microsoft.Extensions.*`
   * MVVM sample (MainViewModel + MainWindow).
   * `Serilog` initialisation in `Program.cs`:

     ```csharp
     Log.Logger = new LoggerConfiguration()
         .MinimumLevel.Debug()
         .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day, retainedFileCountLimit: 7)
         .WriteTo.EventLog("MyApp", manageEventSource:true, restrictedToMinimumLevel:LogEventLevel.Warning)
         .CreateLogger();
     ```
   * RollingFile path `logs/` (relative).
3. Add **SQLite** EF Core context & sample migration in `App.Data`.
4. Scaffold **BackupService** (`src/BackupService`):

   * `IBackupOrchestrator` – detects changes, queues JSON diff.
   * `HttpClient` wrapper posting to Functions `/api/sync` endpoint.
5. Create **Functions** project (`src/Functions`):

   * `HttpTrigger` `POST /sync` – validates API key, bulk‐merges.
   * `TimerTrigger` nightly bacpac export to cold Blob.
6. **Docker compose**:

   ```yaml
   services:
     sqlserver:
       image: mcr.microsoft.com/mssql/server:2022-latest
       environment:
         SA_PASSWORD: "YourStrong!Password1"
         ACCEPT_EULA: "Y"
       ports: ["1433:1433"]
     azurite:
       image: mcr.microsoft.com/azure-storage/azurite
       environment:
         AZURITE_ACCOUNTS: "devstoreaccount1:${AZURITE_KEY}"
       ports: ["10000:10000"]
   ```
7. **CI pipelines** (`.github/workflows`):

   * `test-and-build.yml` – PR trigger, runs `dotnet test`, builds.
   * `release-desktop.yml` – tag trigger, publishes self‑contained executables, signs (powershell signtool step placeholder), runs `Squirrel --releasify`, uploads to Blob.
   * `deploy-infra.yml` – manual, runs `az deployment group create` for `infra/`.
8. **Bicep modules** (`infra/`): minimal serverless SQL, Storage, Functions with MI.
9. Provide **Winget manifest** sample (optional) under `installer/`.
10. Add **README** with local dev steps + update feed URL doc.

---

## 4 🔍 Post‑Generation Checklist

* [ ] *dotnet build* passes on Windows & macOS.
* [ ] App runs offline; editing sample data persists.
* [ ] `BackupService` successfully posts to Azurite endpoint in dev.
* [ ] `Functions` unit tests cover bulk merge logic.
* [ ] GitHub Actions complete without secrets (use `${{ secrets.* }}` placeholders).
* [ ] README shows badge links for build status.

---

## 5 📚 References

* Avalonia Docs – [https://avaloniaui.net/docs](https://avaloniaui.net/docs)
* Serilog Docs – [https://serilog.net/](https://serilog.net/)
* Squirrel .Windows – [https://github.com/Squirrel/Squirrel.Windows](https://github.com/Squirrel/Squirrel.Windows)
* NetSparkleUpdater – [https://github.com/NetSparkleUpdater/NetSparkle](https://github.com/NetSparkleUpdater/NetSparkle)
* EF Core – [https://learn.microsoft.com/ef/core](https://learn.microsoft.com/ef/core)
* Azure Functions Isolated Worker – [https://learn.microsoft.com/azure/azure-functions/dotnet-isolated-process-guide](https://learn.microsoft.com/azure/azure-functions/dotnet-isolated-process-guide)
* Bicep – [https://learn.microsoft.com/azure/azure-resource-manager/bicep](https://learn.microsoft.com/azure/azure-resource-manager/bicep)

---

> **EOF** – AI should create all files, commit initial code, and push to `main`.
