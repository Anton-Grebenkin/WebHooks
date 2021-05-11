using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebHooks.DAL.Models;
using WebHooks.BLL;
using WebHooks.BLL.Services;
using WebHooks.BLL.DTO;
using WebHooks.BLL.Interfaces;

namespace WebHooks.Service.Authentication
{

    public class BasicAuthenticationMiddleware
    {
        private readonly RequestDelegate _next;
        //private IAccountService AccountService;

        public BasicAuthenticationMiddleware(RequestDelegate next)
        {
            //AccountService = accountService;
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext, IAccountService accountService)
        {
            string authHeader = httpContext.Request.Headers["Authorization"];
            if (authHeader != null && authHeader.StartsWith("Basic"))
            {
                string ecodeUsernameAndPass = authHeader.Substring("Basic ".Length).Trim();
                Encoding encoding = Encoding.GetEncoding("UTF-8");
                string usernameAndPassword = encoding.GetString(Convert.FromBase64String(ecodeUsernameAndPass));


                int index = usernameAndPassword.IndexOf(":");
                var username = usernameAndPassword.Substring(0, index);
                var password = usernameAndPassword.Substring(index + 1);


                //получаем данные аккаунта для подключения
                AccountDTO account = accountService.LogIn(username);

                if (account != null && username.Equals(account.Login) && password.Equals(account.Password))
                {
                    //добавляем данные в context
                    httpContext.User.AddIdentities(new ClaimsIdentity[] {
                    new ClaimsIdentity(new Claim[] {
                        new Claim("accountId", account.Id.ToString())
                    })});

                    await _next.Invoke(httpContext);
                }
                else
                {
                    httpContext.Response.StatusCode = 401;
                    return;
                }
            }
            else
            {
                httpContext.Response.StatusCode = 401;
                return;
            }   
        }
    }

    public static class BasicAuthenticationMiddlewareExtension
    {
        public static IApplicationBuilder UseBasicAuthenticationMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<BasicAuthenticationMiddleware>();
        }
    }
}

