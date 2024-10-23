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
    public class TakerController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TakerController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Tasks
        [Authorize(Roles="Taker,Admin")]
        public async Task<IActionResult> TakerIndex()
        {
            // var currentUserId = User.Identity?.Name;
            // var userTasks = await _context.Task
            //                       .Where(t => t.OwnerId == currentUserId)
            //                       .ToListAsync();
            // return View(userTasks);
            return View(await _context.Task.ToListAsync());
        }

        // GET: Tasks/Details/5
        [Authorize(Roles="Taker,Admin")]
        public async Task<IActionResult> TakerDetails(int? id)
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


        [Authorize(Roles="Taker,Admin")]
        // GET: Tasks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            Console.WriteLine("general editor hit");

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

        // [HttpPost]
        // [ValidateAntiForgeryToken]
        // [Authorize(Roles="Poster,Admin")]
        // public async Task<IActionResult> Edit(int id, [Bind("Id,OwnerId,Title,Description,Length,Height,DeniedList,Status,Location,TakenById,CreatedTime")] TaskModel taskModel)
        // {
        //     if (id != taskModel.Id)
        //     {
        //         return NotFound();
        //     }

        //     if (ModelState.IsValid)
        //     {
        //         try
        //         {
        //             _context.Update(taskModel);
        //             await _context.SaveChangesAsync();
        //         }
        //         catch (DbUpdateConcurrencyException)
        //         {
        //             if (!TaskModelExists(taskModel.Id))
        //             {
        //                 return NotFound();
        //             }
        //             else
        //             {
        //                 throw;
        //             }
        //         }
        //         return RedirectToAction(nameof(Index));
        //     }
        //     return View(taskModel);
        // }

        // POST: Tasks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        private bool TaskModelExists(int id)
        {
            return _context.Task.Any(e => e.Id == id);
        }
    }
}
