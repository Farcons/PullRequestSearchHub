using PipelineSearchHub.MicrosoftDevops.LogginBase;
using PipelineSearchHub.DBContexts;
using PipelineSearchHub.DBContexts.SystemCollections;
using PipelineSearchHub.DBContexts.SystemCollections.Collections.Views;
using PipelineSearchHub.DBContexts.SystemCollections.Views;
using PipelineSearchHub.MicrosoftDevops.Conecting.Fabrics;
using PipelineSearchHub.DBContexts.Base;
using Microsoft.EntityFrameworkCore;
using PipelineSearchHub.ExceptionsFilters;
using Microsoft.AspNetCore.Diagnostics;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

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

        app.UseMiddleware<GlobalExceptionHandler>();

        using (var scope = app.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            db.Database.Migrate();
        }

        app.UseCors("AllowAll");
        var factory = new ConnectionFabric();

        app.MapGet("/loggin", async (IServLoggin _servLoggin, IRepSystemUserCollection _repSystemUserCollection, HttpRequest request) =>
        {
            if (!request.Headers.TryGetValue("username", out var username) ||
                !request.Headers.TryGetValue("token", out var token) ||
                username.ToString().Trim() == string.Empty || token.ToString().Trim() == string.Empty)
            {
                return Results.BadRequest("Usuario e token são necessários e não podem estar vazios.");
            }

            var userId = _servLoggin.LogginBase(username, token);
            var collections = _repSystemUserCollection.CollectionInUse(userId);
            factory.SetConnections(collections, userId);
            factory.Authenticate(userId);

            return Results.Ok(new { UserId = userId });

        });

        app.MapGet("/pullrequests", (HttpRequest request) =>
        {
            if (!request.Headers.TryGetValue("userId", out var userId))
                return Results.BadRequest("Código do usuario não encontrado no cabeçalho da requisição.");

            List<CollectionView> ret = [];

            ret = factory.Search(new Guid(userId));

            return Results.Ok(ret);

        });

        app.MapGet("/collectionsInUse", (HttpRequest request, IRepSystemUserCollection _repSystemUserCollection) =>
        {
            if (!request.Headers.TryGetValue("userId", out var userId))
                return Results.BadRequest("Código do usuario não encontrado no cabeçalho da requisição.");

            var ret = _repSystemUserCollection.BffUserCollections(new Guid(userId));

            return Results.Ok(ret);

        });

        app.MapPost("/collectionsForUse", (HttpRequest request, IRepSystemUserCollection _repSystemUserCollection) =>
        {
            BffSystemUserCollectionView? collectionsRequest = request.ReadFromJsonAsync<BffSystemUserCollectionView>().Result;

            if (collectionsRequest == null || !collectionsRequest.Collections.Any())
                return Results.BadRequest("Como não foi encontrado dados de atualização, nada foi efetuado.");

            var collections = _repSystemUserCollection.AtualizeUserCollections(collectionsRequest.Collections);

            if (collections.Count != 0)
            {
                var userId = collections.FirstOrDefault().UserId;
                factory.SetConnections(_repSystemUserCollection.CollectionInUse(userId), userId);
                factory.Authenticate(userId);
            }

            return Results.Ok();

        });

        app.MapPost("/atualizeConnections", (HttpRequest request, IRepSystemUserCollection _repSystemUserCollection) =>
        {
            if (!request.Headers.TryGetValue("userId", out var userId))
                return Results.BadRequest("Código do usuario não encontrado no cabeçalho da requisição.");

            factory.SetConnections(_repSystemUserCollection.CollectionInUse(new Guid(userId)), new Guid(userId));
            factory.Authenticate(new Guid(userId));

            var ret = factory.Search(new Guid(userId));
            return Results.Ok(ret);
        });

        app.UseSwagger();
        app.UseSwaggerUI();
        app.Run();
    }
}