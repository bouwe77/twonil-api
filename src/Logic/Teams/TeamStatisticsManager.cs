using System.Collections.Generic;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.Logic.Teams
{
    public class TeamStatisticsManager
    {
        private readonly Dictionary<string, TeamStatistics> _teamStatistics;

        public TeamStatisticsManager(Dictionary<string, TeamStatistics> teamStatistics)
        {
            _teamStatistics = teamStatistics;
        }

        public void Update(IEnumerable<LeagueTable> leagueTables, Dictionary<string, League> leagues)
        {
            foreach (var leagueTable in leagueTables)
            {
                var leagueStatisticsScore = leagues[leagueTable.SeasonCompetition.CompetitionId].StatisticsScore;

                foreach (var position in leagueTable.LeagueTablePositions)
                {
                    var positionScore = leagueStatisticsScore - position.Position;
                    var stat = _teamStatistics[position.TeamId];
                    stat.LeagueTablePositions += positionScore + ",";
                }
            }
        }
    }
}
