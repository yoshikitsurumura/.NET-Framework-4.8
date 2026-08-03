using System;

namespace TaskManager.Core.Models
{
    /// <summary>
    /// タスクの優先度。
    /// </summary>
    public enum TaskPriority
    {
        Low = 0,
        Normal = 1,
        High = 2
    }

    public static class TaskPriorityExtensions
    {
        /// <summary>
        /// 画面表示用の日本語名を返す。
        /// </summary>
        public static string ToDisplayName(this TaskPriority priority)
        {
            switch (priority)
            {
                case TaskPriority.Low:
                    return "低";
                case TaskPriority.High:
                    return "高";
                case TaskPriority.Normal:
                    return "中";
                default:
                    throw new ArgumentOutOfRangeException("priority");
            }
        }
    }
}
