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
        [InlineData(1725, @"2 bills of 500.
3 bills of 200.
1 bill of 100.
1 bill of 20.
1 bill of 5.")]
        public void Display_expected_result(int withdrawalAmount, string expected)
        {
            using StringWriter sw = new();
            Console.SetOut(sw);

            var atm = Atm.Create(Atm.DefaultMoneyStock, Atm.DefaultDisplay);
            atm.WithDraw(withdrawalAmount);

            var actual = sw.ToString().Trim();
            Assert.True(expected.Equals(actual), $"expected - {expected} | actual - {actual}");
        }

        [Fact]
        public void Handle_not_enough_money()
        {
            using StringWriter sw = new();
            Console.SetOut(sw);

            var atm = Atm.Create(Atm.DefaultMoneyStock, Atm.DefaultDisplay);
            atm.WithDraw(10000);

            var actual = sw.ToString().Trim();
            Assert.True("Not enough money in ATM.".Equals(actual), $"expected - Not enough money in ATM. | actual - {actual}");
        }

        [Fact]
        public void Work_as_expected_when_withdrawn_in_sequence()
        {
            using StringWriter sw = new();
            Console.SetOut(sw);

            var atm = Atm.Create(Atm.DefaultMoneyStock, Atm.DefaultDisplay);
            atm.WithDraw(1725);
            atm.WithDraw(1825);

            var actual = sw.ToString().Trim();
            var expected = @"2 bills of 500.
3 bills of 200.
1 bill of 100.
1 bill of 20.
1 bill of 5.

4 bills of 100.
12 bills of 50.
19 bills of 20.
44 bills of 10.
1 bill of 5.";
            Assert.True(expected.Equals(actual), $"expected - {expected}. | actual - {actual}");
        }
    }

    public class CreateShould
    {
        [Fact]
        public void Create_atm()
        {
            var atm = Atm.Create(Atm.DefaultMoneyStock, Atm.DefaultDisplay);

            Assert.NotNull(atm);
        }
    }
}