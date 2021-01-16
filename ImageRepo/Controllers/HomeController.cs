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

        public async Task<IActionResult> Index()
        {
            IndexVm listOfImages = new IndexVm()
            {
                Images = await _imageRepo.GetAllAsync(StaticDetails.ImagePath, HttpContext.Session.GetString("JWToken"))
            };
            if (listOfImages != null)
                return View();
            
            return View(listOfImages);
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
            HttpContext.Session.SetString("Username", objResponse.Username);
            return RedirectToAction("Index");
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
            await HttpContext.SignOutAsync();
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
