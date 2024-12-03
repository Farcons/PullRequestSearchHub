using PipelineSearchHub.MicrosoftDevops.Conecting.DevOpsHelpper;
using PipelineSearchHub.MicrosoftDevops.Conecting.DevOpsHelpper.Dtos;
using PipelineSearchHub.DBContexts.SystemCollections.Collections.Views;
using PipelineSearchHub.MicrosoftDevops.PullRequests;

namespace PipelineSearchHub.MicrosoftDevops.Conecting
{
    public class ServConnectM2() : IConnect
    {
        #region Ctor
        private readonly string collectionName = "M2";
        private List<Project> projects = [];
        private readonly string _baseUrl = "https://devops.useall.com.br";
        #endregion

        public void Connect(Guid userId)
        {
            projects = ServDevOpsHellper.Instance.Projects(collectionName, userId).Result;
        }

        public CollectionView List(Guid userId)
        {
            CollectionView collection = new()
            {
                Id = new Guid("15861aa9-87c8-45db-87dc-257db789e014"),
                Name = "M2",
                Projects = []
            };

            foreach (var project in projects)
            {
                var pullRequest = ServDevOpsHellper.Instance.PullRequests(collectionName, project.Id, userId).Result;
                pullRequest = pullRequest.Where(pr => pr.Reviewers.Any(reviewer => reviewer.Vote != 10)).ToList();

                if (pullRequest.Count == 0)
                    continue;

                collection.Projects.Add(new ProjectView
                {
                    Id = project.Id,
                    Name = project.Name,
                    PullRequests = pullRequest.Select(pr => new PullRequestView
                    {
                        Id = pr.PullRequestId,
                        Name = pr.Title,
                        CreationDate = pr.CreationDate,
                        OwnerName = pr.CreatedBy.DisplayName,
                        RepositoryName = pr.Repository.Name,
                        Url = RemakeUrl(pr)
                    }).ToList()
                });
            };

            return collection;
        }

        private string RemakeUrl(PullRequest dto)
        {
            var urlParts = dto.Url.Split('/');
            var newUrl = string.Empty;

            string projectGuid = urlParts[4];
            string repoGuid = urlParts[8];
            string pullRequestId = urlParts.Last();

            var project = projects.FirstOrDefault(t => t.Id.ToString().Equals(projectGuid, StringComparison.OrdinalIgnoreCase));

            if (project != null && dto.Repository != null)
            {
                newUrl = $"{_baseUrl}/{collectionName}/{project.Name}/_git/{dto.Repository.Name}/pullrequest/{pullRequestId}";
            }
            else
            {
                newUrl = "Url não encontrada";
            }

            return newUrl;
        }
    }
}