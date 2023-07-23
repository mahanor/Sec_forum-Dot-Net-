using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;
using Sec_Forum.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.FileProviders;

namespace Sec_Forum.Controllers
{
    public class TblAddPostDetailsController : Controller
    {
        private readonly SecForumContext _context;
        string uid = Guid.NewGuid().ToString();
        private readonly IWebHostEnvironment _webHostEnvironment;

        public TblAddPostDetailsController(SecForumContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }
    

        public async Task<IActionResult> Index(string searchString, string sortFilter, int page = 1)
        {
            /*  return _context.TblAddPostDetails != null ?
                          View(await _context.TblAddPostDetails.ToListAsync()) :
                          Problem("Entity set 'SecForumContext.TblAddPostDetails'  is null.");*/


            string User_uid = HttpContext.Session.GetString("User_uid");

            int pageSize = 5; // Number of posts to display per page

            var query = _context.TblAddPostDetails
                .Join(
                    _context.TblUserMasters,
                    addPost => addPost.UserUid,
                    user => user.UId,
                    (addPost, user) => new TblAddPost
                    {
                        TblAddPosts = addPost,
                        TblUser = user
                    }
                )
                .Select(joined => new ResultModel
                {
                    CreatedDate = (DateTime)joined.TblAddPosts.CreatedDate,
                    Id = joined.TblUser.Id,
                    UId = joined.TblUser.UId,
                    Name = joined.TblUser.Name,
                    ProfileImage = joined.TblUser.ProfileImage,
                    Designation = joined.TblUser.Designation,
                    ProjectBody = joined.TblAddPosts.ProjectBody,
                    UploadDocument = joined.TblAddPosts.UploadDocument,
                    PostUid = joined.TblAddPosts.UId,
                    Tags = joined.TblAddPosts.Tags,
                    ProjectTitle = joined.TblAddPosts.ProjectTitle,
                    tblpostid = joined.TblAddPosts.UId
                })
                .ToList(); // Fetch the initial project details

            if (!string.IsNullOrEmpty(searchString))
            {
                query = query.Where(item => item.Name.ToLower().Contains(searchString.ToLower()) || item.Name.ToUpper().Contains(searchString.ToUpper())).ToList();
            }
            if (sortFilter == "option2")
            {
                query = query.OrderByDescending(item => item.CreatedDate).ToList();
            }

            int totalItems = query.Count();
            int totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            query = query.Skip((page - 1) * pageSize).Take(pageSize).ToList(); // Apply paging

            ViewBag.TotalPages = totalPages;
            ViewBag.CurrentPage = page;

            return View(query);


        }

        public IActionResult Create()
        {
            return View();
        }

        // POST: TblAddPostDetails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,SeqId,ProjectTitle,ProjectBody,UploadDocument,UploadId,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate,Tags,Status")] TblAddPostDetail tblAddPostDetail, List<IFormFile> imageFile, IFormFile pdfFile)
        {


            //if (imageFile == null || imageFile.Length == 0)
            //{
            //    return BadRequest("Please select an image file to upload.");
            //}
            //if (imageFile.ContentType != "image/jpeg" && imageFile.ContentType != "image/jpg" && imageFile.ContentType != "image/png" && imageFile.ContentType != "image/gif")
            //{
            //    TempData["ErrorMessage"] = "Please upload a JPEG, JPG, PNG or GIF file.";
            //}

            //add modified date
            //var imageName = Path.GetFileName(imageFile.FileName);
            //var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", imageName);

            //using (var stream = new FileStream(imagePath, FileMode.Create))
            //{
            //    await imageFile.CopyToAsync(stream);
            //}

            if (imageFile != null)
            {
                if (imageFile.Count > 0)
                {
                    //if (imageFile.ContentType != "image/jpeg" && imageFile.ContentType != "image/jpg" && imageFile.ContentType != "image/png" && imageFile.ContentType != "image/gif")
                    //{
                    //    TempData["ErrorMessage"] = "Please upload a JPEG, JPG, PNG or GIF file.";
                    //}
                    //else
                    //{
                         List<string> imageFileNames = new List<string>();
                    foreach (var imageFiles in imageFile)
                    {
                        //Getting FileName
                        var fileName = Path.GetFileName(imageFiles.FileName);

                        //Assigning Unique Filename (Guid)
                        var myUniqueFileName = Convert.ToString(Guid.NewGuid());

                        //Getting file Extension
                        var fileExtension = Path.GetExtension(fileName);

                        // concatenating  FileName + FileExtension
                        var newFileName = String.Concat(myUniqueFileName, fileExtension);

                        // Combines two strings into a path.
                        var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", newFileName);

                        using (FileStream fs = System.IO.File.Create(imagePath))
                        {
                            imageFiles.CopyTo(fs);
                            fs.Flush();
                        }
                        imageFileNames.Add("/images/" + fileName);
                        tblAddPostDetail.UploadDocument = string.Join(",", imageFileNames);
                        tblAddPostDetail.CreatedDate = DateTime.Now;
                    }
                    //}
                    //add list
                   

                    if (pdfFile != null && pdfFile.Length > 0)
                    {
                        string filePath = Path.Combine(
                            Directory.GetCurrentDirectory(), "wwwroot/files",
                            pdfFile.FileName);

                        var fileName = Path.GetFileName(pdfFile.FileName);

                        using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            pdfFile.CopyTo(fileStream);
                        }
                        tblAddPostDetail.UploadFile = ("/files/" + fileName);
                    }


                    if (ModelState.IsValid)
                    {
                        tblAddPostDetail.UId = uid;
                        tblAddPostDetail.UserUid = HttpContext.Session.GetString("User_uid");
                        tblAddPostDetail.CreatedBy = HttpContext.Session.GetString("User_Mobile");
                        _context.Add(tblAddPostDetail);
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                }
            }
            return View();
        }

        // GET: TblAddPostDetails/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.TblAddPostDetails == null)
            {
                return NotFound();
            }

            var tblAddPostDetail = await _context.TblAddPostDetails.FindAsync(id);
            if (tblAddPostDetail == null)
            {
                return NotFound();
            }
            return View(tblAddPostDetail);
        }

        // POST: TblAddPostDetails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(string id, [Bind("Id,SeqId,ProjectTitle,ProjectBody,UploadDocument,UploadId,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate,Tags,Status")] TblAddPostDetail tblAddPostDetail)
        //{
        //    if (id != tblAddPostDetail.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(tblAddPostDetail);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!TblAddPostDetailExists(tblAddPostDetail.Id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(tblAddPostDetail);
        //}

        // GET: TblAddPostDetails/Delete/5
        //public async Task<IActionResult> Delete(string id)
        //{
        //    if (id == null || _context.TblAddPostDetails == null)
        //    {
        //        return NotFound();
        //    }

        //    var tblAddPostDetail = await _context.TblAddPostDetails
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (tblAddPostDetail == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(tblAddPostDetail);
        //}

        // POST: TblAddPostDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        // [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.TblAddPostDetails == null)
            {
                return Problem("Entity set 'SecForumContext.TblAddPostDetails'  is null.");
            }
            var tblAddPostDetail = await _context.TblAddPostDetails.FindAsync(id);
            if (tblAddPostDetail != null)
            {
                _context.TblAddPostDetails.Remove(tblAddPostDetail);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TblAddPostDetailExists(int id)
        {
            return (_context.TblAddPostDetails?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        // GET: UserMasters/Details/5
        public async Task<IActionResult> Details(int id)
        {
            if (id == null || _context.TblAddPostDetails == null)
            {
                return NotFound();
            }

            var tblAddPostDetail = await _context.TblAddPostDetails
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tblAddPostDetail == null)
            {
                return NotFound();
            }

            return View(tblAddPostDetail);
        }

        public IActionResult DownloadPdf(string UploadFile)
        {
            // Get the file path of the PDF file
            //string filePath = Path.Combine(_webHostEnvironment.WebRootPath, "files", "path_to_your_pdf_file.pdf");

            // Set the content type to application/pdf
            string contentType = "application/pdf";

            // Return the file as a FileStreamResult
            return File(new FileStream(UploadFile, FileMode.Open, FileAccess.Read), contentType);
        }



    }
}
