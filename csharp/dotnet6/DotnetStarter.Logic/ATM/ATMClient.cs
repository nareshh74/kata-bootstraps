using DotnetStarter.Logic.ATM.Domain;

namespace DotnetStarter.Logic.ATM
{
    public class AtmClient
    {
        public static void WithDraw(int amount)
        {
            var atm = Atm.Create();
            atm.WithDraw(amount);
        }
    }
}
