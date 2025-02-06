using Kooliprojekt.Data;
using Kooliprojekt.Search;
using Kooliprojekt.Services;
using KooliProjekt.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Services
{
    public class ProjectListService : IProjectListService
    {
        private readonly IUnitOfWork _IUnitOfWork;

        public ProjectListService(IUnitOfWork IUnitOfWork)
        {
            _IUnitOfWork = IUnitOfWork;
        }

        public async Task Delete(int id)
        {
            await _IUnitOfWork.ProjectListRepository.Delete(id);
        }

        public async Task<ProjectList> Get(int id)
        {
            return await _IUnitOfWork.ProjectListRepository.Get(id);
        }

        public async Task<PagedResult<ProjectList>> List(int page, int pageSize, ProjectListSearch search = null)
        {
            return await _IUnitOfWork.ProjectListRepository.List(page, pageSize);
        }

        public async Task Save(ProjectList list)
        {
            _IUnitOfWork.ProjectListRepository.Save(list);
        }
    }
}
