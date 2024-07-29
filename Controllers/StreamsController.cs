using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using School_Management_System.Models;
using School_Management_System.Models.StreamX;
using Stream = School_Management_System.Models.StreamX.Stream;
using AutoMapper;

namespace School_Management_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StreamsController : ControllerBase
    {
        private readonly DBContext _context;
        private readonly IMapper _mapper;

        public StreamsController(DBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Streams
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StreamDto>>> GetStreams([FromQuery] Guid? Class = null)
        {
          if (_context.Streams == null)
          {
              return NotFound();
          }

            var query = _context.Streams.AsQueryable();

            if (Class != null)
            {
                query = query.Where(c => c.Class.Id == Class);
            }

            return Ok(_mapper.Map<List<StreamDto>>(await query.ToListAsync()));
        }

        // GET: api/Streams/5
        [HttpGet("{id}")]
        public async Task<ActionResult<StreamDto>> GetStream(Guid id)
        {
          if (_context.Streams == null)
          {
              return NotFound();
          }
            var stream = await _context.Streams.FindAsync(id);

            if (stream == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<StreamDto>(stream));
        }

        // PUT: api/Streams/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStream(Guid id, Stream stream)
        {
            if (id != stream.Id)
            {
                return BadRequest();
            }

            _context.Entry(stream).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StreamExists(id))
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

        // POST: api/Streams
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Stream>> PostStream(StreamDto stream)
        {
          if (_context.Streams == null)
          {
              return Problem("Entity set 'DBContext.Streams'  is null.");
          }
            _context.Streams.Add(_mapper.Map<Stream>(stream));
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetStream", new { id = stream.Id }, stream);
        }

        // DELETE: api/Streams/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStream(Guid id)
        {
            if (_context.Streams == null)
            {
                return NotFound();
            }
            var stream = await _context.Streams.FindAsync(id);
            if (stream == null)
            {
                return NotFound();
            }


            _context.SoftDelete(stream);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool StreamExists(Guid id)
        {
            return (_context.Streams?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
