using Kooliprojekt.Data;
using Kooliprojekt.Search;
using Kooliprojekt.Services;
using KooliProjekt.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Services
{
    public class ProjectListService : IProjectListService
    {
        private readonly IProjectListRepository _projectListRepository;

        public ProjectListService(IProjectListRepository projectListRepository)
        {
            _projectListRepository = projectListRepository;
        }

        public async Task Delete(int id)
        {
            await _projectListRepository.Delete(id);
        }

        public async Task<ProjectList> Get(int id)
        {
            return await _projectListRepository.Get(id);
        }

        public async Task<PagedResult<ProjectList>> List(int page, int pageSize, ProjectListSearch search = null)
        {
            return await _projectListRepository.List(page, pageSize);
        }

        public async Task Save(ProjectList list)
        {
            await _projectListRepository.Save(list);
        }

        public bool ProjectListExists(int id)
        {
            var projectList = _projectListRepository.Get(id).Result;
            return projectList != null;
        }
    }
}
