using System.Collections.Generic;
using System.Linq;
using TwoNil.Logic.Exceptions;
using TwoNil.Logic.Functionality.Competitions;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.Logic.Services
{
    public class SeasonService : ServiceWithGameBase
    {
        internal SeasonService(GameInfo gameInfo)
           : base(gameInfo)
        {
        }

        public Season GetCurrentSeason()
        {
            using (var seasonRepository = RepositoryFactory.CreateSeasonRepository())
            {
                return seasonRepository.GetCurrentSeason();
            }
        }

        public Season Get(string seasonId)
        {
            using (var seasonRepository = RepositoryFactory.CreateSeasonRepository())
            {
                return seasonRepository.GetOne(seasonId);
            }
        }

        public IEnumerable<Season> GetAll()
        {
            using (var seasonRepository = RepositoryFactory.CreateSeasonRepository())
            {
                return seasonRepository.GetAll();
            }
        }

        public bool DetermineSeasonEnded(string seasonId)
        {
            bool seasonEnded;
            using (var matchRepository = RepositoryFactory.CreateMatchRepository())
            {
                seasonEnded = matchRepository.GetBySeason(seasonId).All(m => m.MatchStatus == MatchStatus.Ended);
            }

            return seasonEnded;
        }

        public void EndSeasonAndCreateNext(string seasonId)
        {
            Team managersTeam;
            using (var repo = RepositoryFactory.CreateGameInfoRepository())
            {
                managersTeam = repo.GetGameInfo().CurrentTeam;
            }

            var seasonManager = new SeasonManager(RepositoryFactory, managersTeam);

            Season season;
            using (var seasonRepository = RepositoryFactory.CreateSeasonRepository())
            {
                season = seasonRepository.GetOne(seasonId);
            }

            // Create a transaction which ends the current season and creates a new one.
            var transactionManager = RepositoryFactory.CreateTransactionManager();
            seasonManager.EndSeason(season, transactionManager);
            seasonManager.CreateNextSeason(season, transactionManager);
            transactionManager.Save();
        }
    }
}
