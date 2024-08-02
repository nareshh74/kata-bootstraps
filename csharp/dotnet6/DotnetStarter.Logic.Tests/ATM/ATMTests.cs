using System;
using System.IO;
using DotnetStarter.Logic.ATM;
using DotnetStarter.Logic.ATM.Domain;
using Xunit;

namespace DotnetStarter.Logic.Tests.ATM;

public class AtmShould
{
    public class WithdrawShould
    {
        [Theory]
        [InlineData(100, "1 bill of 100.")]
        [InlineData(50, "1 bill of 50.")]
        [InlineData(150, @"1 bill of 100.
1 bill of 50.")]
        [InlineData(434, @"2 bills of 200.
1 bill of 20.
1 bill of 10.
2 coins of 2.")]
        public void Display_expected_result(int withdrawalAmount, string expected)
        {
            using StringWriter sw = new();
            Console.SetOut(sw);

            var atm = Atm.Create();
            atm.WithDraw(withdrawalAmount);

            var actual = sw.ToString().Trim();
            Assert.True(expected.Equals(actual), $"expected - {expected} | actual - {actual}");
        }

        [Fact]
        public void Handle_not_enough_money()
        {
            using StringWriter sw = new();
            Console.SetOut(sw);

            var atm = Atm.Create();
            atm.WithDraw(10000);

            var actual = sw.ToString().Trim();
            Assert.True("Not enough money in ATM.".Equals(actual), $"expected - Not enough money in ATM. | actual - {actual}");
        }
    }

    public class CreateShould
    {
        [Fact]
        public void Create_atm()
        {
            var atm = Atm.Create();

            Assert.NotNull(atm);
        }
    }
}