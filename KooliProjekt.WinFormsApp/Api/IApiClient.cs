﻿namespace KooliProjekt.WinFormsApp.Api
{
    public interface IApiClient
    {
        Task<Result<List<WorkLog>>> List();
        Task Save(WorkLog list);
        Task Delete(int id);
    }
}