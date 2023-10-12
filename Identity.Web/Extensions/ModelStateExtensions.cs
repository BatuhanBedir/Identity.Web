using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Identity.Web.Extensions;

public static class ModelStateExtensions
{
    public static void AddModelErrorList(this ModelStateDictionary modelStateDictionary, List<string> errors)
    {
        errors.ForEach(x => { modelStateDictionary.AddModelError(string.Empty, x); });
    }
}