using System;
using System.Collections.Generic;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.Data.Repositories
{
   public class InMemoryData
   {
      #region Competitions

      public League GetLeague1()
      {
         return new League
         {
            Id = "eb5537",
            LastModified = GetLastModified(),
            CompetitionType = CompetitionType.League,
            Name = "League 1",
            Order = 1,
            StatisticsScore = 400
         };
      }

      public League GetLeague2()
      {
         return new League
         {
            Id = "8e380f",
            LastModified = GetLastModified(),
            CompetitionType = CompetitionType.League,
            Name = "League 2",
            Order = 2,
            StatisticsScore = 300
         };
      }

      public League GetLeague3()
      {
         return new League
         {
            Id = "j3e0fg",
            LastModified = GetLastModified(),
            CompetitionType = CompetitionType.League,
            Name = "League 3",
            Order = 3,
            StatisticsScore = 200
         };
      }

      public League GetLeague4()
      {
         return new League
         {
            Id = "z5qe2c",
            LastModified = GetLastModified(),
            CompetitionType = CompetitionType.League,
            Name = "League 4",
            Order = 4,
            StatisticsScore = 100
         };
      }

      public Competition GetNationalCup()
      {
         return new Competition
         {
            Id = "4793cd",
            LastModified = GetLastModified(),
            CompetitionType = CompetitionType.NationalCup,
            Name = "Cup",
            Order = 5
         };
      }

      public Competition GetFriendly()
      {
         return new Competition
         {
            Id = "b1e920",
            LastModified = GetLastModified(),
            CompetitionType = CompetitionType.Friendly,
            Name = "Friendly",
            Order = 6
         };
      }

      public Competition GetNationalSuperCup()
      {
         return new Competition
         {
            Id = "c88104",
            LastModified = GetLastModified(),
            CompetitionType = CompetitionType.NationalSuperCup,
            Name = "Supercup",
            Order = 7
         };
      }

      #endregion Competitions

      #region Formations

      public Formation Get541()
      {
         return new Formation
         {
            Id = "90ae72",
            LastModified = GetLastModified(),
            Name = "5-4-1",
            Defenders = 5,
            Midfielders = 4,
            Attackers = 1,
            Positions = new List<Position>
            {
               GetGoalkeeper(),
               GetCentreBack(),
               GetCentreBack(),
               GetCentreBack(),
               GetWingBack(),
               GetWingBack(),
               GetCentralMidfield(),
               GetForwardMidfield(),
               GetWideMidfield(),
               GetWideMidfield(),
               GetStriker(),
            }
         };
      }

      public Formation Get451()
      {
         return new Formation
         {
            Id = "20bc15",
            LastModified = GetLastModified(),
            Name = "4-5-1",
            Defenders = 4,
            Midfielders = 5,
            Attackers = 1,
            Positions = new List<Position>
            {
               GetGoalkeeper(),
               GetCentreBack(),
               GetCentreBack(),
               GetWingBack(),
               GetWingBack(),
               GetDefensiveMidfield(),
               GetCentralMidfield(),
               GetForwardMidfield(),
               GetWinger(),
               GetWinger(),
               GetStriker(),
            }
         };
      }

      public Formation Get442()
      {
         return new Formation
         {
            Id = "1d3961",
            LastModified = GetLastModified(),
            Name = "4-4-2",
            Defenders = 4,
            Midfielders = 4,
            Attackers = 2,
            Positions = new List<Position>
            {
               GetGoalkeeper(),
               GetCentreBack(),
               GetCentreBack(),
               GetWingBack(),
               GetWingBack(),
               GetDefensiveMidfield(),
               GetForwardMidfield(),
               GetWideMidfield(),
               GetWideMidfield(),
               GetCentreForward(),
               GetStriker(),
            }
         };
      }

      public Formation Get433()
      {
         return new Formation
         {
            Id = "b36d68",
            LastModified = GetLastModified(),
            Name = "4-3-3",
            Defenders = 4,
            Midfielders = 3,
            Attackers = 3,
            Positions = new List<Position>
            {
               GetGoalkeeper(),
               GetCentreBack(),
               GetCentreBack(),
               GetWingBack(),
               GetWingBack(),
               GetCentralMidfield(),
               GetWideMidfield(),
               GetWideMidfield(),
               GetStriker(),
               GetWinger(),
               GetWinger(),
            }
         };
      }

      public Formation Get343()
      {
         return new Formation
         {
            Id = "bd0275",
            LastModified = GetLastModified(),
            Name = "3-4-3",
            Defenders = 3,
            Midfielders = 4,
            Attackers = 3,
            Positions = new List<Position>
            {
               GetGoalkeeper(),
               GetCentreBack(),
               GetWingBack(),
               GetWingBack(),
               GetDefensiveMidfield(),
               GetForwardMidfield(),
               GetWideMidfield(),
               GetWideMidfield(),
               GetStriker(),
               GetWinger(),
               GetWinger(),
            }
         };
      }

      #endregion Formations

      #region Lines

      public Line GetGoalkeeperLine()
      {
         return new Line("Goalkeeper", false)
         {
            Id = "197370",
            LastModified = GetLastModified(),
         };
      }

      public Line GetDefence()
      {
         return new Line("Defence", true)
         {
            Id = "54ccf3",
            LastModified = GetLastModified(),
         };
      }

      public Line GetMidfield()
      {
         return new Line("Midfield", true)
         {
            Id = "574ec8",
            LastModified = GetLastModified(),
         };
      }

      public Line GetAttack()
      {
         return new Line("Attack", true)
         {
            Id = "1e5858",
            LastModified = GetLastModified(),
         };
      }

      #endregion Lines

      #region PlayerProfiles

      /// <summary>
      /// Gets the profile for a goalkeeper.
      /// </summary>
      public PlayerProfile GetGoalkeeperProfile()
      {
         var playerProfile = new PlayerProfile
         {
            Id = "531645",
            LastModified = GetLastModified(),
            Name = "Goalkeeper",
            Lines = new List<Line>
            {
               GetGoalkeeperLine()
            },
            Positions = new List<Position>
            {
               GetGoalkeeper()
            }
         };

         AddSkillsToProfile(
            playerProfile: playerProfile,
            goalkeeping: ProfileSkillPriority.Primary,
            defending: ProfileSkillPriority.Tertiary,
            passing: ProfileSkillPriority.Tertiary,
            speed: ProfileSkillPriority.Quatenary,
            shooting: ProfileSkillPriority.Quatenary,
            heading: ProfileSkillPriority.Quatenary,
            tactics: ProfileSkillPriority.Quatenary,
            fitness: ProfileSkillPriority.Random,
            talent: ProfileSkillPriority.Random,
            technique: ProfileSkillPriority.Tertiary,
            form: ProfileSkillPriority.Random,
            confidence: ProfileSkillPriority.Random,
            pace: ProfileSkillPriority.Random,
            stamina: ProfileSkillPriority.Random,
            strength: ProfileSkillPriority.Random,
            mentality: ProfileSkillPriority.Random,
            intelligence: ProfileSkillPriority.Random);

         return playerProfile;
      }

      /// <summary>
      /// Gets the profile for a sweeper.
      /// </summary>
      public PlayerProfile GetSweeperProfile()
      {
         var playerProfile = new PlayerProfile
         {
            Id = "1b34d3",
            LastModified = GetLastModified(),
            Name = "Sweeper",
            Lines = new List<Line>
            {
               GetDefence()
            },
            Positions = new List<Position>
            {
               GetSweeper()
            }
         };

         AddSkillsToProfile(
            playerProfile: playerProfile,
            goalkeeping: ProfileSkillPriority.Quatenary,
            defending: ProfileSkillPriority.Primary,
            passing: ProfileSkillPriority.Secondary,
            speed: ProfileSkillPriority.Tertiary,
            shooting: ProfileSkillPriority.Tertiary,
            heading: ProfileSkillPriority.Tertiary,
            tactics: ProfileSkillPriority.Secondary,
            fitness: ProfileSkillPriority.Random,
            talent: ProfileSkillPriority.Random,
            technique: ProfileSkillPriority.Secondary,
            form: ProfileSkillPriority.Random,
            confidence: ProfileSkillPriority.Random,
            pace: ProfileSkillPriority.Random,
            stamina: ProfileSkillPriority.Random,
            strength: ProfileSkillPriority.Random,
            mentality: ProfileSkillPriority.Random,
            intelligence: ProfileSkillPriority.Random);

         return playerProfile;
      }

      /// <summary>
      /// Gets the profile for a centre back.
      /// </summary>
      public PlayerProfile GetCentreBackProfile()
      {
         var playerProfile = new PlayerProfile
         {
            Id = "7db815",
            LastModified = GetLastModified(),
            Name = "Centre Back",
            Lines = new List<Line>
            {
               GetDefence()
            },
            Positions = new List<Position>
            {
               GetCentreBack()
            }
         };

         AddSkillsToProfile(
            playerProfile: playerProfile,
            goalkeeping: ProfileSkillPriority.Quatenary,
            defending: ProfileSkillPriority.Primary,
            passing: ProfileSkillPriority.Tertiary,
            speed: ProfileSkillPriority.Tertiary,
            shooting: ProfileSkillPriority.Tertiary,
            heading: ProfileSkillPriority.Primary,
            tactics: ProfileSkillPriority.Random,
            fitness: ProfileSkillPriority.Random,
            talent: ProfileSkillPriority.Random,
            technique: ProfileSkillPriority.Tertiary,
            form: ProfileSkillPriority.Random,
            confidence: ProfileSkillPriority.Random,
            pace: ProfileSkillPriority.Random,
            stamina: ProfileSkillPriority.Random,
            strength: ProfileSkillPriority.Random,
            mentality: ProfileSkillPriority.Random,
            intelligence: ProfileSkillPriority.Random);

         return playerProfile;
      }

      /// <summary>
      /// Gets the profile for a Wing back.
      /// </summary>
      public PlayerProfile GetWingBackProfile()
      {
         var playerProfile = new PlayerProfile
         {
            Id = "fe1cfe",
            LastModified = GetLastModified(),
            Name = "Wing Back",
            Lines = new List<Line>
            {
               GetDefence()
            },
            Positions = new List<Position>
            {
               GetWingBack()
            }
         };

         AddSkillsToProfile(
            playerProfile: playerProfile,
            goalkeeping: ProfileSkillPriority.Quatenary,
            defending: ProfileSkillPriority.Primary,
            passing: ProfileSkillPriority.Secondary,
            speed: ProfileSkillPriority.Primary,
            shooting: ProfileSkillPriority.Tertiary,
            heading: ProfileSkillPriority.Tertiary,
            tactics: ProfileSkillPriority.Random,
            fitness: ProfileSkillPriority.Random,
            talent: ProfileSkillPriority.Random,
            technique: ProfileSkillPriority.Primary,
            form: ProfileSkillPriority.Random,
            confidence: ProfileSkillPriority.Random,
            pace: ProfileSkillPriority.Random,
            stamina: ProfileSkillPriority.Random,
            strength: ProfileSkillPriority.Random,
            mentality: ProfileSkillPriority.Random,
            intelligence: ProfileSkillPriority.Random);

         return playerProfile;
      }

      /// <summary>
      /// Gets the profile for a Defensive midfield.
      /// </summary>
      public PlayerProfile GetDefensiveMidfieldProfile()
      {
         var playerProfile = new PlayerProfile
         {
            Id = "39fe62",
            LastModified = GetLastModified(),
            Name = "Defensive Midfield",
            Lines = new List<Line>
            {
               GetMidfield()
            },
            Positions = new List<Position>
            {
               GetDefensiveMidfield()
            }
         };

         AddSkillsToProfile(
            playerProfile: playerProfile,
            goalkeeping: ProfileSkillPriority.Quatenary,
            defending: ProfileSkillPriority.Secondary,
            passing: ProfileSkillPriority.Primary,
            speed: ProfileSkillPriority.Tertiary,
            shooting: ProfileSkillPriority.Tertiary,
            heading: ProfileSkillPriority.Secondary,
            tactics: ProfileSkillPriority.Random,
            fitness: ProfileSkillPriority.Random,
            talent: ProfileSkillPriority.Random,
            technique: ProfileSkillPriority.Secondary,
            form: ProfileSkillPriority.Random,
            confidence: ProfileSkillPriority.Random,
            pace: ProfileSkillPriority.Random,
            stamina: ProfileSkillPriority.Random,
            strength: ProfileSkillPriority.Random,
            mentality: ProfileSkillPriority.Random,
            intelligence: ProfileSkillPriority.Random);

         return playerProfile;
      }

      /// <summary>
      /// Gets the profile for a Central midfield.
      /// </summary>
      public PlayerProfile GetCentralMidfieldProfile()
      {
         var playerProfile = new PlayerProfile
         {
            Id = "18d0c3",
            LastModified = GetLastModified(),
            Name = "Central Midfield",
            Lines = new List<Line>
            {
               GetMidfield()
            },
            Positions = new List<Position>
            {
               GetCentralMidfield()
            }
         };

         AddSkillsToProfile(
            playerProfile: playerProfile,
            goalkeeping: ProfileSkillPriority.Quatenary,
            defending: ProfileSkillPriority.Tertiary,
            passing: ProfileSkillPriority.Primary,
            speed: ProfileSkillPriority.Tertiary,
            shooting: ProfileSkillPriority.Tertiary,
            heading: ProfileSkillPriority.Tertiary,
            tactics: ProfileSkillPriority.Primary,
            fitness: ProfileSkillPriority.Random,
            talent: ProfileSkillPriority.Random,
            technique: ProfileSkillPriority.Secondary,
            form: ProfileSkillPriority.Random,
            confidence: ProfileSkillPriority.Random,
            pace: ProfileSkillPriority.Random,
            stamina: ProfileSkillPriority.Random,
            strength: ProfileSkillPriority.Random,
            mentality: ProfileSkillPriority.Random,
            intelligence: ProfileSkillPriority.Random);

         return playerProfile;
      }

      /// <summary>
      /// Gets the profile for a Forward midfield.
      /// </summary>
      public PlayerProfile GetForwardMidfieldProfile()
      {
         var playerProfile = new PlayerProfile
         {
            Id = "5312f0",
            LastModified = GetLastModified(),
            Name = "Forward Midfield",
            Lines = new List<Line>
            {
               GetMidfield()
            },
            Positions = new List<Position>
            {
               GetForwardMidfield()
            }
         };

         AddSkillsToProfile(
            playerProfile: playerProfile,
            goalkeeping: ProfileSkillPriority.Quatenary,
            defending: ProfileSkillPriority.Tertiary,
            passing: ProfileSkillPriority.Primary,
            speed: ProfileSkillPriority.Tertiary,
            shooting: ProfileSkillPriority.Secondary,
            heading: ProfileSkillPriority.Tertiary,
            tactics: ProfileSkillPriority.Secondary,
            fitness: ProfileSkillPriority.Random,
            talent: ProfileSkillPriority.Random,
            technique: ProfileSkillPriority.Primary,
            form: ProfileSkillPriority.Random,
            confidence: ProfileSkillPriority.Random,
            pace: ProfileSkillPriority.Random,
            stamina: ProfileSkillPriority.Random,
            strength: ProfileSkillPriority.Random,
            mentality: ProfileSkillPriority.Random,
            intelligence: ProfileSkillPriority.Random);

         return playerProfile;
      }

      /// <summary>
      /// Gets the profile for a Wide midfield.
      /// </summary>
      public PlayerProfile GetWideMidfieldProfile()
      {
         var playerProfile = new PlayerProfile
         {
            Id = "015c48",
            LastModified = GetLastModified(),
            Name = "Wide Midfield",
            Lines = new List<Line>
            {
               GetMidfield()
            },
            Positions = new List<Position>
            {
               GetWideMidfield()
            }
         };

         AddSkillsToProfile(
            playerProfile: playerProfile,
            goalkeeping: ProfileSkillPriority.Quatenary,
            defending: ProfileSkillPriority.Tertiary,
            passing: ProfileSkillPriority.Primary,
            speed: ProfileSkillPriority.Tertiary,
            shooting: ProfileSkillPriority.Tertiary,
            heading: ProfileSkillPriority.Tertiary,
            tactics: ProfileSkillPriority.Secondary,
            fitness: ProfileSkillPriority.Random,
            talent: ProfileSkillPriority.Random,
            technique: ProfileSkillPriority.Secondary,
            form: ProfileSkillPriority.Random,
            confidence: ProfileSkillPriority.Random,
            pace: ProfileSkillPriority.Random,
            stamina: ProfileSkillPriority.Random,
            strength: ProfileSkillPriority.Random,
            mentality: ProfileSkillPriority.Random,
            intelligence: ProfileSkillPriority.Random);

         return playerProfile;
      }

      /// <summary>
      /// Gets the profile for a Striker.
      /// </summary>
      public PlayerProfile GetStrikerProfile()
      {
         var playerProfile = new PlayerProfile
         {
            Id = "89e8f8",
            LastModified = GetLastModified(),
            Name = "Striker",
            Lines = new List<Line>
            {
               GetAttack()
            },
            Positions = new List<Position>
            {
               GetStriker()
            }
         };

         AddSkillsToProfile(
            playerProfile: playerProfile,
            goalkeeping: ProfileSkillPriority.Quatenary,
            defending: ProfileSkillPriority.Tertiary,
            passing: ProfileSkillPriority.Tertiary,
            speed: ProfileSkillPriority.Secondary,
            shooting: ProfileSkillPriority.Primary,
            heading: ProfileSkillPriority.Secondary,
            tactics: ProfileSkillPriority.Random,
            fitness: ProfileSkillPriority.Random,
            talent: ProfileSkillPriority.Random,
            technique: ProfileSkillPriority.Primary,
            form: ProfileSkillPriority.Random,
            confidence: ProfileSkillPriority.Random,
            pace: ProfileSkillPriority.Random,
            stamina: ProfileSkillPriority.Random,
            strength: ProfileSkillPriority.Random,
            mentality: ProfileSkillPriority.Random,
            intelligence: ProfileSkillPriority.Random);

         return playerProfile;
      }

      /// <summary>
      /// Gets the profile for a False nine.
      /// </summary>
      public PlayerProfile GetCentreForwardProfile()
      {
         var playerProfile = new PlayerProfile
         {
            Id = "9475d9",
            LastModified = GetLastModified(),
            Name = "Centre Forward",
            Lines = new List<Line>
            {
               GetAttack()
            },
            Positions = new List<Position>
            {
               GetCentreForward()
            }
         };

         AddSkillsToProfile(
            playerProfile: playerProfile,
            goalkeeping: ProfileSkillPriority.Quatenary,
            defending: ProfileSkillPriority.Tertiary,
            passing: ProfileSkillPriority.Primary,
            speed: ProfileSkillPriority.Tertiary,
            shooting: ProfileSkillPriority.Secondary,
            heading: ProfileSkillPriority.Secondary,
            tactics: ProfileSkillPriority.Random,
            fitness: ProfileSkillPriority.Random,
            talent: ProfileSkillPriority.Random,
            technique: ProfileSkillPriority.Secondary,
            form: ProfileSkillPriority.Random,
            confidence: ProfileSkillPriority.Random,
            pace: ProfileSkillPriority.Random,
            stamina: ProfileSkillPriority.Random,
            strength: ProfileSkillPriority.Random,
            mentality: ProfileSkillPriority.Random,
            intelligence: ProfileSkillPriority.Random);

         return playerProfile;
      }

      /// <summary>
      /// Gets the profile for a Winger.
      /// </summary>
      public PlayerProfile GetWingerProfile()
      {
         var playerProfile = new PlayerProfile
         {
            Id = "98a14d",
            LastModified = GetLastModified(),
            Name = "Winger",
            Lines = new List<Line>
            {
               GetAttack()
            },
            Positions = new List<Position>
            {
               GetWinger()
            }
         };

         AddSkillsToProfile(
            playerProfile: playerProfile,
            goalkeeping: ProfileSkillPriority.Quatenary,
            defending: ProfileSkillPriority.Tertiary,
            passing: ProfileSkillPriority.Primary,
            speed: ProfileSkillPriority.Primary,
            shooting: ProfileSkillPriority.Secondary,
            heading: ProfileSkillPriority.Tertiary,
            tactics: ProfileSkillPriority.Random,
            fitness: ProfileSkillPriority.Random,
            talent: ProfileSkillPriority.Random,
            technique: ProfileSkillPriority.Primary,
            form: ProfileSkillPriority.Random,
            confidence: ProfileSkillPriority.Random,
            pace: ProfileSkillPriority.Random,
            stamina: ProfileSkillPriority.Random,
            strength: ProfileSkillPriority.Random,
            mentality: ProfileSkillPriority.Random,
            intelligence: ProfileSkillPriority.Random);

         return playerProfile;
      }

      /// <summary>
      /// Gets a profile for a versatile field player.
      /// </summary>
      public PlayerProfile GetVersatileProfile()
      {
         var playerProfile = new PlayerProfile
         {
            Id = "8a1078",
            LastModified = GetLastModified(),
            Name = "Versatile",
            // Alle lines behalve goalkeeper.
            Lines = new List<Line>
            {
               GetDefence(),
               GetMidfield(),
               GetAttack()
            },
            // Alle positions behalve goalkeeper.
            Positions = new List<Position>
            {
               GetSweeper(),
               GetCentreBack(),
               GetWingBack(),
               GetDefensiveMidfield(),
               GetCentralMidfield(),
               GetWideMidfield(),
               GetForwardMidfield(),
               GetStriker(),
               GetCentreForward(),
               GetWinger()
            }
         };

         AddSkillsToProfile(
            playerProfile: playerProfile,
            goalkeeping: ProfileSkillPriority.Quatenary,
            defending: ProfileSkillPriority.Primary,
            passing: ProfileSkillPriority.Primary,
            speed: ProfileSkillPriority.Primary,
            shooting: ProfileSkillPriority.Primary,
            heading: ProfileSkillPriority.Primary,
            tactics: ProfileSkillPriority.Random,
            fitness: ProfileSkillPriority.Random,
            talent: ProfileSkillPriority.Random,
            technique: ProfileSkillPriority.Primary,
            form: ProfileSkillPriority.Random,
            confidence: ProfileSkillPriority.Random,
            pace: ProfileSkillPriority.Random,
            stamina: ProfileSkillPriority.Random,
            strength: ProfileSkillPriority.Random,
            mentality: ProfileSkillPriority.Random,
            intelligence: ProfileSkillPriority.Random);

         return playerProfile;
      }

      private void AddSkillsToProfile(
         PlayerProfile playerProfile,
         ProfileSkillPriority goalkeeping,
         ProfileSkillPriority defending,
         ProfileSkillPriority passing,
         ProfileSkillPriority speed,
         ProfileSkillPriority shooting,
         ProfileSkillPriority heading,
         ProfileSkillPriority tactics,
         ProfileSkillPriority fitness,
         ProfileSkillPriority talent,
         ProfileSkillPriority technique,
         ProfileSkillPriority form,
         ProfileSkillPriority confidence,
         ProfileSkillPriority pace,
         ProfileSkillPriority stamina,
         ProfileSkillPriority strength,
         ProfileSkillPriority mentality,
         ProfileSkillPriority intelligence)
      {
         playerProfile.PlayerProfileSkills = new List<PlayerProfileSkill>();

         using (var playerSkillRepository = new RepositoryFactory().CreatePlayerSkillRepository())
         {
            // Add Goalkeeping skill.
            var playerSkill = playerSkillRepository.GetGoalkeeping();
            var profileSkill = new PlayerProfileSkill(playerSkill, goalkeeping);
            playerProfile.PlayerProfileSkills.Add(profileSkill);

            // Add Defending skill.
            playerSkill = playerSkillRepository.GetDefending();
            profileSkill = new PlayerProfileSkill(playerSkill, defending);
            playerProfile.PlayerProfileSkills.Add(profileSkill);

            // Add Passing skill.
            playerSkill = playerSkillRepository.GetPassing();
            profileSkill = new PlayerProfileSkill(playerSkill, passing);
            playerProfile.PlayerProfileSkills.Add(profileSkill);

            // Add Speed skill.
            playerSkill = playerSkillRepository.GetSpeed();
            profileSkill = new PlayerProfileSkill(playerSkill, speed);
            playerProfile.PlayerProfileSkills.Add(profileSkill);

            // Add Technique skill.
            playerSkill = playerSkillRepository.GetTechnique();
            profileSkill = new PlayerProfileSkill(playerSkill, technique);
            playerProfile.PlayerProfileSkills.Add(profileSkill);

            // Add Shooting skill.
            playerSkill = playerSkillRepository.GetShooting();
            profileSkill = new PlayerProfileSkill(playerSkill, shooting);
            playerProfile.PlayerProfileSkills.Add(profileSkill);

            // Add Heading skill.
            playerSkill = playerSkillRepository.GetHeading();
            profileSkill = new PlayerProfileSkill(playerSkill, heading);
            playerProfile.PlayerProfileSkills.Add(profileSkill);

            // Add Tactics skill.
            playerSkill = playerSkillRepository.GetTactics();
            profileSkill = new PlayerProfileSkill(playerSkill, tactics);
            playerProfile.PlayerProfileSkills.Add(profileSkill);

            // Add Talent skill.
            playerSkill = playerSkillRepository.GetTalent();
            profileSkill = new PlayerProfileSkill(playerSkill, talent);
            playerProfile.PlayerProfileSkills.Add(profileSkill);

            // Add Fitness skill.
            playerSkill = playerSkillRepository.GetFitness();
            profileSkill = new PlayerProfileSkill(playerSkill, fitness);
            playerProfile.PlayerProfileSkills.Add(profileSkill);

            // Add Form skill.
            playerSkill = playerSkillRepository.GetForm();
            profileSkill = new PlayerProfileSkill(playerSkill, form);
            playerProfile.PlayerProfileSkills.Add(profileSkill);

            // Add Confidence skill.
            playerSkill = playerSkillRepository.GetConfidence();
            profileSkill = new PlayerProfileSkill(playerSkill, confidence);
            playerProfile.PlayerProfileSkills.Add(profileSkill);

            // Add Pace skill.
            playerSkill = playerSkillRepository.GetPace();
            profileSkill = new PlayerProfileSkill(playerSkill, pace);
            playerProfile.PlayerProfileSkills.Add(profileSkill);

            // Add Stamina skill.
            playerSkill = playerSkillRepository.GetStamina();
            profileSkill = new PlayerProfileSkill(playerSkill, stamina);
            playerProfile.PlayerProfileSkills.Add(profileSkill);

            // Add Strength skill.
            playerSkill = playerSkillRepository.GetStrength();
            profileSkill = new PlayerProfileSkill(playerSkill, strength);
            playerProfile.PlayerProfileSkills.Add(profileSkill);

            // Add Mentality skill.
            playerSkill = playerSkillRepository.GetMentality();
            profileSkill = new PlayerProfileSkill(playerSkill, mentality);
            playerProfile.PlayerProfileSkills.Add(profileSkill);

            // Add Intelligence skill.
            playerSkill = playerSkillRepository.GetIntelligence();
            profileSkill = new PlayerProfileSkill(playerSkill, intelligence);
            playerProfile.PlayerProfileSkills.Add(profileSkill);
         }
      }

      #endregion PlayerProfiles

      #region PlayerSkills

      public PlayerSkill GetGoalkeeping()
      {
         return new PlayerSkill
         {
            Id = "08bed7",
            LastModified = GetLastModified(),
            Name = "Goalkeeping",
            //PlayerSkillType = PlayerSkillType.Goalkeeper,
         };
      }

      public PlayerSkill GetDefending()
      {
         return new PlayerSkill
         {
            Id = "dd804f",
            LastModified = GetLastModified(),
            Name = "Defending",
            //PlayerSkillType = PlayerSkillType.Field,
         };
      }

      public PlayerSkill GetPassing()
      {
         return new PlayerSkill
         {
            Id = "23eaff",
            LastModified = GetLastModified(),
            Name = "Passing",
            //PlayerSkillType = PlayerSkillType.Field,
         };
      }

      public PlayerSkill GetSpeed()
      {
         return new PlayerSkill
         {
            Id = "499153",
            LastModified = GetLastModified(),
            Name = "Speed",
            //PlayerSkillType = PlayerSkillType.Field,
         };
      }

      public PlayerSkill GetTechnique()
      {
         return new PlayerSkill
         {
            Id = "9f00b0",
            LastModified = GetLastModified(),
            Name = "Technique",
            //PlayerSkillType = PlayerSkillType.Field,
         };
      }

      public PlayerSkill GetShooting()
      {
         return new PlayerSkill
         {
            Id = "997205",
            LastModified = GetLastModified(),
            Name = "Shooting",
            //PlayerSkillType = PlayerSkillType.Field,
         };
      }

      public PlayerSkill GetHeading()
      {
         return new PlayerSkill
         {
            Id = "6c78f9",
            LastModified = GetLastModified(),
            Name = "Heading",
            //PlayerSkillType = PlayerSkillType.Field,
         };
      }

      public PlayerSkill GetTactics()
      {
         return new PlayerSkill
         {
            Id = "e24e9e",
            LastModified = GetLastModified(),
            Name = "Tactics",
            //PlayerSkillType = PlayerSkillType.Field,
         };
      }

      public PlayerSkill GetFitness()
      {
         return new PlayerSkill
         {
            Id = "1d2ae2",
            LastModified = GetLastModified(),
            Name = "Fitness",
            //PlayerSkillType = PlayerSkillType.Physical,
         };
      }

      public PlayerSkill GetTalent()
      {
         return new PlayerSkill
         {
            Id = "295034",
            LastModified = GetLastModified(),
            Name = "Talent",
            //PlayerSkillType = PlayerSkillType.Undefined, //Deze even tijdelijk op undefined
         };
      }

      public PlayerSkill GetForm()
      {
         return new PlayerSkill
         {
            Id = "aba0eb",
            LastModified = GetLastModified(),
            Name = "Form",
            //PlayerSkillType = PlayerSkillType.Mental,
         };
      }

      public PlayerSkill GetConfidence()
      {
         return new PlayerSkill
         {
            Id = "7e22a8",
            LastModified = GetLastModified(),
            Name = "Confidence",
            //PlayerSkillType = PlayerSkillType.Mental,
         };
      }

      internal PlayerSkill GetStamina()
      {
         return new PlayerSkill
         {
            Id = "8f33b9",
            LastModified = GetLastModified(),
            Name = "Stamina",
            //PlayerSkillType = PlayerSkillType.Physical,
         };
      }

      internal PlayerSkill GetStrength()
      {
         return new PlayerSkill
         {
            Id = "9a44c0",
            LastModified = GetLastModified(),
            Name = "Strength",
            //PlayerSkillType = PlayerSkillType.Physical,
         };
      }

      internal PlayerSkill GetMentality()
      {
         return new PlayerSkill
         {
            Id = "0b55d1",
            LastModified = GetLastModified(),
            Name = "Mentality",
            //PlayerSkillType = PlayerSkillType.Mental,
         };
      }

      internal PlayerSkill GetIntelligence()
      {
         return new PlayerSkill
         {
            Id = "1c66e2",
            LastModified = GetLastModified(),
            Name = "Intelligence",
            //PlayerSkillType = PlayerSkillType.Mental,
         };
      }

      internal PlayerSkill GetPace()
      {
         return new PlayerSkill
         {
            Id = "2d77f3",
            LastModified = GetLastModified(),
            Name = "Pace",
            //PlayerSkillType = PlayerSkillType.Physical,
         };
      }

      #endregion PlayerSkills

      #region Positions

      public Position GetGoalkeeper()
      {
         var primarySkills = new List<PlayerSkill>
         {
            GetGoalkeeping()
         };

         return new Position
         {
            Id = "4868c1",
            LastModified = GetLastModified(),
            Name = "Goalkeeper",
            ShortName = "GK",
            Line = GetGoalkeeperLine(),
            PrimarySkills = primarySkills
         };
      }

      // LET OP: Deze positie is puur om spelers te identificeren die goed zijn in verdedigen en ook kunnen voetballen.
      // Zij krijgen dan de Position Centre Back, zie PositionDeterminator.
      public Position GetSweeper()
      {
         var primarySkills = new List<PlayerSkill>
         {
            GetDefending(),
            GetPassing(),
            GetTechnique()
         };

         return new Position
         {
            Id = "d2cee7",
            LastModified = GetLastModified(),
            Name = "Sweeper",
            ShortName = "SW",
            Line = GetDefence(),
            PrimarySkills = primarySkills
         };
      }

      public Position GetCentreBack()
      {
         var primarySkills = new List<PlayerSkill>
         {
            GetDefending(),
            GetHeading()
         };

         return new Position
         {
            Id = "1a67a9",
            LastModified = GetLastModified(),
            Name = "Centre Back",
            ShortName = "CB",
            Line = GetDefence(),
            PrimarySkills = primarySkills
         };
      }

      public Position GetWingBack()
      {
         var primarySkills = new List<PlayerSkill>
         {
            GetDefending(),
            GetTechnique(),
            GetSpeed()
         };

         return new Position
         {
            Id = "d56ba0",
            LastModified = GetLastModified(),
            Name = "Wing Back",
            ShortName = "WB",
            Line = GetDefence(),
            PrimarySkills = primarySkills
         };
      }

      public Position GetDefensiveMidfield()
      {
         var primarySkills = new List<PlayerSkill>
         {
            GetPassing(),
            GetDefending()
         };

         return new Position
         {
            Id = "2359ef",
            LastModified = GetLastModified(),
            Name = "Defensive Midfield",
            ShortName = "DM",
            Line = GetMidfield(),
            PrimarySkills = primarySkills
         };
      }

      public Position GetCentralMidfield()
      {
         var primarySkills = new List<PlayerSkill>
         {
            GetPassing(),
            GetTactics(),
            GetTechnique()
         };

         return new Position
         {
            Id = "73bdd8",
            LastModified = GetLastModified(),
            Name = "Central Midfield",
            ShortName = "CM",
            Line = GetMidfield(),
            PrimarySkills = primarySkills
         };
      }

      public Position GetWideMidfield()
      {
         var primarySkills = new List<PlayerSkill>
         {
            GetPassing()
         };

         return new Position
         {
            Id = "1a0397",
            LastModified = GetLastModified(),
            Name = "Wide Midfield",
            ShortName = "WM",
            Line = GetMidfield(),
            PrimarySkills = primarySkills
         };
      }

      public Position GetForwardMidfield()
      {
         var primarySkills = new List<PlayerSkill>
         {
            GetPassing(),
            GetTechnique(),
            GetShooting()
         };

         return new Position
         {
            Id = "795709",
            LastModified = GetLastModified(),
            Name = "Forward Midfield",
            ShortName = "FW",
            Line = GetMidfield(),
            PrimarySkills = primarySkills
         };
      }

      public Position GetCentreForward()
      {
         var primarySkills = new List<PlayerSkill>
         {
            GetTechnique(),
            GetPassing(),
            GetShooting()
         };

         return new Position
         {
            Id = "d3a7eb",
            LastModified = GetLastModified(),
            Name = "Centre Forward",
            ShortName = "CF",
            Line = GetAttack(),
            PrimarySkills = primarySkills
         };
      }

      public Position GetStriker()
      {
         var primarySkills = new List<PlayerSkill>
         {
            GetShooting(),
            GetHeading(),
            GetTechnique()
         };

         return new Position
         {
            Id = "d10683",
            LastModified = GetLastModified(),
            Name = "Striker",
            ShortName = "ST",
            Line = GetAttack(),
            PrimarySkills = primarySkills
         };
      }

      public Position GetWinger()
      {
         var primarySkills = new List<PlayerSkill>
         {
            GetSpeed(),
            GetTechnique(),
            GetPassing(),
         };

         return new Position
         {
            Id = "1b42a6",
            LastModified = GetLastModified(),
            Name = "Winger",
            ShortName = "WI",
            Line = GetAttack(),
            PrimarySkills = primarySkills
         };
      }

      #endregion Positions

      private string GetLastModified()
      {
         var lastModified = new DateTime(2016, 5, 20);
         return SqliteRepository.Format(lastModified);
      }
   }
}
