using PipelineSearchHub.DBContexts.SystemCollections;
using PipelineSearchHub.DBContexts.SystemCollections.Collections.PipelineSearchHub.DBContexts.SystemCollections.Collections;
using PipelineSearchHub.DBContexts.SystemCollections.Collections.Views;

namespace PipelineSearchHub.MicrosoftDevops.Conecting.Fabrics
{
    public class ConnectionFabric() : IDisposable
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
                    case "M2":
                        connect.Add(new ServConnectM2());
                        break;
                    case "PCP M2":
                        connect.Add(new ServConnectPCPM2());
                        break;
                    default:
                        throw new Exception($"Erro400 Collection {c.Name} não configurada no sistema");
                }
            });

            _connections[userId] = connect;
        }

        public void Authenticate(Guid userId)
        {
            var connections = ConnectionsForUser(userId) ??
                throw new Exception($"Erro401 Collections para uso não definidas! Atualize suas collections em 'Opções'");

            connections.ForEach(c =>
            {
                c.Connect(userId);
            });
        }

        public List<CollectionView> Search(Guid userId)
        {
            var ret = new List<CollectionView>();

            var connections = ConnectionsForUser(userId) ??
                throw new Exception($"Erro401 Collections para uso não definidas! Atualize suas collections em 'Opções'");

            foreach (var c in connections)
            {
                ret.Add(GetCollectionSynchronously(() => c.ListAsync(userId)));
            };

            return ret;
        }

        private T GetCollectionSynchronously<T>(Func<Task<T>> asyncFunc)
        {
            return asyncFunc().GetAwaiter().GetResult();
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
