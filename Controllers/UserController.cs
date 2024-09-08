using CRUD_1st.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace CRUD_1st.Controllers
{
	public class UserController : Controller
	{

		private readonly ApplicationDbContext _db;
		public UserController(ApplicationDbContext db)
		{
			_db = db;
		}
		public IActionResult Index1()
		{
			return View();
		}
		public IActionResult Index()
		{
			IEnumerable<User> users = _db.Users.Include(u => u.Rids);
			if (users.IsNullOrEmpty())
			{
				return NotFound();
			}
			return View(users);
		}
		//Get
		public IActionResult Create()
		{
			var model = new viewModel
			{
				user = new User(),
				roles = _db.Roles.ToList(),
				SelectedIds = new List<int>()
			};
			if (model.roles != null)
			{
				return View(model);
			}
			return RedirectToAction("Error");
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Create(viewModel model)
		{
			if (model == null)
			{
				return Json(new { success = false, errors = new[] { "User not found" } });
			}
			if (string.IsNullOrEmpty(model.user.Uname) || string.IsNullOrEmpty(model.user.Upass))
			{
				return Json(new { success = false, errors = new[] { "All feilds must be filled" } });
			}
			if (model.user.Uname == model.user.Upass)
			{
				return Json(new { success = false, errors = new[] { "User name and password cannot be same" } });
			}
			if (!model.SelectedIds.Any() || model.SelectedIds == null)
			{
				return Json(new { success = false, errors = new[] { "User must have atleast one role" } });
			}
			if (ModelState.IsValid)
			{
				var roles = _db.Roles
			   .Where(r => model.SelectedIds.Contains(r.Rid))
			   .ToList();

				model.user.Rids = roles;

				_db.Users.Add(model.user);
				_db.SaveChanges();
				return Json(new { success = true });
			}
			model.roles = _db.Roles.ToList();
			return Json(new { success = false });
		}

		//Get
		public IActionResult Edit(int? id)
		{
			if (id == null || id == 0)
			{
				return NotFound();
			}
			var userForm = _db.Users.Where(u => u.Uid == id).Include(u => u.Rids).FirstOrDefault();
			if (userForm == null)
			{
				return NotFound();
			}
			viewModel model = new viewModel
			{
				user = userForm,
				roles = _db.Roles.ToList(),
				SelectedIds = userForm.Rids.Select(r => r.Rid).ToList()

			};
			if (model.user == null)
			{
				ViewBag.ErrorMessage = "User not found. Please ensure the user ID is correct.";
				return View("Error");
			}
			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]

		public IActionResult Edit(viewModel model)
		{
			if (model.user == null)
			{
				return Json(new { success = false, errors = new[] { "User Not Found" } });
			}
			if (string.IsNullOrEmpty(model.user.Uname) || string.IsNullOrEmpty(model.user.Upass))
			{
				return Json(new { success = false, errors = new[] { "All feilds must be filled" } });
			}
			if (model.user.Uname == model.user.Upass)
			{
				return Json(new { success = false, errors = new[] { "User's Name and Password can't be same" } });
			}
			if (!model.SelectedIds.Any() || model.SelectedIds == null)
			{
				return Json(new { success = false, errors = new[] { "A least one role must be selected" } });
			}

			if (!ModelState.IsValid)
			{
				return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)) });
			}

			var roles = _db.Roles
			   .Where(r => model.SelectedIds.Contains(r.Rid))
			   .ToList();

			var existingUser = _db.Users
				.Include(u => u.Rids)
				.FirstOrDefault(u => u.Uid == model.user.Uid);

			if (existingUser == null)
			{
				return Json(new { success = false, errors = new[] { "User not found in the database" } });
			}

			existingUser.Rids.Clear();
			existingUser.Rids = roles;
			existingUser.Uname = model.user.Uname;
			existingUser.Upass = model.user.Upass;

			if ((existingUser.Upass == null || existingUser.Upass != model.user.Upass) || (existingUser.Uname == null || existingUser.Uname != model.user.Uname))
			{
				return Json(new { success = false, errors = new[] { "Something went wrong while updating Name/Password" } });
			}
			_db.Users.Update(existingUser);
			_db.SaveChanges();
			return Json(new { success = true });

		}

		public IActionResult Delete(int? id)
		{
			if (id == null || id == 0)
			{
				return NotFound();
			}
			var userDb = _db.Users.Where(u => u.Uid == id)
				.Include(u => u.Rids).FirstOrDefault();
			if (userDb == null)
			{
				return NotFound();
			}
			viewModel model = new viewModel
			{
				user = userDb,
				roles = _db.Roles.ToList(),
				SelectedIds = userDb.Rids.Select(r => r.Rid).ToList()
			};
			if (model == null)
			{
				ViewBag.ErrorMessage = "User not found. Please ensure the user ID is correct.";
				return View("Error");
			}
			return View(model);
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Delete(viewModel model)
		{
			if (model == null)
			{
				return Json(new { success = false, errors = new[] { "No user found" } });
			}

			var existingUser = _db.Users
				.Include(u => u.Rids)
				.FirstOrDefault(u => u.Uid == model.user.Uid);

			if (existingUser == null)
			{
				return Json(new { success = false, error = new[] { "User not found." } });
			}

			existingUser.Rids.Clear();
			_db.Users.Remove(existingUser);
			_db.SaveChanges();

			var deletedUser = _db.Users.Find(existingUser.Uid);
			if (deletedUser == null)
			{
				return Json(new { success = true });
			}
			return Json(new { success = false });
		}

		public IActionResult GetUserDetails(int? id)
		{
			if (id == 0 || id == null)
			{
				return Json(new { success = false, errors = new[] { "user not found" } });
			}
			var userDb = _db.Users
				.Where(u => u.Uid == id)
				.Include(u => u.Rids)
				.FirstOrDefault();
			if (userDb == null)
			{
				return Json(new { success = false, errors = new[] { "user not found" } });
			}
			viewModel model = new viewModel
			{
				user = userDb,
				roles = _db.Roles.ToList()
			};

			return PartialView("_userDetailsPopUp", model);
		}

	}
}
