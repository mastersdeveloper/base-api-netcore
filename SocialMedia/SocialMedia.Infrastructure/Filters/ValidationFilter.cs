using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;

namespace SocialMedia.Infrastructure.Filters
{
    /*
     * ActionFilter: validacion que se realiza en el api
     * Hay 3 ambitos para el ActionFilter:
        - global
        - controlador
        - accion
    */

    public class ValidationFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.ModelState.IsValid)
            {
                context.Result = new BadRequestObjectResult(context.ModelState);
                return;
            }

            await next();
        }
    }
}
