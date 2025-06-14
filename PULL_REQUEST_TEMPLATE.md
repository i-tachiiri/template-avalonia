<!--
Pull Request Template – Avalonia Desktop Template Repository
Last updated: 2025‑06‑14
See CONTRIBUTING.md §4 for detailed guidance.
-->

## 📋 Overview

*Describe what this PR changes and why. Link any related Issue(s).*
Fixes #

---

## ✅ Checklist (MUST pass before review)

### Build & Test

* [ ] `dotnet build -c Release` succeeds on **Windows** and **macOS**
* [ ] `dotnet test` passes (coverage ≥ 70 %)
* [ ] `dotnet format --verify-no-changes` shows no style diffs

### CI / Workflows

* [ ] All GitHub Actions are green
* [ ] No new secrets or hard‑coded credentials added

### Spec & Docs

* [ ] Changes comply with \[/spec/template-spec.md]
* [ ] Public APIs / configs documented (README or docs/\* updated)
* [ ] `CHANGELOG.md` updated under **Unreleased**

### Cost & Dependencies

* [ ] Additional Azure cost < **¥500/month** (else label `cost:review`)
* [ ] No preview/experimental NuGet packages introduced
* [ ] New dependencies ≤ 3 and rationale provided below

---

## ℹ️ Dependencies / Cost Notes (if any)

*List new NuGet packages or infra costs introduced and justify.*

---

## 🧪 Manual Test Steps (optional)

1. …
2. …

---

## 📸 Screenshots / GIF (optional)

*Provide UI screenshots if visual changes.*

---

## 🔄 How to Rebase / Merge

> **Squash‑merge** preferred. Ensure commit message follows **Conventional Commits**.

---
