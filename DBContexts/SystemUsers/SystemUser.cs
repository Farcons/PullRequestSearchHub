using PipelinesTeste2.DBContexts.Base;

namespace PipelinesTeste2.DBContexts.SystemUsers
{
    public class SystemUser : SystemBaseIdentifier
    {
        public required string Username { get; set; }
        public DateTime LoginDate { get; set; }
    }
}