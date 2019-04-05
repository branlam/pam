﻿using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using PAM.Data;
using Newtonsoft.Json;

namespace PAM.Controllers
{
    public class SystemController : Controller
    {
        private readonly SystemService _systemService;
        private readonly OrganizationService _organizationService;
        private readonly IMapper _mapper;
        private readonly ILogger<SystemController> _logger;
        private readonly FormService _formService;

        public SystemController(SystemService systemService, OrganizationService organizationService, IMapper mapper, ILogger<SystemController> logger, FormService formService)
        {
            _systemService = systemService;
            _organizationService = organizationService;
            _mapper = mapper;
            _logger = logger;
            _formService = formService;
        }


        public IActionResult Systems()
        {
            return View(_systemService.GetSystems());
        }

        public IActionResult ViewSystem(int id)
        {
            return View(_systemService.GetSystem(id));
        }

        [HttpGet]
        public IActionResult AddSystem()
        {
            ViewData["processingUnits"] = new SelectList(_organizationService.GetProcessingUnits(), "ProcessingUnitId", "Name");
            return View(new Models.System());
        }

        [HttpPost]
        public IActionResult AddSystem(Models.System system)
        {
            system = _systemService.AddSystem(system);
            return RedirectToAction(nameof(ViewSystem), new { id = system.SystemId });
        }

        [HttpGet]
        public IActionResult EditSystem(int id)
        {
            ViewData["processingUnits"] = new SelectList(_organizationService.GetProcessingUnits(), "ProcessingUnitId", "Name");
            ViewData["allForms"] = JsonConvert.SerializeObject(_formService.GetForms());
            return View(_systemService.GetSystem(id));
        }

        [HttpPost]
        public IActionResult EditSystem(int id, Models.System update, List<int> formIds)
        {
            var system = _systemService.GetSystem(id);
            _mapper.Map(update, system);
            _systemService.SaveChanges();

            //var forms = _formService.GetForms(formIds);
            //foreach (var form in forms)
            //    system.ProcessingUnitId = unit.ProcessingUnitId;
            //_systemService.SaveChanges();

            return RedirectToAction(nameof(ViewSystem), new { id });
        }

        public IActionResult RemoveSystem(int id)
        {
            var system = _systemService.GetSystem(id);
            system.Retired = true;
            _systemService.SaveChanges();
            return RedirectToAction(nameof(Systems));
        }
    }
}
