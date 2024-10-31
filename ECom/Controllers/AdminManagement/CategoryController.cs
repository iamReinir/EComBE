using EComBusiness.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace ECom.Controllers.AdminManagement
{
    [Route("odata/Category")]
  //  [ApiController]
    // [Authorize(Roles = "Admin")]
    public class CategoryController : ODataController
    {
        private readonly EComContext db;

        public CategoryController(EComContext context)
        {
            db = context;
        }

        [EnableQuery]
        [HttpGet("GetCategories")]

        public ActionResult<IQueryable<Category>> GetCategories()
        {
            return Ok(db.Categories);
        }
        [EnableQuery]
        [HttpGet("GetCategoriesById/{key}")]
        public IActionResult GetCategoryById([FromRoute] string key)
        {
            var category = db.Categories.FirstOrDefault(c => c.CategoryId == key);
            if (category == null)
            {
                return NotFound();
            }
            return Ok(category);
        }

        [HttpPost("CreateCategory")]
        public IActionResult CreateCategory([FromBody] Category category)
        {
            db.Categories.Add(category);
            db.SaveChanges();
            return Created(category);
        }

        //[HttpDelete("DeleteCategory/{key}")]
        //public IActionResult DeleteCategory([FromRoute] string key)
        //{
        //    var category = db.Categories.FirstOrDefault(c => c.CategoryId == key);
        //    if (category == null)
        //    {
        //        return NotFound();
        //    }
        //    db.Categories.Remove(category);
        //    db.SaveChanges();
        //    return Ok($"Category with ID {key} has been deleted.");
        //}
    }
}