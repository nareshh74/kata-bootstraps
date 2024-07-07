namespace GridLogic
{
    public class GridFactory
    {
        public static IGridOperations CreateLightGrid(int width, int height)
        {
            return new Grid(width, height);
        }

        public static IGridOperations CreateLightGridV2(int width, int height)
        {
            return new GridV2(width, height);
        }
    }
}
