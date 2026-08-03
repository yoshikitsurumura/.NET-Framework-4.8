namespace TaskManager.App.Forms
{
    partial class TaskEditForm
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
            this.titleLabel = new System.Windows.Forms.Label();
            this.titleTextBox = new System.Windows.Forms.TextBox();
            this.categoryLabel = new System.Windows.Forms.Label();
            this.categoryComboBox = new System.Windows.Forms.ComboBox();
            this.priorityLabel = new System.Windows.Forms.Label();
            this.priorityComboBox = new System.Windows.Forms.ComboBox();
            this.dueDateCheckBox = new System.Windows.Forms.CheckBox();
            this.dueDatePicker = new System.Windows.Forms.DateTimePicker();
            this.completedCheckBox = new System.Windows.Forms.CheckBox();
            this.notesLabel = new System.Windows.Forms.Label();
            this.notesTextBox = new System.Windows.Forms.TextBox();
            this.infoLabel = new System.Windows.Forms.Label();
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            //
            // titleLabel
            //
            this.titleLabel.AutoSize = true;
            this.titleLabel.Location = new System.Drawing.Point(12, 12);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(80, 15);
            this.titleLabel.TabIndex = 0;
            this.titleLabel.Text = "タイトル (必須)";
            //
            // titleTextBox
            //
            this.titleTextBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            this.titleTextBox.Location = new System.Drawing.Point(12, 32);
            this.titleTextBox.MaxLength = 200;
            this.titleTextBox.Name = "titleTextBox";
            this.titleTextBox.Size = new System.Drawing.Size(444, 23);
            this.titleTextBox.TabIndex = 1;
            //
            // categoryLabel
            //
            this.categoryLabel.AutoSize = true;
            this.categoryLabel.Location = new System.Drawing.Point(12, 68);
            this.categoryLabel.Name = "categoryLabel";
            this.categoryLabel.Size = new System.Drawing.Size(55, 15);
            this.categoryLabel.TabIndex = 2;
            this.categoryLabel.Text = "カテゴリ";
            //
            // categoryComboBox
            //
            this.categoryComboBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.categoryComboBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.categoryComboBox.Location = new System.Drawing.Point(12, 88);
            this.categoryComboBox.MaxLength = 60;
            this.categoryComboBox.Name = "categoryComboBox";
            this.categoryComboBox.Size = new System.Drawing.Size(220, 23);
            this.categoryComboBox.TabIndex = 3;
            //
            // priorityLabel
            //
            this.priorityLabel.AutoSize = true;
            this.priorityLabel.Location = new System.Drawing.Point(252, 68);
            this.priorityLabel.Name = "priorityLabel";
            this.priorityLabel.Size = new System.Drawing.Size(45, 15);
            this.priorityLabel.TabIndex = 4;
            this.priorityLabel.Text = "優先度";
            //
            // priorityComboBox
            //
            this.priorityComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.priorityComboBox.Location = new System.Drawing.Point(252, 88);
            this.priorityComboBox.Name = "priorityComboBox";
            this.priorityComboBox.Size = new System.Drawing.Size(140, 23);
            this.priorityComboBox.TabIndex = 5;
            //
            // dueDateCheckBox
            //
            this.dueDateCheckBox.AutoSize = true;
            this.dueDateCheckBox.Location = new System.Drawing.Point(12, 128);
            this.dueDateCheckBox.Name = "dueDateCheckBox";
            this.dueDateCheckBox.Size = new System.Drawing.Size(100, 19);
            this.dueDateCheckBox.TabIndex = 6;
            this.dueDateCheckBox.Text = "期限を設定する";
            this.dueDateCheckBox.UseVisualStyleBackColor = true;
            this.dueDateCheckBox.CheckedChanged += new System.EventHandler(this.DueDateCheckBox_CheckedChanged);
            //
            // dueDatePicker
            //
            this.dueDatePicker.Enabled = false;
            this.dueDatePicker.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dueDatePicker.Location = new System.Drawing.Point(140, 126);
            this.dueDatePicker.Name = "dueDatePicker";
            this.dueDatePicker.Size = new System.Drawing.Size(160, 23);
            this.dueDatePicker.TabIndex = 7;
            //
            // completedCheckBox
            //
            this.completedCheckBox.AutoSize = true;
            this.completedCheckBox.Location = new System.Drawing.Point(320, 128);
            this.completedCheckBox.Name = "completedCheckBox";
            this.completedCheckBox.Size = new System.Drawing.Size(70, 19);
            this.completedCheckBox.TabIndex = 8;
            this.completedCheckBox.Text = "完了済み";
            this.completedCheckBox.UseVisualStyleBackColor = true;
            //
            // notesLabel
            //
            this.notesLabel.AutoSize = true;
            this.notesLabel.Location = new System.Drawing.Point(12, 160);
            this.notesLabel.Name = "notesLabel";
            this.notesLabel.Size = new System.Drawing.Size(30, 15);
            this.notesLabel.TabIndex = 9;
            this.notesLabel.Text = "メモ";
            //
            // notesTextBox
            //
            this.notesTextBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            this.notesTextBox.Location = new System.Drawing.Point(12, 180);
            this.notesTextBox.MaxLength = 4000;
            this.notesTextBox.Multiline = true;
            this.notesTextBox.Name = "notesTextBox";
            this.notesTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.notesTextBox.Size = new System.Drawing.Size(444, 140);
            this.notesTextBox.TabIndex = 10;
            //
            // infoLabel
            //
            this.infoLabel.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            this.infoLabel.AutoSize = true;
            this.infoLabel.ForeColor = System.Drawing.SystemColors.GrayText;
            this.infoLabel.Location = new System.Drawing.Point(12, 336);
            this.infoLabel.Name = "infoLabel";
            this.infoLabel.Size = new System.Drawing.Size(0, 15);
            this.infoLabel.TabIndex = 11;
            //
            // okButton
            //
            this.okButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            this.okButton.Location = new System.Drawing.Point(276, 332);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(86, 28);
            this.okButton.TabIndex = 12;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.OkButton_Click);
            //
            // cancelButton
            //
            this.cancelButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(370, 332);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(86, 28);
            this.cancelButton.TabIndex = 13;
            this.cancelButton.Text = "キャンセル";
            this.cancelButton.UseVisualStyleBackColor = true;
            //
            // TaskEditForm
            //
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(468, 372);
            this.Controls.Add(this.titleLabel);
            this.Controls.Add(this.titleTextBox);
            this.Controls.Add(this.categoryLabel);
            this.Controls.Add(this.categoryComboBox);
            this.Controls.Add(this.priorityLabel);
            this.Controls.Add(this.priorityComboBox);
            this.Controls.Add(this.dueDateCheckBox);
            this.Controls.Add(this.dueDatePicker);
            this.Controls.Add(this.completedCheckBox);
            this.Controls.Add(this.notesLabel);
            this.Controls.Add(this.notesTextBox);
            this.Controls.Add(this.infoLabel);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.cancelButton);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(420, 380);
            this.Name = "TaskEditForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "タスクの編集";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label titleLabel;
        private System.Windows.Forms.TextBox titleTextBox;
        private System.Windows.Forms.Label categoryLabel;
        private System.Windows.Forms.ComboBox categoryComboBox;
        private System.Windows.Forms.Label priorityLabel;
        private System.Windows.Forms.ComboBox priorityComboBox;
        private System.Windows.Forms.CheckBox dueDateCheckBox;
        private System.Windows.Forms.DateTimePicker dueDatePicker;
        private System.Windows.Forms.CheckBox completedCheckBox;
        private System.Windows.Forms.Label notesLabel;
        private System.Windows.Forms.TextBox notesTextBox;
        private System.Windows.Forms.Label infoLabel;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
    }
}
