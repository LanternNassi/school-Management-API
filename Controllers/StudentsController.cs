using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using School_Management_System.Models;
using School_Management_System.Models.StudentX;
using AutoMapper;

namespace School_Management_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly DBContext _context;
        private readonly IMapper _mapper;
        private readonly TermsController _termsController;

        public StudentsController(DBContext context, IMapper mapper , TermsController termsController)
        {
            _context = context;
            _mapper = mapper;
            _termsController = termsController;
        }

        // GET: api/Students
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StudentDto>>> GetStudents([FromQuery]Guid? streamId=null)
        {
          if (_context.Students == null)
          {
              return NotFound();
          }
          
            var query = _context.Students.Include(s => s.Terms).AsQueryable();

            if (streamId != null){
                query = query.Where(c => c.Stream.Id == streamId);
            }

            query = query.Include(c => c.Stream);

            return Ok(_mapper.Map<List<StudentDto>>(await query.ToListAsync()));
        }


        // GET: api/Students/5
        [HttpGet("{id}")]
        public async Task<ActionResult<StudentDto>> GetStudent(Guid id)
        {
          if (_context.Students == null)
          {
              return NotFound();
          }
            var student = await _context.Students.Include(s => s.Terms).FirstOrDefaultAsync(s => s.Id == id);

            if (student == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<StudentDto>(student));
        }

        // PUT: api/Students/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStudent(Guid id, StudentDtoUpdate student)
        {
           

            var initial_student = _context.Students.Find(id);

            if (initial_student == null)
            {
                return NotFound();
            }

            _mapper.Map(student, initial_student);


            _context.Entry(initial_student).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Students
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<StudentDto>> PostStudent(StudentDto student)
        {
            if (_context.Students == null)
            {
                return Problem("Entity set 'DBContext.Students' is null.");
            }

            // Ensure Student.Id is set before calling AddStudents
            if (student.Id == Guid.Empty)
            {
                student.Id = Guid.NewGuid();
            }

            // Map DTO to Entity and add the student
            _context.Students.Add(_mapper.Map<Student>(student));
            await _context.SaveChangesAsync(); // Save the new student to the database

            // Get the active term
            var activeTerm = await _context.Terms.FirstOrDefaultAsync(t => t.IsActive);
            if (activeTerm == null)
            {
                return Problem("No active term found.");
            }

            // Call AddStudents asynchronously
            var createdResult = await _termsController.AddStudents(activeTerm.Id, new List<Guid> { student.Id });
            
            // Check if the AddStudents operation was successful
            if (createdResult is BadRequestResult)
            {
                return BadRequest("Failed to add student to active term.");
            }

            // Return the created student
            return CreatedAtAction("GetStudent", new { id = student.Id }, student);
        }


        // DELETE: api/Students/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(Guid id)
        {
            if (_context.Students == null)
            {
                return NotFound();
            }
            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            _context.SoftDelete(student);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool StudentExists(Guid id)
        {
            return (_context.Students?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
