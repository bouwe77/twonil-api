using System.Collections.Generic;
using System.Linq;
using TwoNil.Logic.Matches;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.Logic.Teams
{
    public class SeasonTeamStatisticsManager
    {
        private readonly IDictionary<string, SeasonTeamStatistics> _seasonTeamStatistics;

        public SeasonTeamStatisticsManager(IDictionary<string, SeasonTeamStatistics> seasonTeamStatistics, string seasonId)
        {
            _seasonTeamStatistics = seasonTeamStatistics;
        }

        public void Update(string seasonId, IEnumerable<Match> matches, LeagueTable leagueTable)
        {
            foreach (var match in matches)
            {
                UpdateTeamStatistics(seasonId, match);
            }

            UpdateLeagueTablePositions(seasonId, leagueTable);
        }

        private void UpdateTeamStatistics(string seasonId, Match match)
        {
            if (match.MatchStatus == MatchStatus.Ended)
            {
                char homeMatchResult = 'D';
                char awayMatchResult = 'D';

                if (!match.EndedInDraw())
                {
                    if (match.GetWinner().Id == _seasonTeamStatistics[match.HomeTeamId].TeamId)
                    {
                        homeMatchResult = 'W';
                        awayMatchResult = 'L';
                    }
                    else
                    {
                        homeMatchResult = 'L';
                        awayMatchResult = 'W';
                    }
                }

                _seasonTeamStatistics[match.HomeTeamId].MatchResults += homeMatchResult + ",";
                _seasonTeamStatistics[match.AwayTeamId].MatchResults += awayMatchResult + ",";
            }
        }

        private void UpdateLeagueTablePositions(string seasonId, LeagueTable leagueTable)
        {
            var teams = leagueTable.LeagueTablePositions.Select(x => x.Team);

            foreach (var team in teams)
            {
                var teamStatistics = _seasonTeamStatistics[team.Id];
                teamStatistics.CurrentLeagueTablePosition = team.CurrentLeaguePosition;
                teamStatistics.LeagueTablePositions += team.CurrentLeaguePosition + ",";
            }
        }
    }
}