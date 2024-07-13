using System.Collections.Generic;

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
        }

        public int GetScore()
        {
            if (this.Frames.Count != 10)
            {
                throw new System.InvalidOperationException();
            }

            return 0;
        }
    }

    public class Frame
    {
    }
}