﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ImageRepoAPI.Models;
using ImageRepoAPI.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ImageRepoAPI.Controllers
{
    [Authorize]
    [Route("api/v{version:apiVersion}/Users")]
    //[Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepo;
        public UsersController(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] AuthenticationModel model)
        {
            var user = _userRepo.Authenticate(model.Username, model.Password);
            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });
            
            return Ok(user);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterModel model)
        {
            bool ifUserNameUnique = _userRepo.IsUniqueUser(model.Username, model.EmailAddress);
            if (!ifUserNameUnique)
                return BadRequest(new { message = "Username already exists" });
            
            var user = _userRepo.Register(model.Username, model.Password, model.EmailAddress);

            if(user == null)
                return BadRequest(new { message = "Error while registering" });

            return Ok(user);
        }
    }
}