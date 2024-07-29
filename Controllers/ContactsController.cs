using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using School_Management_System.Models;
using School_Management_System.Models.ContactInfoX;
using AutoMapper;

namespace School_Management_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactsController : ControllerBase
    {
        private readonly DBContext _context;
        private readonly IMapper _mapper;

        public ContactsController(DBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Contacts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ContactInfoDto>>> GetContacts()
        {
          if (_context.Contacts == null)
          {
              return NotFound();
          }
            var query = _context.Contacts.AsQueryable();
            return Ok(_mapper.Map<List<ContactInfoDto>>(await _context.Contacts.ToListAsync()));
        }

        // GET: api/Contacts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ContactInfoDto>> GetContactInfo(Guid id)
        {
          if (_context.Contacts == null)
          {
              return NotFound();
          }
            var contactInfo = await _context.Contacts.FindAsync(id);

            if (contactInfo == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<ContactInfoDto>(contactInfo));
        }

        // PUT: api/Contacts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutContactInfo(Guid id, ContactInfo contactInfo)
        {
            if (id != contactInfo.id)
            {
                return BadRequest();
            }

            _context.Entry(contactInfo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ContactInfoExists(id))
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

        // POST: api/Contacts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IActionResult> PostContactInfo(ContactInfoDto contactInfo)
        {
          if (_context.Contacts == null)
          {
              return Problem("Entity set 'DBContext.Contacts'  is null.");
          }
            _context.Contacts.Add(_mapper.Map<ContactInfo>(contactInfo));
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetContactInfo", new { id = contactInfo.id }, contactInfo);
        }

        // DELETE: api/Contacts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContactInfo(Guid id)
        {
            if (_context.Contacts == null)
            {
                return NotFound();
            }
            var contactInfo = await _context.Contacts.FindAsync(id);
            if (contactInfo == null)
            {
                return NotFound();
            }

            _context.SoftDelete(contactInfo);

            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ContactInfoExists(Guid id)
        {
            return (_context.Contacts?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
