using System.Collections.Generic;
using Randomization;
using TwoNil.Data;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.Logic.Functionality.Players
{
   internal class PlayerManager
   {
      private NumberRandomizer _numberRandomizer;
      private PlayerGenerator _playerGenerator;

      public PlayerManager()
      {
         _numberRandomizer = new NumberRandomizer();
         _playerGenerator = new PlayerGenerator();
      }

      public IEnumerable<Player> GenerateSquad(Team team, int averageRating)
      {
         var squad = new List<Player>();

         using (var lineRepository = new MemoryRepositoryFactory().CreateLineRepository())
         {
            // Generate 2 goalkeepers.
            int howMany = 2;
            var line = lineRepository.GetGoalkeeper();
            squad.AddRange(GeneratePlayersForLine(line, howMany, averageRating));

            // Generate between 5 and 7 defenders.
            howMany = _numberRandomizer.GetNumber(5, 7);
            line = lineRepository.GetDefence();
            squad.AddRange(GeneratePlayersForLine(line, howMany, averageRating));

            // Generate between 5 and 7 midfielders.
            howMany = _numberRandomizer.GetNumber(5, 7);
            line = lineRepository.GetMidfield();
            squad.AddRange(GeneratePlayersForLine(line, howMany, averageRating));

            // Generate between 4 and 6 attackers.
            howMany = _numberRandomizer.GetNumber(4, 6);
            line = lineRepository.GetAttack();
            squad.AddRange(GeneratePlayersForLine(line, howMany, averageRating));
         }

         int teamOrder = 0;
         foreach (var player in squad)
         {
            player.Team = team;
            player.TeamOrder = teamOrder++;
         }

         return squad;
      }

      private IEnumerable<Player> GeneratePlayersForLine(Line line, int howMany, int averageRating)
      {
         var players = new List<Player>();

         int generated = 0;
         while (generated < howMany)
         {
            var player = _playerGenerator.Generate(line, averageRating);

            // Only add the player if his line is the expected one.
            if (player.PreferredPosition.Line.Equals(line))
            {
               players.Add(player);
               generated++;
            }
         }

         return players;
      }

   }
}
