using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LionTaskManagementApp.Data;
using LionTaskManagementApp.Models;
using Microsoft.AspNetCore.Authorization;

namespace LionTaskManagementApp.Controllers
{
    public class TasksController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TasksController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Tasks
        public async Task<IActionResult> Index()
        {
            return View(await _context.Task.ToListAsync());
        }

        [Authorize(Roles="Poster,Admin")]
        public async Task<IActionResult> PosterIndex()
        {
            var currentUserId = User.Identity?.Name;
            var userTasks = await _context.Task
                                  .Where(t => t.OwnerId == currentUserId)
                                  .ToListAsync();
            return View(userTasks);
        }

        // GET: Tasks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taskModel = await _context.Task
                .FirstOrDefaultAsync(m => m.Id == id);
            if (taskModel == null)
            {
                return NotFound();
            }

            return View(taskModel);
        }

        [Authorize(Roles="Poster,Admin")]
        // GET: Tasks/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Tasks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles="Poster,Admin")]
        public async Task<IActionResult> Create([Bind("OwnerId,Title,Description,Length,Height,Location")] TaskModel taskModel)
        {
            ModelState.Remove("Status");
            if (!ModelState.IsValid)
            {
                foreach (var modelState in ModelState.Values)
                {
                    foreach (var error in modelState.Errors)
                    {
                        // Access error.ErrorMessage to see the specific validation error
                        Console.WriteLine($"Error: {error.ErrorMessage}"); 

                        // You can also inspect error.Exception if an exception was thrown
                    }
                }
            }

            if (ModelState.IsValid)
            {
                taskModel.Status = "Initialized";  // Set default status
                taskModel.DeniedList = null;  // Initialize DeniedList if it's required to be non-null
                taskModel.TakenById = null;  // Set TakenById to null or another default value
                taskModel.CreatedTime = DateTimeOffset.UtcNow;  // Set the current timestamp for CreatedTime

                // Add the task to the context and save changes
                _context.Add(taskModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(PosterIndex));
            }

            return View(taskModel);
        }   


        [Authorize(Roles="Poster,Admin")]
        // GET: Tasks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taskModel = await _context.Task.FindAsync(id);
            if (taskModel == null)
            {
                return NotFound();
            }
            return View(taskModel);
        }


        // POST: Tasks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles="Poster,Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,OwnerId,Title,Description,Length,Height,DeniedList,Status,Location,TakenById,CreatedTime")] TaskModel taskModel)
        {
            if (id != taskModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(taskModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TaskModelExists(taskModel.Id))
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
            return View(taskModel);
        }

        // GET: Tasks/Delete/5
        [Authorize(Roles="Poster,Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taskModel = await _context.Task
                .FirstOrDefaultAsync(m => m.Id == id);
            if (taskModel == null)
            {
                return NotFound();
            }

            return View(taskModel);
        }

        // POST: Tasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles="Poster,Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var taskModel = await _context.Task.FindAsync(id);
            if (taskModel != null)
            {
                _context.Task.Remove(taskModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TaskModelExists(int id)
        {
            return _context.Task.Any(e => e.Id == id);
        }
    }
}
