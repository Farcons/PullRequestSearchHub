namespace PipelineSearchHub.MicrosoftDevops.PullRequests
{
    public class ProjectView
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<PullRequestView> PullRequests { get; set; }
        public int PendingQuant { get => PullRequests.Count; }
    }
}