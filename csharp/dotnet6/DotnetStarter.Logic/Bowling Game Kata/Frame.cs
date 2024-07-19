using System.Collections.Generic;
using System.Linq;

namespace BowlingGameKata;

public class Frame
{
    private readonly Frame _nextFrame;
    public List<Roll> Rolls { get; }

    private Frame(Frame nextFrame)
    {
        this._nextFrame = nextFrame;
        this.Rolls = new();
    }

    public void Roll(int knockedPinCount)
    {
        this.Rolls.Add(new Roll(knockedPinCount));
    }

    public virtual bool IsComplete()
    {
        // strike/spare
        if (this.Rolls.Count > 0 && this.Rolls[0].GetScore() == 10)
        {
            return true;
        }

        return this.Rolls.Count == 2; // normal case
    }

    public virtual int GetScore()
    {
        int score = this.Rolls.Sum(roll => roll.GetScore());
        if (score >= 10)
        {
            // spare
            if (this.Rolls.Count == 2)
            {
                score += this._nextFrame.GetFirstRollScore();
            }
            // strike
            else if (this.Rolls.Count == 1)
            {
                score += this._nextFrame.GetFirstRollScore();
                if (this._nextFrame.Rolls.Count >= 2)
                {
                    score += this._nextFrame.Rolls[1].GetScore();
                }
                else if (this._nextFrame.Rolls.Count == 1 && this._nextFrame._nextFrame != null)
                {
                    score += this._nextFrame._nextFrame.GetFirstRollScore();
                }
            }
        }

        return score;
    }

    public int GetFirstRollScore()
    {
        return this.Rolls.First().GetScore();
    }

    public static Frame New(int frameIndex, Frame nextFrame)
    {
        if(frameIndex == 9)
        {
            return new TenthFrame(nextFrame);
        }
        return new Frame(nextFrame);
    }

    private class TenthFrame : Frame
    {
        public TenthFrame(Frame nextFrame) : base(nextFrame) { }

        public override bool IsComplete()
        {
            if (this.GetScore() >= 10)
            {
                return this.Rolls.Count == 3;
            }
            return this.Rolls.Count == 2;
        }

        public override int GetScore()
        {
            return this.Rolls.Sum(roll => roll.GetScore());
        }
    }
}