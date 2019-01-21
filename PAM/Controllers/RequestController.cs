﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PAM.Data;
using PAM.Extensions;
using PAM.Models;
using PAM.Services;


namespace PAM.Controllers
{
    public class RequestController : Controller
    {
        private readonly AppDbContext _dbContext;
        private readonly UserService _userService;
        //private readonly SessionHelper _sessionHelp;

        private IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;

        public RequestController(IADService adService, UserService userService,
            AppDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = context;
            _httpContextAccessor = httpContextAccessor;
            _userService = userService;
           //_sessionHelp = sessionHelp;
        }

        [HttpGet]
        public IActionResult CreateRequester()
        {
            //var employee = _sessionHelp.GetSessionObj<Employee>("Employee");
            var employee = _session.GetObject<Employee>("Employee");
            Requester requester = new Requester
            {
                EmployeeNumber = "3",
                Email = employee.Email,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Username = employee.Username
            };

            requester = _userService.SaveRequester(requester);            

            HttpContext.Session.SetObject("Requester", requester);
            return View("NewRequest");
        }

        [HttpGet]
        public IActionResult CreateRequest()
        {
            return View("NewRequest");
        }

        [HttpPost]
        public async Task<IActionResult> CreateRequest (NewRegistrationViewModel reg)
        {
            if(reg.RequestedFor != null)
            {
                _dbContext.Add(reg.RequestedFor);
            }
            _dbContext.Add(reg.Request);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction("Registrations", "Home");
        }
    }
}
