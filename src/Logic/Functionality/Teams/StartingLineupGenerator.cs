using System;
using System.Collections.Generic;
using System.Linq;
using TwoNil.Data;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.Logic.Functionality.Teams
{
   internal class StartingLineupGenerator
   {
      private readonly IDictionary<string, List<Position>> _alternativePositions;

      public StartingLineupGenerator()
      {
         using (var positionRepository = new MemoryRepositoryFactory().CreatePositionRepository())
         {
            _alternativePositions = new Dictionary<string, List<Position>>
            {
               { "Goalkeeper", new List<Position> {positionRepository.GetGoalkeeper()} },
               { "Centre Back", new List<Position> {positionRepository.GetWingBack(), positionRepository.GetDefensiveMidfield()} },
               { "Wing Back", new List<Position> {positionRepository.GetCentreBack(), positionRepository.GetDefensiveMidfield()} },
               { "Defensive Midfield", new List<Position> {positionRepository.GetWingBack(), positionRepository.GetCentreBack()} },
               { "Central Midfield", new List<Position> {positionRepository.GetWideMidfield(), positionRepository.GetForwardMidfield(), positionRepository.GetDefensiveMidfield()} },
               { "Wide Midfield", new List<Position> {positionRepository.GetCentralMidfield(), positionRepository.GetDefensiveMidfield(), positionRepository.GetForwardMidfield()} },
               { "Forward Midfield", new List<Position> {positionRepository.GetCentralMidfield(), positionRepository.GetCentreForward(), positionRepository.GetWinger()} },
               { "Winger", new List<Position> {positionRepository.GetForwardMidfield(), positionRepository.GetCentreForward()} },
               { "Centre Forward", new List<Position> {positionRepository.GetForwardMidfield(), positionRepository.GetStriker(), positionRepository.GetWinger()} },
               { "Striker", new List<Position> {positionRepository.GetCentreForward(), positionRepository.GetWinger(), positionRepository.GetForwardMidfield()} }
            };
         }
      }

      public List<Player> GenerateStartingLineup(List<Player> players, Formation formation)
      {
         // Obviously we need at least 11 players.
         if (players.Count < 11)
         {
            throw new Exception("There are not enough players");
         }
         
         // Sort the players on rating and then on name.
         players.Sort(delegate (Player p1, Player p2)
         {
            int ratingDiff = p1.Rating.CompareTo(p2.Rating);
            if (ratingDiff != 0) return ratingDiff;
            return string.Compare(p1.Name, p2.Name, StringComparison.OrdinalIgnoreCase);
         });

         var newPlayers = new Player[players.Count];
         int teamOrder = 0;

         // Split the players into three lists: the goalkeepers, the best 10 and the rest.
         Position goalkeeperPosition;
         using (var positionRepository = new MemoryRepositoryFactory().CreatePositionRepository())
         {
            goalkeeperPosition = positionRepository.GetGoalkeeper();
         }

         var goalkeepers = players.Where(player => player.PreferredPosition.Equals(goalkeeperPosition)).ToList();
         var bestFieldPlayers = players.Where(player => !player.PreferredPosition.Equals(goalkeeperPosition)).Take(10).ToList();
         var otherFieldPlayers = players.Where(player => !player.PreferredPosition.Equals(goalkeeperPosition)).Skip(10).ToList();

         // Pick the best goalkeeper.
         var bestGoalkeeper = goalkeepers.FirstOrDefault();
         if (bestGoalkeeper != null)
         {
            goalkeepers.Remove(bestGoalkeeper);
            bestGoalkeeper.CurrentPosition = goalkeeperPosition;
            bestGoalkeeper.TeamOrder = teamOrder;
            newPlayers[teamOrder] = bestGoalkeeper;
            teamOrder++;
         }

         // Put the 10 best field players on their preferred position if possible.
         while (teamOrder < 11)
         {
            var positionWeNeed = formation.Positions[teamOrder];
            var playerForPos = bestFieldPlayers.FirstOrDefault(player => player.PreferredPosition.Equals(positionWeNeed));
            if (playerForPos != null)
            {
               playerForPos.CurrentPosition = positionWeNeed;
               playerForPos.TeamOrder = teamOrder;
               newPlayers[teamOrder] = playerForPos;
               bestFieldPlayers.Remove(playerForPos);
            }

            teamOrder++;
         }

         // Check whether the starting eleven has positions without a player and pick players for these positions.
         for (int i = 0; i < 11; i++)
         {
            if (newPlayers[i] == null)
            {
               Player playerForPos = null;
               var positionWeNeed = formation.Positions[i];

               // 1) Get a player from the best 10 players (if any left) that has an alternative for this position.
               var alternativePositions = GetAlternativePositions(positionWeNeed);
               foreach (var alternativePosition in alternativePositions)
               {
                  playerForPos = bestFieldPlayers.FirstOrDefault(player => player.PreferredPosition.Equals(alternativePosition));
                  if (playerForPos != null)
                  {
                     playerForPos.CurrentPosition = positionWeNeed;
                     playerForPos.TeamOrder = i;
                     newPlayers[i] = playerForPos;
                     bestFieldPlayers.Remove(playerForPos);
                     break;
                  }
               }

               // 2) If not found, get one of the remaining players that has this position
               if (playerForPos == null)
               {
                  playerForPos = otherFieldPlayers.FirstOrDefault(player => player.PreferredPosition.Equals(positionWeNeed));
                  if (playerForPos != null)
                  {
                     playerForPos.CurrentPosition = positionWeNeed;
                     playerForPos.TeamOrder = i;
                     newPlayers[i] = playerForPos;
                     otherFieldPlayers.Remove(playerForPos);
                  }
               }

               // 3) If not found, get one of the remaining players that has an alternative for this position.
               if (playerForPos == null)
               {
                  foreach (var alternativePosition in alternativePositions)
                  {
                     playerForPos = otherFieldPlayers.FirstOrDefault(player => player.PreferredPosition.Equals(alternativePosition));
                     if (playerForPos != null)
                     {
                        playerForPos.CurrentPosition = positionWeNeed;
                        playerForPos.TeamOrder = i;
                        newPlayers[i] = playerForPos;
                        otherFieldPlayers.Remove(playerForPos);
                        break;
                     }
                  }
               }
            }
         }

         // If the starting eleven still have vacant positions (which is highly unlikely), just pick random players.
         for (int i = 0; i < 11; i++)
         {
            if (newPlayers[i] == null)
            {
               var playerForPos = bestFieldPlayers.FirstOrDefault();
               if (playerForPos != null)
               {
                  playerForPos.TeamOrder = i;
                  newPlayers[i] = playerForPos;
                  bestFieldPlayers.Remove(playerForPos);
               }
               else
               {
                  playerForPos = otherFieldPlayers.FirstOrDefault();
                  if (playerForPos != null)
                  {
                     playerForPos.TeamOrder = i;
                     newPlayers[i] = playerForPos;
                     otherFieldPlayers.Remove(playerForPos);
                  }
                  else
                  {
                     playerForPos = goalkeepers.FirstOrDefault();
                     if (playerForPos != null)
                     {
                        playerForPos.TeamOrder = i;
                        newPlayers[i] = playerForPos;
                        goalkeepers.Remove(playerForPos);
                     }
                  }
               }
            }
         }

         // Assign the team order to all players that are not in the starting eleven.
         foreach (var bestFieldPlayer in bestFieldPlayers)
         {
            bestFieldPlayer.TeamOrder = teamOrder;
            newPlayers[teamOrder] = bestFieldPlayer;
            teamOrder++;
         }

         foreach (var goalkeeper in goalkeepers)
         {
            goalkeeper.TeamOrder = teamOrder;
            newPlayers[teamOrder] = goalkeeper;
            teamOrder++;
         }

         foreach (var otherFieldPlayer in otherFieldPlayers)
         {
            otherFieldPlayer.TeamOrder = teamOrder;
            newPlayers[teamOrder] = otherFieldPlayer;
            teamOrder++;
         }

         return newPlayers.ToList();
      }

      private List<Position> GetAlternativePositions(Position positionWeNeed)
      {
         List<Position> alternativePositions;
         _alternativePositions.TryGetValue(positionWeNeed.Name, out alternativePositions);
         return alternativePositions;
      }
   }
}
