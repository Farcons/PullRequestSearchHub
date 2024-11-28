namespace PipelineSearchHub.DBContexts.SystemCollections.Collections
{
    namespace PipelineSearchHub.DBContexts.SystemCollections.Collections
    {
        public static class SystemCollectionInUse
        {
            public static IReadOnlyList<SystemCollection> Collections => _collections.AsReadOnly();

            private static readonly List<SystemCollection> _collections =
            [
                new SystemCollection(new Guid("bc03cd0c-9641-4847-9160-3b30f56e69dd"), "Serviços"),
                new SystemCollection(new Guid("3c66cd3b-dab4-46a5-9dea-218ef7dcf207"), "Comércio"),
                new SystemCollection(new Guid("15861aa9-87c8-45db-87dc-257db789e014"), "M2"),
                new SystemCollection(new Guid("8b96b3eb-2971-48a7-876b-4dd8b657e498"), "PCP M2")
            ];

        }

    }
}