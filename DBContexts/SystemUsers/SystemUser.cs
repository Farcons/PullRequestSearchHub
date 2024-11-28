using PipelineSearchHub.DBContexts.Base;

namespace PipelineSearchHub.DBContexts.SystemUsers
{
    public class SystemUser : SystemBaseIdentifier
    {
        public required string Username { get; set; }
        public DateTime LoginDate { get; set; }
    }
}