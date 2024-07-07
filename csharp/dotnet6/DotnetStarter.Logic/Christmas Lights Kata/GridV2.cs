using System;

namespace GridLogic
{
    public class GridV2 : IGridOperations
    {
        private readonly int[,] _lights;

        public GridV2(int width, int height)
        {
            this._lights = new int[width, height];
        }

        private int Width => this._lights.GetLength(0);
        private int Height => this._lights.GetLength(1);

        public int GetLightCount() => this.Width * this.Height;

        public void TurnOn(Tuple<int, int> startPosition, Tuple<int, int> endPosition)
        {
            for (int x = startPosition.Item1; x <= endPosition.Item1; x++)
            {
                for (int y = startPosition.Item2; y <= endPosition.Item2; y++)
                {
                    this._lights[x, y]++;
                }
            }
        }

        public void TurnOff(Tuple<int, int> startPosition, Tuple<int, int> endPosition)
        {
            for (int x = startPosition.Item1; x <= endPosition.Item1; x++)
            {
                for (int y = startPosition.Item2; y <= endPosition.Item2; y++)
                {
                    this._lights[x, y] = Math.Max(0, this._lights[x, y] - 1);
                }
            }
        }

        public void Toggle(Tuple<int, int> startPosition, Tuple<int, int> endPosition)
        {
            for (int x = startPosition.Item1; x <= endPosition.Item1; x++)
            {
                for (int y = startPosition.Item2; y <= endPosition.Item2; y++)
                {
                    this._lights[x, y] += 2;
                }
            }
        }

        public int GetTotalBrightness()
        {
            int count = 0;
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    count += this._lights[x, y];
                }
            }
            return count;
        }
    }
}
