using ECom.Controllers;
using ECom.Service;
using EComBusiness.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;

namespace ECom.ODataControllers
{
    [Route("odata/products")]
    [ApiController]
    public class ProductODataController : ODataController
    {
        EComContext _db;

        public ProductODataController(EComContext db)
        {
            _db = db;
        }

        [EnableQuery]
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public ActionResult<IQueryable<Product>> Get()
        {
            return Ok(_db.Products);
        }        
    }
}
