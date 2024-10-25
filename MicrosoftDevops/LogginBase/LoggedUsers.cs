using Microsoft.TeamFoundation.Core.WebApi;
using Microsoft.TeamFoundation.SourceControl.WebApi;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.WebApi;

namespace PipelinesTeste2.MicrosoftDevops.LogginBase
{
    public static class LoggedUsers
    {
        public static string BaseUrl = "https://devops.useall.com.br";
        public static Dictionary<Guid, VssBasicCredential> RegisteredUsers { get; set; } = [];

        public static VssBasicCredential FindByUserId(Guid userId) 
        {
            var ret = RegisteredUsers.FirstOrDefault(p => p.Key == userId).Value;

            return ret ?? throw new UnauthorizedAccessException("Erro de autenticação! Não foi identificado credenciais. Faça o Loggin novamente!");
        }

    }
    public class UserConnections
    {
        public VssConnection Connection { get; set; }
        public GitHttpClient GitClient { get; set; }
        public ProjectHttpClient ProjectClient { get; set; }
    }
}
