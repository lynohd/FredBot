using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FredBot.Services;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FredBot.Tests;
public class TimeGuessrTests
{
    //TimeGuessrService _service;

    //public TimeGuessrTests()
    //{
    //    _service = new TimeGuessrService();
    //}

    //[InlineData("TimeGuessr #210 50000/50000")]
    //[InlineData("TimeGuessr #210 50,000/50000")]
    //[InlineData("TimeGuessr #210 1,200/50000")]
    //[InlineData("TimeGuessr #210 1200/50000")]
    //[InlineData("TimeGuessr #210 5/50000")]
    //[InlineData("TimeGuessr #210 0/50000")]
    //[Theory]
    //public void IS_VALID_REGEX(string text)
    //{
    //    var success = _service.IsValidTimeGuessr().Match(text).Success;

    //    Assert.True(success);
    //}

    //[InlineData(50_000)]
    //[InlineData(1_000)]
    //[InlineData(15)]
    //[InlineData(0)]
    //[Theory]
    //public void IS_VALID_VALUE(int score)
    //{
    //    string text = "TimeGuessr #210 */50000";

    //    text = text.Replace("*", score.ToString());
    //    var match = _service.IsValidTimeGuessr().Match(text).Success;

    //    Assert.True(match);
    //}

    //[Fact]
    //public void IS_NO_HIGHER_THAN_50_000()
    //{
    //    var results = _service.IsValidTimeGuessr().Match("TimeGuessr #210 50001/50000");

    //    var colls = results.Captures.ToList();

    //    Assert.False(results.Success);
    //}
}
