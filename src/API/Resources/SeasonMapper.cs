using System.Collections.Generic;
using System.Linq;
using Shally.Hal;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.API.Resources
{
    public class SeasonMapper : IResourceMapper<Season>
    {
        private readonly UriHelper _uriHelper;
        public static string SeasonShortName = "short-name";
        public static string SeasonLongName = "long-name";

        public SeasonMapper(UriHelper uriHelper)
        {
            _uriHelper = uriHelper;
        }

        public Resource Map(Season season, params string[] properties)
        {
            var resource = new Resource(new Link(_uriHelper.GetSeasonUri(season.GameId, season.Id)));

            if (properties.Contains(SeasonShortName))
            {
                resource.AddProperty(SeasonShortName, season.ShortName);
            }

            if (properties.Contains(SeasonLongName))
            {
                resource.AddProperty(SeasonLongName, season.LongName);
            }

            return resource;
        }

        public IEnumerable<Resource> Map(IEnumerable<Season> seasons, params string[] properties)
        {
            var resources = new List<Resource>();

            foreach (var season in seasons)
            {
                var resource = Map(season, properties);
                resources.Add(resource);
            }

            return resources;
        }
    }
}