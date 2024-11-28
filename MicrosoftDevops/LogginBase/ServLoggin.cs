using PipelineSearchHub.DBContexts.SystemCollections;
using PipelineSearchHub.DBContexts.SystemUsers;
using PipelineSearchHub.MicrosoftDevops.LogginBase;

namespace PipelineSearchHub.MicrosoftDevops.LogginBase
{
    public class ServLoggin(IRepSystemUser repSystemUser, IRepSystemUserCollection repSystemUserCollection) : IServLoggin
    {
        private readonly IRepSystemUser _repSystemUser = repSystemUser;
        private readonly IRepSystemUserCollection _repSystemUserCollection = repSystemUserCollection;

        public Guid LogginBase(string username, string token)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(token))
                throw new ArgumentException("Usuario e token devem ser informados.");

            var user = _repSystemUser.FindWithUsername(username) ?? _repSystemUser.CreateWithUsername(username);
            _repSystemUserCollection.AtualizeCollecionsForUser(user.Id);

            LoggedUsers.RegisteredUsers[user.Id] = token;

            return user.Id;
        }
    }
}