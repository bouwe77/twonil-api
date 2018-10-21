using System.Collections.Generic;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.Logic.Matches.MatchPlay
{
    public class PenaltyTaker
    {
        private readonly MatchPlayerRandomizer _randomizer;

        public PenaltyTaker(MatchPlayerRandomizer randomizer)
        {
            _randomizer = randomizer;
        }

        public void TakePenalties(Match match)
        {
            var possiblePenaltyScores = new Dictionary<int, float>
            {
                { 5, 40 }, { 4, 35 }, { 3, 30 }, { 2, 18 }, { 1, 4 }, { 0, 1 },
                { 6, 4 }, { 7, 3 }, { 8, 2 }, { 9, 1 }, { 10, 1 }, { 11, 1 }, { 12, 1 }, { 13, 1 },
            };

            match.HomePenaltyScore = _randomizer.HomePenaltyScore(possiblePenaltyScores);
            match.AwayPenaltyScore = _randomizer.AwayPenaltyScore(possiblePenaltyScores);
            match.PenaltiesTaken = true;

            // Randomly pick a winner if the penalty shootout also ended undecisive.
            bool penaltyDraw = match.HomePenaltyScore == match.AwayPenaltyScore;
            if (penaltyDraw)
            {
                bool homeTeamWinsPenalties = _randomizer.HomeTeamWinsPenalties();
                if (homeTeamWinsPenalties)
                {
                    match.HomePenaltyScore++;
                }
                else
                {
                    match.AwayPenaltyScore++;
                }
            }
            else
            {
                CorrectIfNecessary(match);
            }
        }

        private static void CorrectIfNecessary(Match match)
        {
            // As the penalty shootout score is determined randomly without any specific logic, it might be unrealistic according to the rules.
            // The following goal differences are accepted:
            //  * 1 is always accepted.
            //  * 2 is only accepted if the winner scored maximum of 5 penalties.
            //  * 3 is only accepted if the winner scored maximum of 4 penalties.
            //  * All other goal differences are not accepted.
            int diff = match.AwayPenaltyScore - match.HomePenaltyScore;
            int winnerScore = match.AwayPenaltyScore;
            bool hometeamWins = false;
            if (match.HomePenaltyScore > match.AwayPenaltyScore)
            {
                winnerScore = match.HomePenaltyScore;
                diff = match.HomePenaltyScore - match.AwayPenaltyScore;
                hometeamWins = true;
            }

            bool correctionNecessary =
               (diff == 2 && winnerScore > 5) ||
               (diff == 3 && winnerScore > 4) ||
               diff > 3;
            if (correctionNecessary)
            {
                if (hometeamWins)
                {
                    match.AwayPenaltyScore = match.HomePenaltyScore - 1;
                }
                else
                {
                    match.HomePenaltyScore = match.AwayPenaltyScore - 1;
                }
            }
        }
    }
}
