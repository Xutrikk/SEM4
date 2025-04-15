using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

public class ValidateObjectAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value == null)
        {
            return new ValidationResult($"{validationContext.DisplayName} обязателен.");
        }

        var results = new List<ValidationResult>();
        var context = new ValidationContext(value, null, null);

        if (!Validator.TryValidateObject(value, context, results, true))
        {
            var errorMessages = results.Select(r => r.ErrorMessage);
            return new ValidationResult(string.Join(Environment.NewLine, errorMessages));
        }

        return ValidationResult.Success;
    }
}