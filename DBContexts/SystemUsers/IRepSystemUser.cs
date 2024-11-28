using PipelineSearchHub.DBContexts.Base;

namespace PipelineSearchHub.DBContexts.SystemUsers
{
    public interface IRepSystemUser : IBaseDbContext<SystemUser>
    {
        SystemUser CreateWithUsername(string username);
        SystemUser FindWithUsername(string username);
    }
}
