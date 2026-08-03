using System.Collections.Generic;
using System.Linq;
using TaskManager.Core.Models;

namespace TaskManager.Core.Services
{
    /// <summary>
    /// テストや一時利用のための、ファイルを使わないリポジトリ。
    /// </summary>
    public class InMemoryTaskRepository : ITaskRepository
    {
        private List<TodoItem> _items = new List<TodoItem>();

        public InMemoryTaskRepository()
        {
        }

        public InMemoryTaskRepository(IEnumerable<TodoItem> initialItems)
        {
            if (initialItems != null)
            {
                _items = initialItems.Select(i => i.Clone()).ToList();
            }
        }

        public string Location
        {
            get { return "(メモリ上)"; }
        }

        /// <summary>Save が呼ばれた回数。</summary>
        public int SaveCount { get; private set; }

        public IList<TodoItem> Load()
        {
            return _items.Select(i => i.Clone()).ToList();
        }

        public void Save(IEnumerable<TodoItem> items)
        {
            _items = items.Select(i => i.Clone()).ToList();
            SaveCount++;
        }
    }
}
