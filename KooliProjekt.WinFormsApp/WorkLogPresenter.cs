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

        public WorkLogPresenter(IApiClient apiClient, IWorkLogView view)
        {
            _apiClient = apiClient;
            _view = view;
        }

        public async Task Load()
        {
            var result = await _apiClient.List();
            if (!string.IsNullOrEmpty(result.Error))
            {
                MessageBox.Show("Andmete laadimisel tekkis viga: " + result.Error);
                _view.WorkLogs = new List<ApiWorkLog>();
                return;
            }

            _view.WorkLogs = result.Value;
        }

        public void New()
        {
            _view.Id = 0;
            _view.Date = DateTime.Now;
            _view.TimeSpent = 0;
            _view.WorkerName = string.Empty;
            _view.Description = string.Empty;
        }

        public async Task Save()
        {
            var item = new ApiWorkLog
            {
                Id = _view.Id,
                Date = _view.Date,
                TimeSpentInMinutes = _view.TimeSpent,
                WorkerName = _view.WorkerName,
                Description = _view.Description
            };

            await _apiClient.Save(item);
            await Load();
            New();
        }

        public async Task Delete()
        {
            if (_view.Id != 0)
            {
                var dialogResult = MessageBox.Show("Kas soovid kustutada valitud kirje?", "Kustutamine", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    await _apiClient.Delete(_view.Id);
                    await Load();
                    New();
                }
            }
            else
            {
                MessageBox.Show("Palun vali kustutatav kirje.");
            }
        }

        public void UpdateView(ApiWorkLog item)
        {
            if (item == null)
            {
                New();
                return;
            }

            _view.Id = item.Id;
            _view.Date = item.Date;
            _view.TimeSpent = item.TimeSpentInMinutes;
            _view.WorkerName = item.WorkerName;
            _view.Description = item.Description;
        }
    }
}
