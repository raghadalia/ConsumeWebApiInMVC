using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDo.Authentication_Models;
using ToDo.Data;
using ToDo.Models;

namespace ToDo.Controllers
{
    
    public class ToDosController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ToDosController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        private Task<ApplicationUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);
		[Authorize(Roles = "User,Admin")]
		// GET: ToDos
		public async Task<IActionResult> Index()
        {
            var currentUser = await GetCurrentUserAsync();
            var userTasks = await _context.ToDos.Where(todo => todo.User.Id== currentUser.Id).ToListAsync();
            return View(userTasks);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> All()
        {
            var toDos = await _context.ToDos.Include(t => t.User).ToListAsync();
            return View(toDos);
        }
		[Authorize(Roles = "User")]
		// GET: ToDos/Details/5
		public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var toDos = await _context.ToDos.FirstOrDefaultAsync(m => m.Id == id);
            if (toDos == null)
            {
                return NotFound();
            }

            return View(toDos);
        }
		[Authorize(Roles = "User")]
		// GET: ToDos/Create
		public IActionResult Create()
        {
            return View();
        }
		[Authorize(Roles = "User")]
		[HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,IsCompleted,DueDate,Categories,PriorityLevel")] ToDos toDos)
        {
            var currentUser = await GetCurrentUserAsync();
            if (currentUser != null)
            {
                
                toDos.User= currentUser;
                _context.Add(toDos);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(toDos);
        }
		[Authorize(Roles = "User")]
		// GET: ToDos/Edit/5
		public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
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

        [HttpPost]
        [ValidateAntiForgeryToken]
		[Authorize(Roles = "User")]
		public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,IsCompleted,DueDate,Categories,PriorityLevel")] ToDos toDos)
        {
            if (id != toDos.Id)
            {
                return NotFound();
            }

            try
                {
                var currentUser = await GetCurrentUserAsync();
                if (currentUser != null)
                {

                    toDos.User = currentUser;
                    _context.Update(toDos);
                    await _context.SaveChangesAsync();
                }
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
		[Authorize(Roles = "User")]
		// GET: ToDos/Delete/5
		public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var toDos = await _context.ToDos.FirstOrDefaultAsync(m => m.Id == id);
            if (toDos == null)
            {
                return NotFound();
            }

            return View(toDos);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
		[Authorize(Roles = "User")]
		public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var toDos = await _context.ToDos.FindAsync(id);
            if (toDos != null)
            {
                _context.ToDos.Remove(toDos);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool ToDosExists(int id)
        {
            return _context.ToDos.Any(e => e.Id == id);
        }
    }
}
