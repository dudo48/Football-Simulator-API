using FootballSimulatorAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FootballSimulatorAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MatchResultsController : ControllerBase
{
    private readonly FootballSimulatorAPIContext _context;

    public MatchResultsController(FootballSimulatorAPIContext context)
    {
        _context = context;
    }

    // return Match object which include all its foreign key objects
    private IQueryable<MatchResult> GetIncludeAll()
    {
        return _context.MatchResult
            .Include(r => r.StandardTimeResult)
            .Include(r => r.ExtraTimeResult)
            .Include(r => r.PenaltyShootoutResult);
    }

    [HttpGet("{matchId}")]
    public async Task<ActionResult<MatchResult>> GetMatchResult(int matchId)
    {
        if (_context.Match == null)
        {
            return NotFound();
        }
        var matchResult = await GetIncludeAll().FirstOrDefaultAsync(r => r.matchId == matchId);

        if (matchResult == null)
        {
            return NotFound();
        }

        return matchResult;
    }

    [HttpPost]
    public async Task<ActionResult<MatchResult>> PostMatchResult(MatchResult matchResult)
    {
        if (_context.MatchResult == null)
        {
            return Problem("Entity set 'FootballSimulatorAPIContext.MatchResult'  is null.");
        }
        _context.MatchResult.Add(matchResult);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetMatchResult", new { matchId = matchResult.matchId }, matchResult);
    }

    [HttpDelete("{matchId}")]
    public async Task<IActionResult> DeleteMatchResult(int matchId)
    {
        if (_context.Match == null)
        {
            return NotFound();
        }
        var matchResult = await _context.MatchResult.FindAsync(matchId);
        if (matchResult == null)
        {
            return NotFound();
        }

        _context.MatchResult.Remove(matchResult);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
