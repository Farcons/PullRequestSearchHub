using Microsoft.TeamFoundation.Core.WebApi;
using Microsoft.TeamFoundation.SourceControl.WebApi;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.WebApi;
using PipelinesTeste2.DBContexts.SystemCollections.Collections.Views;
using PipelinesTeste2.MicrosoftDevops.LogginBase;
using PipelinesTeste2.MicrosoftDevops.PullRequests;

namespace PipelinesTeste2.MicrosoftDevops.Conecting
{
    public class ServConnectServicos : IConnect
    {
        private GitHttpClient gitClient;
        private ProjectHttpClient projectClient;
        private List<GitRepository> repositoryList;
        private IPagedList<TeamProjectReference> projects;
        private readonly string _urlBase = $"{LoggedUsers.BaseUrl}/Serviços";

        private readonly GitPullRequestSearchCriteria searchCriteria = new() { Status = PullRequestStatus.Active, TargetRefName = "refs/heads/Teste" };

        public void Connect(Guid userId)
        {
            var credentials = LoggedUsers.FindByUserId(userId);

            try
            {
                var connection = new VssConnection(new Uri(_urlBase), credentials);

                gitClient = connection.GetClient<GitHttpClient>();
                projectClient = connection.GetClient<ProjectHttpClient>();

                projects = projectClient.GetProjects().Result;
                repositoryList = gitClient.GetRepositoriesAsync().Result;
            }
            catch (VssUnauthorizedException)
            {
                throw new UnauthorizedAccessException("Erro de autenticação! As credenciais utilizadas não tem permissão para o DevOps do sistema de Serviços.");
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Erro ao conectar: " + ex.Message, ex);
            }
        }

        public CollectionView List()
        {
            CollectionView collection = new()
            {
                Id = new Guid("bc03cd0c-9641-4847-9160-3b30f56e69dd"),
                Name = "Serviços",
                Projects = []
            };

            foreach (var project in projects)
            {
                var pullRequest = gitClient.GetPullRequestsByProjectAsync(project.Id, searchCriteria).Result;

                if (pullRequest.Count == 0)
                    continue;

                collection.Projects.Add(new ProjectView
                {
                    Id = project.Id,
                    Name = project.Name,
                    PullRequests = pullRequest.Where(pr => pr.Reviewers.Any(reviewer => reviewer.Vote != 10))
                                  .Select(pr => new PullRequestView
                                  {
                                      Id = pr.PullRequestId,
                                      Name = pr.Title,
                                      OwnerName = pr.CreatedBy.DisplayName,
                                      RepositoryName = pr.Repository.Name,
                                      Url = RemakeUrl(pr.Url)
                                  }).ToList()
                });
            };

            return collection;
        }

        private string RemakeUrl(string url)
        {
            var urlParts = url.Split('/');
            var newUrl = string.Empty;

            string projectGuid = urlParts[4];
            string repoGuid = urlParts[8];
            string pullRequestId = urlParts.Last();

            var project = projects.FirstOrDefault(t => t.Id.ToString().Equals(projectGuid, StringComparison.OrdinalIgnoreCase));

            var repository = repositoryList.FirstOrDefault(r => r.Id.ToString().Equals(repoGuid, StringComparison.OrdinalIgnoreCase));

            if (project != null && repository != null)
            {
                newUrl = $"{_urlBase}/{project.Name}/_git/{repository.Name}/pullrequest/{pullRequestId}";
            }
            else
            {
                throw new Exception("Projeto ou repositório não encontrado.");
            }

            return newUrl;
        }
    }
}