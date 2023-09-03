using System;
using System.Collections.Generic;

namespace Dotnet9WPFControls.Helpers
{
    /// <summary>
    ///     判断一个版本号是否在给定的判断表达式内，比如表达式字符串为“(0,90)||100||(103,104)||(105,120)||125”,类似数学中的区间表示方式，"("和")"表示不包含,“[”和“]”表示包含，“(0,90)”表示0到90之间，“(103,104]”表示大于103且小于等于104，"(109,120)"表示大于109且小于120，"[130,]"表示大于等于130，”105“表示等于
    ///     105
    /// </summary>
    public static class VersionRangeChecker
    {
        /// <summary>
        ///     判断版本号是否在指定范围内
        /// </summary>
        /// <param name="versionStr"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static bool IsInRange(string versionStr, string expression)
        {
            CustomVersion version = CustomVersion.Parse(versionStr);
            // 解析表达式，获取每个范围的上下界和包含关系
            List<VersionRange> ranges = ParseExpression(expression);

            // 遍历每个范围，判断版本号是否在范围内
            foreach (VersionRange range in ranges)
            {
                if (range.IsInRange(version))
                {
                    return true;
                }
            }

            return false;
        }

        public static List<VersionRange> ParseExpression(string expression)
        {
            List<VersionRange> ranges = new();

            // 分割表达式，获取每个范围的字符串表示
            string[] rangeStrings = expression.Split("||", StringSplitOptions.RemoveEmptyEntries);

            foreach (string rangeString in rangeStrings)
            {
                VersionRange range = new();
                if (CustomVersion.TryParse(rangeString, out CustomVersion version))
                {
                    range.EqualsBound = version;
                    ranges.Add(range);
                    continue;
                }

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
                string[] bounds = rangeString.TrimStart('(').TrimStart('[').TrimEnd(')').TrimEnd(']').Split(',');

                if (bounds.Length > 0 && !string.IsNullOrEmpty(bounds[0]))
                {
                    range.LowerBound = CustomVersion.Parse(bounds[0]);
                }


                if (bounds.Length > 1 && !string.IsNullOrEmpty(bounds[1]))
                {
                    range.UpperBound = CustomVersion.Parse(bounds[1]);
                }

                ranges.Add(range);
            }

            return ranges;
        }
    }

    public class VersionRange
    {
        public CustomVersion? EqualsBound { get; set; }
        public CustomVersion? LowerBound { get; set; }
        public CustomVersion? UpperBound { get; set; }
        public bool IncludeLowerBound { get; set; }
        public bool IncludeUpperBound { get; set; }

        public bool IsInRange(CustomVersion number)
        {
            // 配置的单个数
            if (EqualsBound != null)
            {
                return number == EqualsBound;
            }

            bool isBiggerThan = false;
            // 配置是否大小（或大于等于）
            if (LowerBound != null)
            {
                if (IncludeLowerBound)
                {
                    isBiggerThan = number >= LowerBound;
                }
                else
                {
                    isBiggerThan = number > LowerBound;
                }
            }
            // 配置负无穷
            else
            {
                isBiggerThan = true;
            }

            bool isSmallerThan = false;
            //  配置是否小于（或小于等等）
            if (UpperBound != null)
            {
                if (IncludeUpperBound)
                {
                    isSmallerThan = number <= UpperBound;
                }
                else
                {
                    isSmallerThan = number < UpperBound;
                }
            }
            // 配置正无穷
            else
            {
                isSmallerThan = true;
            }

            return isBiggerThan && isSmallerThan;
        }
    }

    public class CustomVersion : IComparable<CustomVersion>
    {
        private readonly List<int> _versionNumbers;

        public CustomVersion(string versionString)
        {
            _versionNumbers = new List<int>();
            string[] numbers = versionString.Split('.');
            foreach (string number in numbers)
            {
                if (int.TryParse(number, out int num)) { _versionNumbers.Add(num); }
                else { throw new ArgumentException("Invalid version string"); }
            }
        }

        public int CompareTo(CustomVersion? other)
        {
            if (other == null) { return 1; }

            int minLength = Math.Min(_versionNumbers.Count, other._versionNumbers.Count);
            for (int i = 0; i < minLength; i++)
            {
                if (_versionNumbers[i] < other._versionNumbers[i]) { return -1; }

                if (_versionNumbers[i] > other._versionNumbers[i]) { return 1; }
            }

            if (_versionNumbers.Count < other._versionNumbers.Count) { return -1; }

            return _versionNumbers.Count > other._versionNumbers.Count ? 1 : 0;
        }

        public static CustomVersion Parse(string versionStr)
        {
            return new CustomVersion(versionStr);
        }

        public static bool TryParse(string versionStr, out CustomVersion version)
        {
            try
            {
                version = new CustomVersion(versionStr);
                return true;
            }
            catch
            {
                version = null;
                return false;
            }
        }

        public override string ToString() { return string.Join(".", _versionNumbers); }

        public static bool operator >(CustomVersion v1, CustomVersion v2) { return v1.CompareTo(v2) > 0; }
        public static bool operator >=(CustomVersion v1, CustomVersion v2) { return v1.CompareTo(v2) >= 0; }
        public static bool operator <(CustomVersion v1, CustomVersion v2) { return v1.CompareTo(v2) < 0; }
        public static bool operator <=(CustomVersion v1, CustomVersion v2) { return v1.CompareTo(v2) <= 0; }

        public static bool operator ==(CustomVersion? v1, CustomVersion? v2)
        {
            if (ReferenceEquals(v1, v2)) { return true; }

            if (ReferenceEquals(v1, null) || ReferenceEquals(v2, null)) { return false; }

            return v1.CompareTo(v2) == 0;
        }

        public static bool operator !=(CustomVersion v1, CustomVersion v2) { return !(v1 == v2); }

        public override bool Equals(object? obj)
        {
            if (obj is CustomVersion other) { return this == other; }

            return false;
        }

        public override int GetHashCode() { return _versionNumbers.GetHashCode(); }
    }
}