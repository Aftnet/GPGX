﻿using LibRetriX.BeetleNGP;
using LibRetriX.Test.Base;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace LibRetriX.Test.UWP
{
    public class BeetleNGPTest : CoreTestBase
    {
        public static IEnumerable<object[]> RomNames => new List<object[]>
        {
            new object[] { "NeoGeoPocketGame.ngc" },
        };

        public BeetleNGPTest() : base(() => Core.Instance)
        {
        }

        [Theory]
        [MemberData(nameof(RomNames))]
        public override Task LoadingRomWorks(string romName)
        {
            return LoadingRomWorksInternal(romName);
        }

        [Theory]
        [MemberData(nameof(RomNames))]
        public override Task ExecutionWorks(string romName)
        {
            return ExecutionWorksInternal(romName);
        }
    }
}
