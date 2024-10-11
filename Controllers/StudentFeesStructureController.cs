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

    public class StudentTermReq
    {
        public Guid classId {get; set;}
        public decimal amount {get; set;}
    }

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

            
            if (termId != null)
            {
                query = query.Where(c => c.TermId == termId);
            }

            if (studentId != null)
            {
                query = query.Where(c => c.StudentId == studentId);
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

            var existing_struct =  _context.StudentFeesStructures
                                    .Where(c => c.StudentId == studentFeesStructureDto.StudentId)
                                    .Where(c => c.TermId == studentFeesStructureDto.TermId)
                                    .FirstOrDefault();
            if (existing_struct != null){
                return BadRequest("Student Fees structure already exists");
            }
            _context.StudentFeesStructures.Add(_mapper.Map<StudentFeesStructure>(studentFeesStructureDto));
            await _context.SaveChangesAsync();
            return Ok(studentFeesStructureDto);
        }

        [HttpPost("term/{term_id}")]
        public async Task<IActionResult> PostStudentStructures(Guid term_id ,StudentTermReq streq)
        {
            var term = await _context.Terms.FindAsync(term_id);

            if (term == null){
                return BadRequest("Term doesnot exist");
            }

            var active_class = await _context.Classes.FindAsync(streq.classId);

            if (active_class == null){
                return BadRequest("Class doesnot exist");
            }

            var students = await _context.Students.Where(c => c.Stream.Class.Id == streq.classId).ToListAsync();
            
            foreach(var student in students){
                var existing_struct =  _context.StudentFeesStructures
                                            .Where(c => c.StudentId == student.Id)
                                            .Where(c => c.TermId == term_id)
                                            .FirstOrDefault();

                if (existing_struct != null)
                {
                    existing_struct.Amount = streq.amount;
                }else{
                    _context.StudentFeesStructures.Add(new StudentFeesStructure {
                        StudentId = student.Id,
                        TermId = term_id,
                        Amount = streq.amount,
                        IsPaid = false,
                    });
                }
                
            }

            await _context.SaveChangesAsync();

            return Ok("Student structures created successfully");

        }

        private bool StudentFeesStructureExists(Guid id)
        {
            return _context.StudentFeesStructures.Any(e => e.Id == id);
        }

    }    
}