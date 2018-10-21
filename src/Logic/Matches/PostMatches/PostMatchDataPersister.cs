using System.Linq;
using TwoNil.Data;

namespace TwoNil.Logic.Matches.PostMatches
{
    public class PostMatchDataPersister
    {
        private readonly IUnitOfWork _uow;

        public PostMatchDataPersister(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public void Persist(PostMatchData postMatchData)
        {
            // Update teams
            _uow.Teams.Update(postMatchData.Teams.Values);

            // Update LeagueTables en positions.
            _uow.LeagueTables.Update(postMatchData.LeagueTables.Values);
            _uow.LeagueTablePositions.Update(postMatchData.LeagueTables.Values.SelectMany(x => x.LeagueTablePositions));

            // Update statistics
            _uow.SeasonStatics.Update(postMatchData.SeasonStatistics);
            _uow.SeasonTeamStatistics.Update(postMatchData.SeasonTeamStatistics.Values);
            _uow.TeamStatistics.Update(postMatchData.TeamStatistics.Values);

            // Insert new cup matches
            if (postMatchData.CupMatchesNextRound.Any())
                _uow.Matches.Add(postMatchData.CupMatchesNextRound);

            // Insert new friendly matches
            if (postMatchData.DuringSeasonFriendlies.Any())
                _uow.Matches.Add(postMatchData.DuringSeasonFriendlies);
        }
    }
}
