using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using School_Management_System.Models;
using School_Management_System.Models.ClassX;
using AutoMapper;

namespace School_Management_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassesController : ControllerBase
    {
        private readonly DBContext _context;
        private readonly IMapper _mapper;

        public ClassesController(DBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClassDto>>> GetClasses()
        {
          if (_context.Classes == null)
          {
              return NotFound();
          }

          var query = _context.Classes.AsQueryable();

          return Ok(_mapper.Map<List<ClassDto>>(await query.ToListAsync()));
        }

        // GET: api/Classes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ClassDto>> GetClass(Guid id)
        {
          if (_context.Classes == null)
          {
              return NotFound();
          }
            var @class = await _context.Classes.FindAsync(id);

            if (@class == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<ClassDto>(@class));
        }

        // PUT: api/Classes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutClass(Guid id, Class @class)
        {
            if (id != @class.Id)
            {
                return BadRequest();
            }

            _context.Entry(@class).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClassExists(id))
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

        // POST: api/Classes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Class>> PostClass(ClassDto classdto)
        {
          if (_context.Classes == null)
          {
              return Problem("Entity set 'DBContext.Classes'  is null.");
          }
            _context.Classes.Add(_mapper.Map<Class>(classdto));
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetClass", new { id = classdto.Id }, classdto);
        }

        // DELETE: api/Classes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClass(Guid id)
        {
            if (_context.Classes == null)
            {
                return NotFound();
            }
            var @class = await _context.Classes.FindAsync(id);
            if (@class == null)
            {
                return NotFound();
            }


            _context.SoftDelete(@class);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ClassExists(Guid id)
        {
            return (_context.Classes?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
