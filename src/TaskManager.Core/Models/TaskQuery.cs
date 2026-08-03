using System;

namespace TaskManager.Core.Models
{
    /// <summary>期限による絞り込み条件。</summary>
    public enum DueFilter
    {
        All = 0,
        Overdue = 1,
        Today = 2,
        Within7Days = 3,
        NoDueDate = 4
    }

    /// <summary>一覧の並び順。</summary>
    public enum TaskSortOrder
    {
        DueDateAscending = 0,
        PriorityDescending = 1,
        TitleAscending = 2,
        CreatedAtDescending = 3,
        CategoryAscending = 4
    }

    /// <summary>
    /// 一覧の絞り込み・並べ替え条件。
    /// </summary>
    public class TaskQuery
    {
        public TaskQuery()
        {
            SearchText = string.Empty;
            IncludeCompleted = false;
            Due = DueFilter.All;
            SortOrder = TaskSortOrder.DueDateAscending;
        }

        /// <summary>タイトル・メモ・カテゴリの部分一致検索(大文字小文字を区別しない)。</summary>
        public string SearchText { get; set; }

        /// <summary>完了済みタスクも一覧に含めるか。</summary>
        public bool IncludeCompleted { get; set; }

        /// <summary>指定した優先度のみに絞り込む。null なら絞り込まない。</summary>
        public TaskPriority? Priority { get; set; }

        /// <summary>指定したカテゴリのみに絞り込む。null または空なら絞り込まない。</summary>
        public string Category { get; set; }

        public DueFilter Due { get; set; }

        public TaskSortOrder SortOrder { get; set; }
    }
}
