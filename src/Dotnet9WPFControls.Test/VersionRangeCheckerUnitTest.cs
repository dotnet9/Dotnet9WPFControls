using Dotnet9WPFControls.Helpers;

namespace Dotnet9WPFControls.Test
{
    public class VersionRangeCheckerUnitTest
    {
        [Fact]
        public void IsInRange_Success()
        {
            var expression = "(,90)||100||(103,104)||(105,120)||125||(126,)";
            var inRangeVersion = new[] { "32.32.32", "100", "103.2", "105.3", "125", "130" };
            var notInRangeVersion = new[] { "90", "90.2", "102.23.32.23", "104", "104.2.3.5", "120", "123.0" };

            foreach (var version in inRangeVersion)
            {
                bool isInRange = VersionRangeChecker.IsInRange(version, expression);
                Assert.True(isInRange);
            }

            foreach (var version in notInRangeVersion)
            {
                bool isInRange = VersionRangeChecker.IsInRange(version, expression);
                Assert.False(isInRange);
            }
        }

        [Fact]
        public void CompareVersoin_Success()
        {
            var versions = new[] { "1", "2", "3.2.3.3", "3.3", "4.2.5", "5" };
            for (int i = 0; i < versions.Length; i++)
            {
                for (var j = i + 1; j < versions.Length; j++)
                {
                    var version1 = CustomVersion.Parse(versions[i]);
                    var version2 = CustomVersion.Parse(versions[j]);
                    Assert.True(version1 < version2);
                }
            }
        }
    }
}