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
            this.Frames.Add(new Frame());
        }

        public void Roll(int knockedPinCount)
        {
            if (this.IsComplete())
            {
                throw new System.InvalidOperationException();
            }

            var currentFrame = this.Frames.Last();
            currentFrame.Roll(knockedPinCount);
            if (currentFrame.IsComplete())
            {
                this.Frames.Add(new Frame());
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
            if (this.Frames.Count < 11)
            {
                return false;
            }

            if (this.Frames.Count > 11)
            {
                return true;
            }

            return this.Frames[9].GetScore() != 10;
        }
    }

    public class Frame
    {
        public List<Roll> Rolls { get; }

        public Frame()
        {
            this.Rolls = new();
        }

        public void Roll(int knockedPinCount)
        {
            this.Rolls.Add(new Roll(knockedPinCount));
        }

        public bool IsComplete()
        {
            return this.Rolls.Count == 2 || this.Rolls.First().GetScore() == 10;
        }

        public int GetScore()
        {
            return this.Rolls.Sum(roll => roll.GetScore());
        }

        public int GetFirstRollScore()
        {
            return this.Rolls.First().GetScore();
        }
    }

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
}