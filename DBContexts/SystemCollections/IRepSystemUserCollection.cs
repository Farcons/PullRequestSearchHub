
using PipelinesTeste2.DBContexts.Base;
using PipelinesTeste2.DBContexts.SystemCollections.Views;

namespace PipelinesTeste2.DBContexts.SystemCollections
{
    public interface IRepSystemUserCollection : IBaseDbContext<SystemUserCollection>
    {
        List<SystemUserCollection> CollectionInUse(Guid userId);
        List<SystemUserCollection> UserCollections(Guid userId);
        void AtualizeCollecionsForUser(Guid userId);
        List<SystemUserCollectionView> BffUserCollections(Guid userId);
        List<SystemUserCollection> AtualizeUserCollections(List<SystemUserCollectionView> collections);
    }
}