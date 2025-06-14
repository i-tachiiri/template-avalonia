# 🤖 AGENT Manifest — Avalonia Desktop Template

> **Audience**: Pair‑coding AI that will generate/maintain the repository.
> **Linkage**: Read *spec/avalonia-desktop-template-spec.md* **first**; treat this file as execution rules.
> **Last‑updated**: 2025‑06‑14.

---

## 1. Mission

Produce and iteratively refine a GitHub template repository that satisfies all functional & non‑functional requirements defined in the Specification while keeping monthly Azure cost minimal.

---

## 2. Input Sources

1. **Specification** – `/spec/avalonia-desktop-template-spec.md` *(single source of truth)*.
2. **User Prompts** – ChatGPT / GitHub Issues labelled `ai:request`.
3. **Existing Code** – Repository contents on default (`main`) branch.

The agent **must refuse** any instruction that contradicts the Specification unless the instruction comes with spec revision.

---

## 3. Success Criteria

| #  | Criterion                                                                                                                |
| -- | ------------------------------------------------------------------------------------------------------------------------ |
| S1 | `dotnet build` & `dotnet test` succeed on Windows and macOS (GitHub Actions).                                            |
| S2 | Initial Azure deployment (`deploy-infra.yml`, `deploy-to-azure.yml`) passes using dummy RG names & secrets placeholders. |
| S3 | Desktop app runs offline, generates `logs/log-YYYYMMDD.txt`, and shows a sample CRUD workflow persisted to SQLite.       |
| S4 | Manual command `BackupService --sync` posts JSON to local Azurite endpoint and returns *200 OK* from Functions.          |
| S5 | `release-desktop.yml` produces signed installers and uploads to Blob `$web/updates`.                                     |

---

## 4. Agent Workflow

1. **Pull latest** `main` branch.
2. **Parse Specification** and compare with repo state.
3. **Plan** – create/modify files; update tests; adjust workflows.
4. **Execute** granular commits following §6 conventions.
5. **Push** to `ai/<ticket‑id>` branch; open PR with auto‑generated release notes.
6. Await human review. Respond to review comments within 24 h.

> ⏱ *Hard budget*: ≤30 CPU mins per action; network calls only to GitHub & Azure CLI.

---

## 5. Coding Standards

* **Language** – C# 11+. Nullable enabled. `async`/`await` only.
* **Style** – dotnet format (`.editorconfig` from repo) must pass.
* **Project layout** – keep namespace per folder (`App.Pages`, `App.Data`…).
* **Logging** – Via Serilog; never `Console.WriteLine` in production code.
* **Secrets** – Read from `IConfiguration`; do **not** hard‑code.

---

## 6. Commit & PR Conventions

* **Branch name** – `ai/{issue-number}-{slug}`.
* **Commit message** – Conventional Commits: `feat:`, `fix:`, `chore:`, `ci:`, `docs:`.
* **PR title** – `[AI] <conventional scope>: summary`.
* **PR body** – auto‑generated checklist:

  ```markdown
  - [ ] Builds pass (`dotnet build`)
  - [ ] Tests pass (`dotnet test`)
  - [ ] Workflows green
  - [ ] Spec compliance validated (link to artefact)
  ```

---

## 7. Tooling Rules

| Task         | Tool                         | Flags                                              |
| ------------ | ---------------------------- | -------------------------------------------------- |
| Build        | `dotnet build`               | `-c Release`                                       |
| Tests        | `dotnet test`                | –                                                  |
| Publish      | `dotnet publish`             | `-c Release -r win-x64 --self-contained` (example) |
| Deploy Infra | `az deployment group create` | **No** auto‑approval; require manual RG param.     |

---

## 8. Guardrails

* Max new dependencies per PR = **3**.
* No preview/experimental NuGet packages without explicit spec change.
* Azure pricing change ≥¥500/month requires human approval PR label `cost:review`.
* All code must be covered by ≥70 % line coverage (`coverlet`).

---

## 9. Exit Criteria (MVP)

1. Human tester can **clone, build, run, sync, and update** with zero manual Azure config besides adding secrets.
2. CI/CD green; README badges show passing.
3. Costs confirmed ≤¥800/month at idle.

> **EOF** – The agent must keep this file in sync with future spec revisions.
