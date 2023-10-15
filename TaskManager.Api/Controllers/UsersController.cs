﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManager.Api.Models;
using TaskManager.Api.Models.Data;
using TaskManager.Api.Models.Services;
using TaskManager.Common.Models;

namespace TaskManager.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationContext _db;
        private readonly UserService _userService;
        public UsersController(ApplicationContext db)
        {
            _db = db;
            _userService = new UserService(db);
        }

        [HttpGet("test")]
        [AllowAnonymous]
        public IActionResult TesApi()
        {
            return Ok("Server started in " + DateTime.Now);
        }

        [HttpPost]
        public IActionResult CreateUSer([FromBody] UserModel userModel)
        {
            if (userModel != null)
            {
                bool result = _userService.Create(userModel);

                return  result ? Ok() : NotFound();
            }
            return BadRequest();
        }

        [HttpPatch("{id}")]
        public IActionResult UpdateUser(int id,[FromBody] UserModel userModel)
        {
            if(userModel != null)
            {
                bool result = _userService.Update(id, userModel);

                return result ? Ok() : NotFound();
            }
            return BadRequest();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            bool result = _userService.Delete(id);

            return result ? Ok() : NotFound();
        }

        [HttpGet]
        public async Task<IEnumerable<UserModel>> GetUsers()
        {
            return await _db.Users.Select(u => u.ToDto()).ToListAsync();
        }

        [HttpPost("all")]
        public async Task<IActionResult> CreateMultipleUsers([FromBody] List<UserModel> userModels)
        {
            if(userModels != null && userModels.Count > 0)
            {
                bool result = _userService.CreateMultipleUsers(userModels);

                return result ? Ok() : NotFound();
            }
            return BadRequest();
        }
    }
}
