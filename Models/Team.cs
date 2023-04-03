namespace FootballSimulatorAPI.Models;

public class Team
{
    public int Id { get; set; }

    public string Name { get; set; }
    public double BaseAttack { get; set; } = 1;
    public double BaseDefense { get; set; } = 1;
    public double HomeAdvantage { get; set; } = 1.25;
    public string Color { get; set; } = "#ffffff";

    public double GetAttack(bool onHomeGround = false)
    {
        return BaseAttack * (onHomeGround ? HomeAdvantage : 1);
    }

    public double GetDefense(bool onHomeGround = false)
    {
        return BaseDefense * (onHomeGround ? HomeAdvantage : 1);
    }
}