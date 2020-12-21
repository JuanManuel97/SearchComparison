using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using SearchComparison.BL;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SearchComparison.Test
{
    [TestClass]
    public class UnitTest1
    {
        //private ISearchLogic _searchLogic;

        //[SetUp]
        //public void Init()
        //{
        //    _searchLogic = SearchLogicConstr.CreateSearchLogic();
        //}

        [TestMethod]
        public async Task checkResults()
        {
            var _searchLogic = SearchLogicConstr.CreateSearchLogic();

            var querys = new List<string>() { "net", "java" };

            var result = await _searchLogic.GetResultsReport(querys);

            result.Should().BeOfType<string>();
        }
    }
}
