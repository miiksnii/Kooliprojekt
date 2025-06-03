namespace KooliProjekt.WinFormsApp
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            TodoListsGrid = new DataGridView();
            IdLabel = new Label();
            IdField = new TextBox();
            TitleLabel = new Label();
            TitleField = new TextBox();
            NewButton = new Button();
            SaveButton = new Button();
            DeleteButton = new Button();
            DateLabel = new Label();
            DateField = new TextBox();
            TimeSpentLabel = new Label();
            TimeSpentField = new TextBox();
            WorkerNameLabel = new Label();
            WorkerNameField = new TextBox();
            DescriptionLabel = new Label();
            DescriptionField = new TextBox();
            ((System.ComponentModel.ISupportInitialize)TodoListsGrid).BeginInit();
            SuspendLayout();
            // 
            // TodoListsGrid
            // 
            TodoListsGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            TodoListsGrid.Location = new Point(5, 6);
            TodoListsGrid.MultiSelect = false;
            TodoListsGrid.Name = "TodoListsGrid";
            TodoListsGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            TodoListsGrid.Size = new Size(419, 432);
            TodoListsGrid.TabIndex = 0;
            // 
            // IdLabel
            // 
            IdLabel.AutoSize = true;
            IdLabel.Location = new Point(460, 16);
            IdLabel.Name = "IdLabel";
            IdLabel.Size = new Size(21, 15);
            IdLabel.TabIndex = 1;
            IdLabel.Text = "ID:";
            // 
            // IdField
            // 
            IdField.Location = new Point(507, 13);
            IdField.Name = "IdField";
            IdField.ReadOnly = true;
            IdField.Size = new Size(281, 23);
            IdField.TabIndex = 2;
            // 
            // TitleLabel
            // 
            TitleLabel.AutoSize = true;
            TitleLabel.Location = new Point(460, 56);
            TitleLabel.Name = "TitleLabel";
            TitleLabel.Size = new Size(33, 15);
            TitleLabel.TabIndex = 3;
            TitleLabel.Text = "Title:";
            // 
            // TitleField
            // 
            TitleField.Location = new Point(507, 53);
            TitleField.Name = "TitleField";
            TitleField.Size = new Size(281, 23);
            TitleField.TabIndex = 4;
            // 
            // NewButton
            // 
            NewButton.Location = new Point(522, 96);
            NewButton.Name = "NewButton";
            NewButton.Size = new Size(75, 23);
            NewButton.TabIndex = 5;
            NewButton.Text = "New";
            NewButton.UseVisualStyleBackColor = true;
            // 
            // SaveButton
            // 
            SaveButton.Location = new Point(603, 96);
            SaveButton.Name = "SaveButton";
            SaveButton.Size = new Size(75, 23);
            SaveButton.TabIndex = 6;
            SaveButton.Text = "Save";
            SaveButton.UseVisualStyleBackColor = true;
            // 
            // DeleteButton
            // 
            DeleteButton.Location = new Point(684, 96);
            DeleteButton.Name = "DeleteButton";
            DeleteButton.Size = new Size(75, 23);
            DeleteButton.TabIndex = 7;
            DeleteButton.Text = "Delete";
            DeleteButton.UseVisualStyleBackColor = true;
            // 
            // DateLabel
            // 
            DateLabel.AutoSize = true;
            DateLabel.Location = new Point(460, 140);
            DateLabel.Name = "DateLabel";
            DateLabel.Size = new Size(34, 15);
            DateLabel.TabIndex = 8;
            DateLabel.Text = "Date:";
            // 
            // DateField
            // 
            DateField.Location = new Point(507, 137);
            DateField.Name = "DateField";
            DateField.Size = new Size(281, 23);
            DateField.TabIndex = 9;
            // 
            // TimeSpentLabel
            // 
            TimeSpentLabel.AutoSize = true;
            TimeSpentLabel.Location = new Point(460, 180);
            TimeSpentLabel.Name = "TimeSpentLabel";
            TimeSpentLabel.Size = new Size(102, 15);
            TimeSpentLabel.TabIndex = 10;
            TimeSpentLabel.Text = "Time Spent (min):";
            // 
            // TimeSpentField
            // 
            TimeSpentField.Location = new Point(557, 177);
            TimeSpentField.Name = "TimeSpentField";
            TimeSpentField.Size = new Size(231, 23);
            TimeSpentField.TabIndex = 11;            
            // 
            // WorkerNameLabel
            // 
            WorkerNameLabel.AutoSize = true;
            WorkerNameLabel.Location = new Point(460, 220);
            WorkerNameLabel.Name = "WorkerNameLabel";
            WorkerNameLabel.Size = new Size(83, 15);
            WorkerNameLabel.TabIndex = 12;
            WorkerNameLabel.Text = "Worker Name:";
            // 
            // WorkerNameField
            // 
            WorkerNameField.Location = new Point(555, 217);
            WorkerNameField.Name = "WorkerNameField";
            WorkerNameField.Size = new Size(233, 23);
            WorkerNameField.TabIndex = 13;
            // 
            // DescriptionLabel
            // 
            DescriptionLabel.AutoSize = true;
            DescriptionLabel.Location = new Point(460, 260);
            DescriptionLabel.Name = "DescriptionLabel";
            DescriptionLabel.Size = new Size(70, 15);
            DescriptionLabel.TabIndex = 14;
            DescriptionLabel.Text = "Description:";
            // 
            // DescriptionField
            // 
            DescriptionField.Location = new Point(537, 257);
            DescriptionField.Multiline = true;
            DescriptionField.Name = "DescriptionField";
            DescriptionField.Size = new Size(251, 75);
            DescriptionField.TabIndex = 15;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(DescriptionField);
            Controls.Add(DescriptionLabel);
            Controls.Add(WorkerNameField);
            Controls.Add(WorkerNameLabel);
            Controls.Add(TimeSpentField);
            Controls.Add(TimeSpentLabel);
            Controls.Add(DateField);
            Controls.Add(DateLabel);
            Controls.Add(DeleteButton);
            Controls.Add(SaveButton);
            Controls.Add(NewButton);
            Controls.Add(TitleField);
            Controls.Add(TitleLabel);
            Controls.Add(IdField);
            Controls.Add(IdLabel);
            Controls.Add(TodoListsGrid);
            Name = "Form1";
            Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)TodoListsGrid).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView TodoListsGrid;
        private Label IdLabel;
        private TextBox IdField;
        private Label TitleLabel;
        private TextBox TitleField;
        private Button NewButton;
        private Button SaveButton;
        private Button DeleteButton;

        // Uued väljad ja sildid
        private Label DateLabel;
        private TextBox DateField;
        private Label TimeSpentLabel;
        private TextBox TimeSpentField;
        private Label WorkerNameLabel;
        private TextBox WorkerNameField;
        private Label DescriptionLabel;
        private TextBox DescriptionField;
    }
}
