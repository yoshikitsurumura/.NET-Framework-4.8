namespace TaskManager.App.Forms
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        private void InitializeComponent()
        {
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.newButton = new System.Windows.Forms.ToolStripButton();
            this.editButton = new System.Windows.Forms.ToolStripButton();
            this.deleteButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toggleButton = new System.Windows.Forms.ToolStripButton();
            this.purgeButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.reloadButton = new System.Windows.Forms.ToolStripButton();
            this.filterPanel = new System.Windows.Forms.Panel();
            this.searchLabel = new System.Windows.Forms.Label();
            this.searchTextBox = new System.Windows.Forms.TextBox();
            this.priorityLabel = new System.Windows.Forms.Label();
            this.priorityFilterComboBox = new System.Windows.Forms.ComboBox();
            this.dueLabel = new System.Windows.Forms.Label();
            this.dueFilterComboBox = new System.Windows.Forms.ComboBox();
            this.categoryLabel = new System.Windows.Forms.Label();
            this.categoryFilterComboBox = new System.Windows.Forms.ComboBox();
            this.showCompletedCheckBox = new System.Windows.Forms.CheckBox();
            this.taskListView = new System.Windows.Forms.ListView();
            this.titleColumn = new System.Windows.Forms.ColumnHeader();
            this.priorityColumn = new System.Windows.Forms.ColumnHeader();
            this.dueColumn = new System.Windows.Forms.ColumnHeader();
            this.categoryColumn = new System.Windows.Forms.ColumnHeader();
            this.statusColumn = new System.Windows.Forms.ColumnHeader();
            this.notesColumn = new System.Windows.Forms.ColumnHeader();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.summaryStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.storageStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStrip.SuspendLayout();
            this.filterPanel.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            //
            // toolStrip
            //
            this.toolStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newButton,
            this.editButton,
            this.deleteButton,
            this.toolStripSeparator1,
            this.toggleButton,
            this.purgeButton,
            this.toolStripSeparator2,
            this.reloadButton});
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(984, 27);
            this.toolStrip.TabIndex = 0;
            //
            // newButton
            //
            this.newButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.newButton.Name = "newButton";
            this.newButton.Size = new System.Drawing.Size(23, 24);
            this.newButton.Text = "新規(&N)";
            this.newButton.ToolTipText = "新しいタスクを追加します (Insert)";
            this.newButton.Click += new System.EventHandler(this.NewButton_Click);
            //
            // editButton
            //
            this.editButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.editButton.Name = "editButton";
            this.editButton.Size = new System.Drawing.Size(23, 24);
            this.editButton.Text = "編集(&E)";
            this.editButton.ToolTipText = "選択したタスクを編集します (Enter)";
            this.editButton.Click += new System.EventHandler(this.EditButton_Click);
            //
            // deleteButton
            //
            this.deleteButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Size = new System.Drawing.Size(23, 24);
            this.deleteButton.Text = "削除(&D)";
            this.deleteButton.ToolTipText = "選択したタスクを削除します (Delete)";
            this.deleteButton.Click += new System.EventHandler(this.DeleteButton_Click);
            //
            // toolStripSeparator1
            //
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
            //
            // toggleButton
            //
            this.toggleButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toggleButton.Name = "toggleButton";
            this.toggleButton.Size = new System.Drawing.Size(23, 24);
            this.toggleButton.Text = "完了/未完了(&C)";
            this.toggleButton.ToolTipText = "選択したタスクの完了状態を切り替えます (Space)";
            this.toggleButton.Click += new System.EventHandler(this.ToggleButton_Click);
            //
            // purgeButton
            //
            this.purgeButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.purgeButton.Name = "purgeButton";
            this.purgeButton.Size = new System.Drawing.Size(23, 24);
            this.purgeButton.Text = "完了済みを削除";
            this.purgeButton.ToolTipText = "完了したタスクをまとめて削除します";
            this.purgeButton.Click += new System.EventHandler(this.PurgeButton_Click);
            //
            // toolStripSeparator2
            //
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 27);
            //
            // reloadButton
            //
            this.reloadButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.reloadButton.Name = "reloadButton";
            this.reloadButton.Size = new System.Drawing.Size(23, 24);
            this.reloadButton.Text = "再読み込み";
            this.reloadButton.ToolTipText = "保存ファイルから読み直します (F5)";
            this.reloadButton.Click += new System.EventHandler(this.ReloadButton_Click);
            //
            // filterPanel
            //
            this.filterPanel.Controls.Add(this.searchLabel);
            this.filterPanel.Controls.Add(this.searchTextBox);
            this.filterPanel.Controls.Add(this.priorityLabel);
            this.filterPanel.Controls.Add(this.priorityFilterComboBox);
            this.filterPanel.Controls.Add(this.dueLabel);
            this.filterPanel.Controls.Add(this.dueFilterComboBox);
            this.filterPanel.Controls.Add(this.categoryLabel);
            this.filterPanel.Controls.Add(this.categoryFilterComboBox);
            this.filterPanel.Controls.Add(this.showCompletedCheckBox);
            this.filterPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.filterPanel.Location = new System.Drawing.Point(0, 27);
            this.filterPanel.Name = "filterPanel";
            this.filterPanel.Size = new System.Drawing.Size(984, 44);
            this.filterPanel.TabIndex = 1;
            //
            // searchLabel
            //
            this.searchLabel.AutoSize = true;
            this.searchLabel.Location = new System.Drawing.Point(10, 14);
            this.searchLabel.Name = "searchLabel";
            this.searchLabel.Size = new System.Drawing.Size(35, 15);
            this.searchLabel.TabIndex = 0;
            this.searchLabel.Text = "検索:";
            //
            // searchTextBox
            //
            this.searchTextBox.Location = new System.Drawing.Point(52, 10);
            this.searchTextBox.Name = "searchTextBox";
            this.searchTextBox.Size = new System.Drawing.Size(180, 23);
            this.searchTextBox.TabIndex = 1;
            this.searchTextBox.TextChanged += new System.EventHandler(this.Filter_Changed);
            //
            // priorityLabel
            //
            this.priorityLabel.AutoSize = true;
            this.priorityLabel.Location = new System.Drawing.Point(244, 14);
            this.priorityLabel.Name = "priorityLabel";
            this.priorityLabel.Size = new System.Drawing.Size(50, 15);
            this.priorityLabel.TabIndex = 2;
            this.priorityLabel.Text = "優先度:";
            //
            // priorityFilterComboBox
            //
            this.priorityFilterComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.priorityFilterComboBox.Location = new System.Drawing.Point(300, 10);
            this.priorityFilterComboBox.Name = "priorityFilterComboBox";
            this.priorityFilterComboBox.Size = new System.Drawing.Size(90, 23);
            this.priorityFilterComboBox.TabIndex = 3;
            this.priorityFilterComboBox.SelectedIndexChanged += new System.EventHandler(this.Filter_Changed);
            //
            // dueLabel
            //
            this.dueLabel.AutoSize = true;
            this.dueLabel.Location = new System.Drawing.Point(402, 14);
            this.dueLabel.Name = "dueLabel";
            this.dueLabel.Size = new System.Drawing.Size(35, 15);
            this.dueLabel.TabIndex = 4;
            this.dueLabel.Text = "期限:";
            //
            // dueFilterComboBox
            //
            this.dueFilterComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.dueFilterComboBox.Location = new System.Drawing.Point(444, 10);
            this.dueFilterComboBox.Name = "dueFilterComboBox";
            this.dueFilterComboBox.Size = new System.Drawing.Size(130, 23);
            this.dueFilterComboBox.TabIndex = 5;
            this.dueFilterComboBox.SelectedIndexChanged += new System.EventHandler(this.Filter_Changed);
            //
            // categoryLabel
            //
            this.categoryLabel.AutoSize = true;
            this.categoryLabel.Location = new System.Drawing.Point(586, 14);
            this.categoryLabel.Name = "categoryLabel";
            this.categoryLabel.Size = new System.Drawing.Size(60, 15);
            this.categoryLabel.TabIndex = 6;
            this.categoryLabel.Text = "カテゴリ:";
            //
            // categoryFilterComboBox
            //
            this.categoryFilterComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.categoryFilterComboBox.Location = new System.Drawing.Point(652, 10);
            this.categoryFilterComboBox.Name = "categoryFilterComboBox";
            this.categoryFilterComboBox.Size = new System.Drawing.Size(140, 23);
            this.categoryFilterComboBox.TabIndex = 7;
            this.categoryFilterComboBox.SelectedIndexChanged += new System.EventHandler(this.Filter_Changed);
            //
            // showCompletedCheckBox
            //
            this.showCompletedCheckBox.AutoSize = true;
            this.showCompletedCheckBox.Location = new System.Drawing.Point(806, 12);
            this.showCompletedCheckBox.Name = "showCompletedCheckBox";
            this.showCompletedCheckBox.Size = new System.Drawing.Size(120, 19);
            this.showCompletedCheckBox.TabIndex = 8;
            this.showCompletedCheckBox.Text = "完了済みも表示";
            this.showCompletedCheckBox.UseVisualStyleBackColor = true;
            this.showCompletedCheckBox.CheckedChanged += new System.EventHandler(this.Filter_Changed);
            //
            // taskListView
            //
            this.taskListView.CheckBoxes = true;
            this.taskListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.titleColumn,
            this.priorityColumn,
            this.dueColumn,
            this.categoryColumn,
            this.statusColumn,
            this.notesColumn});
            this.taskListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.taskListView.FullRowSelect = true;
            this.taskListView.GridLines = true;
            this.taskListView.HideSelection = false;
            this.taskListView.Location = new System.Drawing.Point(0, 71);
            this.taskListView.Name = "taskListView";
            this.taskListView.Size = new System.Drawing.Size(984, 468);
            this.taskListView.TabIndex = 2;
            this.taskListView.UseCompatibleStateImageBehavior = false;
            this.taskListView.View = System.Windows.Forms.View.Details;
            this.taskListView.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.TaskListView_ColumnClick);
            this.taskListView.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.TaskListView_ItemChecked);
            this.taskListView.SelectedIndexChanged += new System.EventHandler(this.TaskListView_SelectedIndexChanged);
            this.taskListView.DoubleClick += new System.EventHandler(this.TaskListView_DoubleClick);
            this.taskListView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TaskListView_KeyDown);
            //
            // titleColumn
            //
            this.titleColumn.Text = "タイトル";
            this.titleColumn.Width = 300;
            //
            // priorityColumn
            //
            this.priorityColumn.Text = "優先度";
            this.priorityColumn.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.priorityColumn.Width = 70;
            //
            // dueColumn
            //
            this.dueColumn.Text = "期限";
            this.dueColumn.Width = 130;
            //
            // categoryColumn
            //
            this.categoryColumn.Text = "カテゴリ";
            this.categoryColumn.Width = 120;
            //
            // statusColumn
            //
            this.statusColumn.Text = "状態";
            this.statusColumn.Width = 80;
            //
            // notesColumn
            //
            this.notesColumn.Text = "メモ";
            this.notesColumn.Width = 240;
            //
            // statusStrip
            //
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.summaryStatusLabel,
            this.storageStatusLabel});
            this.statusStrip.Location = new System.Drawing.Point(0, 539);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(984, 22);
            this.statusStrip.TabIndex = 3;
            //
            // summaryStatusLabel
            //
            this.summaryStatusLabel.Name = "summaryStatusLabel";
            this.summaryStatusLabel.Size = new System.Drawing.Size(100, 17);
            this.summaryStatusLabel.Spring = true;
            this.summaryStatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // storageStatusLabel
            //
            this.storageStatusLabel.Name = "storageStatusLabel";
            this.storageStatusLabel.Size = new System.Drawing.Size(100, 17);
            //
            // MainForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 561);
            this.Controls.Add(this.taskListView);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.filterPanel);
            this.Controls.Add(this.toolStrip);
            this.KeyPreview = true;
            this.MinimumSize = new System.Drawing.Size(720, 400);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "タスク管理";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyDown);
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.filterPanel.ResumeLayout(false);
            this.filterPanel.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripButton newButton;
        private System.Windows.Forms.ToolStripButton editButton;
        private System.Windows.Forms.ToolStripButton deleteButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toggleButton;
        private System.Windows.Forms.ToolStripButton purgeButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton reloadButton;
        private System.Windows.Forms.Panel filterPanel;
        private System.Windows.Forms.Label searchLabel;
        private System.Windows.Forms.TextBox searchTextBox;
        private System.Windows.Forms.Label priorityLabel;
        private System.Windows.Forms.ComboBox priorityFilterComboBox;
        private System.Windows.Forms.Label dueLabel;
        private System.Windows.Forms.ComboBox dueFilterComboBox;
        private System.Windows.Forms.Label categoryLabel;
        private System.Windows.Forms.ComboBox categoryFilterComboBox;
        private System.Windows.Forms.CheckBox showCompletedCheckBox;
        private System.Windows.Forms.ListView taskListView;
        private System.Windows.Forms.ColumnHeader titleColumn;
        private System.Windows.Forms.ColumnHeader priorityColumn;
        private System.Windows.Forms.ColumnHeader dueColumn;
        private System.Windows.Forms.ColumnHeader categoryColumn;
        private System.Windows.Forms.ColumnHeader statusColumn;
        private System.Windows.Forms.ColumnHeader notesColumn;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel summaryStatusLabel;
        private System.Windows.Forms.ToolStripStatusLabel storageStatusLabel;
    }
}
