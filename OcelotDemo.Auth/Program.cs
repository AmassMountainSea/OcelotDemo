using IdentityServer4.Models;
using Microsoft.Extensions.DependencyInjection;
using OcelotDemo.Auth.DataInit;

namespace OcelotDemo.Auth
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var idResources = new List<IdentityResource>
            {
                new IdentityResources.OpenId(), //必须要添加，否则报无效的 scope 错误
                new IdentityResources.Profile()
            };
            //注入IdentityServer服务
            builder.Services.AddIdentityServer()
                .AddDeveloperSigningCredential()//默认的开发者证书--临时证书--生产环境为了保证token不失效，证书是不变的
                                                .AddInMemoryIdentityResources(idResources)
                                                .AddInMemoryApiScopes(ClientInitConfig.GetApiScopes())
                .AddInMemoryClients(ClientInitConfig.GetClients())
                .AddInMemoryApiResources(ClientInitConfig.GetApiResources())
              .AddResourceOwnerValidator<ResourceOwnerPasswordValidator>()
              .AddProfileService<ProfileService>();//能访问啥资源


            builder.WebHost.UseUrls("http://localhost:9005");

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            //添加认证中间件
            app.UseIdentityServer();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}