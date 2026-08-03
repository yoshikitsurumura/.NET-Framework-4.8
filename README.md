# TaskManager — .NET Framework 4.8 タスク管理アプリ

Windows Forms 製のタスク管理 (ToDo) アプリケーションです。
外部パッケージ (NuGet) に一切依存せず、.NET Framework 4.8 の標準ライブラリだけで動作します。

## 機能

| 機能 | 説明 |
| --- | --- |
| タスクの登録・編集・削除 | タイトル・メモ・カテゴリ・優先度 (高/中/低)・期限を設定できます |
| 完了管理 | 一覧のチェックボックス、または複数選択してまとめて完了/未完了を切り替えられます |
| 絞り込み | キーワード検索 (タイトル・メモ・カテゴリ)、優先度、カテゴリ、期限 (期限切れ/今日/7 日以内/期限なし)、完了済みの表示切替 |
| 並べ替え | 列ヘッダーのクリックで、期限順・優先度順・タイトル順・カテゴリ順・作成日順に切り替え。完了済みは常に末尾 |
| 色分け | 期限切れは赤、今日が期限は青、完了済みはグレーで表示 |
| 集計表示 | ステータスバーに 表示件数 / 全件数 / 未完了 / 完了 (完了率) / 期限切れ / 今日まで を表示 |
| 自動保存 | 変更のたびに XML ファイルへ保存。一時ファイル経由で置き換えるため、書き込み中に落ちても既存データが壊れません |
| 破損時の復旧 | 保存ファイルが読めない場合は `.corrupt` として退避し、空の状態で起動します |

### キーボード操作

| キー | 動作 |
| --- | --- |
| `Insert` | 新規タスク |
| `Enter` | 選択中のタスクを編集 |
| `Delete` | 選択中のタスクを削除 |
| `Space` | 選択中のタスクの完了状態を切り替え |
| `F5` | 保存ファイルから読み直し |
| `Ctrl` + `F` | 検索ボックスへフォーカス |

### データの保存先

```
%APPDATA%\TaskManager\tasks.xml
```

保存先はアプリのステータスバー右側にも表示されます。

## 構成

```
TaskManager.sln
├── src/TaskManager.Core/        業務ロジックと永続化 (クラスライブラリ / UI 非依存)
│   ├── Models/                  TodoItem, TaskPriority, TaskQuery, TaskStatistics, 入力検証
│   └── Services/                TaskService, ITaskRepository, XmlTaskRepository, InMemoryTaskRepository
├── src/TaskManager.App/         Windows Forms アプリ (画面のみ)
│   ├── Forms/MainForm           一覧・絞り込み・並べ替え
│   └── Forms/TaskEditForm       追加・編集ダイアログ
└── tests/TaskManager.Tests/     Core のテスト (コンソールアプリ / 外部依存なし)
```

UI とロジックを分けているため、`TaskManager.Core` はテストから直接呼び出せます。
テストは NuGet パッケージを使わない自前のランナーで、成功なら終了コード 0、失敗があれば 1 を返します。

## ビルドと実行

### 必要なもの

- Windows
- .NET Framework 4.8 Developer Pack
- Visual Studio 2019/2022、または Build Tools for Visual Studio (MSBuild)

### コマンドラインから

```cmd
build.cmd            :: Release でビルドしてテストまで実行
build.cmd Debug      :: Debug でビルド
```

ビルドが通ると、実行ファイルは次の場所に出力されます。

```
src\TaskManager.App\bin\Release\TaskManager.exe
```

### Visual Studio から

`TaskManager.sln` を開き、`TaskManager.App` をスタートアッププロジェクトに設定して実行 (F5) してください。

### テストだけを実行する

```cmd
tests\TaskManager.Tests\bin\Release\TaskManager.Tests.exe
```

## CI

`.github/workflows/build.yml` で、push / pull request のたびに windows-latest 上で
MSBuild によるビルドとテスト実行を行い、ビルド成果物をアーティファクトとして保存します。
