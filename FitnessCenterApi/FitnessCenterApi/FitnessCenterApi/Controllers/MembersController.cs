using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FitnessCenterApi.Models;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;
using Microsoft.AspNetCore.Http.HttpResults;

namespace FitnessCenterApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MembersController : ControllerBase
    {
        private readonly AppDbContext _context;
        public MembersController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Member>>> GetMembers()
        {
            return await _context.Members.Include(m => m.Bookings).ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<Member>> CreateMember(Member member)
        {
            _context.Members.Add(member);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetMembers), new { id = member.Id }, member);
        }


        [HttpGet("{memberId}/bookings")]
        public async Task<IActionResult> GetMemberBookings(int memberId)
        {
            var member = await _context.Members
                .Include(m => m.Bookings)
                .FirstOrDefaultAsync(m => m.Id == memberId);

            if (member == null) return NotFound("Member not found.");

            return Ok(member.Bookings.ToList());
        }


        [HttpPost("{memberId}/bookings/{courseId}")]
        public async Task<IActionResult> BookCourse(int memberId, int courseId)
        {
            var member = await _context.Members.FindAsync(memberId);
            if (member == null) return NotFound("Member not found.");

            var course = await _context.Courses
                .Include(c => c.Bookings)
                .FirstOrDefaultAsync(c => c.Id == courseId);

            if (course == null) return NotFound("Course not found.");

            bool isAlreadyBooked = await _context.Bookings
                .AnyAsync(b => b.MemberId == memberId && b.CourseId == courseId);

            if (isAlreadyBooked)
            {
                return BadRequest("Member is already booked for this course.");
            }

            if (course.Bookings.Count >= course.MaxParticipants)
            {
                return BadRequest("Maximum number of participants reached");
            }

            var newBooking = new Booking
            {
                MemberId = memberId,
                CourseId = courseId,
                CreatedAt = DateTime.Now
            };

            _context.Bookings.Add(newBooking);
            await _context.SaveChangesAsync();

            return StatusCode(201, newBooking);
        }
    }
}
