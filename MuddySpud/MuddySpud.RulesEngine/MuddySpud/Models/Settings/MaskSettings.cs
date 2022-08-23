
using System.Collections.Generic;

namespace MuddySpud.RulesEngine
{
    public class MaskSettings
    {
        public List<string> Boundaries { get; } = new();
        public List<FacetSettings> Facets { get; } = new();

        public MaskSettings(
            List<string> boundaries,
            List<FacetSettings> facets)
        {
            Boundaries = boundaries;
            Facets = facets;
        }
    }
}