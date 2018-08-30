using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Linq;
using TwoNil.Data;
using TwoNil.Data.Repositories;
using TwoNil.Logic.Functionality.Players;

namespace TwoNil.Logic
{
    [TestClass]
    public class ProfileScoreCalculatorTests
    {
        private PlayerProfileRepository _playerProfileRepository;

        [TestInitialize]
        public void TestInitialize()
        {
            // Although this is a unit test, we can use the PlayerProfileRepo because it is an in-memory (i.e. hard-coded) repo.
            _playerProfileRepository = new RepositoryFactory().CreatePlayerProfileRepository();
        }

        /// <summary>
        /// This unit test is not a real test, its purpose is to demonstrate/check which random skillscores are determined
        /// depending on the start skill score and PlayerProfile.
        /// The result is written to a file to visualize the skill scores.
        /// </summary>
        [TestMethod]
        public void Calculate_Demo()
        {
            //            var playerProfile = _playerProfileRepository.GetForwardMidfieldProfile();
            var playerProfile = _playerProfileRepository.GetVersatileProfile();
            var calculator = new ProfileScoreCalculator();

            int numberOfPlayers = 20;
            int startSkillScore = 100;

            bool writeToFile = false;
            if (writeToFile)
            {
                using (TextWriter tw = new StreamWriter(@"D:\Temp\Player.txt"))
                {
                    for (int i = 0; i < numberOfPlayers; i++)
                    {
                        var skillScores = calculator.Calculate(startSkillScore, playerProfile, 1).OrderBy(x => x.PlayerSkill.Name);

                        foreach (var skillScore in skillScores)
                            tw.WriteLine($"{skillScore.PlayerSkill.Name} {new String(' ', 15 - skillScore.PlayerSkill.Name.Length)} {new String('#', skillScore.Score)}");

                        tw.WriteLine();
                        tw.WriteLine();
                    }
                }
            }
        }
    }
}