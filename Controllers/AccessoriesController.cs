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

namespace MVCTodoList.Controllers
{
    public class AccessoriesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public AccessoriesController(ApplicationDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Accessories
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Accessory.Include(a => a.AppUser);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Accessories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Accessory == null)
            {
                return NotFound();
            }

            var accessory = await _context.Accessory
                .Include(a => a.AppUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (accessory == null)
            {
                return NotFound();
            }

            return View(accessory);
        }

        // GET: Accessories/Create
        public IActionResult Create()
        {
            ViewData["AppUserID"] = new SelectList(_context.Set<AppUser>(), "Id", "Id");
            return View();
        }

        // POST: Accessories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,AppUserID,AccessoryName")] Accessory accessory)
        {
            ModelState.Remove("AppUserID"); 

            if (ModelState.IsValid)
            {
                accessory.AppUserID = _userManager.GetUserId(User);
                accessory.AccessoryName = accessory.AccessoryName;

                _context.Add(accessory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            
            return View(accessory);
        }

        // GET: Accessories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Accessory == null)
            {
                return NotFound();
            }

            var accessory = await _context.Accessory.FindAsync(id);
            if (accessory == null)
            {
                return NotFound();
            }
            ViewData["AppUserID"] = new SelectList(_context.Set<AppUser>(), "Id", "Id", accessory.AppUserID);
            return View(accessory);
        }

        // POST: Accessories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,AppUserID,FirstName,LastName")] Accessory accessory)
        {
            if (id != accessory.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(accessory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AccessoryExists(accessory.Id))
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
            ViewData["AppUserID"] = new SelectList(_context.Set<AppUser>(), "Id", "Id", accessory.AppUserID);
            return View(accessory);
        }

        // GET: Accessories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Accessory == null)
            {
                return NotFound();
            }

            var accessory = await _context.Accessory
                .Include(a => a.AppUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (accessory == null)
            {
                return NotFound();
            }

            return View(accessory);
        }

        // POST: Accessories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Accessory == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Accessory'  is null.");
            }
            var accessory = await _context.Accessory.FindAsync(id);
            if (accessory != null)
            {
                _context.Accessory.Remove(accessory);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AccessoryExists(int id)
        {
          return (_context.Accessory?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
