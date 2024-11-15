using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LionTaskManagementApp.Data;
using LionTaskManagementApp.Models;
using Microsoft.AspNetCore.Authorization;
using LionTaskManagementApp.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using NuGet.Packaging.Signing;

namespace LionTaskManagementApp.Controllers
{
    public class TakerController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<TaskUser> _userManager;
        private readonly SignInManager<TaskUser> _signInManager;

        public TakerController(ApplicationDbContext context, UserManager<TaskUser> userManager, SignInManager<TaskUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [Authorize(Roles="Taker, Inactive_Taker, Admin")]    
        public IActionResult EditProfile()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles="Taker, Inactive_Taker, Admin")]  
        public async Task<IActionResult> EditProfileTaker(ContractorInfoViewModel model) 
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                // 1. Update ContractorInfo
                var contractorInfo = await _context.ContractorInfos.FirstOrDefaultAsync(c => c.UserId == user.Id);
                if (contractorInfo == null)
                {
                    contractorInfo = new ContractorInfo { UserId = user.Id };
                    _context.ContractorInfos.Add(contractorInfo);
                }
                contractorInfo.CostPerSqrFoot = model.PricePerSquareFoot;
                contractorInfo.ZipCode = model.ZipCode;
                contractorInfo.PreferenceDistance = model.PreferenceDistance;

                var fullAddr = model.FirstLine + " "
                                + model.SecondLine + " "
                                + model.City + " "
                                + model.StateProvince
                                + model.ZipCode;
                contractorInfo.FullAddress = fullAddr;
                contractorInfo.LatAndLongitude = model.LatAndLongitude;

                var roles = await _userManager.GetRolesAsync(user);
                var resultRemove = await _userManager.RemoveFromRolesAsync(user, roles.ToArray());
                var resultAdd = await _userManager.AddToRoleAsync(user, "Taker");
                await _context.SaveChangesAsync(); 
                await _context.Entry(user).ReloadAsync();
                await _signInManager.RefreshSignInAsync(user); 
                    
                TempData["SuccessMessage"] = "You are an active user now";
                TempData.Keep("SuccessMessage");
            }

            return RedirectToAction("Index", "Home"); 
        }

        // GET: Tasks
        [Authorize(Roles = "Taker,Admin")]
        public async Task<IActionResult> TakerIndex()
        {
            var currentUserId = User.Identity?.Name;
            var userTasks = await _context.Tasks
                                  .Where(t => t.OwnerId == currentUserId || t.Status.Equals(MyTaskStatus.Initialized))
                                  .ToListAsync();
            // return View(userTasks);
            return View(await _context.Tasks.ToListAsync());
        }

        // GET: Tasks/Details/5
        [Authorize(Roles = "Taker,Admin")]
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

        [Authorize(Roles = "Taker,Admin")]
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

        [Authorize(Roles="Taker, Admin")]  
        public IActionResult RequestTask(int taskId, bool doAccept)
        {
            // 1. Get the current user's ID
            string currentUserId = _userManager.GetUserId(User); 

            // 2. Check if the task exists
            var task = _context.Tasks.Find(taskId); // Assuming 'Tasks' is your DbSet
            if (task == null)
            {
                return NotFound(); // Or handle the error appropriately
            }

            var requestList = task.RequestList.Split(";").ToList();
            if (doAccept)
            {
                
                if (!requestList.Contains(currentUserId))
                {
                    requestList.Add(currentUserId);
                    task.RequestList = string.Join(";", requestList);
                }
            }
            else 
            {
                if (requestList.Remove(currentUserId)) 
                { 
                    task.RequestList = string.Join(";", requestList);
                }

                var deniedList = task.DeniedList.Split(";").ToList();
                if (!deniedList.Contains(currentUserId))
                {
                    deniedList.Add(currentUserId);
                    task.DeniedList = string.Join(";", deniedList);
                }
            }

            _context.SaveChanges();
            var RequestResultAlert = doAccept
                ? "The request is sent successfully, please wait for approval!"
                : "The task has been declined!";
            
            TempData["RequestResult"] = RequestResultAlert; 
            TempData.Keep("RequestResult");

            return RedirectToAction("TakerDetails", new {id = taskId });
        }

        [Authorize(Roles = "Taker, Admin")]
        public IActionResult ProcessToInProgress(int taskId, bool doAccept)
        {
            // 1. Get the current user's ID
            string currentUserId = _userManager.GetUserId(User);

            // 2. Check if the task exists
            var task = _context.Tasks.Find(taskId); // Assuming 'Tasks' is your DbSet
            if (task == null)
            {
                return NotFound(); // Or handle the error appropriately
            }

            
            if (doAccept)
            {
                task.Status = MyTaskStatus.InProgress.ToString();
            }
            else
            {
                var requestList = task.RequestList.Split(";").ToList();
                if (requestList.Remove(currentUserId))
                {
                    task.RequestList = string.Join(";", requestList);
                }

                var deniedList = task.DeniedList.Split(";").ToList();
                if (!deniedList.Contains(currentUserId))
                {
                    deniedList.Add(currentUserId);
                    task.DeniedList = string.Join(";", deniedList);
                }

                task.TakenById = string.Empty;
            }

            _context.SaveChanges();
            var RequestResultAlert = doAccept
                ? "Successfully started the task!"
                : "The task has been declined!";

            TempData["RequestResult"] = RequestResultAlert;
            TempData.Keep("RequestResult");

            return RedirectToAction("TakerDetails", new { id = taskId });
        }

        [Authorize(Roles = "Taker, Admin")]
        public IActionResult RequestProjectComplete(int taskId)
        {
            // 1. Get the current user's ID
            string currentUserId = _userManager.GetUserId(User);

            // 2. Check if the task exists
            var task = _context.Tasks.Find(taskId); // Assuming 'Tasks' is your DbSet
            if (task == null)
            {
                return NotFound(); // Or handle the error appropriately
            }

            task.Status = MyTaskStatus.PendingComplete.ToString();
            _context.SaveChanges();
            var RequestResultAlert = "Successfully requested task completion!";
            TempData["RequestResult"] = RequestResultAlert;
            TempData.Keep("RequestResult");

            return RedirectToAction("TakerDetails", new { id = taskId });
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
