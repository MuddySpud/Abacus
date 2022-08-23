using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace MuddySpud.Metrics.Engine
{
    public static class CSharp3
    {
        public static void StringAndCommentTest(
            this List<TagCounter> tagCounts,
            string tag,
            int count = 1)
        {
            // Comment 1
            string fred = "//";

            // Comment 2 //
            string dave = "////";
            // Comment 3 "//
            #region test
            string joe = "////";
            /*
             2 line "
            comment // */
            string bert = @"
2 line
string";
            /*
             " */
            // Comment 4 //
            string baz = @" "" ";
            // Comment 5 //
            string daz = @" ""joe"" ";
            /* Comment 6
             2 line */
            #endregion
            /*

            @" 

*/        /* comment 7 */
            int ben = 0;
            /*

            " 

*/
            string ken = "ken";
#pragma warning disable CS8604 // Possible null reference argument.
            string stu = " \"ben\" ";

            string jane = @""""; // Comment 8
            string kate = "" + ""; // Comment 9
            string harry = "" + @"

// Not a comment"; // Comment 10
            string kev = "" + @"

"" ""// Not a comment"; // Comment 11
            string kay = "" + @"

"" ""// " // Comment 12
            string bev = "" + /*@"

"" ""// */ // Comment 13
            string trev = "" + @"/*

"" */"" // not a comment
"
            string jack = "" + @"/*

"" */""" // Comment 14
            string rob = "" + @"/*

"" */""" // Comment 14 /* in comment */
        }

        string Method1() => "Fred was here";
        string Method2() => "Fred was \"NOT here";
        string Method3() => "Fred was NOT\" here";
        string Method4() => "Fred was \"NOT\" here";

        string Method5() => @"Fred
was
here ""LATE""";

        string Method6() => @"Fred was here";
        string Method7() => @"Fred was here" + " today";
        string Method8() => @"Fred was ""NOT\ here";
        string Method9() => @"Fred was NOT"" here";
        string Method10() => @"Fred was ""NOT"" here";
        string Method11() => $"Fred was here: {DateTime.Now.ToString()} 2 \"hours\" late";
        string Method12() => @$"Fred was here: {DateTime.Now.ToString()} 2 hours "" late";
        string Method13() => @$"Fred was here: {DateTime.Now.ToString()} 2 ""hours "" late";
        string Method14() => $@"Fred was here: {DateTime.Now.ToString()} 2 ""hours "" late";
        string Method15() => $"Fred was here: {DateTime.Now.ToString("yyyy/MM/dd_HH:mm:ss::fff")} 2 hours late";
        string Method16() => $"Fred was here: {DateTime.Now.ToString("yyyy/MM/dd_HH:mm:ss::fff")}";
        string Method17() => $"{DateTime.Now.ToString("yyyy/MM/dd_HH:mm:ss::fff")} late";
        string Method18() => $"{DateTime.Now.ToString("yyyy/MM/dd_HH:mm:ss::fff")} late {{fred}}";
        string Method19() => $"{{fred}}";

        string Method20() => $"Fred was here: { String.IsNullOrWhiteSpace(test1) }";
    }
}
