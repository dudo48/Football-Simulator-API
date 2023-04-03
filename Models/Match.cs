using MathNet.Numerics.Distributions;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace FootballSimulatorAPI.Models;

public class Match
{
    public int Id { get; set; }
    public int HomeTeamId { get; set; }
    public int AwayTeamId { get; set; }
    public int? HostTeamId { get; set; }

    public MatchResult? MatchResult { get; set; }
    public Team? HomeTeam { get; set; }
    public Team? AwayTeam { get; set; }
    public Team? HostTeam { get; set; }

    private double GetXG(Team attacker, Team defender, double minutes = Constants.STANDARD_TIME_MINUTES)
    {
        double relativeAttack = attacker.GetAttack(attacker == HostTeam) / defender.GetDefense(defender == HostTeam);
        return Math.Sqrt(relativeAttack) * (minutes / Constants.STANDARD_TIME_MINUTES) * (Constants.AVERAGE_MATCH_GOALS / 2);
    }

    private static int PredictGoals(double xg)
    {
        Poisson poissonDistribution = new Poisson(xg);
        return poissonDistribution.Sample();
    }

    public MatchResult Simulate()
    {
        MatchResult result = new MatchResult();
        result.matchId = Id;

        double homeTeamXG = GetXG(HomeTeam, AwayTeam);
        double awayTeamXG = GetXG(AwayTeam, HomeTeam);

        result.StandardTimeResult = new Result(PredictGoals(homeTeamXG), PredictGoals(awayTeamXG));

        return result;
    }
}
