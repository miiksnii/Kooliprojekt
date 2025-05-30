using KooliProjekt.PublicApi.Api;
using System;
using System.Collections.Generic;

namespace KooliProjekt.WinFormsApp
{
    public interface IWorkLogView
    {
        IList<ApiWorkLog> WorkLogs { get; set; }
        ApiWorkLog SelectedItem { get; set; }

        int Id { get; set; }
        DateTime Date { get; set; }
        int TimeSpent { get; set; }
        string WorkerName { get; set; }
        string Description { get; set; }

        WorkLogPresenter Presenter { get; set; }
    }
}
