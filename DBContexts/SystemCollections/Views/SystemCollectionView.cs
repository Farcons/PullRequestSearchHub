namespace PipelinesTeste2.DBContexts.SystemCollections.Views
{
    public class SystemUserCollectionView
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool InUse { get; set; }
    }

    public class BffSystemUserCollectionView
    {
        public List<SystemUserCollectionView> Collections { get; set; }
    }
}