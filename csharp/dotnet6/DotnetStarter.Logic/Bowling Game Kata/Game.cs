using System.Collections.Generic;
using System.Linq;

namespace BowlingGameKata
{
    public class Game
    {
        public List<Frame> Frames { get; }

        public Game()
        {
            this.Frames = new();
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
                this.Frames.Add(Frame.New(this.Frames.Count));
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
                var currentFrameScore = this.Frames[i].GetScore();
                score += currentFrameScore;
                if (currentFrameScore == 10 && i + 1 < 10)
                {
                    // spare
                    if (this.Frames[i].Rolls.Count == 2)
                    {
                        score += this.Frames[i + 1].GetFirstRollScore();
                    }

                    // strike
                    else if (this.Frames[i].Rolls.Count == 1)
                    {
                        score += this.Frames[i + 1].GetFirstRollScore();
                        if (this.Frames[i + 1].Rolls.Count == 1)
                        {
                            if (i + 2 < 10)
                            {
                                score += this.Frames[i + 2].Rolls[0].GetScore();
                            }
                        }
                        else
                        {
                            score += this.Frames[i + 1].Rolls[1].GetScore();
                        }
                    }
                }
            }
            return score;
        }

        public bool IsComplete()
        {
            return this.Frames.Count == 10 && this.Frames.Last().IsComplete();
        }
    }
}