using Microsoft.EntityFrameworkCore;
using PipelinesTeste2.DBContexts.Base;
using PipelinesTeste2.DBContexts.SystemCollections;
using PipelinesTeste2.DBContexts.SystemUsers;

namespace PipelinesTeste2.DBContexts
{
    public class DBBuilderBase
    {
        public void ModelCreating(WebApplicationBuilder builder)
        {
            builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite("Data Source=app.db"));

            builder.Services.AddScoped<IRepSystemUserCollection, RepSystemUserCollection>();
            builder.Services.AddScoped<IRepSystemUser, RepSystemUser>();

            builder.Services.AddScoped(typeof(IBaseDbContext<>), typeof(BaseDbContext<>));

        }
    }
}