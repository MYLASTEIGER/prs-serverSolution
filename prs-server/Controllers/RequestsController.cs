using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using prs_server.Models;

namespace prs_server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RequestsController(AppDbContext context)
        {
            _context = context;
        }
        [HttpGet("Review/{userId}")]
        public async Task<ActionResult<IEnumerable<Request>>> GetRequestsInReview(int userId) {
            var requests = await _context.Requests
                .Where(x => x.Status == "Review"
                && x.UserId != userId)
                .ToListAsync();
            return requests;
        }
                
        [HttpPut("Approve/{id}")]
        public async Task<IActionResult> SetApproved(int id,Request request) {
            request.Status = "Approved";
            return await PutRequest(id, request);
        }
        [HttpPut("Reject/{id}")]
        public async Task<IActionResult> SetRejected(int id, Request request) {
            request.Status = "Rejected";
            return await PutRequest(id, request);
        }
        [HttpPut("review/{id}")]
        public async Task<IActionResult> SetReview(int id,Request request) {
            if(request.Total <= 50) {
                request.Status = "Approved";
            }else {
                request.Status = "Review";
            }
            return await PutRequest(id, request);
        }




        // GET: api/Requests
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Request>>> GetRequest()
        {
            return await _context.Requests.Include(u => u.User).ToListAsync();
        }

        // GET: api/Requests/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Request>> GetRequest(int id)
        {
            var request = await _context.Requests
                 .Include(x => x.User)
                 .Include(x => x.RequestLines)
                 .ThenInclude (x => x.product)
                .SingleOrDefaultAsync(x => x.Id == id);

            if (request == null)
            {
                return NotFound();
            }

            return request;
        }

        // PUT: api/Requests/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRequest(int id, Request request)
        {
            if (id != request.Id)
            {
                return BadRequest();
            }

            _context.Entry(request).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RequestExists(id))
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

        // POST: api/Requests
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Request>> PostRequest(Request request)
        {
            _context.Requests.Add(request);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRequest", new { id = request.Id }, request);
        }

        // DELETE: api/Requests/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRequest(int id)
        {
            var request = await _context.Requests.FindAsync(id);
            if (request == null)
            {
                return NotFound();
            }

            _context.Requests.Remove(request);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RequestExists(int id)
        {
            return _context.Requests.Any(e => e.Id == id);
        }
    }
}
