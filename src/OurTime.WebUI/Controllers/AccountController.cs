﻿using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using OurTime.WebUI.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OurTime.WebUI.Models.Entities;

namespace OurTime.WebUI.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _db;
        public AccountController(ApplicationDbContext db)
        {
            _db = db;
        }

        // ----- REGISTER -----
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(string username, string password)
        {
            // Kontrollera inmatning
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                ModelState.AddModelError("", "Användarnamn och lösenord krävs");
                return View();
            }

            bool exists = await _db.Users.AnyAsync(u => u.Username == username);
            if (exists)
            {
                ModelState.AddModelError("", "Användarnamnet är redan taget");
                return View();
            }

            // Spara ny user (OBS: i en riktig app måste du hash:a lösen!)
            _db.Users.Add(new User
            {
                Username = username,
                Password = password
            });
            await _db.SaveChangesAsync();

            // Skicka vidare till login-sidan
            return RedirectToAction("Login");
        }

        // ----- LOGIN -----
        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password, string returnUrl = null)
        {
            var user = _db.Users
                .FirstOrDefault(u => u.Username == username && u.Password == password);

            if (user != null)
            {
                var claims = new[] { new Claim(ClaimTypes.Name, user.Username) };
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(identity));

                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    return Redirect(returnUrl);

                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Fel användarnamn eller lösenord");
            return View();
        }

        // ----- LOGOUT -----

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}
