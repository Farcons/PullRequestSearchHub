using PipelinesTeste2.DBContexts.Base;

namespace PipelinesTeste2.DBContexts.SystemUsers
{
    public interface IRepSystemUser : IBaseDbContext<SystemUser>
    {
        SystemUser CreateWithUsername(string username);
        SystemUser FindWithUsername(string username);
    }
}
