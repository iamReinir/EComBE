using EComBusiness.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;

namespace ECom.ODataControllers
{
    [Route("odata/categories")]
    [ApiController]
    public class CategoryODataController : Controller
    {

        EComContext _db;

        public CategoryODataController(EComContext db)
        {
            _db = db;
        }

        [EnableQuery]
        [HttpGet]
        public ActionResult<IQueryable<Category>> Get()
        {
            return Ok(_db.Categories);
        }
    }
}
