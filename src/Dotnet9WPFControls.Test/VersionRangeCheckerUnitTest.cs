using Dotnet9WPFControls.Helpers;

namespace Dotnet9WPFControls.Test
{
    public class VersionRangeCheckerUnitTest
    {
        [Fact]
        public void IsInRange_Success()
        {
            var expression = "(0.0,90.0),100.0,(103.0,104.0],[109.0,120.0),[130.0,],105.0";
            var versions = new string[]{"87.0","100.0","104.0","110.0","130.0", "132.0","105.0"};

            foreach (var version in versions)
            {
                bool isInRange = VersionRangeChecker.IsInRange(version, expression);
                Assert.True(isInRange);
            }
        }
    }
}