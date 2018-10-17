using System.Collections.Generic;
using System.Linq;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.Logic.Matches
{
    public static class MatchExtensions
    {
        /// <summary>
        /// Determines the match ended in a draw.
        /// If penalties where taken after a draw, the method returns true.
        /// Even if the match is not ended yet, the method still returns whether it is a draw or not.
        /// </summary>
        /// <param name="match">The match.</param>
        /// <returns>True if HomeScore and AwayScore are equal, otherwise false.</returns>
        public static bool EndedInDraw(this Match match)
        {
            return match.HomeScore == match.AwayScore;
        }

        /// <summary>
        /// Determines the winner of the match.
        /// If the match has not ended, no winner is returned.
        /// If the match ended in a draw and no penalties were taken, no winner is returned.
        /// </summary>
        /// <param name="match">the match to determine the winner for.</param>
        /// <returns>The winner, or null.</returns>
        public static Team GetWinner(this Match match)
        {
            Team winner = null;

            if (match.MatchStatus == MatchStatus.Ended)
            {
                winner = match.HomeTeam;

                if (match.AwayScore == match.HomeScore)
                {
                    if (match.PenaltiesTaken)
                    {
                        if (match.AwayPenaltyScore > match.HomePenaltyScore)
                        {
                            winner = match.AwayTeam;
                        }
                    }
                    else
                    {
                        winner = null;
                    }
                }
                else if (match.AwayScore > match.HomeScore)
                {
                    winner = match.AwayTeam;
                }
            }

            return winner;
        }

        public static Team GetLoser(this Match match)
        {
            var winner = match.GetWinner();

            if (winner == null)
                return null;

            return match.HomeTeamId == winner.Id ? match.AwayTeam : match.HomeTeam;
        }

        public static void SwapHomeAndAway(this Match match)
        {
            var temp = match.HomeTeam;
            match.HomeTeam = match.AwayTeam;
            match.AwayTeam = temp;
        }

        /// <summary>
        /// Determines whether the specified team already plays a match (either home or away) in the given list of matches.
        /// </summary>
        /// <param name="matches">The matches.</param>
        /// <param name="team">The team.</param>
        /// <returns>True or false.</returns>
        public static bool TeamPlaysMatch(this List<Match> matches, Team team)
        {
            return matches.Any(match => match.HomeTeamId == team.Id || match.AwayTeamId == team.Id);
        }

        /// <summary>
        /// Determines whether the specified team plays a match (either home or away) in the given match.
        /// </summary>
        /// <param name="matches">The match.</param>
        /// <param name="team">The team.</param>
        /// <returns>True or false.</returns>
        public static bool TeamPlaysMatch(this Match match, Team team)
        {
            return match.HomeTeamId == team.Id || match.AwayTeamId == team.Id;
        }

        /// <summary>
        /// Determines whether the specified match already exists in the list of matches by looking at the home and away teams.
        /// </summary>
        /// <param name="matches">The matches.</param>
        /// <param name="match">The match.</param>
        /// <returns>True or false.</returns>
        public static bool MatchExists(this List<Match> matches, Match match)
        {
            return matches.Any(m => (m.HomeTeamId == match.HomeTeamId && m.AwayTeamId == match.AwayTeamId)
                                    || (m.HomeTeamId == match.AwayTeamId && m.AwayTeamId == match.HomeTeamId));
        }
    }
}
