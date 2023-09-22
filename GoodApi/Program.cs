using Com.Ctrip.Framework.Apollo;
using Com.Ctrip.Framework.Apollo.Core;
using Com.Ctrip.Framework.Apollo.Enums;
using Com.Ctrip.Framework.Apollo.Logging;
using GoodApi.Dto;
using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;

namespace GoodApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
 

            #region 添加apollo

            builder.Host.ConfigureAppConfiguration((hostBuilderContext, configurationBuilder) =>
            {
                configurationBuilder.AddApollo(configurationBuilder.Build().GetSection("Apollo")).AddDefault();
            });

            #endregion

            #region freesql
            Func<IServiceProvider, IFreeSql> fsqlFactory = r =>
            {
                IFreeSql fsql = new FreeSql.FreeSqlBuilder()
                    .UseConnectionString(FreeSql.DataType.MySql, builder.Configuration.GetValue<string>("DbConfig"))
                    .UseMonitorCommand(cmd => Console.WriteLine($"Sql：{cmd.CommandText}"))//监听SQL语句
                    //.UseAutoSyncStructure(true) //自动同步实体结构到数据库，FreeSql不会扫描程序集，只有CRUD时才会生成表。
                    .Build();
                return fsql;
            };
            builder.Services.AddSingleton<IFreeSql>(fsqlFactory);

            builder.Services.AddConfigChan(builder.Configuration);

            #endregion

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.WebHost.UseUrls("http://localhost:1001");

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();

            app.UseAuthentication();

            app.MapControllers();

            app.Run();
        }
    }
}