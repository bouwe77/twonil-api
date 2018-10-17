using System.Collections.Generic;
using Randomization;
using TwoNil.Data;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.Logic.Players
{
    public interface IPlayerManager
    {
        IEnumerable<Player> GenerateSquad(Team team, int averageRating);
    }

    public class PlayerManager : IPlayerManager
    {
        private INumberRandomizer _numberRandomizer;
        private readonly IUnitOfWorkFactory _uowFactory;
        private IPlayerGenerator _playerGenerator;

        public PlayerManager(IUnitOfWorkFactory uowFactory, IPlayerGenerator playerGenerator, INumberRandomizer numberRandomizer)
        {
            _uowFactory = uowFactory;
            _playerGenerator = playerGenerator;
            _numberRandomizer = numberRandomizer;
        }

        public IEnumerable<Player> GenerateSquad(Team team, int averageRating)
        {
            var squad = new List<Player>();

            using (var uow = _uowFactory.Create())
            {
                // Generate 2 or 3 goalkeepers.
                int howMany = _numberRandomizer.GetNumber(2, 3);
                var line = uow.Lines.GetGoalkeeper();
                squad.AddRange(GeneratePlayersForLine(line, howMany, averageRating));

                // Generate between 5 and 7 defenders.
                howMany = _numberRandomizer.GetNumber(5, 7);
                line = uow.Lines.GetDefence();
                squad.AddRange(GeneratePlayersForLine(line, howMany, averageRating));

                // Generate between 5 and 7 midfielders.
                howMany = _numberRandomizer.GetNumber(5, 7);
                line = uow.Lines.GetMidfield();
                squad.AddRange(GeneratePlayersForLine(line, howMany, averageRating));

                // Generate between 4 and 6 attackers.
                howMany = _numberRandomizer.GetNumber(4, 6);
                line = uow.Lines.GetAttack();
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
                if (player.PreferredPosition.Line.Id == line.Id)
                {
                    players.Add(player);
                    generated++;
                }
            }

            return players;
        }

    }
}
