using System;
using System.Collections.Generic;
using System.Windows.Forms;
using TaskManager.App.Controls;
using TaskManager.Core.Models;

namespace TaskManager.App.Forms
{
    /// <summary>
    /// タスクの新規作成・編集ダイアログ。
    /// </summary>
    public partial class TaskEditForm : Form
    {
        private readonly TodoItem _item;

        /// <summary>
        /// 編集対象のタスクを受け取る。呼び出し側は編集用の複製を渡すこと。
        /// </summary>
        /// <param name="item">編集するタスク。</param>
        /// <param name="categories">カテゴリ入力の候補。</param>
        /// <param name="isNew">新規作成なら true。</param>
        public TaskEditForm(TodoItem item, IEnumerable<string> categories, bool isNew)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            InitializeComponent();

            _item = item;
            Text = isNew ? "タスクの追加" : "タスクの編集";

            InitializePriorityItems();
            InitializeCategoryItems(categories);
            LoadFromItem(isNew);
        }

        /// <summary>編集結果。ダイアログが OK で閉じられたときのみ有効。</summary>
        public TodoItem Item
        {
            get { return _item; }
        }

        private void InitializePriorityItems()
        {
            priorityComboBox.Items.Add(new ComboItem(TaskPriority.High.ToDisplayName(), TaskPriority.High));
            priorityComboBox.Items.Add(new ComboItem(TaskPriority.Normal.ToDisplayName(), TaskPriority.Normal));
            priorityComboBox.Items.Add(new ComboItem(TaskPriority.Low.ToDisplayName(), TaskPriority.Low));
        }

        private void InitializeCategoryItems(IEnumerable<string> categories)
        {
            if (categories == null)
            {
                return;
            }

            foreach (string category in categories)
            {
                categoryComboBox.Items.Add(category);
            }
        }

        private void LoadFromItem(bool isNew)
        {
            titleTextBox.Text = _item.Title;
            notesTextBox.Text = _item.Notes;
            categoryComboBox.Text = _item.Category;
            completedCheckBox.Checked = _item.IsCompleted;

            SelectPriority(_item.Priority);

            dueDateCheckBox.Checked = _item.DueDate.HasValue;
            dueDatePicker.Value = _item.DueDate ?? DateTime.Today;
            dueDatePicker.Enabled = dueDateCheckBox.Checked;

            infoLabel.Text = isNew
                ? string.Empty
                : BuildInfoText();
        }

        private string BuildInfoText()
        {
            string text = "作成: " + _item.CreatedAt.ToString("yyyy/MM/dd HH:mm");
            if (_item.CompletedAt.HasValue)
            {
                text += "   完了: " + _item.CompletedAt.Value.ToString("yyyy/MM/dd HH:mm");
            }
            return text;
        }

        private void SelectPriority(TaskPriority priority)
        {
            for (int i = 0; i < priorityComboBox.Items.Count; i++)
            {
                var comboItem = (ComboItem)priorityComboBox.Items[i];
                if ((TaskPriority)comboItem.Value == priority)
                {
                    priorityComboBox.SelectedIndex = i;
                    return;
                }
            }

            priorityComboBox.SelectedIndex = 0;
        }

        private void DueDateCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            dueDatePicker.Enabled = dueDateCheckBox.Checked;
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            ApplyToItem();

            ValidationResult validation = TodoItemValidator.Validate(_item);
            if (!validation.IsValid)
            {
                MessageBox.Show(this, validation.ToMessage(), "入力エラー",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                titleTextBox.Focus();
                return;
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        private void ApplyToItem()
        {
            _item.Title = titleTextBox.Text.Trim();
            _item.Notes = notesTextBox.Text;
            _item.Category = categoryComboBox.Text.Trim();

            var selectedPriority = priorityComboBox.SelectedItem as ComboItem;
            if (selectedPriority != null)
            {
                _item.Priority = (TaskPriority)selectedPriority.Value;
            }

            _item.DueDate = dueDateCheckBox.Checked ? dueDatePicker.Value.Date : (DateTime?)null;
            _item.SetCompleted(completedCheckBox.Checked);
        }
    }
}
