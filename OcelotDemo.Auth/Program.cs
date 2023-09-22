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
                new IdentityResources.OpenId(), //����Ҫ��ӣ�������Ч�� scope ����
                new IdentityResources.Profile()
            };
            //ע��IdentityServer����
            builder.Services.AddIdentityServer()
                .AddDeveloperSigningCredential()//Ĭ�ϵĿ�����֤��--��ʱ֤��--��������Ϊ�˱�֤token��ʧЧ��֤���ǲ����
                                                .AddInMemoryIdentityResources(idResources)
                                                .AddInMemoryApiScopes(ClientInitConfig.GetApiScopes())
                .AddInMemoryClients(ClientInitConfig.GetClients())
                .AddInMemoryApiResources(ClientInitConfig.GetApiResources())
              .AddResourceOwnerValidator<ResourceOwnerPasswordValidator>()
              .AddProfileService<ProfileService>();//�ܷ���ɶ��Դ


            builder.WebHost.UseUrls("http://localhost:9005");

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            //�����֤�м��
            app.UseIdentityServer();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}