
# 📄 Requirements ― Horoscope PDF Generator

## 1. 概要

| 項目      | 内容                                                         |
| ------- | ---------------------------------------------------------- |
| プロジェクト名 | **AstroReport** 🟦 *(仮)*                                   |
| 目的      | 占い師が鑑定対象者の出生情報を入力し、解釈テキストとチャート画像を合成した PDF レポートを自動生成・履歴管理する |
| ターゲット   | プロ占い師（個人 / 小規模オフィス）                                        |

---

## 2. 背景・課題

* 鑑定コメントを手作業で PDF にまとめる工数が大きい
* チャート画像を外部占星術アプリで生成しており、画像貼り付け作業やレイアウト崩れが頻発
* ハウスシステムや使用天体の設定は占い師ごとに流儀が異なり、設定ミスが起こりやすい
* サブスクリプション型 SaaS は維持費が高く、オフライン対応も求められる

---

## 3. 用語

| 用語                   | 定義                                  |
| -------------------- | ----------------------------------- |
| **Astrologer (占い師)** | 本アプリのライセンスを持つユーザー。個別設定・解釈辞書を保持      |
| **Client (鑑定対象者)**   | 占い師が鑑定する人物                          |
| **BirthInfo**        | 生年月日・出生時刻・出生地（緯度経度・タイムゾーン）          |
| **ChartData**        | SWETL で計算された天体配置・ハウス配置（数値データ）       |
| **ChartImage**       | 占い師が別ツールで生成しアップロードする PNG/SVG 画像ファイル |
| **Interpretation**   | アスペクト・ハウスなど条件別の解釈テキスト（DB テーブルで管理）   |
| **Report**           | QuestPDF で出力される PDF ファイル            |
| **Monitoring**       | ログ収集、メトリクス監視、アラート設定をまとめた運用仕組み       |

---

## 4. 機能一覧

### 4.1 コア機能

| ID    | 機能                 | 優先度 | 説明                                  |
| ----- | ------------------ | --- | ----------------------------------- |
| FR-01 | 出生情報入力             | ★★★ | 手入力または Google スプレッドシート取り込み          |
| FR-02 | チャート計算 (ChartData) | ★★★ | SWETL による天体・ハウス計算                   |
| FR-03 | チャート画像アップロード       | ★★★ | 占い師が別ツールで生成した ChartImage を登録        |
| FR-04 | テキスト抽出             | ★★★ | DB の Interpretation から該当解釈を抽出       |
| FR-05 | PDF 生成             | ★★★ | ChartImage + テキストを合成して QuestPDF で出力 |
| FR-06 | 占い師設定              | ★★☆ | ハウス方式・使用天体・架空星など個別設定                |
| FR-07 | テキスト辞書編集           | ★★☆ | 占い師が Interpretation（DB）を CRUD       |
| FR-08 | レポート履歴             | ★☆☆ | 生成済み PDF の閲覧・再ダウンロード                |

### 4.2 運用・拡張機能

| ID    | 機能           | 優先度 | 説明                                             |
| ----- | ------------ | --- | ---------------------------------------------- |
| OP-01 | ログ収集／エラーレポート | ★★☆ | Serilog + Application Insights へ送信し、ダッシュボード表示  |
| OP-02 | 障害アラート       | ★★☆ | Azure Monitor で Critical ログを Slack / Teams に通知 |
| OP-03 | 自動アップデート     | ★★☆ | Squirrel による差分更新フィード提供                         |
| OP-04 | バージョン互換テスト   | ★☆☆ | GitHub Actions で回帰テスト＋PDF レンダリングスナップショット比較     |

---

## 5. 非機能要件

| 区分      | 指標       | 目標値                      |
| ------- | -------- | ------------------------ |
| 性能      | PDF 出力時間 | **≤ 10 秒**（平均）           |
| 保守性     | テストカバレッジ | Domain + Application 80% |
| 運用コスト   | Azure 課金 | **≤ 5 USD / 月 / 占い師**    |
| クロスプラット | OS 対応    | Windows 10+ / macOS 13+  |
| 可観測性    | メトリクス遅延  | ログ/メトリクス 1 分以内集約         |

---

## 6. 技術選定（Clean Architecture + Infra）

|     |                                  |                                                             |                                   |
| --- | -------------------------------- | ----------------------------------------------------------- | --------------------------------- |
| R1  | **Presentation‑Desktop**         | Avalonia UI (.NET 8) + CommunityToolkit.MVVM                | UI に限定・View ⇄ ViewModel バインディングのみ |
| R2  | **Presentation‑Functions**       | Azure Functions (Isolated Worker, .NET 8)                   | ユースケースを HTTP / Timer 経由で公開        |
| R3  | **Application Layer**            | CQRS ハンドラー & DTO                                            | Domain へのみ依存・外部 I/F はポートとして抽象化    |
| R4  | **Domain Layer**                 | Entities / Value Objects / Domain Events                    | 完全に外部依存ゼロの純粋 C#                   |
| R5  | **Infrastructure Layer**         | EF Core (SQLite & Azure SQL), Azure Blob SDK, Serilog sinks | Application のポート実装                |
| R6  | **Local DB / Cloud Backup**      | SQLite (ローカル) → Azure SQL Serverless (同期)                   | 同期トリガー: App 終了 + 毎日 03:00 JST     |
| R7  | **Auto‑Update**                  | Squirrel.Azure (または NetSparkleUpdater)                      | Blob `$web/updates/` にフィード配置      |
| R8  | **Logging**                      | Serilog RollingFile + *optional* Windows EventLog           | 保持 7 日。次フェーズで App Insights 検討     |
| R9  | **CI/CD**                        | GitHub Actions (build → test → release → deploy)            | 層単位ワークフローでキャッシュ効率化                |
| R10 | **Infrastructure as Code (IaC)** | Bicep modules（共通 + サービス別）                                   | 冪等・プレビューで差分確認可                    |
| R11 | **Local Dev Container**          | dev‑container + `docker‑compose` (Azurite & SQL Server)     | `user‑secrets` で接続文字列を注入          |
| R12 | **Documentation**                | README（概要） + Spec + ADR （Markdown）                          | すべて PR ベースでレビュー                   |

---

## 7. 運用・障害対応方針

1. **ログ & トレース** ― 重要処理に構造化ログを埋め込み、エラースタックを送信。
2. **可観測性 (Observability)** ― Application Insights + Azure Monitor で性能・例外・依存関係を可視化。
3. **アラート** ― SLA を外れたレスポンスタイム or 未処理例外が 5 分継続した場合、Webhook で通知。
4. **障害時フロー** ― ダッシュボード確認 → 障害テンプレート (RUNBOOK) に沿って一次対応 → GitHub Issues 発行。
5. **アップデート戦略** ― `release/*` ブランチへマージで自動ビルド & Squirrel フィード更新。利用者側は次回起動時に自動更新適用。

---

## 8. 制約

* オフラインファースト：ネット接続なしでも PDF 生成可能
* Interpretation は **DB テーブル** で管理し、占い師ごとのスコープを保持
* PDF フォーマットは A4 縦固定（将来拡張可）
* ChartImage は 300 DPI PNG/SVG 推奨（10 MB 上限）

---

## 9. リスクと対策

| リスク                          | 対策                               |
| ---------------------------- | -------------------------------- |
| SWETL API 変更で計算結果がずれる        | バージョンを LTS に固定し半年ごと検証            |
| ChartImage アップロード忘れでレポート欠損   | 必須バリデーション + プレースホルダー警告           |
| DB 拡張で Interpretation サイズ肥大化 | ページング + 検索インデックス + 圧縮バックアップ      |
| Azure Functions コールドスタート遅延   | プリウォーム Timer + Queue Trigger 再試行 |

---
