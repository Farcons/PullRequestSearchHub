using Microsoft.VisualStudio.Services.Common;
using PipelinesTeste2.DBContexts.SystemCollections;
using PipelinesTeste2.DBContexts.SystemUsers;

namespace PipelinesTeste2.MicrosoftDevops.LogginBase
{
    public class ServLoggin(IRepSystemUser repSystemUser, IRepSystemUserCollection repSystemUserCollection) : IServLoggin
    {
        private readonly IRepSystemUser _repSystemUser = repSystemUser;
        private readonly IRepSystemUserCollection _repSystemUserCollection  = repSystemUserCollection;

        public Guid LogginBase(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Usuario e senha devem ser informados.");

            var credentials = new VssBasicCredential(username, password);

            var user = _repSystemUser.FindWithUsername(username) ?? _repSystemUser.CreateWithUsername(username);
            _repSystemUserCollection.AtualizeCollecionsForUser(user.Id);

            LoggedUsers.RegisteredUsers[user.Id] = credentials;

            return user.Id;
        }

        public VssBasicCredential Autenticate(Guid userId)
        {
            var connection = LoggedUsers.FindByUserId(userId)
                ?? throw new InvalidOperationException($"Erro de autenticação! Faça loggin no sistema novamete");

            return connection;
        }
    }
}