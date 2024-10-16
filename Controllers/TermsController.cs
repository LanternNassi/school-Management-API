using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using School_Management_System.Models.TermX;
using School_Management_System.Models.StudentX;
using School_Management_System;
using School_Management_System.Models;
using School_Management_System.Models.TransactionX;

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

				var transactionsQuery = _context.Transactions.Where(t => t.StudentFeesStructure.TermId == id).AsQueryable();

				// Include all related entities
				var meta_data = new {
					total_term_fees = fees_query.Sum(f => f.Amount),
					total_paid_fees = transactionsQuery.Sum(t => t.Amount),
					total_unpaid_fees = fees_query.Sum(f => f.Amount) - transactionsQuery.Sum(t => t.Amount),
					total_students = fees_query.Count(),
				};

				var class_meta_data = new List<object>();

				foreach (var xclass in _context.Classes.ToList())
				{
					var class_fees_query = fees_query.Where(f => f.Student.Stream.Class.Id == xclass.Id).AsQueryable();
					var class_transactions_query = transactionsQuery.Where(t => t.StudentFeesStructure.Student.Stream.Class.Id == xclass.Id).AsQueryable();

					class_meta_data.Add(new {
						id  = xclass.Id,
						@class = xclass,
						total_term_fees = class_fees_query.Sum(f => f.Amount),
						total_paid_fees = class_transactions_query.Sum(t => t.Amount),
						total_unpaid_fees = class_fees_query.Sum(f => f.Amount) - class_transactions_query.Sum(t => t.Amount),
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

			var active_term = _context.Terms.FirstOrDefault(t => t.IsActive);

			if ((active_term != null)  && term.IsActive)
			{
				return BadRequest("There is already an active term");
			}

			_context.Terms.Add(term);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetTerm", new { id = term.Id }, term);
		}

		[HttpPost("{id}/add_students")]
		public async Task<IActionResult> AddStudents(Guid id, List<Guid> studentIds)
		{
    		var term = await _context.Terms.Include(t => t.Students).FirstOrDefaultAsync(t => t.Id == id);

			if (term == null)
			{
				return NotFound();
			}

			foreach (var studentId in studentIds)
			{
				var student = await _context.Students.FindAsync(studentId);

				if (student == null)
				{
					return NotFound();
				}

				term.Students.Add(student);
			}

			await _context.SaveChangesAsync();

			return Ok("Students added successfully");
		}

		// PUT: api/Terms/5
		[HttpPut("{id}")]
		public async Task<IActionResult> PutTerm(Guid id, Term term)
		{
			if (id != term.Id)
			{
				return BadRequest("Term ID mismatch");
			}

			var active_term = _context.Terms.FirstOrDefault(t => t.IsActive);
			var term_to_update = await _context.Terms.FindAsync(id);

			if (term_to_update == null)
			{
				return NotFound();
			}

			// If the term is being set to active, check if there's already an active term
			if (term.IsActive && !term_to_update.IsActive)
			{
				if (active_term != null)
				{
					return BadRequest("There is already an active term");
				}
			}

			// Update only the necessary fields
			term_to_update.Name = term.Name;
			term_to_update.Description = term.Description;
			term_to_update.StartDate = term.StartDate;
			term_to_update.EndDate = term.EndDate;
			term_to_update.IsActive = term.IsActive;
			

			_context.Entry(term_to_update).State = EntityState.Modified;

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