using PipelineSearchHub.MicrosoftDevops.Conecting.DevOpsHelpper;
using PipelineSearchHub.MicrosoftDevops.Conecting.DevOpsHelpper.Dtos;
using PipelineSearchHub.DBContexts.SystemCollections.Collections.Views;
using PipelineSearchHub.MicrosoftDevops.PullRequests;

namespace PipelineSearchHub.MicrosoftDevops.Conecting
{
    public class ServConnectServicos() : IConnect
    {
        #region Ctor
        private readonly string collectionName = "Serviços";
        private List<Project> projects = [];
        private readonly string _baseUrl = "https://devops.useall.com.br";
        #endregion

        public void Connect(Guid userId)
        {
            projects = ServDevOpsHellper.Instance.Projects(collectionName, userId).Result;
        }

        public async Task<CollectionView> ListAsync(Guid userId)
        {
            CollectionView collection = new()
            {
                Id = new Guid("bc03cd0c-9641-4847-9160-3b30f56e69dd"),
                Name = "Serviços",
                Projects = []
            };

            var tasks = projects.Select(async project =>
            {
                var pullRequests = await ServDevOpsHellper.Instance.PullRequests(collectionName, project.Id, userId);
                pullRequests = pullRequests.Where(pr => pr.Reviewers.Any(reviewer => reviewer.Vote != 10)).ToList();

                if (pullRequests.Count > 0)
                {
                    var projectView = new ProjectView
                    {
                        Id = project.Id,
                        Name = project.Name,
                        PullRequests = []
                    };

                    var pullRequestTasks = pullRequests.Select(async pr =>
                    {
                        var comments = await ServDevOpsHellper.Instance.PullRequestComments(collectionName, pr.Repository.Id, pr.PullRequestId, userId);
                        var quantComments = comments.Where(p => !p.IsDeleted &&
                                                                p.Status == "active" &&
                                                                p.Comments.Any(q => q.CommentType == "text"))
                                                    .Count();


                        return new PullRequestView
                        {
                            Id = pr.PullRequestId,
                            Name = pr.Title,
                            CreationDate = pr.CreationDate,
                            OwnerName = pr.CreatedBy.DisplayName,
                            RepositoryName = pr.Repository.Name,
                            Url = RemakeUrl(pr),
                            QuantComents = quantComments
                        };
                    });

                    projectView.PullRequests = [.. (await Task.WhenAll(pullRequestTasks))];
                    return projectView;
                }

                return null;
            });

            var projectViews = (await Task.WhenAll(tasks)).Where(p => p != null).ToList();
            collection.Projects.AddRange(projectViews);

            return collection;
        }

        private string RemakeUrl(PullRequest dto)
        {
            var urlParts = dto.Url.Split('/');
            string projectGuid = urlParts[4];
            string repoGuid = urlParts[8];
            string pullRequestId = urlParts.Last();

            var project = projects.FirstOrDefault(t => t.Id.ToString().Equals(projectGuid, StringComparison.OrdinalIgnoreCase));

            if (project != null && dto.Repository != null)
            {
                return $"{_baseUrl}/{collectionName}/{project.Name}/_git/{dto.Repository.Name}/pullrequest/{pullRequestId}";
            }
            return "Url não encontrada";
        }


        //Pesou de mais
        private bool WiAvaliableForTest(PullRequest pr, Guid userId)
        {
            var ret = true;

            List<WorkItemResponse> linkedWis = ServDevOpsHellper.Instance.PullRequestWis(collectionName, pr.Repository.Id, pr.PullRequestId, userId).Result;

            if (linkedWis == null || linkedWis.Count == 0)
                return false;

            foreach (var wi in linkedWis)
            {
                List<WorkItemDetails> workItemDetails = ServDevOpsHellper.Instance.WorkItemDetails(collectionName, wi.Id, userId).Result;

                if (!workItemDetails.Any(p => p.Fields.SystemState == "Avaliable for test"))
                {
                    ret = false;
                    break;
                }
            }
            return ret;
        }
    }
}