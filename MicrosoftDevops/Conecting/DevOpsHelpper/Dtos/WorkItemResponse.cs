using Newtonsoft.Json;

namespace PipelineSearchHub.MicrosoftDevops.Conecting.DevOpsHelpper.Dtos
{
    public class WorkItemResponse
    {
        public int Id { get; set; }
    }
    public class WorkItemFields
    {
        [JsonProperty("System.WorkItemType")]
        public string SystemWorkItemType { get; set; }

        [JsonProperty("System.State")]
        public string SystemState { get; set; }
    }

    public class WorkItemDetails
    {
        public int Id { get; set; }
        public int Rev { get; set; }
        public WorkItemFields Fields { get; set; }
    }
}
