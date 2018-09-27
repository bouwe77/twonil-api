using System.Collections.Generic;
using System.Linq;
using Shally.Hal;
using TwoNil.Services;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.API.Resources
{
    namespace TwoNil.API.Resources
    {
        public class SeasonListResourceFactory
        {
            private readonly GameInfo _gameInfo;
            private readonly SeasonMapper _seasonMapper;
            private readonly string _contextUriPlaceholder;

            public SeasonListResourceFactory(GameInfo gameInfo, UriHelper uriHelper, string contextUriPlaceholder)
            {
                _gameInfo = gameInfo;
                _seasonMapper = new SeasonMapper(uriHelper);
                _contextUriPlaceholder = contextUriPlaceholder;
            }

            public IEnumerable<Resource> Create()
            {
                var seasonService = new ServiceFactory().CreateSeasonService(_gameInfo);
                var seasons = seasonService.GetAll().ToList();

                var seasonResources = _seasonMapper.Map(seasons, SeasonMapper.SeasonShortName, SeasonMapper.SeasonLongName).ToList();

                for (int i = 0; i < seasonResources.Count; i++)
                {
                    string contextUri = _contextUriPlaceholder.Replace("###seasonid###", seasons[i].Id);
                    seasonResources[i].AddLink("context", new Link(contextUri));
                }

                return seasonResources;
            }
        }
    }
}
