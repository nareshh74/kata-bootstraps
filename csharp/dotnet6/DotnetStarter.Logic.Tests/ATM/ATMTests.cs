using System;
using System.IO;
using DotnetStarter.Logic.ATM;
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
        public void display_expected_result(int withdrawalAmount, string expected)
        {
            using StringWriter sw = new();
            Console.SetOut(sw);

            AtmClient.WithDraw(withdrawalAmount);

            var actual = sw.ToString().Trim();
            Assert.True(expected.Equals(actual), $"expected - {expected} | actual - {actual}");
        }
    }
}