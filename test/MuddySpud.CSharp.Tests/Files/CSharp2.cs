using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace MuddySpud.Metrics.Engine
{
    public static class CSharp2
    {
        public static void MergeTagCounts(
            this List<TagCounter> a,
            List<TagCounter> b)
        {
            foreach (TagCounter tagCountB in b)
            {
                a.IncrementTagCount(
                    tagCountB.Tag,
                    tagCountB.Count
                );
            }
        }

        public static void IncrementTagCount(
            this List<TagCounter> tagCounts,
            string tag,
            int count = 1)
        {
            TagCounter tagCount;

            for (int i = 0; i < tagCounts.Count; i++)
            {
                tagCount = tagCounts[i];

                if (String.Equals(tagCount.Tag, tag))
                {
                    tagCount.Count += count;

                    return;
                }
            }

            tagCounts.Add(new TagCounter(tag, 1));
        }
    }
}
