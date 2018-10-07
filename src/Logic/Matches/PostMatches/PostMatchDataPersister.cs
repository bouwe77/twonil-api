using System.Linq;
using TwoNil.Data;

namespace TwoNil.Logic.Matches.PostMatches
{
    public class PostMatchDataPersister
    {
        private readonly ITransactionManager _transactionManager;
        private readonly PostMatchData _postMatchData;

        public PostMatchDataPersister(ITransactionManager transactionManager, PostMatchData postMatchData)
        {
            _transactionManager = transactionManager;
            _postMatchData = postMatchData;
        }

        public void Persist()
        {
            // Update teams
            _transactionManager.RegisterUpdate(_postMatchData.Teams.Values);

            // Update LeagueTables en positions.
            _transactionManager.RegisterUpdate(_postMatchData.LeagueTables.Values);
            _transactionManager.RegisterUpdate(_postMatchData.LeagueTables.Values.SelectMany(x => x.LeagueTablePositions));

            // Update statistics
            _transactionManager.RegisterUpdate(_postMatchData.SeasonStatistics);
            _transactionManager.RegisterUpdate(_postMatchData.SeasonTeamStatistics.Values);
            _transactionManager.RegisterUpdate(_postMatchData.TeamStatistics.Values);

            // Insert new cup matches
            if (_postMatchData.CupMatchesNextRound.Any())
                _transactionManager.RegisterInsert(_postMatchData.CupMatchesNextRound);

            // Insert new friendly matches
            if (_postMatchData.DuringSeasonFriendlies.Any())
                _transactionManager.RegisterInsert(_postMatchData.DuringSeasonFriendlies);


        }
    }
}
