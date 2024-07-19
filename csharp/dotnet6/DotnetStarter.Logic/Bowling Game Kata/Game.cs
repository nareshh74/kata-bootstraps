using System.Collections.Generic;
using System.Linq;

namespace BowlingGameKata
{
    public class Game
    {
        private readonly List<Frame> _frames;
        private int _currentFrameIndex;

        public IReadOnlyCollection<Frame> Frames { get; }

        public Game()
        {
            var frames = new List<Frame>();
            for (int i = 0; i < 10; i++)
            {
                frames.Add(Frame.New(i));
            }

            this.Frames = frames.AsReadOnly();
            this._currentFrameIndex = 0;
        }

        public void Roll(int knockedPinCount)
        {
            if (this.IsComplete())
            {
                throw new System.InvalidOperationException();
            }

            this.Frames.ElementAt(this._currentFrameIndex).Roll(knockedPinCount);

            if (this.Frames.ElementAt(this._currentFrameIndex).IsComplete())
            {
                this._currentFrameIndex++;
            }
        }

        public int GetScore()
        {
            if (!this.IsComplete())
            {
                throw new System.InvalidOperationException();
            }

            var score = 0;
            for (int i = 0; i < 10; i++)
            {
                var frame = this.Frames.ElementAt(i);
                var currentFrameScore = frame.GetScore();
                score += currentFrameScore;
                if (currentFrameScore == 10 && i + 1 < 10)
                {
                    // spare
                    if (frame.Rolls.Count == 2)
                    {
                        score += this.Frames.ElementAt(i + 1).GetFirstRollScore();
                    }

                    // strike
                    else if (frame.Rolls.Count == 1)
                    {
                        score += this.Frames.ElementAt(i + 1).GetFirstRollScore();
                        if (this.Frames.ElementAt(i + 1).Rolls.Count == 1)
                        {
                            if (i + 2 < 10)
                            {
                                score += this.Frames.ElementAt(i + 2).Rolls[0].GetScore();
                            }
                        }
                        else
                        {
                            score += this.Frames.ElementAt(i + 1).Rolls[1].GetScore();
                        }
                    }
                }
            }
            return score;
        }

        public bool IsComplete()
        {
            return this._currentFrameIndex == 10;
        }
    }
}