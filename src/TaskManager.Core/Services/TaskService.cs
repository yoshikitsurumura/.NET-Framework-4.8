using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using TaskManager.Core.Models;

namespace TaskManager.Core.Services
{
    /// <summary>
    /// タスクの追加・更新・削除・絞り込みをまとめた業務ロジック。
    /// 画面(WinForms)からはこのクラスだけを使う。
    /// </summary>
    public class TaskService
    {
        private readonly ITaskRepository _repository;
        private readonly List<TodoItem> _items;

        /// <summary>
        /// リポジトリから既存のタスクを読み込んでサービスを作る。
        /// </summary>
        public TaskService(ITaskRepository repository)
        {
            if (repository == null)
            {
                throw new ArgumentNullException("repository");
            }
            _repository = repository;
            _items = repository.Load().ToList();
        }

        /// <summary>変更のたびに自動保存するか。既定は true。</summary>
        public bool AutoSave { get; set; } = true;

        public string StorageLocation
        {
            get { return _repository.Location; }
        }

        public ReadOnlyCollection<TodoItem> Items
        {
            get { return _items.AsReadOnly(); }
        }

        public int Count
        {
            get { return _items.Count; }
        }

        public TodoItem GetById(Guid id)
        {
            return _items.FirstOrDefault(i => i.Id == id);
        }

        /// <summary>
        /// タスクを追加する。検証に失敗した場合は <see cref="ArgumentException"/>。
        /// </summary>
        public TodoItem Add(TodoItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            Normalize(item);
            ThrowIfInvalid(item);

            if (_items.Any(i => i.Id == item.Id))
            {
                throw new InvalidOperationException("同じ ID のタスクが既に存在します: " + item.Id);
            }

            _items.Add(item);
            SaveIfNeeded();
            return item;
        }

        /// <summary>
        /// 既存タスクを、同じ ID を持つ <paramref name="item"/> の内容で置き換える。
        /// </summary>
        public void Update(TodoItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            Normalize(item);
            ThrowIfInvalid(item);

            int index = _items.FindIndex(i => i.Id == item.Id);
            if (index < 0)
            {
                throw new InvalidOperationException("更新対象のタスクが見つかりません: " + item.Id);
            }

            _items[index] = item;
            SaveIfNeeded();
        }

        /// <summary>指定したタスクを削除する。削除できたら true。</summary>
        public bool Remove(Guid id)
        {
            int removed = _items.RemoveAll(i => i.Id == id);
            if (removed == 0)
            {
                return false;
            }
            SaveIfNeeded();
            return true;
        }

        /// <summary>複数のタスクをまとめて削除し、削除した件数を返す。</summary>
        public int RemoveRange(IEnumerable<Guid> ids)
        {
            if (ids == null)
            {
                throw new ArgumentNullException("ids");
            }

            var idSet = new HashSet<Guid>(ids);
            if (idSet.Count == 0)
            {
                return 0;
            }

            int removed = _items.RemoveAll(i => idSet.Contains(i.Id));
            if (removed > 0)
            {
                SaveIfNeeded();
            }
            return removed;
        }

        /// <summary>完了済みのタスクをすべて削除し、削除した件数を返す。</summary>
        public int RemoveCompleted()
        {
            int removed = _items.RemoveAll(i => i.IsCompleted);
            if (removed > 0)
            {
                SaveIfNeeded();
            }
            return removed;
        }

        /// <summary>完了・未完了を切り替える。対象が無ければ false。</summary>
        public bool ToggleCompleted(Guid id)
        {
            TodoItem item = GetById(id);
            if (item == null)
            {
                return false;
            }

            item.SetCompleted(!item.IsCompleted);
            SaveIfNeeded();
            return true;
        }

        /// <summary>完了状態を明示的に設定する。状態が変わったら true。</summary>
        public bool SetCompleted(Guid id, bool completed)
        {
            TodoItem item = GetById(id);
            if (item == null || item.IsCompleted == completed)
            {
                return false;
            }

            item.SetCompleted(completed);
            SaveIfNeeded();
            return true;
        }

        /// <summary>
        /// 条件で絞り込み、並べ替えた一覧を返す。
        /// </summary>
        public IList<TodoItem> Query(TaskQuery query)
        {
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }

            IEnumerable<TodoItem> result = _items;

            if (!query.IncludeCompleted)
            {
                result = result.Where(i => !i.IsCompleted);
            }

            if (query.Priority.HasValue)
            {
                TaskPriority priority = query.Priority.Value;
                result = result.Where(i => i.Priority == priority);
            }

            if (!string.IsNullOrWhiteSpace(query.Category))
            {
                string category = query.Category.Trim();
                result = result.Where(i => string.Equals(i.Category ?? string.Empty, category, StringComparison.CurrentCultureIgnoreCase));
            }

            if (!string.IsNullOrWhiteSpace(query.SearchText))
            {
                string keyword = query.SearchText.Trim();
                result = result.Where(i => Contains(i.Title, keyword) || Contains(i.Notes, keyword) || Contains(i.Category, keyword));
            }

            result = ApplyDueFilter(result, query.Due);

            return Sort(result, query.SortOrder).ToList();
        }

        /// <summary>登録されているカテゴリの一覧(重複なし・五十音/アルファベット順)。</summary>
        public IList<string> GetCategories()
        {
            return _items
                .Select(i => (i.Category ?? string.Empty).Trim())
                .Where(c => c.Length > 0)
                .Distinct(StringComparer.CurrentCultureIgnoreCase)
                .OrderBy(c => c, StringComparer.CurrentCulture)
                .ToList();
        }

        public TaskStatistics GetStatistics()
        {
            return new TaskStatistics
            {
                Total = _items.Count,
                Completed = _items.Count(i => i.IsCompleted),
                Active = _items.Count(i => !i.IsCompleted),
                Overdue = _items.Count(i => i.IsOverdue),
                DueToday = _items.Count(i => i.IsDueToday)
            };
        }

        /// <summary>現在の内容を明示的に保存する。</summary>
        public void Save()
        {
            _repository.Save(_items);
        }

        /// <summary>リポジトリから読み直して、メモリ上の内容を破棄する。</summary>
        public void Reload()
        {
            _items.Clear();
            _items.AddRange(_repository.Load());
        }

        private static IEnumerable<TodoItem> ApplyDueFilter(IEnumerable<TodoItem> source, DueFilter filter)
        {
            DateTime today = DateTime.Today;

            switch (filter)
            {
                case DueFilter.All:
                    return source;
                case DueFilter.Overdue:
                    return source.Where(i => i.IsOverdue);
                case DueFilter.Today:
                    return source.Where(i => i.DueDate.HasValue && i.DueDate.Value.Date == today);
                case DueFilter.Within7Days:
                    return source.Where(i => i.DueDate.HasValue && i.DueDate.Value.Date <= today.AddDays(7));
                case DueFilter.NoDueDate:
                    return source.Where(i => !i.DueDate.HasValue);
                default:
                    throw new ArgumentOutOfRangeException("filter");
            }
        }

        private static IEnumerable<TodoItem> Sort(IEnumerable<TodoItem> source, TaskSortOrder order)
        {
            // 完了済みは常に後ろへ。
            IOrderedEnumerable<TodoItem> ordered = source.OrderBy(i => i.IsCompleted);

            switch (order)
            {
                case TaskSortOrder.DueDateAscending:
                    return ordered
                        .ThenBy(i => i.DueDate.HasValue ? 0 : 1)
                        .ThenBy(i => i.DueDate ?? DateTime.MaxValue)
                        .ThenByDescending(i => i.Priority)
                        .ThenBy(i => i.Title, StringComparer.CurrentCultureIgnoreCase);
                case TaskSortOrder.PriorityDescending:
                    return ordered
                        .ThenByDescending(i => i.Priority)
                        .ThenBy(i => i.DueDate.HasValue ? 0 : 1)
                        .ThenBy(i => i.DueDate ?? DateTime.MaxValue)
                        .ThenBy(i => i.Title, StringComparer.CurrentCultureIgnoreCase);
                case TaskSortOrder.TitleAscending:
                    return ordered
                        .ThenBy(i => i.Title, StringComparer.CurrentCulture);
                case TaskSortOrder.CreatedAtDescending:
                    return ordered
                        .ThenByDescending(i => i.CreatedAt);
                case TaskSortOrder.CategoryAscending:
                    return ordered
                        .ThenBy(i => i.Category ?? string.Empty, StringComparer.CurrentCulture)
                        .ThenBy(i => i.Title, StringComparer.CurrentCultureIgnoreCase);
                default:
                    throw new ArgumentOutOfRangeException("order");
            }
        }

        private static bool Contains(string source, string keyword)
        {
            return !string.IsNullOrEmpty(source) &&
                   CultureInfo.CurrentCulture.CompareInfo.IndexOf(source, keyword, CompareOptions.IgnoreCase) >= 0;
        }

        private static void Normalize(TodoItem item)
        {
            item.Title = (item.Title ?? string.Empty).Trim();
            item.Notes = item.Notes ?? string.Empty;
            item.Category = (item.Category ?? string.Empty).Trim();

            if (item.DueDate.HasValue)
            {
                item.DueDate = item.DueDate.Value.Date;
            }

            if (!item.IsCompleted)
            {
                item.CompletedAt = null;
            }
            else if (!item.CompletedAt.HasValue)
            {
                item.CompletedAt = DateTime.Now;
            }
        }

        private static void ThrowIfInvalid(TodoItem item)
        {
            ValidationResult validation = TodoItemValidator.Validate(item);
            if (!validation.IsValid)
            {
                throw new ArgumentException(validation.ToMessage(), "item");
            }
        }

        private void SaveIfNeeded()
        {
            if (AutoSave)
            {
                Save();
            }
        }
    }
}
