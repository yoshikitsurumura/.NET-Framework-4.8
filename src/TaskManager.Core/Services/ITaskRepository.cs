using System.Collections.Generic;
using TaskManager.Core.Models;

namespace TaskManager.Core.Services
{
    /// <summary>
    /// タスクの永続化を担う。
    /// </summary>
    public interface ITaskRepository
    {
        /// <summary>保存済みのタスクを読み込む。保存先が無い場合は空のリストを返す。</summary>
        IList<TodoItem> Load();

        /// <summary>タスクをすべて保存する。</summary>
        void Save(IEnumerable<TodoItem> items);

        /// <summary>保存先の説明(ファイルパスなど)。</summary>
        string Location { get; }
    }
}
