using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Skrmiish.Models;

namespace Skrmiish.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatesController : ControllerBase
    {
        private readonly StateContext _context;

        public StatesController(StateContext context)
        {
            _context = context;
        }

        #region CRUD Operations

        // GET: api/States
        [HttpGet]
        public async Task<ActionResult<IEnumerable<State>>> GetStates()
        {
          if (_context.States == null)
          {
              return NotFound();
          }
            return await _context.States.Where(x => x.IsDeleted == false).ToListAsync();
        }

        // GET: api/States/5
        [HttpGet("{id}")]
        public async Task<ActionResult<State>> GetState(int id)
        {
          if (_context.States == null)
          {
              return NotFound();
          }
            var state = await _context
                                .States
                                .Where(x => x.StateId == id && x.IsDeleted == false)
                                .FirstOrDefaultAsync();

            if (state == null)
            {
                return NotFound();
            }

            return state;
        }

        // PUT: api/States/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutState(int id, State state)
        {
            if (id != state.StateId)
            {
                return BadRequest();
            }

            _context.Entry(state).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StateExists(id))
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

        // POST: api/States
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<State>> PostState(State state)
        {
          if (_context.States == null)
          {
              return Problem("Entity set 'StateContext.States'  is null.");
          }
            _context.States.Add(state);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetState), new { id = state.StateId }, state);
        }

        // DELETE: api/States/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteState(int id)
        {
            if (_context.States == null)
            {
                return NotFound();
            }

            var state = await _context
                                .States
                                .Where(x => x.StateId == id && x.IsDeleted == false)
                                .FirstOrDefaultAsync();
            if (state == null)
            {
                return NotFound();
            }

            state.IsDeleted = true;
            _context.States.Update(state);

            await _context.SaveChangesAsync();

            return Ok();
        }

        #endregion

        #region Custom Operations

        // GET: api/States/5
        [HttpGet("isBlacklisted/{stateCode}")]
        public async Task<ActionResult<bool>> IsBlacklisted(string stateCode)
        {
            if (_context.States == null)
            {
                return NotFound();
            }
            var state = await _context
                                .States
                                .Where(x => (x.StateCode == stateCode) && x.IsDeleted == false)
                                .FirstOrDefaultAsync();

            if (state == null)
            {
                return NotFound();
            }

            return state.IsBlacklisted;
        }

        #endregion

        private bool StateExists(int id)
        {
            return (_context.States?.Any(e => e.StateId == id && e.IsDeleted == false)).GetValueOrDefault();
        }
    }
}
