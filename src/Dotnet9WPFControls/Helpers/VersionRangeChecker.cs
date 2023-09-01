using System;
using System.Collections.Generic;

namespace Dotnet9WPFControls.Helpers
{
    /// <summary>
    /// 判断一个版本号是否在给定的判断表达式内，比如表达式字符串为“(0,90),100,(103,104],[109,120),[130,],105”,类似数学中的范围表示方式，"("和")"表示不包含,“[”和“]”表示包含，“(0,90)”表示0到90之间，“(103,104]”表示大于103且小于等于104，"(109,120)"表示大于109且小于120，"[130,]"表示大于等于130，”105“表示等于 105
    /// </summary>
    public static class VersionRangeChecker
    {
        /// <summary>
        /// 判断版本号是否在指定范围内
        /// </summary>
        /// <param name="versionStr"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static bool IsInRange(string versionStr, string expression)
        {
            var version = Version.Parse(versionStr);
            // 解析表达式，获取每个范围的上下界和包含关系
            List<Range> ranges = ParseExpression(expression);

            // 遍历每个范围，判断数字是否在范围内
            foreach (Range range in ranges)
            {
                if (range.IsInRange(version))
                {
                    return true;
                }
            }

            return false;
        }

        public static List<Range> ParseExpression(string expression)
        {
            List<Range> ranges = new List<Range>();

            // 分割表达式，获取每个范围的字符串表示
            string[] rangeStrings = expression.Split(',');

            foreach (string rangeString in rangeStrings)
            {
                Range range = new Range();

                // 判断范围的包含关系
                if (rangeString.StartsWith("("))
                {
                    range.IncludeLowerBound = false;
                }
                else if (rangeString.StartsWith("["))
                {
                    range.IncludeLowerBound = true;
                }

                if (rangeString.EndsWith(")"))
                {
                    range.IncludeUpperBound = false;
                }
                else if (rangeString.EndsWith("]"))
                {
                    range.IncludeUpperBound = true;
                }

                // 解析范围的上下界
                var bounds = rangeString.TrimStart('(').TrimStart('[').TrimEnd(')').TrimEnd(']').Split(',');

                if (bounds.Length > 0 && !string.IsNullOrEmpty(bounds[0]))
                {
                    range.LowerBound = Version.Parse(bounds[0]);
                }


                if (bounds.Length > 1 && !string.IsNullOrEmpty(bounds[1]))
                {
                    range.UpperBound = Version.Parse(bounds[1]);
                }

                ranges.Add(range);
            }

            return ranges;
        }
    }

    public class Range
    {
        public Version? LowerBound { get; set; }
        public Version? UpperBound { get; set; }
        public bool IncludeLowerBound { get; set; }
        public bool IncludeUpperBound { get; set; }

        public bool IsInRange(Version number)
        {
            bool isInRange = true;

            if (LowerBound != null)
            {
                if (IncludeLowerBound)
                {
                    isInRange = number >= LowerBound;
                }
                else
                {
                    isInRange = number > LowerBound;
                }
            }

            if (UpperBound != null)
            {
                if (IncludeUpperBound)
                {
                    isInRange = isInRange && number <= UpperBound;
                }
                else
                {
                    isInRange = isInRange && number < UpperBound;
                }
            }

            return isInRange;
        }
    }
}