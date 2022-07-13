using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace WebsiteBlazor.Infrastructure
{
    public class SampleMiddleWare
    {
        private readonly RequestDelegate _next;

        public SampleMiddleWare(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context?.User?.Identity?.IsAuthenticated == false && context.Request.Path.Value != "/account/signin")
            {
                context.Response.Redirect("/account/signin");
            }

            //do some type check
            /*if (context?.User?.Claims != null && context.User.Claims.Any())
            {
                //do some work
            }
*/
            // Call the next delegate/middleware in the pipeline
            await _next(context);
        }
    }
}