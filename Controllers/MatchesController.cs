using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FootballSimulatorAPI.Models;

namespace FootballSimulatorAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MatchesController : ControllerBase
{
    private readonly FootballSimulatorAPIContext _context;

    public MatchesController(FootballSimulatorAPIContext context)
    {
        _context = context;
    }

    // return Match object which include all its foreign key objects
    private IQueryable<Match> GetIncludeAll()
    {
        return _context.Match
            .Include(m => m.HomeTeam)
            .Include(m => m.AwayTeam)
            .Include(m => m.HostTeam)
            .Include(m => m.MatchResult.StandardTimeResult)
            .Include(m => m.MatchResult.ExtraTimeResult)
            .Include(m => m.MatchResult.PenaltyShootoutResult);
    }

    // GET: api/Matches
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Match>>> GetMatch()
    {
        if (_context.Match == null)
        {
            return NotFound();
        }
        return await GetIncludeAll().ToListAsync();
    }

    // GET: api/Matches/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Match>> GetMatch(int id)
    {
        if (_context.Match == null)
        {
            return NotFound();
        }
        var match = await GetIncludeAll().FirstOrDefaultAsync(m => m.Id == id);

        if (match == null)
        {
            return NotFound();
        }

        return match;
    }

    // POST: api/Matches
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Match>> PostMatch(Match match)
    {
        if (_context.Match == null)
        {
            return Problem("Entity set 'FootballSimulatorAPIContext.Match'  is null.");
        }
        _context.Match.Add(match);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetMatch", new { id = match.Id }, match);
    }

    // DELETE: api/Matches/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMatch(int id)
    {
        if (_context.Match == null)
        {
            return NotFound();
        }
        var match = await _context.Match.FindAsync(id);
        if (match == null)
        {
            return NotFound();
        }

        _context.Match.Remove(match);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool MatchExists(int id)
    {
        return (_context.Match?.Any(e => e.Id == id)).GetValueOrDefault();
    }

    // simulate a match n times without saving any result
    [HttpGet("{id}/simulate")]
    public async Task<ActionResult<MatchResult[]>> Simulate(int id, int n = 1)
    {
        if (_context.Match == null)
        {
            return NotFound();
        }

        var match = await _context.Match.Include("HomeTeam").Include("AwayTeam").Include("HostTeam").FirstOrDefaultAsync(m => m.Id == id);

        if (match == null)
        {
            return NotFound();
        }

        if (n < 1)
        {
            return BadRequest();
        }

        return Enumerable.Repeat(0, n).Select(_ => match.Simulate()).ToArray();
    }
}
