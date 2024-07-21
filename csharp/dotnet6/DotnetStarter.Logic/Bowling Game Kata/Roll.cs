namespace BowlingGameKata;

public class Roll
{
    public int KnockedPinCount { get; }

    public Roll(int knockedPinCount)
    {
        this.KnockedPinCount = knockedPinCount;
    }

    public int GetScore()
    {
        return this.KnockedPinCount;
    }
}