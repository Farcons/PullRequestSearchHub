using PipelinesTeste2.DBContexts.Base;

namespace PipelinesTeste2.DBContexts.SystemCollections
{
    public class SystemUserCollection : SystemBsaeUserIdentifier
    {
        public bool InUse { get; set; } = false;
        public required Guid CollectionId { get; set; }
    }
}