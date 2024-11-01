using EComBusiness.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Results;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;

namespace ECom.Controllers.AdminManagement
{
  [Route("odata/Product")]
 //   [ApiController]
   // [Authorize(Roles = "Admin")]
    public class ProductController : ODataController
    {
        private readonly EComContext db;

        public ProductController(EComContext context)
        {
            db = context;
        }

        [EnableQuery]
        [HttpGet("GetProduct")]
        public ActionResult<IQueryable<Product>> GetProduct()
        {
            return Ok(db.Products);
        }

        [EnableQuery]
        [HttpGet("GetProductById/{key}")]
        public IActionResult GetProductById([FromRoute] string key)
        {
            var product = db.Products.FirstOrDefault(c => c.ProductId == key);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }


  
        [HttpPost("CreateProduct")]
        public IActionResult CreateProduct([FromBody] Product product)
        {
            db.Products.Add(product);
            db.SaveChanges();
            return Created(product);
        }

        //[HttpDelete("DeleteProduct/{key}")]
        //public IActionResult DeleteProduct([FromRoute] string key)
        //{
        //    var product = db.Products.FirstOrDefault(c => c.ProductId == key);
        //    if (product == null)
        //    {
        //        return NotFound();
        //    }
        //    db.Products.Remove(product);
        //    db.SaveChanges();
        //    return Ok($"Product with ID {key} has been deleted.");
        //}
    }
}