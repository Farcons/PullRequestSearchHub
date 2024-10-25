using PipelinesTeste2.DBContexts.SystemCollections;
using PipelinesTeste2.DBContexts.SystemCollections.Collections.PipelinesTeste2.DBContexts.SystemCollections.Collections;
using PipelinesTeste2.DBContexts.SystemCollections.Collections.Views;

namespace PipelinesTeste2.MicrosoftDevops.Conecting.Fabrics
{
    public class ConnectionFabric : IDisposable
    {
        private bool _disposed = false;

        private Dictionary<Guid, List<IConnect>> _connections { get; set; } = [];

        public void SetConnections(List<SystemUserCollection> collectionsInUse, Guid userId)
        {
            var connect = new List<IConnect>();

            var collectionIds = new HashSet<Guid>(collectionsInUse.Select(q => q.CollectionId));
            var systemCollections = SystemCollectionInUse.Collections.Where(p => collectionIds.Contains(p.Id)).ToList();

            systemCollections.ForEach(c =>
            {
                switch (c.Name)
                {
                    case "Serviços":
                        connect.Add(new ServConnectServicos());
                        break;
                    case "Comércio":
                        connect.Add(new ServConnectComercio());
                        break;
                    default:
                        throw new Exception($"Collection {c.Name} não configurada no sistema");
                }
            });

            _connections[userId] = connect;
        }

        public void Authenticate(Guid userId)
        {
            ConnectionsForUser(userId).ForEach(c =>
            {
                c.Connect(userId);
            });
        }

        public List<CollectionView> Search(Guid userId)
        {
            var ret = new List<CollectionView>();

            ConnectionsForUser(userId).ForEach(c =>
            {
                ret.Add(c.List());
            });

            return ret;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            _connections = null;

            _disposed = true;
        }

        private List<IConnect> ConnectionsForUser(Guid userId)
        {
            return _connections.FirstOrDefault(p => p.Key == userId).Value;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
