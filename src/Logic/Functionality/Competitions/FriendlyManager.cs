using System;
using System.Collections.Generic;
using System.Linq;
using Randomization;
using TwoNil.Data;
using TwoNil.Logic.Functionality.Matches;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.Logic.Functionality.Competitions
{
   internal class FriendlyManager
   {
      private readonly Randomizer _randomizer;
      private readonly NumberRandomizer _numberRandomizer;
      private DatabaseRepositoryFactory _repositoryFactory;
      private Competition _competition;

      public FriendlyManager(DatabaseRepositoryFactory repositoryFactory)
      {
         _repositoryFactory = repositoryFactory;
         _randomizer = new Randomizer();
         _numberRandomizer = new NumberRandomizer();

         using (var competitionRepository = new MemoryRepositoryFactory().CreateCompetitionRepository())
         {
            _competition = competitionRepository.GetFriendly();
         }
      }

      public CompetitionSchedule CreatePreSeasonSchedule(
         List<Team> teams,
         int howMany,
         Season season,
         MatchDateManager matchDateManager)
      {
         var competitionSchedule = new CompetitionSchedule();

         // Create a friendly season competition for all friendlies in the season.
         SeasonCompetition friendlySeasonCompetition;
         using (var competitionRepository = new MemoryRepositoryFactory().CreateCompetitionRepository())
         {
            friendlySeasonCompetition = new SeasonCompetition
            {
               Competition = _competition,
               Season = season
            };
         }

         competitionSchedule.SeasonCompetitions.Add(friendlySeasonCompetition);

         // Create pre-season friendly schedule.
         var preSeasonFriendlySchedule = GetPreSeasonSchedule(teams);

         foreach (var round in preSeasonFriendlySchedule)
         {
            var matchDate = matchDateManager.GetNextMatchDate(CompetitionType.Friendly, round.Key);

            var friendlyRound = RoundFactory.CreateRound(null, friendlySeasonCompetition, matchDate, round.Key, _competition);
            competitionSchedule.Rounds.Add(friendlyRound);

            foreach (var match in round.Value)
            {
               match.Season = season;
               match.Round = friendlyRound;
               match.Date = matchDate;
               match.CompetitionId = _competition.Id;
               competitionSchedule.Matches.Add(match);
            }
         }

         // Add the teams to the pre-season friendly competition of this season.
         foreach (var team in teams)
         {
            var seasonCompetitionTeam = new SeasonCompetitionTeam
            {
               SeasonCompetition = friendlySeasonCompetition,
               Team = team
            };
            competitionSchedule.SeasonCompetitionTeams.Add(seasonCompetitionTeam);
         }

         return competitionSchedule;
      }

      public CompetitionSchedule CreateDuringSeasonSchedule(SeasonCompetition seasonCompetition, List<DateTime> matchDates, int startIndex)
      {
         // The during season friendly schedule only consists of rounds. The matches in the rounds are determined during the season.
         var competitionSchedule = new CompetitionSchedule();

         foreach (var matchDate in matchDates)
         {
            var friendlyRound = RoundFactory.CreateRound(null, seasonCompetition, matchDate, startIndex, _competition);

            competitionSchedule.Rounds.Add(friendlyRound);
            startIndex++;
         }

         return competitionSchedule;
      }

      public IEnumerable<Match> CreateMatchesForFriendlyRound(IEnumerable<Round> roundsAndMatches, IEnumerable<Team> allTeams)
      {
         IEnumerable<Match> matches = new List<Match>();

         var friendlyRoundsWithoutMatches = roundsAndMatches.Where(r => r.CompetitionId == _competition.Id && !r.Matches.Any());
         foreach (var friendlyRound in friendlyRoundsWithoutMatches)
         {
            // Determine there are other rounds at the same date.
            var otherRoundIds = roundsAndMatches.Where(r => r.MatchDate == friendlyRound.MatchDate && r.Id != friendlyRound.Id).Select(r => r.Id);

            // If there are no other rounds, new matches for this friendly round can be generated.
            // All teams are available for this friendly round.
            if (!otherRoundIds.Any())
            {
               int howManyTeams = _numberRandomizer.GetEvenNumber(2, allTeams.Count());
               matches = ArrangeFriendlies(allTeams.Take(howManyTeams).ToList(), friendlyRound);
            }
            else
            {
               // If there are other rounds, then all of this rounds must have unplayed matches.
               // If this is the case a random number of teams that do not play these matches are available for this friendly round.
               var otherMatchesOnMatchDay = roundsAndMatches.SelectMany(m => m.Matches).Where(m => m.Date == friendlyRound.MatchDate && m.RoundId != friendlyRound.Id && m.MatchStatus == MatchStatus.NotStarted);
               int numberOfUniqueRoundIds = otherMatchesOnMatchDay.GroupBy(x => x.RoundId).Distinct().Count();
               bool generateFriendlies = numberOfUniqueRoundIds == otherRoundIds.Count();
               if (generateFriendlies)
               {
                  var teams = allTeams.Except(otherMatchesOnMatchDay.Select(m => m.HomeTeam)).Except(otherMatchesOnMatchDay.Select(m => m.AwayTeam));
                  int howManyTeams = _numberRandomizer.GetEvenNumber(2, teams.Count());
                  matches = ArrangeFriendlies(teams.Take(howManyTeams).ToList(), friendlyRound);
               }
            }
         }

         return matches;
      }

      private Dictionary<int, List<Match>> GetPreSeasonSchedule(List<Team> teams)
      {
         var schedule = new Dictionary<int, List<Match>>
         {
            { 0, new List<Match>() },
            { 1, new List<Match>() },
            { 2, new List<Match>() },
            { 3, new List<Match>() }
         };

         ArrangePreSeasonSchedule(schedule, teams);

         return schedule;
      }

      /// <summary>
      /// Arranges the pre season schedule.
      /// LET OP: Deze methode ondersteunt het volgende:
      /// Stel dat de manager zelf mag kiezen tegen welke teams hij tegen wil spelen, dan kan dat.
      /// Er wordt dan een schedule gegenereerd waar deze wedstrijden alvast inzitten.
      /// Vervolgens regelt deze methode dat er voor de rest van de teams ook wedstrijden worden ingepland.
      /// </summary>
      /// <param name="schedule">The schedule.</param>
      /// <param name="teams">The teams.</param>
      /// <returns></returns>
      private void ArrangePreSeasonSchedule(Dictionary<int, List<Match>> schedule, List<Team> teams)
      {
         teams.Shuffle();

         foreach (var team in teams)
         {
            // We keep on creating matches for the team until it plays a match in each round.
            bool teamPlaysEachRound = false;
            while (!teamPlaysEachRound)
            {
               // Is there a round this team does not play a match yet?
               int theRound = -1;
               foreach (var round in schedule.Where(round => !round.Value.TeamPlaysMatch(team)))
               {
                  theRound = round.Key;
                  break;
               }

               // If no round found the team already plays in each round so continue to the next team.
               if (theRound == -1)
               {
                  teamPlaysEachRound = true;
                  continue;
               }

               // Team will play in the foundRound, find an opponent.
               var possibleOpponents = teams.Where(t => !t.Equals(team));
               foreach (var possibleOpponent in possibleOpponents)
               {
                  // The opponent must not already play a match in this round.
                  if (schedule[theRound].TeamPlaysMatch(possibleOpponent))
                  {
                     continue;
                  }

                  // Both teams do not already play in this round, but also the match between the team and opponent 
                  // must not already exist in any other round.
                  var possibleMatch = MatchFactory.CreateMatch(homeTeam: team, awayTeam: possibleOpponent);
                  var otherRounds = schedule.Where(r => r.Key != theRound);
                  bool alreadyPlayedInAnotherRound = false;
                  foreach (var otherRound in otherRounds)
                  {
                     alreadyPlayedInAnotherRound = otherRound.Value.MatchExists(possibleMatch);
                     if (alreadyPlayedInAnotherRound)
                     {
                        break;
                     }
                  }

                  if (!alreadyPlayedInAnotherRound)
                  {
                     // A new match found, add it to the schedule. Quit searching for a possible opponent in this round.
                     schedule[theRound].Add(possibleMatch);

                     bool swapHomeAndAway = _randomizer.GetRandomBoolean();
                     if (swapHomeAndAway)
                     {
                        possibleMatch.SwapHomeAndAway();
                     }

                     break;
                  }
               }
            }
         }

         // Shuffle the ordering of the matches.
         foreach (var round in schedule)
         {
            round.Value.Shuffle();
         }
      }

      private IEnumerable<Match> ArrangeFriendlies(List<Team> teams, Round round)
      {
         // If the number of teams is not even, remove a team.
         if (teams.Count() % 2 != 0)
         {
            teams.RemoveAt(0);
         }

         var matches = new List<Match>();
         if (teams.Any())
         {
            // Create a so-called single round tournament between the teams.
            matches = new SingleRoundTournamentManager().GetMatches(teams);
         }

         foreach (var match in matches)
         {
            match.Season = round.Season;
            match.Round = round;
            match.Date = round.MatchDate;
            match.CompetitionId = _competition.Id;
         }

         return matches;
      }
   }
}
