using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FootballSimulatorAPI.Models;

namespace FootballSimulatorAPI.Models;

public class FootballSimulatorAPIContext : DbContext
{
    public FootballSimulatorAPIContext(DbContextOptions<FootballSimulatorAPIContext> options)
        : base(options)
    {
    }

    public DbSet<Team> Team { get; set; } = default!;
    public DbSet<Match> Match { get; set; } = default!;
    public DbSet<MatchResult> MatchResult { get; set; } = default!;
}