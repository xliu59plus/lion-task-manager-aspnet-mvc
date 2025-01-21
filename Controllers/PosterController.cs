using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LionTaskManagementApp.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using LionTaskManagementApp.Areas.Identity.Data;
using LionTaskManagementApp.Models;
using LionTaskManagementApp.Models.Poster;
using LionTaskManagementApp.Services;
using LionTaskManagementApp.Utils;
using LionTaskManagementApp.Services.Hubs;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Razor.TagHelpers;
using LionTaskManagementApp.Models.Constants;

namespace LionTaskManagementApp.Controllers
{
    public class PosterController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<TaskUser> _userManager;
        private readonly SignInManager<TaskUser> _signInManager;
        private readonly S3Service _s3Service;
        private readonly NotificationHubService _notificationHubService;
        private readonly IServiceProvider _serviceProvider;
        private readonly PaymentService _paymentService;

        public PosterController(ApplicationDbContext context,
                                UserManager<TaskUser> userManager,
                                SignInManager<TaskUser> signInManager,
                                S3Service s3Service,
                                NotificationHubService notificationHubService,
                                IServiceProvider serviceProvider,
                                PaymentService paymentService)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _s3Service = s3Service;
            _notificationHubService = notificationHubService;
            _serviceProvider = serviceProvider;
            _paymentService = paymentService;
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

            taskModel.WallPicUrl = await _s3Service.GetPreSignedUrlAsync(taskModel.WallPicKey, TimeSpan.FromMinutes(10));
            taskModel.ArtworkUrl = await _s3Service.GetPreSignedUrlAsync(taskModel.ArtworkKey, TimeSpan.FromMinutes(10));
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
            ModelState.Remove("PaymentSessionId");
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
                taskModel.LastUpdatedTime = taskModel.CreatedTime;

                // Add the task to the context and save changes
                _context.Add(taskModel);
                await _context.SaveChangesAsync(); // please check whether the taskModel has an id.

                // Push Notification to TaskTakers.
                var currentUsername = User.Identity?.Name;
                var currUser = await _userManager.FindByNameAsync(currentUsername);
                var notificationBackgroundService = _serviceProvider.GetRequiredService<NotificationBackgroundService>();
                notificationBackgroundService.QueueNotification(taskModel, currUser.Id);

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

            taskModel.WallPicUrl = await _s3Service.GetPreSignedUrlAsync(taskModel.WallPicKey, TimeSpan.FromMinutes(10));
            taskModel.ArtworkUrl = await _s3Service.GetPreSignedUrlAsync(taskModel.ArtworkKey, TimeSpan.FromMinutes(10));

            return View(taskModel);
        }

        [Authorize(Roles = "Poster,Admin")]
        [HttpPost]
        public async Task<IActionResult> TaskEdit(TaskModel updatedTask)
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
                try
                {
                    // Fetch the existing task from the database
                    var existingTask = await _context.Tasks.FindAsync(updatedTask.Id);

                    if (existingTask == null)
                    {
                        return NotFound();
                    }

                    // Update the properties of the existing task with the values from taskModel
                    existingTask.Title = updatedTask.Title;
                    existingTask.Budget = updatedTask.Budget;
                    existingTask.Length = updatedTask.Length;
                    existingTask.Height = updatedTask.Height;
                    existingTask.Description = updatedTask.Description;
                    existingTask.FullAddress = updatedTask.FullAddress;
                    existingTask.FirstLine = updatedTask.FirstLine;
                    existingTask.SecondLine = updatedTask.SecondLine;
                    existingTask.StateProvince = updatedTask.StateProvince;
                    existingTask.City = updatedTask.City;
                    existingTask.ZipCode = updatedTask.ZipCode;
                    existingTask.Latitude = updatedTask.Latitude;
                    existingTask.Longitude = updatedTask.Longitude;
                    existingTask.Deadline = updatedTask.Deadline.UtcDateTime;
                    existingTask.ProjectResolution = updatedTask.ProjectResolution;
                    existingTask.IndoorOutdoor = updatedTask.IndoorOutdoor;
                    existingTask.WallType = updatedTask.WallType;
                    existingTask.DowngradeResolution = updatedTask.DowngradeResolution;

                    // Handle file uploads (if any)
                    if (updatedTask.WallPic != null && updatedTask.WallPic.Length > 0)
                    {
                        // 1. Create a temporary file
                        string tempFilePath = Path.GetTempFileName();

                        // 2. Save the uploaded file to the temporary location
                        using (var stream = new FileStream(tempFilePath, FileMode.Create))
                        {
                            await updatedTask.WallPic.CopyToAsync(stream);
                        }

                        // 3. Upload to S3 from the temporary file
                        string key = $"wallpics/{Guid.NewGuid()}_{updatedTask.WallPic.FileName}";
                        await _s3Service.UploadFileAsync(tempFilePath, key);

                        // 4. Get the pre-signed URL
                        string uploadedWallPicUrl = await _s3Service.GetPreSignedUrlAsync(key, TimeSpan.FromMinutes(10));
                        updatedTask.WallPicKey = key;

                        // 5. Delete the temporary file
                        System.IO.File.Delete(tempFilePath);
                    }

                    if (updatedTask.Artwork != null && updatedTask.Artwork.Length > 0)
                    {
                        // 1. Create a temporary file
                        string tempFilePath = Path.GetTempFileName();

                        // 2. Save the uploaded file to the temporary location
                        using (var stream = new FileStream(tempFilePath, FileMode.Create))
                        {
                            await updatedTask.Artwork.CopyToAsync(stream);
                        }

                        // 3. Upload to S3 from the temporary file
                        string key = $"artworks/{Guid.NewGuid()}_{updatedTask.Artwork.FileName}";
                        await _s3Service.UploadFileAsync(tempFilePath, key);

                        // 4. Get the pre-signed URL
                        string uploadedArtworkUrl = await _s3Service.GetPreSignedUrlAsync(key, TimeSpan.FromMinutes(10));
                        updatedTask.ArtworkKey = key;

                        // 5. Delete the temporary file
                        System.IO.File.Delete(tempFilePath);
                    }

                    // Save changes to the database
                    _context.Update(existingTask);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    Console.WriteLine("Something wrong when storing the picture into S3:" + ex.Message);
                }
            }

            // If ModelState is not valid, return to the edit view with validation errors
            return View(updatedTask);
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

            taskModel.WallPicUrl = await _s3Service.GetPreSignedUrlAsync(taskModel.WallPicKey, TimeSpan.FromMinutes(10));
            taskModel.ArtworkUrl = await _s3Service.GetPreSignedUrlAsync(taskModel.ArtworkKey, TimeSpan.FromMinutes(10));

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
        public async Task<IActionResult> CreateProfilePoster(PosterInfoViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user != null)
                {
                    var modelToStore = new PosterInfo
                    {
                        PosterId = user.Id,
                        FullName = model.FirstName + model.LastName,
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

                    var activationRequest = new ActivationRequest
                    {
                        UserId = user.Id,
                        UserName = user.UserName,
                        UserEmail = user.Email,
                        RequestedRole = RoleConstants.Poster,
                        IsApproved = false,
                        RequestTime = DateTimeOffset.UtcNow,
                        LastUpdateTime = DateTimeOffset.UtcNow,
                        DenyComents = string.Empty
                    };

                    _context.ActivationRequests.Add(activationRequest);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Successfully sent application for activation, please wait for approval.";
                    TempData.Keep("SuccessMessage");
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
        public async Task<IActionResult> ConfirmRequest(int taskId, string applicantId, decimal cost)
        {
            Console.WriteLine("ApproveRequest hit");

            if (applicantId == null)
            {
                return NotFound();
            }

            var taskModel = await _context.Tasks.FindAsync(taskId);
            if (taskModel == null)
            {
                return NotFound();
            }

            taskModel.TakenById = applicantId;
            taskModel.Status = MyTaskStatus.PaymentProcessing.ToString();
            _context.SaveChanges();

            var paymentSession = await _paymentService.CreatePayment(taskModel, cost);

            TempData["PaymentSession"] = paymentSession.Id;
            Response.Headers.Add("Location", paymentSession.Url);
            return new StatusCodeResult(303);
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

        [Authorize(Roles = "Poster,Admin")]
        public async Task<IActionResult> ViewApplicantDetail(string applicantId, int taskId)
        {
            // Get contractor info
            var contractorInfo = await _context.ContractorInfos.FirstOrDefaultAsync(u => u.UserId.Equals(applicantId));
            var taskModel = await _context.Tasks.FindAsync(taskId);

            if (contractorInfo == null || taskModel == null)
            {
                return NotFound();
            }

            // Calculate distance
            double distance = DistanceCalculator.GetDistanceInMiles(contractorInfo.Latitude, contractorInfo.Longitude, taskModel.Latitude, taskModel.Longitude);

            // Calculate cost
            double area = taskModel.Length * taskModel.Height;
            decimal cost = contractorInfo.CostPerSqrFoot * (decimal)area;

            // Create a view model to pass to the view
            var viewModel = new ApplicantDetailViewModel
            {
                ContractorInfo = contractorInfo,
                Distance = distance,
                Cost = cost,
                TaskId = taskModel.Id
            };

            return View("ApplicantDetail", viewModel);
        }

        private bool TaskModelExists(int id)
        {
            return _context.Tasks.Any(e => e.Id == id);
        }
    }

    public class ApplicantDetailViewModel
    {
        public ContractorInfo ContractorInfo { get; set; }
        public double Distance { get; set; }
        public decimal Cost { get; set; }
        public int TaskId { get; set; }
    }
}
