
using IdentityServer4;
using IdentityServer4.Models;
using Microsoft.AspNetCore.DataProtection;

namespace OcelotDemo.Auth.DataInit
{
    public class ClientInitConfig
    {
        /// <summary>
        /// 定义ApiResource   
        /// 这里的资源（Resources）指的就是管理的API
        /// </summary>
        /// <returns>多个ApiResource</returns>
        public static IEnumerable<ApiScope> GetApiScopes()
        {
            return new[]
            {
                new ApiScope("Gateway")
            };
        }
        /// <summary>
        /// 定义ApiResource   
        /// 这里的资源（Resources）指的就是管理的API
        /// </summary>
        /// <returns>多个ApiResource</returns>
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new[]
            {
                new ApiResource("Gateway", "网关")
                {
                    Scopes = { "Gateway" }

                }
            };
        }

        /// <summary>
        /// 定义验证条件的Client
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Client> GetClients()
        {
            return new[]
            {
                new Client
                {
                    ClientId = "TSP.client",//客户端惟一标识
                    ClientSecrets = new [] { new IdentityServer4.Models.Secret("123456".Sha256()) },//客户端密码，进行了加密
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    AccessTokenLifetime =  168 * 3600,  //过期时间
                    //授权方式，客户端认证，只要ClientId+ClientSecrets 
                    AllowedScopes = new [] { "Gateway",IdentityServerConstants.StandardScopes.OpenId, //必须要添加，否则报forbidden错误
                    IdentityServerConstants.StandardScopes.Profile },//允许访问的资源
              
                }
            };
        }
    }
}
