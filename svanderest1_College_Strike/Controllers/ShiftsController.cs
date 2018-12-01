using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using svanderest1_College_Strike.Data;
using svanderest1_College_Strike.Models;

namespace svanderest1_College_Strike.Controllers
{
    [Authorize(Roles ="Admin,Steward")]
    public class ShiftsController : Controller
    {
        private readonly svanderest1_College_StrikeContext _context;

        public ShiftsController(svanderest1_College_StrikeContext context)
        {
            _context = context;
        }

        // GET: Shifts
        public async Task<IActionResult> Index()
        {
            var finalProjectContext = _context.Shift.Include(s => s.Assignment).Include(s => s.Member);
            return View(await finalProjectContext.ToListAsync());
        }

        // GET: Shifts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shift = await _context.Shift
                .Include(s => s.Assignment)
                .Include(s => s.Member)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (shift == null)
            {
                return NotFound();
            }

            return View(shift);
        }

        // GET: Shifts/Create
        public IActionResult Create()
        {
            PopulateDropDownLists();
            return View();
        }

        // POST: Shifts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Date,MemberID,AssignmentID")] Shift shift)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(shift);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateException dex)
            {
                if (dex.InnerException.Message.Contains("IX"))
                {
                    ModelState.AddModelError("Date", "Unable to save changes. You have already assigned this member to a shift on this date.");
                }
                else
                {
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
                }
            }
            PopulateDropDownLists(shift);
            return View(shift);
        }

        // GET: Shifts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shift = await _context.Shift.FindAsync(id);
            if (shift == null)
            {
                return NotFound();
            }
            PopulateDropDownLists(shift);
            return View(shift);
        }

        // POST: Shifts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Byte[] RowVersion)// [Bind("ID,Date,MemberID,AssignmentID")] Shift shift)
        {
            var shift = await _context.Shift.SingleOrDefaultAsync(s => s.ID == id);
            if (shift == null)
            {
                return NotFound();
            }

            if (await TryUpdateModelAsync(shift, "", s => s.Date, s => s.MemberID, s => s.AssignmentID))
            {
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    var exceptionEntry = ex.Entries.Single();
                    var clientValues = (Shift)exceptionEntry.Entity;
                    var databaseEntry = exceptionEntry.GetDatabaseValues();
                    if (databaseEntry == null)
                    {
                        ModelState.AddModelError("",
                            "Unable to save changes. The Patient was deleted by another user.");
                    }
                    else
                    {
                        var databaseValues = (Shift)databaseEntry.ToObject();
                        if (databaseValues.Date != clientValues.Date)
                            ModelState.AddModelError("Date", "Current value: "
                                + databaseValues.Date);
                        if (databaseValues.MemberID != clientValues.MemberID)
                        {
                            Member databaseMember = await _context.Member.SingleOrDefaultAsync(i => i.ID == databaseValues.MemberID);
                            ModelState.AddModelError("DoctorID", $"Current value: {databaseMember?.FullName}");
                        }
                        if (databaseValues.AssignmentID != clientValues.AssignmentID)
                        {
                            Assignment databaseAssignment = await _context.Assignment.SingleOrDefaultAsync(i => i.ID == databaseValues.AssignmentID);
                            ModelState.AddModelError("DoctorID", $"Current value: {databaseAssignment?.Name}");
                        }
                        ModelState.AddModelError(string.Empty, "The record you attempted to edit "
                                + "was modified by another user after you received your values. The "
                                + "edit operation was canceled and the current values in the database "
                                + "have been displayed. If you still want to save your version of this record, click "
                                + "the Save button again. Otherwise click the 'Back to List' hyperlink.");
                        shift.RowVersion = (byte[])databaseValues.RowVersion;
                        ModelState.Remove("RowVersion");
                    }
                }
                catch (DbUpdateException dex)
                {
                    if (dex.InnerException.Message.Contains("IX"))
                    {
                        ModelState.AddModelError("Date", "Unable to save changes. You have already assigned this member to a shift on this date.");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
                    }
                }
            }
            PopulateDropDownLists(shift);
            return View(shift);
        }

        // GET: Shifts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shift = await _context.Shift
                .Include(s => s.Assignment)
                .Include(s => s.Member)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (shift == null)
            {
                return NotFound();
            }

            return View(shift);
        }

        // POST: Shifts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var shift = await _context.Shift.FindAsync(id);
            try
            {
                _context.Shift.Remove(shift);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            PopulateDropDownLists(shift);
            return View(shift);
        }

        private bool ShiftExists(int id)
        {
            return _context.Shift.Any(e => e.ID == id);
        }

        private void PopulateDropDownLists(Shift shift = null)
        {
            var aQuery = from a in _context.Assignment
                         orderby a.Name
                         select a;
            var mQuery = from m in _context.Member
                         orderby m.LastName, m.FirstName
                         select m;
            ViewData["AssignmentID"] = new SelectList(aQuery, "ID", "Name", shift?.AssignmentID);
            ViewData["MemberID"] = new SelectList(mQuery, "ID", "FullName", shift?.MemberID);
        }
    }
}
