using CRUD_1st.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CRUD_1st.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UsersApiiController : ControllerBase
	{
		private readonly ApplicationDbContext _db;

		public UsersApiiController(ApplicationDbContext db)
		{
			_db = db;
		}



		[HttpGet]

		public async Task<ActionResult> GetUsers()
		{
			try
			{
				var users = await _db.Users.ToListAsync();
				return Ok(users);
			}
			catch (Exception ex)
			{
				// Log the exception or return a meaningful error response
				return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error: " + ex.Message);
			}
		}
	}
}
