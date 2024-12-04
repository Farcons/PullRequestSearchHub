using Newtonsoft.Json;
using PipelineSearchHub.MicrosoftDevops.Conecting.DevOpsHelpper.Dtos;
using PipelineSearchHub.MicrosoftDevops.LogginBase;
using System.Net;
using System.Text;

namespace PipelineSearchHub.MicrosoftDevops.Conecting.DevOpsHelpper
{
    public sealed class ServDevOpsHellper
    {
        private static readonly Lazy<ServDevOpsHellper> lazy = new Lazy<ServDevOpsHellper>(() => new ServDevOpsHellper());
        public static ServDevOpsHellper Instance => lazy.Value;

        private readonly string baseUrl = "https://devops.useall.com.br";

        public async Task<List<Repository>> Repositories(string collectionName, Guid projectId, Guid userId)
        {
            var authToken = LoggedUsers.FindByUserId(userId);
            var apiUrl = $"{baseUrl}/{collectionName}/{projectId}/_apis/git/repositories?api-version=6.1-preview.1";

            using HttpClient client = new() { BaseAddress = new Uri("https://devops.useall.com.br/") };

            var encodedToken = Convert.ToBase64String(Encoding.UTF8.GetBytes($"\"\":{authToken}"));
            client.DefaultRequestHeaders.Add("Authorization", $"Basic {encodedToken}");

            HttpResponseMessage response = await client.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();

                var repositories = JsonConvert.DeserializeObject<DevopsResponse<Repository>>(jsonResponse);
                return repositories?.Value ?? [];
            }
            else
            {
                throw response.StatusCode switch
                {
                    HttpStatusCode.BadRequest => new ArgumentException($"Erro400 Requisição inválida ao tentar recuperar os repositórios do sistema. {Environment.NewLine}Verifique os parâmetros enviados."),
                    HttpStatusCode.Unauthorized => new UnauthorizedAccessException($"Erro401 Acesso não autorizado ao tentar recuperar os repositórios do sistema. {Environment.NewLine}Verifique suas credenciais."),
                    HttpStatusCode.Forbidden => new UnauthorizedAccessException($"Erro403 Permissão negada para acessar os repositórios do sistema."),
                    HttpStatusCode.NotFound => new KeyNotFoundException($"Erro404 Endereço de acesso aos repositórios não foi encontrado."),
                    HttpStatusCode.InternalServerError => new InvalidOperationException("Erro500 Erro interno no servidor. Tente novamente mais tarde."),
                    _ => new Exception($"Erro500 Erro ao tentar recuperar os repositórios do sistema. Erro: {response.ReasonPhrase} (StatusCode: {(int)response.StatusCode})"),
                };
            }
        }

        public async Task<List<Project>> Projects(string collectionName, Guid userId)
        {
            var authToken = LoggedUsers.FindByUserId(userId);
            var apiUrl = $"{baseUrl}/{collectionName}/_apis/projects?api-version=6.1-preview.1";

            using HttpClient client = new() { BaseAddress = new Uri("https://devops.useall.com.br/") };
            var encodedToken = Convert.ToBase64String(Encoding.UTF8.GetBytes($"\"\":{authToken}"));
            client.DefaultRequestHeaders.Add("Authorization", $"Basic {encodedToken}");

            HttpResponseMessage response = await client.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();

                var projects = JsonConvert.DeserializeObject<DevopsResponse<Project>>(jsonResponse);
                return projects?.Value ?? [];
            }
            else
            {
                throw response.StatusCode switch
                {
                    HttpStatusCode.BadRequest => new ArgumentException($"Erro400 Requisição inválida ao tentar recuperar os projetos do sistema. {Environment.NewLine}Verifique os parâmetros enviados."),
                    HttpStatusCode.Unauthorized => new UnauthorizedAccessException($"Erro401 Acesso não autorizado ao tentar recuperar os projetos do sistema. {Environment.NewLine}Verifique suas credenciais."),
                    HttpStatusCode.Forbidden => new UnauthorizedAccessException($"Erro403 Permissão negada para acessar os projetos do sistema."),
                    HttpStatusCode.NotFound => new KeyNotFoundException($"Erro404 Endereço de acesso aos projetos não foi encontrado."),
                    HttpStatusCode.InternalServerError => new InvalidOperationException("Erro500 Erro interno no servidor. Tente novamente mais tarde."),
                    _ => new Exception($"Erro500 Erro ao tentar recuperar os projetos do sistema. Erro: {response.ReasonPhrase} (StatusCode: {(int)response.StatusCode})"),
                };
            }
        }

        public async Task<List<PullRequest>> PullRequests(string collectionName, Guid projectId, Guid userId)
        {
            var authToken = LoggedUsers.FindByUserId(userId);
            var apiUrl = $"{baseUrl}/{collectionName}/{projectId}/_apis/git/pullrequests?api-version=6.1-preview.1&targetRefName=refs/heads/Teste&status=active";

            using HttpClient client = new() { BaseAddress = new Uri("https://devops.useall.com.br/") };

            var encodedToken = Convert.ToBase64String(Encoding.UTF8.GetBytes($"\"\":{authToken}"));
            client.DefaultRequestHeaders.Add("Authorization", $"Basic {encodedToken}");

            HttpResponseMessage response = await client.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();

                var pullRequests = JsonConvert.DeserializeObject<DevopsResponse<PullRequest>>(jsonResponse);
                return pullRequests?.Value ?? [];
            }
            else
            {
                throw response.StatusCode switch
                {
                    HttpStatusCode.BadRequest => new ArgumentException($"Erro400 Requisição inválida ao tentar recuperar as pull request's do sistema. {Environment.NewLine}Verifique os parâmetros enviados."),
                    HttpStatusCode.Unauthorized => new UnauthorizedAccessException($"Erro401 Acesso não autorizado ao tentar recuperar as pull request's do sistema. {Environment.NewLine}Verifique suas credenciais."),
                    HttpStatusCode.Forbidden => new UnauthorizedAccessException($"Erro403 Permissão negada para acessar as pull request's do sistema."),
                    HttpStatusCode.NotFound => new KeyNotFoundException($"Erro404 Endereço de acesso aas pull request's não foi encontrado."),
                    HttpStatusCode.InternalServerError => new InvalidOperationException("Erro500 Erro interno no servidor. Tente novamente mais tarde."),
                    _ => new Exception($"Erro500 Erro ao tentar recuperar as pull request's do sistema. Erro: {response.ReasonPhrase} (StatusCode: {(int)response.StatusCode})"),
                };
            }
        }

        public async Task<List<PullRequestComment>> PullRequestComments(string collectionName, Guid repositoryId, int pullRequestId, Guid userId)
        {
            var authToken = LoggedUsers.FindByUserId(userId);
            var apiUrl = $"{baseUrl}/{collectionName}/_apis/git/repositories/{repositoryId}/pullRequests/{pullRequestId}/threads?api-version=6.1-preview.1";

            using HttpClient client = new() { BaseAddress = new Uri("https://devops.useall.com.br/") };

            var encodedToken = Convert.ToBase64String(Encoding.UTF8.GetBytes($"\"\":{authToken}"));
            client.DefaultRequestHeaders.Add("Authorization", $"Basic {encodedToken}");

            HttpResponseMessage response = await client.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();

                var pullRequestComments = JsonConvert.DeserializeObject<DevopsResponse<PullRequestComment>>(jsonResponse);
                return pullRequestComments?.Value ?? [];
            }
            else
            {
                throw response.StatusCode switch
                {
                    HttpStatusCode.BadRequest => new ArgumentException($"Erro400 Requisição inválida ao tentar recuperar os comentários da pull request's do sistema. {Environment.NewLine}Verifique os parâmetros enviados."),
                    HttpStatusCode.Unauthorized => new UnauthorizedAccessException($"Erro401 Acesso não autorizado ao tentar recuperar os comentários da pull request's do sistema. {Environment.NewLine}Verifique suas credenciais."),
                    HttpStatusCode.Forbidden => new UnauthorizedAccessException($"Erro403 Permissão negada para acessar os comentários da pull request's do sistema."),
                    HttpStatusCode.NotFound => new KeyNotFoundException($"Erro404 Endereço de acesso dos comentários da pull request's não foi encontrado."),
                    HttpStatusCode.InternalServerError => new InvalidOperationException("Erro500 Erro interno no servidor. Tente novamente mais tarde."),
                    _ => new Exception($"Erro500 Erro ao tentar recuperar os comentários da pull request's do sistema. Erro: {response.ReasonPhrase} (StatusCode: {(int)response.StatusCode})"),
                };
            }
        }

        public async Task<List<WorkItemResponse>> PullRequestWis(string collectionName, Guid repositoryId, int pullRequestId, Guid userId)
        {
            var authToken = LoggedUsers.FindByUserId(userId);
            var apiUrl = $"{baseUrl}/{collectionName}/_apis/git/repositories/{repositoryId}/pullRequests/{pullRequestId}/workitems?api-version=6.1-preview.1";

            using HttpClient client = new() { BaseAddress = new Uri("https://devops.useall.com.br/") };

            var encodedToken = Convert.ToBase64String(Encoding.UTF8.GetBytes($"\"\":{authToken}"));
            client.DefaultRequestHeaders.Add("Authorization", $"Basic {encodedToken}");

            HttpResponseMessage response = await client.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();

                var WIDetails = JsonConvert.DeserializeObject<DevopsResponse<WorkItemResponse>>(jsonResponse);
                return WIDetails?.Value ?? [];
            }
            else
            {
                throw response.StatusCode switch
                {
                    HttpStatusCode.BadRequest => new ArgumentException($"Erro400 Requisição inválida ao tentar recuperar os comentários da pull request's do sistema. {Environment.NewLine}Verifique os parâmetros enviados."),
                    HttpStatusCode.Unauthorized => new UnauthorizedAccessException($"Erro401 Acesso não autorizado ao tentar recuperar os comentários da pull request's do sistema. {Environment.NewLine}Verifique suas credenciais."),
                    HttpStatusCode.Forbidden => new UnauthorizedAccessException($"Erro403 Permissão negada para acessar os comentários da pull request's do sistema."),
                    HttpStatusCode.NotFound => new KeyNotFoundException($"Erro404 Endereço de acesso dos comentários da pull request's não foi encontrado."),
                    HttpStatusCode.InternalServerError => new InvalidOperationException("Erro500 Erro interno no servidor. Tente novamente mais tarde."),
                    _ => new Exception($"Erro500 Erro ao tentar recuperar os comentários da pull request's do sistema. Erro: {response.ReasonPhrase} (StatusCode: {(int)response.StatusCode})"),
                };
            }
        }

        public async Task<List<WorkItemDetails>> WorkItemDetails(string collectionName, int wiId, Guid userId)
        {
            var authToken = LoggedUsers.FindByUserId(userId);
            var apiUrl = $"{baseUrl}/{collectionName}/_apis/wit/workitems/{wiId}?api-version=6.0";

            using HttpClient client = new() { BaseAddress = new Uri("https://devops.useall.com.br/") };

            var encodedToken = Convert.ToBase64String(Encoding.UTF8.GetBytes($"\"\":{authToken}"));
            client.DefaultRequestHeaders.Add("Authorization", $"Basic {encodedToken}");

            HttpResponseMessage response = await client.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();

                var pullRequestComments = JsonConvert.DeserializeObject<DevopsResponse<WorkItemDetails>>(jsonResponse);
                return pullRequestComments?.Value ?? [];
            }
            else
            {
                throw response.StatusCode switch
                {
                    HttpStatusCode.BadRequest => new ArgumentException($"Erro400 Requisição inválida ao tentar recuperar os dados de Work Itens do sistema. {Environment.NewLine}Verifique os parâmetros enviados."),
                    HttpStatusCode.Unauthorized => new UnauthorizedAccessException($"Erro401 Acesso não autorizado ao tentar recuperar os dados de Work Itens do sistema. {Environment.NewLine}Verifique suas credenciais."),
                    HttpStatusCode.Forbidden => new UnauthorizedAccessException($"Erro403 Permissão negada para acessar os dados de Work Itens do sistema."),
                    HttpStatusCode.NotFound => new KeyNotFoundException($"Erro404 Endereço de acesso dos dados de Work Itens não foi encontrado."),
                    HttpStatusCode.InternalServerError => new InvalidOperationException("Erro500 Erro interno no servidor. Tente novamente mais tarde."),
                    _ => new Exception($"Erro500 Erro ao tentar recuperar os dados de Work Itens do sistema. Erro: {response.ReasonPhrase} (StatusCode: {(int)response.StatusCode})"),
                };
            }
        }
    }
}