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
            this.Frames = Game.CreateFrames().AsReadOnly();
            this._currentFrameIndex = 0;
        }

        private static List<Frame> CreateFrames()
        {
            var frames = new List<Frame>();
            Frame currentFrame = null, nextFrame = null;
            for (int i = 9; i >= 0; i--)
            {
                currentFrame = Frame.New(i, nextFrame);
                frames.Insert(0, currentFrame);
                nextFrame = currentFrame;
            }

            return frames;
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
                score += this.Frames.ElementAt(i).GetScore();
            }
            return score;
        }

        public bool IsComplete()
        {
            return this._currentFrameIndex == 10;
        }
    }
}