using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LionTaskManagementApp.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using LionTaskManagementApp.Areas.Identity.Data;
using LionTaskManagementApp.Models;
using LionTaskManagementApp.Models.Poster;
using LionTaskManagementApp.Services;

namespace LionTaskManagementApp.Controllers
{
    public class PosterController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<TaskUser> _userManager;
        private readonly SignInManager<TaskUser> _signInManager;
        private readonly S3Service _s3Service;

        public PosterController(ApplicationDbContext context, UserManager<TaskUser> userManager, SignInManager<TaskUser> signInManager, S3Service s3Service)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _s3Service = s3Service;
        }

        [Authorize(Roles = "Poster,Admin")]
        public async Task<IActionResult> Index()
        {
            var currentUserId = User.Identity?.Name;
            var userTasks = await _context.Tasks
                                  .Where(t => t.OwnerId == currentUserId)
                                  .ToListAsync();
            return View(userTasks);
        }

        // GET: Tasks/Details/5
        public async Task<IActionResult> TaskDetails(int? id)
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

        [Authorize(Roles = "Poster,Admin")]
        public IActionResult TaskCreate()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Poster,Admin")]
        public async Task<IActionResult> TaskCreate(TaskModel taskModel)
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

                return View();
            }

            if (ModelState.IsValid)
            {
                // taskModel.DeniedList = string.Empty;  // Initialize DeniedList if it's required to be non-null
                // taskModel.TakenById = null;  // Set TakenById to null or another default value
                taskModel.CreatedTime = DateTimeOffset.UtcNow;  // Set the current timestamp for CreatedTime
                taskModel.Status = MyTaskStatus.Initialized.ToString();

                // Handle file uploads (if any)
                if (taskModel.WallPic != null && taskModel.WallPic.Length > 0)
                {
                    // 1. Create a temporary file
                    string tempFilePath = Path.GetTempFileName();

                    // 2. Save the uploaded file to the temporary location
                    using (var stream = new FileStream(tempFilePath, FileMode.Create))
                    {
                        await taskModel.WallPic.CopyToAsync(stream);
                    }

                    // 3. Upload to S3 from the temporary file
                    string key = $"wallpics/{Guid.NewGuid()}_{taskModel.WallPic.FileName}";
                    await _s3Service.UploadFileAsync(tempFilePath, key);

                    // 4. Get the pre-signed URL
                    string uploadedWallPicUrl = await _s3Service.GetPreSignedUrlAsync(key, TimeSpan.FromMinutes(10));
                    taskModel.WallPicKey = key;

                    // 5. Delete the temporary file
                    System.IO.File.Delete(tempFilePath);
                }

                if (taskModel.Artwork != null && taskModel.Artwork.Length > 0)
                {
                    // 1. Create a temporary file
                    string tempFilePath = Path.GetTempFileName();

                    // 2. Save the uploaded file to the temporary location
                    using (var stream = new FileStream(tempFilePath, FileMode.Create))
                    {
                        await taskModel.Artwork.CopyToAsync(stream);
                    }

                    // 3. Upload to S3 from the temporary file
                    string key = $"artworks/{Guid.NewGuid()}_{taskModel.Artwork.FileName}";
                    await _s3Service.UploadFileAsync(tempFilePath, key);

                    // 4. Get the pre-signed URL
                    string uploadedArtworkUrl = await _s3Service.GetPreSignedUrlAsync(key, TimeSpan.FromMinutes(10));
                    taskModel.ArtworkKey = key;

                    // 5. Delete the temporary file
                    System.IO.File.Delete(tempFilePath);
                }

                // Convert Deadline to UTC
                taskModel.Deadline = taskModel.Deadline.UtcDateTime;
                // Add the task to the context and save changes
                _context.Add(taskModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(taskModel);
        }

        [Authorize(Roles = "Poster,Admin")]
        // GET: Tasks/Edit/5
        public async Task<IActionResult> TaskEdit(int? id)
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

        // GET: Tasks/Delete/5
        [Authorize(Roles = "Poster,Admin")]
        public async Task<IActionResult> TaskDelete(int? id)
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

        // POST: Tasks/PosterDeleteConfirmedDelete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Poster,Admin")]
        public async Task<IActionResult> TaskDeleteConfirmed(int id)
        {
            var taskModel = await _context.Tasks.FindAsync(id);
            if (taskModel != null)
            {
                _context.Tasks.Remove(taskModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }



        [Authorize(Roles="Poster, Inactive_Poster, Admin")]    
        public IActionResult EditProfile()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles="Poster, Inactive_Poster, Admin")]   
        public async Task<IActionResult> EditProfilePoster(PosterInfoViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user != null)
                {
                    var modelToStore = new PosterInfo
                    {
                        PosterId = user.Id,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        PhoneNumber = model.PhoneNumber,
                        CompanyName = model.CompanyName,
                        AddressLine1 = model.AddressLine1,
                        AddressLine2 = model.AddressLine2,
                        City = model.City,
                        StateProvince = model.StateProvince,
                        Zipcode = model.Zipcode,
                        EIN = model.EIN,
                        IndustryInformation = model.IndustryInformation
                    };

                    _context.PosterInfos.Add(modelToStore);
                    await _context.SaveChangesAsync();

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
                else
                {
                    Console.WriteLine("User Not Exist");
                }
            }
            else 
            {
                Console.WriteLine("Model is not Valid");
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
        public async Task<IActionResult> ApproveRequest(int taskId, string username)
        {
            Console.WriteLine("ApproveRequest hit");

            if (username == null)
            {
                return NotFound();
            }

            var taskModel = await _context.Tasks.FindAsync(taskId);
            if (taskModel == null)
            {
                return NotFound();
            }

            taskModel.TakenById = username;

            _context.SaveChanges();
            return RedirectToAction("TaskDetails", "Poster", new { id = taskId });
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
            return RedirectToAction("TaskDetails", "Poster", new { id = taskId });
        }

        private bool TaskModelExists(int id)
        {
            return _context.Tasks.Any(e => e.Id == id);
        }
    }
}
