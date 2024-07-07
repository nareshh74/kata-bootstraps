using System;

namespace GridLogic
{
    public interface IGridOperations
    {
        public int GetLightCount();
        public int GetTotalBrightness();
        public void Toggle(Tuple<int, int> startPosition, Tuple<int, int> endPosition);
        public void TurnOff(Tuple<int, int> startPosition, Tuple<int, int> endPosition);
        public void TurnOn(Tuple<int, int> startPosition, Tuple<int, int> endPosition);
    }
}