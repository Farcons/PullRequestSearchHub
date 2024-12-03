namespace PipelineSearchHub.MicrosoftDevops.LogginBase
{
    public static class LoggedUsers
    {
        public static Dictionary<Guid, string> RegisteredUsers { get; set; } = [];

        public static string FindByUserId(Guid userId) 
        {
            var ret = RegisteredUsers.FirstOrDefault(p => p.Key == userId).Value;

            return ret ?? throw new UnauthorizedAccessException("Erro401 Erro de autenticação! Não foi identificado credenciais. Faça o Loggin novamente!");
        }

    }
}
