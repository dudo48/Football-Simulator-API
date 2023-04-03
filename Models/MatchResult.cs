using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace FootballSimulatorAPI.Models;

// single result
public class Result
{
    [JsonIgnore]
    public int Id { get; set; }

    public int HomeTeam { get; set; }
    public int AwayTeam { get; set; }

    public Result(int homeTeam, int awayTeam)
    {
        HomeTeam = homeTeam;
        AwayTeam = awayTeam;
    }
}

public class MatchResult
{
    [Key]
    public int matchId { get; set; }

    public Result StandardTimeResult { get; set; }
    public Result? ExtraTimeResult { get; set; }
    public Result? PenaltyShootoutResult { get; set; }
}
