using PipelineSearchHub.DBContexts.Base;

namespace PipelineSearchHub.DBContexts.SystemCollections
{
    public class SystemUserCollection : SystemBsaeUserIdentifier
    {
        public bool InUse { get; set; } = false;
        public required Guid CollectionId { get; set; }
    }
}