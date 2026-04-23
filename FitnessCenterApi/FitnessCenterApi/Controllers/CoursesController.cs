using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using FitnessCenterApi.Models;
using SQLitePCL;

namespace FitnessCenterApi.Controllers
{
    [Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CoursesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionsResult<IEnumerable<Course>>> GetCourses()
        {
            return await _context.Courses.Include(c => c.Buch)
        }
    }
}
