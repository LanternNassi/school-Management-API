using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using School_Management_System.Models;
using School_Management_System.Models.StudentFeesStructureX;
using AutoMapper;

namespace School_Management_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentFeesStructureController : ControllerBase
    {
        private readonly DBContext _context;
        private readonly IMapper _mapper;

        public StudentFeesStructureController(DBContext context , IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/StudentFeesStructure
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StudentFeesStructureDto>>> GetStudentFeesStructures(
            [FromQuery] Guid? studentId,
            [FromQuery] Guid? termId
        )
        {
          if (_context.StudentFeesStructures == null)
          {
              return NotFound();
          }
            var query = _context.StudentFeesStructures.AsQueryable();

            query = query.Include(c => c.Student);
            query = query.Include(c => c.Term);

            if (studentId != null)
            {
                query = query.Where(c => c.StudentId == studentId);
            }

            if (termId != null)
            {
                query = query.Where(c => c.TermId == termId);
            }

            return Ok(_mapper.Map<List<StudentFeesStructureDto>>(await query.ToListAsync()));
        }
        
        // GET: api/StudentFeesStructure/5
        [HttpGet("{id}")]
        public async Task<ActionResult<StudentFeesStructureDto>> GetStudentFeesStructure(
            Guid id,
            [FromQuery] bool verbosity = true
        )
        {
            if (_context.StudentFeesStructures == null)
            {
                return NotFound();
            }
                
            var query = _context.StudentFeesStructures
                                .Include(c => c.Student)
                                .Include(c => c.Term)
                                .AsQueryable();

            var studentFeesStructure = await query.FirstOrDefaultAsync(c => c.Id == id);

            if (studentFeesStructure == null)
            {
                return NotFound();
            }

            var result = _mapper.Map<StudentFeesStructureDto>(studentFeesStructure);

            // Implement verbosity logic if needed
            if (!verbosity)
            {
                // Adjust result based on verbosity
            }

            return Ok(result);
        }


        // PUT: api/StudentFeesStructure/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStudentFeesStructure(Guid id, StudentFeesStructureDto studentFeesStructureDto)
        {
            if (id != studentFeesStructureDto.Id)
            {
                return BadRequest();
            }

            _context.Entry(_mapper.Map<StudentFeesStructure>(studentFeesStructureDto)).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentFeesStructureExists(id))
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

        // POST: api/StudentFeesStructure
        [HttpPost]
        public async Task<ActionResult<StudentFeesStructureDto>> PostStudentFeesStructure(StudentFeesStructureDto studentFeesStructureDto)
        {
            _context.StudentFeesStructures.Add(_mapper.Map<StudentFeesStructure>(studentFeesStructureDto));
            await _context.SaveChangesAsync();
            return Ok(studentFeesStructureDto);
        }

        private bool StudentFeesStructureExists(Guid id)
        {
            return _context.StudentFeesStructures.Any(e => e.Id == id);
        }

    }    
}