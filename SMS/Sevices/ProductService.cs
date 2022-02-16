using SMS.Contracts;
using SMS.Data.Common;
using SMS.Data.Models;
using SMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMS.Sevices
{
    public class ProductService : IProductService
    {
        private readonly IRepository repo;
        public ProductService(IRepository _repo)
        { repo = _repo; }

        public (bool created, string error) Create(CreateViewModel model)
        {
            bool created = false; string error = null;
            var (isValid, validationError) = ValidateCreateModel(model);

            if (!isValid)
            {
                return (isValid, validationError);
            }

            Product product = new Product()
            {
                Name = model.Name,
                Price = (decimal)model.Price
            };

            try
            {
                repo.Add(product);
                repo.SaveChanges();
                created = true;
            }
            catch (Exception)
            {
                error = "Couldn't create the product in DB!";
            }
            return (created, error);
        }

        public IEnumerable<ProductListViewModel> GetProducts()
        {
            return repo.All<Product>()
                .Select(p => new ProductListViewModel()
                {
                    ProductName = p.Name,
                    ProductPrice = $"${p.Price.ToString("F2")}",
                    ProductId = p.Id
                })
                .ToList(); 
        }

        public (bool isValid, string error) ValidateCreateModel(CreateViewModel model)
        {
            bool isValid = true;
            StringBuilder error = new StringBuilder();

            if (model == null)
            {
                return (false, "Create model is required!");
            }

            if (String.IsNullOrWhiteSpace(model.Name) || model.Name.Length < 4 || model.Name.Length > 20)
            {
                isValid = false;
                error.AppendLine("Product name must be between 4 and 20 characters!");
            }

            if (model.Price < (decimal)0.05D || model.Price > (decimal)1000D)
            {
                isValid = false;
                error.AppendLine($"Product price must be between 0.05 and 1000 characters! You entered {model.Price}");
            }

            return (isValid, error.ToString());

        }
    }
}
