using Fences.Model.DataModels;
using Fences.ViewModels.VM;
using System.Linq.Expressions;

namespace Fences.Services.Interfaces
{
    public interface IJobService
    {
        Task<IEnumerable<JobVm>> GetJobsAsync(Expression<Func<Job, bool>>? filterExpression = null);
        Task<JobVm> GetJobAsync(Expression<Func<Job, bool>>? filterExpression = null);
        Task AddJobAsync(AddJobVm addJobVm);
        Task UpdateJobAsync(UpdateJobVm updateJobVm);
        Task DeleteJobAsync(JobVm jobVm);
    }
}
