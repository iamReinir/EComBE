using ECom.Controllers;
using EComBusiness.HelperModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ECom.Service
{
    public static class ProductService
    {
        public static async Task<IEnumerable<ProductDTO>> GetProducts(EComContext _context, string? userId)
        {
            var products = await _context.Products
                .AsNoTracking()
                .NotDeleted()
                .Include(p => p.Category)
                .ToListAsync();
            IEnumerable<string> wishedListed = new List<string>();
            IEnumerable<string> rated = new List<string>();
            if (string.IsNullOrEmpty(userId) == false)
            {
                wishedListed = await (from product in _context.Products.NotDeleted()
                                      join wishlist in _context.WishLists.NotDeleted()
                                      on product.ProductId equals wishlist.ProductId
                                      where wishlist.UserId == userId
                                      select product.ProductId).ToListAsync();
                rated = await (from product in _context.Products.NotDeleted()
                               join wishlist in _context.Ratings.NotDeleted()
                               on product.ProductId equals wishlist.ProductId
                               where wishlist.UserId == userId
                               select product.ProductId).ToListAsync();
            }
            var result = (from product in products
                          join productid in wishedListed
                          on product.ProductId equals productid into productids
                          from productid in productids.DefaultIfEmpty()
                          select new ProductDTO
                          {
                              ProductId = product.ProductId,
                              Description = product.Description,
                              Name = product.Name,
                              ImageUrl = product.ImageUrl,
                              Price = product.Price,
                              QuantityAvailable = product.QuantityAvailable,
                              Rating = product.Rating,
                              RatingCount = product.RatingCount,
                              IsWishlisted = !string.IsNullOrEmpty(productid),
                              Category = new CategoryDTO
                              {
                                  CategoryId = product.Category?.CategoryId,
                                  Description = product.Category?.Description,
                                  ImageUrl = product.Category?.ImageUrl,
                                  Name = product.Category?.Name
                              }
                          });
            result = (from product in result
                      join productid in rated
                      on product.ProductId equals productid into productids
                      from productid in productids.DefaultIfEmpty()
                      select new { product, isRated = !string.IsNullOrEmpty(productid) })
                      .Select(p =>
                      {
                          p.product.IsRated = p.isRated;
                          return p.product;
                      });
            return result ?? new List<ProductDTO>();
        }

        public static async Task<IEnumerable<ProductDTO>> GetProducts(
            EComContext _context, 
            IEnumerable<string> userIds)
        {
            var products = await _context.Products
                .NotDeleted()
                .Include(p => p.Category)
                .ToListAsync();
            IEnumerable<string> wishedListed = new List<string>();
            IEnumerable<string> rated = new List<string>();
            if (userIds.Any() == false)
            {
                wishedListed = await (from product in _context.Products.NotDeleted()
                                      join wishlist in _context.WishLists.NotDeleted()
                                      on product.ProductId equals wishlist.ProductId
                                      where userIds.Contains(wishlist.UserId)
                                      select product.ProductId).ToListAsync();
                rated = await (from product in _context.Products.NotDeleted()
                               join wishlist in _context.Ratings.NotDeleted()
                               on product.ProductId equals wishlist.ProductId
                               where userIds.Contains(wishlist.UserId)
                               select product.ProductId).ToListAsync();
            }
            var result = (from product in products
                          join productid in wishedListed
                          on product.ProductId equals productid into productids
                          from productid in productids.DefaultIfEmpty()
                          select new ProductDTO
                          {
                              ProductId = product.ProductId,
                              Description = product.Description,
                              Name = product.Name,
                              ImageUrl = product.ImageUrl,
                              Price = product.Price,
                              QuantityAvailable = product.QuantityAvailable,
                              Rating = product.Rating,
                              RatingCount = product.RatingCount,
                              IsWishlisted = !string.IsNullOrEmpty(productid),
                              Category = new CategoryDTO
                              {
                                  CategoryId = product.Category?.CategoryId,
                                  Description = product.Category?.Description,
                                  ImageUrl = product.Category?.ImageUrl,
                                  Name = product.Category?.Name
                              }
                          });
            result = (from product in result
                      join productid in rated
                      on product.ProductId equals productid into productids
                      from productid in productids.DefaultIfEmpty()
                      select new { product, isRated = !string.IsNullOrEmpty(productid) })
                      .Select(p =>
                      {
                          p.product.IsRated = p.isRated;
                          return p.product;
                      });
            return result ?? new List<ProductDTO>();
        }

    }
}
