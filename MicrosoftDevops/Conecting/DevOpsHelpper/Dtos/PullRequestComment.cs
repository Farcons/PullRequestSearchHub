namespace PipelineSearchHub.MicrosoftDevops.Conecting.DevOpsHelpper.Dtos
{
    public class PullRequestComment
    {
        public int Id { get; set; }
        public List<Comment> Comments { get; set; }
        public string Status { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class Comment
    {
        public int Id { get; set; }
        public int ParentCommentId { get; set; }
        public string Content { get; set; }
        public string CommentType { get; set; }
    }
}
