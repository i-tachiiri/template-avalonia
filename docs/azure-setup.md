# Azure 開発ガイド

このテンプレートをフォークまたはコピーした後、Azure 上で必要なリソースを作成してローカル/クラウド両方でアプリを動かすための手順を示します。

## 前提条件

- [Azure CLI](https://learn.microsoft.com/cli/azure/install-azure-cli) がインストール済み
- Docker Desktop (または互換環境)
- .NET 8 SDK

## コンテナ環境の起動

ローカル開発では `docker-compose.yml` を利用して SQL Server と Azurite を起動します。

```bash
# リポジトリルートで実行
$ docker-compose up -d
```

Azruite では `AZURITE_KEY` 環境変数にストレージアカウントキーを指定します。既定値は `AzuriteAzuriteAzuriteAzurite` です。

## Azure リソースの作成

1. リソースグループ作成

```bash
$ az group create -n <rg-name> -l <location>
```

2. Bicep テンプレートをデプロイ

```bash
$ az deployment group create \
    -g <rg-name> \
    -f infra/main.bicep \
    -p prefix=<name-prefix> sqlAdminUsername=<admin> sqlAdminPassword=<password>
```

このデプロイで以下のリソースが作成されます：

- Azure Storage Account
- Azure SQL Database (Serverless)
- Azure Functions (HTTP / Timer)

## 必要な環境変数

アプリケーションおよび Functions 実行時に必要な主要な環境変数は次のとおりです。

| 変数名 | 用途 |
|--------|------|
| `AZURITE_KEY` | ローカルストレージ用のアカウントキー |
| `ConnectionStrings__Default` | SQLite または Azure SQL の接続文字列 |
| `AzureWebJobsStorage` | Functions 用ストレージ接続文字列 |

ローカル開発では `.env` またはユーザーシークレットで設定しておきます。CI/CD では GitHub Secrets に登録してください。

## 開発を始める

コンテナを起動後、次のコマンドでアプリを実行します。

```bash
$ dotnet restore
$ dotnet run --project src/Presentation.Desktop
```

バックアップサービスを手動で実行する場合は次のコマンドを利用します。

```bash
$ dotnet run --project src/BackupService -- sync
```

## 参考

- [Azure CLI ドキュメント](https://learn.microsoft.com/cli/azure/)
- [Bicep テンプレート](infra/)

