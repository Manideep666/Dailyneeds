using System;
using System.Collections.Generic;
using System.Linq;
using DailyNeeds1.DTO;
using DailyNeeds1.Entities;

namespace DailyNeeds1.Repo
{
    public class ProductImpl : IRepo<ProductDTO>
    {
        private DailyNeedsDbContext context;

        public ProductImpl(DailyNeedsDbContext ctx)
        {
            context = ctx;
        }

        public bool Add(ProductDTO item)
        {
            try
            {
                Product product = new Product
                {
                    ProductName = item.ProductName,
                    CategoryID = item.CategoryID,
                    UserID = item.UserID,
                    Price = item.Price,
                    Availability = item.Availability
                };

                context.products.Add(product);
                context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                // Handle exceptions/log if necessary
                return false;
            }
        }

        public bool Delete(int productId)
        {
            try
            {
                var product = context.products.Find(productId);
                if (product != null)
                {
                    context.products.Remove(product);
                    context.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                // Handle exceptions/log if necessary
                return false;
            }
        }

        public List<ProductDTO> GetAll()
        {
            var products = context.products
                .Select(p => new ProductDTO
                {
                    ProductID = p.ProductID,
                    ProductName = p.ProductName,
                    CategoryID = p.CategoryID,
                    UserID = p.UserID,
                    Price = p.Price,
                    Availability = p.Availability
                })
                .ToList();

            return products;
        }

        public bool Update(ProductDTO item)
        {
            try
            {
                var product = context.products.Find(item.ProductID);
                if (product != null)
                {
                    product.ProductName = item.ProductName;
                    product.CategoryID = item.CategoryID;
                    product.UserID = item.UserID;
                    product.Price = item.Price;
                    product.Availability = item.Availability;
                    context.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                // Handle exceptions/log if necessary
                return false;
            }
        }
    }
}
