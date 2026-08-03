using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace App.Services.Products.Update
{
    public class UpdateProductRequestValidator : AbstractValidator<UpdateProductRequest>
    {
        public UpdateProductRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .Length(3, 10).WithMessage("Name must be between 3 and 10 characters.");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Price must be a positive number.");

            RuleFor(x=>x.Stock)
                .InclusiveBetween(1,100).WithMessage("Stock must be between 1 and 100.");

            RuleFor(x=>x.CategoryId)
                .GreaterThan(0).WithMessage("Category ID must be a positive value.");
        }

    }
}
