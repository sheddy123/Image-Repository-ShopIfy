using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ImageRepo.Models;
using ImageRepo.Repository.IRepository;
using ImageRepo.Models.SD;
using Microsoft.AspNetCore.Http;
using ImageRepo.Models.ViewModels;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using System.IO;

namespace ImageRepo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IAccountRepository _accountRepo;
        private readonly IImageRepository _imageRepo;

        public HomeController(ILogger<HomeController> logger, IAccountRepository accountRepo, IImageRepository imageRepo)
        {
            _logger = logger;
            _imageRepo = imageRepo;
            _accountRepo = accountRepo;
        }

        public async Task<IActionResult> Index(ImageUploads uploadImageToDb)
        {
            IndexVm listOfImages = new IndexVm()
            {
                Images = await _imageRepo.GetAllAsync(StaticDetails.ImagePath, HttpContext.Session.GetString("JWToken"))
            };
            //HttpContext.Session.Clear();
            if (listOfImages != null)
            {
                var username = HttpContext.Session.GetString("Username");
                if (username != null && username != "")
                {
                    TempData["Authenticated"] = "Authenticated";
                }
                else
                {
                    TempData["Authenticated"] = null;
                }
                return View(uploadImageToDb);
            }
            TempData["Authenticated"] = "Authenticated";
            return View(uploadImageToDb);
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public IActionResult Login()
        {
            User obj = new User();
            return View("LoginRegister",obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(User obj)
        {
            var objResponse = await _accountRepo.LoginAsync(StaticDetails.AccountPath + "authenticate", obj);
            if (objResponse.Token == null)
                return View("LoginRegister", objResponse);

            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            identity.AddClaim(new Claim(ClaimTypes.Name, objResponse.Username));
            identity.AddClaim(new Claim(ClaimTypes.Role, objResponse.Role));
            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            HttpContext.Session.SetString("JWToken", objResponse.Token);
            HttpContext.Session.SetString("UserId", objResponse.Id.ToString());
            
            HttpContext.Session.SetString("Username", objResponse.Username);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult UploadImages()
        {
            return RedirectToAction("Index");
        }
        [HttpPost]
        //[Authorize(Roles ="test")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadImages(ImageUploads imageUploads)
        {
            try
            {
                using var ms1 = new MemoryStream();
                var files = HttpContext.Request.Form.Files;
                int count = 0;
                

                imageUploads.Username = HttpContext.Session.GetString("Username");
                if (imageUploads.Username == null || imageUploads.Username == "")
                    return View(nameof(AccessDenied));

                if (files.Count > 0)
                {
                    count = files.Count;
                    for (int i = 0; i < files.Count; i++)
                    {
                        using (var fs1 = files[i].OpenReadStream())
                        {
                            switch (files[i].Name)
                            {
                                case "ImagesUploads":
                                    fs1.CopyTo(ms1);
                                    imageUploads.FileExtension = Path.GetExtension(files[i].FileName).ToLowerInvariant();
                                    imageUploads.ImageName = files[i].FileName;
                                    imageUploads.FileType = files[i].ContentType;
                                    imageUploads.DateCreated = DateTime.Now;
                                    imageUploads.UserId = Convert.ToInt32(HttpContext.Session.GetString("UserId"));
                                    imageUploads.ImagesUploads = ms1.ToArray();
                                    imageUploads.imageExistName.Add(imageUploads.ImageName);
                                    imageUploads.Images = Convert.ToBase64String(imageUploads.ImagesUploads, 0, imageUploads.ImagesUploads.Length);
                                    imageUploads.ImageUploadList.Add(imageUploads.Images);
                                    break;
                            }
                        }
                    }
                }

                var uploadImageToDb = await _imageRepo.UploadImageAsync(StaticDetails.UploadImagePath, imageUploads, HttpContext.Session.GetString("JWToken"));
                
                if (uploadImageToDb.imageExist[0] == "Success")
                    uploadImageToDb.imageExist[0] = (count > 1) ? "Images uploaded successfully" : "Image uploaded successfully";

                

                return View("Index", uploadImageToDb);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }


        [HttpGet]
        public IActionResult Register()
        {
            User obj = new User();
            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(User obj)
        {
            var objResponse = await _accountRepo.RegisterAsync(StaticDetails.AccountPath + "register", obj);
            if (objResponse == false)
                return View();

            return RedirectToAction("Login");
        }

        
        public async Task<IActionResult> LogoutAsync(User obj)
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.SetString("JWToken", "");
            HttpContext.Session.SetString("Username", "");
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
