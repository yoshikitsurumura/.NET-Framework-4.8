using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TaskManager.Core.Models;
using TaskManager.Core.Services;

namespace TaskManager.Tests
{
    /// <summary>
    /// TaskManager.Core のテスト一式。
    /// </summary>
    public static class CoreTests
    {
        public static void Register(TestRunner runner)
        {
            RegisterValidationTests(runner);
            RegisterServiceTests(runner);
            RegisterQueryTests(runner);
            RegisterRepositoryTests(runner);
        }

        #region 入力検証

        private static void RegisterValidationTests(TestRunner runner)
        {
            runner.Run("タイトルが空だと検証エラーになる", () =>
            {
                var item = new TodoItem { Title = "   " };
                ValidationResult result = TodoItemValidator.Validate(item);
                Assert.IsFalse(result.IsValid, "空タイトルが通ってしまった");
                Assert.AreEqual(1, result.Errors.Count, "エラー件数が想定と違う");
            });

            runner.Run("タイトルが長すぎると検証エラーになる", () =>
            {
                var item = new TodoItem { Title = new string('あ', TodoItem.MaxTitleLength + 1) };
                Assert.IsFalse(TodoItemValidator.Validate(item).IsValid, "長すぎるタイトルが通ってしまった");
            });

            runner.Run("正しいタスクは検証を通る", () =>
            {
                var item = new TodoItem { Title = "資料を作成する", Category = "仕事" };
                Assert.IsTrue(TodoItemValidator.Validate(item).IsValid, "正常なタスクが弾かれた");
            });

            runner.Run("完了にすると完了日時が入り、戻すと消える", () =>
            {
                var item = new TodoItem { Title = "テスト" };
                Assert.IsNull(item.CompletedAt, "初期状態で完了日時が入っている");

                item.SetCompleted(true);
                Assert.IsTrue(item.IsCompleted, "完了にならなかった");
                Assert.IsNotNull(item.CompletedAt, "完了日時が設定されなかった");

                item.SetCompleted(false);
                Assert.IsFalse(item.IsCompleted, "未完了に戻らなかった");
                Assert.IsNull(item.CompletedAt, "完了日時が残っている");
            });

            runner.Run("期限切れ・今日期限の判定", () =>
            {
                var overdue = new TodoItem { Title = "遅れ", DueDate = DateTime.Today.AddDays(-1) };
                var today = new TodoItem { Title = "今日", DueDate = DateTime.Today };
                var future = new TodoItem { Title = "先", DueDate = DateTime.Today.AddDays(3) };

                Assert.IsTrue(overdue.IsOverdue, "期限切れと判定されなかった");
                Assert.IsFalse(today.IsOverdue, "今日期限が期限切れ扱いになった");
                Assert.IsTrue(today.IsDueToday, "今日期限と判定されなかった");
                Assert.IsFalse(future.IsOverdue, "未来の期限が期限切れ扱いになった");

                overdue.SetCompleted(true);
                Assert.IsFalse(overdue.IsOverdue, "完了済みが期限切れ扱いのままになった");
            });
        }

        #endregion

        #region サービス

        private static void RegisterServiceTests(TestRunner runner)
        {
            runner.Run("追加すると件数が増え、保存が呼ばれる", () =>
            {
                var repository = new InMemoryTaskRepository();
                var service = new TaskService(repository);

                service.Add(new TodoItem { Title = "買い物" });

                Assert.AreEqual(1, service.Count, "件数が増えていない");
                Assert.AreEqual(1, repository.SaveCount, "保存が呼ばれていない");
            });

            runner.Run("タイトルが空のタスクは追加できない", () =>
            {
                var service = new TaskService(new InMemoryTaskRepository());
                Assert.Throws<ArgumentException>(
                    () => service.Add(new TodoItem { Title = string.Empty }),
                    "空タイトルが追加できてしまった");
                Assert.AreEqual(0, service.Count, "無効なタスクが登録された");
            });

            runner.Run("追加時にタイトルと期限が正規化される", () =>
            {
                var service = new TaskService(new InMemoryTaskRepository());
                TodoItem added = service.Add(new TodoItem
                {
                    Title = "  余白つき  ",
                    DueDate = new DateTime(2030, 1, 2, 13, 45, 0)
                });

                Assert.AreEqual("余白つき", added.Title, "タイトルの前後空白が除去されていない");
                Assert.AreEqual(new DateTime(2030, 1, 2), added.DueDate.Value, "期限が日付単位に丸められていない");
            });

            runner.Run("更新すると内容が置き換わる", () =>
            {
                var service = new TaskService(new InMemoryTaskRepository());
                TodoItem added = service.Add(new TodoItem { Title = "元のタイトル" });

                TodoItem edited = added.Clone();
                edited.Title = "新しいタイトル";
                edited.Priority = TaskPriority.High;
                service.Update(edited);

                TodoItem stored = service.GetById(added.Id);
                Assert.AreEqual("新しいタイトル", stored.Title, "タイトルが更新されていない");
                Assert.AreEqual(TaskPriority.High, stored.Priority, "優先度が更新されていない");
                Assert.AreEqual(1, service.Count, "更新で件数が変わってしまった");
            });

            runner.Run("存在しないタスクの更新は失敗する", () =>
            {
                var service = new TaskService(new InMemoryTaskRepository());
                Assert.Throws<InvalidOperationException>(
                    () => service.Update(new TodoItem { Title = "未登録" }),
                    "未登録のタスクが更新できてしまった");
            });

            runner.Run("削除と一括削除", () =>
            {
                var service = new TaskService(new InMemoryTaskRepository());
                TodoItem a = service.Add(new TodoItem { Title = "A" });
                TodoItem b = service.Add(new TodoItem { Title = "B" });
                TodoItem c = service.Add(new TodoItem { Title = "C" });

                Assert.IsTrue(service.Remove(a.Id), "削除に失敗した");
                Assert.IsFalse(service.Remove(a.Id), "同じタスクを二重に削除できてしまった");
                Assert.AreEqual(2, service.RemoveRange(new[] { b.Id, c.Id }), "一括削除の件数が違う");
                Assert.AreEqual(0, service.Count, "タスクが残っている");
            });

            runner.Run("完了済みの一括削除", () =>
            {
                var service = new TaskService(new InMemoryTaskRepository());
                TodoItem a = service.Add(new TodoItem { Title = "A" });
                service.Add(new TodoItem { Title = "B" });
                service.ToggleCompleted(a.Id);

                Assert.AreEqual(1, service.RemoveCompleted(), "削除件数が違う");
                Assert.AreEqual(1, service.Count, "未完了まで削除された");
            });

            runner.Run("完了状態の切り替え", () =>
            {
                var service = new TaskService(new InMemoryTaskRepository());
                TodoItem item = service.Add(new TodoItem { Title = "A" });

                Assert.IsTrue(service.ToggleCompleted(item.Id), "切り替えに失敗した");
                Assert.IsTrue(service.GetById(item.Id).IsCompleted, "完了にならなかった");

                Assert.IsFalse(service.SetCompleted(item.Id, true), "同じ状態への設定が変更扱いになった");
                Assert.IsTrue(service.SetCompleted(item.Id, false), "未完了に戻せなかった");
                Assert.IsFalse(service.ToggleCompleted(Guid.NewGuid()), "存在しない ID で成功が返った");
            });

            runner.Run("統計値の計算", () =>
            {
                var service = new TaskService(new InMemoryTaskRepository());
                TodoItem done = service.Add(new TodoItem { Title = "完了予定" });
                service.Add(new TodoItem { Title = "期限切れ", DueDate = DateTime.Today.AddDays(-2) });
                service.Add(new TodoItem { Title = "今日", DueDate = DateTime.Today });
                service.ToggleCompleted(done.Id);

                TaskStatistics stats = service.GetStatistics();
                Assert.AreEqual(3, stats.Total, "全件数が違う");
                Assert.AreEqual(1, stats.Completed, "完了件数が違う");
                Assert.AreEqual(2, stats.Active, "未完了件数が違う");
                Assert.AreEqual(1, stats.Overdue, "期限切れ件数が違う");
                Assert.AreEqual(1, stats.DueToday, "今日期限の件数が違う");
                Assert.AreEqual(33, stats.CompletionPercentage, "完了率が違う");
            });

            runner.Run("タスクが 0 件なら完了率は 0", () =>
            {
                var service = new TaskService(new InMemoryTaskRepository());
                Assert.AreEqual(0, service.GetStatistics().CompletionPercentage, "0 件の完了率が 0 でない");
            });

            runner.Run("AutoSave を切ると保存されない", () =>
            {
                var repository = new InMemoryTaskRepository();
                var service = new TaskService(repository) { AutoSave = false };

                service.Add(new TodoItem { Title = "A" });
                Assert.AreEqual(0, repository.SaveCount, "自動保存が無効なのに保存された");

                service.Save();
                Assert.AreEqual(1, repository.SaveCount, "明示的な保存が行われなかった");
            });

            runner.Run("カテゴリ一覧は重複を除いて返る", () =>
            {
                var service = new TaskService(new InMemoryTaskRepository());
                service.Add(new TodoItem { Title = "A", Category = "仕事" });
                service.Add(new TodoItem { Title = "B", Category = "仕事" });
                service.Add(new TodoItem { Title = "C", Category = "私用" });
                service.Add(new TodoItem { Title = "D", Category = "  " });

                IList<string> categories = service.GetCategories();
                Assert.AreEqual(2, categories.Count, "カテゴリ件数が違う");
                Assert.IsTrue(categories.Contains("仕事") && categories.Contains("私用"), "カテゴリの内容が違う");
            });
        }

        #endregion

        #region 絞り込み・並べ替え

        private static void RegisterQueryTests(TestRunner runner)
        {
            runner.Run("既定では完了済みが一覧に出ない", () =>
            {
                TaskService service = CreateSampleService();
                service.ToggleCompleted(service.Items.First(i => i.Title == "請求書の送付").Id);

                IList<TodoItem> result = service.Query(new TaskQuery());
                Assert.IsFalse(result.Any(i => i.Title == "請求書の送付"), "完了済みが表示された");

                result = service.Query(new TaskQuery { IncludeCompleted = true });
                Assert.IsTrue(result.Any(i => i.Title == "請求書の送付"), "完了済みを含める指定が効いていない");
            });

            runner.Run("キーワード検索はタイトル・メモ・カテゴリを対象にする", () =>
            {
                TaskService service = CreateSampleService();

                Assert.AreEqual(1, service.Query(new TaskQuery { SearchText = "請求書" }).Count, "タイトル検索が効かない");
                Assert.AreEqual(1, service.Query(new TaskQuery { SearchText = "牛乳" }).Count, "メモ検索が効かない");
                Assert.AreEqual(2, service.Query(new TaskQuery { SearchText = "仕事" }).Count, "カテゴリ検索が効かない");
                Assert.AreEqual(0, service.Query(new TaskQuery { SearchText = "該当なし" }).Count, "一致しない語で結果が返った");
            });

            runner.Run("優先度・カテゴリでの絞り込み", () =>
            {
                TaskService service = CreateSampleService();

                Assert.AreEqual(1, service.Query(new TaskQuery { Priority = TaskPriority.High }).Count, "優先度の絞り込みが違う");
                Assert.AreEqual(2, service.Query(new TaskQuery { Category = "仕事" }).Count, "カテゴリの絞り込みが違う");
                Assert.AreEqual(3, service.Query(new TaskQuery { Category = "  " }).Count, "空白のカテゴリ指定で絞り込まれた");
            });

            runner.Run("期限による絞り込み", () =>
            {
                TaskService service = CreateSampleService();

                Assert.AreEqual(1, service.Query(new TaskQuery { Due = DueFilter.Overdue }).Count, "期限切れの件数が違う");
                Assert.AreEqual(1, service.Query(new TaskQuery { Due = DueFilter.Today }).Count, "今日期限の件数が違う");
                Assert.AreEqual(2, service.Query(new TaskQuery { Due = DueFilter.Within7Days }).Count, "7 日以内の件数が違う");
                Assert.AreEqual(1, service.Query(new TaskQuery { Due = DueFilter.NoDueDate }).Count, "期限なしの件数が違う");
            });

            runner.Run("期限順では期限なしが末尾になる", () =>
            {
                TaskService service = CreateSampleService();
                IList<TodoItem> result = service.Query(new TaskQuery { SortOrder = TaskSortOrder.DueDateAscending });

                Assert.AreEqual("請求書の送付", result[0].Title, "期限が最も近いタスクが先頭でない");
                Assert.IsFalse(result[result.Count - 1].DueDate.HasValue, "期限なしが末尾に来ていない");
            });

            runner.Run("優先度順では高いものが先頭に来る", () =>
            {
                TaskService service = CreateSampleService();
                IList<TodoItem> result = service.Query(new TaskQuery { SortOrder = TaskSortOrder.PriorityDescending });

                Assert.AreEqual(TaskPriority.High, result[0].Priority, "優先度の高いタスクが先頭でない");
            });

            runner.Run("完了済みは並べ替えても末尾に来る", () =>
            {
                TaskService service = CreateSampleService();
                service.ToggleCompleted(service.Items.First(i => i.Title == "請求書の送付").Id);

                IList<TodoItem> result = service.Query(new TaskQuery
                {
                    IncludeCompleted = true,
                    SortOrder = TaskSortOrder.DueDateAscending
                });

                Assert.IsTrue(result[result.Count - 1].IsCompleted, "完了済みが末尾に来ていない");
            });
        }

        #endregion

        #region 永続化

        private static void RegisterRepositoryTests(TestRunner runner)
        {
            runner.Run("XML に保存して読み直せる", () =>
            {
                RunInTempDirectory(directory =>
                {
                    string path = Path.Combine(directory, "tasks.xml");
                    var repository = new XmlTaskRepository(path);
                    var service = new TaskService(repository);

                    TodoItem added = service.Add(new TodoItem
                    {
                        Title = "設計レビュー",
                        Notes = "資料を事前に共有する" + Environment.NewLine + "会議室 A",
                        Category = "仕事",
                        Priority = TaskPriority.High,
                        DueDate = new DateTime(2030, 5, 1)
                    });
                    service.ToggleCompleted(added.Id);

                    Assert.IsTrue(File.Exists(path), "ファイルが作成されていない");

                    var reloaded = new TaskService(new XmlTaskRepository(path));
                    Assert.AreEqual(1, reloaded.Count, "読み込み後の件数が違う");

                    TodoItem stored = reloaded.GetById(added.Id);
                    Assert.IsNotNull(stored, "ID が保持されていない");
                    Assert.AreEqual("設計レビュー", stored.Title, "タイトルが保持されていない");
                    Assert.AreEqual("仕事", stored.Category, "カテゴリが保持されていない");
                    Assert.AreEqual(TaskPriority.High, stored.Priority, "優先度が保持されていない");
                    Assert.AreEqual(new DateTime(2030, 5, 1), stored.DueDate.Value, "期限が保持されていない");
                    Assert.IsTrue(stored.IsCompleted, "完了状態が保持されていない");
                    Assert.IsNotNull(stored.CompletedAt, "完了日時が保持されていない");
                    Assert.IsTrue(stored.Notes.Contains("会議室 A"), "メモが保持されていない");
                });
            });

            runner.Run("保存先ファイルが無ければ空の一覧を返す", () =>
            {
                RunInTempDirectory(directory =>
                {
                    var repository = new XmlTaskRepository(Path.Combine(directory, "not-created-yet.xml"));
                    Assert.AreEqual(0, repository.Load().Count, "空でない一覧が返った");
                });
            });

            runner.Run("保存先のフォルダーが無ければ作成される", () =>
            {
                RunInTempDirectory(directory =>
                {
                    string path = Path.Combine(directory, "nested", "deeper", "tasks.xml");
                    var repository = new XmlTaskRepository(path);
                    repository.Save(new[] { new TodoItem { Title = "A" } });

                    Assert.IsTrue(File.Exists(path), "入れ子のフォルダーにファイルが作られていない");
                });
            });

            runner.Run("上書き保存しても内容が入れ替わる", () =>
            {
                RunInTempDirectory(directory =>
                {
                    string path = Path.Combine(directory, "tasks.xml");
                    var repository = new XmlTaskRepository(path);

                    repository.Save(new[] { new TodoItem { Title = "1 回目" } });
                    repository.Save(new[] { new TodoItem { Title = "2 回目" }, new TodoItem { Title = "3 回目" } });

                    IList<TodoItem> loaded = repository.Load();
                    Assert.AreEqual(2, loaded.Count, "上書き後の件数が違う");
                    Assert.IsFalse(loaded.Any(i => i.Title == "1 回目"), "古い内容が残っている");
                    Assert.IsFalse(File.Exists(path + ".tmp"), "一時ファイルが残っている");
                });
            });

            runner.Run("壊れたファイルは退避されて例外になる", () =>
            {
                RunInTempDirectory(directory =>
                {
                    string path = Path.Combine(directory, "tasks.xml");
                    File.WriteAllText(path, "これは XML ではありません");

                    var repository = new XmlTaskRepository(path);
                    Assert.Throws<TaskStorageException>(() => repository.Load(), "壊れたファイルで例外が出なかった");
                    Assert.IsFalse(File.Exists(path), "壊れたファイルが退避されていない");
                    Assert.IsTrue(Directory.GetFiles(directory, "*.corrupt").Length == 1, "退避ファイルが作られていない");

                    // 退避後は空の状態として読み込めること。
                    Assert.AreEqual(0, repository.Load().Count, "退避後に読み込めない");
                });
            });
        }

        #endregion

        #region ヘルパー

        /// <summary>テスト用のサンプルデータを持つサービスを作る。</summary>
        private static TaskService CreateSampleService()
        {
            var service = new TaskService(new InMemoryTaskRepository()) { AutoSave = false };

            service.Add(new TodoItem
            {
                Title = "請求書の送付",
                Category = "仕事",
                Priority = TaskPriority.High,
                DueDate = DateTime.Today.AddDays(-1)
            });
            service.Add(new TodoItem
            {
                Title = "定例ミーティング",
                Category = "仕事",
                Priority = TaskPriority.Normal,
                DueDate = DateTime.Today
            });
            service.Add(new TodoItem
            {
                Title = "スーパーで買い物",
                Category = "私用",
                Notes = "牛乳とパン",
                Priority = TaskPriority.Low
            });

            return service;
        }

        private static void RunInTempDirectory(Action<string> action)
        {
            string directory = Path.Combine(Path.GetTempPath(), "TaskManagerTests_" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(directory);
            try
            {
                action(directory);
            }
            finally
            {
                try
                {
                    Directory.Delete(directory, true);
                }
                catch (IOException)
                {
                    // 後始末に失敗してもテスト結果には影響させない。
                }
            }
        }

        #endregion
    }
}
