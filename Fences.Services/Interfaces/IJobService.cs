using Fences.Model.DataModels;
using Fences.ViewModels.VM;
using System.Linq.Expressions;

namespace Fences.Services.Interfaces
{
    public interface IJobService
    {
        Task<IEnumerable<JobVm>> GetJobsAsync(Expression<Func<Job, bool>>? filterExpression = null);
        Task<JobVm> GetJobAsync(Expression<Func<Job, bool>>? filterExpression = null);
        Task<JobVm> AddOrUpdateJobAsync(AddOrUpdateJobVm addOrUpdateJobVm);
    }
}
