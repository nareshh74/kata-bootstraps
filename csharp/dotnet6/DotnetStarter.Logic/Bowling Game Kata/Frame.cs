using System.Collections.Generic;
using System.Linq;

namespace BowlingGameKata;

public class Frame
{
    private readonly Frame _nextFrame;
    private readonly ScoreCalculatorFactory _scoreCalculatorFactory;
    public List<Roll> Rolls { get; }
    public Frame NextFrame => this._nextFrame;

    private Frame(Frame nextFrame, ScoreCalculatorFactory scoreCalculatorFactory)
    {
        this._nextFrame = nextFrame;
        this._scoreCalculatorFactory = scoreCalculatorFactory;
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
        return this._scoreCalculatorFactory.Create(this).GetScore();
    }

    public int GetFirstRollScore()
    {
        return this.Rolls.First().GetScore();
    }

    public static Frame New(int frameIndex, Frame nextFrame, ScoreCalculatorFactory scoreCalculatorFactory)
    {
        if(frameIndex == 9)
        {
            return new TenthFrame(nextFrame, scoreCalculatorFactory);
        }
        return new Frame(nextFrame, scoreCalculatorFactory);
    }

    private class TenthFrame : Frame
    {
        public TenthFrame(Frame nextFrame, ScoreCalculatorFactory scoreCalculatorFactory) : base(nextFrame, scoreCalculatorFactory) { }

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