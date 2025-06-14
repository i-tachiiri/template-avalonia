# 📝 Avalonia Desktop Template — **Clean Architecture Scaffold Specification**

> **Audience** – AI pair‑coding tools & human contributors.
> **Goal** – Provide a ready‑to‑fork *clean‑architecture* desktop template (Avalonia UI) with offline‑first data, cloud backup, and minimal Azure running cost.
> **Version** – 2025‑06‑14 (🏗 clean‑arch revision 1)

---

## 1 📌 High‑Level Requirements

| #   | Requirement                                                                        | Notes                                                  |
| --- | ---------------------------------------------------------------------------------- | ------------------------------------------------------ |
| R1  | **Presentation‑Desktop** = Avalonia UI (.NET 8) + CommunityToolkit.MVVM            | Strictly UI concerns only.                             |
| R2  | **Presentation‑Functions** = Azure Functions (Isolated Worker, .NET 8)             | Exposes application use‑cases over HTTP/Timer.         |
| R3  | **Application Layer** = CQRS handlers & DTOs                                       | Depends only on **Domain** & abstraction packages.     |
| R4  | **Domain Layer** = Entities, VOs, Domain Events                                    | No external dependencies.                              |
| R5  | **Infrastructure Layer** = EF Core (SQLite & Azure SQL), Blob SDK, Serilog sinks   | Implements interfaces declared in Application.         |
| R6  | **Local DB** = SQLite; **Cloud backup** = Azure SQL (Serverless) via sync use‑case | Trigger: App exit + Daily 03:00 JST.                   |
| R7  | **Auto‑Update** = Squirrel.Azure (or NetSparkleUpdater)                            | Feed in Blob `$web/updates/`.                          |
| R8  | **Logging** = Serilog RollingFile + optional Windows EventLog                      | Retain 7 days.                                         |
| R9  | **CI/CD** = GitHub Actions (build → test → release → deploy)                       | Workflows per layer.                                   |
| R10 | **IaC** = Bicep modules (common & svc‑specific)                                    | Idempotent.                                            |
| R11 | **Local Dev** = dev‑container + `docker‑compose` for Azurite & SQL Server          | Connection strings identical to prod via user‑secrets. |
| R12 | **Documentation** – README + Spec link + ADRs                                      | Markdown only.                                         |

---

## 2 🏛 Project Structure

```text
/ (root)
├─ src/
│   ├─ Core/
│   │   ├─ Domain/                 # Pure domain model
│   │   └─ Application/            # Use‑cases, interfaces, DTOs
│   ├─ Infrastructure/             # EF Core, Azure SDK, Serilog impls
│   ├─ Presentation.Desktop/       # Avalonia UI (no EF/SDK refs)
│   └─ Presentation.Functions/     # Azure Functions API / Timer
├─ tests/
│   ├─ Unit/                       # Domain & Application tests
│   └─ Integration/                # EF Core & Functions tests
├─ infra/
│   ├─ common/                     # KV, Log, OTEL, VNet
│   └─ svc-backup/                 # SQL Serverless, Storage, Functions
├─ .github/workflows/              # CI pipelines
├─ docker-compose.yml              # sqlserver + azurite
└─ spec/template-spec.md
```

### 2.1 Reference Rules

* **Domain** → *no‑one* (top of onion)
* **Application** → Domain
* **Infrastructure** → Application & Domain
* **Presentation.<XY>** → Application (via DI) *only* — **never** Infrastructure

> Enforced via `ProjectReference` structure & `ArchUnitNET` tests.

---

## 3 🛠 Generation Steps (AI‑Automation)

1. `dotnet new sln -n Template` and add projects per structure above.
2. Scaffold **Domain**: sample `Note` entity + `NoteId` VO.
3. Scaffold **Application**:

   * `INoteRepository`, `CreateNoteCommand`, `CreateNoteHandler` (MediatR pattern).
4. Scaffold **Infrastructure**:

   * EF Core DbContext implementing `INoteRepository` for SQLite/Azure SQL.
   * Serilog provider & `IBackupService` implementation.
5. Scaffold **Presentation.Desktop**:

   * MVVM sample view binding to `INoteService` (Application façade).
   * Serilog bootstrap (RollingFile + EventLog).
6. Scaffold **Presentation.Functions**:

   * `POST /api/sync` – receives diff, invokes `SyncNotesCommand`.
   * `TimerTrigger` – nightly `ExportBacpacCommand`.
7. Add **docker‑compose.yml** (SQL Server + Azurite) & **devcontainer.json**.
8. Create **tests**:

   * Domain rule tests (ArchUnitNET).
   * Application handler unit tests with in‑memory repository.
9. Produce **workflows**:

   * `build-test.yml` – matrix (win, mac, ubuntu) runs `dotnet test`.
   * `release-desktop.yml` – builds Squirrel package, signs, uploads.
   * `deploy-iac.yml` – manual trigger → deploys `infra/`.
10. Populate **README**, **CONTRIBUTING**, **AGENT.md** links.

---

## 4 🎯 Clean Architecture Policy Checks

| Policy                                        | Enforcement                                                              |
| --------------------------------------------- | ------------------------------------------------------------------------ |
| No Infrastructure reference from Presentation | `ArchUnitNET` test + `Directory.Build.props` `PrivateAssets="all"` guard |
| Application pure (no EF/SDK)                  | Package reference analyser (`dotnet ban‑packages`).                      |
| Async all the way                             | Roslyn analyser rule `VSTHRD200` enabled.                                |
| Logging only via `ILogger<T>` abstraction     | Custom analyzer; DI registers Serilog.                                   |

---

## 5 🔍 Post‑Generation Checklist

* [ ] `dotnet test` (coverage ≥70 %) passes Windows/macOS/Linux.
* [ ] `ArchUnitNET` policy tests green.
* [ ] Desktop app runs offline; can CRUD notes; logs under `logs/`.
* [ ] Manual `Sync` CLI posts to local Functions & merges to SQL.
* [ ] GitHub Actions all green.
* [ ] Monthly idle Azure cost ≤ ¥800.

---

## 6 🚀 Recommended Post‑Init Template Settings *(Merged)*

> **Scope** – These defaults are applied **after** running `dotnet new avalonia.app` and are baked into the template so every fork starts production‑ready.

### 6.1 `.csproj` & `Directory.Build.props`

| Purpose                    | Property example                                     | Notes                    |
| -------------------------- | ---------------------------------------------------- | ------------------------ |
| Self‑contained single file | `<SelfContained>true` / `<PublishSingleFile>true>`   | Portable EXE.            |
| Trim & link                | `<PublishTrimmed>true>`                              | Requires .NET 8.         |
| Startup perf               | `<PublishReadyToRun>true>`                           | Cross‑gen optimisations. |
| Treat warnings as errors   | `<TreatWarningsAsErrors>true>`                       | Code quality gate.       |
| RID matrix                 | `win-x64;osx-x64;linux-x64` via `RuntimeIdentifiers` | CI artefacts.            |

> Shared props live in `build/Directory.Build.props` and flow to every project.

### 6.2 MVVM & Dependency Injection Skeleton

1. Add **CommunityToolkit.MVVM** package.
2. Register ViewModels & Services in `Program.cs` (or `AppHostBuilder`).
3. Enforce **constructor injection**; prohibit service locator pattern.

```csharp
builder.Services.AddSingleton<MainWindowViewModel>();
builder.Services.AddTransient<INoteService, NoteService>();
```

### 6.3 Centralised Styles & Theme

* `Styles/_Colors.axaml` – brand colours.
* Include both `FluentTheme` light/dark; toggle via `RequestedThemeVariant`.
* Embed font & SVG icon set once.

### 6.4 Developer Experience & Diagnostics

| Feature             | Default                                           |
| ------------------- | ------------------------------------------------- |
| Hot‑Reload          | `<EnableHotReload>true>` + extension in `.vscode` |
| XAML previewer      | `Avalonia.Designer` NuGet.                        |
| Diagnostics overlay | `Avalonia.Diagnostics` auto‑enabled in Debug.     |
| GPU forcing         | `AVALONIA_GPU=1` env in `launchSettings.json`.    |

### 6.5 Cross‑Platform Packaging Scripts

| OS          | Tooling snippet                                 | Output       |
| ----------- | ----------------------------------------------- | ------------ |
| Windows     | `<WindowsPackageType>Msix</WindowsPackageType>` | `*.msix`     |
| macOS       | `scripts/pack_dmg.sh` (hdiutil + notarize)      | `*.dmg`      |
| Linux       | `scripts/pack_appimage.sh`                      | `*.AppImage` |
| Auto‑update | Squirrel.Azure feed (Blob)                      | SemVer dirs  |

### 6.6 Localization & A11y

* Use `Avalonia.Localization` and `/Resources/Strings.<culture>.axaml`.
* Apply `AutomationProperties.Name` to interactive controls.

### 6.7 Misc Initial Adjustments

* **Icons/Version** – `Assets/App.ico`, `AssemblyInfo.cs`.
* **Settings** – `IConfiguration` via `appsettings.json` even on Desktop.
* **Logging** – Serilog RollingFile + EventLog sinks.
* **Testing** – `Avalonia.Headless` snapshot UI tests.
* **Code style** – value rules in `.editorconfig`.

---

## 7 📚 References

* Clean Architecture – *The Onion Architecture* (Jeffrey Palermo) / *Clean Architecture* (R. Martin)
* ArchUnitNET – [https://github.com/TNG/ArchUnitNET](https://github.com/TNG/ArchUnitNET)
* Avalonia Docs – [https://avaloniaui.net/docs](https://avaloniaui.net/docs)
* MediatR – [https://github.com/jbogard/MediatR](https://github.com/jbogard/MediatR)
* Serilog – [https://serilog.net/](https://serilog.net/)
* Azure Functions (Isolated) – [https://learn.microsoft.com/azure/azure-functions/dotnet-isolated-process-guide](https://learn.microsoft.com/azure/azure-functions/dotnet-isolated-process-guide)
* Bicep – [https://learn.microsoft.com/azure/azure-resource-manager/bicep](https://learn.microsoft.com/azure/azure-resource-manager/bicep)
* Avalonia Packaging – [https://learn.microsoft.com/dotnet/desktop/packaging?view=netdesktop-8.0](https://learn.microsoft.com/dotnet/desktop/packaging?view=netdesktop-8.0)
* Squirrel.Azure – [https://github.com/shiftkey/squirrel.azure](https://github.com/shiftkey/squirrel.azure)

---

> **EOF** – Honour layer boundaries, generate all files, commit to `main`.
