using AutoMapper;
using Fences.DAL.EF;
namespace Fences.Tests.UnitTests;
public abstract class BaseUnitTests
{
    protected readonly ApplicationDbContext DbContext = null!;
    protected readonly IMapper _mapper = null!;
    public BaseUnitTests(ApplicationDbContext dbContext, IMapper mapper)
    {
        DbContext = dbContext;
        _mapper = mapper;
    }
}