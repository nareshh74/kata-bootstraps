namespace BowlingGameKata
{
    public class Game
    {
        public Frame[] Frames { get; }

        public Game()
        {
            Frames = new Frame[10];
            for (int i = 0; i < Frames.Length; i++)
            {
                Frames[i] = new Frame();
            }
        }

        public void Roll(int knockedPinCount)
        {
        }
    }

    public class Frame
    {
    }
}