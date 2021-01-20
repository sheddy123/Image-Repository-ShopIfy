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
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IAccountRepository _accountRepo;
        private readonly IImageRepository _imageRepo;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HomeController(ILogger<HomeController> logger, IHttpContextAccessor httpContextAccessor, IAccountRepository accountRepo, IImageRepository imageRepo)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _imageRepo = imageRepo;
            _accountRepo = accountRepo;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            ImageUploads imgUploads = new ImageUploads();
            IndexVm listOfImages = new IndexVm()
            {
                Images = await _imageRepo.GetAllImages(StaticDetails.GetImages)
                //HttpContext.User.Identity.IsAuthenticated ? await _imageRepo.GetAllImageByIdAsync
                //(StaticDetails.GetSingleImage, UserId(), HttpContext.Session.GetString("JWToken")) :
            };
            imgUploads.IndexViewModel = listOfImages;
            ViewBag.ImageSuccess = TempData["ImageSuccess"];
            ViewBag.ImageExist = TempData["ImageExist"];
            ViewBag.Deleted = TempData["ImageDeleted"];
            if (User.Identity.IsAuthenticated)
            {
                //ViewData["ReturnUrl"] = returnUrl;
                
                //HttpContext.Session.Clear();
                if (listOfImages.Images != null)
                {
                    TempData["Authenticated"] = User.Identity.IsAuthenticated ? "Authenticated" : null;
                    return View(imgUploads);
                }

                return View(imgUploads);
            }
            return View(imgUploads);
        }


        private int UserId()
        {
            ClaimsIdentity claimsIdentity = User.Identity as ClaimsIdentity;
            var userName = claimsIdentity.IsAuthenticated ? claimsIdentity.FindFirst(ClaimTypes.Sid).Value : null;
            return userName == null || userName == "" ? 0 : Convert.ToInt32(userName);
        }

        private string Token()
        {
            ClaimsIdentity claimsIdentity = User.Identity as ClaimsIdentity;
            var token = claimsIdentity.IsAuthenticated ? claimsIdentity.FindFirst(ClaimTypes.Hash).Value : null;
            return token == null || token == "" ? "" : token;
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
        [AllowAnonymous]
        public IActionResult Login()
        {
            User obj = new User();
            return View("LoginRegister", obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Login(User obj)
        {
            var objResponse = await _accountRepo.LoginAsync(StaticDetails.AccountPath + "authenticate", obj);
            if (objResponse.Token == null)
                return View("LoginRegister", objResponse);

            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            identity.AddClaim(new Claim(ClaimTypes.Name, objResponse.Username));
            identity.AddClaim(new Claim(ClaimTypes.Role, objResponse.Role));
            identity.AddClaim(new Claim(ClaimTypes.Sid, objResponse.Id.ToString()));
            identity.AddClaim(new Claim(ClaimTypes.Hash, objResponse.Token.ToString()));

            var principal = new ClaimsPrincipal(identity);
            await _httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            //HttpContext.Session.SetString("JWToken", objResponse.Token);
            //_httpContextAccessor.HttpContext.Session.SetString("UserId", objResponse.Id.ToString());

            HttpContext.Session.SetString("Username", objResponse.Username);
            return RedirectToAction("Index");
        }

        [HttpPost]
        //[AllowAnonymous]
        //[Authorize(Roles ="test")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(ImageUploads imageUploads)
        {
            try
            {
                var ms1 = new MemoryStream();
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
                                    imageUploads.UserId = UserId();
                                    imageUploads.ImagesUploads = ms1.ToArray();
                                    imageUploads.Images = imageUploads.ImagesUploads;
                                    var fileImage = new FileImage()
                                    {
                                        FileName = imageUploads.ImageName,
                                        Image = imageUploads.Images,
                                        FileType = imageUploads.FileType,
                                        FileExtension = imageUploads.FileExtension
                                    };
                                    imageUploads.fileImages.Add(fileImage);
                                    ms1 = new MemoryStream();
                                    break;
                            }
                        }
                    }
                }

                var uploadImageToDb = await _imageRepo.UploadImageAsync(StaticDetails.UploadImagePath, imageUploads, Token().ToString());

                if (uploadImageToDb.imageExist[0] == "Success")
                    TempData["ImageSuccess"] = (count > 1) ? "Images uploaded successfully" : "Image uploaded successfully";

                TempData["ImageExist"] = uploadImageToDb.imageExist;

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            User obj = new User();
            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Register(User obj)
        {
            var objResponse = await _accountRepo.RegisterAsync(StaticDetails.AccountPath + "register", obj);
            if (objResponse == false)
                return View();

            return RedirectToAction("Login");
        }


        public async Task<IActionResult> LogoutAsync(User obj)
        {
            HttpContext.Session.Clear();
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            //HttpContext.Session.SetString("JWToken", "");
            //HttpContext.Session.SetString("Username", "");
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            if (id > 0)
            {
                var deleteImage = _imageRepo.DeleteAsync(StaticDetails.DeleteImage, id, Token().ToString());
                TempData["ImageDeleted"] = deleteImage.Result == true ? "Image Deleted Successfully" : "Error Deleting Image";
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
