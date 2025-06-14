# 開発フローガイド

このテンプレートで新しいアプリを構築するときの設計・実装の進め方をまとめます。各ステップの詳細は必要に応じて個別のドキュメントに記録してください。

## 1. 要件整理

1. 目的・ターゲットユーザー、主要機能を洗い出す。
2. ``docs/requirements.md`` を作成し、機能一覧・非機能要件を記載する。
3. 重要な技術的選択や方針は ``docs/adr/`` 以下にADR(Architecture Decision Record)として残す。

## 2. データ設計

1. ドメインモデル(エンティティ、値オブジェクト、集約)を定義する。
2. ER図やテーブル定義を ``docs/data-design.md`` にまとめる。
3. インフラ層で利用するDBスキーマを決め、マイグレーションファイルを追加する。

## 3. アプリケーション/ユースケース設計

1. CQRSのコマンド・クエリを洗い出す。
2. ``src/Core/Application`` にインターフェースとDTOを定義する。
3. ユースケースごとにハンドラーのテストを先に作成する。

## 4. UI設計

1. 画面遷移図とワイヤーフレームを ``docs/ui-design.md`` に用意する。
2. MVVMパターンでViewModelを設計し、必要なサービスインターフェースをApplication層に追加する。
3. 画面ごとに最小のViewとViewModelを実装し、スタイルを ``src/Presentation.Desktop/Styles`` にまとめる。

## 5. インフラ実装

1. Application層のインターフェースを実装するクラスを ``src/Infrastructure`` に追加する。
2. EF CoreのDbContextとリポジトリ実装を作成し、SQLite/Azure SQLに接続する。
3. ローカル・クラウド両方で動作するよう ``appsettings.json`` と環境変数を整備する。
4. Azure リソースを CLI で作成し、取得した接続情報を ``.env`` に設定する。詳しくは ``docs/azure-setup.md`` を参照。

## 6. プレゼンテーション層実装

1. Desktopアプリのエントリポイント ``Program.cs`` でDI設定を行う。
2. 必要な画面(View)とViewModelを順次追加し、コマンド/クエリを呼び出す。
3. テーマやリソース、ロギング設定を仕上げる。

## 7. テスト & CI

1. DomainとApplicationのユニットテストを ``tests/Unit`` に実装する。
2. InfrastructureやFunctionsの統合テストを ``tests/Integration`` に追加する。
3. GitHub Actionsが全て成功することを確認する。

## 8. デプロイ & リリース

1. ``infra`` 以下のBicepテンプレートを使い、ステージング環境をデプロイする。
2. Azure Functionsとデスクトップアプリをリリースし、Squirrelの更新フィードを公開する。
3. 重要なリリースごとに ``CHANGELOG.md`` を更新し、タグを切る。

---

設計ドキュメントやADRは ``docs/`` 配下にMarkdownで管理するのが推奨です。READMEには概要のみを記載し、詳細は各ドキュメントへリンクしてください。
