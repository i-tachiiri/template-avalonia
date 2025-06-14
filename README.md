# 🚀 Avalonia Desktop Template

> **Spec** → see [/spec/avalonia-desktop-template-spec.md](spec/avalonia-desktop-template-spec.md)
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
$ docker-compose up -d

# 3. Restore & run desktop app
$ dotnet restore
$ dotnet run --project src/App
```

> **Prerequisites** – .NET 8 SDK, Docker Desktop, (optional) Azure CLI.

### Sync to Azure (manual test)

```bash
$ dotnet run --project src/BackupService -- sync
```

---

## 🗂 Repository Layout

```text
/               # repo root
├─ src/         # C# projects
│   ├─ App/                # Avalonia desktop
│   ├─ BackupService/      # Sync logic library/CLI
│   └─ Functions/          # Azure Functions (Isolated Worker)
├─ infra/       # Bicep IaC (common + svc-backup)
├─ .github/     # Workflows & templates
├─ docker-compose.yml
└─ spec/        # Design specs & docs
```

---

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
