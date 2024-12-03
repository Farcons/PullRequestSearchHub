using PipelineSearchHub.DBContexts.Base;
using PipelineSearchHub.DBContexts.SystemCollections.Collections.PipelineSearchHub.DBContexts.SystemCollections.Collections;
using PipelineSearchHub.DBContexts.SystemCollections.Views;

namespace PipelineSearchHub.DBContexts.SystemCollections
{
    public class RepSystemUserCollection(AppDbContext dbContext) : BaseDbContext<SystemUserCollection>(dbContext), IRepSystemUserCollection
    {
        public List<SystemUserCollection> CollectionInUse(Guid userId) => [.. Where(p => p.InUse && p.UserId == userId)];

        public List<SystemUserCollection> UserCollections(Guid userId) => [.. Where(p => p.UserId == userId)];
        
        public List<SystemUserCollectionView> BffUserCollections(Guid userId)
        {
            var collections = UserCollections(userId);

            var ret = (from ucl in collections
                       join cl in SystemCollectionInUse.Collections on ucl.CollectionId equals cl.Id
                       select new SystemUserCollectionView
                       {
                           Id = ucl.Id,
                           InUse = ucl.InUse,
                           Name = cl.Name
                       }).ToList();

            return ret;
        }

        public List<SystemUserCollection> AtualizeUserCollections(List<SystemUserCollectionView> collections)
        {
            var coll = Find(collections.Select(p => p.Id));

            coll.ForEach(c =>
            {
                var atualCollection = collections.FirstOrDefault(p => p.Id == c.Id);

                if (atualCollection == null)
                    return;

                c.InUse = atualCollection.InUse;
            });

            SaveChanges();

            return coll;
        }

        public void AtualizeCollecionsForUser(Guid userId)
        {
            if (userId == Guid.Empty)
                throw new ArgumentException("Erro400 Código de usuario deve ser informado", nameof(userId));

            var userCollections = UserCollections(userId)
                .Select(q => q.CollectionId)
                .ToHashSet();

            var neededCollections = SystemCollectionInUse.Collections
                .Where(p => !userCollections.Contains(p.Id))
                .Select(collection => new SystemUserCollection
                {
                    CollectionId = collection.Id,
                    Id = Guid.NewGuid(),
                    UserId = userId
                })
                .ToList();

            AddRange(neededCollections);
            SaveChanges();
        }
    }
}