using Microsoft.EntityFrameworkCore;
using PipelineSearchHub.DBContexts.Base;
using PipelineSearchHub.DBContexts.SystemCollections;
using PipelineSearchHub.DBContexts.SystemUsers;

namespace PipelineSearchHub.DBContexts
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