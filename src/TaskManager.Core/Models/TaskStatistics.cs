namespace TaskManager.Core.Models
{
    /// <summary>
    /// ステータスバーに表示する集計値。
    /// </summary>
    public class TaskStatistics
    {
        public int Total { get; set; }

        public int Completed { get; set; }

        public int Active { get; set; }

        public int Overdue { get; set; }

        public int DueToday { get; set; }

        /// <summary>完了率(0～100)。タスクが 0 件なら 0。</summary>
        public int CompletionPercentage
        {
            get
            {
                if (Total == 0)
                {
                    return 0;
                }
                return (int)System.Math.Round(Completed * 100.0 / Total);
            }
        }
    }
}
