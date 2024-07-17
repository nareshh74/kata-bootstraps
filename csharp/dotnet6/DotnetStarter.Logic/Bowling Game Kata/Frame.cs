using System.Collections.Generic;
using System.Linq;

namespace BowlingGameKata;

public class Frame
{
    public List<Roll> Rolls { get; }

    private Frame(int index)
    {
        this.Rolls = new();
    }

    public void Roll(int knockedPinCount)
    {
        this.Rolls.Add(new Roll(knockedPinCount));
    }

    public virtual bool IsComplete()
    {
        return this.Rolls.First().GetScore() == 10 // strike/spare
               || this.Rolls.Count == 2; // normal case
    }

    public int GetScore()
    {
        return this.Rolls.Sum(roll => roll.GetScore());
    }

    public int GetFirstRollScore()
    {
        return this.Rolls.First().GetScore();
    }

    public static Frame New(int frameIndex)
    {
        if(frameIndex == 9)
        {
            return new TenthFrame(frameIndex);
        }
        return new Frame(frameIndex);
    }

    private class TenthFrame : Frame
    {
        public TenthFrame(int index) : base(index) { }

        public override bool IsComplete()
        {
            if (this.GetScore() >= 10)
            {
                return this.Rolls.Count == 3;
            }
            return this.Rolls.Count == 2;
        }
    }
}