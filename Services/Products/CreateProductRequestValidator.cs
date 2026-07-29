using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace App.Services.Products
{
    public class CreateProductRequestValidator : AbstractValidator<CreateProductRequest>
    {
        public CreateProductRequestValidator()
        {
            //Name notEmpty, notNull, length validation
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Product name is required.")
                .NotNull().WithMessage("Product name is required.")
                .Length(3, 10).WithMessage("Product name must be between 3 and 10 characters.");
            
            //Price greaterThan validation

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Product price must be a positive value.");

            //Stock inclusiveBetween 1-100 validation

            RuleFor(x=>x.Stock)
                .InclusiveBetween(1, 100).WithMessage("Product stock must be between 1 and 100.");
        }
    }
}
