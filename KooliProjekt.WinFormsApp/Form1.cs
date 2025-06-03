using System;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using KooliProjekt.PublicApi.Api;

namespace KooliProjekt.WinFormsApp
{
    public partial class Form1 : Form, IWorkLogView
    {
        public Form1()
        {
            InitializeComponent();

            // meetodid
            TodoListsGrid.SelectionChanged += TodoListsGrid_SelectionChanged;
            NewButton.Click += NewButton_Click;
            SaveButton.Click += SaveButton_Click;
            DeleteButton.Click += DeleteButton_Click;
            TimeSpentField.TextChanged += TimeSpentField_TextChanged;
            
        }

        protected override async void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            await LoadDataAsync();
        }

        /// <summary>
        ///  Andmete laadimine: kutsub ApiClient.List(),
        ///  kuvab vea või seob DataGridView’ga.
        /// </summary>
        private async Task LoadDataAsync()
        {
            try
            {
                var apiClient = new ApiClient();
                var result = await apiClient.List();

                if (!string.IsNullOrEmpty(result.Error))
                {
                    MessageBox.Show(
                        $"Andmete laadimine ebaõnnestus: {result.Error}",
                        "Viga",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                    return;
                }

                TodoListsGrid.AutoGenerateColumns = true;
                TodoListsGrid.DataSource = result.Value;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Andmete laadimisel tekkis ootamatu viga: {ex.Message}",
                    "Viga",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void NewButton_Click(object? sender, EventArgs e)
        {
            TodoListsGrid.ClearSelection();

            IdField.Text = "0";
            DateField.Text = DateTime.Now.ToString("yyyy-MM-dd");
            TimeSpentField.Text = "1";
            WorkerNameField.Text = "Unknown";
            DescriptionField.Text = string.Empty;

            DateField.Focus();
        }


        /// <summary>
        ///  “Save” nupp:
        ///  - kui IdField tühi või 0 → POST
        ///  - muidu PUT
        ///  Pärast salvestust: uuesti LoadDataAsync().
        /// </summary>
        private async void SaveButton_Click(object? sender, EventArgs e)
        {
            // 1) ID-parsimine
            int id = 0;
            if (!string.IsNullOrWhiteSpace(IdField.Text))
            {   

                if (!int.TryParse(IdField.Text, out id))
                {
                    MessageBox.Show(
                        "ID väli peab olema täisarv või tühi.",
                        "Viga",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );
                    return;
                }
            }

            // 2) Kuupäeva parsimine (yyyy-MM-dd)
            DateTime? dateValue = null;
            if (!string.IsNullOrWhiteSpace(DateField.Text))
            {
                if (DateTime.TryParseExact(
                        DateField.Text,
                        "yyyy-MM-dd",
                        CultureInfo.InvariantCulture,
                        DateTimeStyles.None,
                        out var dt
                    ))
                {
                    dateValue = dt;
                }
                else
                {
                    MessageBox.Show(
                        "Kuupäeva formaat peab olema yyyy-MM-dd.",
                        "Viga",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );
                    return;
                }
            }

            // 3) TimeSpentInMinutes (int?)
            int? timeSpent = null;
            if (!string.IsNullOrWhiteSpace(TimeSpentField.Text))
            {
                if (int.TryParse(TimeSpentField.Text, out var tsParsed))
                {
                    timeSpent = tsParsed;
                }
                else
                {
                    MessageBox.Show(
                        "Aja kulu minutites peab olema täisarv.",
                        "Viga",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );
                    return;
                }
            }

            // 4) WorkerName ja Description (nullable)
            var workerName = WorkerNameField.Text;
            var description = DescriptionField.Text;

            // 5) Koosta ApiWorkLog
            var workLog = new ApiWorkLog
            {
                Id = id,
                Date = dateValue,
                TimeSpentInMinutes = timeSpent,
                WorkerName = string.IsNullOrWhiteSpace(workerName) ? null : workerName,
                Description = string.IsNullOrWhiteSpace(description) ? null : description
            };

            // 6) Salvesta API kaudu
            try
            {
                var apiClient = new ApiClient();
                var saveResult = await apiClient.Save(workLog);

                if (!string.IsNullOrEmpty(saveResult.Error))
                {
                    // Server tagastas vea
                    MessageBox.Show(
                        $"Salvestamine ebaõnnestus: {saveResult.Error}",
                        "Viga",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                }
                else
                {
                    // Edukas salvestus
                    if (id <= 0)
                        MessageBox.Show(
                            "Uus kirje on loodud.",
                            "Info",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information
                        );
                    else
                        MessageBox.Show(
                            "Kirje on uuendatud.",
                            "Info",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information
                        );

                    // 7) Uuesti andmed gridi
                    await LoadDataAsync();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Salvestamisel tekkis ootamatu viga: {ex.Message}",
                    "Viga",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        /// <summary>
        ///  “Delete” nupp:
        ///  - kontrollid, et rida on valitud ja Id > 0
        ///  - küsi kinnitust
        ///  - kutsu apiClient.Delete(id)
        ///  - uuenda gridi
        /// </summary>
        private async void DeleteButton_Click(object? sender, EventArgs e)
        {
            // 1) Kontroll, kas rida valitud
            if (TodoListsGrid.SelectedRows.Count == 0)
            {
                MessageBox.Show(
                    "Palun vali rida, mida soovid kustutada.",
                    "Hoiatus",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                return;
            }

            // 2) Võta ApiWorkLog
            var selected = (ApiWorkLog)TodoListsGrid.SelectedRows[0].DataBoundItem;
            if (selected == null || selected.Id <= 0)
            {
                MessageBox.Show(
                    "Valitud objekt ei sisalda kehtivat Id-d.",
                    "Viga",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return;
            }

            // 3) Kinnita
            var dlg = MessageBox.Show(
                $"Kas oled kindel, et soovid kustutada töölogi ID={selected.Id}?",
                "Kustuta kinnitamine",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (dlg != DialogResult.Yes)
                return;

            // 4) Kustuta API kaudu ja uuenda gridi
            try
            {
                var apiClient = new ApiClient();
                await apiClient.Delete(selected.Id);

                MessageBox.Show(
                    "Kirje edukalt kustutatud.",
                    "Info",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                await LoadDataAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Kustutamisel tekkis viga: {ex.Message}",
                    "Viga",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        /// <summary>
        ///  Kui kasutaja valib rea gridi’s, täida tekstiväljad vastavalt valitud ApiWorkLog objektile.
        /// </summary>
        private void TodoListsGrid_SelectionChanged(object? sender, EventArgs e)
        {
            if (TodoListsGrid.SelectedRows.Count == 0)
            {
                IdField.Text = string.Empty;
                DateField.Text = string.Empty;
                TimeSpentField.Text = string.Empty;
                WorkerNameField.Text = string.Empty;
                DescriptionField.Text = string.Empty;
                return;
            }

            var todoList = (ApiWorkLog)TodoListsGrid.SelectedRows[0].DataBoundItem;
            if (todoList == null)
            {
                IdField.Text = string.Empty;
                DateField.Text = string.Empty;
                TimeSpentField.Text = string.Empty;
                WorkerNameField.Text = string.Empty;
                DescriptionField.Text = string.Empty;
            }
            else
            {
                // Id
                IdField.Text = todoList.Id.ToString();

                // Kuupäev
                if (todoList.Date.HasValue)
                    DateField.Text = todoList.Date.Value.ToString("yyyy-MM-dd");
                else
                    DateField.Text = string.Empty;

                // Kulutatud aeg
                TimeSpentField.Text = todoList.TimeSpentInMinutes.HasValue
                    ? todoList.TimeSpentInMinutes.Value.ToString()
                    : string.Empty;

                // Nimi ja kirjeldus
                WorkerNameField.Text = todoList.WorkerName ?? string.Empty;
                DescriptionField.Text = todoList.Description ?? string.Empty;
            }
        }

        /// <summary>
        ///  Live-valiide: kui TimeSpentField-s pole täisarv, muuda taust punaseks, vastasel juhul valgeks.
        /// </summary>
        private void TimeSpentField_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(TimeSpentField.Text) && !int.TryParse(TimeSpentField.Text, out _))
                TimeSpentField.BackColor = Color.LightPink;
            else
                TimeSpentField.BackColor = Color.White;
        }
    }
}
