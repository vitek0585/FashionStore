using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using FashionStore.Domain.Core.Entities.Store;
using Microsoft.Security.Application;

namespace FashionStore.Utils.Custom_Validation
{
    [AttributeUsage(AttributeTargets.Property,AllowMultiple = false,Inherited = false)]
    public class ValidateAntiXssAttribute : ValidationAttribute
    {

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (!string.IsNullOrWhiteSpace(value as string))
            {
            
                //if (!string.Equals(Sanitizer.GetSafeHtmlFragment((string) value), (string)value,
                //    StringComparison.InvariantCulture))
                //{
                //    ErrorMessage = string.Format("The value {0} is subject to xss attack", value.ToString());
                //    return new ValidationResult(ErrorMessage);
                //}
            }

            return ValidationResult.Success;


        }
    }

}