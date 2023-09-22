using Com.Ctrip.Framework.Apollo;
using Com.Ctrip.Framework.Apollo.Core;
using Com.Ctrip.Framework.Apollo.Logging;
using IdentityServer4.AccessTokenValidation;
using IdentityServer4.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Values;

namespace OcelotDemo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region ��ʼ�������ļ�
            builder.Configuration.SetBasePath(builder.Environment.ContentRootPath)
                      .AddJsonFile("appsettings.json", true, true)
                  .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true, true)
                  .AddJsonFile("ocelot.json")
                  .AddEnvironmentVariables();
            #endregion

            #region ��Ȩ
            var authenticationProviderKey = "OcelotKey";
            //var identityServerOptions = new IdentityServerOptions();
            //builder.Configuration.Bind("IdentityServerOptions", identityServerOptions);
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddIdentityServerAuthentication(authenticationProviderKey, options =>
                {
                    options.RequireHttpsMetadata = false; //�Ƿ�����https
                    options.Authority = $"http://localhost:9005";//������Ȩ��֤�ĵ�ַ
                    options.ApiName = "Gateway"; //��Դ���ƣ�����֤������ע�����Դ�б������е�apiResourceһ��
                    options.SupportedTokens = SupportedTokens.Both;
                }
                );
            #endregion

            #region ���apollo

            builder.WebHost.ConfigureAppConfiguration((hostBuilderContext, configurationBuilder) =>
            {
                configurationBuilder.AddApollo(configurationBuilder.Build().GetSection("Apollo"))
                .AddNamespace(ConfigConsts.NamespaceApplication).AddDefault();
                LogManager.UseConsoleLogging(Com.Ctrip.Framework.Apollo.Logging.LogLevel.Info);
            });

            #endregion


            builder.WebHost.UseUrls("http://localhost:1000");


            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddOcelot() ;//����


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }


            app.UseOcelot().Wait();//��������

            app.MapControllers();

            app.Run();
        }
    }
}