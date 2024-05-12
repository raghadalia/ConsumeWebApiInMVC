using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ToDo.Authentication_Models;
using ToDo.Data;
using ToDo.Models;

namespace ToDo.Controllers
{

    public class ToDosController : Controller
    {
        Uri baseAddress = new Uri("https://localhost:44349/api/");
        private readonly HttpClient _client;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ToDosController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, HttpClient client)
        {
            _client = client;
            _client.BaseAddress = baseAddress;
            _context = context;
            _userManager = userManager;
        }
        private async Task<ApplicationUser> GetUserByIdAsync(string userId)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        }
        private Task<ApplicationUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);
        [Authorize(Roles = "User,Admin")]
        // GET: ToDos
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var response = await _client.GetAsync("ToDoApi/Index");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var userTasks = JsonConvert.DeserializeObject<List<ToDos>>(content);
                return View(userTasks);
            }
            else
            {
                return View("Error");
            }
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> All()
        {
            var toDos = await _context.ToDos.Include(t => t.User).ToListAsync();
            return View(toDos);
        }
        [Authorize(Roles = "User")]
        // GET: ToDos/Details/5
        [HttpGet("ToDos/Details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var response = await _client.GetAsync($"ToDoApi/Details/{id}");
            if (!response.IsSuccessStatusCode)
            {
                return NotFound();
            }

            var content = await response.Content.ReadAsStringAsync();
            var toDos = JsonConvert.DeserializeObject<ToDos>(content);

            return View(toDos);
        }
        [Authorize(Roles = "User")]
        // GET: ToDos/Create
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> Create(ToDos toDos)
        {
            try
            {
                    var data = JsonConvert.SerializeObject(toDos);
                    var content = new StringContent(data, Encoding.UTF8, "application/json");
                    var response = await _client.PostAsync("ToDoApi/Create", content);
                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        return View(toDos);
                    }
                
            }
            catch (Exception ex)
            {
             
                TempData["errorMessage"] = ex.Message;
                return View(toDos);
            }
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            ToDos toDos = new ToDos();
            HttpResponseMessage response = _client.GetAsync(_client.BaseAddress + "ToDoApi/Details/" + id).Result;

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                toDos = JsonConvert.DeserializeObject<ToDos>(data);
                return View(toDos);
            }
            else
            {
              
                return View("Error");
            }
        }

        // POST: ToDos/Edit/5
        [HttpPost("ToDos/Edit/{id}")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> Edit(int id, ToDos toDos)
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
                    var data = JsonConvert.SerializeObject(toDos);
                    var content = new StringContent(data, Encoding.UTF8, "application/json");
                    var response = _client.PutAsync(_client.BaseAddress + $"ToDoApi/Edit/{id}", content).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                    
                        return View(toDos);
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exception
                TempData["errorMessage"] = ex.Message;
                return View(toDos);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost("{id}")]
        [Authorize(Roles = "User")]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
               HttpResponseMessage response =  _client.DeleteAsync(_client.BaseAddress + $"ToDoApi/Delete/{id}" ).Result;
                if (response.IsSuccessStatusCode)
                {       
                    return RedirectToAction("Index");
                }
                else
                {
                    return View("Error"); 
                }
            }
            catch (Exception )
            { 
                return View("Error");
            }
        }

        private bool ToDosExists(int id)
        {
            return _context.ToDos.Any(e => e.Id == id);
        }
    }
}
