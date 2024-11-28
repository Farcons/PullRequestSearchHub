using PipelineSearchHub.DBContexts.SystemCollections.Collections.Views;

namespace PipelineSearchHub.MicrosoftDevops.Conecting
{
    public interface IConnect
    {
        void Connect(Guid userId);
        CollectionView List(Guid userId);
    }
}