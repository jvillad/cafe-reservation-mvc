using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace BeanScene.Tests.Infrastructure
{
    public class ObjectValidator : IObjectModelValidator
    {
        public void Validate(
            ActionContext actionContext,
            ValidationStateDictionary? validationState,
            string prefix,
            object? model)
        {
            // Exit if no model
            if (model == null) return;

            // Initialise validation context and results
            var context = new ValidationContext(model, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();

            // Validate model
            bool isValid = Validator.TryValidateObject(model, context, results, validateAllProperties: true);

            // If not valid, add errors to context
            if (!isValid)
            {
                results.ForEach(r =>
                {
                    // Add validation errors to the ModelState
                    actionContext.ModelState.AddModelError("", r.ErrorMessage ?? "");
                });
            }
        }
    }
}
