// File: WorkLogPresenter.cs
using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using KooliProjekt.PublicApi.Api;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KooliProjekt.WinFormsApp
{
    public class WorkLogPresenter
    {
        private readonly IApiClient _apiClient;
        private readonly IWorkLogView _view;

        public WorkLogPresenter(IWorkLogView view, IApiClient apiClient)
        {
            _apiClient = apiClient;
            _view = view;
            _view.Presenter = this;
        }

        public async Task LoadAsync()
        {
            try
            {
                var result = await _apiClient.List();
                if (!string.IsNullOrEmpty(result.Error))
                {
                    _view.ShowMessage($"Andmete laadimine ebaõnnestus: {result.Error}", "Viga", MessageBoxIcon.Error);
                    return;
                }

                _view.WorkLogs = result.Value;
                _view.ClearSelection();
            }
            catch (Exception ex)
            {
                _view.ShowMessage($"Andmete laadimisel tekkis ootamatu viga: {ex.Message}", "Viga", MessageBoxIcon.Error);
            }
        }

        public void New()
        {
            _view.ClearSelection();
            _view.Id = "0";
            _view.Date = DateTime.Now.ToString("yyyy-MM-dd");
            _view.TimeSpent = "1";
            _view.WorkerName = "Unknown";
            _view.Description = string.Empty;
        }

        public async Task SaveAsync()
        {
            if (!int.TryParse(_view.Id, out var id))
            {
                _view.ShowMessage("ID väli peab olema täisarv või tühi.", "Viga", MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(_view.Date) ||
                !DateTime.TryParseExact(_view.Date, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var dt))
            {
                _view.ShowMessage("Kuupäev on kohustuslik ja peab olema formaadis yyyy-MM-dd.", "Viga", MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(_view.TimeSpent) ||
                !int.TryParse(_view.TimeSpent, out var tsParsed) ||
                tsParsed < 1 || tsParsed > 1440)
            {
                _view.ShowMessage("Aja kulu minutites on kohustuslik (1–1440).", "Viga", MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(_view.WorkerName))
            {
                _view.ShowMessage("Töötaja nimi on kohustuslik.", "Viga", MessageBoxIcon.Warning);
                return;
            }

            var workLog = new ApiWorkLog
            {
                Id = id,
                Date = dt,
                TimeSpentInMinutes = tsParsed,
                WorkerName = _view.WorkerName,
                Description = string.IsNullOrWhiteSpace(_view.Description) ? null : _view.Description
            };

            try
            {
                var saveResult = await _apiClient.Save(workLog);
                if (!string.IsNullOrEmpty(saveResult.Error))
                {
                    _view.ShowMessage($"Salvestamine ebaõnnestus: {saveResult.Error}", "Viga", MessageBoxIcon.Error);
                }
                else
                {
                    _view.ShowMessage(id <= 0 ? "Uus kirje on loodud." : "Kirje on uuendatud.",
                                      "Info", MessageBoxIcon.Information);
                    await LoadAsync();
                }
            }
            catch (Exception ex)
            {
                _view.ShowMessage($"Salvestamisel tekkis ootamatu viga: {ex.Message}", "Viga", MessageBoxIcon.Error);
            }
        }

        public async Task DeleteAsync()
        {
            var selected = _view.SelectedItem;
            if (selected == null || selected.Id <= 0)
            {
                _view.ShowMessage("Palun vali rida, mida soovid kustutada.", "Hoiatus", MessageBoxIcon.Warning);
                return;
            }

            var dlg = MessageBox.Show($"Kas oled kindel, et soovid kustutada töölogi ID={selected.Id}?",
                                      "Kustuta kinnitamine", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dlg != DialogResult.Yes) return;

            try
            {
                await _apiClient.Delete(selected.Id);
                _view.ShowMessage("Kirje edukalt kustutatud.", "Info", MessageBoxIcon.Information);
                await LoadAsync();
            }
            catch (Exception ex)
            {
                _view.ShowMessage($"Kustutamisel tekkis viga: {ex.Message}", "Viga", MessageBoxIcon.Error);
            }
        }
    }
}
