using System.Collections.Generic;
using TwoNil.Data;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.Logic.Matches.PostMatches
{
    /// <summary>
    /// Orchestrates which data in which order must be saved after matches have been played.
    /// </summary>
    public class PostMatchOrchestrator
    {
        private readonly TransactionManager _transactionManager;
        private readonly IRepositoryFactory _repositoryFactory;
        private readonly IEnumerable<Match> _matches;
        private readonly string _seasonId;

        public PostMatchOrchestrator(TransactionManager transactionManager, IRepositoryFactory repositoryFactory)
        {
            _transactionManager = transactionManager;
            _repositoryFactory = repositoryFactory;
        }

        public void Handle(IEnumerable<Match> matches)
        {
            var postMatchData = new PostMatchDataFactory(_repositoryFactory, matches).InitializePostMatchData();

            var handlers = new List<IPostMatchesHandler>
            {
                new LeagueTableHandler(_repositoryFactory),
                new DrawNextCupRoundHandler(_repositoryFactory),
                new GenerateDuringSeasonFriendliesHandler(_repositoryFactory),
                //new SeasonStatisticsHandler(_transactionManager, _repositoryFactory),
                //new SeasonTeamStatisticsHandler(_repositoryFactory),
                //new TeamStatisticsHandler(_repositoryFactory),
                //new GameDateTimeHandler(_repositoryFactory),
                //new TeamHandler(),
            };

            foreach (var postMatchHandler in handlers)
            {
                postMatchHandler.Handle(postMatchData);
            }

            //TODO POSTMATCH Hier dus PostMatchData in de TransactionManager stoppen
            //TODO POSTMATCH Let trouwens op dat je naast de LeagueTable ook de LeagueTablePositions erin stopt, beide RegisterUpdate.
        }
    }
}