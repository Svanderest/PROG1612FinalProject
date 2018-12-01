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
    [Authorize]
    public class AssignmentsController : Controller
    {
        private readonly svanderest1_College_StrikeContext _context;

        public AssignmentsController(svanderest1_College_StrikeContext context)
        {
            _context = context;
        }

        // GET: Assignments
        public async Task<IActionResult> Index()
        {
            var context = _context.Assignment
                .Include(a => a.Members);                
            return View(await context.ToListAsync());
        }

        // GET: Assignments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var assignment = await _context.Assignment
                .Include(a => a.Members)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (assignment == null)
            {
                return NotFound();
            }

            return View(assignment);
        }

        // GET: Assignments/Create
        [Authorize(Roles = "Admin,Steward")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Assignments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Steward")]
        public async Task<IActionResult> Create([Bind("ID,Name")] Assignment assignment)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(assignment);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateException dex)
            {
                if (dex.InnerException.Message.Contains("IX"))
                {
                    ModelState.AddModelError("Name", "Unable to save changes. Remember, you cannot have duplicate assignment names.");
                }
                else
                {
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
                }
            }
            return View(assignment);
        }

        // GET: Assignments/Edit/5
        [Authorize(Roles = "Admin,Steward")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var assignment = await _context.Assignment.FindAsync(id);
            if (assignment == null)
            {
                return NotFound();
            }
            return View(assignment);
        }

        // POST: Assignments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Steward")]
        public async Task<IActionResult> Edit(int id, Byte[] RowVersion)//, [Bind("ID,Name")] Assignment assignment)
        {
            var assignment = await _context.Assignment.SingleOrDefaultAsync(a => a.ID == id);

            if (assignment == null)
            {
                return NotFound();
            }

            if (await TryUpdateModelAsync(assignment, "", a => a.Name))
            {
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    var exceptionEntry = ex.Entries.Single();
                    var clientValues = (Assignment)exceptionEntry.Entity;
                    var databaseEntry = exceptionEntry.GetDatabaseValues();
                    if (databaseEntry == null)
                    {
                        ModelState.AddModelError("",
                            "Unable to save changes. The Patient was deleted by another user.");
                    }
                    else
                    {
                        var databaseValues = (Assignment)databaseEntry.ToObject();
                        if (databaseValues.Name != clientValues.Name)
                            ModelState.AddModelError("Name", "Current value: " + databaseValues.Name);
                    }
                }
                catch (DbUpdateException dex)
                {
                    if (dex.InnerException.Message.Contains("IX"))
                    {
                        ModelState.AddModelError("Name", "Unable to save changes. Remember, you cannot have duplicate assignment names.");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
                    }
                }
            }
            return View(assignment);
        }

        // GET: Assignments/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var assignment = await _context.Assignment
                .FirstOrDefaultAsync(m => m.ID == id);
            if (assignment == null)
            {
                return NotFound();
            }

            return View(assignment);
        }

        // POST: Assignments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var assignment = await _context.Assignment.FindAsync(id);
            try
            {
                _context.Assignment.Remove(assignment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            return View(assignment);
        }

        private bool AssignmentExists(int id)
        {
            return _context.Assignment.Any(e => e.ID == id);
        }
    }
}
