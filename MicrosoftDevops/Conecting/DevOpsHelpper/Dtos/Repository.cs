namespace PipelineSearchHub.MicrosoftDevops.Conecting.DevOpsHelpper.Dtos
{
    public class Repository
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public Project Project { get; set; }
        public string DefaultBranch { get; set; }
        public int Size { get; set; }
        public string RemoteUrl { get; set; }
        public string SshUrl { get; set; }
        public string WebUrl { get; set; }
    }
}
