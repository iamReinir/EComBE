using EComBusiness.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace ECom.Controllers.AdminManagement
{

        [Route("odata/User")]
        // [Authorize(Roles = "Admin")]
        public class UserController : ODataController
        {
            private readonly EComContext db;

            public UserController(EComContext context)
            {
                db = context;
            }

        [EnableQuery]
        [HttpGet("GetUser")]
        public ActionResult<IQueryable<User>> GetUser()
        {
            return Ok(db.AppUsers);
        }
        [EnableQuery]
        [HttpGet("GetUserById/{key}")]
        public IActionResult GetUserById([FromRoute] string key)
        {
            var user = db.AppUsers.FirstOrDefault(c => c.UserId == key); 
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpPost("CreateUser")]
        public IActionResult CreateUser([FromBody] User user)
        {
            db.AppUsers.Add(user);
            db.SaveChanges();
            return Created(user);
        }

        //[HttpDelete("DeleteUser/{key}")]
        //public IActionResult DeleteUser([FromRoute] string key)
        //{
        //    var user = db.AppUsers.FirstOrDefault(c => c.UserId == key);
        //    if (user == null)
        //    {
        //        return NotFound();
        //    }
        //    db.AppUsers.Remove(user);
        //    db.SaveChanges();
        //    return Ok($"User with ID {key} has been deleted.");
        //}
    }
}