using Fences.Model.DataModels;
using Fences.ViewModels.VM;
using System.Linq.Expressions;

namespace Fences.Services.Interfaces
{
    public interface IJobService
    {
        IEnumerable<JobVm> GetJobs(Expression<Func<Job, bool>>? filterExpression = null);
        JobVm GetJob(Expression<Func<Job, bool>>? filterExpression = null);
        JobVm AddOrUpdateJob(AddOrUpdateJobVm addOrUpdateJobVm);
    }
}
