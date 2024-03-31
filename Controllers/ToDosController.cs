using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ToDo.Data;
using ToDo.Models;

namespace ToDo.Controllers
{
    [Authorize]
    public class ToDosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ToDosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ToDos
        public async Task<IActionResult> Index()
        {
              return _context.ToDos != null ? 
                          View(await _context.ToDos.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.ToDos'  is null.");
        }

        // GET: ToDos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ToDos == null)
            {
                return NotFound();
            }

            var toDos = await _context.ToDos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (toDos == null)
            {
                return NotFound();
            }

            return View(toDos);
        }

        // GET: ToDos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ToDos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,IsCompleted,DueDate,Categories,PriorityLevel")] ToDos toDos)
        {
            if (ModelState.IsValid)
            {
                _context.Add(toDos);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(toDos);
        }

        // GET: ToDos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ToDos == null)
            {
                return NotFound();
            }

            var toDos = await _context.ToDos.FindAsync(id);
            if (toDos == null)
            {
                return NotFound();
            }
            return View(toDos);
        }

        // POST: ToDos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,IsCompleted,DueDate,Categories,PriorityLevel")] ToDos toDos)
        {
            if (id != toDos.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(toDos);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ToDosExists(toDos.Id))
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
            return View(toDos);
        }
        // GET: ToDos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ToDos == null)
            {
                return NotFound();
            }
            var toDos = await _context.ToDos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (toDos == null)
            {
                return NotFound();
            }

            return View(toDos);
        }
        // POST: ToDos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ToDos == null)
            {
                return Problem("Entity set 'ApplicationDbContext.ToDos'  is null.");
            }
            var toDos = await _context.ToDos.FindAsync(id);
            if (toDos != null)
            {
                _context.ToDos.Remove(toDos);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        private bool ToDosExists(int id)
        {
          return (_context.ToDos?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
