using Microsoft.VisualStudio.Services.Common;
using PipelinesTeste2.DBContexts.SystemCollections.Collections.Views;

namespace PipelinesTeste2.MicrosoftDevops.Conecting
{
    public interface IConnect
    {
        void Connect(Guid userId);
        CollectionView List();
    }
}