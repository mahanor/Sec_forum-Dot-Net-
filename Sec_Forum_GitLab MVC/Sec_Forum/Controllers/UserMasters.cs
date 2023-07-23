using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Sec_Forum.Models;
using System.Security.Cryptography;
using Humanizer;
using Microsoft.AspNetCore.Http.HttpResults;
using MySqlX.XDevAPI;
using static System.Web.Razor.Parser.SyntaxConstants;

namespace Sec_Forum.Controllers
{
    public class UserMasters : Controller
    {
        private readonly SecForumContext _context;
        string uid = Guid.NewGuid().ToString();
        public UserMasters(SecForumContext context)
        {
            _context = context;
        }

        // GET: UserMasters
        public async Task<IActionResult> Index()
        {
            return _context.TblUserMasters != null ?
                        View(await _context.TblUserMasters.ToListAsync()) :
                        Problem("Entity set 'SecForumContext.TblUserMasters'  is null.");

        }

        [HttpGet]
        public ActionResult Login()
        {
            ClaimsPrincipal claimUser = HttpContext.User;
            if (claimUser.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");

            TblUserMaster _UserMaster = new TblUserMaster();
            return View(_UserMaster);
        }


        [HttpPost]
        public async Task<IActionResult> Login(TblUserMaster _UserMaster)
        {
            if (string.IsNullOrEmpty(_UserMaster.Username) || string.IsNullOrEmpty(_UserMaster.Password))
            {
                TempData["ErrorMessage"] = "Please Enter Username and Password.";
            }
            else
            {


                //using var sha256 = SHA256.Create();

                //// Convert the plain-text password into a byte array
                //var passwordBytes = Encoding.UTF8.GetBytes(_UserMaster.Password);


                //// Compute the hash value for the password
                //var hashBytes = sha256.ComputeHash(passwordBytes);


                //// Convert the hash value back to a string and store it in the database
                //var hashedPassword = Convert.ToBase64String(hashBytes);


                //_UserMaster.Password = hashedPassword;

                // Proceed with login
                // Create a new instance of the SHA256 hash algorithm
                using var sha256 = SHA256.Create();

                // Convert the user-entered password into a byte array
                var passwordBytes = Encoding.UTF8.GetBytes(_UserMaster.Password);

                // Compute the hash value for the user-entered password
                var hashBytes = sha256.ComputeHash(passwordBytes);

                var password = Convert.ToBase64String(hashBytes);

                //var UserName = _dbfitunContext.TblUserMasters.Where(m => m.Username);
                var status = await _context.TblUserMasters.FirstOrDefaultAsync(m => m.Username == _UserMaster.Username && m.Password == password);

                if (status == null)
                {
                    TempData["ErrorMessage"] = "Please check Username and Password.";


                }
                else
                {
                    // Set the Name property of the ClaimsIdentity object to a unique value
                    var identity = new ClaimsIdentity(new[] {
                            new Claim(ClaimTypes.NameIdentifier, _UserMaster.Id.ToString()),
                            new Claim(ClaimTypes.Name, _UserMaster.Username)
                        }, "ApplicationCookie");

                    // Set the authentication cookie with the updated ClaimsIdentity object
                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity)).Wait();


                    HttpContext.Session.SetString("Username", status.Username);
                    HttpContext.Session.SetString("Designation", status.Designation);
                    HttpContext.Session.SetString("User_uid", status.UId);
                    HttpContext.Session.SetString("User_Mobile", status.MobileNumber);
                    HttpContext.Session.SetString("ProfileImage", status.ProfileImage);
                    HttpContext.Session.SetInt32("Id", status.Id);

                    ViewBag.Designation = HttpContext.Session.GetString("Designation");
                    ViewBag.ProfileImage = HttpContext.Session.GetString("ProfileImage");
                    ViewBag.Name = HttpContext.Session.GetString("Username");
                    return RedirectToAction("HomePage", "HomePage");

                }
            }

            return View(_UserMaster);
        }


        // GET: UserMasters/Details/5
        public async Task<IActionResult> Details(int id)
        {
            if (id == null || _context.TblUserMasters == null)
            {
                return NotFound();
            }

            var tblUserMaster = await _context.TblUserMasters
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tblUserMaster == null)
            {
                return NotFound();
            }

            return View(tblUserMaster);
        }

        // GET: UserMasters/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: UserMasters/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,SeqId,OrgId,RoleId, Name,FirstName,LastName,MobileNumber,Email,Designation,ProfileImage,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate,Username,Password,ConfirmPassword")] TblUserMaster tblUserMaster, IFormFile imageFile)
        {
            // Create a new instance of the SHA256 hash algorithm
            using var sha256 = SHA256.Create();

            // Convert the plain-text password into a byte array
            var passwordBytes = Encoding.UTF8.GetBytes(tblUserMaster.Password);
            var passwordBytes1 = Encoding.UTF8.GetBytes(tblUserMaster.ConfirmPassword);

            // Compute the hash value for the password
            var hashBytes = sha256.ComputeHash(passwordBytes);
            var hashBytes1 = sha256.ComputeHash(passwordBytes1);

            // Convert the hash value back to a string and store it in the database
            var hashedPassword = Convert.ToBase64String(hashBytes);
            var hashedPassword1 = Convert.ToBase64String(hashBytes1);

            tblUserMaster.Password = hashedPassword;
            tblUserMaster.ConfirmPassword = hashedPassword1;
            var imageName = Path.GetFileName(imageFile.FileName);
            var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", imageName);

            using (var stream = new FileStream(imagePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }
            tblUserMaster.ProfileImage = "/images/" + imageName;
            tblUserMaster.CreatedDate = DateTime.Now;
            tblUserMaster.UId = uid;
            _context.Add(tblUserMaster);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "User login create successfully.";
            //return RedirectToAction(nameof(Create));

            return View(tblUserMaster);
        }

        public IActionResult AddSecProfile()
        {
            List<SelectListItem> Designation = new()
            {
                new SelectListItem { Value = "1", Text = "--Select--" },
                new SelectListItem { Value = "2", Text = "State Election Commissioner" },
                new SelectListItem { Value = "3", Text = "State Election Secretory" },
                new SelectListItem { Value = "4", Text = "State Election Deputy Commissioner" },
            };

            //assigning SelectListItem to view Bag
            ViewBag.Designation = Designation;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddSecProfile([Bind("Id,SeqId,OrgId,RoleId, Name,FirstName,LastName,MobileNumber,Email,Designation,DateOfBirth,Languages,EducationalQualification,FromDate,ToDate,ProfileImage,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate,Username,Password,ConfirmPassword")] TblUserMaster tblUserMaster, IFormFile imageFile)
        {
            // Create a new instance of the SHA256 hash algorithm
            using var sha256 = SHA256.Create();

            // Convert the plain-text password into a byte array
            var passwordBytes = Encoding.UTF8.GetBytes(tblUserMaster.Password);
            var passwordBytes1 = Encoding.UTF8.GetBytes(tblUserMaster.ConfirmPassword);

            // Compute the hash value for the password
            var hashBytes = sha256.ComputeHash(passwordBytes);
            var hashBytes1 = sha256.ComputeHash(passwordBytes1);

            // Convert the hash value back to a string and store it in the database
            var hashedPassword = Convert.ToBase64String(hashBytes);
            var hashedPassword1 = Convert.ToBase64String(hashBytes1);

            tblUserMaster.Password = hashedPassword;
            tblUserMaster.ConfirmPassword = hashedPassword1;
            var imageName = Path.GetFileName(imageFile.FileName);
            var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", imageName);

            using (var stream = new FileStream(imagePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }
            tblUserMaster.ProfileImage = "/images/" + imageName;
            tblUserMaster.CreatedDate = DateTime.Now;
            tblUserMaster.UId = uid;
            _context.Add(tblUserMaster);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "SEC profile create successfully.";
            //return RedirectToAction(nameof(Create));

            return View(tblUserMaster);
        }


        public async Task<IActionResult> EditCommissioner(int id)
        {
            //int id = 5;
            if (id == null || _context.TblUserMasters == null)
            {
                return NotFound();
            }

            var tblUserMaster = await _context.TblUserMasters.FindAsync(id);
            if (tblUserMaster == null)
            {
                return NotFound();
            }
            return View(tblUserMaster);
        }

        public async Task<IActionResult> EditDeputyCommissioner(int id)
        {
            //int id = 6;
            if (id == null || _context.TblUserMasters == null)
            {
                return NotFound();
            }

            var tblUserMaster = await _context.TblUserMasters.FindAsync(id);
            if (tblUserMaster == null)
            {
                return NotFound();
            }
            return View(tblUserMaster);
        }


        public async Task<IActionResult> EditSecretory(int id)
        {
            // int id = 7;
            if (id == null || _context.TblUserMasters == null)
            {
                return NotFound();
            }

            var tblUserMaster = await _context.TblUserMasters.FindAsync(id);
            if (tblUserMaster == null)
            {
                return NotFound();
            }
            return View(tblUserMaster);
        }


        // GET: UserMasters/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            int uid = (int)HttpContext.Session.GetInt32("Id");
            if (uid == null || _context.TblUserMasters == null)
            {
                return NotFound();
            }

            var tblUserMaster = await _context.TblUserMasters.FindAsync(uid);
            if (tblUserMaster == null)
            {
                return NotFound();
            }
            return View(tblUserMaster);
        }

        // POST: UserMasters/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,SeqId,OrgId,RoleId,Name,FirstName,LastName,MobileNumber,Email,Designation,ProfileImage,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate,UserName,Password")] TblUserMaster tblUserMaster)
        {
            if (id != tblUserMaster.Id)
            {
                return NotFound();
            }


            try
            {
                //var imageName = Path.GetFileName(imageFile.FileName);
                //var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", imageName);

                //using (var stream = new FileStream(imagePath, FileMode.Create))
                //{
                //    await imageFile.CopyToAsync(stream);
                //}
                //tblUserMaster.ProfileImage = "/images/" + imageName;
                _context.Update(tblUserMaster);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TblUserMasterExists(tblUserMaster.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));

            return View(tblUserMaster);
        }

        // GET: UserMasters/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            if (id == null || _context.TblUserMasters == null)
            {
                return NotFound();
            }

            var tblUserMaster = await _context.TblUserMasters
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tblUserMaster == null)
            {
                return NotFound();
            }

            return View(tblUserMaster);
        }

        // POST: UserMasters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.TblUserMasters == null)
            {
                return Problem("Entity set 'SecForumContext.TblUserMasters'  is null.");
            }
            var tblUserMaster = await _context.TblUserMasters.FindAsync(id);
            if (tblUserMaster != null)
            {
                _context.TblUserMasters.Remove(tblUserMaster);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TblUserMasterExists(int id)
        {
            return (_context.TblUserMasters?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        //public IActionResult GetSessionValues()
        //{
        //    var username = HttpContext.Session.GetString("Name");
        //    var designation = HttpContext.Session.GetString("Designation");
        //    return Ok($"Name: {username}, Designation: {designation}");
        //}
        [HttpGet]
        public async Task<IActionResult> Out()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "UserMasters");
        }

        [HttpPost]
        public async Task<IActionResult> CheckUsername(string username)
        {
            bool isUsernameTaken = await _context.TblUserMasters.AnyAsync(u => u.Username == username);
            return Json(!isUsernameTaken);
        }
    }










}
