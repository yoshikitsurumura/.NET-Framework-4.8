using System;
using System.Xml.Serialization;

namespace TaskManager.Core.Models
{
    /// <summary>
    /// 1 件のタスク。XmlSerializer で永続化するため public な既定コンストラクターと
    /// 読み書き可能なプロパティのみで構成する。
    /// </summary>
    [Serializable]
    public class TodoItem
    {
        public const int MaxTitleLength = 200;
        public const int MaxNotesLength = 4000;
        public const int MaxCategoryLength = 60;

        public TodoItem()
        {
            Id = Guid.NewGuid();
            CreatedAt = DateTime.Now;
            Priority = TaskPriority.Normal;
            Title = string.Empty;
            Notes = string.Empty;
            Category = string.Empty;
        }

        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Notes { get; set; }

        public string Category { get; set; }

        public TaskPriority Priority { get; set; }

        /// <summary>期限。未設定なら null。</summary>
        public DateTime? DueDate { get; set; }

        public bool IsCompleted { get; set; }

        public DateTime CreatedAt { get; set; }

        /// <summary>完了した日時。未完了なら null。</summary>
        public DateTime? CompletedAt { get; set; }

        /// <summary>未完了かつ期限が今日より前なら true。</summary>
        [XmlIgnore]
        public bool IsOverdue
        {
            get { return !IsCompleted && DueDate.HasValue && DueDate.Value.Date < DateTime.Today; }
        }

        /// <summary>未完了かつ期限が今日なら true。</summary>
        [XmlIgnore]
        public bool IsDueToday
        {
            get { return !IsCompleted && DueDate.HasValue && DueDate.Value.Date == DateTime.Today; }
        }

        /// <summary>
        /// 完了状態を設定し、あわせて完了日時を更新する。
        /// </summary>
        public void SetCompleted(bool completed)
        {
            if (completed)
            {
                if (!IsCompleted)
                {
                    CompletedAt = DateTime.Now;
                }
                IsCompleted = true;
            }
            else
            {
                IsCompleted = false;
                CompletedAt = null;
            }
        }

        /// <summary>編集ダイアログ用のコピーを作る。</summary>
        public TodoItem Clone()
        {
            return (TodoItem)MemberwiseClone();
        }

        public override string ToString()
        {
            return Title;
        }
    }
}
