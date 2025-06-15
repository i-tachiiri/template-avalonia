# 開発フローガイド

このテンプレートで新しいアプリを構築するときの設計・実装の進め方をまとめます。各ステップの詳細は必要に応じて個別ドキュメントに記録してください。

---

## 0. 前提ツールインストール

* **Azure CLI** / Docker Desktop / .NET 8 SDK を導入（詳細は `docs/azure-setup.md`）。

---

## 1. 要件整理

1. 目的・ターゲットユーザー、主要機能を洗い出す。
2. **AI との対話で以下の初期ドキュメントを生成し、`docs/` 配下に配置する。**

   * `docs/requirements.md` — 機能一覧・非機能要件を記載
   * `docs/data-design.md` — ドメインモデル & テーブル定義のドラフト
   * `docs/ui-design.md` — 画面一覧・遷移図・ワイヤーフレームのたたき台
   * `docs/architecture/application-design.md` — CQRS 一覧 & フォルダ構成指針
3. 重要な技術的選択や方針は `docs/adr/` 以下に ADR を残す。

## 2. データ設計. データ設計

1. ドメインモデル(エンティティ、値オブジェクト、集約)を定義する。
2. ER 図やテーブル定義を `docs/data-design.md` にまとめる。
3. インフラ層で利用する DB スキーマを決め、マイグレーションファイルを追加する。

## 3. アプリケーション / ユースケース設計

1. CQRS のコマンド・クエリを洗い出す。
2. `src/Core/Application` にインターフェースと DTO を定義。
3. ユースケースごとにハンドラーのテストを先に作成する。

## 4. UI 設計

1. 画面遷移図とワイヤーフレームを `docs/ui-design.md` に用意。
2. MVVM パターンで ViewModel を設計し、必要なサービス I/F を Application 層に追加。
3. 画面ごとに最小の View と ViewModel を実装し、スタイルを `src/Presentation.Desktop/Styles` にまとめる。

## 5. 環境セットアップ（ローカル & Azure）

1. **ローカル開発環境** – `docker compose up -d` で **SQL Server + Azurite** を起動。
2. **Azure リソースデプロイ** – `infra/main.bicep` を `az deployment group create` で展開。
3. CLI で取得した接続文字列を `.env` または GitHub Secrets に登録。
4. 手順詳細は `docs/azure-setup.md` を参照。

> 🕒 **実施タイミング**: UI 基盤が動く段階（ステップ 4 完了直後）で行うと、後続の機能をクラウド上でも即検証できる。

## 6. インフラ実装

1. Application 層インターフェースの実装を `src/Infrastructure` に追加。
2. EF Core の DbContext / リポジトリを作成し SQLite・Azure SQL に接続。
3. `appsettings.json` と環境変数でローカル／クラウド両対応。
4. Azure Functions プロジェクトを用意し、HTTP／Timer トリガーでユースケースを公開。

## 7. プレゼンテーション層実装

1. `Program.cs` で DI 設定。
2. 画面と ViewModel を順次追加し、コマンド／クエリを呼び出す。
3. テーマ・リソース・ロギングを仕上げる。

## 8. テスト & CI

1. Domain と Application のユニットテストを `tests/Unit` に実装。
2. Infrastructure／Functions の統合テストを `tests/Integration` に追加。
3. GitHub Actions でビルド → テスト → パッケージ → デプロイを自動化。

## 9. デプロイ & リリース

1. `infra` 以下の Bicep でステージング環境をデプロイ。
2. Azure Functions とデスクトップアプリをリリースし、Squirrel 更新フィードを公開。
3. 重要リリースごとに `CHANGELOG.md` を更新し、タグを付与。

---

> 設計ドキュメントや ADR は `docs/` 配下に Markdown で管理するのが推奨。README には概要のみを記載し、詳細は各ドキュメントへのリンクを設置してください。
