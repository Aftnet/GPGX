﻿using System.Threading.Tasks;
using Xunit;

namespace LibretroRT.Test.Cores
{
    public class ParallelN64CoreTest : TestBase
    {
        protected const string RomName = "Super Mario Bros 3.nes";

        public ParallelN64CoreTest() : base(() => ParallelN64RT.ParallelN64Core.Instance)
        {
        }

        [Theory]
        [InlineData(RomName)]
        public override Task LoadingRomWorks(string romName)
        {
            return LoadingRomWorksInternal(romName);
        }

        [Theory]
        [InlineData(RomName)]
        public override Task ExecutionWorks(string romName)
        {
            return ExecutionWorksInternal(romName);
        }
    }
}
