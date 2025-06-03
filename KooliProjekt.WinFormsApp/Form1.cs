using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows.Forms;
using KooliProjekt.PublicApi.Api;

namespace KooliProjekt.WinFormsApp
{
    public partial class Form1 : Form, IWorkLogView
    {
        public WorkLogPresenter Presenter { get; set; }

        public Form1()
        {
            InitializeComponent();

            TodoListsGrid.SelectionChanged += (s, e) => OnSelectionChanged();
            NewButton.Click += (s, e) => Presenter.UpdateView(null);
            SaveButton.Click += async (s, e) => await SaveCurrentAsync();
            DeleteButton.Click += async (s, e) => await DeleteCurrentAsync();

            TimeSpentField.TextChanged += (s, e) =>
            {
                if (!string.IsNullOrEmpty(TimeSpentField.Text) && !int.TryParse(TimeSpentField.Text, out _))
                    TimeSpentField.BackColor = Color.LightPink;
                else
                    TimeSpentField.BackColor = Color.White;
            };
        }

        protected override async void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            // Presenter is set in Program.cs before Application.Run(form)
            await Presenter.Load();
        }

        // IWorkLogView implementation

        public IList<ApiWorkLog> WorkLogs
        {
            get => (IList<ApiWorkLog>)TodoListsGrid.DataSource;
            set
            {
                TodoListsGrid.AutoGenerateColumns = true;
                TodoListsGrid.DataSource = value;
                ClearSelection();
            }
        }

        public ApiWorkLog SelectedItem
        {
            get
            {
                if (TodoListsGrid.SelectedRows.Count == 0) return null;
                return (ApiWorkLog)TodoListsGrid.SelectedRows[0].DataBoundItem;
            }
            set
            {
                if (value == null)
                {
                    ClearFields();
                    return;
                }

                Id = value.Id.ToString();
                Date = value.Date?.ToString("yyyy-MM-dd") ?? string.Empty;
                TimeSpent = value.TimeSpentInMinutes?.ToString() ?? string.Empty;
                WorkerName = value.WorkerName;
                Description = value.Description;
            }
        }

        public string Id
        {
            get => IdField.Text;
            set => IdField.Text = value;
        }

        public string Date
        {
            get => DateField.Text;
            set => DateField.Text = value;
        }

        public string TimeSpent
        {
            get => TimeSpentField.Text;
            set => TimeSpentField.Text = value;
        }

        public string WorkerName
        {
            get => WorkerNameField.Text;
            set => WorkerNameField.Text = value;
        }

        public string Description
        {
            get => DescriptionField.Text;
            set => DescriptionField.Text = value;
        }

        public void ShowMessage(string text, string caption, MessageBoxIcon icon)
        {
            MessageBox.Show(text, caption, MessageBoxButtons.OK, icon);
        }

        public void ClearSelection()
        {
            TodoListsGrid.ClearSelection();
        }

        public void ClearFields()
        {
            IdField.Text = string.Empty;
            DateField.Text = string.Empty;
            TimeSpentField.Text = string.Empty;
            WorkerNameField.Text = string.Empty;
            DescriptionField.Text = string.Empty;
        }

        private void OnSelectionChanged()
        {
            var item = SelectedItem;
            Presenter.UpdateView(item);
        }

        private async Task SaveCurrentAsync()
        {
            // Kogume vaate väljade väärtused uue ApiWorkLog-objekti sisse
            if (!int.TryParse(Id, out var id)) id = 0;

            if (!DateTime.TryParseExact(Date, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var dt))
            {
                ShowMessage("Kuupäev on kohustuslik ja peab olema formaadis yyyy-MM-dd.", "Viga", MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(TimeSpent, out var ts) || ts < 1 || ts > 1440)
            {
                ShowMessage("Aja kulu minutites on kohustuslik (1–1440).", "Viga", MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(WorkerName))
            {
                ShowMessage("Töötaja nimi on kohustuslik.", "Viga", MessageBoxIcon.Warning);
                return;
            }

            var workLog = new ApiWorkLog
            {
                Id = id,
                Date = dt,
                TimeSpentInMinutes = ts,
                WorkerName = WorkerName,
                Description = string.IsNullOrWhiteSpace(Description) ? null : Description
            };

            try
            {
                var result = await new ApiClient().Save(workLog);
                if (!string.IsNullOrEmpty(result.Error))
                {
                    ShowMessage($"Salvestamine ebaõnnestus: {result.Error}", "Viga", MessageBoxIcon.Error);
                    return;
                }

                ShowMessage(id <= 0 ? "Uus kirje on loodud." : "Kirje on uuendatud.", "Info", MessageBoxIcon.Information);
                await Presenter.Load();
            }
            catch (Exception ex)
            {
                ShowMessage($"Salvestamisel tekkis ootamatu viga: {ex.Message}", "Viga", MessageBoxIcon.Error);
            }
        }

        private async Task DeleteCurrentAsync()
        {
            var item = SelectedItem;
            if (item == null || item.Id <= 0)
            {
                ShowMessage("Palun vali rida, mida soovid kustutada.", "Hoiatus", MessageBoxIcon.Warning);
                return;
            }

            var dlg = MessageBox.Show(
                $"Kas oled kindel, et soovid kustutada töölogi ID={item.Id}?",
                "Kustuta kinnitamine",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );
            if (dlg != DialogResult.Yes) return;

            try
            {
                await new ApiClient().Delete(item.Id);
                ShowMessage("Kirje edukalt kustutatud.", "Info", MessageBoxIcon.Information);
                await Presenter.Load();
            }
            catch (Exception ex)
            {
                ShowMessage($"Kustutamisel tekkis viga: {ex.Message}", "Viga", MessageBoxIcon.Error);
            }
        }
    }
}
