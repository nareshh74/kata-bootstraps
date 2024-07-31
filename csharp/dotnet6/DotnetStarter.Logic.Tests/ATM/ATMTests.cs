using System.IO;
using System;
using DotnetStarter.Logic.ATM;
using Xunit;

namespace ATMTests;

public class AtmTests
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
    public void Atm_should_display_expected_result_after_withdrawal(int withdrawalAmount, string expected)
    {
        using StringWriter sw = new();
        Console.SetOut(sw);

        AtmClient.WithDraw(withdrawalAmount);

        var actual = sw.ToString().Trim();
        Assert.True(expected.Equals(actual), $"expected - {expected} | actual - {actual}");
    }
}