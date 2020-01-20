using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BoulderApp.GraphQL;
using BoulderApp.Web.Types;
using GraphQL;
using GraphQL.Server;
using GraphQL.Server.Ui.Playground;
using GraphQL.Types;
using log4net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BoulderApp.Web
{
    public class Startup
    {
        private static ILog _logger;
        private readonly string CorsPolicyName = "_BoulderAppCorsPolicy";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
            _logger.Debug("Startup");
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var pgsqlConString = Configuration.GetConnectionString("PostgresConnectionString");
            if (string.IsNullOrWhiteSpace(pgsqlConString))
            {
                _logger.Warn("Postgres connection string is null or empty");
            }
            services.AddDbContext<BoulderAppContext>(
                options => options.UseNpgsql(pgsqlConString));

            if (Configuration.GetValue<bool>("EnableCors"))
            {
                _logger.Debug("Enabling CORS policy");
                services.AddCors(o => o.AddPolicy(CorsPolicyName, builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader()
                           .AllowCredentials();
                }));
            }


            AddGraphQL(services);
            services.AddSingleton<ILog, ILog>(prov => _logger);
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseGraphQL<BoulderAppSchema>();
            app.UseGraphQLPlayground(new GraphQLPlaygroundOptions());
            app.UseHttpsRedirection();
            app.UseMvc();
        }

        private static void AddGraphQL(IServiceCollection services)
        {
            services.AddScoped<IDependencyResolver>(s => new FuncDependencyResolver(s.GetRequiredService));
            services.AddScoped<BoulderAppSchema>();
            foreach (var type in GetGraphQlTypes())
            {
                services.AddScoped(type);
            }

            services.AddGraphQL(o => { o.ExposeExceptions = false; })
                .AddGraphTypes(ServiceLifetime.Scoped);
        }

        private static IEnumerable<Type> GetGraphQlTypes()
        {
            return typeof(BoulderAppSchema).Assembly
                .GetTypes()
                .Where(x => !x.IsAbstract &&
                            (typeof(GraphType).IsAssignableFrom(x)));
        }
    }
}
