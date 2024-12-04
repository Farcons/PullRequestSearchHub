using PipelineSearchHub.DBContexts.SystemCollections.Collections.Views;

namespace PipelineSearchHub.MicrosoftDevops.Conecting
{
    public interface IConnect
    {
        void Connect(Guid userId);
        Task<CollectionView> ListAsync(Guid userId);
    }
}