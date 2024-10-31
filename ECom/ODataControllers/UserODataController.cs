using EComBusiness.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace ECom.ODataControllers
{
    [Route("odata/users")]
    [ApiController]    
    public class UserODataController : ODataController
    {
        EComContext _db;

        public UserODataController(EComContext db)
        {
            _db = db;
        }

        [EnableQuery]
        [HttpGet]
        public ActionResult<IQueryable<User>> Get()
        {
            return Ok(_db.AppUsers);
        }
    }
}
