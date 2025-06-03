// File: Program.cs
using System;
using System.Windows.Forms;
using KooliProjekt.PublicApi.Api;

namespace KooliProjekt.WinFormsApp
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            var form = new Form1();
            var apiClient = new ApiClient();
            var presenter = new WorkLogPresenter(form, apiClient);
            Application.Run(form);
        }
    }
}
