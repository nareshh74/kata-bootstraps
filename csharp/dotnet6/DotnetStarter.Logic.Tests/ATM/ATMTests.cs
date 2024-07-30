using System.IO;
using System;
using DotnetStarter.Logic.ATM;
using Xunit;

namespace ATMTests;

public class AtmTests
{
    [Theory]
    [InlineData(100, "1 bill of 100.")]
    [InlineData(200, "2 bills of 100.")]
    public void Atm_should_display_expected_result_after_withdrawal(int withdrawalAmount, string expected)
    {
        using StringWriter sw = new();
        Console.SetOut(sw);

        AtmClient.WithDraw(withdrawalAmount);

        var actual = sw.ToString().Trim();
        Assert.True(expected.Equals(actual), $"expected - {expected} | actual - {actual}");
    }
}