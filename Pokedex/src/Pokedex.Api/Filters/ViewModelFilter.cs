using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Pokedex.Business.Core;

namespace Pokedex.Api.Filters
{
    public class ViewModelFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Result is not ObjectResult)
                return;

            var result = context.Result as ObjectResult;

            if (result!.Value is Entity)
                throw new InvalidOperationException("Domain entities cannot be sent via REST. Please use a model type.");
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if(context.ActionArguments.Values.Any(a => a is Entity))
                throw new InvalidOperationException("Domain entities cannot be received via REST. Please use a model type.");
        }
    }
}