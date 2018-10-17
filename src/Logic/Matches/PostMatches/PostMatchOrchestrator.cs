using System.Collections.Generic;
using TwoNil.Data;
using TwoNil.Logic.Matches.PostMatches.Handlers;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.Logic.Matches.PostMatches
{
    public interface IPostMatchOrchestrator
    {
        void Handle(IEnumerable<Match> matches);
    }

    /// <summary>
    /// Orchestrates which data in which order must be saved after matches have been played.
    /// </summary>
    public class PostMatchOrchestrator : IPostMatchOrchestrator
    {
        private readonly IEnumerable<Match> _matches;
        private readonly string _seasonId;
        private readonly IUnitOfWork _uow;
        private readonly IPostMatchDataFactory _postMatchDataFactory;
        private readonly IPostMatchDataPersister _postMatchDataPersister;

        public PostMatchOrchestrator(IUnitOfWork uow, IPostMatchDataFactory postMatchDataFactory, IPostMatchDataPersister postMatchDataPersister)
        {
            _uow = uow;
            _postMatchDataFactory = postMatchDataFactory;
            _postMatchDataPersister = postMatchDataPersister;
        }

        public void Handle(IEnumerable<Match> matches)
        {
            var postMatchData = _postMatchDataFactory.InitializePostMatchData(matches);

            //TODO deze news kunnen niet meer, dus welk pattern moet ik toepassen? Waarschijnlijk kan ik via Unity vragen om mij alle geregistreerde IPostMatchHandlers terug te geven?
            var handlers = new List<IPostMatchesHandler>
            {
                //new LeagueTableHandler(),
                //new DrawNextCupRoundHandler(),
                //new GenerateDuringSeasonFriendliesHandler(),
                //new SeasonStatisticsHandler(),
                //new SeasonTeamStatisticsHandler(),
                //new TeamStatisticsHandler(),
                //new TeamHandler(),
                //new GameDateTimeHandler(),
            };

            foreach (var postMatchHandler in handlers)
            {
                postMatchHandler.Handle(postMatchData);
            }

            _postMatchDataPersister.Persist(postMatchData);
        }
    }
}