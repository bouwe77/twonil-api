using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Randomization;
using TwoNil.Data.Repositories;
using TwoNil.Logic.Functionality.Players;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.Logic
{
   /// <summary>
   /// Unit tests for determining a position based on primary skills.
   /// </summary>
   [TestClass]
   public class PositionPrimarySkillsTests
   {
      private string _skill1 = "Skill1";
      private string _skill2 = "Skill2";
      private string _skill3 = "Skill3";
      private string _skill4 = "Skill4";

      private Mock<IPositionRepository> _mockedPositionRepository;
      private Mock<IListRandomizer> _mockedListRandomizer;

      [TestInitialize]
      public void TestInitialize()
      {
         _mockedPositionRepository = new Mock<IPositionRepository>();
         _mockedListRandomizer = new Mock<IListRandomizer>();
      }

      [TestCleanup]
      public void TestCleanup()
      {
         _mockedPositionRepository.VerifyAll();
         _mockedListRandomizer.VerifyAll();
      }

      /// <summary>
      /// First skill score is highest.
      /// </summary>
      [TestMethod]
      public void DeterminePosition_OnePositionHasSkillAsPrimarySkill()
      {
         // Skill2 has the highest score.
         var skillScores = GetSkillScores(1, 20, 1, 1);

         // There are two positions where only Position2 has the Skill2 as primary skill.
         var positions = new List<Position>
         {
            new Position
            {
               Name = "Position1",
               Line = new Line("Line", true),
               PrimarySkills = new List<PlayerSkill>
               {
                  new PlayerSkill { Id = _skill1, Name = _skill1 } //, PlayerSkillType = PlayerSkillType.Field, Order = 0 }
               }
            },
            new Position
            {
               Name = "Position2",
               Line = new Line("Line", true),
               PrimarySkills = new List<PlayerSkill>
               {
                  new PlayerSkill { Id = _skill2, Name = _skill2 } //, PlayerSkillType = PlayerSkillType.Field, Order = 0 }
               }
            },
         };

         _mockedPositionRepository.Setup(x => x.GetAll()).Returns(positions);

         var positionDeterminator = new PositionDeterminator(_mockedPositionRepository.Object, _mockedListRandomizer.Object);
         var determinedPosition = positionDeterminator.Determine(skillScores);

         // Expect Position2 because this has Skill2 as primary skill.
         Assert.IsNotNull(determinedPosition);
         Assert.AreEqual("Position2", determinedPosition.Name);
      }

      /// <summary>
      /// The highest score is not a primary skill for any position, but the second highest score is.
      /// </summary>
      [TestMethod]
      public void DeterminePosition_SecondHighestScore()
      {
         // Skill3 has the highest score and Skill2 has the second highest score.
         var skillScores = GetSkillScores(1, 19, 20, 1);

         // There are two positions and none of the positions has Skill3 as primary skill.
         // Skill2, however, is primary skill for Position2.
         var positions = new List<Position>
         {
            new Position
            {
               Name = "Position1",
               Line = new Line("Line", true),
               PrimarySkills = new List<PlayerSkill>
               {
                  new PlayerSkill
                  {
                     Id = _skill1,
                     Name = _skill1,
                     //PlayerSkillType = PlayerSkillType.Field,
                     //Order = 0
                  }
               }
            },
            new Position
            {
               Name = "Position2",
               Line = new Line("Line", true),
               PrimarySkills = new List<PlayerSkill>
               {
                  new PlayerSkill
                  {
                     Id = _skill2,
                     Name = _skill2,
                     //PlayerSkillType = PlayerSkillType.Field,
                     //Order = 0
                  }
               }
            }
         };

         _mockedPositionRepository.Setup(x => x.GetAll()).Returns(positions);

         var positionDeterminator = new PositionDeterminator(_mockedPositionRepository.Object, _mockedListRandomizer.Object);
         var determinedPosition = positionDeterminator.Determine(skillScores);

         // Expect Position2 because this has Skill2 (the socond highest score) as primary skill.
         Assert.IsNotNull(determinedPosition);
         Assert.AreEqual("Position2", determinedPosition.Name);
      }

      [TestMethod]
      public void DeterminePosition_Test3()
      {
         var skillScores = GetSkillScores(1, 2, 4, 3);

         var positions = new List<Position>
         {
            new Position { Name = "Position1", Line = new Line("Line", true), PrimarySkills = new List<PlayerSkill>
            {
               new PlayerSkill { Id = _skill1, Name = _skill1 }, //, PlayerSkillType = PlayerSkillType.Field, Order = 0},
               new PlayerSkill { Id = _skill2, Name = _skill2 }, //, PlayerSkillType = PlayerSkillType.Field, Order = 0},
               new PlayerSkill { Id = _skill3, Name = _skill3 } //, PlayerSkillType = PlayerSkillType.Field, Order = 0},
            }},
            new Position{Name = "Position2", Line = new Line("Line", true), PrimarySkills = new List<PlayerSkill>
            {
               new PlayerSkill { Id = _skill3, Name = _skill3 }, //, PlayerSkillType = PlayerSkillType.Field, Order = 0 },
               new PlayerSkill { Id = _skill2, Name = _skill2 }, //, PlayerSkillType = PlayerSkillType.Field, Order = 0 },
               new PlayerSkill { Id = _skill4, Name = _skill4 }, //, PlayerSkillType = PlayerSkillType.Field, Order = 0 }
            }}
         };

         _mockedPositionRepository.Setup(x => x.GetAll()).Returns(positions);

         var positionDeterminator = new PositionDeterminator(_mockedPositionRepository.Object, _mockedListRandomizer.Object);
         var determinedPosition = positionDeterminator.Determine(skillScores);

         Assert.IsNotNull(determinedPosition);
         Assert.AreEqual("Position2", determinedPosition.Name);
      }

      private List<PlayerSkillScore> GetSkillScores(int score1, int score2, int score3, int score4)
      {
         var skillScores = new List<PlayerSkillScore>
         {
            new PlayerSkillScore { PlayerSkill = new PlayerSkill { Id = _skill1, Name = _skill1 }, Score = score1 },
            new PlayerSkillScore { PlayerSkill = new PlayerSkill { Id = _skill2, Name = _skill2 }, Score = score2 },
            new PlayerSkillScore { PlayerSkill = new PlayerSkill { Id = _skill3, Name = _skill3 }, Score = score3 },
            new PlayerSkillScore { PlayerSkill = new PlayerSkill { Id = _skill4, Name = _skill4 }, Score = score4 },
         };

         return skillScores;
      }
   }
}