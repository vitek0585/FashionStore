using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using FashionStore.Domain.Core.Entities.Store;

namespace FashionStore.Utils.Custom_Validation
{
    public class ClassificationGoodValidator : ValidationAttribute
    {

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is IEnumerable<ClassificationGood>)
            {
                var cassificationGoods = value as IEnumerable<ClassificationGood>;
                var isUniqueCls = cassificationGoods.GroupBy(g => new
                {
                    g.SizeId,
                    g.ColorId

                }).Count() == cassificationGoods.Count();
                if (!isUniqueCls)
                {
                    return new ValidationResult(ErrorMessage);
                }
            }

            return ValidationResult.Success;


        }
    }
}