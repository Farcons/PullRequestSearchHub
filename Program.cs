using PipelinesTeste2.DBContexts;
using PipelinesTeste2.DBContexts.SystemCollections;
using PipelinesTeste2.DBContexts.SystemCollections.Collections.Views;
using PipelinesTeste2.DBContexts.SystemCollections.Views;
using PipelinesTeste2.MicrosoftDevops.Conecting.Fabrics;
using PipelinesTeste2.MicrosoftDevops.LogginBase;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddMemoryCache();
        builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
        new DBBuilderBase().ModelCreating(builder);
        builder.Services.AddScoped<IServLoggin, ServLoggin>();
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAll",
                policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
        });
        var app = builder.Build();
        app.UseCors("AllowAll");
        var factory = new ConnectionFabric();

        app.MapGet("/loggin", async (IServLoggin _servLoggin, IRepSystemUserCollection _repSystemUserCollection, HttpRequest request) =>
        {
            if (!request.Headers.TryGetValue("username", out var username) ||
                !request.Headers.TryGetValue("password", out var password) ||
                username.ToString().Trim() == string.Empty || password.ToString().Trim() == string.Empty)
            {
                return Results.BadRequest("Usuario e senha são necessários e não podem estar vazios.");
            }

            try
            {
                var userId = _servLoggin.LogginBase(username, password);
                var collections = _repSystemUserCollection.CollectionInUse(userId);
                factory.SetConnections(collections, userId);

                return Results.Ok(new { UserId = userId });
            }
            catch (Exception ex)
            {
                return Results.Problem($"Erro ao fazer login: {ex.Message}");
            }
        });

        app.MapGet("/pullrequests", (HttpRequest request) =>
        {
            if (!request.Headers.TryGetValue("userId", out var userId))
                return Results.BadRequest("Código do usuario não encontrado no cabeçalho da requisição.");

            try
            {
                List<CollectionView> ret = [];

                ret = factory.Search(new Guid(userId));

                return Results.Ok(ret);
            }
            catch (Exception ex)
            {
                return Results.Problem($"Erro ao obter pull requests: {ex.Message}");
            }
        });

        app.MapGet("/collectionsForUse", (HttpRequest request, IRepSystemUserCollection _repSystemUserCollection) =>
        {
            if (!request.Headers.TryGetValue("userId", out var userId))
                return Results.BadRequest("Código do usuario não encontrado no cabeçalho da requisição.");

            try
            {
                var ret = _repSystemUserCollection.BffUserCollections(new Guid(userId));

                return Results.Ok(ret);
            }
            catch (Exception ex)
            {
                return Results.Problem($"Erro ao obter collections do usuario: {ex.Message}");
            }
        });

        app.MapPut("/collectionsForUse", (HttpRequest request, IRepSystemUserCollection _repSystemUserCollection) =>
        {
            var collectionsRequest = request.ReadFromJsonAsync<BffSystemUserCollectionView>().Result;

            if (collectionsRequest == null || !collectionsRequest.Collections.Any())
                return Results.BadRequest("Como não foi encontrado dados de atualização, nada foi efetuado.");

            try
            {
                var collections = _repSystemUserCollection.AtualizeUserCollections(collectionsRequest.Collections);

                if (collections.Count != 0)
                {
                    var userId = collections.FirstOrDefault().UserId;
                    factory.SetConnections(_repSystemUserCollection.CollectionInUse(userId), userId);
                    factory.Authenticate(userId);
                }

                return Results.Ok();
            }
            catch (Exception ex)
            {
                return Results.Problem($"Erro ao atualizar collections do usuario: {ex.Message}");
            }
        });

        app.MapPost("/atualizeConnections", (HttpRequest request, IRepSystemUserCollection _repSystemUserCollection) =>
        {
            if (!request.Headers.TryGetValue("userId", out var userId))
                return Results.BadRequest("Código do usuario não encontrado no cabeçalho da requisição.");

            try
            {
                factory.SetConnections(_repSystemUserCollection.CollectionInUse(new Guid(userId)), new Guid(userId));
                factory.Authenticate(new Guid(userId));

                var ret = factory.Search(new Guid(userId));
                return Results.Ok(ret);
            }
            catch (Exception ex)
            {
                return Results.Problem($"Erro ao atualizar collections do usuario: {ex.Message}");
            }
        });

        app.Run();
    }
}