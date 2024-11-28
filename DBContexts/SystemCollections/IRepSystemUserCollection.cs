
using PipelineSearchHub.DBContexts.Base;
using PipelineSearchHub.DBContexts.SystemCollections.Views;

namespace PipelineSearchHub.DBContexts.SystemCollections
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