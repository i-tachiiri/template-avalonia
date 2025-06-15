# 📑 Application / Use‑Case Design (CQRS)

> **目的** – CQRS コマンド・クエリの一覧、`src/Core/Application` の構成指針、テスト方針をまとめる “作業台帳”。実装フェーズで都度更新していく **Living Document** です。
>
> **場所** – `docs/architecture/application-design.md` で管理。（README からリンク）

---

## 1. CQRS 一覧

| 領域                 | コマンド / クエリ                        | ファイル名 (@ Commands/Queries)              | 概要                      |
| ------------------ | --------------------------------- | --------------------------------------- | ----------------------- |
| **Astrologer**     | `GetAstrologerSettingsQuery`      | `Queries/GetAstrologerSettings/...`     | 占い師設定読込                 |
|                    | `UpdateAstrologerSettingsCommand` | `Commands/UpdateAstrologerSettings/...` | 占い師設定更新                 |
| **Client**         | `CreateClientCommand`             | `Commands/CreateClient/...`             | クライアント + 出生情報登録         |
|                    | `ListClientsQuery`                | `Queries/ListClients/...`               | クライアント一覧取得              |
| **Chart**          | `GenerateChartCommand`            | `Commands/GenerateChart/...`            | SWETL 計算 & ChartData 保存 |
| **Interpretation** | `FindInterpretationsQuery`        | `Queries/FindInterpretations/...`       | 条件キー検索（全文検索）            |
| **Report**         | `CreateReportCommand`             | `Commands/CreateReport/...`             | PDF 生成 & 保存             |
|                    | `ListReportsQuery`                | `Queries/ListReports/...`               | レポート履歴取得                |
| **Sync**           | `StartSyncJobCommand`             | `Commands/StartSyncJob/...`             | PII マスク同期開始             |
|                    | `GetSyncJobStatusQuery`           | `Queries/GetSyncJobStatus/...`          | 同期進捗確認                  |

> **メモ** – 一覧は実装中に追加・削除 OK。Pull Request ベースで更新してください。

---

## 2. `src/Core/Application` フォルダ構成指針

```
Core/Application/
├── Common/               # 例外型・Result<T>・バリューオブジェクト
├── Interfaces/           # ポート: IAstrologerRepository など
├── Commands/
│   └── CreateClient/
│       ├── CreateClientCommand.cs
│       ├── CreateClientHandler.cs
│       └── CreateClientDto.cs
└── Queries/
    └── ListClients/...
```

* **コマンド / クエリごとにサブフォルダ** を作り、DTO・Command/Query・Handler を同居させる。
* `Interfaces` は **Infrastructure 実装を隔離**するポート群。

---

## 3. テスト先行 (TDD) 方針

1. `tests/Unit/Application/{Commands|Queries}/...` にハンドラー単体テストを作成。
2. In‑Memory リポジトリ or モック (NSubstitute) で依存を代替。
3. **Given‑When‑Then** で振る舞いを検証。

例: *CreateClientTests.cs*

```csharp
// Arrange: fake IClientRepository, AutoFixture for DTO
// Act: await handler.Handle(cmd, ct);
// Assert: repo.AddAsync が呼ばれた & DomainEvent が発火
```

---

## 4. 今後の運用ルール

* **ドラフト OK** – あいまいでもまず追加 → 後でリネーム・削除可。
* **Pull Request テンプレート**に「`docs/architecture/application-design.md` 更新したか？」チェックを追加。
* コマンド / クエリ追加時は「Why」を ADR に残す（大きな設計変更のみ）。

---

## 5. TODO（初期タスク）

* [ ] CQRS 一覧の空欄（エッジケース）を洗い出し
* [ ] Interfaces（Repository, Service ラッパー）スケルトン生成
* [ ] CreateClient, GenerateChart, CreateReport の **赤テスト** を追加
* [ ] GitHub Actions にテスト実行ジョブを追加


