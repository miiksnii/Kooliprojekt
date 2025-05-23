namespace KooliProjekt.WinFormsApp
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

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
            DateLabel = new Label();
            DateField = new TextBox();
            TimeSpentLabel = new Label();
            TimeSpentField = new TextBox();
            WorkerNameLabel = new Label();
            WorkerNameField = new TextBox();
            DescriptionLabel = new Label();
            DescriptionField = new TextBox();
            NewButton = new Button();
            SaveButton = new Button();
            DeleteButton = new Button();

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
            // DateLabel
            // 
            DateLabel.AutoSize = true;
            DateLabel.Location = new Point(460, 96);
            DateLabel.Name = "DateLabel";
            DateLabel.Size = new Size(36, 15);
            DateLabel.TabIndex = 5;
            DateLabel.Text = "Date:";

            // 
            // DateField
            // 
            DateField.Location = new Point(507, 93);
            DateField.Name = "DateField";
            DateField.Size = new Size(281, 23);
            DateField.TabIndex = 6;

            // 
            // TimeSpentLabel
            // 
            TimeSpentLabel.AutoSize = true;
            TimeSpentLabel.Location = new Point(460, 136);
            TimeSpentLabel.Name = "TimeSpentLabel";
            TimeSpentLabel.Size = new Size(81, 15);
            TimeSpentLabel.TabIndex = 7;
            TimeSpentLabel.Text = "Time Spent (m):";

            // 
            // TimeSpentField
            // 
            TimeSpentField.Location = new Point(547, 133);
            TimeSpentField.Name = "TimeSpentField";
            TimeSpentField.Size = new Size(241, 23);
            TimeSpentField.TabIndex = 8;

            // 
            // WorkerNameLabel
            // 
            WorkerNameLabel.AutoSize = true;
            WorkerNameLabel.Location = new Point(460, 176);
            WorkerNameLabel.Name = "WorkerNameLabel";
            WorkerNameLabel.Size = new Size(85, 15);
            WorkerNameLabel.TabIndex = 9;
            WorkerNameLabel.Text = "Worker Name:";

            // 
            // WorkerNameField
            // 
            WorkerNameField.Location = new Point(551, 173);
            WorkerNameField.Name = "WorkerNameField";
            WorkerNameField.Size = new Size(237, 23);
            WorkerNameField.TabIndex = 10;

            // 
            // DescriptionLabel
            // 
            DescriptionLabel.AutoSize = true;
            DescriptionLabel.Location = new Point(460, 216);
            DescriptionLabel.Name = "DescriptionLabel";
            DescriptionLabel.Size = new Size(70, 15);
            DescriptionLabel.TabIndex = 11;
            DescriptionLabel.Text = "Description:";

            // 
            // DescriptionField
            // 
            DescriptionField.Location = new Point(460, 234);
            DescriptionField.Multiline = true;
            DescriptionField.Name = "DescriptionField";
            DescriptionField.Size = new Size(328, 150);
            DescriptionField.TabIndex = 12;

            // 
            // NewButton
            // 
            NewButton.Location = new Point(460, 400);
            NewButton.Name = "NewButton";
            NewButton.Size = new Size(75, 23);
            NewButton.TabIndex = 13;
            NewButton.Text = "New";
            NewButton.UseVisualStyleBackColor = true;

            // 
            // SaveButton
            // 
            SaveButton.Location = new Point(541, 400);
            SaveButton.Name = "SaveButton";
            SaveButton.Size = new Size(75, 23);
            SaveButton.TabIndex = 14;
            SaveButton.Text = "Save";
            SaveButton.UseVisualStyleBackColor = true;

            // 
            // DeleteButton
            // 
            DeleteButton.Location = new Point(622, 400);
            DeleteButton.Name = "DeleteButton";
            DeleteButton.Size = new Size(75, 23);
            DeleteButton.TabIndex = 15;
            DeleteButton.Text = "Delete";
            DeleteButton.UseVisualStyleBackColor = true;

            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(DeleteButton);
            Controls.Add(SaveButton);
            Controls.Add(NewButton);
            Controls.Add(DescriptionField);
            Controls.Add(DescriptionLabel);
            Controls.Add(WorkerNameField);
            Controls.Add(WorkerNameLabel);
            Controls.Add(TimeSpentField);
            Controls.Add(TimeSpentLabel);
            Controls.Add(DateField);
            Controls.Add(DateLabel);            
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

        private Label DateLabel;
        private TextBox DateField;

        private Label TimeSpentLabel;
        private TextBox TimeSpentField;

        private Label WorkerNameLabel;
        private TextBox WorkerNameField;

        private Label DescriptionLabel;
        private TextBox DescriptionField;

        private Button NewButton;
        private Button SaveButton;
        private Button DeleteButton;

    }
}
