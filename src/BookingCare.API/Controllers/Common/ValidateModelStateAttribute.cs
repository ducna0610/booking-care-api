using BookingCare.API.Controllers.Common.Wrapper;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace BookingCare.API.Controllers.Common;

public class ValidateModelStateAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ModelState.IsValid)
        {
            var errors = context.ModelState.Values.Where(v => v.Errors.Count > 0)
                    .SelectMany(v => v.Errors)
                    .Select(v => v.ErrorMessage)
                    .ToList();

            var value = context.ModelState.Keys.ToList();
            Dictionary<string, string[]> dictionary = new Dictionary<string, string[]>();
            foreach (var modelStateKey in context.ModelState.Keys.ToList())
            {
                string[] arr = null;
                List<string> list = new List<string>();
                foreach (var error in context.ModelState[modelStateKey].Errors)
                {
                    list.Add(error.ErrorMessage);
                }
                arr = list.ToArray();
                if (arr.Length > 0)
                {
                    dictionary.Add(modelStateKey, arr);
                }
            }

            var responseObj = new ErrorResult(dictionary, "Validation errors in your request");

            context.Result = new BadRequestObjectResult(responseObj);
        }
    }
}