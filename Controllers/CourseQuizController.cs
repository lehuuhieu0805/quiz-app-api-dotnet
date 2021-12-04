using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCaching;
using quiz_app_dotnet_api.Entities;
using quiz_app_dotnet_api.Modals;
using quiz_app_dotnet_api.Repositories;
using quiz_app_dotnet_api.Services;

namespace quiz_app_dotnet_api.Controllers
{
    // [Authorize(Roles = "User")]
    public class CourseQuizController : BaseApiController
    {
        private readonly CourseQuizService _courseQuizService;
        private readonly ICourseQuizRepository<CourseQuiz> _courseQuiz;

        public CourseQuizController(ICourseQuizRepository<CourseQuiz> courseQuiz, CourseQuizService courseQuizService)
        {
            _courseQuizService = courseQuizService;
            _courseQuiz = courseQuiz;
        }
        [HttpGet]
        [AllowAnonymous]
        public ActionResult GetAll()
        {
            return Ok(_courseQuizService.GetAll());
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult> GetById(int id)
        {
            CourseQuiz response = await _courseQuizService.GetById(id);
            if (response == null)
            {
                return NotFound();
            }
            ResponseCourseQuizModal course = new ResponseCourseQuizModal
            {
                Id = response.Id,
                Image = response.image,
                Name = response.name
            };
            return Ok(course);
        }

        [HttpPost]
        public async Task<ActionResult> CreateCourse(CourseQuiz newCourse)
        {
            CourseQuiz response = await _courseQuizService.CreateCourse(newCourse);
            ResponseCourseQuizModal course = new ResponseCourseQuizModal
            {
                Id = response.Id,
                Image = response.image,
                Name = response.name
            };
            return CreatedAtAction(nameof(GetById), new { id = course.Id }, course);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateCourse(int id, CourseQuiz course)
        {
            if (id != course.Id)
            {
                return BadRequest();
            }
            await _courseQuizService.UpdateCourse(course);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCourse(int id)
        {
            bool check = await _courseQuizService.DeleteCourse(id);
            if (!check)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}