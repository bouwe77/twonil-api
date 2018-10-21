using System.Collections.Generic;
using System.Linq;
using TwoNil.Logic.Competitions;
using TwoNil.Shared.DomainObjects;
using TwoNil.Data;

namespace TwoNil.Services
{
    public class SeasonService : ServiceWithGameBase
    {
        private readonly SeasonManager _seasonManager;

        internal SeasonService(IUnitOfWorkFactory uowFactory, GameInfo gameInfo, SeasonManager seasonManager)
           : base(uowFactory, gameInfo)
        {
            _seasonManager = seasonManager;
        }

        public Season GetCurrentSeason()
        {
            using (var uow = UowFactory.Create())
            {
                return uow.Seasons.GetCurrentSeason();
            }
        }

        public Season Get(string seasonId)
        {
            using (var uow = UowFactory.Create())
            {
                return uow.Seasons.GetOne(seasonId);
            }
        }

        public IEnumerable<Season> GetAll()
        {
            using (var uow = UowFactory.Create())
            {
                return uow.Seasons.GetAll();
            }
        }

        public bool DetermineSeasonEnded(string seasonId)
        {
            bool seasonEnded;
            using (var uow = UowFactory.Create())
            {
                seasonEnded = uow.Matches.GetBySeason(seasonId).All(m => m.MatchStatus == MatchStatus.Ended);
            }

            return seasonEnded;
        }

        public void EndSeasonAndCreateNext(string seasonId)
        {
            Team managersTeam;
            using (var uow = UowFactory.Create())
            {
                managersTeam = uow.GameInfos.GetGameInfo().CurrentTeam;
                Season season = uow.Seasons.GetOne(seasonId);

                // Create a transaction which ends the current season and creates a new one.
                //TODO create transaction on the UOW
                _seasonManager.EndSeason(season, uow);
                _seasonManager.CreateNextSeason(season, uow);

                //TODO commit
            }
        }
    }
}
