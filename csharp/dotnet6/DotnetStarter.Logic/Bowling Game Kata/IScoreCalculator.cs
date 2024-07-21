using System.Linq;

namespace BowlingGameKata
{
    public interface IScoreCalculator
    {
        public int GetScore();
    }

    public class DefaultScoreCalculator : IScoreCalculator
    {
        private readonly Frame _frame;

        public DefaultScoreCalculator(Frame frame)
        {
            this._frame = frame;
        }

        public int GetScore()
        {
            return this._frame.Rolls.Sum(roll => roll.GetScore());
        }
    }

    public class StrikeScoreCalculator : IScoreCalculator
    {
        private readonly Frame _frame;

        public StrikeScoreCalculator(Frame frame)
        {
            this._frame = frame;
        }

        public int GetScore()
        {
            int score = this._frame.Rolls.Sum(roll => roll.GetScore());
            score += this._frame.NextFrame.GetFirstRollScore();
            if (this._frame.NextFrame.Rolls.Count >= 2)
            {
                score += this._frame.NextFrame.Rolls[1].GetScore();
            }
            else if (this._frame.NextFrame.Rolls.Count == 1 && this._frame.NextFrame.NextFrame != null)
            {
                score += this._frame.NextFrame.NextFrame.GetFirstRollScore();
            }

            return score;
        }
    }

    public class SpareScoreCalculator : IScoreCalculator
    {
        private readonly Frame _frame;

        public SpareScoreCalculator(Frame frame)
        {
            this._frame = frame;
        }

        public int GetScore()
        {
            int score = this._frame.Rolls.Sum(roll => roll.GetScore());
            score += this._frame.NextFrame.GetFirstRollScore();
            return score;
        }
    }

    public class ScoreCalculatorFactory
    {
        public IScoreCalculator Create(Frame frame)
        {
            if (frame.Rolls.Count == 1 && frame.Rolls[0].GetScore() == 10)
            {
                return new StrikeScoreCalculator(frame);
            }
            if (frame.Rolls.Count == 2 && frame.Rolls.Sum(roll => roll.GetScore()) == 10)
            {
                return new SpareScoreCalculator(frame);
            }
            return new DefaultScoreCalculator(frame);
        }
    }
}
