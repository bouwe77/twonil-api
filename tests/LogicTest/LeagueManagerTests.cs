using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TwoNil.Logic.Functionality.Competitions;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.Logic
{
   [TestClass]
   public class LeagueManagerTests
   {
      [TestMethod]
      public void PromoteAndRelegateTeams_ThrowsException_WhenNumberOfLeaguesIsNot4()
      {
         var leagues = new List<List<Team>>
         {
            new List<Team>(),
            new List<Team>(),
            new List<Team>()
         };

         var leagueManager = new LeagueManager();

         try
         {
            leagueManager.PromoteAndRelegateTeams(leagues, 1);
            Assert.Fail("NotImplementedException was expected");
         }
         catch (NotImplementedException)
         {
         }
         catch
         {
            Assert.Fail("NotImplementedException was expected");
         }

         try
         {
            leagues.Add(new List<Team>());
            leagues.Add(new List<Team>());

            leagueManager.PromoteAndRelegateTeams(leagues, 1);
            Assert.Fail("NotImplementedException was expected");
         }
         catch (NotImplementedException)
         {
         }
         catch
         {
            Assert.Fail("NotImplementedException was expected");
         }
      }

      /// <summary>
      /// Test the PromoteAndRelegateTeams method works with 4 leagues, 4 teams per league and 1 team promotes and relegates.
      /// </summary>
      [TestMethod]
      public void PromoteAndRelegateTeams_Successful_When4TeamsPerLeagueAnd1TeamPromotesAndRelegates()
      {
         var league1 = new List<Team>
         {
            new Team { Id = "1a" },
            new Team { Id = "1b" },
            new Team { Id = "1c" },
            new Team { Id = "1d" },
            new Team { Id = "1e" },
            new Team { Id = "1f" },
            new Team { Id = "1g" },
            new Team { Id = "1h" },
            new Team { Id = "1i" },
            new Team { Id = "1j" },
            new Team { Id = "1k" },
            new Team { Id = "1l" },
            new Team { Id = "1m" },
            new Team { Id = "1n" },
            new Team { Id = "1o" },
            new Team { Id = "1p" }
         };
         var league2 = new List<Team>
         {
            new Team { Id = "2a" },
            new Team { Id = "2b" },
            new Team { Id = "2c" },
            new Team { Id = "2d" },
            new Team { Id = "2e" },
            new Team { Id = "2f" },
            new Team { Id = "2g" },
            new Team { Id = "2h" },
            new Team { Id = "2i" },
            new Team { Id = "2j" },
            new Team { Id = "2k" },
            new Team { Id = "2l" },
            new Team { Id = "2m" },
            new Team { Id = "2n" },
            new Team { Id = "2o" },
            new Team { Id = "2p" }
         };
         var league3 = new List<Team>
         {
            new Team { Id = "3a" },
            new Team { Id = "3b" },
            new Team { Id = "3c" },
            new Team { Id = "3d" },
            new Team { Id = "3e" },
            new Team { Id = "3f" },
            new Team { Id = "3g" },
            new Team { Id = "3h" },
            new Team { Id = "3i" },
            new Team { Id = "3j" },
            new Team { Id = "3k" },
            new Team { Id = "3l" },
            new Team { Id = "3m" },
            new Team { Id = "3n" },
            new Team { Id = "3o" },
            new Team { Id = "3p" }
         };
         var league4 = new List<Team>
         {
            new Team { Id = "4a" },
            new Team { Id = "4b" },
            new Team { Id = "4c" },
            new Team { Id = "4d" },
            new Team { Id = "4e" },
            new Team { Id = "4f" },
            new Team { Id = "4g" },
            new Team { Id = "4h" },
            new Team { Id = "4i" },
            new Team { Id = "4j" },
            new Team { Id = "4k" },
            new Team { Id = "4l" },
            new Team { Id = "4m" },
            new Team { Id = "4n" },
            new Team { Id = "4o" },
            new Team { Id = "4p" }
         };

         var leagues = new List<List<Team>> { league1, league2, league3, league4 };

         var leagueManager = new LeagueManager();
         var newLeagues = leagueManager.PromoteAndRelegateTeams(leagues, 3);

         Assert.AreEqual(4, newLeagues.Count);

         // New League 1
         Assert.AreEqual(16, newLeagues[0].Count);
         Assert.AreEqual("1a", newLeagues[0][0].Id);
         Assert.AreEqual("1b", newLeagues[0][1].Id);
         Assert.AreEqual("1c", newLeagues[0][2].Id);
         Assert.AreEqual("1d", newLeagues[0][3].Id);
         Assert.AreEqual("1e", newLeagues[0][4].Id);
         Assert.AreEqual("1f", newLeagues[0][5].Id);
         Assert.AreEqual("1g", newLeagues[0][6].Id);
         Assert.AreEqual("1h", newLeagues[0][7].Id);
         Assert.AreEqual("1i", newLeagues[0][8].Id);
         Assert.AreEqual("1j", newLeagues[0][9].Id);
         Assert.AreEqual("1k", newLeagues[0][10].Id);
         Assert.AreEqual("1l", newLeagues[0][11].Id);
         Assert.AreEqual("1m", newLeagues[0][12].Id);
         Assert.AreEqual("2a", newLeagues[0][13].Id);
         Assert.AreEqual("2b", newLeagues[0][14].Id);
         Assert.AreEqual("2c", newLeagues[0][15].Id);

         // New League 2
         Assert.AreEqual(16, newLeagues[1].Count);
         Assert.AreEqual("1n", newLeagues[1][0].Id);
         Assert.AreEqual("1o", newLeagues[1][1].Id);
         Assert.AreEqual("1p", newLeagues[1][2].Id);
         Assert.AreEqual("2d", newLeagues[1][3].Id);
         Assert.AreEqual("2e", newLeagues[1][4].Id);
         Assert.AreEqual("2f", newLeagues[1][5].Id);
         Assert.AreEqual("2g", newLeagues[1][6].Id);
         Assert.AreEqual("2h", newLeagues[1][7].Id);
         Assert.AreEqual("2i", newLeagues[1][8].Id);
         Assert.AreEqual("2j", newLeagues[1][9].Id);
         Assert.AreEqual("2k", newLeagues[1][10].Id);
         Assert.AreEqual("2l", newLeagues[1][11].Id);
         Assert.AreEqual("2m", newLeagues[1][12].Id);
         Assert.AreEqual("3a", newLeagues[1][13].Id);
         Assert.AreEqual("3b", newLeagues[1][14].Id);
         Assert.AreEqual("3c", newLeagues[1][15].Id);

         // New League 3
         Assert.AreEqual(16, newLeagues[2].Count);
         Assert.AreEqual("2n", newLeagues[2][0].Id);
         Assert.AreEqual("2o", newLeagues[2][1].Id);
         Assert.AreEqual("2p", newLeagues[2][2].Id);
         Assert.AreEqual("3d", newLeagues[2][3].Id);
         Assert.AreEqual("3e", newLeagues[2][4].Id);
         Assert.AreEqual("3f", newLeagues[2][5].Id);
         Assert.AreEqual("3g", newLeagues[2][6].Id);
         Assert.AreEqual("3h", newLeagues[2][7].Id);
         Assert.AreEqual("3i", newLeagues[2][8].Id);
         Assert.AreEqual("3j", newLeagues[2][9].Id);
         Assert.AreEqual("3k", newLeagues[2][10].Id);
         Assert.AreEqual("3l", newLeagues[2][11].Id);
         Assert.AreEqual("3m", newLeagues[2][12].Id);
         Assert.AreEqual("4a", newLeagues[2][13].Id);
         Assert.AreEqual("4b", newLeagues[2][14].Id);
         Assert.AreEqual("4c", newLeagues[2][15].Id);

         // New League 4
         Assert.AreEqual(16, newLeagues[3].Count);
         Assert.AreEqual("3n", newLeagues[3][0].Id);
         Assert.AreEqual("3o", newLeagues[3][1].Id);
         Assert.AreEqual("3p", newLeagues[3][2].Id);
         Assert.AreEqual("4d", newLeagues[3][3].Id);
         Assert.AreEqual("4e", newLeagues[3][4].Id);
         Assert.AreEqual("4f", newLeagues[3][5].Id);
         Assert.AreEqual("4g", newLeagues[3][6].Id);
         Assert.AreEqual("4h", newLeagues[3][7].Id);
         Assert.AreEqual("4i", newLeagues[3][8].Id);
         Assert.AreEqual("4j", newLeagues[3][9].Id);
         Assert.AreEqual("4k", newLeagues[3][10].Id);
         Assert.AreEqual("4l", newLeagues[3][11].Id);
         Assert.AreEqual("4m", newLeagues[3][12].Id);
         Assert.AreEqual("4n", newLeagues[3][13].Id);
         Assert.AreEqual("4o", newLeagues[3][14].Id);
         Assert.AreEqual("4p", newLeagues[3][15].Id);
      }

      /// <summary>
      /// Test the PromoteAndRelegateTeams method works with 4 leagues, 16 teams per league and 3 teams promote and relegate.
      /// </summary>
      [TestMethod]
      public void PromoteAndRelegateTeams_Successful_When16TeamsPerLeagueAnd3TeamsPromoteAndRelegate()
      {
         var league1 = new List<Team>
         {
            new Team { Id = "a" },
            new Team { Id = "b" },
            new Team { Id = "c" },
            new Team { Id = "d" }
         };
         var league2 = new List<Team>
         {
            new Team { Id = "e" },
            new Team { Id = "f" },
            new Team { Id = "g" },
            new Team { Id = "h" }
         };
         var league3 = new List<Team>
         {
            new Team { Id = "i" },
            new Team { Id = "j" },
            new Team { Id = "k" },
            new Team { Id = "l" }
         };
         var league4 = new List<Team>
         {
            new Team { Id = "m" },
            new Team { Id = "n" },
            new Team { Id = "o" },
            new Team { Id = "p" }
         };

         var leagues = new List<List<Team>> { league1, league2, league3, league4 };

         var leagueManager = new LeagueManager();
         var newLeagues = leagueManager.PromoteAndRelegateTeams(leagues, 1);

         Assert.AreEqual(4, newLeagues.Count);

         // New League 1
         Assert.AreEqual(4, newLeagues[0].Count);
         Assert.AreEqual("a", newLeagues[0][0].Id);
         Assert.AreEqual("b", newLeagues[0][1].Id);
         Assert.AreEqual("c", newLeagues[0][2].Id);
         Assert.AreEqual("e", newLeagues[0][3].Id);

         // New League 2
         Assert.AreEqual(4, newLeagues[1].Count);
         Assert.AreEqual("d", newLeagues[1][0].Id);
         Assert.AreEqual("f", newLeagues[1][1].Id);
         Assert.AreEqual("g", newLeagues[1][2].Id);
         Assert.AreEqual("i", newLeagues[1][3].Id);

         // New League 3
         Assert.AreEqual(4, newLeagues[2].Count);
         Assert.AreEqual("h", newLeagues[2][0].Id);
         Assert.AreEqual("j", newLeagues[2][1].Id);
         Assert.AreEqual("k", newLeagues[2][2].Id);
         Assert.AreEqual("m", newLeagues[2][3].Id);

         // New League 4
         Assert.AreEqual(4, newLeagues[3].Count);
         Assert.AreEqual("l", newLeagues[3][0].Id);
         Assert.AreEqual("n", newLeagues[3][1].Id);
         Assert.AreEqual("o", newLeagues[3][2].Id);
         Assert.AreEqual("p", newLeagues[3][3].Id);
      }
   }
}
