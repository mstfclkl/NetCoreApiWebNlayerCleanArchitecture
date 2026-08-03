using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using App.Repositories.Products;
using App.Repositories;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using App.Services.Products.Create;
using App.Services.Products.Update;
using AutoMapper;
using App.Services.Products.UpdateStock;

namespace App.Services.Products
{
    public class ProductService(IProductRepository productRepository,IUnitOfWork unitOfWork,IValidator<CreateProductRequest> createProductRequestValidator,IMapper mapper) 
        : IProductService
    {
        public async Task<ServiceResult<List<ProductDto>>> GetTopPriceProductsAsync(int count)
        {
            var products = await productRepository.GetTopPriceProductsAsync(count);

            //var productsAsDto = products.Select(p => new ProductDto(p.Id, p.Name, p.Price, p.Stock)).ToList();
            var productsAsDto = mapper.Map<List<ProductDto>>(products);

            return new ServiceResult<List<ProductDto>>()
            {
                Data = productsAsDto
            };
        }

        public async Task<ServiceResult<List<ProductDto>>> GetAllListAsync()
        {
            var products = await productRepository.GetAll().ToListAsync();
          
            //var productsAsDto = products.Select(p => new ProductDto(p.Id, p.Name, p.Price, p.Stock)).ToList();
            
            var productsAsDto= mapper.Map<List<ProductDto>>(products); // AutoMapper kullanarak Product nesnelerini ProductDto nesnelerine dönüştürme
            return ServiceResult<List<ProductDto>>.Success(productsAsDto);
        }

        public async Task<ServiceResult<List<ProductDto>>> GetPagedAllListAsync(int pageNumber, int pageSize)
        {
            //1-10 => ilk 10 Kayit skip(0).Take(10)


            var products = await productRepository.GetAll().Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
            
            //var productsAsDto = products.Select(p => new ProductDto(p.Id, p.Name, p.Price, p.Stock)).ToList();
           
            var productsAsDto= mapper.Map<List<ProductDto>>(products); 
            return ServiceResult<List<ProductDto>>.Success(productsAsDto);
        }
       
        public async Task<ServiceResult<ProductDto?>> GetByIdAsync(int id)
        {
            var product = await productRepository.GetByIdAsync(id);

            if (product == null)
            {
                return ServiceResult<ProductDto?>.Fail("Product not found.", HttpStatusCode.NotFound);
            }

            // var productsAsDto = new ProductDto(product.Id, product.Name, product.Price, product.Stock);
            var productsAsDto= mapper.Map<ProductDto>(product); 
            return ServiceResult<ProductDto?>.Success(productsAsDto);
        }

        public async Task<ServiceResult<CreateProductResponse>> CreateAsync(CreateProductRequest request)
        {
          // async manuel service business check 
            var anyProduct = await productRepository.Where(p => p.Name == request.Name).AnyAsync();

            if (anyProduct)
            {
                return ServiceResult<CreateProductResponse>.Fail("Product name already exists.", HttpStatusCode.BadRequest);
            }




            /* When we do not use FluentValidation.AspNetCore package in the project, we can use this code to validate the request manually async
             
            var validationResult = await createProductRequestValidator.ValidateAsync(request);
            if (validationResult.IsValid)
            {
                return ServiceResult<CreateProductResponse>.Fail(validationResult.Errors.Select(e => e.ErrorMessage).ToList());

            }

             */

            var product = mapper.Map<Product>(request); // AutoMapper kullanarak CreateProductRequest nesnesini Product nesnesine dönüştürme
            /*
            var product = new Product
           {
               Name = request.Name,
               Price = request.Price,
               Stock = request.Stock
           };
            */
            await productRepository.AddAsync(product);
           await unitOfWork.SaveChangesAsync();
           return ServiceResult<CreateProductResponse>.SuccessAsCreated(new CreateProductResponse(product.Id),$"api/products/{product.Id}");
        }

        public async Task<ServiceResult> UpdateAsync(int id, UpdateProductRequest request)
        {
            var product = await productRepository.GetByIdAsync(id);
           
            if (product == null)
            {
                return ServiceResult.Fail("Product not found.", HttpStatusCode.NotFound);
            }

            var isProductNameExist = await productRepository.Where(p => p.Name == request.Name && p.Id != id).AnyAsync();

            if (isProductNameExist)
            {
                return ServiceResult.Fail("Product name already exists.", HttpStatusCode.BadRequest);
            }
            /*
            product.Name = request.Name;
            product.Price = request.Price;
            product.Stock = request.Stock; 
            */
            product = mapper.Map(request, product); // AutoMapper kullanarak UpdateProductRequest nesnesini mevcut Product nesnesine dönüştürme


            productRepository.Update(product);
            await unitOfWork.SaveChangesAsync();
            return ServiceResult.Success(HttpStatusCode.NoContent);
        }

        public async Task<ServiceResult> UpdateStockAsync(UpdateProductStockRequest request)
        {
            var product = await productRepository.GetByIdAsync(request.productId);
            if (product is null)
            {
                return ServiceResult.Fail("Product not found.", HttpStatusCode.NotFound);
            }




            product.Stock = request.quantity;
            productRepository.Update(product);
            await unitOfWork.SaveChangesAsync();
            return ServiceResult.Success(HttpStatusCode.NoContent);
        }

        public async Task<ServiceResult> DeleteAsync(int id)
        {
            var product = await productRepository.GetByIdAsync(id);
            if (product is null)
            {
                return ServiceResult.Fail("Product not found.", HttpStatusCode.NotFound);
            }
            productRepository.Delete(product);
            await unitOfWork.SaveChangesAsync();
            return ServiceResult.Success(HttpStatusCode.NoContent);
        }

        
    }
}
