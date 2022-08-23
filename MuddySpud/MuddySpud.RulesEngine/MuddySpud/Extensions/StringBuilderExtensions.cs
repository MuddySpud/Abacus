using System;
using System.Text;

namespace MuddySpud.Metrics.Engine
{
    public static class StringBuilderExtensions
    {
        public static int IndexOfSlow(
            this StringBuilder haystack,
            string needle,
            int startSearchIndex)
        {
            int index = -1;

            if (haystack == null
                || String.IsNullOrEmpty(needle))
            {
                throw new ArgumentNullException();
            }

            char firstChar = needle[0];
            int start = startSearchIndex;

            while (start < haystack.Length)
            {
                for (int idx = start; idx != haystack.Length; ++idx)
                {
                    if (haystack[idx] == firstChar)
                    {
                        index = idx;

                        break;
                    }
                }

                if (index == -1)
                {
                    return -1;
                }

                if (haystack.CompareSection(needle, index))
                {
                    return index;
                }

                start = index + 1;
                index = -1;
            }

            return -1;
        }

        public static (int index, int needleIndex) IndexOfAny(
            this StringBuilder content,
            char[] needleFirstCharacters,
            string[] needles,
            int startSearchIndex)
        {
            int needleIndex = -1;
            int index = -1;

            while (needleIndex < 0)
            {
                index = content.IndexOfAny(
                    needleFirstCharacters,
                    startSearchIndex);

                if (index < 0)
                {
                    return (-1, -1);
                }

                int i = 0;

                for (; i < needles.Length; i++)
                {
                    if (content.CompareSection(needles[i], index))
                    {
                        return (index, i);
                    }
                }

                startSearchIndex = index + 1;
            }

            return (index, needleIndex);
        }

        public static bool Contains(
            this StringBuilder haystack,
            string needle,
            int startIndex)
        {
            return haystack.IndexOf(needle, startIndex) != -1;
        }

        public static int LastIndexOf(
            this StringBuilder haystack,
            char needle,
            int startIndex)
        {
            if (haystack == null)
            {
                throw new ArgumentNullException();
            }

            for (int idx = startIndex; idx >= 0; idx--)
            {
                if (haystack[idx] == needle)
                {
                    return idx;
                }
            }

            return -1;
        }

        public static int IndexOfAny(
            this StringBuilder haystack,
            char[] needles,
            int startIndex)
        {
            if (haystack == null
                || needles == null
                || needles.Length == 0)
            {
                throw new ArgumentNullException();
            }

            for (int idx = startIndex; idx != haystack.Length; ++idx)
            {
                for (int i = 0; i < needles.Length; i++)
                {
                    if (haystack[idx] == needles[i])
                    {
                        return idx;
                    }
                }
            }

            return -1;
        }

        public static bool CompareSection(
            this StringBuilder haystack,
            string needle,
            int startIndex)
        {
            if (haystack == null
                || needle == null
                || needle == String.Empty)
            {
                throw new ArgumentNullException();
            }

            for (int i = 0; i < needle.Length; i++)
            {
                if (haystack[startIndex + i] != needle[i])
                {
                    return false;
                }
            }

            return true;
        }

        public static int IndexOf(
            this StringBuilder haystack,
            string needle,
            int startIndex,
            int stopIndex)
        {
            if (haystack == null
                || needle == null)
            {
                throw new ArgumentNullException();
            }

            if (needle.Length == 0)
            {
                return 0;//empty strings are everywhere!
            }

            if (needle.Length == 1)//can't beat just spinning through for it
            {
                char c = needle[0];

                for (int idx = startIndex; idx < stopIndex; ++idx)
                {
                    if (haystack[idx] == c)
                    {
                        return idx;
                    }
                }

                return -1;
            }

            int m = startIndex;
            int i = 0;
            int[] T = KMPTable(needle);

            while (m + i < stopIndex)
            {
                if (needle[i] == haystack[m + i])
                {
                    if (i == needle.Length - 1)
                    {
                        return m == needle.Length ? -1 : m;//match -1 = failure to find conventional in .NET
                    }

                    ++i;
                }
                else
                {
                    m = m + i - T[i];
                    i = T[i] > -1 ? T[i] : 0;
                }
            }

            return -1;
        }

        public static int IndexOf(
            this StringBuilder haystack,
            char needle,
            int startIndex)
        {
            if (haystack == null)
            {
                throw new ArgumentNullException();
            }

            for (int idx = startIndex; idx != haystack.Length; ++idx)
            {
                if (haystack[idx] == needle)
                {
                    return idx;
                }
            }

            return -1;
        }

        public static int IndexOf(
            this StringBuilder haystack,
            string needle,
            int startIndex)
        {
            //return IndexOf(
            //    haystack,
            //    needle,
            //    startIndex,
            //    haystack.Length);

            return IndexOfSlow(
                haystack,
                needle,
                startIndex
            );
        }

        public static bool IsMatchAt(
            this StringBuilder haystack,
            string needle,
            int startIndex)
        {
            for (int i = 0; i < needle.Length; ++i)
            {
                if (haystack[i + startIndex] != needle[i])
                {
                    return false;
                }
            }

            return true;
        }

        public static bool IsMatchAnyAt(
            this StringBuilder haystack,
            string[] needles,
            int startIndex)
        {
            string needle;
            bool success = true;

            for (int j = 0; j < needles.Length; ++j)
            {
                needle = needles[j];
                success = true;

                for (int i = 0; i < needle.Length; ++i)
                {
                    if (haystack[i + startIndex] != needle[i])
                    {
                        success = false;

                        break;
                    }
                }
            }

            return success;
        }

        private static int[] KMPTable(string sought)
        {
            int[] table = new int[sought.Length];
            int pos = 2;
            int cnd = 0;
            table[0] = -1;
            table[1] = 0;

            while (pos < table.Length)
            {
                if (sought[pos - 1] == sought[cnd])
                {
                    table[pos++] = ++cnd;
                }
                else if (cnd > 0)
                {
                    cnd = table[cnd];
                }
                else
                {
                    table[pos++] = 0;
                }
            }

            return table;
        }
    }
}
