using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.IdentityModel.Tokens;
using MVCTodoList.Data;
using MVCTodoList.Models;
using Microsoft.VisualBasic;

namespace MVCTodoList.Controllers
{
    public class ToDoItemsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public ToDoItemsController(ApplicationDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: ToDoItems
        public async Task<IActionResult> Index()
        {
            //Get the current users ID
            string userId = _userManager.GetUserId(User)!;


            //Get Todo from AppUser
            List<ToDoItem> todos = new List<ToDoItem>();

           todos = await _context.ToDoItem.Where(c=>c.AppUserID== userId).ToListAsync();


            //var applicationDbContext = _context.ToDoItem.Include(t => t.AppUser);
            return View(todos);
        }

        // GET: ToDoItems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ToDoItem == null)
            {
                return NotFound();
            }

            var toDoItem = await _context.ToDoItem
                .Include(t => t.AppUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (toDoItem == null)
            {
                return NotFound();
            }

            return View(toDoItem);
        }

        // GET: ToDoItems/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ToDoItems/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,AppUserID,TaskName,Created,DueDate,IsCompleted")] ToDoItem toDoItem)
        {
            ModelState.Remove("AppUserID");

            if (ModelState.IsValid)
            {
                toDoItem.AppUserID = _userManager.GetUserId(User);
                toDoItem.Created= DateTime.UtcNow;
                toDoItem.DueDate = DateTime.SpecifyKind(toDoItem.DueDate, DateTimeKind.Utc);
                toDoItem.IsCompleted = false;
                _context.Add(toDoItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(toDoItem);
        }

        // GET: ToDoItems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ToDoItem == null)
            {
                return NotFound();
            }

            var toDoItem = await _context.ToDoItem.FindAsync(id);
            if (toDoItem == null)
            {
                return NotFound();
            }
            ViewData["AppUserID"] = new SelectList(_context.Set<AppUser>(), "Id", "Id", toDoItem.AppUserID);
            return View(toDoItem);
        }

        // POST: ToDoItems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,AppUserID,TaskName,Created,DueDate,IsCompleted")] ToDoItem toDoItem)
        {
            if (id != toDoItem.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //Reformat Created Date
                    toDoItem.Created = DateTime.SpecifyKind(toDoItem.Created, DateTimeKind.Utc);
                    //Check if image was updated
              
                    //Reformat DueDate
                
                    toDoItem.DueDate = DateTime.SpecifyKind(toDoItem.DueDate, DateTimeKind.Utc);
                
                    _context.Update(toDoItem);
                    await _context.SaveChangesAsync();

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TodoExists(toDoItem.Id))
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
            ViewData["AppUserID"] = new SelectList(_context.Users, "Id", "Id", toDoItem.AppUserID);
            return View(toDoItem);
        }

        // GET: ToDoItems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ToDoItem == null)
            {
                return NotFound();
            }

            var toDoItem = await _context.ToDoItem
                .Include(t => t.AppUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (toDoItem == null)
            {
                return NotFound();
            }

            return View(toDoItem);
        }

        // POST: ToDoItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ToDoItem == null)
            {
                return Problem("Entity set 'ApplicationDbContext.ToDoItem'  is null.");
            }
            var toDoItem = await _context.ToDoItem.FindAsync(id);
            if (toDoItem != null)
            {
                _context.ToDoItem.Remove(toDoItem);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ToDoItemExists(int id)
        {
          return (_context.ToDoItem?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        private bool TodoExists(int id)
        {
            return (_context.ToDoItem?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
