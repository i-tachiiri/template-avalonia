# 🙌 Contributing Guide

> **Applies to**: *Avalonia Desktop Template Repository*
> **Last updated**: 2025‑06‑14

Welcome — we appreciate any help improving this template! Please read this guide **before** opening a Pull Request (PR) or Issue.

---

## 1. Code of Conduct

All contributors are expected to follow the [Contributor Covenant 2.1](https://www.contributor-covenant.org/version/2/1/code_of_conduct/) — be respectful.

---

## 2. Branching Model – *Git flow*

| Purpose           | Naming Convention           | Target Branch                             |
| ----------------- | --------------------------- | ----------------------------------------- |
| Stable production | `main`                      | –                                         |
| Integration       | `develop`                   | `main`                                    |
| Feature work      | `feature/<ticket-or-topic>` | `develop`                                 |
| Release prep      | `release/<version>`         | merge into *develop* **and** *main* (tag) |
| Hot‑fix           | `hotfix/<issue>`            | `main` (then back‑merge to *develop*)     |

### Quick Steps

```bash
# 1. Sync
git checkout develop && git pull origin develop

# 2. Create feature branch
git checkout -b feature/123-login-retry

# 3. Work & commit
# 4. Push & open PR → develop
```

> **Tip:** Use `git flow init -d` to initialise the defaults if you have git‑flow client installed.

---

## 3. Commit Style – *Conventional Commits*

```
<type>[optional scope]: <description>

[optional body]
[optional footer]
```

Valid **types**: `feat`, `fix`, `docs`, `style`, `refactor`, `test`, `ci`, `chore`, `perf`, `build`.

Example:

```
feat(data): add bulk‑merge helper for backup API
```

---

## 4. Pull Request Checklist

> A PR **must** satisfy **all** items below before requesting review.

* [ ] **Builds pass** – `dotnet build -c Release` on all OS.
* [ ] **Tests pass** – `dotnet test` (≥70 % line coverage).
* [ ] **Lint** – `dotnet format --verify-no-changes` has no diff.
* [ ] **Workflows** – GitHub Actions all green.
* [ ] **Spec compliance** – no contradiction with \[/spec/avalonia-desktop-template-spec.md].
* [ ] **Docs updated** – public APIs, configs, or behaviours.
* [ ] **Changelog** – update `CHANGELOG.md` under “Unreleased”.

### Review Rules

* At least **one core maintainer** approval + zero blocking comments.
* Re‑request review after each change pushed.
* Squash‑merge preferred (GitHub UI option).

---

## 5. Code Standards

| Area     | Rule                                                    |
| -------- | ------------------------------------------------------- |
| Language | C# 11+, `nullable enable`, `async`/`await` only         |
| Style    | .editorconfig enforced via `dotnet format`              |
| Logging  | Serilog only — **no** `Console.WriteLine` in production |
| Tests    | xUnit + FluentAssertions recommended                    |
| Coverage | `coverlet.collector` to report ≥70 %                    |

---

## 6. Running Locally

```bash
# Start containers
docker-compose up -d          # SQL Server + Azurite

# Restore & build
dotnet restore

dotnet build

# Start desktop app
cd src/App
 dotnet run
```

Use `Run → Attach to Process` in VS/VS Code to debug Azure Functions locally.

---

## 7. Issues & Discussions

* **Bugs / tasks**: open an *Issue* using the provided template.
* **Ideas / Q\&A**: create a *Discussion*.
* Label suggestions: `bug`, `enhancement`, `question`, `ai:request`, `cost:review`.

---

## 8. Security Reporting

Please disclose vulnerabilities privately — see `SECURITY.md`.

---

## 9. CLA / Licensing

By submitting code you agree it can be released under this repo’s MIT license.

---

Thank you for contributing 💙
