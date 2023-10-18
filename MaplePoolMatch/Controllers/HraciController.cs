using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MaplePoolMatch.Data;
using MaplePoolMatch.Models;
using Microsoft.AspNetCore.Authorization;

namespace MaplePoolMatch.Controllers
{
    [Authorize(Roles = UserRoles.Admin)]
    public class HraciController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HraciController(ApplicationDbContext context)
        {
            _context = context;
        }

        [AllowAnonymous]
        // GET: Hraci
        public async Task<IActionResult> Index()
        {
              return _context.Hraci != null ? 
                          View(await _context.Hraci.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Hraci'  is null.");
        }

        [AllowAnonymous]
        // GET: Hraci/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Hraci == null)
            {
                return NotFound();
            }

            var hraci = await _context.Hraci
                .FirstOrDefaultAsync(m => m.Id == id);
            if (hraci == null)
            {
                return NotFound();
            }

            return View(hraci);
        }

        // GET: Hraci/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Hraci/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Jmeno,Prijmeni,Email,Vyhry,Prohry,Uspesnost")] Hraci hraci)
        {
            if (ModelState.IsValid)
            {
                _context.Add(hraci);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(hraci);
        }

        // GET: Hraci/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Hraci == null)
            {
                return NotFound();
            }

            var hraci = await _context.Hraci.FindAsync(id);
            if (hraci == null)
            {
                return NotFound();
            }
            return View(hraci);
        }

        // POST: Hraci/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Jmeno,Prijmeni,Email,Vyhry,Prohry,Uspesnost")] Hraci hraci)
        {
            if (id != hraci.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(hraci);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HraciExists(hraci.Id))
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
            return View(hraci);
        }

        // GET: Hraci/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Hraci == null)
            {
                return NotFound();
            }

            var hraci = await _context.Hraci
                .FirstOrDefaultAsync(m => m.Id == id);
            if (hraci == null)
            {
                return NotFound();
            }

            return View(hraci);
        }

        // POST: Hraci/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Hraci == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Hraci'  is null.");
            }
            var hraci = await _context.Hraci.FindAsync(id);
            if (hraci != null)
            {
                _context.Hraci.Remove(hraci);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HraciExists(int id)
        {
          return (_context.Hraci?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
