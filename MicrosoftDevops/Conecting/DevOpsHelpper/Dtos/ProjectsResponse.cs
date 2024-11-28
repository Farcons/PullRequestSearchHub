namespace PipelineSearchHub.MicrosoftDevops.Conecting.DevOpsHelpper.Dtos
{
    public class DevopsResponse<T>
    {
        public int Count { get; set; }
        public List<T> Value { get; set; } = [];

    }
}
