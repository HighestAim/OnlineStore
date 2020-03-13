using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using OnlineStore.DAL;
using OnlineStore.Core.Abstractions;
using OnlineStore.Core.Abstractions.RepositoryInterfaces;
using OnlineStore.DAL.Repositories;
using OnlineStore.Core.Abstractions.OperationInterfaces;
using OnlineStore.BLL.Operations;
using System.Text;
using AutoMapper;
using OnlineStore.API.ErrorHandling;
using OnlineStore.Core.Models;
using Microsoft.EntityFrameworkCore;
using OnlineStore.API;
using OnlineStore.BLL.Managers;
using OnlineStore.API.Hubs;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.OpenApi.Models;
using OnlineStore.API.Middleware;
using System.IdentityModel.Tokens.Jwt;

namespace OnlineStore
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Online Store API" });
            });
            services.AddControllers();
            services.AddCors();
            services.AddSignalR()
                .AddJsonProtocol(options =>
                {
                    options.PayloadSerializerOptions.WriteIndented = false;
                });
            services.AddDbContextPool<OnlineStoreContext>(options =>
            {
                options.UseSqlServer(Configuration["ConnectionString"], m => m.MigrationsAssembly("OnlineStore.API"));
                options.EnableSensitiveDataLogging(true);
            });
            services.AddMvcCore()
                    .AddApiExplorer();

            services.AddCors(o => o.AddPolicy("CorsPolicy", builder => {
                builder
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials()
                .WithOrigins("http://localhost:4200");
            }));
            services.AddAutoMapper();

            services.AddTransient<IRepositoryManager, RepositoryManager>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IOrderRepository, OrderRepository>();
            services.AddTransient<ICategoryRepository, CategoryRepository>();
            services.AddTransient<IProductRepository, ProductRepository>();
            services.AddTransient<IOrderProductRepository, OrderProductRepository>();
            services.AddTransient<IUserOperations, UserOperations>();
            services.AddTransient<IProductOperations, ProductOperations>();
            services.AddTransient<IOrderOperations, OrderOperations>();
            services.AddTransient<ICategoryOperations, CategoryOperations>();
            services.AddTransient<ILiveUpdateOperations, LiveUpdateOperations>();
            services.AddSingleton<SocketManager>();

            var appSettingsSection = Configuration.GetSection("TokenAuthentification");
            services.Configure<TokenAuthentification>(appSettingsSection);
            var appSettings = appSettingsSection.Get<TokenAuthentification>();
            var key = Encoding.ASCII.GetBytes(appSettings.SecretKey);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.Events = new JwtBearerEvents
                {
                    OnTokenValidated = context =>
                    {
                        var userService = context.HttpContext.RequestServices.GetRequiredService<IUserOperations>();
                        var userId = int.Parse(context.Principal.Identity.Name);
                        var user = userService.GetById(userId);
                        if (user == null)
                        {
                            context.Fail("Unauthorized");
                        }
                        return Task.CompletedTask;
                    }
                };
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
            services.AddScoped<IUserOperations, UserOperations>();
            services.AddScoped<JwtSecurityTokenHandler>();
            services.AddGrpc();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment environment)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"/swagger/v1/swagger.json", "My API V1");
            });
            app.UseMiddleware<ErrorHandlingMiddleware>();
            
            app.UseCors("CorsPolicy");
            //app.UseCors(op => {
            //    op.AllowAnyOrigin();
            //    op.AllowAnyMethod();
            //    op.AllowAnyHeader();
            //    op.AllowCredentials();
            //    });
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseWebSockets();
            app.UseMiddleware<WebSocketMiddleware>();

            app.UseEndpoints(endpoints => 
            {
                endpoints.MapControllers();
                endpoints.MapHub<EventsHub>("/eventHub");
            });

            app.SeedData();
        }
    }
}
