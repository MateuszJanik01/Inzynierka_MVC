using AutoMapper;
using Fences.DAL.EF;
using Fences.Services.Interfaces;
using Fences.ViewModels.VM;
using Microsoft.EntityFrameworkCore;

namespace Fences.Tests.UnitTests;

public class JobServiceUnitTests : BaseUnitTests
{
    private readonly IJobService _jobService = null!;

    public JobServiceUnitTests(IJobService jobService, ApplicationDbContext dbContext, IMapper mapper)
        : base(dbContext, mapper)
    {
        _jobService = jobService;
    }

    [Fact]
    public async void AddJobAsync_ShouldAddJob_WhenValidData()
    {
        var addJobVm = new AddJobVm
        {
            UserId = 1,
            Town = "Poznań",
            Street = "Kościelna",
            Number = "34",
            ZipCode = "60-201",
            TotalLength = 100,
            Height = 2,
            JobType = "Ogrodzenie Betonowe",
            Description = "Description",
            RegistrationDate = DateTime.Now,
            DateOfExecution = DateTime.Now.AddDays(4),
        };

        await _jobService.AddJobAsync(addJobVm);
        var result = await DbContext.Jobs.FirstOrDefaultAsync(j => j.Town == addJobVm.Town);

        Assert.NotNull(result);
        Assert.Equal(addJobVm.Town, result.Town);
    }

    [Fact]
    public async void GetJobAsync_ShouldReturnOneJob()
    {
        var job = await _jobService.GetJobAsync(x => x.Town == "Poznań");
        Assert.NotNull(job);
    }

    [Fact]
    public async void GetJobsAsync_ShouldReturnJobs()
    {
        var jobs = await _jobService.GetJobsAsync();

        Assert.NotNull(jobs);
        Assert.NotEmpty(jobs);
    }

    [Fact]
    public async void UpdateJobAsync_ShouldUpdateJob()
    {
        var job = await DbContext.Jobs.FirstOrDefaultAsync(x => x.Town == "Krakow");
        job!.DateOfExecution = DateTime.Now.AddDays(3);
        var UpdateJobVm = _mapper.Map<UpdateJobVm>(job);

        DbContext.ChangeTracker.Clear();
        await _jobService.UpdateJobAsync(UpdateJobVm);
        var updatedJob = await DbContext.Jobs.FirstOrDefaultAsync(j => j.Town == "Krakow");

        Assert.NotNull(updatedJob);
        Assert.Equal(job.DateOfExecution, updatedJob.DateOfExecution);
    }

    [Fact]
    public async void DeleteJobAsync_ShouldRemoveJob_WhenJobExists()
    {
        var job = await _jobService.GetJobAsync(x => x.UserId == 1);

        await _jobService.DeleteJobAsync(job);
        var deletedJob = await DbContext.Jobs.FirstOrDefaultAsync(j => j.Id == job.Id);

        Assert.Null(deletedJob);
    }
}
