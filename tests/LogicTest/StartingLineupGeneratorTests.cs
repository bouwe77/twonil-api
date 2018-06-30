using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TwoNil.Data;
using TwoNil.Data.Memory;
using TwoNil.Logic.Functionality.Teams;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.Logic
{
   [TestClass]
   public class StartingLineupGeneratorTests
   {
      private IPositionRepository _positionRepository;
      private FormationRepository _formationRepository;

      [TestInitialize]
      public void Initialize()
      {
         // No need to mock this repositories as they do not connect to the database but have their data in-memory.
         var repositoryFactory = new MemoryRepositoryFactory();
         _positionRepository = repositoryFactory.CreatePositionRepository();
         _formationRepository = repositoryFactory.CreateFormationRepository();
      }

      [TestCleanup]
      public void Cleanup()
      {
         _positionRepository.Dispose();
         _formationRepository.Dispose();
      }

      /// <summary>
      /// Test that the players are sorted on rating and then by position.
      /// </summary>
      [TestMethod]
      public void PlayersAreSortedOnRatingAndPosition()
      {
         var players = new List<Player>
         {
            new Player { Name = "PlayerA", PreferredPosition = _positionRepository.GetCentralMidfield(), Rating = 10 },
            new Player { Name = "PlayerB", PreferredPosition = _positionRepository.GetCentreBack(), Rating = 10 },
            new Player { Name = "PlayerC", PreferredPosition = _positionRepository.GetStriker(), Rating = 10 },
            new Player { Name = "PlayerD", PreferredPosition = _positionRepository.GetDefensiveMidfield(), Rating = 10 },
            new Player { Name = "PlayerE", PreferredPosition = _positionRepository.GetGoalkeeper(), Rating = 10 },
            new Player { Name = "PlayerF", PreferredPosition = _positionRepository.GetWideMidfield(), Rating = 10 },
            new Player { Name = "PlayerG", PreferredPosition = _positionRepository.GetStriker(), Rating = 10 },
            new Player { Name = "PlayerH", PreferredPosition = _positionRepository.GetWingBack(), Rating = 10 },
            new Player { Name = "PlayerI", PreferredPosition = _positionRepository.GetWingBack(), Rating = 10 },
            new Player { Name = "PlayerJ", PreferredPosition = _positionRepository.GetGoalkeeper(), Rating = 10 },
            new Player { Name = "PlayerK", PreferredPosition = _positionRepository.GetCentreBack(), Rating = 10 },
            new Player { Name = "PlayerL", PreferredPosition = _positionRepository.GetWideMidfield(), Rating = 10 },
            //---        
            new Player { Name = "PlayerM", PreferredPosition = _positionRepository.GetWinger(), Rating = 10 },
            new Player { Name = "PlayerN", PreferredPosition = _positionRepository.GetCentreBack(), Rating = 10 },
         };

         var formation = _formationRepository.Get442();

         var generator = new StartingLineupGenerator();
         var newPlayers = generator.GenerateStartingLineup(players, formation);

         AssertGeneralStuff(newPlayers, 14);

         // First player is a goalkeeper PlayerE.
         var playerToAssert = newPlayers[0];
         Assert.AreEqual(_positionRepository.GetGoalkeeper(), playerToAssert.CurrentPosition);
         Assert.AreEqual("PlayerE", playerToAssert.Name);

         // Second player is a centre back PlayerB.
         playerToAssert = newPlayers[1];
         Assert.AreEqual(_positionRepository.GetCentreBack(), playerToAssert.CurrentPosition);
         Assert.AreEqual("PlayerB", playerToAssert.Name);

         // Second player is a centre back PlayerK.
         playerToAssert = newPlayers[2];
         Assert.AreEqual(_positionRepository.GetCentreBack(), playerToAssert.CurrentPosition);
         Assert.AreEqual("PlayerK", playerToAssert.Name);

         // Fourth player is wing back PlayerH.
         playerToAssert = newPlayers[3];
         Assert.AreEqual(_positionRepository.GetWingBack(), playerToAssert.CurrentPosition);
         Assert.AreEqual("PlayerH", playerToAssert.Name);

         // Fifth player is wing back PlayerI.
         playerToAssert = newPlayers[4];
         Assert.AreEqual(_positionRepository.GetWingBack(), playerToAssert.CurrentPosition);
         Assert.AreEqual("PlayerI", playerToAssert.Name);

         // Sixth player is defensive midfield PlayerD.
         playerToAssert = newPlayers[5];
         Assert.AreEqual(_positionRepository.GetDefensiveMidfield(), playerToAssert.CurrentPosition);
         Assert.AreEqual("PlayerD", playerToAssert.Name);

         // Seventh player is forward midfield PlayerA.
         playerToAssert = newPlayers[6];
         Assert.AreEqual(_positionRepository.GetForwardMidfield(), playerToAssert.CurrentPosition);
         Assert.AreEqual("PlayerA", playerToAssert.Name);

         // Eighth player is wide midfield PlayerF.
         playerToAssert = newPlayers[7];
         Assert.AreEqual(_positionRepository.GetWideMidfield(), playerToAssert.CurrentPosition);
         Assert.AreEqual("PlayerF", playerToAssert.Name);

         // Ninth player is wide midfield PlayerL.
         playerToAssert = newPlayers[8];
         Assert.AreEqual(_positionRepository.GetWideMidfield(), playerToAssert.CurrentPosition);
         Assert.AreEqual("PlayerL", playerToAssert.Name);

         // Tenth player is centre forward PlayerG
         playerToAssert = newPlayers[9];
         Assert.AreEqual(_positionRepository.GetCentreForward(), playerToAssert.CurrentPosition);
         Assert.AreEqual("PlayerG", playerToAssert.Name);

         // Eleventh player is striker PlayerC.
         playerToAssert = newPlayers[10];
         Assert.AreEqual(_positionRepository.GetStriker(), playerToAssert.CurrentPosition);
         Assert.AreEqual("PlayerC", playerToAssert.Name);
      }

      /// <summary>
      /// This team consists of 11 strikers...
      /// </summary>
      [TestMethod]
      public void TeamOfStrikers()
      {
         var players = new List<Player>
         {
            new Player { Name = "PlayerA", PreferredPosition = _positionRepository.GetStriker(), Rating = 10 },
            new Player { Name = "PlayerB", PreferredPosition = _positionRepository.GetStriker(), Rating = 10 },
            new Player { Name = "PlayerC", PreferredPosition = _positionRepository.GetStriker(), Rating = 10 },
            new Player { Name = "PlayerD", PreferredPosition = _positionRepository.GetStriker(), Rating = 10 },
            new Player { Name = "PlayerE", PreferredPosition = _positionRepository.GetStriker(), Rating = 10 },
            new Player { Name = "PlayerF", PreferredPosition = _positionRepository.GetStriker(), Rating = 10 },
            new Player { Name = "PlayerG", PreferredPosition = _positionRepository.GetStriker(), Rating = 10 },
            new Player { Name = "PlayerH", PreferredPosition = _positionRepository.GetStriker(), Rating = 10 },
            new Player { Name = "PlayerI", PreferredPosition = _positionRepository.GetStriker(), Rating = 10 },
            new Player { Name = "PlayerJ", PreferredPosition = _positionRepository.GetStriker(), Rating = 10 },
            new Player { Name = "PlayerK", PreferredPosition = _positionRepository.GetStriker(), Rating = 10 },
         };

         var formation = _formationRepository.Get433();

         var generator = new StartingLineupGenerator();
         var newPlayers = generator.GenerateStartingLineup(players, formation);

         // We only assert that all positions are taken.
         AssertGeneralStuff(newPlayers, 11);
      }

      private void AssertGeneralStuff(List<Player> players, int howManyPlayers)
      {
         // Assert the expected number of players.
         Assert.IsNotNull(players);
         Assert.AreEqual(howManyPlayers, players.Count);

         // Assert there are no empty positions.
         bool emptyPositionsFound = players.Any(x => x == null);
         Assert.IsFalse(emptyPositionsFound);

         // Assert the first 11 players have a CurrentPosition and a boolean indicating they are in the starting eleven, and all others have not.
         for (int i = 0; i < players.Count; i++)
         {
            if (i <= 10)
            {
               Assert.IsNotNull(players[i].CurrentPosition);
               Assert.IsTrue(players[i].InStartingEleven);
            }
            else
            {
               Assert.IsNull(players[i].CurrentPosition);
               Assert.IsFalse(players[i].InStartingEleven);
            }
         }

         // Assert the PlayerId is unique.
         var uniquePlayers = players.Select(p => p.Id).Distinct().ToList();
         Assert.AreEqual(howManyPlayers, uniquePlayers.Count);

         // Assert the TeamOrder is unique for each player.
         var uniqueTeamOrder = players.Select(p => p.TeamOrder).Distinct().ToList();
         Assert.AreEqual(howManyPlayers, uniqueTeamOrder.Count);

         // Assert the player list is ordered on the TeamOrder property.
         int minimumTeamOrder = players.Min(p => p.TeamOrder);
         var firstPlayer = players[0];
         Assert.AreEqual(minimumTeamOrder, firstPlayer.TeamOrder);
         int maximumTeamOrder = players.Max(p => p.TeamOrder);
         var lastPlayer = players[howManyPlayers - 1];
         Assert.AreEqual(maximumTeamOrder, lastPlayer.TeamOrder);

         // Assert the player list is also ordered on the rating.
         decimal maxRating = players.Max(p => p.Rating);
         var bestPlayer = players[0];
         Assert.AreEqual(maxRating, bestPlayer.Rating);
         decimal minRating = players.Min(p => p.Rating);
         var worstPlayer = players[howManyPlayers - 1];
         Assert.AreEqual(minRating, worstPlayer.Rating);
      }
   }
}
