using Microsoft.VisualStudio.Services.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PipelineSearchHub.MicrosoftDevops.Conecting.DevOpsHelpper.Dtos;
using PipelineSearchHub.MicrosoftDevops.LogginBase;
using System.Text;

namespace PipelineSearchHub.MicrosoftDevops.Conecting.DevOpsHelpper
{
    public sealed class ServDevOpsHellper
    {
        private static readonly Lazy<ServDevOpsHellper> lazy = new Lazy<ServDevOpsHellper> (() => new ServDevOpsHellper());
        public static ServDevOpsHellper Instance => lazy.Value;

        private readonly string baseUrl = "https://devops.useall.com.br";

        public async Task<List<Repository>> Repositories(string collectionName, Guid projectId, Guid userId)
        {
            var authToken = LoggedUsers.FindByUserId(userId);
            var apiUrl = $"{baseUrl}/{collectionName}/{projectId}/_apis/git/repositories?api-version=6.1-preview.1";

            try
            {
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
                    throw new Exception($"Erro ao tentar recuperar os repositórios do sistema. Erro: {response.ReasonPhrase}");
                }
            }
            catch (VssUnauthorizedException)
            {
                throw new UnauthorizedAccessException("Erro de autenticação! As credenciais utilizadas não tem permissão para o DevOps.");
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<List<Project>> Projects(string collectionName, Guid userId)
        {
            var authToken = LoggedUsers.FindByUserId(userId);
            var apiUrl = $"{baseUrl}/{collectionName}/_apis/projects?api-version=6.1-preview.1";

            try
            {
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
                    throw new Exception($"Erro ao tentar recuperar os projetos do sistema. Erro: {response.ReasonPhrase}");
                }
            }
            catch (VssUnauthorizedException)
            {
                throw new UnauthorizedAccessException("Erro de autenticação! As credenciais utilizadas não tem permissão para o DevOps.");
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<List<PullRequest>> PullRequests(string collectionName, Guid projectId, Guid userId)
        {
            var authToken = LoggedUsers.FindByUserId(userId);
            var apiUrl = $"{baseUrl}/{collectionName}/{projectId}/_apis/git/pullrequests?api-version=6.1-preview.1&targetRefName=refs/heads/Teste&status=active";

            try
            {
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
                    throw new Exception($"Erro ao tentar recuperar as pull requests do sistema. Erro: {response.ReasonPhrase}");
                }
            }
            catch (VssUnauthorizedException)
            {
                throw new UnauthorizedAccessException("Erro de autenticação! As credenciais utilizadas não tem permissão para o DevOps.");
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
