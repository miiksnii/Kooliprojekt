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
        private readonly IWorkLogView _todoView;

        public WorkLogPresenter(IWorkLogView todoView, IApiClient apiClient)
        {
            _apiClient = apiClient;
            _todoView = todoView;

            todoView.Presenter = this;
        }

        public void UpdateView(ApiWorkLog workLog) // Parandatud tüüp: ApiWorkLog
        {
            if (workLog == null)
            {
                // Kui workLog on null, puhasta kõik vaate väljad ja sea vaikimisi väärtused uueks kirjeks
                _todoView.Id = "0"; // Vaikimisi ID uue kirje jaoks
                _todoView.Date = DateTime.Now.ToString("yyyy-MM-dd"); // Vaikimisi tänane kuupäev
                _todoView.TimeSpent = "1"; // Vaikimisi 1 minut
                _todoView.WorkerName = string.Empty;
                _todoView.Description = string.Empty;                
            }
            else
            {
                // Kui workLog ei ole null, täida vaate väljad objekti andmetega
                _todoView.Id = workLog.Id.ToString();
                _todoView.Date = workLog.Date?.ToString("yyyy-MM-dd");
                _todoView.TimeSpent = workLog.TimeSpentInMinutes?.ToString() ?? string.Empty; // Kasutame null-coalescing operaatorit
                _todoView.WorkerName = workLog.WorkerName ?? string.Empty; // Kasutame null-coalescing operaatorit
                _todoView.Description = workLog.Description ?? string.Empty; // Kasutame null-coalescing operaatorit
            }
        }

        public async Task Load()
        {
            var todoLists = await _apiClient.List();
            _todoView.WorkLogs = todoLists.Value;
        }
    }
}
