namespace GridLogic
{
    public class GridFactory
    {
        public static IGridOperations CreateLightGrid(int width, int height)
        {
            return new Grid(width, height);
        }
    }
}
