using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using School_Management_System.Models.TermX;
using School_Management_System;
using School_Management_System.Models;
using AutoMapper;

namespace School_Management_System.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class TermsController : ControllerBase
	{
		private readonly DBContext _context;
        private readonly IMapper _mapper;


		public TermsController(DBContext context, IMapper mapper)
		{
			_context = context;
            _mapper = mapper;
		}

		// GET: api/Terms
		[HttpGet]
		public async Task<ActionResult<IEnumerable<Term>>> GetTerms()
		{
			return await _context.Terms.ToListAsync();
		}

		// GET: api/Terms/5
		[HttpGet("{id}")]
		public async Task<IActionResult> GetTerm(
			Guid id,
			[FromQuery] bool includeAll = false
		)
		{
			var term = await _context.Terms.FindAsync(id);

			var fees_query = _context.StudentFeesStructures.Where(f => f.TermId == id).AsQueryable();

			if (term == null)
			{
				return NotFound();
			}


			var termDTO = _mapper.Map<TermDTO>(term);

			var term_activities = new{
				term = termDTO
			};

			if (includeAll)
			{

				// Include all related entities
				var meta_data = new {
					total_term_fees = fees_query.Sum(f => f.Amount),
					total_paid_fees = fees_query.Where(f => f.IsPaid).Sum(f => f.Amount),
					total_unpaid_fees = fees_query.Where(f => !f.IsPaid).Sum(f => f.Amount),
					total_students = fees_query.Count(),
				};

				var class_meta_data = new List<object>();

				foreach (var xclass in _context.Classes.ToList())
				{
					var class_fees_query = fees_query.Where(f => f.Student.Stream.Class.Id == xclass.Id).AsQueryable();

					class_meta_data.Add(new {
						@class = xclass,
						total_term_fees = class_fees_query.Sum(f => f.Amount),
						total_paid_fees = class_fees_query.Where(f => f.IsPaid).Sum(f => f.Amount),
						total_unpaid_fees = class_fees_query.Where(f => !f.IsPaid).Sum(f => f.Amount),
						total_students = class_fees_query.Count(),
					});
					
				}

				

				return Ok(new{
					meta_data,
					class_meta_data,
					term = termDTO,
				});


			}


			return Ok(term_activities);
		}

		// POST: api/Terms
		[HttpPost]
		public async Task<ActionResult<Term>> PostTerm(Term term)
		{
			_context.Terms.Add(term);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetTerm", new { id = term.Id }, term);
		}

		// PUT: api/Terms/5
		[HttpPut("{id}")]
		public async Task<IActionResult> PutTerm(Guid id, Term term)
		{
			if (id != term.Id)
			{
				return BadRequest();
			}

			_context.Entry(term).State = EntityState.Modified;

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!TermExists(id))
				{
					return NotFound();
				}
				else
				{
					throw;
				}
			}

			return Ok();
		}

		// DELETE: api/Terms/5
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteTerm(int id)
		{
			var term = await _context.Terms.FindAsync(id);
			if (term == null)
			{
				return NotFound();
			}

			_context.Terms.Remove(term);
			await _context.SaveChangesAsync();

			return NoContent();
		}

		private bool TermExists(Guid id)
		{
			return _context.Terms.Any(e => e.Id == id);
		}
	}
}