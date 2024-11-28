using Microsoft.VisualStudio.Services.ExternalEvent;
using System;

namespace PipelineSearchHub.MicrosoftDevops.PullRequests
{
    public class PullRequestView
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string OwnerName { get; set; }
        public string Url { get; set; }
        public string RepositoryName { get; set; }
        public DateTime CreationDate { get; set; }
    }
}