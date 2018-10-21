using System.Collections.Generic;
using System.Linq;
using Randomization;
using TwoNil.Data;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.Logic.Players
{
    public class PlayerGenerator
    {
        private readonly INumberRandomizer _numberRandomizer;
        private readonly IListRandomizer _listRandomizer;
        private readonly PersonNameGenerator _personNameGenerator;
        private readonly ProfileScoreCalculator _profileScoreCalculator;
        private readonly PositionDeterminator _positionDeterminator;
        private readonly IEnumerable<Position> _positions;
        private readonly IEnumerable<Line> _lines;
        private readonly IEnumerable<PlayerProfile> _playerProfiles;
        private readonly IUnitOfWorkFactory _uowFactory;

        public PlayerGenerator(IUnitOfWorkFactory uowFactory, PersonNameGenerator personNameGenerator, ProfileScoreCalculator profileScoreCalculator,
            INumberRandomizer numberRandomizer, IListRandomizer listRandomizer, PositionDeterminator positionDeterminator)
        {
            _uowFactory = uowFactory;

            _personNameGenerator = personNameGenerator;
            _profileScoreCalculator = profileScoreCalculator;
            _numberRandomizer = numberRandomizer;
            _listRandomizer = listRandomizer;
            _positionDeterminator = positionDeterminator;

            using (var uow = uowFactory.Create())
            {
                _lines = uow.Lines.GetAll();
                _positions = uow.Positions.GetAll();
                _playerProfiles = uow.PlayerProfiles.GetAll();
            }
        }

        private Player GeneratePlayer(Line line, Position position, AgeRange ageRange, int startNumber)
        {
            var player = new Player();

            player.Name = GetPlayerName();

            // A position is needed to randomly pick a player profile for a specific position.
            // The position can be passed into this method as an argument.
            // Also a line can be provided, then a position within that line will be randomly determined.
            // If no line and position is provided a position will be picked randomly.
            if (position == null)
            {
                // If no Line is provided, pick one randomly.
                if (line == null)
                {
                    line = _listRandomizer.GetItem(_lines);
                }

                // Get random position for this line.
                var positions = _positions.Where(x => x.Line.Id == line.Id);
                position = _listRandomizer.GetItem(positions);
            }

            // A Position for getting a random player profile is determined now, so get the player profile.
            var playerProfile = GetPlayerProfile(position);

            // The profile name and percentage is stored for testing purposes only.
            player.PlayerProfile = playerProfile.Name;

            // Age.
            if (ageRange == null)
            {
                ageRange = new AgeRange(16, 36);
            }
            var age = GetPersonAge(ageRange);
            player.Age = age;

            // Randomly calculate skill scores.
            var skillScores = startNumber > 0
                                    ? _profileScoreCalculator.Calculate(startNumber, playerProfile, age)
                                    : _profileScoreCalculator.Calculate(playerProfile, age);

            using (var uow = _uowFactory.Create())
            {
                player.SkillGoalkeeping = skillScores.Single(x => x.PlayerSkill.Id == uow.PlayerSkills.GetGoalkeeping().Id).Score;
                player.SkillDefending = skillScores.Single(x => x.PlayerSkill.Id == uow.PlayerSkills.GetDefending().Id).Score;
                player.SkillPassing = skillScores.Single(x => x.PlayerSkill.Id == uow.PlayerSkills.GetPassing().Id).Score;
                player.SkillSpeed = skillScores.Single(x => x.PlayerSkill.Id == uow.PlayerSkills.GetSpeed().Id).Score;
                player.SkillTechnique = skillScores.Single(x => x.PlayerSkill.Id == uow.PlayerSkills.GetTechnique().Id).Score;
                player.SkillShooting = skillScores.Single(x => x.PlayerSkill.Id == uow.PlayerSkills.GetShooting().Id).Score;
                player.SkillHeading = skillScores.Single(x => x.PlayerSkill.Id == uow.PlayerSkills.GetHeading().Id).Score;
                player.SkillTactics = skillScores.Single(x => x.PlayerSkill.Id == uow.PlayerSkills.GetTactics().Id).Score;
                player.SkillFitness = skillScores.Single(x => x.PlayerSkill.Id == uow.PlayerSkills.GetFitness().Id).Score;
                player.SkillForm = skillScores.Single(x => x.PlayerSkill.Id == uow.PlayerSkills.GetForm().Id).Score;
                player.SkillConfidence = skillScores.Single(x => x.PlayerSkill.Id == uow.PlayerSkills.GetConfidence().Id).Score;
                player.SkillPace = skillScores.Single(x => x.PlayerSkill.Id == uow.PlayerSkills.GetPace().Id).Score;
                player.SkillIntelligence = skillScores.Single(x => x.PlayerSkill.Id == uow.PlayerSkills.GetIntelligence().Id).Score;
                player.SkillMentality = skillScores.Single(x => x.PlayerSkill.Id == uow.PlayerSkills.GetMentality().Id).Score;
                player.SkillStrength = skillScores.Single(x => x.PlayerSkill.Id == uow.PlayerSkills.GetStrength().Id).Score;
                player.SkillStamina = skillScores.Single(x => x.PlayerSkill.Id == uow.PlayerSkills.GetStamina().Id).Score;
            }

            // Now the skill scores for the player are determined, determine the real position for the player.
            // This position can be different from the position that either was passed into this method or picked randomly.
            var determinedPosition = _positionDeterminator.Determine(skillScores);
            player.PreferredPosition = determinedPosition;

            DetermineRating(player);

            return player;
        }

        private static void DetermineRating(Player player)
        {
            var rating = PlayerRater.GetRating(player);

            player.Rating = rating.rating;
            player.RatingGoalkeeping = rating.ratingGoalkeeping;
            player.RatingDefence = rating.ratingDefence;
            player.RatingMidfield = rating.ratingMidfield;
            player.RatingAttack = rating.ratingAttack;
        }

        public Player Generate()
        {
            return GeneratePlayer(null, null, null, 0);
        }

        public Player Generate(int startNumber)
        {
            return GeneratePlayer(null, null, null, startNumber);
        }

        public Player Generate(Line line)
        {
            return GeneratePlayer(line, null, null, 0);
        }

        public Player Generate(Line line, int startNumber)
        {
            return GeneratePlayer(line, null, null, startNumber);
        }

        public Player Generate(Line line, AgeRange ageRange)
        {
            return GeneratePlayer(line, null, ageRange, 0);
        }

        public Player Generate(Position position)
        {
            return GeneratePlayer(null, position, null, 0);
        }

        public Player Generate(Position position, AgeRange ageRange)
        {
            return GeneratePlayer(null, position, ageRange, 0);
        }

        public Player Generate(AgeRange ageRange)
        {
            return GeneratePlayer(null, null, ageRange, 0);
        }

        /// <summary>
        /// Gets a random player full name.
        /// </summary>
        private string GetPlayerName()
        {
            var fullName = $"{_personNameGenerator.GetFirstName()} {_personNameGenerator.GetLastName()}";
            return fullName;
        }

        /// <summary>
        /// Gets a random <see cref="PlayerProfile"/> for a <see cref="Position"/>.
        /// </summary>
        private PlayerProfile GetPlayerProfile(Position position)
        {
            var playerProfiles = _playerProfiles.Where(x => x.Positions.Contains(position));
            var playerProfile = _listRandomizer.GetItem(playerProfiles);
            return playerProfile;
        }

        private int GetPersonAge(AgeRange ageRange)
        {
            return _numberRandomizer.GetNumber(ageRange.MinimumAge, ageRange.MaximumAge);
        }
    }
}
