
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections.Generic;
using KooliProjekt.PublicApi.Api;
using System;
using System.Collections.Generic;


namespace KooliProjekt.WinFormsApp
{
    public interface IWorkLogView
    {
        IList<ApiWorkLog> WorkLogs { get; set; }
        ApiWorkLog SelectedItem { get; set; }
        string Id { get; set; }
        string Date { get; set; }
        string TimeSpent { get; set; }
        string WorkerName { get; set; }
        string Description { get; set; }

        WorkLogPresenter Presenter { get; set; }
    }
}
