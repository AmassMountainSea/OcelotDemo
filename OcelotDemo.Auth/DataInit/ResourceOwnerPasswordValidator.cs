using IdentityServer4.Validation;
using System.Security.Claims;

namespace OcelotDemo.Auth.DataInit
{
    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            await Task.Run(() =>
            {
                //根据context.UserName和context.Password与数据库的数据做校验，判断是否合法
                context.Result = new GrantValidationResult(
                subject: context.UserName,
                authenticationMethod: "custom",
                claims: new Claim[]
                {
                        new Claim("UserName",context.UserName),
                        new Claim("UserTenement",context.Password),
                });
            });

        }
    }
}
