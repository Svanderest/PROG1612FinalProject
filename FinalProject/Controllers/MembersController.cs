using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FinalProject.Data;
using FinalProject.Models;
using FinalProject.ViewModels;
using Microsoft.EntityFrameworkCore.Storage;
using System.Linq.Expressions;

namespace FinalProject.Controllers
{
    public class MembersController : Controller
    {
        private readonly FinalProjectContext _context;

        public MembersController(FinalProjectContext context)
        {
            _context = context;
        }

        // GET: Members
        public async Task<IActionResult> Index(string searchString, int? AssignmentID, int? page, string actionButton, int sortDir = 1, string sortField = "First Name")
        {
            bool sortDirection = Convert.ToBoolean(sortDir);
            PopulateDropDownLists();
            var finalProjectContext = from m in _context.Member
                .Include(m => m.Assignment)
                .Include(m => m.Positions).ThenInclude(p => p.Position)
                select m;

            if (AssignmentID.HasValue)
                finalProjectContext = finalProjectContext.Where(m => m.AssignmentID == AssignmentID);

            if (!String.IsNullOrEmpty(searchString))
                finalProjectContext = finalProjectContext.Where(m => m.FirstName.ToUpper().Contains(searchString) || m.LastName.ToUpper().Contains(searchString));

            if (!String.IsNullOrEmpty(actionButton))
            {
                page = 1;
                if (actionButton != "Fileter")
                {
                    if (actionButton == sortField)
                        sortDirection = !sortDirection;
                    sortField = actionButton;
                }
            }
            switch (sortField)
            {
                case "First Name":
                    if (sortDirection)
                        finalProjectContext.OrderBy(m => m.FirstName);
                    else
                        finalProjectContext.OrderByDescending(m => m.FirstName);
                    break;
                case "Last Name":
                    if (sortDirection)
                        finalProjectContext.OrderBy(m => m.LastName);
                    else
                        finalProjectContext.OrderByDescending(m => m.LastName);
                    break;
                case "Phone":
                    if (sortDirection)
                        finalProjectContext.OrderBy(m => m.Phone);
                    else
                        finalProjectContext.OrderByDescending(m => m.Phone);
                    break;
                case "Email":
                    if (sortDirection)
                        finalProjectContext.OrderBy(m => m.eMail);
                    else
                        finalProjectContext.OrderByDescending(m => m.eMail);
                    break;
                case "Assignment":
                    if (sortDirection)
                        finalProjectContext.OrderBy(m => m.Assignment.Name);
                    else
                        finalProjectContext.OrderByDescending(m => m.Assignment.Name);
                    break;
            }
            ViewData["sortField"] = sortField;
            ViewData["sortDir"] = Convert.ToInt32(sortDirection);
            int pageSize = 1;
            var pagedData = await PaginatedList<Member>.CreateAsync(finalProjectContext.AsNoTracking(), page ?? 1, pageSize);
            return View(pagedData);
        }

        // GET: Members/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = await _context.Member
                .Include(m => m.Assignment)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (member == null)
            {
                return NotFound();
            }

            return View(member);
        }

        // GET: Members/Create
        public IActionResult Create()
        {
            var member = new Member();
            member.Positions = new List<MemberPosition>();
            PopulateAssignedPositionData(member);
            PopulateDropDownLists();
            return View();
        }

        // POST: Members/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,FirstName,LastName,Phone,eMail,AssignmentID")] Member member, string[] selectedPositions)
        {
            try
            {
                if (selectedPositions != null)
                {
                    member.Positions = new List<MemberPosition>();
                    foreach (var p in selectedPositions)
                    {
                        var posToAdd = new MemberPosition { MemberID = member.ID, PositionID = int.Parse(p) };
                        member.Positions.Add(posToAdd);
                    }
                }
                if (ModelState.IsValid)
                {
                    _context.Add(member);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (RetryLimitExceededException)
            {
                ModelState.AddModelError("", "Unable to save changes after multiple attempts. Try again, and if the problem persists, see your system administrator.");
            }
            catch (DbUpdateException dex)
            {
                if (dex.InnerException.Message.Contains("IX"))
                {
                    ModelState.AddModelError("eMail", "Unable to save changes. Remember, you cannot have duplicate emails.");
                }
                else
                {
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
                }
            }
            PopulateAssignedPositionData(member);
            PopulateDropDownLists(member);
            return View(member);
        }

        // GET: Members/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = await _context.Member
                .Include(m => m.Assignment)
                .Include(m => m.Positions).ThenInclude(p => p.Position)
                .AsNoTracking()
                .SingleOrDefaultAsync(m => m.ID == id);

            if (member == null)
            {
                return NotFound();
            }

            PopulateAssignedPositionData(member);
            PopulateDropDownLists(member);
            return View(member);
        }

        // POST: Members/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Byte[] RowVersion, string[] selectedPositions)//, [Bind("ID,FirstName,LastName,Phone,eMail,AssignmentID")] Member member)
        {
            var member = await _context.Member
                .Include(m => m.Assignment)
                .Include(m => m.Positions).ThenInclude(p => p.Position)
                .AsNoTracking()
                .SingleOrDefaultAsync(m => m.ID == id);

            if (member == null)
            {
                return NotFound();
            }
            UpdateMemberPositions(selectedPositions, member);

            if (await TryUpdateModelAsync<Member>(member,"",m => m.FirstName,m=>m.LastName, m=>m.Phone, m=>m.eMail, m=>m.AssignmentID))
            {
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (RetryLimitExceededException /* dex */)
                {
                    ModelState.AddModelError("", "Unable to save changes after multiple attempts. Try again, and if the problem persists, see your system administrator.");
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    var exceptionEntry = ex.Entries.Single();
                    var clientValues = (Member)exceptionEntry.Entity;
                    var databaseEntry = exceptionEntry.GetDatabaseValues();
                    if (databaseEntry == null)
                    {
                        ModelState.AddModelError("",
                            "Unable to save changes. The Patient was deleted by another user.");
                    }
                    else
                    {
                        var databaseValues = (Member)databaseEntry.ToObject();
                        if (databaseValues.FirstName != clientValues.FirstName)
                            ModelState.AddModelError("FirstName", "Current value: "
                                + databaseValues.FirstName);
                        if (databaseValues.LastName != clientValues.LastName)
                            ModelState.AddModelError("LastName", "Current value: "
                                + databaseValues.LastName);
                        if (databaseValues.Phone != clientValues.Phone)
                            ModelState.AddModelError("Phone", "Current value: "
                                + String.Format("{0:(###) ###-####}", databaseValues.Phone));
                        if (databaseValues.eMail != clientValues.eMail)
                            ModelState.AddModelError("eMail", "Current value: "
                                + databaseValues.eMail);
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
                        member.RowVersion = (byte[])databaseValues.RowVersion;
                        ModelState.Remove("RowVersion");
                    }
                }
                catch (DbUpdateException dex)
                {
                    if (dex.InnerException.Message.Contains("IX"))
                    {
                        ModelState.AddModelError("eMail", "Unable to save changes. Remember, you cannot have duplicate emails.");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
                    }
                }
            }
            PopulateAssignedPositionData(member);
            PopulateDropDownLists(member);
            return View(member);
        }

        // GET: Members/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = await _context.Member
                .Include(m => m.Assignment)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (member == null)
            {
                return NotFound();
            }

            return View(member);
        }

        // POST: Members/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var member = await _context.Member.FindAsync(id);
            try
            {
                _context.Member.Remove(member);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {                
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            return View(member);
        }

        private bool MemberExists(int id)
        {
            return _context.Member.Any(e => e.ID == id);
        }

        private void PopulateDropDownLists(Member member = null)
        {
            var aQuery = from a in _context.Assignment
                         orderby a.Name
                         select a;
            ViewData["AssignmentID"] = new SelectList(aQuery, "ID", "Name", member?.AssignmentID);
        }

        private void PopulateAssignedPositionData(Member member)
        {
            var allPositions = _context.Position;
            var mPositions = new HashSet<int>(member.Positions.Select(b => b.PositionID));
            var viewModel = new List<AssignedPositions>();
            foreach (var p in allPositions)
            {
                viewModel.Add(new AssignedPositions
                {
                    PositionID = p.ID,
                    PositionTitle = p.Title,
                    Assigned = mPositions.Contains(p.ID)
                });
            }
            ViewData["Positions"] = viewModel;
        }

        private void UpdateMemberPositions(string[] selectedPositions, Member memberToUpdate)
        {
            if (selectedPositions == null)
            {
                memberToUpdate.Positions = new List<MemberPosition>();
                return;
            }

            var selectedPositionsHS = new HashSet<string>(selectedPositions);
            var memberPos = new HashSet<int>
                (memberToUpdate.Positions.Select(c => c.PositionID));//IDs of the currently selected positions
            foreach (var p in _context.Position)
            {
                if (selectedPositionsHS.Contains(p.ID.ToString()))
                {
                    if (!memberPos.Contains(p.ID))
                    {
                        memberToUpdate.Positions.Add(new MemberPosition { MemberID = memberToUpdate.ID, PositionID = p.ID });
                    }
                }
                else
                {
                    if (memberPos.Contains(p.ID))
                    {
                        MemberPosition positionToRemove = memberToUpdate.Positions.SingleOrDefault(c => c.PositionID == p.ID);
                        _context.Remove(positionToRemove);
                    }
                }
            }
        }
    }
}
