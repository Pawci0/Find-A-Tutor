﻿using Find_A_Tutor.Infrastructure.Commands.PrivateLesson;
using Find_A_Tutor.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Find_A_Tutor.Api.Controllers
{
    [Route("[controller]")]
    public class PrivateLessonController : Controller
    {
        private readonly IPrivateLessonService _privateLessonService;
        public PrivateLessonController(IPrivateLessonService privateLessonService)
        {
            _privateLessonService = privateLessonService;
        }

        [HttpGet]
        public async Task<IActionResult> Get(string description)
        {
            var privateLessons = await _privateLessonService.BrowseAsync(description);

            return Json(privateLessons);
        }

        [HttpGet("{privateLessonId}")]
        public async Task<IActionResult> Get(Guid privateLessonId)
        {
            var privateLesson = await _privateLessonService.GetAsync(privateLessonId);
            if (privateLesson == null)
            {
                return NotFound();
            }

            return Json(privateLesson);
        }

        [HttpPost]
        [Authorize(Policy = "HasAdminRole")]
        public async Task<IActionResult> Post([FromBody]CreatePrivateLesson command)
        {
            command.PrivateLessonId = Guid.NewGuid();
            await _privateLessonService.CreateAsync(command.PrivateLessonId, command.StudnetId, command.RelevantTo, command.Description, command.Subject);

            return Created($"/PrivateLesson/{command.PrivateLessonId}", null);
        }

        [HttpPut("{privateLessonId}")]
        [Authorize(Policy = "HasAdminRole")]
        public async Task<IActionResult> Put(Guid privateLessonId, [FromBody]UpdatePrivateLesson command)
        {
            await _privateLessonService.UpdateAsync(privateLessonId, command.RelevantTo, command.Description, command.SchoolSubject);

            return NoContent();
        }

        [HttpDelete("{privateLessonId}")]
        [Authorize(Policy = "HasAdminRole")]
        public async Task<IActionResult> Delete(Guid privateLessonId)
        {
            await _privateLessonService.DeleteAsync(privateLessonId);

            return NoContent();
        }
    }
}
