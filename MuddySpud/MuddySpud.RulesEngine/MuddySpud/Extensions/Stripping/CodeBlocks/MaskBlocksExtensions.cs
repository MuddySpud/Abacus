using MuddySpud.RulesEngine;
using System;
using System.Collections.Generic;
using System.Text;

/*
    "mask": {
      "boundaries": [
        "Curly"
      ],
      "enabled": true,
      "facets": [
        {
          "name": "switch expression",
          "keyword": "switch",
          "blocks": [
            "Code"
          ],
          "enabled": true
        }
      ]
    },

    Find any blocks listed in boundaries
    Use them to run scan blocks
    Then use facets to group them 
    On those groups replace any non white spaces char with an x
    Return this new stringbuilder 
 */
namespace MuddySpud.Metrics.Engine
{
    static class MaskBlocksExtensions
    {
        internal static StringBuilder MaskContent(
            this StringBuilder content,
            CodeBlockLoopCache cache)
        {
            if (cache.CodeBlockSettings.Mask is null)
            {
                return content;
            }

            List<string> opens = new();
            List<char> openFirstChars = new();

            foreach (string name in cache.CodeBlockSettings.Mask.Boundaries)
            {
                foreach (BlockBoundarySettings boundary in cache.CodeBlockSettings.Boundaries)
                {
                    if (String.Equals(boundary.Name, name, StringComparison.Ordinal))
                    {
                        opens.Add(boundary.Open);
                        openFirstChars.Add(boundary.Open[0]);
                    }
                }
            }

            content.ScanForBlocks(
                cache,
                opens.ToArray(),
                openFirstChars.ToArray()
            );

            cache.RootCodeBlock.CollectChildren(cache.BlockStats);
            cache.BlockStats.Sort(BlockStatsExtensions.Compare);

            string text = content.ToString();
            StringBuilder maskedContent = new(text);

            // Run Facet like a block in content but save nothing just replace the text with xxx
            foreach (FacetSettings facetSettings in cache.CodeBlockSettings.Mask.Facets)
            {
                cache.BlockStats.FindFacet(
                    facetSettings,
                    maskedContent,
                    text
                );
            }

            return maskedContent;
        }

        private static void CollectChildren(
            this BlockStats block,
            List<BlockStats> stats)
        {
            foreach (BlockStats child in block.ChildBlocks)
            {
                stats.Add(child);
                child.CollectChildren(stats);
            }
        }
    }
}
