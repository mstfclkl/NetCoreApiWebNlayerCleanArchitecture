using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using App.Repositories.Products;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.IdentityModel.Tokens;

namespace App.Services.Products.Create
{
    public class CreateProductRequestValidator : AbstractValidator<CreateProductRequest>
    {
        private readonly IProductRepository _productRepository;
        public CreateProductRequestValidator(IProductRepository productRepository)
        {
            _productRepository = productRepository;

            //Name notEmpty, notNull, length validation
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Product name is required.")
                .Length(3, 10).WithMessage("Product name must be between 3 and 10 characters.");
            //  .MustAsync(MustUniqueProductNameAsync).WithMessage("Product name must be unique.");
            //  .Must(MustUniqueProductName).WithMessage("Product name must be unique.");
            
            //Price greaterThan validation

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Product price must be a positive value.");

            //Stock inclusiveBetween 1-100 validation

            RuleFor(x=>x.Stock)
                .InclusiveBetween(1, 100).WithMessage("Product stock must be between 1 and 100.");

            RuleFor(x => x.CategoryId)
                .GreaterThan(0).WithMessage("Product category ID must be a positive value.");
        }
        /*

        private async Task<bool> MustUniqueProductNameAsync(string name, CancellationToken cancellationToken)
        {
            return !await _productRepository.Where(x => x.Name == name).AnyAsync(cancellationToken);
            //false => validation failed
            //true => validation passed
        }
        
        sync version of the above method

        private bool MustUniqueProductName(string name)
        {
            return !_productRepository.Where(x=>x.Name == name).Any();
            //false => validation failed
            //true => validation passed
        }

        */
    }
}
