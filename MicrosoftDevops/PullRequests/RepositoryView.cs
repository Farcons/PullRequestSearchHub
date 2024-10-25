namespace PipelinesTeste2.MicrosoftDevops.PullRequests
{
    public class RepositoryView
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<PullRequestView> PullRequests { get; set; }
    }
}
