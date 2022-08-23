using System.IO;
using System.Threading.Tasks;

namespace MuddySpud.Rule.Generator
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string line;
            string rule;
            int counter = 1000;

            string inputPath = "C:\\Code4\\ApplicationInspector-main\\MuddySpud.Rule.Generator\\keywords.txt";
            string outputPath = "C:\\Code4\\ApplicationInspector-main\\MuddySpud.Rule.Generator\\keywordRules.json";

            if (File.Exists(@"C:\test.txt"))
            {
                File.Delete(@"C:\test.txt");
            }

            using (StreamReader input = new StreamReader(inputPath))
            using (StreamWriter output = new StreamWriter(outputPath))
            {
                await output.WriteLineAsync("[");

                while ((line = input.ReadLine()) != null)
                {
                    rule = BuildRule(
                        line,
                        counter += 100);

                    await output.WriteLineAsync(rule);
                }

                await output.WriteLineAsync("]");
            }
        }

        private static string BuildRule(
            string keyword,
            int counter)
        {
            string pattern = "00000000";

            string rule = 
$@"{{
    ""name"": ""C-m: CSharp {keyword}"",
    ""id"": ""CY{counter.ToString(pattern)}"",
    ""description"": ""C-m: CSharp {keyword}"",
    ""applies_to"": [ ""csharp"" ],
    ""tags"": [
      ""C-m.Code.CSharp.{keyword}""
    ],
    ""severity"": ""moderate"",
    ""_comment"": """",
    ""patterns"": [
      {{
        ""pattern"": ""{keyword}"",
        ""type"": ""RegexWord"",
        ""scopes"": [ ""code"" ],
        ""confidence"": ""medium"",
        ""_comment"": """"
      }}
    ]
  }},";


            return rule;
        }
    }
}
