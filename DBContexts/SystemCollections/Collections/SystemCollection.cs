using PipelineSearchHub.DBContexts.Base;

namespace PipelineSearchHub.DBContexts.SystemCollections.Collections
{
    public class SystemCollection : SystemBaseIdentifier
    {
        public SystemCollection(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        public string Name { get; set; }
    }
}