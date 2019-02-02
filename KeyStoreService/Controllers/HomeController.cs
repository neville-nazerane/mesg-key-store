using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KeyStoreService.Data;
using KeyStoreService.Models;

namespace KeyStoreService.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        const string existsMessage = "Alias already exists";

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Home
        public async Task<IActionResult> Index() => View(await _context.KeyInfos.ToListAsync());

        public string Show(int id) => _context.KeyInfos.Find(id)?.Key;

        // GET: Home/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var keyInfo = await _context.KeyInfos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (keyInfo == null)
            {
                return NotFound();
            }

            return View(keyInfo);
        }

        // GET: Home/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Home/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Alias,Key,Description")] KeyInfo keyInfo)
        {
            if (ModelState.IsValid)
            {
                if (_context.KeyInfos.Any(k => k.Alias == keyInfo.Alias))
                {
                    ModelState.AddModelError(nameof(KeyInfo.Alias), existsMessage);
                    return View(keyInfo);
                }
                _context.Add(keyInfo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(keyInfo);
        }

        // GET: Home/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var keyInfo = await _context.KeyInfos.FindAsync(id);
            if (keyInfo == null)
            {
                return NotFound();
            }
            return View(keyInfo);
        }

        // POST: Home/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Alias,Key,Description")] KeyInfo keyInfo)
        {
            if (id != keyInfo.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (_context.KeyInfos.Any(k => k.Alias == keyInfo.Alias && k.Id != keyInfo.Id))
                {
                    ModelState.AddModelError(nameof(KeyInfo.Alias), existsMessage);
                    return View(keyInfo);
                }
                try
                {
                    _context.Update(keyInfo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KeyInfoExists(keyInfo.Id))
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
            return View(keyInfo);
        }

        // GET: Home/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var keyInfo = await _context.KeyInfos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (keyInfo == null)
            {
                return NotFound();
            }

            return View(keyInfo);
        }

        // POST: Home/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var keyInfo = await _context.KeyInfos.FindAsync(id);
            _context.KeyInfos.Remove(keyInfo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool KeyInfoExists(int id)
        {
            return _context.KeyInfos.Any(e => e.Id == id);
        }
    }
}
