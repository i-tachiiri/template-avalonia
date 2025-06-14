# 🚀 Avalonia Desktop Template

Clean Architecture scaffold for Avalonia UI apps.

> **Spec** → see [/spec/template-spec.md](spec/template-spec.md)
>
> **Purpose** – Jump‑start cross‑platform desktop apps (Windows/macOS/Linux) with **SQLite**, **Azure backup**, **auto‑update**, and GitHub Actions CI out‑of‑the‑box.

![Build](https://img.shields.io/github/actions/workflow/status/your-org/your-repo/test-and-build.yml?label=Build\&style=flat-square)
![Release](https://img.shields.io/github/actions/workflow/status/your-org/your-repo/release-desktop.yml?label=Installer\&style=flat-square)
![License](https://img.shields.io/github/license/your-org/your-repo?style=flat-square)

---

## ✨ Features

* **Avalonia UI (.NET 8)** – MVVM with CommunityToolkit.
* **Local-first data** – SQLite (EF Core) for offline work.
* **Cloud backup** – JSON diff ➜ Azure SQL (Serverless, auto‑sleep).
* **Auto‑update** – Squirrel.Azure feed in Blob Storage.
* **Logging** – Serilog RollingFile (+ Windows EventLog optional).
* **One‑command dev** – `docker-compose up` spins up Azurite + SQL Server.
* **Full CI/CD** – GitHub Actions: build, test, release, IaC deploy.
* **Cost‑optimised** – Idle cloud cost ≈ **¥500/月**.

---

## 🏃‍♂️ Quick Start

```bash
# 1. Clone
$ git clone https://github.com/your-org/your-repo.git && cd your-repo

# 2. Launch containers (SQL Server + Azurite)
# (ensure Docker Desktop is running)
$ docker-compose up -d

# 3. Restore & run desktop app
$ dotnet restore
$ dotnet run --project src/Presentation.Desktop
```

> **Prerequisites** – .NET 8 SDK, Docker Desktop, (optional) Azure CLI.

### Dev Container

Open in **VS Code** or **GitHub Codespaces** and choose **Reopen in Container**.
The `.devcontainer` setup spins up SQL Server and Azurite using
`docker-compose.yml` so the app runs with the same connection strings as
production.

### Sync to Azure (manual test)

```bash
$ dotnet run --project src/BackupService -- sync
```

For detailed Azure setup and required environment variables, see
[docs/azure-setup.md](docs/azure-setup.md).
After provisioning, copy `.env.example` to `.env` and fill in the connection strings.

---

## 🗂 Repository Layout

```text
/
├─ src/
│   ├─ Core/
│   │   ├─ Domain/
│   │   └─ Application/
│   ├─ Infrastructure/
│   ├─ Presentation.Desktop/
│   └─ Presentation.Functions/
├─ tests/
│   └─ Unit/
├─ infra/
├─ docker-compose.yml
└─ spec/
```

---
## 🛠 Development Flow

See [docs/development-flow.md](docs/development-flow.md) for a recommended order of design and implementation when building your app.

## 📦 Update Feed URL

Installers & RELEASES.json are published to:

```
https://<storage>.blob.core.windows.net/public/updates
```

The desktop app checks this URL at startup via Squirrel.

---

## 🔨 Development Scripts

| Task              | Command                                 |
| ----------------- | --------------------------------------- |
| Lint & format     | `dotnet format`                         |
| Run tests         | `dotnet test`                           |
| Build release     | `dotnet publish -c Release`             |
| Generate coverage | `dotnet test /p:CollectCoverage=true`   |
| Deploy infra\*    | `az deployment group create -f infra/…` |

\* **Azure login & resource group parameters required**.

---

## 🧪 CI Pipelines

| Workflow                  | Trigger          | Purpose                   |
| ------------------------- | ---------------- | ------------------------- |
| **test-and-build.yml**    | Any PR           | Build & unit tests        |
| **deploy-infra.yml**      | Manual/Tag       | Provision Azure (Bicep)   |
| **deploy-to-staging.yml** | Push → `develop` | Deploy to staging slot    |
| **deploy-to-azure.yml**   | Push → `main`    | Promote to prod           |
| **release-desktop.yml**   | GitHub Release   | Build & upload installers |

---

## 🤝 Contributing

We follow **Git flow** + **Conventional Commits**. Read
[`CONTRIBUTING.md`](CONTRIBUTING.md) before opening a PR. A PR checklist is auto‑injected from [`PULL_REQUEST_TEMPLATE.md`](PULL_REQUEST_TEMPLATE.md).

---

## 🔒 Security

Vulnerability reports → see [`SECURITY.md`](SECURITY.md).

---

## 📝 License

[MIT](LICENSE)

---

## 🙏 Acknowledgements

Built with ❤️ using [Avalonia](https://avaloniaui.net/) and [Azure](https://azure.microsoft.com/).
