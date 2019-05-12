using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Ogame.Data;
using Ogame.Models;

namespace Ogame.Controllers
{
    [Authorize]
    public class CapsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CapsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Caps
        public async Task<IActionResult> Index()
        {
            return View(await _context.Caps.ToListAsync());
        }

        // GET: Caps/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var caps = await _context.Caps
                .FirstOrDefaultAsync(m => m.CapsID == id);
            if (caps == null)
            {
                return NotFound();
            }

            return View(caps);
        }

        // GET: Caps/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Caps/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CapsID,Growth_factor,Metal_cap,Cristal_cap,Deuterium_cap,Energy_cap,Repair_factor")] Caps caps)
        {
            if (ModelState.IsValid)
            {
                _context.Add(caps);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(caps);
        }

        // GET: Caps/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var caps = await _context.Caps.FindAsync(id);
            if (caps == null)
            {
                return NotFound();
            }
            return View(caps);
        }

        // POST: Caps/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CapsID,Growth_factor,Metal_cap,Cristal_cap,Deuterium_cap,Energy_cap,Repair_factor")] Caps caps)
        {
            if (id != caps.CapsID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(caps);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CapsExists(caps.CapsID))
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
            return View(caps);
        }

        // GET: Caps/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var caps = await _context.Caps
                .FirstOrDefaultAsync(m => m.CapsID == id);
            if (caps == null)
            {
                return NotFound();
            }

            return View(caps);
        }

        // POST: Caps/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var caps = await _context.Caps.FindAsync(id);
            _context.Caps.Remove(caps);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CapsExists(int id)
        {
            return _context.Caps.Any(e => e.CapsID == id);
        }
    }
}
