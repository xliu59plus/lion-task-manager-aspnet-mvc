using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LionTaskManagementApp.Data;
using LionTaskManagementApp.Models;
using Microsoft.AspNetCore.Authorization;
using LionTaskManagementApp.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using NuGet.Packaging.Signing;
using Stripe;
using LionTaskManagementApp.Services;
using LionTaskManagementApp.Models.Constants;

namespace LionTaskManagementApp.Controllers
{
    public class TakerController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<TaskUser> _userManager;
        private readonly SignInManager<TaskUser> _signInManager;
        private readonly S3Service _s3Service;

        public TakerController(ApplicationDbContext context, UserManager<TaskUser> userManager, SignInManager<TaskUser> signInManager, S3Service s3Service)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _s3Service = s3Service;
        }

        [Authorize(Roles="Taker, Inactive_Taker, Admin")]    
        public IActionResult EditProfile()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Taker, Inactive_Taker, Admin")]
        public async Task<IActionResult> CreateProfileTaker(ContractorInfoViewModel model,IFormFile BusinessDocumentationUpload)
        {
            Console.WriteLine("Check if businessDocumentationUpload is null : " + BusinessDocumentationUpload == null);
            Console.WriteLine(BusinessDocumentationUpload);

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
                // Handle file upload for BusinessDocumentation
               // Handle file upload for BusinessDocumentation
                if (BusinessDocumentationUpload != null && BusinessDocumentationUpload.Length > 0)
                {
                    // Validate file type (only PDF or images allowed)
                    if (!BusinessDocumentationUpload.ContentType.StartsWith("application/pdf") &&
                        !BusinessDocumentationUpload.ContentType.StartsWith("image/"))
                    {
                        ModelState.AddModelError("BusinessDocumentationUpload", "Only PDF or image files are allowed.");
                        return View(model); // Return to the form with an error message
                    }

                    // Validate file size (maximum 5 MB)
                    if (BusinessDocumentationUpload.Length > 5 * 1024 * 1024) // 5 MB limit
                    {
                        ModelState.AddModelError("BusinessDocumentationUpload", "File size must not exceed 5 MB.");
                        return View(model); // Return to the form with an error message
                    }

                    // If validation passes, proceed with file upload
                    // 1. Create a temporary file
                    string tempFilePath = Path.GetTempFileName();

                    // 2. Save the uploaded file to the temporary location
                    using (var stream = new FileStream(tempFilePath, FileMode.Create))
                    {
                        await BusinessDocumentationUpload.CopyToAsync(stream);
                    }

                    // 3. Upload to S3 from the temporary file
                    string key = $"artworks/{Guid.NewGuid()}_{BusinessDocumentationUpload.FileName}";
                    await _s3Service.UploadFileAsync(tempFilePath, key);

                    // 4. Get the pre-signed URL
                    string uploadedArtworkUrl = await _s3Service.GetPreSignedUrlAsync(key, TimeSpan.FromMinutes(10));
                    contractorInfo.BusinessDocumentationKey = key;

                    // 5. Delete the temporary file
                    System.IO.File.Delete(tempFilePath);
                }

                // Update ContractorInfo fields from the model
                var currentUser = await _userManager.GetUserAsync(User);  
                contractorInfo.ZipCode = model.ZipCode;
                contractorInfo.PreferenceDistance = model.MaxTravelDistanceMiles;
                contractorInfo.FirstLine = model.FirstLine;
                contractorInfo.SecondLine = model.SecondLine;
                contractorInfo.City = model.City;
                contractorInfo.StateProvince = model.StateProvince;
                contractorInfo.CompanyName = model.CompanyName;
                contractorInfo.EIN = model.EIN;
                contractorInfo.FacebookLink = model.FacebookLink;
                contractorInfo.InstagramLink = model.InstagramLink;
                contractorInfo.TikTokLink = model.TikTokLink;
                contractorInfo.WallpenHubProfileLink = model.WallpenHubProfileLink;
                contractorInfo.ArtworkSpecialization = model.ArtworkSpecialization;
                contractorInfo.DoesPrintWhiteColor = model.DoesPrintWhiteColor;
                contractorInfo.SupportsCMYK = model.SupportsCMYK;
                contractorInfo.WallpenMachineModel = model.WallpenMachineModel;
                contractorInfo.WallpenSerialNumber = model.WallpenSerialNumber;
                contractorInfo.DoesChargeTravelFeesOverLimit = model.DoesChargeTravelFeesOverLimit;
                contractorInfo.TravelFeeOverLimit = model.TravelFeeOverLimit;
                contractorInfo.Longitude = model.Longitude;
                contractorInfo.Latitude = model.Latitude;
                contractorInfo.CostPerSqrFoot = model.CMYKWhiteColorPrice ?? 0;

                // Pricing field
                contractorInfo.CMYKPrice = model.CMYKPrice;
                contractorInfo.WhiteColorPrice = model.WhiteColorPrice;
                contractorInfo.CMYKWhiteColorPrice = model.CMYKWhiteColorPrice;

                // Create a new ActivationRequest
                var activationRequest = new ActivationRequest
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                    UserEmail = user.Email,
                    RequestedRole = @RoleConstants.Taker,
                    IsApproved = false,
                    RequestTime = DateTimeOffset.UtcNow,
                    LastUpdateTime = DateTimeOffset.UtcNow,
                    DenyComents = string.Empty
                };

                _context.ActivationRequests.Add(activationRequest);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Successfully sent application for activation, please wait for approval.";
                TempData.Keep("SuccessMessage");

                // Add the ActivationRequested role to the user
                await _userManager.AddToRoleAsync(user, RoleConstants.ActivationRequested);

                // Refresh the sign-in session
                await _signInManager.RefreshSignInAsync(user);
            }
            else
            {
                ModelState.AddModelError("", "User not found.");
                return View(model); // Return to the form with validation errors
            }

            return RedirectToAction("Index", "Home");
        }


        // GET: Tasks
        [Authorize(Roles = "Taker,Admin")]
        public async Task<IActionResult> TakerIndex()
        {
            var currentUserId = User.Identity?.Name;
            var userTasks = await _context.Tasks
                                  .Where(t => t.TakenById.Equals(currentUserId))
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

            if (taskModel.WallPicKey != null) {
                taskModel.WallPicUrl = await _s3Service.GetPreSignedUrlAsync(taskModel.WallPicKey, TimeSpan.FromMinutes(10));
            }

            if (taskModel.ArtworkKey != null)
            {
                taskModel.ArtworkUrl = await _s3Service.GetPreSignedUrlAsync(taskModel.ArtworkKey, TimeSpan.FromMinutes(10));
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
            if (User == null) { 
                return NotFound();
            }

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
