using System;
using System.Collections.Generic;
using System.Linq;
using TwoNil.Data;
using TwoNil.Logic.Matches;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.Logic.Competitions
{
   public class NationalCupManager
   {
      private IRepositoryFactory _repositoryFactory;
      private Competition _competition;

      public NationalCupManager(IRepositoryFactory repositoryFactory)
      {
         _repositoryFactory = repositoryFactory;

         using (var competitionRepository = new RepositoryFactory().CreateCompetitionRepository())
         {
            _competition = competitionRepository.GetNationalCup();
         }
      }

      public CompetitionSchedule CreateSchedule(List<Team> teams, Season season, MatchDateManager matchDateManager)
      {
         var competitionSchedule = new CompetitionSchedule();

         // Create a cup season competition.
         SeasonCompetition cupSeasonCompetition = new SeasonCompetition
         {
            Competition = _competition,
            Season = season
         };

         competitionSchedule.SeasonCompetitions.Add(cupSeasonCompetition);

         var cupSchedule = new KnockoutTournamentManager().GetSchedule(teams);

         int numberOfRounds = DetermineNumberOfRounds(teams.Count);

         var firstScheduleItem = cupSchedule.First();
         var matchDate = matchDateManager.GetNextMatchDate(CompetitionType.NationalCup, firstScheduleItem.Key);

         // Create the first round and its matches.
         int roundIndex = 0;
         var firstRound = RoundFactory.CreateRound(GetCupRoundName(numberOfRounds, roundIndex), cupSeasonCompetition, matchDate, roundIndex, _competition);

         foreach (var match in firstScheduleItem.Value)
         {
            match.Season = season;
            match.Round = firstRound;
            match.Date = matchDate;
            match.DrawPermitted = false;
            match.CompetitionId = _competition.Id;
            competitionSchedule.Matches.Add(match);
         }

         competitionSchedule.Rounds.Add(firstRound);

         // Create remaining rounds for the tournament, these rounds do not have matches yet.
         // The date on which the matches on these rounds will be played are stored in the round.
         int numberOfRoundsLeft = numberOfRounds - 1;
         if (numberOfRoundsLeft > 0)
         {
            for (int i = 0; i < numberOfRoundsLeft; i++)
            {
               roundIndex++;

               matchDate = matchDateManager.GetNextMatchDate(CompetitionType.NationalCup, roundIndex);
               var round = RoundFactory.CreateRound(GetCupRoundName(numberOfRounds, roundIndex), cupSeasonCompetition, matchDate, roundIndex, _competition);
               competitionSchedule.Rounds.Add(round);
            }
         }

         // Add the teams to the cup of this season.
         foreach (var team in teams)
         {
            var seasonCompetitionTeam = new SeasonCompetitionTeam
            {
               SeasonCompetition = cupSeasonCompetition,
               Team = team
            };
            competitionSchedule.SeasonCompetitionTeams.Add(seasonCompetitionTeam);
         }

         return competitionSchedule;
      }

      private static int DetermineNumberOfRounds(int numberOfTeams)
      {
         int numberOfRounds = 0;
         int numberOfMatches = numberOfTeams / 2;
         while (numberOfMatches >= 1)
         {
            numberOfRounds++;
            numberOfMatches = numberOfMatches / 2;
         }

         return numberOfRounds;
      }

      public List<Match> DrawNextRound(Round previousRound, IEnumerable<Match> matchesPreviousRound, Season season)
      {
         var matchesToUpdate = new List<Match>();

         // Determine whether for the given round all matches have been played.
         using (var matchRepository = _repositoryFactory.CreateMatchRepository())
         using (var roundRepository = _repositoryFactory.CreateRoundRepository())
         {
            bool allMatchesPlayed = matchesPreviousRound.All(match => match.MatchStatus == MatchStatus.Ended);

            if (allMatchesPlayed)
            {
               // Determine the next round and draw matches for the next round from the winners of the previous round.
               var nextRound = roundRepository.GetNextRound(previousRound);
               if (nextRound != null)
               {
                  var winners = matchesPreviousRound.Select(match => match.GetWinner()).ToList();

                  bool allWinnersDetermined = !winners.Any(x => x == null);
                  if (!allWinnersDetermined)
                  {
                     throw new Exception("Not all winners could be detected");
                  }

                  int numberOfMatchesToDraw = matchesPreviousRound.Count() / 2;

                  var matchesNextRound = new KnockoutTournamentManager().DrawNextRound(winners);

                  foreach (var match in matchesNextRound)
                  {
                     match.Season = season;
                     match.Round = nextRound;
                     match.DrawPermitted = false;
                     match.CompetitionId = _competition.Id;
                     match.Date = nextRound.MatchDate;
                     matchesToUpdate.Add(match);
                  }
               }
            }
         }

         return matchesToUpdate;
      }

      private string GetCupRoundName(int numberOfRounds, int index)
      {
         // Cup round names for total 16 teams.
         var cupRoundNames1 = new[]
         {
            "Round 1",
            "Round 2",
            "Semi Final",
            "Final"
         };

         // Cup round names for total 64 teams.
         var cupRoundNames2 = new[]
         {
            "Round 1",
            "Round 2",
            "Round 3",
            "Quarter Final",
            "Semi Final",
            "Final"
         };

         string cupRoundName;
         if (numberOfRounds == cupRoundNames1.Length)
         {
            cupRoundName = cupRoundNames1[index];
         }
         else if (numberOfRounds == cupRoundNames2.Length)
         {
            cupRoundName = cupRoundNames2[index];
         }
         else
         {
            throw new NotImplementedException("Determining cup round name failed: unexpected number of rounds");
         }

         return cupRoundName;
      }
   }
}
