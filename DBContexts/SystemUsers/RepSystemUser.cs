using PipelineSearchHub.DBContexts.Base;

namespace PipelineSearchHub.DBContexts.SystemUsers
{
    public class RepSystemUser(AppDbContext dbContext) : BaseDbContext<SystemUser>(dbContext), IRepSystemUser
    {
        public SystemUser FindWithUsername(string username) => FirstOrDefault(p => p.Username == username);

        public SystemUser CreateWithUsername(string username)
        {
            var ret = new SystemUser
            {
                Id = Guid.NewGuid(),
                Username = username,
                LoginDate = DateTime.Now
            };

            _dbContext.Add(ret);

            SaveChanges();

            return ret;
        }
    }
}