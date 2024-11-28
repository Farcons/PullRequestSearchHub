namespace PipelineSearchHub.MicrosoftDevops.Conecting.DevOpsHelpper.Dtos
{
    public class PullRequest
    {
        public Repository Repository { get; set; }
        public int PullRequestId { get; set; }
        public int CodeReviewId { get; set; }
        public string Status { get; set; }
        public CreatedBy CreatedBy { get; set; }
        public DateTime CreationDate { get; set; }
        public string Title { get; set; }
        public string SourceRefName { get; set; }
        public string TargetRefName { get; set; }
        public string MergeStatus { get; set; }
        public bool IsDraft { get; set; }
        public string MergeId { get; set; }
        public List<Reviewer> Reviewers { get; set; }
        public string Url { get; set; }
        public bool SupportsIterations { get; set; }
    }

    public class CreatedBy
    {
        public string DisplayName { get; set; }
        public Guid Id { get; set; }
    }

    public class Reviewer
    {
        public int Vote { get; set; }
        public bool HasDeclined { get; set; }
        public bool IsRequired { get; set; }
        public bool IsFlagged { get; set; }
        public string DisplayName { get; set; }
        public Guid Id { get; set; }
        public bool IsContainer { get; set; }
    }
}