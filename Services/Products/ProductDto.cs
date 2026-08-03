using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Services.Products
{
    public record ProductDto(int Id, string Name, decimal Price, int Stock, int CategoryId);
    // This is a record type that represents a product data transfer object (DTO) with properties for Id, Name, Price, Stock, and CategoryId.
}
