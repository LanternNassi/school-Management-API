using System;
using System.ComponentModel.DataAnnotations.Schema;
using School_Management_System.Models.StudentFeesStructureX;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using School_Management_System.Models;
using School_Management_System.Models.TransactionX;
using System.Collections.Generic;

namespace School_Management_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly DBContext _context;
        private readonly IMapper _mapper;

        public TransactionsController(DBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Transactions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TransactionDto>>> GetTransactions(
            [FromQuery] Guid? studentFeesStructureId,
            [FromQuery] Guid? studentId
        )
        {
            if (_context.Transactions == null)
            {
                return NotFound();
            }
            var query = _context.Transactions.AsQueryable();

            query = query.Include(c => c.StudentFeesStructure);

            if (studentFeesStructureId != null)
            {
                query = query.Where(c => c.StudentFeesStructureId == studentFeesStructureId);
            }

            if (studentId != null){
                query = query.Where(c => c.StudentFeesStructure.StudentId == studentId);
            }

            return Ok(_mapper.Map<List<TransactionDto>>(await query.ToListAsync()));
        }

        // GET: api/Transactions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TransactionDto>> GetTransaction(Guid id)
        {
            if (_context.Transactions == null)
            {
                return NotFound();
            }
            var transaction = await _context.Transactions.FindAsync(id);

            return Ok(_mapper.Map<TransactionDto>(transaction));
        }

        [HttpPost]
        public async Task<ActionResult<TransactionDto>> PostTransaction(TransactionDto transactionDto)
        {

            // Check if the studentFeesStructure exists
            var studentFeesStructure = await _context.StudentFeesStructures.FindAsync(transactionDto.StudentFeesStructureId);
            if (studentFeesStructure == null){
                return BadRequest("StudentFeesStructure does not exist");
            }

            // Get balance
            var paid_transactions = _context.Transactions.Where(c => c.StudentFeesStructureId == transactionDto.StudentFeesStructureId).Sum(c => c.Amount);

            if (studentFeesStructure.Amount < paid_transactions + transactionDto.Amount){
                return BadRequest("Amount exceeds the fees structure amount");
            }

            var transaction = _mapper.Map<Transaction>(transactionDto);
            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTransaction", new { id = transaction.Id }, _mapper.Map<TransactionDto>(transaction));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutTransaction(Guid id, TransactionDto transactionDto)
        {
            if (id != transactionDto.Id)
            {
                return BadRequest();
            }

            var transaction = _mapper.Map<Transaction>(transactionDto);
            _context.Entry(transaction).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TransactionExists(id))
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTransaction(Guid id)
        {
            var transaction = await _context.Transactions.FindAsync(id);
            if (transaction == null)
            {
                return NotFound();
            }

            _context.SoftDelete(transaction);
            return NoContent();
        }

        private bool TransactionExists(Guid id)
        {
            return _context.Transactions.Any(e => e.Id == id);
        }
    }        
}