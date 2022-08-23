using System;

namespace MuddySpud.RulesEngine
{
    internal static class CSharpNameInterpreter
    {
        internal static void BuildFullNames(
            this BlockStats block,
            string nameStub,
            string joiner,
            string fullContent,
            string strippedContent,
            bool setBlockContent = false)
        {
            string nameQualifier;

            foreach (BlockStats child in block.ChildBlocks)
            {
                if (block.Settings.Model == "Block")
                {
                    nameQualifier = nameStub;

                    if (!String.IsNullOrWhiteSpace(nameStub))
                    {
                        nameQualifier += joiner;
                    }

                    if (!string.IsNullOrWhiteSpace(child.Name))
                    {
                        child.FullName = $"{nameQualifier}{child.Name}";
                    }

                    if (child.QualifiesName)
                    {
                        nameQualifier = child.FullName;
                    }
                    else
                    {
                        nameQualifier = nameStub;
                    }
                }
                else
                {
                    nameQualifier = String.Empty;
                }

                if (setBlockContent
                    && child.CloseIndex > 0)
                {
                    child.Content = fullContent.Substring(
                        child.AdjustedOpenIndex,
                        child.AdjustedCloseIndex - child.AdjustedOpenIndex);

                    child.StrippedContent = strippedContent.Substring(
                        child.OpenIndex,
                        child.CloseIndex - child.OpenIndex);
                }

                child.BuildFullNames(
                    nameQualifier,
                    joiner,
                    fullContent,
                    strippedContent,
                    setBlockContent);
            }
        }
    }
}
