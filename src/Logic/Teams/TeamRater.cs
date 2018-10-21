using System.Collections.Generic;
using System.Linq;
using TwoNil.Data;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.Logic.Teams
{
    /// <summary>
    /// Calculates the team's rating per <see cref="Line"/> depending on which players are on which position in the field.
    /// </summary>
    public class TeamRater
    {
        private readonly IUnitOfWorkFactory _uowFactory;

        public TeamRater(IUnitOfWorkFactory uowFactory)
        {
            _uowFactory = uowFactory;
        }

        public (decimal ratingGoalkeeper, decimal ratingDefence, decimal ratingMidfield, decimal ratingAttack, decimal ratingTeam) GetRating(List<Player> players)
        {
            using (var uow = _uowFactory.Create())
            {
                var goalkeeper = uow.Lines.GetGoalkeeper();
                var defence = uow.Lines.GetDefence();
                var midfield = uow.Lines.GetMidfield();
                var attack = uow.Lines.GetAttack();

                decimal ratingGoalkeeper = players.Single(p => p.CurrentPosition.Line.Id == goalkeeper.Id).RatingGoalkeeping;
                decimal ratingDefence = players.Where(p => p.CurrentPosition.Line.Id == defence.Id).Select(p => p.RatingDefence).Average();
                decimal ratingMidfield = players.Where(p => p.CurrentPosition.Line.Id == midfield.Id).Select(p => p.RatingMidfield).Average();
                decimal ratingAttack = players.Where(p => p.CurrentPosition.Line.Id == attack.Id).Select(p => p.RatingAttack).Average();

                decimal ratingTeam = (ratingGoalkeeper + ratingDefence + ratingMidfield + ratingAttack) / 4;

                return (ratingGoalkeeper, ratingDefence, ratingMidfield, ratingAttack, ratingTeam);
            }
        }
    }
}
