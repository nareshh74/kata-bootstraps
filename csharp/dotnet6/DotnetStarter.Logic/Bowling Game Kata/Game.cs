using System.Collections.Generic;
using System.Linq;

namespace BowlingGameKata
{
    public class Game
    {
        private readonly List<Frame> _frames;

        public IReadOnlyCollection<Frame> Frames => this._frames.AsReadOnly();

        public Game()
        {
            this._frames = new();
        }

        public void Roll(int knockedPinCount)
        {
            if (this.IsComplete())
            {
                throw new System.InvalidOperationException();
            }

            Frame previousFrame = this.Frames.LastOrDefault()
                , currentFrame = previousFrame;

            if(previousFrame?.IsComplete() ?? true)
            {
                this._frames.Add(Frame.New(this.Frames.Count));
                currentFrame = this.Frames.Last();
            }

            currentFrame.Roll(knockedPinCount);
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
                var currentFrameScore = this._frames[i].GetScore();
                score += currentFrameScore;
                if (currentFrameScore == 10 && i + 1 < 10)
                {
                    // spare
                    if (this._frames[i].Rolls.Count == 2)
                    {
                        score += this._frames[i + 1].GetFirstRollScore();
                    }

                    // strike
                    else if (this._frames[i].Rolls.Count == 1)
                    {
                        score += this._frames[i + 1].GetFirstRollScore();
                        if (this._frames[i + 1].Rolls.Count == 1)
                        {
                            if (i + 2 < 10)
                            {
                                score += this._frames[i + 2].Rolls[0].GetScore();
                            }
                        }
                        else
                        {
                            score += this._frames[i + 1].Rolls[1].GetScore();
                        }
                    }
                }
            }
            return score;
        }

        public bool IsComplete()
        {
            return this._frames.Count == 10 && this._frames.Last().IsComplete();
        }
    }
}