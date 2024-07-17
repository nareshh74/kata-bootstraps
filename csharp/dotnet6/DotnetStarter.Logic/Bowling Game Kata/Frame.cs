using System.Collections.Generic;
using System.Linq;

namespace BowlingGameKata;

public class Frame
{
    private readonly int _index;
    public List<Roll> Rolls { get; }

    private Frame(int index)
    {
        this._index = index;
        this.Rolls = new();
    }

    public void Roll(int knockedPinCount)
    {
        this.Rolls.Add(new Roll(knockedPinCount));
    }

    public bool IsComplete()
    {
        // 10th frame
        if (this._index == 9)
        {
            if (this.GetScore() >= 10)
            {
                return this.Rolls.Count == 3;
            }
            return this.Rolls.Count == 2;
        }

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
            return new Frame(frameIndex);
        }
}