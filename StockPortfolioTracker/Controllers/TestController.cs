using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StockPortfolioTracker.Data;
using StockPortfolioTracker.Models;

namespace StockPortfolioTracker.Controllers
{
    public class TestController : Controller
    {
        private readonly StockPortfolioTrackerContext _context;

        public TestController(StockPortfolioTrackerContext context)
        {
            _context = context;
        }

        // GET: Test
        public async Task<IActionResult> Index()
        {
            return _context.Test != null ?
                        View(await _context.Test.ToListAsync()) :
                        Problem("Entity set 'StockPortfolioTrackerContext.Test'  is null.");
        }

        // GET: Test/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Test == null)
            {
                return NotFound();
            }

            var test = await _context.Test
                .FirstOrDefaultAsync(m => m.Id == id);
            if (test == null)
            {
                return NotFound();
            }

            return View(test);
        }

        // GET: Test/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Test/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Text")] Test test)
        {
            if (ModelState.IsValid)
            {
                _context.Add(test);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(test);
        }

        // GET: Test/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Test == null)
            {
                return NotFound();
            }

            var test = await _context.Test.FindAsync(id);
            if (test == null)
            {
                return NotFound();
            }
            return View(test);
        }

        // POST: Test/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Text")] Test test)
        {
            if (id != test.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(test);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TestExists(test.Id))
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
            return View(test);
        }

        // GET: Test/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Test == null)
            {
                return NotFound();
            }

            var test = await _context.Test
                .FirstOrDefaultAsync(m => m.Id == id);
            if (test == null)
            {
                return NotFound();
            }

            return View(test);
        }

        // POST: Test/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Test == null)
            {
                return Problem("Entity set 'StockPortfolioTrackerContext.Test'  is null.");
            }
            var test = await _context.Test.FindAsync(id);
            if (test != null)
            {
                _context.Test.Remove(test);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TestExists(int id)
        {
          return (_context.Test?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
