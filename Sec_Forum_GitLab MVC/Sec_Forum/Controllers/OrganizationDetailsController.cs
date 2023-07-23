using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Sec_Forum.Models;

namespace Sec_Forum.Controllers
{
    public class OrganizationDetailsController : Controller
    {
        private readonly SecForumContext _context;
        string uid = Guid.NewGuid().ToString();
        public OrganizationDetailsController(SecForumContext context)
        {
            _context = context;
        }

        // GET: OrganizationDetails
        public async Task<IActionResult> Index()
        {
              return _context.TblOrganizationDetails != null ? 
                          View(await _context.TblOrganizationDetails.ToListAsync()) :
                          Problem("Entity set 'SecForumContext.TblOrganizationDetails'  is null.");
        }

        // GET: OrganizationDetails/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TblOrganizationDetails == null)
            {
                return NotFound();
            }

            var tblOrganizationDetail = await _context.TblOrganizationDetails
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tblOrganizationDetail == null)
            {
                return NotFound();
            }

            return View(tblOrganizationDetail);
        }

        // GET: OrganizationDetails/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: OrganizationDetails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,SeqId,OrgName,OrgCode,FieldName,OfficeAddress,EmailId,PhoneNo,RoleId,UserId,State,District,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate")] TblOrganizationDetail tblOrganizationDetail)
        {
           
                tblOrganizationDetail.CreatedDate = DateTime.Now;
                tblOrganizationDetail.UId = uid;
                _context.Add(tblOrganizationDetail);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            
            return View(tblOrganizationDetail);
        }

        // GET: OrganizationDetails/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.TblOrganizationDetails == null)
            {
                return NotFound();
            }

            var tblOrganizationDetail = await _context.TblOrganizationDetails.FindAsync(id);
            if (tblOrganizationDetail == null)
            {
                return NotFound();
            }
            return View(tblOrganizationDetail);
        }

        // POST: OrganizationDetails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
       // [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,SeqId,OrgName,OrgCode,FieldName,OfficeAddress,EmailId,PhoneNo,RoleId,UserId,State,District,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate")] TblOrganizationDetail tblOrganizationDetail)
        {
            if (id != tblOrganizationDetail.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tblOrganizationDetail);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TblOrganizationDetailExists(tblOrganizationDetail.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(tblOrganizationDetail);
        }

        // GET: OrganizationDetails/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.TblOrganizationDetails == null)
            {
                return NotFound();
            }

            var tblOrganizationDetail = await _context.TblOrganizationDetails
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tblOrganizationDetail == null)
            {
                return NotFound();
            }

            return View(tblOrganizationDetail);
        }

        // POST: OrganizationDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.TblOrganizationDetails == null)
            {
                return Problem("Entity set 'SecForumContext.TblOrganizationDetails'  is null.");
            }
            var tblOrganizationDetail = await _context.TblOrganizationDetails.FindAsync(id);
            if (tblOrganizationDetail != null)
            {
                _context.TblOrganizationDetails.Remove(tblOrganizationDetail);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TblOrganizationDetailExists(int id)
        {
          return (_context.TblOrganizationDetails?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
