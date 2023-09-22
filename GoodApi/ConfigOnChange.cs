using FreeSql;
using Microsoft.Extensions.Primitives;
using static System.Net.Mime.MediaTypeNames;

namespace GoodApi
{
    public static class ConfigOnChange
    {
        public static void AddConfigChan(this IServiceCollection services, IConfiguration configuration)
        {
            //监听配置文件发生
            ChangeToken.OnChange(() => configuration.GetReloadToken(), () =>
            {
                Console.WriteLine("监听到配置发生改变，已经更新..");
                FreeSqlBuilder TSPFreeBuild = new FreeSqlBuilder().UseConnectionString(DataType.MySql, configuration.GetValue<string>("DbConfig")).UseNoneCommandParameter(value: true);
            });
        }
    }
}
