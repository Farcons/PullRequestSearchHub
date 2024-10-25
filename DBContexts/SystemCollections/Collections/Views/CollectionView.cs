using PipelinesTeste2.MicrosoftDevops.PullRequests;

namespace PipelinesTeste2.DBContexts.SystemCollections.Collections.Views
{
    public class CollectionView
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<ProjectView> Projects { get; set; }
        public int PendingQuant { get => Projects.Sum(p => p.PendingQuant); }
    }
}