using System.Linq;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.Data.Repositories
{
   public class GameInfoRepository : ReadRepository<GameInfo>
   {
      internal GameInfoRepository(string databaseFilePath, string gameId)
         : base(databaseFilePath, gameId)
      {
      }

      public GameInfo GetGameInfo()
      {
         // The GameInfo table contains only one record.
         var gameInfo = GetAll().First();

         GetReferencedData(gameInfo);

         return gameInfo;
      }


      private void GetReferencedData(GameInfo gameInfo)
      {
         var repositoryFactory = new RepositoryFactory(gameInfo.Id);
         using (var teamRepository = repositoryFactory.CreateRepository<Team>())
         {
            var team = teamRepository.GetOne(gameInfo.CurrentTeamId);
            gameInfo.CurrentTeam = team;
         }
      }
   }
}
