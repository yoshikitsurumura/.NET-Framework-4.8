using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using TaskManager.App.Controls;
using TaskManager.Core.Models;
using TaskManager.Core.Services;

namespace TaskManager.App.Forms
{
    /// <summary>
    /// タスク一覧のメイン画面。
    /// </summary>
    public partial class MainForm : Form
    {
        private static readonly Color OverdueColor = Color.Firebrick;
        private static readonly Color DueTodayColor = Color.FromArgb(0, 90, 158);
        private static readonly Color CompletedColor = Color.Gray;

        private const string AllCategories = "(すべて)";

        private TaskService _service;

        /// <summary>一覧を作り直している間、イベントで再帰的に更新しないためのフラグ。</summary>
        private bool _suppressEvents;

        private TaskSortOrder _sortOrder = TaskSortOrder.DueDateAscending;

        public MainForm()
        {
            InitializeComponent();

            _suppressEvents = true;
            InitializeFilterItems();
            _suppressEvents = false;

            _service = CreateService();
            RefreshList();
        }

        #region 初期化

        private void InitializeFilterItems()
        {
            priorityFilterComboBox.Items.Add(new ComboItem("(すべて)", null));
            priorityFilterComboBox.Items.Add(new ComboItem("高", TaskPriority.High));
            priorityFilterComboBox.Items.Add(new ComboItem("中", TaskPriority.Normal));
            priorityFilterComboBox.Items.Add(new ComboItem("低", TaskPriority.Low));
            priorityFilterComboBox.SelectedIndex = 0;

            dueFilterComboBox.Items.Add(new ComboItem("(すべて)", DueFilter.All));
            dueFilterComboBox.Items.Add(new ComboItem("期限切れ", DueFilter.Overdue));
            dueFilterComboBox.Items.Add(new ComboItem("今日", DueFilter.Today));
            dueFilterComboBox.Items.Add(new ComboItem("7 日以内", DueFilter.Within7Days));
            dueFilterComboBox.Items.Add(new ComboItem("期限なし", DueFilter.NoDueDate));
            dueFilterComboBox.SelectedIndex = 0;

            categoryFilterComboBox.Items.Add(AllCategories);
            categoryFilterComboBox.SelectedIndex = 0;
        }

        /// <summary>
        /// 既定の保存先でサービスを作る。ファイルが壊れていた場合は退避して空の状態で起動する。
        /// </summary>
        private TaskService CreateService()
        {
            ITaskRepository repository = XmlTaskRepository.CreateDefault();

            try
            {
                return new TaskService(repository);
            }
            catch (TaskStorageException ex)
            {
                MessageBox.Show(
                    this,
                    ex.Message + Environment.NewLine + Environment.NewLine + "空の状態で起動します。",
                    "タスク管理",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }

            try
            {
                // 壊れたファイルは退避済みなので、通常はここで成功する。
                return new TaskService(repository);
            }
            catch (TaskStorageException ex)
            {
                MessageBox.Show(
                    this,
                    ex.Message + Environment.NewLine + Environment.NewLine +
                    "保存機能を無効にして起動します。終了すると内容は失われます。",
                    "タスク管理",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return new TaskService(new InMemoryTaskRepository());
            }
        }

        #endregion

        #region 一覧の更新

        private void RefreshList()
        {
            RefreshList(GetSelectedIds());
        }

        private void RefreshList(Guid idToSelect)
        {
            RefreshList(new[] { idToSelect });
        }

        private void RefreshList(IList<Guid> idsToSelect)
        {
            var query = new TaskQuery
            {
                SearchText = searchTextBox.Text,
                IncludeCompleted = showCompletedCheckBox.Checked,
                Priority = (TaskPriority?)GetComboValue(priorityFilterComboBox),
                Due = (DueFilter)(GetComboValue(dueFilterComboBox) ?? DueFilter.All),
                Category = GetSelectedCategoryFilter(),
                SortOrder = _sortOrder
            };

            IList<TodoItem> items = _service.Query(query);

            _suppressEvents = true;
            taskListView.BeginUpdate();
            try
            {
                taskListView.Items.Clear();
                foreach (TodoItem item in items)
                {
                    taskListView.Items.Add(CreateListViewItem(item));
                }

                if (idsToSelect != null && idsToSelect.Count > 0)
                {
                    RestoreSelection(idsToSelect);
                }
            }
            finally
            {
                taskListView.EndUpdate();
                _suppressEvents = false;
            }

            RefreshCategoryFilter();
            UpdateStatusBar(items.Count);
            UpdateButtonState();
        }

        private static ListViewItem CreateListViewItem(TodoItem item)
        {
            var listViewItem = new ListViewItem(item.Title)
            {
                Tag = item,
                Checked = item.IsCompleted
            };

            listViewItem.SubItems.Add(item.Priority.ToDisplayName());
            listViewItem.SubItems.Add(FormatDueDate(item));
            listViewItem.SubItems.Add(item.Category);
            listViewItem.SubItems.Add(FormatStatus(item));
            listViewItem.SubItems.Add(FormatNotes(item.Notes));

            if (item.IsCompleted)
            {
                listViewItem.ForeColor = CompletedColor;
            }
            else if (item.IsOverdue)
            {
                listViewItem.ForeColor = OverdueColor;
            }
            else if (item.IsDueToday)
            {
                listViewItem.ForeColor = DueTodayColor;
            }

            return listViewItem;
        }

        private static string FormatDueDate(TodoItem item)
        {
            if (!item.DueDate.HasValue)
            {
                return string.Empty;
            }

            string text = item.DueDate.Value.ToString("yyyy/MM/dd (ddd)");
            if (item.IsOverdue)
            {
                int days = (DateTime.Today - item.DueDate.Value.Date).Days;
                text += string.Format(" {0} 日超過", days);
            }
            return text;
        }

        private static string FormatStatus(TodoItem item)
        {
            if (item.IsCompleted)
            {
                return "完了";
            }
            if (item.IsOverdue)
            {
                return "期限切れ";
            }
            if (item.IsDueToday)
            {
                return "今日まで";
            }
            return "未完了";
        }

        private static string FormatNotes(string notes)
        {
            if (string.IsNullOrEmpty(notes))
            {
                return string.Empty;
            }

            string single = notes.Replace("\r\n", " ").Replace('\n', ' ').Replace('\r', ' ');
            return single.Length > 80 ? single.Substring(0, 80) + "..." : single;
        }

        private void RefreshCategoryFilter()
        {
            string current = categoryFilterComboBox.SelectedItem as string;
            IList<string> categories = _service.GetCategories();

            var desired = new List<string> { AllCategories };
            desired.AddRange(categories);

            var existing = categoryFilterComboBox.Items.Cast<string>().ToList();
            if (existing.SequenceEqual(desired))
            {
                return;
            }

            bool previous = _suppressEvents;
            _suppressEvents = true;
            try
            {
                categoryFilterComboBox.Items.Clear();
                foreach (string category in desired)
                {
                    categoryFilterComboBox.Items.Add(category);
                }

                int index = current == null ? 0 : desired.IndexOf(current);
                categoryFilterComboBox.SelectedIndex = index < 0 ? 0 : index;
            }
            finally
            {
                _suppressEvents = previous;
            }
        }

        private void UpdateStatusBar(int shownCount)
        {
            TaskStatistics stats = _service.GetStatistics();
            summaryStatusLabel.Text = string.Format(
                "表示 {0} 件 / 全 {1} 件   未完了 {2}   完了 {3} ({4}%)   期限切れ {5}   今日まで {6}",
                shownCount, stats.Total, stats.Active, stats.Completed,
                stats.CompletionPercentage, stats.Overdue, stats.DueToday);

            storageStatusLabel.Text = "保存先: " + _service.StorageLocation;
            storageStatusLabel.ToolTipText = _service.StorageLocation;
        }

        private void UpdateButtonState()
        {
            bool hasSelection = taskListView.SelectedItems.Count > 0;
            editButton.Enabled = taskListView.SelectedItems.Count == 1;
            deleteButton.Enabled = hasSelection;
            toggleButton.Enabled = hasSelection;
            purgeButton.Enabled = _service.GetStatistics().Completed > 0;
        }

        #endregion

        #region 選択の取得

        private TodoItem GetSelectedItem()
        {
            if (taskListView.SelectedItems.Count == 0)
            {
                return null;
            }
            return (TodoItem)taskListView.SelectedItems[0].Tag;
        }

        private IList<TodoItem> GetSelectedItems()
        {
            return taskListView.SelectedItems
                .Cast<ListViewItem>()
                .Select(i => (TodoItem)i.Tag)
                .ToList();
        }

        private IList<Guid> GetSelectedIds()
        {
            return GetSelectedItems().Select(i => i.Id).ToList();
        }

        private void RestoreSelection(IList<Guid> ids)
        {
            var idSet = new HashSet<Guid>(ids);
            ListViewItem first = null;

            foreach (ListViewItem listViewItem in taskListView.Items)
            {
                var item = (TodoItem)listViewItem.Tag;
                if (idSet.Contains(item.Id))
                {
                    listViewItem.Selected = true;
                    if (first == null)
                    {
                        first = listViewItem;
                    }
                }
            }

            if (first != null)
            {
                first.EnsureVisible();
                first.Focused = true;
            }
        }

        private static object GetComboValue(ComboBox comboBox)
        {
            var item = comboBox.SelectedItem as ComboItem;
            return item == null ? null : item.Value;
        }

        private string GetSelectedCategoryFilter()
        {
            var category = categoryFilterComboBox.SelectedItem as string;
            return category == AllCategories ? null : category;
        }

        #endregion

        #region イベントハンドラー

        private void Filter_Changed(object sender, EventArgs e)
        {
            if (_suppressEvents)
            {
                return;
            }
            RefreshList();
        }

        private void NewButton_Click(object sender, EventArgs e)
        {
            AddTask();
        }

        private void EditButton_Click(object sender, EventArgs e)
        {
            EditSelectedTask();
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            DeleteSelectedTasks();
        }

        private void ToggleButton_Click(object sender, EventArgs e)
        {
            ToggleSelectedTasks();
        }

        private void PurgeButton_Click(object sender, EventArgs e)
        {
            RemoveCompletedTasks();
        }

        private void ReloadButton_Click(object sender, EventArgs e)
        {
            if (Execute(() => _service.Reload()))
            {
                RefreshList();
            }
        }

        private void TaskListView_DoubleClick(object sender, EventArgs e)
        {
            EditSelectedTask();
        }

        private void TaskListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_suppressEvents)
            {
                return;
            }
            UpdateButtonState();
        }

        private void TaskListView_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            if (_suppressEvents)
            {
                return;
            }

            var item = (TodoItem)e.Item.Tag;
            bool completed = e.Item.Checked;

            // ItemChecked の処理中に一覧を作り直さないよう、更新はメッセージループに戻してから行う。
            BeginInvoke(new Action(() =>
            {
                if (Execute(() => _service.SetCompleted(item.Id, completed)))
                {
                    RefreshList(item.Id);
                }
            }));
        }

        private void TaskListView_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            TaskSortOrder order;
            switch (e.Column)
            {
                case 0:
                    order = TaskSortOrder.TitleAscending;
                    break;
                case 1:
                    order = TaskSortOrder.PriorityDescending;
                    break;
                case 2:
                    order = TaskSortOrder.DueDateAscending;
                    break;
                case 3:
                    order = TaskSortOrder.CategoryAscending;
                    break;
                default:
                    order = TaskSortOrder.CreatedAtDescending;
                    break;
            }

            _sortOrder = order;
            RefreshList();
        }

        private void TaskListView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                DeleteSelectedTasks();
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Enter)
            {
                EditSelectedTask();
                e.Handled = true;
            }
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Insert)
            {
                AddTask();
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.F5)
            {
                if (Execute(() => _service.Reload()))
                {
                    RefreshList();
                }
                e.Handled = true;
            }
            else if (e.Control && e.KeyCode == Keys.F)
            {
                searchTextBox.Focus();
                searchTextBox.SelectAll();
                e.Handled = true;
            }
        }

        #endregion

        #region 操作

        private void AddTask()
        {
            var newItem = new TodoItem();
            using (var dialog = new TaskEditForm(newItem, _service.GetCategories(), true))
            {
                if (dialog.ShowDialog(this) != DialogResult.OK)
                {
                    return;
                }

                TodoItem edited = dialog.Item;
                if (Execute(() => _service.Add(edited)))
                {
                    RefreshList(edited.Id);
                }
            }
        }

        private void EditSelectedTask()
        {
            TodoItem selected = GetSelectedItem();
            if (selected == null)
            {
                return;
            }

            using (var dialog = new TaskEditForm(selected.Clone(), _service.GetCategories(), false))
            {
                if (dialog.ShowDialog(this) != DialogResult.OK)
                {
                    return;
                }

                TodoItem edited = dialog.Item;
                if (Execute(() => _service.Update(edited)))
                {
                    RefreshList(edited.Id);
                }
            }
        }

        private void DeleteSelectedTasks()
        {
            IList<TodoItem> selected = GetSelectedItems();
            if (selected.Count == 0)
            {
                return;
            }

            string message = selected.Count == 1
                ? string.Format("「{0}」を削除しますか?", selected[0].Title)
                : string.Format("選択した {0} 件のタスクを削除しますか?", selected.Count);

            if (MessageBox.Show(this, message, "削除の確認",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            {
                return;
            }

            if (Execute(() => _service.RemoveRange(selected.Select(i => i.Id))))
            {
                RefreshList(new List<Guid>());
            }
        }

        private void ToggleSelectedTasks()
        {
            IList<TodoItem> selected = GetSelectedItems();
            if (selected.Count == 0)
            {
                return;
            }

            // 選択のうち 1 件でも未完了があれば「すべて完了」にする。
            bool markCompleted = selected.Any(i => !i.IsCompleted);

            bool succeeded = Execute(() =>
            {
                _service.AutoSave = false;
                try
                {
                    foreach (TodoItem item in selected)
                    {
                        _service.SetCompleted(item.Id, markCompleted);
                    }
                }
                finally
                {
                    _service.AutoSave = true;
                }
                _service.Save();
            });

            if (succeeded)
            {
                RefreshList(selected.Select(i => i.Id).ToList());
            }
        }

        private void RemoveCompletedTasks()
        {
            int completed = _service.GetStatistics().Completed;
            if (completed == 0)
            {
                MessageBox.Show(this, "完了済みのタスクはありません。", "タスク管理",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string message = string.Format("完了済みのタスク {0} 件を削除しますか?", completed);
            if (MessageBox.Show(this, message, "削除の確認",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            {
                return;
            }

            if (Execute(() => _service.RemoveCompleted()))
            {
                RefreshList(new List<Guid>());
            }
        }

        #endregion

        /// <summary>
        /// 想定内の例外をメッセージボックスに変換して実行する。成功したら true。
        /// </summary>
        private bool Execute(Action action)
        {
            try
            {
                action();
                return true;
            }
            catch (TaskStorageException ex)
            {
                ShowError("タスクの保存または読み込みに失敗しました。", ex);
            }
            catch (ArgumentException ex)
            {
                ShowError("入力内容に誤りがあります。", ex);
            }
            catch (InvalidOperationException ex)
            {
                ShowError("操作を実行できませんでした。", ex);
            }
            return false;
        }

        private void ShowError(string summary, Exception exception)
        {
            MessageBox.Show(
                this,
                summary + Environment.NewLine + Environment.NewLine + exception.Message,
                "タスク管理",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }
}
