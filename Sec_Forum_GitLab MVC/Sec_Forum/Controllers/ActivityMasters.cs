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
    public class ActivityMasters : Controller
    {
        private readonly SecForumContext _context;
        string uid = Guid.NewGuid().ToString();
        public ActivityMasters(SecForumContext context)
        {
            _context = context;
        }

        // GET: ActivityMasters
        public async Task<IActionResult> Index()
        {
              return _context.TblActivityMasters != null ? 
                          View(await _context.TblActivityMasters.ToListAsync()) :
                          Problem("Entity set 'SecForumContext.TblActivityMasters'  is null.");
        }

        // GET: ActivityMasters/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TblActivityMasters == null)
            {
                return NotFound();
            }

            var tblActivityMaster = await _context.TblActivityMasters
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tblActivityMaster == null)
            {
                return NotFound();
            }

            return View(tblActivityMaster);
        }

        // GET: ActivityMasters/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ActivityMasters/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UId,Likes,Comments,Share,Dislike,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate")] TblActivityMaster tblActivityMaster)
        {
            
                tblActivityMaster.CreatedDate = DateTime.Now;
                tblActivityMaster.UId = uid;
                _context.Add(tblActivityMaster);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            
            return View(tblActivityMaster);
        }

        // GET: ActivityMasters/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.TblActivityMasters == null)
            {
                return NotFound();
            }

            var tblActivityMaster = await _context.TblActivityMasters.FindAsync(id);
            if (tblActivityMaster == null)
            {
                return NotFound();
            }
            return View(tblActivityMaster);
        }

        // POST: ActivityMasters/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,SeqId,Likes,Comments,Share,Dislike,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate")] TblActivityMaster tblActivityMaster)
        {
            if (id != tblActivityMaster.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tblActivityMaster);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TblActivityMasterExists(tblActivityMaster.Id))
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
            return View(tblActivityMaster);
        }

        // GET: ActivityMasters/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.TblActivityMasters == null)
            {
                return NotFound();
            }

            var tblActivityMaster = await _context.TblActivityMasters
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tblActivityMaster == null)
            {
                return NotFound();
            }

            return View(tblActivityMaster);
        }

        // POST: ActivityMasters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.TblActivityMasters == null)
            {
                return Problem("Entity set 'SecForumContext.TblActivityMasters'  is null.");
            }
            var tblActivityMaster = await _context.TblActivityMasters.FindAsync(id);
            if (tblActivityMaster != null)
            {
                _context.TblActivityMasters.Remove(tblActivityMaster);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TblActivityMasterExists(int id)
        {
          return (_context.TblActivityMasters?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
