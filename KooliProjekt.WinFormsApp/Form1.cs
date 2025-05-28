using KooliProjekt.PublicApi.Api;
using System;
using System.Windows.Forms;

namespace KooliProjekt.WinFormsApp
{
    public partial class Form1 : Form
    {
        private readonly IApiClient _apiClient;

        public Form1()
        {
            InitializeComponent();

            _apiClient = new ApiClient();

            TodoListsGrid.SelectionChanged += TodoListsGrid_SelectionChanged;
            NewButton.Click += NewButton_Click;
            SaveButton.Click += SaveButton_Click;
            DeleteButton.Click += DeleteButton_Click;
        }

        private async void DeleteButton_Click(object? sender, EventArgs e)
        {
            if (int.TryParse(IdField.Text, out int id) && id != 0)
            {
                var dialogResult = MessageBox.Show("Kas soovid kustutada valitud kirje?", "Kustutamine", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    await _apiClient.Delete(id);
                    await ReloadData();
                    ClearFields();
                }
            }
            else
            {
                MessageBox.Show("Palun vali kustutatav kirje.");
            }
        }

        private async void SaveButton_Click(object? sender, EventArgs e)
        {
            var workLog = new WorkLog();

            // Parse fields carefully, fallback to defaults if parsing fails
            if (!int.TryParse(IdField.Text, out int id)) id = 0;
            workLog.Id = id;

            if (!DateTime.TryParse(DateField.Text, out DateTime date)) date = DateTime.MinValue;
            workLog.Date = date;

            if (!int.TryParse(TimeSpentField.Text, out int timeSpent)) timeSpent = 0;
            workLog.TimeSpentInMinutes = timeSpent;

            workLog.WorkerName = WorkerNameField.Text ?? string.Empty;
            workLog.Description = DescriptionField.Text ?? string.Empty;

            await _apiClient.Save(workLog);

            await ReloadData();
            ClearFields();
        }

        private void NewButton_Click(object? sender, EventArgs e)
        {
            ClearFields();
        }

        private void TodoListsGrid_SelectionChanged(object? sender, EventArgs e)
        {
            if (TodoListsGrid.SelectedRows.Count > 0)
            {
                var selectedRow = TodoListsGrid.SelectedRows[0];
                var workLog = selectedRow.DataBoundItem as WorkLog;
                if (workLog != null)
                {
                    IdField.Text = workLog.Id.ToString();
                    DateField.Text = workLog.Date.ToString("yyyy-MM-dd");
                    TimeSpentField.Text = workLog.TimeSpentInMinutes.ToString();
                    WorkerNameField.Text = workLog.WorkerName;
                    DescriptionField.Text = workLog.Description;
                    return;
                }
            }
            ClearFields();
        }

        protected override async void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            await ReloadData();
        }

        private async Task ReloadData()
        {
            var result = await _apiClient.List();
            if (!string.IsNullOrEmpty(result.Error))
            {
                MessageBox.Show("Andmete laadimisel tekkis viga: " + result.Error);
                TodoListsGrid.DataSource = null;
                return;
            }
            TodoListsGrid.AutoGenerateColumns = true;
            TodoListsGrid.DataSource = result.Value;
        }

        private void ClearFields()
        {
            IdField.Text = string.Empty;
            DateField.Text = string.Empty;
            TimeSpentField.Text = string.Empty;
            WorkerNameField.Text = string.Empty;
            DescriptionField.Text = string.Empty;
        }
    }
}
