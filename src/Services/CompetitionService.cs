using System.Collections.Generic; //======= KLAAR =======
using TwoNil.Data;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.Services
{
    public class CompetitionService : ServiceBase
    {
        public CompetitionService(IUnitOfWorkFactory uowFactory)
            : base(uowFactory)
        {
        }

        public IEnumerable<Competition> GetAll()
        {
            using (var uow = UowFactory.Create())
            {
                var competitions = uow.Competitions.GetAll();
                return competitions;
            }
        }

        public Competition Get(string competitionId)
        {
            using (var uow = UowFactory.Create())
            {
                var competition = uow.Competitions.GetOne(competitionId);
                return competition;
            }
        }

        public IEnumerable<Competition> GetByType(CompetitionType competitionType)
        {
            using (var uow = UowFactory.Create())
            {
                var competitions = uow.Competitions.GetByCompetitionType(competitionType);
                return competitions;
            }
        }
    }
}
