using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LionTaskManagementApp.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using LionTaskManagementApp.Areas.Identity.Data;
using LionTaskManagementApp.Models;

namespace LionTaskManagementApp.Controllers
{
    public class PosterController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<TaskUser> _userManager;

        private readonly SignInManager<TaskUser> _signInManager;

        public PosterController(ApplicationDbContext context, UserManager<TaskUser> userManager, SignInManager<TaskUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [Authorize(Roles="Poster, Inactive_Poster, Admin")]    
        public IActionResult EditProfile()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles="Poster, Inactive_Poster, Admin")]   
        public async Task<IActionResult> EditProfilePoster()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                var roles = await _userManager.GetRolesAsync(user);
                var resultRemove = await _userManager.RemoveFromRolesAsync(user, roles.ToArray());
                var resultAdd = await _userManager.AddToRoleAsync(user, "Poster");

                if (resultRemove.Succeeded && resultAdd.Succeeded)
                {
                    await _context.SaveChangesAsync();    
                    
                    // 1. Refresh the user object from the database
                    await _context.Entry(user).ReloadAsync();

                    // 2. Update the user's claims principal 
                    await _signInManager.RefreshSignInAsync(user); 
                    
                    TempData["SuccessMessage"] = "You are an active user now";
                    TempData.Keep("SuccessMessage");
                }
                else
                {
                    // Handle role update failures (e.g., log errors, display a message)
                    foreach (var error in resultRemove.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    foreach (var error in resultAdd.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    // You might want to return to the edit view with error messages
                    return View(); // Or return View("YourEditViewName"); 
                }
            }

            
            return RedirectToAction("Index", "Home"); // Or wherever you want to redirect
        }

        // GET: Tasks
        [Authorize(Roles="Poster,Admin")]
        public async Task<IActionResult> TakerIndex()
        {
            // var currentUserId = User.Identity?.Name;
            // var userTasks = await _context.Task
            //                       .Where(t => t.OwnerId == currentUserId)
            //                       .ToListAsync();
            // return View(userTasks);
            return View(await _context.Tasks.ToListAsync());
        }

        // GET: Tasks/Details/5
        [Authorize(Roles="Poster,Admin")]
        public async Task<IActionResult> TakerDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taskModel = await _context.Tasks
                .FirstOrDefaultAsync(m => m.Id == id);
            if (taskModel == null)
            {
                return NotFound();
            }

            return View(taskModel);
        }


        [Authorize(Roles="Poster,Admin")]
        // GET: Tasks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            Console.WriteLine("general editor hit");

            if (id == null)
            {
                return NotFound();
            }

            var taskModel = await _context.Tasks.FindAsync(id);
            if (taskModel == null)
            {
                return NotFound();
            }
            return View(taskModel);
        }

        [Authorize(Roles = "Poster,Admin")]
        // GET: Poster/ApproveRequest
        public async Task<IActionResult> ApproveRequest(int taskId, string userId)
        {
            Console.WriteLine("ApproveRequest hit");

            if (userId == null)
            {
                return NotFound();
            }

            var taskModel = await _context.Tasks.FindAsync(taskId);
            if (taskModel == null)
            {
                return NotFound();
            }

            taskModel.TakenById = userId;

            _context.SaveChanges();
            return RedirectToAction("PosterDetails", "Tasks", new { id = taskId });
        }


        [Authorize(Roles = "Poster,Admin")]
        // GET: Poster/ApproveTask
        public async Task<IActionResult> ApproveComplete(int taskId, bool doApprove)
        {
            var taskModel = await _context.Tasks.FindAsync(taskId);
            if (taskModel == null)
            {
                return NotFound();
            }

            taskModel.Status = doApprove? MyTaskStatus.Completed.ToString(): MyTaskStatus.InProgress.ToString();

            _context.SaveChanges();
            return RedirectToAction("PosterDetails", "Tasks", new { id = taskId });
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
            return _context.Tasks.Any(e => e.Id == id);
        }
    }
}
